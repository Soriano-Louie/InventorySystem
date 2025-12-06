Imports System.Drawing.Printing
Imports Microsoft.Data.SqlClient

Public Class CheckoutForm
    Private parentForm As posForm
    Private totalAmount As Decimal
    Private selectedPaymentMethod As String = ""
    Private isDelivery As Boolean = False
    Private deliveryAddress As String = ""
    Private deliveryLatitude As Double = 0
    Private deliveryLongitude As Double = 0

    ' Payment details for GCash and Bank Transaction
    Private payerName As String = ""

    Private referenceNumber As String = ""
    Private bankName As String = ""

    ' Payment tracking for cash
    Private amountPaid As Decimal = 0

    Private changeGiven As Decimal = 0

    ' Receipt printing support
    Private WithEvents printDocument As New PrintDocument()

    Private receiptContent As String = ""
    Private receiptLines As List(Of String)

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

        ' Set Accept and Cancel buttons for Enter/Esc key support
        Me.AcceptButton = btnConfirm
        Me.CancelButton = btnCancel
    End Sub

    ''' Handle keyboard shortcuts for checkout form
    ''' C = Cash, G = GCash, B = Bank Transaction
    ''' Enter = Confirm, Esc = Cancel
    Private Sub CheckoutForm_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        Select Case e.KeyCode
            Case Keys.C
                ' Press C for Cash
                If Not e.Control AndAlso Not e.Alt Then
                    btnCash.PerformClick()
                    e.Handled = True
                    e.SuppressKeyPress = True
                End If

            Case Keys.G
                ' Press G for GCash
                If Not e.Control AndAlso Not e.Alt Then
                    btnGCash.PerformClick()
                    e.Handled = True
                    e.SuppressKeyPress = True
                End If

            Case Keys.B
                ' Press B for Bank Transaction
                If Not e.Control AndAlso Not e.Alt Then
                    btnBankTransaction.PerformClick()
                    e.Handled = True
                    e.SuppressKeyPress = True
                End If

            Case Keys.Enter
                ' Press Enter to Confirm (only if button is enabled)
                If btnConfirm.Enabled Then
                    btnConfirm.PerformClick()
                    e.Handled = True
                    e.SuppressKeyPress = True
                End If

            Case Keys.Escape
                ' Press Esc to Cancel/Exit
                btnCancel.PerformClick()
                e.Handled = True
                e.SuppressKeyPress = True
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

    ''' <summary>
    ''' Prompts cashier to enter amount paid by customer for cash payments
    ''' </summary>
    Private Function PromptForCashPayment() As Boolean
        Try
            ' Create input dialog
            Dim amountInput As String = InputBox($"Total Amount: ₱{totalAmount:N2}" & vbCrLf & vbCrLf &
                                                "Enter amount paid by customer:",
                                                "Cash Payment",
                                                totalAmount.ToString("F2"))

            If String.IsNullOrWhiteSpace(amountInput) Then
                MessageBox.Show("Payment cancelled.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If

            ' Validate input
            If Not Decimal.TryParse(amountInput, amountPaid) Then
                MessageBox.Show("Please enter a valid amount.", "Invalid Input",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return False
            End If

            ' Check if amount is sufficient
            If amountPaid < totalAmount Then
                MessageBox.Show($"Insufficient amount!" & vbCrLf & vbCrLf &
                              $"Total: ₱{totalAmount:N2}" & vbCrLf &
                              $"Paid: ₱{amountPaid:N2}" & vbCrLf &
                              $"Short: ₱{(totalAmount - amountPaid):N2}",
                              "Insufficient Payment",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Warning)
                Return False
            End If

            ' Calculate change
            changeGiven = amountPaid - totalAmount

            ' Show change to cashier
            If changeGiven > 0 Then
                MessageBox.Show($"Total: ₱{totalAmount:N2}" & vbCrLf &
                              $"Paid: ₱{amountPaid:N2}" & vbCrLf & vbCrLf &
                              $"Change: ₱{changeGiven:N2}",
                              "Change Due",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Information)
            Else
                MessageBox.Show($"Exact amount received: ₱{totalAmount:N2}",
                              "Payment Confirmed",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Information)
            End If

            Return True
        Catch ex As Exception
            MessageBox.Show($"Error processing payment: {ex.Message}", "Error",
                          MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    Private Sub SelectPaymentMethod(method As String, selectedButton As Button)
        selectedPaymentMethod = method
        lblSelectedPayment.Text = $"Selected: {method}"
        btnConfirm.Enabled = True

        ' Move focus to the Confirm button so Enter triggers confirmation
        Try
            btnConfirm.Focus()
            btnConfirm.Select()
            Me.ActiveControl = btnConfirm
        Catch
            ' Ignore focus failures
        End Try

        ' Reset all button colors
        btnCash.BackColor = Color.FromArgb(147, 53, 53)
        btnGCash.BackColor = Color.FromArgb(147, 53, 53)
        btnBankTransaction.BackColor = Color.FromArgb(147, 53, 53)

        ' Highlight selected button
        selectedButton.BackColor = Color.FromArgb(79, 51, 40)

        ' Handle payment method specific actions
        If method = "Cash" Then
            ' For cash, prompt for amount paid immediately
            If Not PromptForCashPayment() Then
                ' User cancelled or entered invalid amount
                selectedPaymentMethod = ""
                lblSelectedPayment.Text = "Please select a payment method"
                btnConfirm.Enabled = False
                selectedButton.BackColor = Color.FromArgb(147, 53, 53)
                amountPaid = 0
                changeGiven = 0

                ' Clear focus so Enter won't activate the previously focused button
                Try
                    Me.ActiveControl = Nothing
                Catch
                End Try
            Else
                ' Ensure Confirm keeps focus after successful cash entry
                Try
                    btnConfirm.Focus()
                    btnConfirm.Select()
                    Me.ActiveControl = btnConfirm
                Catch
                End Try
            End If
        ElseIf method = "GCash" OrElse method = "Bank Transaction" Then
            ' Existing code for GCash/Bank Transaction
            Dim paymentDetailsForm As New PaymentDetailsForm(method)

            If paymentDetailsForm.ShowDialog() = DialogResult.OK Then
                payerName = paymentDetailsForm.PayerName
                referenceNumber = paymentDetailsForm.ReferenceNumber
                bankName = paymentDetailsForm.BankName

                ' For non-cash, amount paid equals total (full payment)
                amountPaid = totalAmount
                changeGiven = 0

                Debug.WriteLine($"Payment details captured:")
                Debug.WriteLine($"  - Payer Name: {payerName}")
                Debug.WriteLine($"  - Reference Number: {referenceNumber}")
                If method = "Bank Transaction" Then
                    Debug.WriteLine($"  - Bank Name: {bankName}")
                End If

                ' Ensure Confirm keeps focus after successful payment details entry
                Try
                    btnConfirm.Focus()
                    btnConfirm.Select()
                    Me.ActiveControl = btnConfirm
                Catch
                End Try
            Else
                ' User cancelled
                selectedPaymentMethod = ""
                lblSelectedPayment.Text = "Please select a payment method"
                btnConfirm.Enabled = False
                selectedButton.BackColor = Color.FromArgb(147, 53, 53)
                payerName = ""
                referenceNumber = ""
                bankName = ""
                amountPaid = 0
                changeGiven = 0

                ' Clear focus so Enter won't activate the previously focused button
                Try
                    Me.ActiveControl = Nothing
                Catch
                End Try
            End If
        End If
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

        ' CRITICAL FIX: Store cart items BEFORE ProcessCheckout clears them
        Dim cartItemsCopy As New List(Of posForm.CartItem)
        For Each item In cartItems
            cartItemsCopy.Add(item)
        Next

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

            ' Ask user if they want to print receipt
            Dim printReceipt As DialogResult = MessageBox.Show(
             "Transaction completed successfully!" & vbCrLf & vbCrLf &
       "Would you like to print a receipt?",
           "Print Receipt?",
                MessageBoxButtons.YesNo,
    MessageBoxIcon.Question)

            If printReceipt = DialogResult.Yes Then
                ' Use the COPY of cart items (original was cleared by ProcessCheckout)
                PrintTransactionReceipt(cartItemsCopy)
            End If

            Me.DialogResult = DialogResult.OK
            Me.Close()
        Else
            MessageBox.Show("Checkout failed. Please try again.", "Error",
  MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    ''' Gets the product type (Wholesale or Retail) for a given product ID
    ''' Returns "Wholesale", "Retail", or "Unknown"
    ''' NOTE: This function is now deprecated - we use CartItem.ProductType instead
    ''' Kept for backward compatibility only
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
        ' Use GETDATE() which includes both date and time components
        Dim query As String = "
INSERT INTO RetailSalesReport
(SaleDate, ProductID, CategoryID, QuantitySold, UnitPrice, TotalAmount, PaymentMethod,
 HandledBy, PayerName, ReferenceNumber, BankName, AmountPaid, ChangeGiven)
VALUES
(GETDATE(), @ProductID, @CategoryID, @QuantitySold, @UnitPrice, @TotalAmount, @PaymentMethod,
 @HandledBy, @PayerName, @ReferenceNumber, @BankName, @AmountPaid, @ChangeGiven)"

        Using cmd As New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@ProductID", item.ProductID)
            cmd.Parameters.AddWithValue("@CategoryID", item.CategoryID)
            cmd.Parameters.AddWithValue("@QuantitySold", item.Quantity)
            cmd.Parameters.AddWithValue("@UnitPrice", effectivePrice)
            cmd.Parameters.AddWithValue("@TotalAmount", itemTotal)
            cmd.Parameters.AddWithValue("@PaymentMethod", selectedPaymentMethod)
            cmd.Parameters.AddWithValue("@HandledBy", handledBy)

            ' Add payment details (NULL for Cash if no payer name)
            cmd.Parameters.AddWithValue("@PayerName", If(String.IsNullOrWhiteSpace(payerName), DBNull.Value, payerName))
            cmd.Parameters.AddWithValue("@ReferenceNumber", If(String.IsNullOrWhiteSpace(referenceNumber), DBNull.Value, referenceNumber))
            cmd.Parameters.AddWithValue("@BankName", If(String.IsNullOrWhiteSpace(bankName), DBNull.Value, bankName))

            ' Add new payment tracking columns
            cmd.Parameters.AddWithValue("@AmountPaid", If(amountPaid > 0, amountPaid, DBNull.Value))
            cmd.Parameters.AddWithValue("@ChangeGiven", If(changeGiven > 0, changeGiven, DBNull.Value))

            ' Log the exact time being inserted
            Debug.WriteLine($"Inserting retail sale at: {DateTime.Now:yyyy-MM-dd HH:mm:ss}")

            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Private Sub InsertWholesaleSalesReport(conn As SqlConnection, item As posForm.CartItem,
                                          effectivePrice As Decimal, itemTotal As Decimal,
                                          handledBy As Integer)
        Dim query As String = "INSERT INTO SalesReport
        (SaleDate, ProductID, CategoryID, QuantitySold, UnitPrice, TotalAmount, PaymentMethod,
         HandledBy, IsDelivery, DeliveryAddress, DeliveryLatitude, DeliveryLongitude,
         DeliveryStatus, PayerName, ReferenceNumber, BankName, AmountPaid, ChangeGiven)
        VALUES
        (GETDATE(), @ProductID, @CategoryID, @QuantitySold, @UnitPrice, @TotalAmount, @PaymentMethod,
         @HandledBy, @IsDelivery, @DeliveryAddress, @DeliveryLatitude, @DeliveryLongitude,
         @DeliveryStatus, @PayerName, @ReferenceNumber, @BankName, @AmountPaid, @ChangeGiven)"

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

                ' Add payment details (NULL for Cash if no payer name)
                cmd.Parameters.Add("@PayerName", SqlDbType.NVarChar, 200).Value = If(String.IsNullOrWhiteSpace(payerName), DBNull.Value, payerName)
                cmd.Parameters.Add("@ReferenceNumber", SqlDbType.NVarChar, 100).Value = If(String.IsNullOrWhiteSpace(referenceNumber), DBNull.Value, referenceNumber)
                cmd.Parameters.Add("@BankName", SqlDbType.NVarChar, 200).Value = If(String.IsNullOrWhiteSpace(bankName), DBNull.Value, bankName)

                ' Add new payment tracking columns
                cmd.Parameters.Add("@AmountPaid", SqlDbType.Decimal).Value = If(amountPaid > 0, amountPaid, DBNull.Value)
                cmd.Parameters.Add("@ChangeGiven", SqlDbType.Decimal).Value = If(changeGiven > 0, changeGiven, DBNull.Value)

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

    ''' Build and print transaction receipt for retail, wholesale, or mixed transactions
    ''' Handles all product types and delivery information
    Private Sub PrintTransactionReceipt(cartItems As List(Of posForm.CartItem))
        Try
            ' Build receipt content
            receiptContent = BuildReceiptContent(cartItems)
            receiptLines = receiptContent.Split(New String() {vbCrLf, vbLf}, StringSplitOptions.None).ToList()

            ' Configure print document for receipt size
            printDocument.DefaultPageSettings.PaperSize = New PaperSize("Receipt", 300, 600) ' 3" width
            printDocument.DefaultPageSettings.Margins = New Margins(10, 10, 10, 10)

            ' Show print preview
            Dim printPreview As New PrintPreviewDialog()
            printPreview.Document = printDocument
            printPreview.Width = 400
            printPreview.Height = 600
            printPreview.StartPosition = FormStartPosition.CenterParent
            printPreview.ShowDialog()
        Catch ex As Exception
            MessageBox.Show("Error preparing receipt: " & ex.Message, "Print Error",
            MessageBoxButtons.OK, MessageBoxIcon.Error)
            Debug.WriteLine($"Receipt print error: {ex.ToString()}")
        End Try
    End Sub

    ''' Build formatted receipt content with all transaction details
    Private Function BuildReceiptContent(cartItems As List(Of posForm.CartItem)) As String
        Dim content As New System.Text.StringBuilder()
        Dim currentUser As String = If(GlobalUserSession.CurrentUsername, "Cashier")

        ' Header
        content.AppendLine("================================")
        content.AppendLine("   INVENTORY SYSTEM POS")
        content.AppendLine("     SALES RECEIPT")
        content.AppendLine("================================")
        content.AppendLine()
        content.AppendLine($"Date: {Date.Now:MMM dd, yyyy}")
        content.AppendLine($"Time: {Date.Now:hh:mm:ss tt}")
        content.AppendLine($"Cashier: {currentUser}")
        content.AppendLine()
        content.AppendLine("--------------------------------")
        content.AppendLine("ITEMS")
        content.AppendLine("--------------------------------")

        ' Group items by product type
        Dim retailItems = cartItems.Where(Function(i) i.ProductType = "Retail").ToList()
        Dim wholesaleItems = cartItems.Where(Function(i) i.ProductType = "Wholesale").ToList()

        Dim retailTotal As Decimal = 0
        Dim wholesaleTotal As Decimal = 0

        ' Print Retail Items
        If retailItems.Count > 0 Then
            content.AppendLine()
            content.AppendLine("** RETAIL ITEMS **")
            content.AppendLine()

            For Each item In retailItems
                Dim effectivePrice As Decimal = If(item.DiscountPrice.HasValue, item.DiscountPrice.Value, item.UnitPrice)
                Dim itemTotal As Decimal = effectivePrice * item.Quantity
                retailTotal += itemTotal

                ' Product name (truncate if too long)
                Dim productName As String = item.ProductName
                If productName.Length > 28 Then
                    productName = productName.Substring(0, 25) & "..."

                End If
                content.AppendLine(productName)

                ' Quantity and price
                content.AppendLine($"  {item.Quantity} x ₱{effectivePrice:N2} = ₱{itemTotal:N2}")

                ' Show discount if applicable
                If item.DiscountPrice.HasValue Then
                    Dim savings As Decimal = (item.UnitPrice - item.DiscountPrice.Value) * item.Quantity
                    content.AppendLine($"  (Saved: ₱{savings:N2})")
                End If
                content.AppendLine()
            Next

            content.AppendLine($"Retail Subtotal: ₱{retailTotal:N2}")
            content.AppendLine()
        End If

        ' Print Wholesale Items
        If wholesaleItems.Count > 0 Then
            content.AppendLine()
            content.AppendLine("** WHOLESALE ITEMS **")
            content.AppendLine()

            For Each item In wholesaleItems
                Dim effectivePrice As Decimal = If(item.DiscountPrice.HasValue, item.DiscountPrice.Value, item.UnitPrice)
                Dim itemTotal As Decimal = effectivePrice * item.Quantity
                wholesaleTotal += itemTotal

                ' Product name (truncate if too long)
                Dim productName As String = item.ProductName
                If productName.Length > 28 Then
                    productName = productName.Substring(0, 25) & "..."

                End If
                content.AppendLine(productName)

                ' Quantity and price - include unit if kg
                Dim quantityDisplay As String = item.Quantity.ToString()

                ' Add unit type if it's kg (or any unit really)
                If Not String.IsNullOrEmpty(item.Unit) Then
                    quantityDisplay = $"{item.Quantity} {item.Unit}"
                End If

                content.AppendLine($"  {quantityDisplay} x ₱{effectivePrice:N2} = ₱{itemTotal:N2}")

                ' Show discount if applicable
                If item.DiscountPrice.HasValue Then
                    Dim savings As Decimal = (item.UnitPrice - item.DiscountPrice.Value) * item.Quantity
                    content.AppendLine($"  (Saved: ₱{savings:N2})")
                End If
                content.AppendLine()
            Next

            content.AppendLine($"Wholesale Subtotal: ₱{wholesaleTotal:N2}")
            content.AppendLine()
        End If

        ' Totals Section
        content.AppendLine("================================")
        If retailItems.Count > 0 AndAlso wholesaleItems.Count > 0 Then
            content.AppendLine($"Retail Total:     ₱{retailTotal:N2}")
            content.AppendLine($"Wholesale Total:  ₱{wholesaleTotal:N2}")
            content.AppendLine("--------------------------------")
        End If
        content.AppendLine($"TOTAL AMOUNT:     ₱{totalAmount:N2}")
        content.AppendLine($"Payment Method: {selectedPaymentMethod}")

        ' Add cash payment details
        If selectedPaymentMethod = "Cash" Then
            If amountPaid > 0 Then
                content.AppendLine($"Amount Paid:      ₱{amountPaid:N2}")
                If changeGiven > 0 Then
                    content.AppendLine($"Change:           ₱{changeGiven:N2}")
                End If
            End If
        End If

        ' Add payment details if GCash or Bank Transaction
        If selectedPaymentMethod = "GCash" OrElse selectedPaymentMethod = "Bank Transaction" Then
            content.AppendLine()
            content.AppendLine("--------------------------------")
            content.AppendLine("PAYMENT DETAILS")
            content.AppendLine("--------------------------------")
            content.AppendLine($"Payer Name: {payerName}")
            content.AppendLine($"Reference #: {referenceNumber}")
            If selectedPaymentMethod = "Bank Transaction" Then
                content.AppendLine($"Bank: {bankName}")
            End If
        End If

        content.AppendLine("================================")

        ' Footer
        content.AppendLine()
        content.AppendLine("================================")
        content.AppendLine("  Thank you for your purchase!")
        content.AppendLine("================================")
        content.AppendLine()
        content.AppendLine($"Items: {cartItems.Count}")
        content.AppendLine($"Receipt #: {Date.Now:yyyyMMddHHmmss}")
        content.AppendLine()

        Return content.ToString()
    End Function

    ''' Handle actual printing of receipt
    Private Sub printDocument_PrintPage(sender As Object, e As PrintPageEventArgs) Handles printDocument.PrintPage
        Try
            ' Set up fonts for receipt
            Dim headerFont As New Font("Courier New", 10, FontStyle.Bold)
            Dim normalFont As New Font("Courier New", 8, FontStyle.Regular)
            Dim boldFont As New Font("Courier New", 8, FontStyle.Bold)

            ' Set up brush
            Dim blackBrush As New SolidBrush(Color.Black)

            ' Starting position
            Dim leftMargin As Single = e.MarginBounds.Left
            Dim topMargin As Single = e.MarginBounds.Top
            Dim yPosition As Single = topMargin

            ' Line height
            Dim lineHeight As Single = normalFont.GetHeight(e.Graphics)

            ' Print each line
            For Each line As String In receiptLines
                ' Check if we need a new page (unlikely for receipts)
                If yPosition + lineHeight > e.MarginBounds.Bottom Then
                    e.HasMorePages = True
                    Return
                End If

                ' Choose font based on content
                Dim currentFont As Font = normalFont
                If line.Contains("INVENTORY SYSTEM") OrElse
          line.Contains("SALES RECEIPT") OrElse
 line.Contains("** RETAIL") OrElse
  line.Contains("** WHOLESALE") OrElse
    line.Contains("TOTAL AMOUNT") OrElse
          line.Contains("DELIVERY INFORMATION") OrElse
  line.Contains("PICKUP INFORMATION") Then
                    currentFont = headerFont
                ElseIf line.Contains("Subtotal") OrElse
     line.Contains("Total:") OrElse
            line.Contains("Payment Method") Then
                    currentFont = boldFont
                End If

                ' Draw the line
                e.Graphics.DrawString(line, currentFont, blackBrush, leftMargin, yPosition)
                yPosition += lineHeight
            Next

            ' No more pages
            e.HasMorePages = False
        Catch ex As Exception
            Debug.WriteLine($"Print page error: {ex.Message}")
            MessageBox.Show("Error during printing: " & ex.Message, "Print Error",
     MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

End Class