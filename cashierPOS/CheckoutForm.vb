Imports Microsoft.Data.SqlClient

Public Class CheckoutForm
    Private parentForm As posForm
    Private totalAmount As Decimal
    Private selectedPaymentMethod As String = ""
    Private isDelivery As Boolean = False
    Private deliveryAddress As String = ""
    Private deliveryLatitude As Double = 0
    Private deliveryLongitude As Double = 0

    Public Sub New(parent As posForm, total As Decimal)
        InitializeComponent()
        Me.parentForm = parent
        Me.totalAmount = total
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.StartPosition = FormStartPosition.CenterParent
        Me.BackColor = Color.FromArgb(230, 216, 177)
        ' Enable keyboard shortcuts
        Me.KeyPreview = True
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

    ''' <summary>
    ''' Handle keyboard shortcuts for checkout form
    ''' C = Cash, G = GCash, B = Bank Transaction
    ''' Enter = Confirm, Esc = Cancel
    ''' </summary>
    Private Sub CheckoutForm_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        Select Case e.KeyCode
            Case Keys.C
                ' Press C for Cash
                btnCash.PerformClick()
                e.Handled = True

            Case Keys.G
                ' Press G for GCash
                btnGCash.PerformClick()
                e.Handled = True

            Case Keys.B
                ' Press B for Bank Transaction
                btnBankTransaction.PerformClick()
                e.Handled = True

            Case Keys.Enter
                ' Press Enter to Confirm (only if button is enabled)
                If btnConfirm.Enabled Then
                    btnConfirm.PerformClick()
                    e.Handled = True
                End If

            Case Keys.Escape
                ' Press Esc to Cancel/Exit
                btnCancel.PerformClick()
                e.Handled = True
        End Select
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

        Debug.WriteLine("==========================================================")
        Debug.WriteLine("========== CHECKOUT PROCESS STARTED ==========")
        Debug.WriteLine("==========================================================")

        ' Check if cart contains wholesale products
        Dim cartItems As List(Of posForm.CartItem) = parentForm.GetCartItems()
        Dim hasWholesaleProducts As Boolean = False
        Dim hasRetailProducts As Boolean = False

        Debug.WriteLine($"Total items in cart: {cartItems.Count}")

        ' Check product types in cart - THOROUGH CHECK
        For Each item In cartItems
            ' Use the ProductType stored in the cart item (no database lookup needed!)
            Dim productType As String = If(String.IsNullOrEmpty(item.ProductType), "Unknown", item.ProductType)
            Debug.WriteLine($"  - ProductID: {item.ProductID}, Name: {item.ProductName}, Type: {productType}")

            If productType = "Wholesale" Then
                hasWholesaleProducts = True
            ElseIf productType = "Retail" Then
                hasRetailProducts = True
            Else
                ' Unknown product type - log warning
                Debug.WriteLine($"  WARNING: Unknown product type for ProductID {item.ProductID}")
            End If
        Next

        Debug.WriteLine($"Cart has wholesale products: {hasWholesaleProducts}")
        Debug.WriteLine($"Cart has retail products: {hasRetailProducts}")

        ' ONLY ask for delivery if cart has wholesale products
        If hasWholesaleProducts Then
            Dim deliveryMessage As String

            ' Customize message based on cart contents
            If hasRetailProducts Then
                deliveryMessage = "Your cart contains wholesale and retail products." & vbCrLf +
                    "Do you want the WHOLESALE items delivered?" & vbCrLf & vbCrLf &
                  "(Retail items are always for pickup)" & vbCrLf & vbCrLf &
                  "Click 'Yes' for Delivery or 'No' for Pickup"
            Else
                deliveryMessage = "Your cart contains wholesale products." & vbCrLf +
                     "Do you want this order delivered?" & vbCrLf & vbCrLf &
                       "Click 'Yes' for Delivery or 'No' for Pickup"
            End If

            Dim deliveryChoice As DialogResult = MessageBox.Show(
             deliveryMessage,
               "Delivery or Pickup?",
                    MessageBoxButtons.YesNoCancel,
                  MessageBoxIcon.Question)

            Debug.WriteLine($"User delivery choice: {deliveryChoice}")

            If deliveryChoice = DialogResult.Cancel Then
                Debug.WriteLine("User CANCELLED the delivery/pickup selection")
                Return
            ElseIf deliveryChoice = DialogResult.Yes Then
                ' Customer wants delivery for wholesale items
                isDelivery = True
                Debug.WriteLine($"✓ User selected DELIVERY for wholesale items - isDelivery set to TRUE")

                ' Open address selection form
                Try
                    Dim addressForm As New DeliveryAddressFormSimple(Me)
                    Debug.WriteLine("Opening DeliveryAddressFormSimple...")

                    If addressForm.ShowDialog() = DialogResult.OK Then
                        deliveryAddress = addressForm.DeliveryAddress
                        deliveryLatitude = addressForm.DeliveryLatitude
                        deliveryLongitude = addressForm.DeliveryLongitude

                        Debug.WriteLine($"✓ DELIVERY DATA CAPTURED FROM ADDRESS FORM:")
                        Debug.WriteLine($"  ✓ isDelivery: {isDelivery}")
                        Debug.WriteLine($"  ✓ Address: {deliveryAddress}")
                        Debug.WriteLine($"  ✓ Latitude: {deliveryLatitude}")
                        Debug.WriteLine($"✓ Longitude: {deliveryLongitude}")
                    Else
                        ' User cancelled address selection
                        Debug.WriteLine("✗ User CANCELLED address form - ABORTING checkout")
                        Return
                    End If
                Catch ex As Exception
                    ' If form fails, use simple text entry as fallback
                    Debug.WriteLine($"✗ Address form error: {ex.Message} - falling back to text entry")

                    ' Create a simple text input dialog as ultimate fallback
                    Dim manualAddress As String = InputBox("Address form not available. Please enter delivery address manually:",
                             "Enter Delivery Address",
                          "")

                    If String.IsNullOrWhiteSpace(manualAddress) Then
                        Debug.WriteLine("✗ User cancelled manual address entry - ABORTING checkout")
                        Return
                    End If

                    deliveryAddress = manualAddress
                    deliveryLatitude = 14.5995 ' Default Manila coordinates
                    deliveryLongitude = 120.9842

                    Debug.WriteLine($"✓ DELIVERY DATA FROM MANUAL ENTRY:")
                    Debug.WriteLine($"  ✓ isDelivery: {isDelivery}")
                    Debug.WriteLine($"  ✓ Address: {deliveryAddress}")
                    Debug.WriteLine($"  ✓ Using default coordinates")
                End Try
            Else
                ' Customer wants pickup for wholesale items
                isDelivery = False
                deliveryAddress = ""
                deliveryLatitude = 0
                deliveryLongitude = 0
                Debug.WriteLine($"✓ User selected PICKUP for wholesale items - isDelivery set to FALSE")
                Debug.WriteLine($"  ✓ Delivery fields reset to empty/zero")
            End If
        Else
            ' No wholesale products in cart - skip delivery prompt entirely
            isDelivery = False
            deliveryAddress = ""
            deliveryLatitude = 0
            deliveryLongitude = 0
            Debug.WriteLine("✓ Cart has NO wholesale products - skipping delivery/pickup selection")
            Debug.WriteLine("✓ All items are retail - processing as pickup without delivery prompt")
        End If

        Debug.WriteLine($"")
        Debug.WriteLine($"========== FINAL STATE BEFORE ProcessCheckout ==========")
        Debug.WriteLine($"  isDelivery: {isDelivery} (Type: {isDelivery.GetType().Name})")
        Debug.WriteLine($"  deliveryAddress: '{deliveryAddress}' (Length: {deliveryAddress.Length})")
        Debug.WriteLine($"  deliveryLatitude: {deliveryLatitude}")
        Debug.WriteLine($"  deliveryLongitude: {deliveryLongitude}")
        Debug.WriteLine($"  hasWholesaleProducts: {hasWholesaleProducts}")
        Debug.WriteLine($"  hasRetailProducts: {hasRetailProducts}")
        Debug.WriteLine($"========================================================")
        Debug.WriteLine($"")

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

    ''' <summary>
    ''' Gets the product type (Wholesale or Retail) for a given product ID
    ''' Returns "Wholesale", "Retail", or "Unknown"
    ''' NOTE: This function is now deprecated - we use CartItem.ProductType instead
    ''' Kept for backward compatibility only
    ''' </summary>
    Private Function GetProductType(productID As Integer) As String
        Try
            Using conn As New SqlConnection(GetConnectionString())
                conn.Open()

                ' Check wholesale products first
                Dim wholesaleQuery As String = "SELECT COUNT(*) FROM wholesaleProducts WHERE ProductID = @ProductID"
                Using cmd As New SqlCommand(wholesaleQuery, conn)
                    cmd.Parameters.AddWithValue("@ProductID", productID)
                    Dim wholesaleCount As Integer = Convert.ToInt32(cmd.ExecuteScalar())
                    If wholesaleCount > 0 Then
                        Debug.WriteLine($"    → ProductID {productID} found in wholesaleProducts table")
                        Return "Wholesale"
                    End If
                End Using

                ' Check retail products
                Dim retailQuery As String = "SELECT COUNT(*) FROM retailProducts WHERE ProductID = @ProductID"
                Using cmd As New SqlCommand(retailQuery, conn)
                    cmd.Parameters.AddWithValue("@ProductID", productID)
                    Dim retailCount As Integer = Convert.ToInt32(cmd.ExecuteScalar())
                    If retailCount > 0 Then
                        Debug.WriteLine($"    → ProductID {productID} found in retailProducts table")
                        Return "Retail"
                    End If
                End Using
            End Using
        Catch ex As Exception
            Debug.WriteLine($"    ✗ Error getting product type for ProductID {productID}: {ex.Message}")
        End Try

        Debug.WriteLine($"    ✗ ProductID {productID} NOT found in any product table - returning Unknown")
        Return "Unknown"
    End Function

    Private Function IsWholesaleProduct(productID As Integer) As Boolean
        Return GetProductType(productID) = "Wholesale"
    End Function

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
                    ' Use the ProductType stored in the cart item (no database lookup!)
                    Dim productType As String = If(String.IsNullOrEmpty(item.ProductType), "Unknown", item.ProductType)
                    Dim productIsRetail As Boolean = (productType = "Retail")
                    Dim productIsWholesale As Boolean = (productType = "Wholesale")

                    Debug.WriteLine($"Processing item: {item.ProductName} (ProductID: {item.ProductID}, Type: {productType})")

                    ' Skip unknown products
                    If productType = "Unknown" Then
                        Debug.WriteLine($"✗ Skipping unknown product type for ProductID {item.ProductID}")
                        Continue For
                    End If

                    ' Get effective price (with discount if applicable)
                    Dim effectivePrice As Decimal = If(item.DiscountPrice.HasValue,
  item.DiscountPrice.Value,
         item.UnitPrice)

                    ' Calculate total correctly (price * quantity)
                    Dim itemTotal As Decimal = effectivePrice * item.Quantity

                    ' Insert into appropriate sales report table
                    If productIsRetail Then
                        ' Retail products - NEVER get delivery info
                        InsertRetailSalesReport(conn, item, effectivePrice, itemTotal, currentUserID)
                        Debug.WriteLine($"  ✓ Inserted into RetailSalesReport (retail product - no delivery)")
                    ElseIf productIsWholesale Then
                        ' Wholesale products - include delivery info ONLY if isDelivery is true
                        InsertWholesaleSalesReport(conn, item, effectivePrice, itemTotal, currentUserID)
                        Debug.WriteLine($"  ✓ Inserted into SalesReport (wholesale product - isDelivery: {isDelivery})")
                    End If

                    ' Update stock quantity
                    UpdateStockQuantity(conn, item.ProductID, item.Quantity, productIsRetail)
                    Debug.WriteLine($"  ✓ Stock updated (-{item.Quantity})")
                Next
            End Using

            ' Clear the cart in parent form
            parentForm.ClearCart()
            Return True
        Catch ex As Exception
            MessageBox.Show("Error processing checkout: " & ex.Message, "Error",
        MessageBoxButtons.OK, MessageBoxIcon.Error)
            Debug.WriteLine($"Checkout error: {ex.Message}")
            Debug.WriteLine($"Stack trace: {ex.StackTrace}")
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
        Dim query As String = "INSERT INTO SalesReport (SaleDate, ProductID, CategoryID, QuantitySold, UnitPrice, TotalAmount, PaymentMethod, HandledBy, IsDelivery, DeliveryAddress, DeliveryLatitude, DeliveryLongitude, DeliveryStatus) VALUES (GETDATE(), @ProductID, @CategoryID, @QuantitySold, @UnitPrice, @TotalAmount, @PaymentMethod, @HandledBy, @IsDelivery, @DeliveryAddress, @DeliveryLatitude, @DeliveryLongitude, @DeliveryStatus)"

        Try
            Using cmd As New SqlCommand(query, conn)
                ' Add ALL parameters with EXPLICIT data types
                cmd.Parameters.Add("@ProductID", SqlDbType.Int).Value = item.ProductID
                cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = item.CategoryID
                cmd.Parameters.Add("@QuantitySold", SqlDbType.Int).Value = item.Quantity
                cmd.Parameters.Add("@UnitPrice", SqlDbType.Decimal).Value = effectivePrice
                cmd.Parameters.Add("@TotalAmount", SqlDbType.Decimal).Value = itemTotal
                cmd.Parameters.Add("@PaymentMethod", SqlDbType.NVarChar, 50).Value = selectedPaymentMethod
                cmd.Parameters.Add("@HandledBy", SqlDbType.Int).Value = handledBy

                ' CRITICAL: Only set delivery info if this is ACTUALLY a delivery for wholesale items
                ' This ensures that when cart has both retail and wholesale, only wholesale items
                ' marked for delivery get the delivery information
                If isDelivery Then
                    ' This is a delivery order for wholesale items
                    cmd.Parameters.Add("@IsDelivery", SqlDbType.Bit).Value = 1
                    cmd.Parameters.Add("@DeliveryAddress", SqlDbType.NVarChar, -1).Value = deliveryAddress
                    cmd.Parameters.Add("@DeliveryLatitude", SqlDbType.Float).Value = deliveryLatitude
                    cmd.Parameters.Add("@DeliveryLongitude", SqlDbType.Float).Value = deliveryLongitude
                    cmd.Parameters.Add("@DeliveryStatus", SqlDbType.NVarChar, 50).Value = "Pending"

                    Debug.WriteLine($"  ✓ Wholesale item (ProductID: {item.ProductID}) set for DELIVERY:")
                    Debug.WriteLine($"    - Address: {deliveryAddress}")
                    Debug.WriteLine($"    - Coordinates: ({deliveryLatitude}, {deliveryLongitude})")
                    Debug.WriteLine($"    - Status: Pending")
                    Debug.WriteLine($"    - This item WILL appear in delivery logs")
                Else
                    ' This is a pickup order for wholesale items (or user selected pickup)
                    cmd.Parameters.Add("@IsDelivery", SqlDbType.Bit).Value = 0
                    cmd.Parameters.Add("@DeliveryAddress", SqlDbType.NVarChar, -1).Value = DBNull.Value
                    cmd.Parameters.Add("@DeliveryLatitude", SqlDbType.Float).Value = DBNull.Value
                    cmd.Parameters.Add("@DeliveryLongitude", SqlDbType.Float).Value = DBNull.Value
                    cmd.Parameters.Add("@DeliveryStatus", SqlDbType.NVarChar, 50).Value = DBNull.Value

                    Debug.WriteLine($"  ✓ Wholesale item (ProductID: {item.ProductID}) set for PICKUP:")
                    Debug.WriteLine($"    - No delivery info stored")
                    Debug.WriteLine($"    - This item will NOT appear in delivery logs")
                End If

                cmd.ExecuteNonQuery()
                Debug.WriteLine($"  ✓ Wholesale sale inserted successfully: ProductID={item.ProductID}, IsDelivery={isDelivery}")
            End Using
        Catch ex As Exception
            Debug.WriteLine($"  ✗ Error inserting wholesale sale: {ex.Message}")
            Throw
        End Try
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