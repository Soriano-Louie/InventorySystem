# POS VAT Calculation Fix

## Problem
The POS system was not correctly calculating and displaying VAT for products marked as VAT-applicable in the database. The `IsVATApplicable` bit field from the database was not being properly utilized to add VAT on top of product prices.

## Solution
Updated the `CalculateAllTotals()` method in `posForm.vb` to properly handle VAT calculations based on the `IsVATApplicable` field.

## How It Works Now

### Database Schema
- **IsVATApplicable**: BIT column in both `wholesaleProducts` and `retailProducts` tables
  - `1` (True): Product is subject to VAT
  - `0` (False): Product is NOT subject to VAT

### Calculation Logic

For each item in the cart:

1. **Get Base Price**: 
   - Use discount price if available, otherwise use unit price
   - `effectivePrice = discountPrice ?? unitPrice`

2. **Calculate Item Subtotal** (before VAT):
   - `itemSubtotal = effectivePrice × quantity`

3. **Check VAT Applicability**:
   ```vb
   If item.IsVATApplicable Then
       ' Product is VAT-applicable
   itemVAT = itemSubtotal × (VATRate / 100)
       totalVatableAmount += itemSubtotal
       totalVATAmount += itemVAT
   Else
       ' Product is not VAT-applicable
       ' No VAT added
   End If
   ```

4. **Calculate Grand Total**:
   - `grandTotal = subtotalBeforeVAT + totalVATAmount`

### Label Display

The POS form displays four key totals:

1. **vatableLabel** (`?X,XXX.XX`):
   - Sum of base prices for VAT-applicable items (BEFORE VAT is added)
   - This is the "Vatable Sales" amount

2. **VATLabel** (`?X,XXX.XX`):
   - Total VAT amount calculated on vatable items
   - Formula: `vatableAmount × (VATRate / 100)`

3. **totalDiscountLabel** (`?X,XXX.XX`):
   - Total discount amount from all discounted items

4. **totalSalesLabel** (`?X,XXX.XX`):
   - Grand total including VAT
   - Formula: `subtotal + VAT`

## Example Calculation

### Scenario:
- VAT Rate: 12%
- Item A: ?100 × 2 qty, VAT-applicable ?
- Item B: ?50 × 1 qty, NOT VAT-applicable ?

### Calculations:
1. **Item A**:
   - Subtotal: ?100 × 2 = ?200
   - VAT-applicable: Yes
   - VAT: ?200 × 0.12 = ?24
   - Total: ?224

2. **Item B**:
   - Subtotal: ?50 × 1 = ?50
   - VAT-applicable: No
   - VAT: ?0
   - Total: ?50

3. **Final Totals**:
   - **Vatable Amount** (vatableLabel): ?200.00
   - **VAT Amount** (VATLabel): ?24.00
   - **Total Discount**: ?0.00
   - **Grand Total** (totalSalesLabel): ?274.00

## Key Changes Made

### File: `cashierPOS\posForm.vb`

**Method**: `CalculateAllTotals()`

**Before**:
- Incorrectly assumed prices already included VAT
- Used formula: `itemSubtotal = itemTotal / (1 + vatRate)` (removing VAT)
- This was backwards!

**After**:
- Correctly treats database prices as base prices (without VAT)
- Uses formula: `itemVAT = itemSubtotal × vatRate` (adding VAT)
- Only applies VAT to items where `IsVATApplicable = True`

## Important Notes

1. **Price Assumption**: 
   - Product prices in the database (RetailPrice, Cost) do NOT include VAT
   - VAT is calculated and added at checkout

2. **VAT Rate Source**:
   - Retrieved from database using `SharedUtilities.GetCurrentVATRate()`
   - Returns percentage value (e.g., 12)
   - Divided by 100 to get decimal (0.12)

3. **Bit Field Handling**:
   - `IsVATApplicable` is a BIT field in SQL Server
   - Retrieved as Boolean in VB.NET
   - Default value: `ISNULL(IsVATApplicable, 0)` treats NULL as False

## Testing Checklist

- [x] Add VAT-applicable product ? VAT calculated correctly
- [x] Add non-VAT product ? No VAT added
- [x] Mix of VAT and non-VAT items ? Correct totals
- [x] Apply discounts to VAT items ? VAT calculated on discounted price
- [x] Labels display correct values:
  - [x] vatableLabel shows vatable base amount
  - [x] VATLabel shows VAT amount
  - [x] totalSalesLabel shows grand total with VAT

## Related Files
- `cashierPOS\posForm.vb` - Main POS form with VAT calculation
- `SharedUtilities.vb` - Provides `GetCurrentVATRate()` method
- `cashierPOS\CheckoutForm.vb` - Uses cart totals for payment processing
- Database tables: `wholesaleProducts`, `retailProducts` (IsVATApplicable column)

## SQL Query Used in GetProductDetails

```sql
SELECT ProductID, ProductName, RetailPrice, CategoryID,
       ISNULL(IsVATApplicable, 0) AS IsVATApplicable
FROM wholesaleProducts
WHERE SKU = @SKU
UNION
SELECT ProductID, ProductName, RetailPrice, CategoryID,
       ISNULL(IsVATApplicable, 0) AS IsVATApplicable
FROM retailProducts
WHERE SKU = @SKU
```

The `ISNULL(IsVATApplicable, 0)` ensures NULL values are treated as FALSE (not VAT-applicable).
