Imports Microsoft.Data.SqlClient

Public Class editItemForm
    Private parentForm As InventoryForm
    Public Sub New(parent As InventoryForm)

        ' This call is required by the designer.
        InitializeComponent()
        Me.parentForm = parent
        Me.MaximizeBox = False
        Me.BackColor = Color.FromArgb(230, 216, 177)
        ' Add any initialization after the InitializeComponent() call.
        Label1.ForeColor = Color.FromArgb(79, 51, 40)

        Label1.BackColor = Color.FromArgb(224, 166, 109)


        updateButton.BackColor = Color.FromArgb(224, 166, 109)
        cancelButton.BackColor = Color.FromArgb(224, 166, 109)
        updateButton.ForeColor = Color.FromArgb(79, 51, 40)
        cancelButton.ForeColor = Color.FromArgb(79, 51, 40)
        editDiscountButton.BackColor = Color.FromArgb(224, 166, 109)
        editDiscountButton.ForeColor = Color.FromArgb(79, 51, 40)
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

    Private Function GetConnectionString() As String
        Return "Server=DESKTOP-3AKTMEV;Database=inventorySystem;User Id=sa;Password=24@Hakaaii07;TrustServerCertificate=True;"
    End Function

    Private Sub LoadCategories()
        Dim query As String = "SELECT CategoryID, CategoryName FROM Categories ORDER BY CategoryName"
        Dim connString As String = GetConnectionString()

        Try
            Using conn As New SqlConnection(connString)
                Using da As New SqlDataAdapter(query, conn)
                    Dim dt As New DataTable()
                    da.Fill(dt)

                    With newCategoryDropdown
                        .DataSource = dt
                        .DisplayMember = "CategoryName"   ' visible to user
                        .ValueMember = "CategoryID"       ' hidden value
                        .SelectedIndex = -1               ' optional: no default selection
                        .DropDownStyle = ComboBoxStyle.DropDown
                        .AutoCompleteMode = AutoCompleteMode.SuggestAppend
                        .AutoCompleteSource = AutoCompleteSource.ListItems
                    End With
                End Using
            End Using

        Catch ex As Exception
            MessageBox.Show("Error loading categories: " & ex.Message)
        End Try
    End Sub

    Private Sub loadProducts()
        Dim query As String = "SELECT ProductID, ProductName FROM Products ORDER BY ProductName"
        Dim connString As String = GetConnectionString()

        Try
            Using conn As New SqlConnection(connString)
                Using da As New SqlDataAdapter(query, conn)
                    Dim dt As New DataTable()
                    da.Fill(dt)

                    With productDropDown
                        .DataSource = dt
                        .DisplayMember = "ProductName"   ' visible to user
                        .ValueMember = "ProductID"       ' hidden value
                        .SelectedIndex = -1               ' optional: no default selection
                        .DropDownStyle = ComboBoxStyle.DropDown
                        .AutoCompleteMode = AutoCompleteMode.SuggestAppend
                        .AutoCompleteSource = AutoCompleteSource.ListItems
                    End With
                End Using
            End Using

        Catch ex As Exception
            MessageBox.Show("Error loading products: " & ex.Message)
        End Try
    End Sub

    Private Sub UpdateProducts(productID As Integer,
                           Optional newProductName As String = Nothing,
                           Optional newCategory As String = Nothing,
                           Optional newQuantity As Integer? = Nothing,
                           Optional newUnit As String = Nothing,
                           Optional newCost As Decimal? = Nothing,
                           Optional newRetailPrice As Decimal? = Nothing,
                           Optional newReorder As Integer? = Nothing,
                           Optional newExpDate As Date? = Nothing)

        'Dim updates As New List(Of String)
        'Dim parameters As New List(Of SqlParameter)

        '' Build query dynamically only with non-null params
        'If Not String.IsNullOrWhiteSpace(newProductName) Then
        '    updates.Add("ProductName = @ProductName")
        '    parameters.Add(New SqlParameter("@ProductName", newProductName))
        'End If

        'If newCategory IsNot Nothing Then
        '    updates.Add("CategoryID = @CategoryID")
        '    parameters.Add(New SqlParameter("@CategoryID", newCategory))
        'End If

        'If newQuantity.HasValue Then
        '    updates.Add("StockQuantity = @Quantity")
        '    parameters.Add(New SqlParameter("@Quantity", newQuantity.Value))
        'End If

        'If newUnit IsNot Nothing Then
        '    updates.Add("Unit = @Unit")
        '    parameters.Add(New SqlParameter("@Unit", newUnit))
        'End If

        'If newCost.HasValue Then
        '    updates.Add("Cost = @Cost")
        '    parameters.Add(New SqlParameter("@Cost", newCost.Value))
        'End If

        'If newRetailPrice.HasValue Then
        '    updates.Add("RetailPrice = @RetailPrice")
        '    parameters.Add(New SqlParameter("@RetailPrice", newRetailPrice.Value))
        'End If

        'If newReorder.HasValue Then
        '    updates.Add("ReorderLevel = @Reorder")
        '    parameters.Add(New SqlParameter("@Reorder", newReorder.Value))
        'End If

        'If newExpDate.HasValue Then
        '    updates.Add("ExpirationDate = @ExpDate")
        '    parameters.Add(New SqlParameter("@ExpDate", newExpDate.Value))
        'End If

        '' Exit if nothing to update
        'If updates.Count = 0 Then
        '    MessageBox.Show("No fields provided to update.")
        '    Return
        'End If

        Dim query As String = "
        UPDATE Products
        SET 
            ProductName    = COALESCE(@ProductName, ProductName),
            CategoryID     = COALESCE(@CategoryID, CategoryID),
            StockQuantity  = COALESCE(@Quantity, StockQuantity),
            Unit           = COALESCE(@Unit, Unit),
            Cost           = COALESCE(@Cost, Cost),
            RetailPrice    = COALESCE(@RetailPrice, RetailPrice),
            ReorderLevel   = COALESCE(@Reorder, ReorderLevel),
            ExpirationDate = COALESCE(@ExpDate, ExpirationDate)
        WHERE ProductID = @ProductID;
    "

        Using conn As New SqlConnection(GetConnectionString())
            Using cmd As New SqlCommand(query, conn)

                ' Add parameters with DBNull fallback
                cmd.Parameters.AddWithValue("@ProductName", If(String.IsNullOrWhiteSpace(newProductName), DBNull.Value, newProductName))
                cmd.Parameters.AddWithValue("@CategoryID", If(newCategory Is Nothing, DBNull.Value, newCategory))
                cmd.Parameters.AddWithValue("@Quantity", If(newQuantity.HasValue, newQuantity.Value, DBNull.Value))
                cmd.Parameters.AddWithValue("@Unit", If(String.IsNullOrWhiteSpace(newUnit), DBNull.Value, newUnit))
                cmd.Parameters.AddWithValue("@Cost", If(newCost.HasValue, newCost.Value, DBNull.Value))
                cmd.Parameters.AddWithValue("@RetailPrice", If(newRetailPrice.HasValue, newRetailPrice.Value, DBNull.Value))
                cmd.Parameters.AddWithValue("@Reorder", If(newReorder.HasValue, newReorder.Value, DBNull.Value))
                cmd.Parameters.AddWithValue("@ExpDate", If(newExpDate.HasValue, newExpDate.Value, DBNull.Value))

                ' WHERE condition
                cmd.Parameters.AddWithValue("@ProductID", productID)

                conn.Open()
                cmd.ExecuteNonQuery()
            End Using
        End Using

        ' Refresh UI
        loadProducts()
        If parentForm IsNot Nothing Then
            parentForm.LoadProducts()
        End If
    End Sub


    Private Sub editItemForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        loadProducts()
        LoadCategories()
    End Sub

    Private Sub ResetControl(ctrl As Control)
        If TypeOf ctrl Is TextBox Then
            DirectCast(ctrl, TextBox).Clear()
        ElseIf TypeOf ctrl Is ComboBox Then
            DirectCast(ctrl, ComboBox).SelectedIndex = -1
        ElseIf TypeOf ctrl Is DateTimePicker Then
            Dim picker As DateTimePicker = DirectCast(ctrl, DateTimePicker)

            ' Use today's date but respect MinDate/MaxDate
            Dim today As Date = DateTime.Today
            If today < picker.MinDate Then
                picker.Value = picker.MinDate
            ElseIf today > picker.MaxDate Then
                picker.Value = picker.MaxDate
            Else
                picker.Value = today
            End If
        End If
    End Sub

    Private Sub cancelButton_Click(sender As Object, e As EventArgs) Handles cancelButton.Click
        Close()
    End Sub

    Private Sub updateButton_Click(sender As Object, e As EventArgs) Handles updateButton.Click
        ' 1. Check if a product is selected
        If productDropDown.SelectedIndex = -1 Then
            MessageBox.Show("Please select a product to update.", "No Product Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        ' 2. Get ProductID from ComboBox
        Dim productID As Integer = Convert.ToInt32(productDropDown.SelectedValue)

        ' 3. Gather updated values from form controls
        Dim newProductName As String = newProductText.Text.Trim()
        Dim newCategory As Object = If(newCategoryDropdown.SelectedValue Is Nothing, Nothing, newCategoryDropdown.SelectedValue)
        Dim newUnit As String = newUnitText.Text.Trim()
        Dim newExpDate As Date = newDateText.Value

        ' Safe parsing with default fallback
        ' Quantity
        Dim newQuantity As Integer? = Nothing
        If Not String.IsNullOrWhiteSpace(newQuantityText.Text) Then
            Dim parsed As Integer
            If Integer.TryParse(newQuantityText.Text.Trim(), parsed) Then
                newQuantity = parsed
            Else
                MessageBox.Show("Invalid quantity value.")
                Return
            End If
        End If

        ' Retail Price
        Dim newRetailPrice As Decimal? = Nothing
        If Not String.IsNullOrWhiteSpace(newRetailText.Text) Then
            Dim parsed As Decimal
            If Decimal.TryParse(newRetailText.Text.Trim(), parsed) Then
                newRetailPrice = parsed
            Else
                MessageBox.Show("Invalid retail price value.")
                Return
            End If
        End If

        ' Cost
        Dim newCost As Decimal? = Nothing
        If Not String.IsNullOrWhiteSpace(newCostText.Text) Then
            Dim parsed As Decimal
            If Decimal.TryParse(newCostText.Text.Trim(), parsed) Then
                newCost = parsed
            Else
                MessageBox.Show("Invalid cost value.")
                Return
            End If
        End If

        ' Reorder Level
        Dim newReorder As Integer? = Nothing
        If Not String.IsNullOrWhiteSpace(newReorderText.Text) Then
            Dim parsed As Integer
            If Integer.TryParse(newReorderText.Text.Trim(), parsed) Then
                newReorder = parsed
            Else
                MessageBox.Show("Invalid reorder level value.")
                Return
            End If
        End If


        ' (Optional) validate required fields
        'If String.IsNullOrWhiteSpace(newProductName) Then
        '    MessageBox.Show("Product name cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    Exit Sub
        'End If

        If newCost < 0 Then
            MessageBox.Show("Cost cannot be negative.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If newRetailPrice < 0 Then
            MessageBox.Show("Retail Price cannot be negative.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If newQuantity < 0 Then
            MessageBox.Show("Stock Quantity cannot be negative.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' 4. Call update method
        Try
            UpdateProducts(productID, newProductName, newCategory, newQuantity, newUnit, newCost, newRetailPrice, newReorder, newExpDate)
            MessageBox.Show("Product updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ' 5. Reset form controls
            ResetControl(newProductText)
            ResetControl(newCategoryDropdown)
            ResetControl(newQuantityText)
            ResetControl(newUnitText)
            ResetControl(newCostText)
            ResetControl(newRetailText)
            ResetControl(newReorderText)
            For Each ctrl As Control In Panel8.Controls
                ResetControl(ctrl)
            Next
        Catch ex As Exception
            MessageBox.Show("Error updating product: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub deleteButton_Click(sender As Object, e As EventArgs) Handles deleteButton.Click
        ' Ensure a product is selected
        If productDropDown.SelectedIndex = -1 Then
            MessageBox.Show("Please select a product to delete.", "No Product Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        ' Get ProductID from dropdown
        Dim productID As Integer = Convert.ToInt32(productDropDown.SelectedValue)

        ' Confirm before deleting
        Dim confirm = MessageBox.Show("Are you sure you want to delete this product?",
                                      "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If confirm = DialogResult.Yes Then
            DeleteProduct(productID)
        End If
    End Sub

    Private Sub DeleteProduct(productID As Integer)
        Dim query As String = "DELETE FROM Products WHERE ProductID = @ProductID"

        Using conn As New SqlConnection(GetConnectionString())
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@ProductID", productID)

                conn.Open()
                Dim rowsAffected As Integer = cmd.ExecuteNonQuery()

                If rowsAffected > 0 Then
                    MessageBox.Show("Product deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    loadProducts()
                    If parentForm IsNot Nothing Then
                        parentForm.LoadProducts()
                    End If
                Else
                    MessageBox.Show("No product found with that ID.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                End If
            End Using
        End Using
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles editDiscountButton.Click
        If productDropDown.SelectedIndex = -1 Then
            MessageBox.Show("Please select a product to discount.", "No Product Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        ' Get ProductID from dropdown
        Dim productID As Integer = Convert.ToInt32(productDropDown.SelectedValue)
        Dim productName As String = productDropDown.Text

        Dim popup As New discountForm(productID, productName)
        popup.ShowDialog()

    End Sub
End Class