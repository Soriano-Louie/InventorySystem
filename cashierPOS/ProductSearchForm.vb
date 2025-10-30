Imports Microsoft.Data.SqlClient

Public Class ProductSearchForm
    Private parentPOSForm As posForm
    Private dt As New DataTable()
    Private dv As New DataView()
    Private bs As New BindingSource()
    Private placeholders As New Dictionary(Of TextBox, String)

    Public Sub New(parent As posForm)
        InitializeComponent()
        Me.parentPOSForm = parent
        Me.MaximizeBox = False
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.StartPosition = FormStartPosition.CenterParent
        Me.BackColor = Color.FromArgb(230, 216, 177)
        Me.Size = New Size(900, 600)
        Me.Text = "Search Products"

        ' Initialize DataTable, DataView, and BindingSource
        dt = New DataTable()
        dv = New DataView(dt)
        bs = New BindingSource()
        bs.DataSource = dv
    End Sub

    Private Sub ProductSearchForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetupUI()
        LoadProducts()
    End Sub

    Private Sub SetupUI()
        ' Create main panel
        Dim mainPanel As New Panel()
        mainPanel.Dock = DockStyle.Fill
        mainPanel.BackColor = Color.FromArgb(230, 216, 177)
        mainPanel.Padding = New Padding(20)
        Me.Controls.Add(mainPanel)

        ' Create title label
        Dim titleLabel As New Label()
        titleLabel.Text = "Product Search"
        titleLabel.Font = New Font("Segoe UI", 16, FontStyle.Bold)
        titleLabel.ForeColor = Color.FromArgb(79, 51, 40)
        titleLabel.AutoSize = True
        titleLabel.Location = New Point(20, 10)
        mainPanel.Controls.Add(titleLabel)

        ' Create search textbox
        Dim searchTextBox As New TextBox()
        searchTextBox.Name = "searchTextBox"
        searchTextBox.Size = New Size(600, 30)
        searchTextBox.Location = New Point(20, 50)
        searchTextBox.Font = New Font("Segoe UI", 12)
        searchTextBox.BackColor = Color.White
        searchTextBox.ForeColor = Color.Black
        SetPlaceholder(searchTextBox, "Search by Product Name or SKU...")
        AddHandler searchTextBox.TextChanged, AddressOf SearchTextBox_TextChanged
        mainPanel.Controls.Add(searchTextBox)

        ' Create search button
        Dim searchButton As New Button()
        searchButton.Text = "Search"
        searchButton.Size = New Size(100, 30)
        searchButton.Location = New Point(630, 50)
        searchButton.BackColor = Color.FromArgb(147, 53, 53)
        searchButton.ForeColor = Color.FromArgb(230, 216, 177)
        searchButton.FlatStyle = FlatStyle.Flat
        searchButton.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        mainPanel.Controls.Add(searchButton)

        ' Create clear button
        Dim clearButton As New Button()
        clearButton.Text = "Clear"
        clearButton.Size = New Size(100, 30)
        clearButton.Location = New Point(740, 50)
        clearButton.BackColor = Color.FromArgb(147, 53, 53)
        clearButton.ForeColor = Color.FromArgb(230, 216, 177)
        clearButton.FlatStyle = FlatStyle.Flat
        clearButton.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        AddHandler clearButton.Click, AddressOf ClearButton_Click
        mainPanel.Controls.Add(clearButton)

        ' Create DataGridView
        Dim dgv As New DataGridView()
        dgv.Name = "productsDataGridView"
        dgv.Location = New Point(20, 100)
        dgv.Size = New Size(840, 380)
        dgv.BackgroundColor = Color.White
        dgv.GridColor = Color.FromArgb(79, 51, 40)
        dgv.EnableHeadersVisualStyles = False
        dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(79, 51, 40)
        dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(230, 216, 177)
        dgv.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        ' Set column header selection colors (when clicking on header)
        dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(79, 51, 40)
        dgv.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.FromArgb(230, 216, 177)

        ' Set default cell colors (unselected)
        dgv.DefaultCellStyle.BackColor = Color.White
        dgv.DefaultCellStyle.ForeColor = Color.Black

        ' Set selection colors - dark brown background with light brown text
        dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(79, 51, 40) ' Dark brown
        dgv.DefaultCellStyle.SelectionForeColor = Color.FromArgb(230, 216, 177) ' Light brown

        ' Set row header selection colors
        dgv.RowHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(79, 51, 40)
        dgv.RowHeadersDefaultCellStyle.SelectionForeColor = Color.FromArgb(230, 216, 177)

        dgv.ReadOnly = True
        dgv.AllowUserToAddRows = False
        dgv.AllowUserToDeleteRows = False
        dgv.RowHeadersVisible = False
        dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgv.MultiSelect = False
        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgv.DataSource = bs
        AddHandler dgv.CellDoubleClick, AddressOf ProductsDataGridView_CellDoubleClick
        mainPanel.Controls.Add(dgv)

        ' Create instructions label
        Dim instructionsLabel As New Label()
        instructionsLabel.Text = "Double-click on a product to add it to cart, or select and click Add to Cart"
        instructionsLabel.Font = New Font("Segoe UI", 9, FontStyle.Italic)
        instructionsLabel.ForeColor = Color.FromArgb(79, 51, 40)
        instructionsLabel.AutoSize = True
        instructionsLabel.Location = New Point(20, 490)
        mainPanel.Controls.Add(instructionsLabel)

        ' Create Add to Cart button
        Dim addToCartButton As New Button()
        addToCartButton.Name = "addToCartButton"
        addToCartButton.Text = "Add to Cart"
        addToCartButton.Size = New Size(150, 40)
        addToCartButton.Location = New Point(600, 510)
        addToCartButton.BackColor = Color.FromArgb(147, 53, 53)
        addToCartButton.ForeColor = Color.FromArgb(230, 216, 177)
        addToCartButton.FlatStyle = FlatStyle.Flat
        addToCartButton.Font = New Font("Segoe UI", 12, FontStyle.Bold)
        AddHandler addToCartButton.Click, AddressOf AddToCartButton_Click
        mainPanel.Controls.Add(addToCartButton)

        ' Create Close button
        Dim closeButton As New Button()
        closeButton.Text = "Close"
        closeButton.Size = New Size(100, 40)
        closeButton.Location = New Point(760, 510)
        closeButton.BackColor = Color.FromArgb(102, 66, 52)
        closeButton.ForeColor = Color.FromArgb(230, 216, 177)
        closeButton.FlatStyle = FlatStyle.Flat
        closeButton.Font = New Font("Segoe UI", 12, FontStyle.Bold)
        AddHandler closeButton.Click, Sub() Me.Close()
        mainPanel.Controls.Add(closeButton)
    End Sub

    Private Function GetConnectionString() As String
        Return SharedUtilities.GetConnectionString()
    End Function

    Private Sub LoadProducts()
        Try
            Using conn As New SqlConnection(GetConnectionString())
                conn.Open()

                ' Query both wholesale and retail products
                Dim query As String = "
      SELECT 
  'W' + CAST(ProductID AS VARCHAR) AS ProductCode,
           SKU,
        ProductName,
     CategoryID,
    Unit,
  RetailPrice,
         StockQuantity,
          ISNULL(IsVATApplicable, 0) AS IsVATApplicable,
         'Wholesale' AS ProductType
         FROM wholesaleProducts
       UNION ALL
       SELECT 
         'R' + CAST(ProductID AS VARCHAR) AS ProductCode,
      SKU,
           ProductName,
            CategoryID,
          Unit,
     RetailPrice,
 StockQuantity,
   ISNULL(IsVATApplicable, 0) AS IsVATApplicable,
  'Retail' AS ProductType
        FROM retailProducts
   ORDER BY ProductName"

                Using cmd As New SqlCommand(query, conn)
                    Using da As New SqlDataAdapter(cmd)
                        dt.Clear()
                        da.Fill(dt)
                    End Using
                End Using
            End Using

            ' Update DataGridView columns
            Dim dgv As DataGridView = Me.Controls.Find("productsDataGridView", True).FirstOrDefault()
            If dgv IsNot Nothing Then
                dgv.DataSource = bs

                With dgv
                    If .Columns.Contains("ProductCode") Then .Columns("ProductCode").Visible = False
                    If .Columns.Contains("CategoryID") Then .Columns("CategoryID").Visible = False
                    If .Columns.Contains("IsVATApplicable") Then .Columns("IsVATApplicable").Visible = False

                    If .Columns.Contains("SKU") Then
                        .Columns("SKU").HeaderText = "SKU"
                        .Columns("SKU").FillWeight = 20
                    End If

                    If .Columns.Contains("ProductName") Then
                        .Columns("ProductName").HeaderText = "Product Name"
                        .Columns("ProductName").FillWeight = 40
                    End If

                    If .Columns.Contains("Unit") Then
                        .Columns("Unit").HeaderText = "Unit"
                        .Columns("Unit").FillWeight = 15
                    End If

                    If .Columns.Contains("RetailPrice") Then
                        .Columns("RetailPrice").HeaderText = "Price"
                        .Columns("RetailPrice").DefaultCellStyle.Format = "₱#,##0.00"
                        .Columns("RetailPrice").FillWeight = 20
                    End If

                    If .Columns.Contains("StockQuantity") Then
                        .Columns("StockQuantity").HeaderText = "Stock"
                        .Columns("StockQuantity").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                        .Columns("StockQuantity").FillWeight = 15
                    End If

                    If .Columns.Contains("ProductType") Then
                        .Columns("ProductType").HeaderText = "Type"
                        .Columns("ProductType").FillWeight = 15
                    End If
                End With
            End If

        Catch ex As Exception
            MessageBox.Show("Error loading products: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub SetPlaceholder(tb As TextBox, text As String)
        placeholders(tb) = text
        tb.Text = text
        tb.ForeColor = Color.Gray

        AddHandler tb.GotFocus, AddressOf RemovePlaceholder
        AddHandler tb.LostFocus, AddressOf RestorePlaceholder
    End Sub

    Private Sub RemovePlaceholder(sender As Object, e As EventArgs)
        Dim tb As TextBox = DirectCast(sender, TextBox)
        If tb.Text = placeholders(tb) Then
            tb.Text = ""
            tb.ForeColor = Color.Black
        End If
    End Sub

    Private Sub RestorePlaceholder(sender As Object, e As EventArgs)
        Dim tb As TextBox = DirectCast(sender, TextBox)
        If String.IsNullOrWhiteSpace(tb.Text) Then
            tb.Text = placeholders(tb)
            tb.ForeColor = Color.Gray
        End If
    End Sub

    Private Sub SearchTextBox_TextChanged(sender As Object, e As EventArgs)
        Dim searchTextBox As TextBox = DirectCast(sender, TextBox)
        Dim placeholder = ""

        If placeholders.ContainsKey(searchTextBox) Then
            placeholder = placeholders(searchTextBox)
        End If

        If String.IsNullOrWhiteSpace(searchTextBox.Text) OrElse searchTextBox.Text = placeholder Then
            bs.Filter = ""
        Else
            bs.Filter = String.Format("ProductName LIKE '%{0}%' OR SKU LIKE '%{0}%'",
                 searchTextBox.Text.Replace("'", "''"))
        End If
    End Sub

    Private Sub ClearButton_Click(sender As Object, e As EventArgs)
        Dim searchTextBox As TextBox = Me.Controls.Find("searchTextBox", True).FirstOrDefault()
        If searchTextBox IsNot Nothing Then
            searchTextBox.Clear()
            If placeholders.ContainsKey(searchTextBox) Then
                RestorePlaceholder(searchTextBox, Nothing)
            End If
        End If
        bs.Filter = ""
    End Sub

    Private Sub ProductsDataGridView_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex >= 0 Then
            AddSelectedProductToCart()
        End If
    End Sub

    Private Sub AddToCartButton_Click(sender As Object, e As EventArgs)
        AddSelectedProductToCart()
    End Sub

    Private Sub AddSelectedProductToCart()
    Dim dgv As DataGridView = Me.Controls.Find("productsDataGridView", True).FirstOrDefault()
    If dgv Is Nothing OrElse dgv.SelectedRows.Count = 0 Then
        MessageBox.Show("Please select a product to add to cart.", "No Selection",
MessageBoxButtons.OK, MessageBoxIcon.Information)
        Return
    End If

    Try
        Dim selectedRow = dgv.SelectedRows(0)

        ' Get product details
        Dim productCode As String = selectedRow.Cells("ProductCode").Value.ToString()
        Dim productName As String = selectedRow.Cells("ProductName").Value.ToString()
        Dim unitPrice As Decimal = Convert.ToDecimal(selectedRow.Cells("RetailPrice").Value)
        Dim stockQuantity As Integer = Convert.ToInt32(selectedRow.Cells("StockQuantity").Value)
        Dim categoryID As Integer = Convert.ToInt32(selectedRow.Cells("CategoryID").Value)
        Dim isVATApplicable As Boolean = Convert.ToBoolean(selectedRow.Cells("IsVATApplicable").Value)

        ' Check stock availability
        If stockQuantity <= 0 Then
            MessageBox.Show("This product is out of stock!", "Out of Stock",
                     MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Ask for quantity
        Dim quantityInput As String = InputBox($"Enter quantity to add (Available: {stockQuantity}):",
      "Add to Cart", "1")

        If String.IsNullOrWhiteSpace(quantityInput) Then
            Return ' User cancelled
        End If

        Dim quantity As Integer
        If Not Integer.TryParse(quantityInput, quantity) OrElse quantity <= 0 Then
            MessageBox.Show("Please enter a valid quantity.", "Invalid Input",
    MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If quantity > stockQuantity Then
            MessageBox.Show($"Only {stockQuantity} units available in stock!", "Insufficient Stock",
               MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Extract ProductID from ProductCode (remove 'W' or 'R' prefix)
        Dim productID As Integer = Convert.ToInt32(productCode.Substring(1))

        ' Add to parent POS form's cart
        parentPOSForm.AddProductToCart(productID, productName, quantity, unitPrice, categoryID, isVATApplicable)

        MessageBox.Show($"Added {quantity} x {productName} to cart!", "Success",
  MessageBoxButtons.OK, MessageBoxIcon.Information)

        ' Optionally close the form after adding
        ' Me.Close()

    Catch ex As Exception
        MessageBox.Show("Error adding product to cart: " & ex.Message, "Error",
 MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Try
End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean
        ' Allow ESC to close the form
        If keyData = Keys.Escape Then
            Me.Close()
            Return True
        End If
        Return MyBase.ProcessCmdKey(msg, keyData)
    End Function
End Class
