# SharedUtilities Module - Usage Guide

## Overview
The `SharedUtilities` module provides commonly used functions that can be accessed from any form in the application.

## Available Functions

### 1. GetConnectionString()
Returns the database connection string.

**Usage:**
```vb
Dim connStr As String = SharedUtilities.GetConnectionString()
```

### 2. GetCurrentVATRate()
Retrieves the current VAT rate from the database.

**Returns:** Decimal value representing the VAT rate (e.g., 12.00 for 12%)

**Usage Example:**
```vb
' Simple usage
Dim vatRate As Decimal = SharedUtilities.GetCurrentVATRate()
MessageBox.Show($"Current VAT Rate: {vatRate}%")

' Use in calculations
Dim subtotal As Decimal = 1000D
Dim vatRate As Decimal = SharedUtilities.GetCurrentVATRate()
Dim vatAmount As Decimal = subtotal * (vatRate / 100)
Dim totalWithVAT As Decimal = subtotal + vatAmount
```

### 3. SaveVATRate(vatRate As Decimal)
Saves or updates the VAT rate in the database.

**Parameters:**
- `vatRate`: The VAT rate to save (as a decimal, e.g., 12.00 for 12%)

**Returns:** Boolean - True if successful, False otherwise

**Usage Example:**
```vb
Dim newRate As Decimal = 15D
If SharedUtilities.SaveVATRate(newRate) Then
    MessageBox.Show("VAT rate updated successfully!")
Else
    MessageBox.Show("Failed to update VAT rate.")
End If
```

## Integration Examples

### Example 1: POS Form - Calculate Total with VAT
```vb
Private Sub CalculateTotal()
    Dim subtotal As Decimal = 0D
    
    ' Calculate subtotal from items
    For Each item In cartItems
        subtotal += item.Price * item.Quantity
    Next
    
    ' Get VAT rate and calculate VAT amount
    Dim vatRate As Decimal = SharedUtilities.GetCurrentVATRate()
    Dim vatAmount As Decimal = subtotal * (vatRate / 100)
    Dim total As Decimal = subtotal + vatAmount
    
    ' Display values
    lblSubtotal.Text = $"?{subtotal:N2}"
    lblVAT.Text = $"VAT ({vatRate}%): ?{vatAmount:N2}"
    lblTotal.Text = $"?{total:N2}"
End Sub
```

### Example 2: Sales Report - Display VAT Breakdown
```vb
Private Sub LoadSalesWithVAT()
    Dim vatRate As Decimal = SharedUtilities.GetCurrentVATRate()
    
    Using conn As New SqlConnection(SharedUtilities.GetConnectionString())
        Dim query As String = "SELECT SaleID, TotalAmount FROM SalesReport"
        Using cmd As New SqlCommand(query, conn)
            conn.Open()
            Using reader As SqlDataReader = cmd.ExecuteReader()
                While reader.Read()
                    Dim saleId As Integer = reader.GetInt32(0)
                    Dim totalAmount As Decimal = reader.GetDecimal(1)
                    
                    ' Calculate VAT components
                    Dim subtotal As Decimal = totalAmount / (1 + (vatRate / 100))
                    Dim vatAmount As Decimal = totalAmount - subtotal
                    
                    Console.WriteLine($"Sale {saleId}: Subtotal=?{subtotal:N2}, VAT=?{vatAmount:N2}, Total=?{totalAmount:N2}")
                End While
            End Using
        End Using
    End Using
End Sub
```

### Example 3: Admin Settings Form - Update VAT Rate
```vb
Private Sub btnUpdateVAT_Click(sender As Object, e As EventArgs) Handles btnUpdateVAT.Click
    ' Get current VAT rate
    Dim currentRate As Decimal = SharedUtilities.GetCurrentVATRate()
    
    ' Show input box
    Dim input As String = InputBox($"Enter new VAT Rate (%):{vbCrLf}Current: {currentRate:N2}%", 
                                   "Update VAT Rate", 
                                   currentRate.ToString())
    
    If String.IsNullOrWhiteSpace(input) Then
        Return
    End If
    
    ' Validate and save
    Dim newRate As Decimal
    If Decimal.TryParse(input, newRate) AndAlso newRate >= 0 AndAlso newRate <= 100 Then
        If SharedUtilities.SaveVATRate(newRate) Then
            MessageBox.Show($"VAT rate updated to {newRate:N2}%", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            MessageBox.Show("Failed to save VAT rate.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    Else
        MessageBox.Show("Invalid VAT rate. Must be between 0 and 100.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    End If
End Sub
```

### Example 4: Invoice Generation - Format with VAT
```vb
Private Function GenerateInvoiceText(saleId As Integer) As String
    Dim vatRate As Decimal = SharedUtilities.GetCurrentVATRate()
    Dim subtotal As Decimal = GetSaleSubtotal(saleId)
    Dim vatAmount As Decimal = subtotal * (vatRate / 100)
    Dim total As Decimal = subtotal + vatAmount
    
    Dim invoice As New System.Text.StringBuilder()
    invoice.AppendLine("=== INVOICE ===")
    invoice.AppendLine($"Sale ID: {saleId}")
    invoice.AppendLine()
    invoice.AppendLine($"Subtotal:    ?{subtotal:N2}")
    invoice.AppendLine($"VAT ({vatRate}%):  ?{vatAmount:N2}")
    invoice.AppendLine($"----------------------------")
    invoice.AppendLine($"TOTAL:       ?{total:N2}")
    
    Return invoice.ToString()
End Function
```

## Notes
- The VAT rate is stored as a decimal in the database (e.g., 12.00 represents 12%)
- Always divide by 100 when using the rate in calculations: `vatAmount = subtotal * (vatRate / 100)`
- The functions handle database errors gracefully and return default values (0) if errors occur
- Only one VAT rate record should exist in the settings table at any time

## Database Requirements
The functions expect a `settings` table with the following structure:
```sql
CREATE TABLE settings (
    id INT PRIMARY KEY IDENTITY(1,1),
    vatRate DECIMAL(5,2)
)
```
