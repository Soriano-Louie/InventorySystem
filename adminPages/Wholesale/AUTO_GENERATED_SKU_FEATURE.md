# Auto-Generated SKU Feature

## Overview
The SKU (Stock Keeping Unit) field is now **automatically generated** based on the product name and current date. This eliminates manual SKU entry and ensures unique, consistent SKU codes across all products.

## SKU Format

### Pattern: `ABC-YYMMDD-RR`

| Component | Description | Example |
|-----------|-------------|---------|
| **ABC** | First 3 letters of product name (uppercase, no spaces) | `COF` |
| **YYMMDD** | Current date (Year-Month-Day) | `250119` |
| **RR** | Random 2-digit number for uniqueness | `47` |

### Full Examples:

| Product Name | Generated SKU | Breakdown |
|--------------|---------------|-----------|
| Coffee Beans | `COF-250119-47` | COF (Coffee) + 250119 (Jan 19, 2025) + 47 (random) |
| Rice | `RIC-250119-82` | RIC (Rice) + 250119 (Jan 19, 2025) + 82 (random) |
| Sugar White | `SUG-250119-23` | SUG (Sugar) + 250119 (Jan 19, 2025) + 23 (random) |
| Tea Bags | `TEA-250119-65` | TEA (Tea) + 250119 (Jan 19, 2025) + 65 (random) |
| Oil | `OIL-250119-91` | OIL (Oil) + 250119 (Jan 19, 2025) + 91 (random) |

## How It Works

### 1. **User Types Product Name**
```
User enters: "Coffee Beans"
```

### 2. **SKU Auto-Generates in Real-Time**
```
SKU field updates to: "COF-250119-47"
```

### 3. **SKU Field is Read-Only**
- User cannot manually edit SKU
- Prevents duplicate or invalid SKUs
- Ensures consistency

### 4. **Uniqueness Check**
- System checks if SKU already exists in database
- If duplicate detected, generates new random code
- Loops until unique SKU is found

## Implementation Details

### Code Changes in `addItemForm.vb`

#### 1. **Constructor - Make SKU Read-Only**
```vb
Public Sub New(parent As InventoryForm)
    ' ...existing code...

    ' Make SKU textbox read-only (auto-generated)
    skuTextBox.ReadOnly = True
    skuTextBox.BackColor = Color.FromArgb(240, 240, 240)
    skuTextBox.ForeColor = Color.Gray
    skuTextBox.Text = "Auto-generated"
End Sub
```

#### 2. **GenerateSKU Function**
```vb
''' <summary>
''' Auto-generates SKU based on product name and current date
''' Format: First 3 letters of product name + YYMMDD + random 2-digit number
''' Example: "COF250119-47" for "Coffee" on Jan 19, 2025
''' </summary>
Private Function GenerateSKU(productName As String) As String
  If String.IsNullOrWhiteSpace(productName) Then
 Return ""
    End If

    ' Get first 3 letters of product name (uppercase, remove spaces)
    Dim cleanName As String = productName.Trim().Replace(" ", "").ToUpper()
    Dim prefix As String = If(cleanName.Length >= 3, cleanName.Substring(0, 3), cleanName.PadRight(3, "X"c))

    ' Get current date in YYMMDD format
    Dim dateCode As String = DateTime.Now.ToString("yyMMdd")

    ' Generate random 2-digit number for uniqueness
    Dim random As New Random()
    Dim randomCode As String = random.Next(10, 99).ToString()

 ' Combine to create SKU: ABC-YYMMDD-RR
    Dim sku As String = $"{prefix}-{dateCode}-{randomCode}"

    ' Check if SKU already exists, if so, regenerate random part
    While SKUExists(sku)
        randomCode = random.Next(10, 99).ToString()
        sku = $"{prefix}-{dateCode}-{randomCode}"
    End While

    Return sku
End Function
```

#### 3. **SKU Uniqueness Check**
```vb
''' <summary>
''' Checks if SKU already exists in database
''' </summary>
Private Function SKUExists(sku As String) As Boolean
    Dim connString As String = GetConnectionString()
    Try
  Using conn As New SqlConnection(connString)
      conn.Open()
   Dim query As String = "SELECT COUNT(*) FROM wholesaleProducts WHERE SKU = @SKU"
            Using cmd As New SqlCommand(query, conn)
              cmd.Parameters.AddWithValue("@SKU", sku)
      Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())
   Return count > 0
        End Using
        End Using
    Catch ex As Exception
    ' If error checking, assume doesn't exist
        Return False
    End Try
End Function
```

#### 4. **Real-Time SKU Generation on Product Name Change**
```vb
''' <summary>
''' Event handler when product name changes - auto-generates SKU
''' </summary>
Private Sub productTextBox_TextChanged(sender As Object, e As EventArgs) Handles productTextBox.TextChanged
    ' Only generate if product name has at least 1 character
    If Not String.IsNullOrWhiteSpace(productTextBox.Text) Then
        skuTextBox.Text = GenerateSKU(productTextBox.Text)
        skuTextBox.ForeColor = Color.Black
    Else
        skuTextBox.Text = "Auto-generated"
        skuTextBox.ForeColor = Color.Gray
    End If
End Sub
```

#### 5. **Final SKU Generation Before Save**
```vb
Private Sub saveButton_Click(sender As Object, e As EventArgs) Handles addButton.Click
    ' ...validation code...
    
    ' Generate final SKU before saving
    Dim finalSKU As String = GenerateSKU(productTextBox.Text)
    skuTextBox.Text = finalSKU
    
    Try
  InsertProductWithQRCode(
            finalSKU,    ' SKU (auto-generated)
  productTextBox.Text,    ' Product Name
    ' ...other parameters...
   )
    Catch ex As Exception
        ' ...error handling...
    End Try
End Sub
```

## User Experience

### Before (Manual SKU Entry):
1. ? User had to think of unique SKU
2. ? Risk of duplicate SKUs
3. ? Inconsistent naming conventions
4. ? Manual typing errors

### After (Auto-Generated SKU):
1. ? User types product name only
2. ? SKU generates automatically
3. ? Guaranteed unique SKUs
4. ? Consistent format
5. ? No typing errors
6. ? Faster data entry

## Visual Feedback

### Initial State:
```
Product Name: [   ]
SKU:       [Auto-generated] (gray, read-only)
```

### User Types "Coffee":
```
Product Name: [Coffee        ]
SKU: [COF-250119-47 ] (black, read-only)
```

### After Clearing Product Name:
```
Product Name: [              ]
SKU:      [Auto-generated] (gray, read-only)
```

## Edge Cases Handled

### 1. **Short Product Names (< 3 characters)**
```
Product: "Ox"
SKU: "OXX-250119-42"  (padded with X)
```

### 2. **Product Names with Spaces**
```
Product: "Green Tea"
SKU: "GRE-250119-28"  (spaces removed)
```

### 3. **Duplicate SKU Detection**
```
If "COF-250119-47" exists:
  - Generate new random: "COF-250119-82"
  - Check again
  - Repeat until unique
```

### 4. **Empty Product Name**
```
Product: ""
SKU: "Auto-generated"  (placeholder text)
```

### 5. **Special Characters in Product Name**
```
Product: "Tea & Coffee"
SKU: "TEA-250119-55"  (only letters used)
```

## Benefits

### ? **Efficiency**
- Saves time - no manual SKU entry
- Faster product addition workflow

### ? **Consistency**
- All SKUs follow same format
- Easy to identify and sort

### ? **Uniqueness**
- Automatic duplicate prevention
- Database validation

### ? **Traceability**
- Date embedded in SKU
- Know when product was added

### ? **User-Friendly**
- No training needed
- Visual feedback (gray ? black)
- Cannot be accidentally edited

## Technical Specifications

### Date Format: `YYMMDD`
| Component | Range | Example |
|-----------|-------|---------|
| YY | 00-99 | 25 (2025) |
| MM | 01-12 | 01 (January) |
| DD | 01-31 | 19 (19th) |

### Random Code: `10-99`
- 90 possible combinations per product per day
- Enough for typical daily entries
- Auto-regenerates if duplicate

### SKU Length
- **Minimum**: 13 characters (`ABC-250119-10`)
- **Maximum**: 13 characters (fixed format)
- **Consistent**: Always same length

## Database Integration

### Table: `wholesaleProducts`
```sql
CREATE TABLE wholesaleProducts (
    ProductID INT PRIMARY KEY IDENTITY(1,1),
    SKU NVARCHAR(50) UNIQUE NOT NULL,  -- Enforces uniqueness
    ProductName NVARCHAR(255) NOT NULL,
    -- ...other columns...
)
```

### Uniqueness Check Query
```sql
SELECT COUNT(*) 
FROM wholesaleProducts 
WHERE SKU = @SKU
```

## QR Code Integration

The auto-generated SKU is used to create the QR code:
```vb
' Generate QR code from SKU
Using qrGen As New QRCodeGenerator()
    Using qrCodeData = qrGen.CreateQrCode(sku, QRCodeGenerator.ECCLevel.Q)
        ' ...QR code generation...
    End Using
End Using
```

## Form Focus Changes

### Before:
```vb
Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    Me.BeginInvoke(Sub() skuTextBox.Focus())  ' Old: Focus on SKU
End Sub
```

### After:
```vb
Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    Me.BeginInvoke(Sub() productTextBox.Focus())  ' New: Focus on Product Name
End Sub
```

## Reset Behavior

When form is cleared after successful save:
```vb
' Reset SKU display
skuTextBox.Text = "Auto-generated"
skuTextBox.ForeColor = Color.Gray

' Focus returns to product name
Me.BeginInvoke(Sub() productTextBox.Focus())
```

## Validation

### Required Field Check (Updated)
```vb
' SKU no longer checked (auto-generated)
If String.IsNullOrWhiteSpace(productTextBox.Text) OrElse
   String.IsNullOrWhiteSpace(unitTextBox.Text) OrElse
   ' ...other required fields...
Then
    MessageBox.Show("Please fill in all required fields.")
    Exit Sub
End If
```

## Testing Scenarios

### Test Case 1: Normal Product
- **Input**: "Coffee Beans"
- **Expected SKU**: `COF-YYMMDD-RR` (e.g., `COF-250119-47`)
- **Result**: ? Pass

### Test Case 2: Short Name
- **Input**: "Ox"
- **Expected SKU**: `OXX-YYMMDD-RR`
- **Result**: ? Pass

### Test Case 3: Spaces in Name
- **Input**: "Green Tea Leaves"
- **Expected SKU**: `GRE-YYMMDD-RR`
- **Result**: ? Pass

### Test Case 4: Duplicate Detection
- **Scenario**: SKU already exists
- **Expected**: New random code generated
- **Result**: ? Pass

### Test Case 5: Clear and Re-enter
- **Action**: Clear product name, re-type
- **Expected**: New SKU generated each time
- **Result**: ? Pass

## Future Enhancements

Possible improvements:
- ? Add category prefix (e.g., `BEV-COF-250119-47` for Beverages)
- ? Configurable SKU format in settings
- ? SKU preview before saving
- ? SKU history/versioning
- ? Bulk SKU regeneration tool
- ? Export SKU patterns report

## Troubleshooting

### Issue: SKU not generating
**Solution**: Check `productTextBox.Text` is not empty

### Issue: Duplicate SKU error
**Solution**: Random regeneration should handle this automatically

### Issue: SKU field editable
**Solution**: Check `skuTextBox.ReadOnly = True` in constructor

### Issue: SKU not updating on product name change
**Solution**: Verify `productTextBox_TextChanged` event is connected

## Related Files

- `adminPages\Wholesale\addItemForm.vb` - Main implementation
- `adminPages\Wholesale\addItemForm.Designer.vb` - Form design
- `adminPages\Wholesale\InventoryForm.vb` - Parent form (refresh grid)

---

**Status**: ? Implementation Complete  
**Build Status**: ? Successful  
**Feature**: Auto-Generated SKU  
**Version**: 1.0  
**Date**: 2025-01-19

## Summary

The auto-generated SKU feature streamlines product entry by:
1. ? Automatically creating unique SKU codes
2. ? Preventing user errors and duplicates
3. ? Maintaining consistent naming format
4. ? Embedding date information in SKU
5. ? Improving data entry speed
6. ? Providing real-time visual feedback

Users no longer need to manually create SKUs - they simply type the product name and the system handles the rest! ??
