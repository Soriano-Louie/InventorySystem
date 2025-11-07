Imports Microsoft.Data.SqlClient

Public Class posForm
    Dim topPanel As New topControlCashier()
    Private WithEvents updateTimer As New Timer()

    ' Store cart items with product details
    Public Class CartItem
        Public Property ProductID As Integer
        Public Property ProductName As String
        Public Property Quantity As Integer
        Public Property UnitPrice As Decimal
        Public Property IsVATApplicable As Boolean
        Public Property DiscountPrice As Decimal?
        Public Property CategoryID As Integer
    End Class

    Private cartItems As New List(Of CartItem)()

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.Controls.Add(topPanel)
        topPanel.Dock = DockStyle.Top
        Me.MaximizeBox = False
        Me.FormBorderStyle = FormBorderStyle.None
        Me.BackColor = Color.FromArgb(224, 166, 109)

        TableLayoutPanel1.BackColor = Color.FromArgb(224, 166, 109)
        bottomPanel.BackColor = Color.FromArgb(230, 216, 177)
        featuresPanel.BackColor = Color.FromArgb(79, 51, 40)

        Button1.BackColor = Color.FromArgb(230, 216, 177)
        Button2.BackColor = Color.FromArgb(230, 216, 177)
        Button3.BackColor = Color.FromArgb(230, 216, 177)
        Button4.BackColor = Color.FromArgb(230, 216, 177)
        Button5.BackColor = Color.FromArgb(230, 216, 177)
        Button6.BackColor = Color.FromArgb(230, 216, 177)

        Button1.ForeColor = Color.FromArgb(79, 51, 40)
        Button2.ForeColor = Color.FromArgb(79, 51, 40)
        Button3.ForeColor = Color.FromArgb(79, 51, 40)
        Button4.ForeColor = Color.FromArgb(79, 51, 40)
        Button5.ForeColor = Color.FromArgb(79, 51, 40)
        Button6.ForeColor = Color.FromArgb(79, 51, 40)

        DataGridView1.BackgroundColor = Color.FromArgb(224, 166, 109)

        Label1.ForeColor = Color.FromArgb(79, 51, 40)
        Label2.ForeColor = Color.FromArgb(79, 51, 40)
        Label3.ForeColor = Color.FromArgb(79, 51, 40)
        Label4.ForeColor = Color.FromArgb(79, 51, 40)

        ' Configure DataGridView1
        ConfigureDataGridView()

        ' Set label colors
        timeLabel.ForeColor = Color.FromArgb(79, 51, 40)
        dateYearLabel.ForeColor = Color.FromArgb(79, 51, 40)

        ' Initialize and start the timer for updating time and date
        updateTimer.Interval = 1000 ' Update every 1 second (1000 milliseconds)
        updateTimer.Start()

        ' Update time and date immediately
        UpdateTimeAndDate()

        ' Initialize labels
        InitializeTotalLabels()

        ' Enable key preview to capture F1
        Me.KeyPreview = True

        ' Add Button1 click event handler
        AddHandler Button1.Click, AddressOf Button1_Click

        ' Add Button3 click event handler
        AddHandler Button3.Click, AddressOf Button3_Click

        ' Add Button2 click event handler for checkout
        AddHandler Button2.Click, AddressOf Button2_Click

        ' Add Button4 click event handler for daily transactions
        AddHandler Button4.Click, AddressOf Button4_Click
    End Sub

    Private Sub InitializeTotalLabels()
        ' Initialize all total labels to zero
        totalSalesLabel.Text = "₱0.00"
        totalDiscountLabel.Text = "₱0.00"
        VATLabel.Text = "₱0.00"
        vatableLabel.Text = "₱0.00"
    End Sub

    Private Sub UpdateTimeAndDate()
        ' Get current date and time
        Dim currentDateTime As DateTime = DateTime.Now

        ' Update time label (format: HH:mm:ss)
        timeLabel.Text = currentDateTime.ToString("hh:mm:ss tt")

        ' Update date and year label (format: Day, Month Date, Year - e.g., "Friday, January 10, 2025")
        dateYearLabel.Text = currentDateTime.ToString("dddd, MMMM dd, yyyy")
    End Sub

    Private Sub UpdateTimer_Tick(sender As Object, e As EventArgs) Handles updateTimer.Tick
        ' This event fires every second to update the time and date
        UpdateTimeAndDate()
    End Sub

    Private Sub ConfigureDataGridView()
        ' Clear any existing columns
        DataGridView1.Columns.Clear()

        ' Set header style
        DataGridView1.EnableHeadersVisualStyles = False
        DataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(79, 51, 40)
        DataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(230, 216, 177)
        DataGridView1.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        DataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        ' Set column header selection colors (when clicking on header)
        DataGridView1.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(79, 51, 40)
        DataGridView1.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.FromArgb(230, 216, 177)

        ' Set default cell colors (unselected)
        DataGridView1.DefaultCellStyle.BackColor = Color.White
        DataGridView1.DefaultCellStyle.ForeColor = Color.Black

        ' Set selection colors - dark brown background with light brown text
        DataGridView1.DefaultCellStyle.SelectionBackColor = Color.FromArgb(79, 51, 40) ' Dark brown
        DataGridView1.DefaultCellStyle.SelectionForeColor = Color.FromArgb(230, 216, 177) ' Light brown

        ' Set row header selection colors (if ever visible)
        DataGridView1.RowHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(79, 51, 40)
        DataGridView1.RowHeadersDefaultCellStyle.SelectionForeColor = Color.FromArgb(230, 216, 177)

        ' Add columns
        ' Column 1: #
        DataGridView1.Columns.Add("ColNumber", "#")
        DataGridView1.Columns("ColNumber").ReadOnly = True
        DataGridView1.Columns("ColNumber").FillWeight = 8

        ' Column 2: PRODUCT NAME
        DataGridView1.Columns.Add("ColProductName", "PRODUCT NAME")
        DataGridView1.Columns("ColProductName").ReadOnly = True
        DataGridView1.Columns("ColProductName").FillWeight = 30

        ' Column 3: QTY
        DataGridView1.Columns.Add("ColQty", "QTY")
        DataGridView1.Columns("ColQty").ReadOnly = True
        DataGridView1.Columns("ColQty").FillWeight = 12

        ' Column 4: PRICE
        DataGridView1.Columns.Add("ColPrice", "PRICE")
        DataGridView1.Columns("ColPrice").ReadOnly = True
        DataGridView1.Columns("ColPrice").DefaultCellStyle.Format = "₱#,##0.00"
        DataGridView1.Columns("ColPrice").FillWeight = 15

        ' Column 5: TOTAL
        DataGridView1.Columns.Add("ColTotal", "TOTAL")
        DataGridView1.Columns("ColTotal").ReadOnly = True
        DataGridView1.Columns("ColTotal").DefaultCellStyle.Format = "₱#,##0.00"
        DataGridView1.Columns("ColTotal").FillWeight = 18

        ' Column 6: ADD (Button)
        Dim btnAdd As New DataGridViewButtonColumn()
        btnAdd.Name = "ColAdd"
        btnAdd.HeaderText = "ADD"
        btnAdd.Text = "+"
        btnAdd.UseColumnTextForButtonValue = True
        btnAdd.FillWeight = 9
        DataGridView1.Columns.Add(btnAdd)

        ' Column 7: SUBTRACT (Button)
        Dim btnSubtract As New DataGridViewButtonColumn()
        btnSubtract.Name = "ColSubtract"
        btnSubtract.HeaderText = "SUBTRACT"
        btnSubtract.Text = "-"
        btnSubtract.UseColumnTextForButtonValue = True
        btnSubtract.FillWeight = 12
        DataGridView1.Columns.Add(btnSubtract)

        ' Column 8: DELETE (Button)
        Dim btnDelete As New DataGridViewButtonColumn()
        btnDelete.Name = "ColDelete"
        btnDelete.HeaderText = "DELETE"
        btnDelete.Text = "X"
        btnDelete.UseColumnTextForButtonValue = True
        btnDelete.FillWeight = 10
        DataGridView1.Columns.Add(btnDelete)

        ' Additional DataGridView settings
        DataGridView1.AllowUserToAddRows = False
        DataGridView1.AllowUserToDeleteRows = False
        DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridView1.MultiSelect = False
        DataGridView1.RowHeadersVisible = False
        DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        ' Handle button clicks
        AddHandler DataGridView1.CellContentClick, AddressOf DataGridView1_CellContentClick
    End Sub

    Protected Overrides Sub WndProc(ByRef m As Message)
        Const WM_SYSCOMMAND As Integer = &H112
        Const SC_RESTORE As Integer = &HF120
        Const SC_MOVE As Integer = &HF010

        If m.Msg = WM_SYSCOMMAND Then
            Dim command As Integer = (m.WParam.ToInt32() And &HFFF0)

            ' Block restore
            If command = SC_RESTORE Then
                Return
            End If

            ' Block moving
            If command = SC_MOVE Then
                Return
            End If
        End If

        MyBase.WndProc(m)
    End Sub

    ' ==================== PRODUCT SEARCH WINDOW ====================

    ''' <summary>
    ''' Opens the Product Search window
    ''' </summary>
    Private Sub Button1_Click(sender As Object, e As EventArgs)
        OpenProductSearchWindow()
    End Sub

    ''' <summary>
    ''' Opens the QR Scanner window
    ''' </summary>
    Private Sub Button2_Click(sender As Object, e As EventArgs)
        OpenCheckoutWindow()
    End Sub

    ''' <summary>
    ''' Clears the cart after user confirmation
    ''' </summary>
    Private Sub Button3_Click(sender As Object, e As EventArgs)
        ClearCartWithConfirmation()
    End Sub

    ''' <summary>
    ''' Opens the daily transactions window
    ''' </summary>
    Private Sub Button4_Click(sender As Object, e As EventArgs)
        OpenDailyTransactionsWindow()
    End Sub

    'logout function
    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        ' Confirm logout
        Dim result As DialogResult = MessageBox.Show("Are you sure you want to log out?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If result = DialogResult.Yes Then
            Application.Restart()
        End If
    End Sub

    ''' <summary>
    ''' Override to handle F1 key press
    ''' </summary>
    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean
        If keyData = Keys.F1 Then
            OpenProductSearchWindow()
            Return True
        ElseIf keyData = Keys.F2 Then
            OpenQRScannerWindow()
            Return True
        ElseIf keyData = Keys.F3 Then
            OpenCheckoutWindow()
            Return True
        ElseIf keyData = Keys.F4 Then
            ClearCartWithConfirmation()
            Return True
        ElseIf keyData = Keys.F5 Then
            OpenDailyTransactionsWindow()
            Return True
        End If
        Return MyBase.ProcessCmdKey(msg, keyData)
    End Function

    ''' <summary>
    ''' Opens the product search form
    ''' </summary>
    Private Sub OpenProductSearchWindow()
        Try
            Dim searchForm As New ProductSearchForm(Me)
            searchForm.ShowDialog(Me)
        Catch ex As Exception
            MessageBox.Show("Error opening product search: " & ex.Message, "Error",
          MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' Opens the QR scanner form
    ''' </summary>
    Private Sub OpenQRScannerWindow()
        Try
            Dim scannerForm As New QRScannerForm(Me)
            scannerForm.ShowDialog(Me)
        Catch ex As Exception
            MessageBox.Show("Error opening QR scanner: " & ex.Message, "Error",
            MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' Opens the checkout form
    ''' </summary>
    Private Sub OpenCheckoutWindow()
        Try
            ' Check if cart is empty
            If cartItems.Count = 0 Then
                MessageBox.Show("Cart is empty! Please add items before checkout.", "Empty Cart",
  MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' Calculate total from totalSalesLabel
            Dim totalText As String = totalSalesLabel.Text.Replace("₱", "").Replace(",", "").Trim()
            Dim totalAmount As Decimal
            If Not Decimal.TryParse(totalText, totalAmount) Then
                MessageBox.Show("Error calculating total amount.", "Error",
             MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            ' Open checkout form
            Dim checkoutForm As New CheckoutForm(Me, totalAmount)
            If checkoutForm.ShowDialog(Me) = DialogResult.OK Then
                ' Checkout was successful, cart is already cleared by checkout form
                MessageBox.Show("Transaction completed successfully!", "Success",
 MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            MessageBox.Show("Error opening checkout: " & ex.Message, "Error",
  MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' Opens the daily transactions form
    ''' </summary>
    Private Sub OpenDailyTransactionsWindow()
        Try
            Dim transactionsForm As New DailyTransactionsForm(Me)
            transactionsForm.ShowDialog(Me)
        Catch ex As Exception
            MessageBox.Show("Error opening daily transactions: " & ex.Message, "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' ==================== POS CALCULATION METHODS ====================

    Private Function GetConnectionString() As String
        Return SharedUtilities.GetConnectionString()
    End Function

    ''' <summary>
    ''' Adds or updates a product in the cart and refreshes calculations
    ''' </summary>
    Public Sub AddProductToCart(productID As Integer, productName As String, quantity As Integer,
    unitPrice As Decimal, categoryID As Integer, isVATApplicable As Boolean)
        ' Check if product already exists in cart
        Dim existingItem = cartItems.FirstOrDefault(Function(x) x.ProductID = productID)

        If existingItem IsNot Nothing Then
            ' Update quantity
            existingItem.Quantity += quantity
        Else
            ' Add new item
            Dim newItem As New CartItem With {
               .ProductID = productID,
               .ProductName = productName,
               .Quantity = quantity,
               .UnitPrice = unitPrice,
               .IsVATApplicable = isVATApplicable,
               .CategoryID = categoryID,
               .DiscountPrice = GetDiscountPrice(productID, quantity)
               }
            cartItems.Add(newItem)
        End If

        ' Refresh display
        RefreshDataGridView()
        CalculateAllTotals()
    End Sub

    ''' <summary>
    ''' Gets the discount price for a product based on quantity (from ProductDiscounts table)
    ''' </summary>
    Private Function GetDiscountPrice(productID As Integer, quantity As Integer) As Decimal?
        Try
            Using conn As New SqlConnection(GetConnectionString())
                conn.Open()
                Dim query As String = "SELECT DiscountPrice FROM ProductDiscounts
       WHERE ProductID = @ProductID
  AND @Quantity >= MinSacks
      AND (@Quantity <= MaxSacks OR MaxSacks IS NULL)
        ORDER BY MinSacks DESC"

                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@ProductID", productID)
                    cmd.Parameters.AddWithValue("@Quantity", quantity)

                    Dim result = cmd.ExecuteScalar()
                    If result IsNot Nothing AndAlso Not IsDBNull(result) Then
                        Return Convert.ToDecimal(result)
                    End If
                End Using
            End Using
        Catch ex As Exception
            Console.WriteLine("Error getting discount: " & ex.Message)
        End Try
        Return Nothing
    End Function

    ''' <summary>
    ''' Refreshes the DataGridView to show current cart items
    ''' </summary>
    Private Sub RefreshDataGridView()
        DataGridView1.Rows.Clear()
        Dim rowNum As Integer = 1

        For Each item In cartItems
            Dim effectivePrice As Decimal = If(item.DiscountPrice.HasValue, item.DiscountPrice.Value, item.UnitPrice)
            Dim total As Decimal = effectivePrice * item.Quantity

            DataGridView1.Rows.Add(
                     rowNum,
                item.ProductName,
               item.Quantity,
                           effectivePrice,
                        total
           )
            rowNum += 1
        Next
    End Sub

    ''' <summary>
    ''' Calculates and updates all total labels (totalSales, discount, VAT, vatable)
    ''' </summary>
    Private Sub CalculateAllTotals()
        ' Get VAT rate from database
        Dim vatRate As Decimal = SharedUtilities.GetCurrentVATRate() / 100 ' Convert percentage to decimal

        Dim subtotalWithoutVAT As Decimal = 0D
        Dim totalDiscountAmount As Decimal = 0D
        Dim totalVATAmount As Decimal = 0D
        Dim totalVatableAmount As Decimal = 0D

        For Each item In cartItems
            Dim effectivePrice As Decimal = If(item.DiscountPrice.HasValue, item.DiscountPrice.Value, item.UnitPrice)
            Dim itemTotal As Decimal = effectivePrice * item.Quantity

            ' Calculate discount per item
            If item.DiscountPrice.HasValue Then
                Dim discountPerItem As Decimal = (item.UnitPrice - item.DiscountPrice.Value) * item.Quantity
                totalDiscountAmount += discountPerItem
            End If

            ' Check if VAT applicable for this product
            If item.IsVATApplicable Then
                ' This item has VAT
                Dim itemSubtotal As Decimal = itemTotal / (1 + vatRate) ' Remove VAT from price to get base
                Dim itemVAT As Decimal = itemTotal - itemSubtotal

                totalVatableAmount += itemSubtotal
                totalVATAmount += itemVAT
                subtotalWithoutVAT += itemTotal ' Include full price in sales
            Else
                ' No VAT for this item
                subtotalWithoutVAT += itemTotal
            End If
        Next

        ' Update labels
        totalSalesLabel.Text = $"₱{subtotalWithoutVAT:N2}"
        totalDiscountLabel.Text = $"₱{totalDiscountAmount:N2}"
        VATLabel.Text = $"₱{totalVATAmount:N2}"
        vatableLabel.Text = $"₱{totalVatableAmount:N2}"
    End Sub

    ''' <summary>
    ''' Handles button clicks in DataGridView (Add, Subtract, Delete)
    ''' </summary>
    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex < 0 Then Return ' Ignore header clicks

        Dim colName As String = DataGridView1.Columns(e.ColumnIndex).Name
        Dim item = cartItems(e.RowIndex)

        Select Case colName
            Case "ColAdd"
                ' Increase quantity
                item.Quantity += 1
                item.DiscountPrice = GetDiscountPrice(item.ProductID, item.Quantity)
                RefreshDataGridView()
                CalculateAllTotals()

            Case "ColSubtract"
                ' Decrease quantity
                If item.Quantity > 1 Then
                    item.Quantity -= 1
                    item.DiscountPrice = GetDiscountPrice(item.ProductID, item.Quantity)
                    RefreshDataGridView()
                    CalculateAllTotals()
                End If

            Case "ColDelete"
                ' Remove item
                cartItems.RemoveAt(e.RowIndex)
                RefreshDataGridView()
                CalculateAllTotals()
        End Select
    End Sub

    ''' <summary>
    ''' Gets product details including VAT applicability from database
    ''' Call this when scanning/adding a product
    ''' </summary>
    Public Function GetProductDetails(productCode As String) As (success As Boolean, productID As Integer,
        productName As String, unitPrice As Decimal,
 categoryID As Integer, isVATApplicable As Boolean)
        Try
            Using conn As New SqlConnection(GetConnectionString())
                conn.Open()

                ' Check both wholesaleProducts and retailProducts
                Dim query As String = "
                SELECT ProductID, ProductName, RetailPrice, CategoryID,
                ISNULL(IsVATApplicable, 0) AS IsVATApplicable
                    FROM wholesaleProducts
                    WHERE SKU = @SKU
                    UNION
                    SELECT ProductID, ProductName, RetailPrice, CategoryID,
                ISNULL(IsVATApplicable, 0) AS IsVATApplicable
                FROM retailProducts
                WHERE SKU = @SKU"

                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@SKU", productCode)

                    Using reader = cmd.ExecuteReader()
                        If reader.Read() Then
                            Return (True,
                                reader.GetInt32(0),
                            reader.GetString(1),
                          reader.GetDecimal(2),
                               reader.GetInt32(3),
                          reader.GetBoolean(4))
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error retrieving product: " & ex.Message)
        End Try

        Return (False, 0, "", 0D, 0, False)
    End Function

    ''' <summary>
    ''' Example: Handle barcode scan or manual product entry
    ''' </summary>
    Public Sub ScanProduct(productCode As String, quantity As Integer)
        Dim productDetails = GetProductDetails(productCode)

        If productDetails.success Then
            AddProductToCart(productDetails.productID,
                productDetails.productName,
                    quantity,
              productDetails.unitPrice,
                      productDetails.categoryID,
         productDetails.isVATApplicable)
        Else
            MessageBox.Show("Product not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    ''' <summary>
    ''' Clears the cart and resets all totals
    ''' </summary>
    Public Sub ClearCart()
        cartItems.Clear()
        RefreshDataGridView()
        InitializeTotalLabels()
    End Sub

    ''' <summary>
    ''' Gets the current cart items (for checkout)
    ''' </summary>
    Public Function GetCartItems() As List(Of posForm.CartItem)
        Return cartItems
    End Function

    ''' <summary>
    ''' Shows confirmation dialog and clears cart if user confirms
    ''' </summary>
    Private Sub ClearCartWithConfirmation()
        ' Check if cart is empty
        If cartItems.Count = 0 Then
            MessageBox.Show("Cart is already empty.", "Clear Cart",
             MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        ' Show confirmation dialog
        Dim result As DialogResult = MessageBox.Show(
   "Are you sure you want to clear all items from the cart?",
            "Confirm Clear Cart",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question)

        ' If user confirms, clear the cart
        If result = DialogResult.Yes Then
            ClearCart()
            MessageBox.Show("Cart has been cleared successfully.", "Clear Cart",
            MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        OpenQRScannerWindow()
    End Sub

End Class