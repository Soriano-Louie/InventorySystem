# VAT Calculation with Discount Implementation

## Overview
This document explains how the POS system calculates VAT (Value Added Tax) on products with discounts applied. The implementation ensures that discounts are applied **first**, then VAT is calculated on the **discounted price**.

---

## Key Principles

### 1. **Calculation Order**
```
Original Price ? Apply Discount ? Calculate VAT ? Final Total
```

### 2. **Per-Product Calculation**
- Each product is processed individually in the cart loop
- VAT is **NOT** applied to the cart total
- VAT applicability is determined by the `IsVATApplicable` BIT field in the database

### 3. **Discount Priority**
- Discounts are applied **before** VAT calculation
- VAT is calculated on the **discounted price**, not the original price
- This is both legally correct and customer-friendly (lower VAT on discounted items)

---

## Database Schema

### Product Tables
```sql
-- wholesaleProducts and retailProducts tables
CREATE TABLE wholesaleProducts (
    ProductID INT PRIMARY KEY,
    ProductName NVARCHAR(255),
  RetailPrice DECIMAL(18,2),
    IsVATApplicable BIT DEFAULT 0,  -- 0 = No VAT, 1 = VAT applicable
...
)
```

### Discount Table
```sql
CREATE TABLE ProductDiscounts (
    DiscountID INT PRIMARY KEY,
    ProductID INT,
    MinSacks INT,     -- Minimum quantity for discount
    MaxSacks INT NULL,      -- Maximum quantity (NULL = no limit)
    DiscountPrice DECIMAL(18,2)  -- Discounted price per unit
)
```

### Settings Table
```sql
CREATE TABLE Settings (
    SettingKey NVARCHAR(50) PRIMARY KEY,
    SettingValue NVARCHAR(255)
)
-- VAT Rate stored as: SettingKey = 'VATRate', SettingValue = '12'
```

---

## Implementation Code

### File: `cashierPOS\posForm.vb`

#### Method: `CalculateAllTotals()`

```vb
Private Sub CalculateAllTotals()
    ' Get VAT rate from database
Dim vatRate As Decimal = SharedUtilities.GetCurrentVATRate() / 100

    Dim subtotalBeforeVAT As Decimal = 0D
    Dim totalDiscountAmount As Decimal = 0D
    Dim totalVATAmount As Decimal = 0D
    Dim totalVatableAmount As Decimal = 0D

    For Each item In cartItems
        ' STEP 1: Get effective price (discount if available)
     Dim effectivePrice As Decimal = If(item.DiscountPrice.HasValue, 
           item.DiscountPrice.Value, 
          item.UnitPrice)
        
        ' STEP 2: Calculate item subtotal (after discount, before VAT)
        Dim itemSubtotal As Decimal = effectivePrice * item.Quantity

        ' STEP 3: Calculate discount amount
        If item.DiscountPrice.HasValue Then
 Dim discountPerItem As Decimal = (item.UnitPrice - item.DiscountPrice.Value) * item.Quantity
            totalDiscountAmount += discountPerItem
        End If

        ' STEP 4: Check if VAT applicable
        If item.IsVATApplicable Then
          ' Calculate VAT on DISCOUNTED price
        Dim itemVAT As Decimal = itemSubtotal * vatRate

         totalVatableAmount += itemSubtotal  ' Base amount
       totalVATAmount += itemVAT      ' VAT amount
      subtotalBeforeVAT += itemSubtotal   ' Add to grand subtotal
        Else
   ' No VAT - just add subtotal
      subtotalBeforeVAT += itemSubtotal
     End If
    Next

    ' STEP 5: Calculate grand total
    Dim grandTotal As Decimal = subtotalBeforeVAT + totalVATAmount

    ' STEP 6: Update UI labels
    totalSalesLabel.Text = $"?{grandTotal:N2}"
    totalDiscountLabel.Text = $"?{totalDiscountAmount:N2}"
    VATLabel.Text = $"?{totalVATAmount:N2}"
    vatableLabel.Text = $"?{totalVatableAmount:N2}"
End Sub
```

---

## Calculation Examples

### Example 1: Product with Discount and VAT

**Scenario:**
- Product: Rice (VAT-applicable)
- Original Price: ?34.00 per unit
- Quantity: 5 units
- Discount Available: ?30.00 per unit (for 5+ units)
- VAT Rate: 12%

**Step-by-Step Calculation:**

```
Step 1: Apply Discount
--------
effectivePrice = ?30.00 (discount price kicks in at 5 units)

Step 2: Calculate Subtotal (after discount, before VAT)
--------
itemSubtotal = ?30.00 × 5 = ?150.00

Step 3: Calculate Discount Amount
--------
discountPerItem = (?34.00 - ?30.00) × 5
discountPerItem = ?4.00 × 5 = ?20.00

Step 4: Calculate VAT (on discounted price)
--------
itemVAT = ?150.00 × 0.12 = ?18.00

Step 5: Calculate Item Total
--------
itemTotal = ?150.00 + ?18.00 = ?168.00
```

**POS Display:**
```
Product: Rice
Quantity: 5
Unit Price: ?30.00 (discounted from ?34.00)
Line Total: ?150.00

Summary Panel:
--------------
Vatable:    ?150.00  (base after discount)
VAT (12%):  ?18.00   (tax on discounted price)
Discount:   ?20.00   (savings from discount)
TOTAL:      ?168.00
```

**Comparison Without Discount:**
```
If no discount applied:
Base:  ?34 × 5 = ?170.00
VAT:   ?170.00 × 0.12 = ?20.40
Total: ?190.40

Customer saves: ?22.40
  (?20.00 from discount + ?2.40 from lower VAT)
```

---

### Example 2: Product WITHOUT Discount (Insufficient Quantity)

**Scenario:**
- Product: Rice (same product as above)
- Price: ?34.00 per unit
- Quantity: 2 units (below minimum for discount)
- VAT Rate: 12%

**Calculation:**

```
Step 1: No Discount Available
--------
effectivePrice = ?34.00 (regular price)

Step 2: Calculate Subtotal
--------
itemSubtotal = ?34.00 × 2 = ?68.00

Step 3: No Discount
--------
discountPerItem = ?0.00

Step 4: Calculate VAT
--------
itemVAT = ?68.00 × 0.12 = ?8.16

Step 5: Calculate Total
--------
itemTotal = ?68.00 + ?8.16 = ?76.16
```

**POS Display:**
```
Product: Rice
Quantity: 2
Unit Price: ?34.00
Line Total: ?68.00

Summary Panel:
--------------
Vatable:    ?68.00
VAT (12%):  ?8.16
Discount:   ?0.00
TOTAL:?76.16
```

---

### Example 3: Mixed Cart (VAT-able and Non-VAT Items)

**Scenario:**
```
Product A: Rice (VAT-able, with discount)
  - Regular: ?34 × 5 qty
  - Discount: ?30 × 5 qty
- VAT Rate: 12%

Product B: Books (NOT VAT-able)
  - Price: ?50 × 2 qty
  - No VAT applies
```

**Calculation:**

```
Product A (Rice):
-----------------
effectivePrice = ?30.00
itemSubtotal = ?30 × 5 = ?150.00
discount = (?34 - ?30) × 5 = ?20.00
itemVAT = ?150 × 0.12 = ?18.00
itemTotal = ?150 + ?18 = ?168.00

Product B (Books):
------------------
effectivePrice = ?50.00
itemSubtotal = ?50 × 2 = ?100.00
discount = ?0.00
itemVAT = ?0.00 (not VAT-applicable)
itemTotal = ?100.00

Grand Total Calculation:
------------------------
subtotalBeforeVAT = ?150 + ?100 = ?250.00
totalVATAmount = ?18.00 + ?0.00 = ?18.00
totalDiscountAmount = ?20.00 + ?0.00 = ?20.00
totalVatableAmount = ?150.00 (only Rice)

grandTotal = ?250 + ?18 = ?268.00
```

**POS Display:**
```
Cart Items:
-----------
1. Rice       5 × ?30.00 = ?150.00
2. Books      2 × ?50.00 = ?100.00

Summary Panel:
--------------
Vatable:    ?150.00  (only Rice counted)
VAT (12%):  ?18.00   (only on Rice)
Discount:   ?20.00   (Rice discount)
TOTAL:      ?268.00
```

---

## Cart Item Class Structure

```vb
Public Class CartItem
  Public Property ProductID As Integer
    Public Property ProductName As String
    Public Property Quantity As Integer
    Public Property UnitPrice As Decimal        ' Original price
    Public Property IsVATApplicable As Boolean    ' VAT flag from DB
    Public Property DiscountPrice As Decimal?     ' Nullable - discount if available
    Public Property CategoryID As Integer
End Class
```

---

## Helper Methods

### 1. Get Discount Price
```vb
Private Function GetDiscountPrice(productID As Integer, quantity As Integer) As Decimal?
    ' Query ProductDiscounts table
    ' Return discount price if quantity qualifies
    ' Return Nothing if no discount available
End Function
```

### 2. Get VAT Rate
```vb
' In SharedUtilities.vb
Public Shared Function GetCurrentVATRate() As Decimal
    ' Query Settings table for current VAT rate
    ' Return as percentage (e.g., 12)
End Function
```

### 3. Add Product to Cart
```vb
Public Sub AddProductToCart(productID, productName, quantity, unitPrice, categoryID, isVATApplicable)
    ' Check if product exists in cart
    ' If exists: update quantity
    ' If new: create CartItem with discount check
    ' Refresh display and recalculate totals
End Sub
```

---

## UI Label Definitions

| Label | Variable Name | Description |
|-------|--------------|-------------|
| **Vatable** | `vatableLabel` | Sum of base prices for VAT-applicable items (BEFORE VAT added) |
| **VAT** | `VATLabel` | Total VAT amount (calculated only on vatable items) |
| **Discount** | `totalDiscountLabel` | Total discount amount from all discounted items |
| **TOTAL SALES** | `totalSalesLabel` | Grand total including all items and VAT |

---

## Important Notes

### ? **Correct Behavior**
1. **Discount First**: Always apply discount before VAT calculation
2. **Per-Product VAT**: Each product's VAT is calculated individually
3. **Discounted Base**: VAT is calculated on the discounted price, not original
4. **Selective VAT**: Only items with `IsVATApplicable = 1` get VAT applied

### ? **Common Mistakes to Avoid**
1. ? Calculating VAT on original price when discount exists
2. ? Applying VAT to cart total instead of per-product
3. ? Ignoring the `IsVATApplicable` flag
4. ? Adding VAT to non-taxable items

---

## Testing Checklist

- [ ] Add VAT-applicable product ? VAT calculated
- [ ] Add non-VAT product ? No VAT added
- [ ] Add product with discount ? Discount applied first, VAT on discounted price
- [ ] Increase quantity to trigger discount ? Discount and VAT recalculated
- [ ] Decrease quantity below discount threshold ? Discount removed, VAT on full price
- [ ] Mix VAT and non-VAT products ? Correct totals for each
- [ ] Change VAT rate in settings ? All calculations update correctly
- [ ] Empty cart ? All totals reset to ?0.00

---

## Related Files

### Core Implementation
- `cashierPOS\posForm.vb` - Main POS logic with VAT calculation
- `SharedUtilities.vb` - VAT rate retrieval from database

### Forms
- `cashierPOS\ProductSearchForm.vb` - Product selection
- `cashierPOS\QRScannerForm.vb` - Barcode scanning
- `cashierPOS\CheckoutForm.vb` - Payment processing

### Database
- `wholesaleProducts` table - Product data with `IsVATApplicable`
- `retailProducts` table - Retail product data with `IsVATApplicable`
- `ProductDiscounts` table - Discount tiers based on quantity
- `Settings` table - VAT rate configuration

### Documentation
- `cashierPOS\VAT_CALCULATION_FIX.md` - Initial VAT implementation
- `adminPages\Retail\VAT_IMPLEMENTATION_SUMMARY.md` - Retail VAT summary

---

## Compliance Notes

### Legal Requirements (Philippines BIR)
1. **VAT Calculation**: 12% on vatable sales (as of current regulation)
2. **VAT-Exempt Items**: Some products may be VAT-exempt by law
3. **Discount Treatment**: Discounts reduce vatable base (correct implementation)
4. **Receipt Requirements**: Must show VAT-able sales, VAT amount, and total separately

### Business Rules
1. Discounts are promotional and reduce the taxable base
2. VAT is calculated on the net selling price (after discounts)
3. Customer sees the benefit of lower VAT on discounted items
4. System maintains accurate records for tax reporting

---

## Troubleshooting

### Issue: VAT Amount Seems Wrong
**Check:**
1. Is the VAT rate correct in Settings table?
2. Is the product marked as `IsVATApplicable = 1`?
3. Is discount being applied correctly?
4. Are you comparing to the right base amount?

### Issue: Discount Not Applying
**Check:**
1. Does quantity meet MinSacks requirement?
2. Is ProductDiscounts record active for this product?
3. Is discount price valid in database?

### Issue: Labels Showing ?0.00
**Check:**
1. Is cart empty?
2. Are all products non-VAT items?
3. Is VAT rate = 0 in settings?

---

## Version History

| Version | Date | Changes |
|---------|------|---------|
| 1.0 | 2025-01-10 | Initial implementation with basic VAT |
| 2.0 | 2025-01-10 | Fixed VAT calculation logic (add vs remove) |
| 3.0 | 2025-01-10 | Added discount-first calculation flow |
| 3.1 | 2025-01-10 | Cleaned up duplicate variables and comments |

---

## Summary

This implementation ensures that:
1. ? Customers get the best price (discount applied first)
2. ? VAT is calculated correctly on the discounted amount
3. ? Non-VAT items are excluded from tax calculation
4. ? All calculations are per-product, not on cart total
5. ? UI displays accurate breakdown of charges

The system is both **legally compliant** and **customer-friendly**! ??
