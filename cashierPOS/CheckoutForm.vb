Imports Microsoft.Data.SqlClient

Public Class CheckoutForm
    Private parentForm As posForm
    Private totalAmount As Decimal
    Private selectedPaymentMethod As String = ""

    Public Sub New(parent As posForm, total As Decimal)
        InitializeComponent()
        Me.parentForm = parent
        Me.totalAmount = total
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.StartPosition = FormStartPosition.CenterParent
        Me.BackColor = Color.FromArgb(230, 216, 177)
    End Sub

    Private Sub CheckoutForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Display total amount
        lblTotalAmount.Text = $"Total Amount: ₱{totalAmount:N2}"
        lblTotalAmount.Font = New Font("Segoe UI", 14, FontStyle.Bold)
        lblTotalAmount.ForeColor = Color.FromArgb(79, 51, 40)

        ' Setup payment method buttons
        btnCash.BackColor = Color.FromArgb(147, 53, 53)
        btnCash.ForeColor = Color.FromArgb(230, 216, 177)
        btnCash.Font = New Font("Segoe UI", 12, FontStyle.Bold)

        btnGCash.BackColor = Color.FromArgb(147, 53, 53)
        btnGCash.ForeColor = Color.FromArgb(230, 216, 177)
        btnGCash.Font = New Font("Segoe UI", 12, FontStyle.Bold)

        btnBankTransaction.BackColor = Color.FromArgb(147, 53, 53)
        btnBankTransaction.ForeColor = Color.FromArgb(230, 216, 177)
        btnBankTransaction.Font = New Font("Segoe UI", 12, FontStyle.Bold)

        btnConfirm.BackColor = Color.FromArgb(79, 51, 40)
        btnConfirm.ForeColor = Color.FromArgb(230, 216, 177)
        btnConfirm.Font = New Font("Segoe UI", 12, FontStyle.Bold)
        btnConfirm.Enabled = False

        btnCancel.BackColor = Color.FromArgb(147, 53, 53)
        btnCancel.ForeColor = Color.FromArgb(230, 216, 177)
        btnCancel.Font = New Font("Segoe UI", 12, FontStyle.Bold)

        lblSelectedPayment.Text = "Please select a payment method"
        lblSelectedPayment.Font = New Font("Segoe UI", 10, FontStyle.Italic)
        lblSelectedPayment.ForeColor = Color.FromArgb(79, 51, 40)
    End Sub

    Private Sub btnCash_Click(sender As Object, e As EventArgs) Handles btnCash.Click
        SelectPaymentMethod("Cash", btnCash)
    End Sub

    Private Sub btnGCash_Click(sender As Object, e As EventArgs) Handles btnGCash.Click
        SelectPaymentMethod("GCash", btnGCash)
    End Sub

    Private Sub btnBankTransaction_Click(sender As Object, e As EventArgs) Handles btnBankTransaction.Click
        SelectPaymentMethod("Bank Transaction", btnBankTransaction)
    End Sub

    Private Sub SelectPaymentMethod(method As String, selectedButton As Button)
        selectedPaymentMethod = method
        lblSelectedPayment.Text = $"Selected: {method}"
        btnConfirm.Enabled = True

        ' Reset all button colors
        btnCash.BackColor = Color.FromArgb(147, 53, 53)
        btnGCash.BackColor = Color.FromArgb(147, 53, 53)
        btnBankTransaction.BackColor = Color.FromArgb(147, 53, 53)

        ' Highlight selected button
        selectedButton.BackColor = Color.FromArgb(79, 51, 40)
    End Sub

    Private Sub btnConfirm_Click(sender As Object, e As EventArgs) Handles btnConfirm.Click
        If String.IsNullOrEmpty(selectedPaymentMethod) Then
            MessageBox.Show("Please select a payment method.", "Payment Method Required",
                             MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Process checkout
        If ProcessCheckout() Then
            MessageBox.Show("Checkout successful!", "Success",
       MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.DialogResult = DialogResult.OK
            Me.Close()
        Else
            MessageBox.Show("Checkout failed. Please try again.", "Error",
          MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    Private Function ProcessCheckout() As Boolean
        Try
            ' Get cart items from parent form
            Dim cartItems As List(Of posForm.CartItem) = parentForm.GetCartItems()

            If cartItems.Count = 0 Then
                MessageBox.Show("Cart is empty!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return False
            End If

            ' Get current user ID
            Dim currentUserID As Integer = GlobalUserSession.CurrentUserID
            If currentUserID = 0 Then
                MessageBox.Show("User session not found. Please log in again.", "Session Error",
               MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If

            Using conn As New SqlConnection(GetConnectionString())
                conn.Open()

                ' Process each cart item
                For Each item As posForm.CartItem In cartItems
                    ' Determine which table the product belongs to
                    Dim productIsRetail As Boolean = IsRetailProduct(item.ProductID, conn)

                    ' Get effective price (with discount if applicable)
                    Dim effectivePrice As Decimal = If(item.DiscountPrice.HasValue,
                     item.DiscountPrice.Value,
                   item.UnitPrice)
                    Dim itemTotal As Decimal = effectivePrice * item.Quantity

                    ' Insert into appropriate sales report table
                    If productIsRetail Then
                        InsertRetailSalesReport(conn, item, effectivePrice, itemTotal, currentUserID)
                    Else
                        InsertWholesaleSalesReport(conn, item, effectivePrice, itemTotal, currentUserID)
                    End If

                    ' Update stock quantity
                    UpdateStockQuantity(conn, item.ProductID, item.Quantity, productIsRetail)
                Next
            End Using

            ' Clear the cart in parent form
            parentForm.ClearCart()
            Return True

        Catch ex As Exception
            MessageBox.Show("Error processing checkout: " & ex.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    Private Function IsRetailProduct(productID As Integer, conn As SqlConnection) As Boolean
        Dim query As String = "SELECT COUNT(*) FROM retailProducts WHERE ProductID = @ProductID"
        Using cmd As New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@ProductID", productID)
            Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())
            Return count > 0
        End Using
    End Function

    Private Sub InsertRetailSalesReport(conn As SqlConnection, item As posForm.CartItem,
         effectivePrice As Decimal, itemTotal As Decimal,
      handledBy As Integer)
        Dim query As String = "
            INSERT INTO RetailSalesReport 
                (SaleDate, ProductID, CategoryID, QuantitySold, UnitPrice, TotalAmount, PaymentMethod, HandledBy)
            VALUES 
                (GETDATE(), @ProductID, @CategoryID, @QuantitySold, @UnitPrice, @TotalAmount, @PaymentMethod, @HandledBy)"

        Using cmd As New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@ProductID", item.ProductID)
            cmd.Parameters.AddWithValue("@CategoryID", item.CategoryID)
            cmd.Parameters.AddWithValue("@QuantitySold", item.Quantity)
            cmd.Parameters.AddWithValue("@UnitPrice", effectivePrice)
            cmd.Parameters.AddWithValue("@TotalAmount", itemTotal)
            cmd.Parameters.AddWithValue("@PaymentMethod", selectedPaymentMethod)
            cmd.Parameters.AddWithValue("@HandledBy", handledBy)
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Private Sub InsertWholesaleSalesReport(conn As SqlConnection, item As posForm.CartItem,
 effectivePrice As Decimal, itemTotal As Decimal,
   handledBy As Integer)
        Dim query As String = "
            INSERT INTO SalesReport 
                (SaleDate, ProductID, CategoryID, QuantitySold, UnitPrice, TotalAmount, PaymentMethod, HandledBy)
            VALUES 
                (GETDATE(), @ProductID, @CategoryID, @QuantitySold, @UnitPrice, @TotalAmount, @PaymentMethod, @HandledBy)"

        Using cmd As New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@ProductID", item.ProductID)
            cmd.Parameters.AddWithValue("@CategoryID", item.CategoryID)
            cmd.Parameters.AddWithValue("@QuantitySold", item.Quantity)
            cmd.Parameters.AddWithValue("@UnitPrice", effectivePrice)
            cmd.Parameters.AddWithValue("@TotalAmount", itemTotal)
            cmd.Parameters.AddWithValue("@PaymentMethod", selectedPaymentMethod)
            cmd.Parameters.AddWithValue("@HandledBy", handledBy)
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Private Sub UpdateStockQuantity(conn As SqlConnection, productID As Integer,
        quantitySold As Integer, isRetailProduct As Boolean)
        Dim tableName As String = If(isRetailProduct, "retailProducts", "wholesaleProducts")
        Dim query As String = $"UPDATE {tableName} 
      SET StockQuantity = StockQuantity - @QuantitySold 
     WHERE ProductID = @ProductID"

        Using cmd As New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@QuantitySold", quantitySold)
            cmd.Parameters.AddWithValue("@ProductID", productID)
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Private Function GetConnectionString() As String
        Return SharedUtilities.GetConnectionString()
    End Function

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub
End Class
