Imports System.IO
Imports Microsoft.Data.SqlClient
Imports QRCoder

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

        ' Set up the DateTimePicker with checkbox
        newDateText.ShowCheckBox = True
        newDateText.Checked = False
        newDateText.MinDate = DateTime.Now.AddDays(1)
    End Sub

    Protected Overrides Sub WndProc(ByRef m As Message)
        Const WM_SYSCOMMAND As Integer = &H112
        Const SC_MAXIMIZE As Integer = &HF030
        Const SC_RESTORE As Integer = &HF120

        If m.Msg = WM_SYSCOMMAND Then
            Dim command As Integer = (m.WParam.ToInt32() And &HFFF0)

            ' Block maximize
            If command = SC_MAXIMIZE Then
                Return
            End If

            ' Block restore (prevents double-click maximize)
            If command = SC_RESTORE Then
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
        Dim query As String = "SELECT ProductID, ProductName, SKU FROM wholesaleProducts WHERE IsArchived = 0 OR IsArchived IS NULL ORDER BY ProductName"
        Dim connString As String = GetConnectionString()

        Try
            Using conn As New SqlConnection(connString)
                Using da As New SqlDataAdapter(query, conn)
                    Dim dt As New DataTable()
                    da.Fill(dt)

                    ' Add a computed column for display that combines SKU and ProductName
                    If Not dt.Columns.Contains("DisplayText") Then
                        dt.Columns.Add("DisplayText", GetType(String))
                    End If

                    ' Populate the display column with format: [SKU] ProductName
                    For Each row As DataRow In dt.Rows
                        Dim sku As String = row("SKU").ToString()
                        Dim productName As String = row("ProductName").ToString()
                        row("DisplayText") = $"[{sku}] {productName}"
                    Next

                    With productDropDown
                        .DataSource = dt
                        .DisplayMember = "DisplayText"    ' Show [SKU] ProductName
                        .ValueMember = "ProductID"        ' Hidden value
                        .SelectedIndex = -1               ' No default selection
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
                           Optional newExpDate As Date? = Nothing,
                           Optional updateExpDate As Boolean = False)

        Dim query As String = "
        UPDATE wholesaleProducts
        SET
            ProductName    = COALESCE(@ProductName, ProductName),
            CategoryID     = COALESCE(@CategoryID, CategoryID),
            StockQuantity  = COALESCE(@Quantity, StockQuantity),
            Unit           = COALESCE(@Unit, Unit),
            Cost           = COALESCE(@Cost, Cost),
            RetailPrice    = COALESCE(@RetailPrice, RetailPrice),
            ReorderLevel   = COALESCE(@Reorder, ReorderLevel)" &
            If(updateExpDate, ",
            ExpirationDate = @ExpDate", "") & ",
            LastUpdated = GETDATE()
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

                ' Only add ExpDate parameter if we're updating it
                If updateExpDate Then
                    cmd.Parameters.AddWithValue("@ExpDate", If(newExpDate.HasValue, newExpDate.Value, DBNull.Value))
                End If

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

    ' New method to insert a product batch with different expiration date
    Private Sub InsertProductBatch(baseProductID As Integer, additionalQuantity As Integer, expirationDate As Date?, editedBy As Integer, editReason As String)
        ' First, get the details from the base product
        Dim connString As String = GetConnectionString()
        Dim sku As String = ""
        Dim productName As String = ""
        Dim categoryID As Integer = 0
        Dim unit As String = ""
        Dim cost As Decimal = 0
        Dim retailPrice As Decimal = 0
        Dim reorderLevel As Integer = 0
        Dim oldQuantity As Decimal = 0

        Using conn As New SqlConnection(connString)
            conn.Open()
            Dim selectQuery As String = "SELECT SKU, ProductName, CategoryID, Unit, Cost, RetailPrice, ReorderLevel, StockQuantity FROM wholesaleProducts WHERE ProductID = @ProductID"

            Using cmd As New SqlCommand(selectQuery, conn)
                cmd.Parameters.AddWithValue("@ProductID", baseProductID)

                Using reader As SqlDataReader = cmd.ExecuteReader()
                    If reader.Read() Then
                        ' Create unique SKU for the batch
                        sku = reader("SKU").ToString() & "-BATCH-" & DateTime.Now.ToString("yyyyMMddHHmmss")
                        productName = reader("ProductName").ToString()
                        categoryID = Convert.ToInt32(reader("CategoryID"))
                        unit = reader("Unit").ToString()
                        cost = Convert.ToDecimal(reader("Cost"))
                        retailPrice = Convert.ToDecimal(reader("RetailPrice"))
                        reorderLevel = Convert.ToInt32(reader("ReorderLevel"))
                        oldQuantity = Convert.ToDecimal(reader("StockQuantity"))
                    Else
                        MessageBox.Show("Product not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return
                    End If
                End Using
            End Using
        End Using

        ' Generate QR code for the new batch using the unique SKU
        Dim qrData As Byte()
        Using qrGen As New QRCodeGenerator()
            Using qrCodeData = qrGen.CreateQrCode(sku, QRCodeGenerator.ECCLevel.Q)
                Using qrCode As New QRCode(qrCodeData)
                    Using qrImage As Bitmap = qrCode.GetGraphic(20)
                        Using ms As New MemoryStream()
                            qrImage.Save(ms, Imaging.ImageFormat.Png)
                            qrData = ms.ToArray()
                        End Using
                    End Using
                End Using
            End Using
        End Using

        ' Now insert the new batch with the new QR code
        Using conn As New SqlConnection(connString)
            conn.Open()
            Dim insertQuery As String = "INSERT INTO wholesaleProducts
                               (SKU, ProductName, Unit, RetailPrice, Cost, StockQuantity, ReorderLevel, ExpirationDate, CategoryID, QRCodeImage, LastUpdated)
                               VALUES
                               (@SKU, @ProductName, @Unit, @RetailPrice, @Cost, @StockQuantity, @ReorderLevel, @ExpirationDate, @CategoryID, @QRCodeImage, GETDATE())"

            Using cmd As New SqlCommand(insertQuery, conn)
                cmd.Parameters.AddWithValue("@SKU", sku)
                cmd.Parameters.AddWithValue("@ProductName", productName)
                cmd.Parameters.AddWithValue("@Unit", unit)
                cmd.Parameters.AddWithValue("@RetailPrice", retailPrice)
                cmd.Parameters.AddWithValue("@Cost", cost)
                cmd.Parameters.AddWithValue("@StockQuantity", additionalQuantity)
                cmd.Parameters.AddWithValue("@ReorderLevel", reorderLevel)
                cmd.Parameters.AddWithValue("@CategoryID", categoryID)

                If expirationDate.HasValue Then
                    cmd.Parameters.AddWithValue("@ExpirationDate", expirationDate.Value)
                Else
                    cmd.Parameters.AddWithValue("@ExpirationDate", DBNull.Value)
                End If

                ' Add the newly generated QR code
                Dim qrParam As New SqlParameter("@QRCodeImage", SqlDbType.VarBinary)
                qrParam.Value = qrData
                cmd.Parameters.Add(qrParam)

                cmd.ExecuteNonQuery()
            End Using
        End Using

        ' Log the stock edit in wholesaleStockEditLogs
        InsertStockEditLog(baseProductID, oldQuantity, oldQuantity + additionalQuantity, unit, editedBy, editReason)

        ' Refresh UI
        loadProducts()
        If parentForm IsNot Nothing Then
            parentForm.LoadProducts()
        End If
    End Sub

    ' Method to log stock edits to wholesaleStockEditLogs table
    Private Sub InsertStockEditLog(productID As Integer, oldQty As Decimal, newQty As Decimal, unitType As String, editedBy As Integer, editReason As String)
        Dim connString As String = GetConnectionString()

        Using conn As New SqlConnection(connString)
            conn.Open()
            Dim logQuery As String = "INSERT INTO wholesaleStockEditLogs
                                     (wholesaleProductId, oldQuantity, newQuantity, unitType, editedBy, editReason, editDate)
                                     VALUES
                                     (@ProductID, @OldQty, @NewQty, @UnitType, @EditedBy, @EditReason, GETDATE())"

            Using cmd As New SqlCommand(logQuery, conn)
                cmd.Parameters.AddWithValue("@ProductID", productID)
                cmd.Parameters.AddWithValue("@OldQty", oldQty)
                cmd.Parameters.AddWithValue("@NewQty", newQty)
                cmd.Parameters.AddWithValue("@UnitType", If(String.IsNullOrWhiteSpace(unitType), DBNull.Value, unitType))
                cmd.Parameters.AddWithValue("@EditedBy", editedBy)
                cmd.Parameters.AddWithValue("@EditReason", If(String.IsNullOrWhiteSpace(editReason), DBNull.Value, editReason))

                cmd.ExecuteNonQuery()
            End Using
        End Using
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

            ' Reset checkbox if it has one
            If picker.ShowCheckBox Then
                picker.Checked = False
            End If

            ' Use today's date but respect MinDate/MaxDate
            Dim today As Date = DateTime.Today.AddDays(1)
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
        Dim newExpDate As Date? = Nothing
        Dim updateExpDate As Boolean = newDateText.Checked

        ' WARNING: Check if expiration date is being modified
        If updateExpDate Then
            newExpDate = newDateText.Value

            ' Get current expiration date from database
            Dim currentExpDate As Date? = GetCurrentExpirationDate(productID)

            ' Show warning if expiration date is being changed
            Dim warningMsg As String = ""
            If currentExpDate.HasValue Then
                ' Changing existing expiration date
                warningMsg = $"⚠️ WARNING: You are changing the expiration date" & vbCrLf & vbCrLf &
                            $"Current Expiration: {currentExpDate.Value:yyyy-MM-dd}" & vbCrLf &
                            $"New Expiration: {newExpDate.Value:yyyy-MM-dd}" & vbCrLf & vbCrLf &
                            "Changing the expiration date will affect:" & vbCrLf &
                            "  • Product visibility in sales" & vbCrLf &
                            "  • Inventory tracking" & vbCrLf &
                            "  • Expiration warnings" & vbCrLf & vbCrLf &
                            "Are you sure you want to proceed?"
            Else
                ' Adding new expiration date
                warningMsg = $"⚠️ WARNING: You are adding an expiration date" & vbCrLf & vbCrLf &
                            $"New Expiration: {newExpDate.Value:yyyy-MM-dd}" & vbCrLf & vbCrLf &
                            "This product previously had no expiration date." & vbCrLf &
                            "Adding an expiration date will:" & vbCrLf &
                            "  • Enable expiration tracking" & vbCrLf &
                            "  • Show expiration warnings when near expiry" & vbCrLf &
                            "  • Affect product visibility based on expiry status" & vbCrLf & vbCrLf &
                            "Are you sure you want to proceed?"
            End If

            Dim result = MessageBox.Show(warningMsg,
                                        "Confirm Expiration Date Change",
                                        MessageBoxButtons.YesNo,
                                        MessageBoxIcon.Warning,
                                        MessageBoxDefaultButton.Button2)

            If result = DialogResult.No Then
                MessageBox.Show("Expiration date update cancelled.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If
        End If

        ' Safe parsing with default fallback
        ' Quantity
        Dim newQuantity As Integer? = Nothing
        Dim isQuantityEdit As Boolean = False
        If Not String.IsNullOrWhiteSpace(newQuantityText.Text) Then
            Dim parsed As Integer
            If Integer.TryParse(newQuantityText.Text.Trim(), parsed) Then
                newQuantity = parsed
                isQuantityEdit = True
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

        ' Validation
        If newCost.HasValue AndAlso newCost < 0 Then
            MessageBox.Show("Cost cannot be negative.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If newRetailPrice.HasValue AndAlso newRetailPrice < 0 Then
            MessageBox.Show("Retail Price cannot be negative.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If newQuantity.HasValue AndAlso newQuantity < 0 Then
            MessageBox.Show("Stock Quantity cannot be negative.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' Check if at least one field has been updated
        Dim hasChanges As Boolean = Not String.IsNullOrWhiteSpace(newProductName) OrElse
                                    newCategory IsNot Nothing OrElse
                                    Not String.IsNullOrWhiteSpace(newUnit) OrElse
                                    newCost.HasValue OrElse
                                    newRetailPrice.HasValue OrElse
                                    newReorder.HasValue OrElse
                                    updateExpDate OrElse
                                    isQuantityEdit

        If Not hasChanges Then
            MessageBox.Show("No changes detected. Please fill in at least one field to update.",
                          "No Changes",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Information)
            Exit Sub
        End If

        ' 4. Handle quantity edit with batch tracking and logging
        If isQuantityEdit Then
            ' Get user ID
            Dim currentUserID As Integer = GetCurrentUserID()

            ' Ensure user is logged in
            If currentUserID = 0 Then
                Exit Sub
            End If

            ' Ask user if this is a new batch or updating existing batch
            Dim batchChoice = MessageBox.Show(
                "Stock Quantity Update" & vbCrLf & vbCrLf &
                $"You are modifying the stock quantity to {newQuantity.Value} units." & vbCrLf & vbCrLf &
                "Is this a NEW BATCH of the product?" & vbCrLf & vbCrLf &
                "• Click YES if this is a NEW BATCH (different expiration date, new shipment)" & vbCrLf &
                "• Click NO if you're CORRECTING the quantity of the SAME BATCH" & vbCrLf & vbCrLf &
                "Choose carefully as this affects inventory tracking.",
                "New Batch or Same Batch?",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button3)

            If batchChoice = DialogResult.Cancel Then
                MessageBox.Show("Operation cancelled.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            Dim isNewBatch As Boolean = (batchChoice = DialogResult.Yes)

            If isNewBatch Then
                ' NEW BATCH - Create separate inventory entry
                ' Prompt for edit reason
                Dim editReason As String = InputBox("Please provide a reason for adding this new batch:", "New Batch Reason", "New shipment received")

                If String.IsNullOrWhiteSpace(editReason) Then
                    MessageBox.Show("Reason is required for adding new batch.", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Exit Sub
                End If

                ' Check if product has expiration date in database
                Dim productHasExpirationDate As Boolean = CheckProductHasExpirationDate(productID)

                ' Handle expiration date based on product type
                If productHasExpirationDate Then
                    ' Product has expiration date, require user to specify one for the new batch
                    If Not updateExpDate Then
                        MessageBox.Show("This product requires an expiration date for the new batch." & vbCrLf &
                                       "Please check the expiration date checkbox and select a date.",
                                       "Expiration Date Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Exit Sub
                    End If
                Else
                    ' Product doesn't have expiration date, use Nothing
                    newExpDate = Nothing
                    updateExpDate = False
                End If

                ' Build confirmation message
                Dim confirmMsg As String
                If productHasExpirationDate AndAlso newExpDate.HasValue Then
                    confirmMsg = String.Format(
                        "You are adding {0} units as a NEW BATCH with expiration date {1}." & vbCrLf & vbCrLf &
                        "This will create a separate inventory entry for tracking purposes." & vbCrLf &
                        "Reason: {2}" & vbCrLf & vbCrLf &
                        "Do you want to continue?",
                        newQuantity.Value,
                        newExpDate.Value.ToShortDateString(),
                        editReason)
                Else
                    confirmMsg = String.Format(
                        "You are adding {0} units as a NEW BATCH without an expiration date." & vbCrLf & vbCrLf &
                        "This will create a separate inventory entry for tracking purposes." & vbCrLf &
                        "Reason: {1}" & vbCrLf & vbCrLf &
                        "Do you want to continue?",
                        newQuantity.Value,
                        editReason)
                End If

                If MessageBox.Show(confirmMsg, "Confirm New Batch Creation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
                    Exit Sub
                End If

                Try
                    ' Insert new batch instead of updating existing quantity
                    InsertProductBatch(productID, newQuantity.Value, newExpDate, currentUserID, editReason)

                    ' Update other fields (except quantity) if provided
                    UpdateProducts(productID, newProductName, newCategory, Nothing, newUnit, newCost, newRetailPrice, newReorder, Nothing, False)

                    MessageBox.Show("New product batch created successfully!" & vbCrLf &
                                   "The quantity has been added as a separate batch." & vbCrLf &
                                   "Stock edit has been logged.",
                                   "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Catch ex As Exception
                    MessageBox.Show("Error creating product batch: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            Else
                ' SAME BATCH - Update existing quantity
                Dim editReason As String = InputBox("Please provide a reason for this stock adjustment:", "Edit Reason", "Stock correction")

                If String.IsNullOrWhiteSpace(editReason) Then
                    MessageBox.Show("Edit reason is required for stock quantity changes.", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Exit Sub
                End If

                ' Get old quantity for logging
                Dim oldQuantity As Decimal = GetCurrentStockQuantity(productID)
                Dim currentUnit As String = GetCurrentUnit(productID)

                ' Confirm the update
                Dim confirmMsg As String = String.Format(
                    "You are updating the SAME BATCH quantity." & vbCrLf & vbCrLf &
                    "Current Quantity: {0} {2}" & vbCrLf &
                    "New Quantity: {1} {2}" & vbCrLf &
                    "Reason: {3}" & vbCrLf & vbCrLf &
                    "Do you want to continue?",
                    oldQuantity,
                    newQuantity.Value,
                    currentUnit,
                    editReason)

                If MessageBox.Show(confirmMsg, "Confirm Stock Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
                    Exit Sub
                End If

                Try
                    ' Update the existing product quantity
                    UpdateProducts(productID, newProductName, newCategory, newQuantity, newUnit, newCost, newRetailPrice, newReorder, Nothing, False)

                    ' Log the stock edit
                    InsertStockEditLog(productID, oldQuantity, newQuantity.Value, If(String.IsNullOrWhiteSpace(newUnit), currentUnit, newUnit), currentUserID, editReason)

                    MessageBox.Show("Product quantity updated successfully!" & vbCrLf &
                                   "Stock edit has been logged.",
                                   "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Catch ex As Exception
                    MessageBox.Show("Error updating product: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
        Else
            ' 5. Normal update without quantity change
            Try
                UpdateProducts(productID, newProductName, newCategory, Nothing, newUnit, newCost, newRetailPrice, newReorder, newExpDate, updateExpDate)
                MessageBox.Show("Product updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception
                MessageBox.Show("Error updating product: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If

        ' 6. Reset form controls
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
        newDateText.Checked = False
    End Sub

    ' Helper method to check if product has expiration date set in database
    Private Function CheckProductHasExpirationDate(productID As Integer) As Boolean
        Dim hasExpirationDate As Boolean = False
        Dim connString As String = GetConnectionString()

        Try
            Using conn As New SqlConnection(connString)
                conn.Open()
                Dim query As String = "SELECT ExpirationDate FROM wholesaleProducts WHERE ProductID = @ProductID"

                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@ProductID", productID)

                    Dim result = cmd.ExecuteScalar()

                    ' If ExpirationDate is not NULL, the product has an expiration date
                    If result IsNot Nothing AndAlso Not IsDBNull(result) Then
                        hasExpirationDate = True
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error checking product expiration date: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        Return hasExpirationDate
    End Function

    ' Helper method to get current expiration date from database (for comparison)
    Private Function GetCurrentExpirationDate(productID As Integer) As Date?
        Dim currentExpDate As Date? = Nothing
        Dim connString As String = GetConnectionString()

        Try
            Using conn As New SqlConnection(connString)
                conn.Open()
                Dim query As String = "SELECT ExpirationDate FROM wholesaleProducts WHERE ProductID = @ProductID"

                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@ProductID", productID)

                    Dim result = cmd.ExecuteScalar()

                    If result IsNot Nothing AndAlso Not IsDBNull(result) Then
                        currentExpDate = Convert.ToDateTime(result)
                    End If
                End Using
            End Using
        Catch ex As Exception
            Debug.WriteLine($"Error getting current expiration date: {ex.Message}")
        End Try

        Return currentExpDate
    End Function

    ' Helper method to get current logged-in user ID
    Private Function GetCurrentUserID() As Integer
        ' Check if user is logged in
        If GlobalUserSession.IsUserLoggedIn() Then
            Return GlobalUserSession.CurrentUserID
        Else
            MessageBox.Show("User session not found. Please log in again.", "Session Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return 0
        End If
    End Function

    Private Sub deleteButton_Click(sender As Object, e As EventArgs) Handles deleteButton.Click
        ' Ensure a product is selected
        If productDropDown.SelectedIndex = -1 Then
            MessageBox.Show("Please select a product to delete.", "No Product Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        ' Get ProductID and product name from dropdown
        Dim productID As Integer = Convert.ToInt32(productDropDown.SelectedValue)
        Dim productName As String = productDropDown.Text

        ' Get additional product details for the warning
        Dim productDetails As String = GetProductDetailsForDeletion(productID)

        ' Show detailed warning with product information
        Dim warningMessage As String = $"⚠️ WARNING: You are about to permanently delete this product!" & vbCrLf & vbCrLf &
          $"Product: {productName}" & vbCrLf &
  productDetails & vbCrLf &
          "This action CANNOT be undone!" & vbCrLf & vbCrLf &
          "⚠️ All associated data including:" & vbCrLf &
          "  • Stock quantities" & vbCrLf &
                "• Pricing information" & vbCrLf &
     "  • QR codes" & vbCrLf &
            "  • Product discounts" & vbCrLf &
  "will be permanently removed." & vbCrLf & vbCrLf &
          "Are you absolutely sure you want to DELETE this product?"

        ' Confirm before deleting with a more prominent warning
        Dim confirm = MessageBox.Show(warningMessage,
    "⚠️ CONFIRM PERMANENT DELETION",
 MessageBoxButtons.YesNo,
    MessageBoxIcon.Warning,
     MessageBoxDefaultButton.Button2) ' Default to "No"

        If confirm = DialogResult.Yes Then
            ' Show a second confirmation to prevent accidental deletion
            Dim finalConfirm = MessageBox.Show($"FINAL CONFIRMATION:{vbCrLf}{vbCrLf}" &
              $"You are about to permanently delete '{productName}'." & vbCrLf & vbCrLf &
     "Click YES to proceed with deletion." & vbCrLf &
       "Click NO to cancel.",
        "⚠️ FINAL DELETION CONFIRMATION",
         MessageBoxButtons.YesNo,
           MessageBoxIcon.Exclamation,
    MessageBoxDefaultButton.Button2)

            If finalConfirm = DialogResult.Yes Then
                DeleteProduct(productID, productName)
            Else
                MessageBox.Show("Deletion cancelled.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Else
            MessageBox.Show("Deletion cancelled.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    ''' <summary>
    ''' Gets product details for the deletion warning message
    ''' </summary>
    Private Function GetProductDetailsForDeletion(productID As Integer) As String
        Dim details As String = ""

        Try
            Using conn As New SqlConnection(GetConnectionString())
                conn.Open()
                Dim query As String = "SELECT StockQuantity, Unit, RetailPrice, Cost, CategoryID FROM wholesaleProducts WHERE ProductID = @ProductID"

                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@ProductID", productID)

                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            Dim stockQty As Object = reader("StockQuantity")
                            Dim unit As Object = reader("Unit")
                            Dim retailPrice As Object = reader("RetailPrice")
                            Dim cost As Object = reader("Cost")

                            details = $"Stock: {If(IsDBNull(stockQty), "N/A", stockQty.ToString())} {If(IsDBNull(unit), "", unit.ToString())}" & vbCrLf &
            $"Retail Price: ₱{If(IsDBNull(retailPrice), "0.00", Convert.ToDecimal(retailPrice).ToString("N2"))}" & vbCrLf &
    $"Cost: ₱{If(IsDBNull(cost), "0.00", Convert.ToDecimal(cost).ToString("N2"))}"
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            details = "(Unable to load product details)"
        End Try

        Return details
    End Function

    ''' <summary>
    ''' Check if a product has sales records
    ''' </summary>
    Private Function ProductHasSalesRecords(productID As Integer) As Boolean
        Try
            Using conn As New SqlConnection(GetConnectionString())
                conn.Open()
                Dim query As String = "SELECT COUNT(*) FROM SalesReport WHERE ProductID = @ProductID"
                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@ProductID", productID)
                    Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())
                    Return count > 0
                End Using
            End Using
        Catch ex As Exception
            Debug.WriteLine($"Error checking sales records: {ex.Message}")
            Return False
        End Try
    End Function

    Private Sub DeleteProduct(productID As Integer, productName As String)
        ' Check if product has sales records first
        If ProductHasSalesRecords(productID) Then
            ' Offer to archive instead of delete
            Dim result = MessageBox.Show(
                "⚠️ Cannot Delete - Sales Records Exist" & vbCrLf & vbCrLf &
                $"Product '{productName}' has sales records and cannot be permanently deleted." & vbCrLf & vbCrLf &
                "Would you like to ARCHIVE this product instead?" & vbCrLf & vbCrLf &
                "Archived products:" & vbCrLf &
                "  ✓ Will be hidden from active inventory" & vbCrLf &
                "  ✓ Can be restored later if needed" & vbCrLf &
                "  ✓ Sales history will be preserved" & vbCrLf & vbCrLf &
                "Click YES to archive, NO to cancel.",
                "Archive Product?",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1)

            If result = DialogResult.Yes Then
                ArchiveProduct(productID, productName)
            End If
            Return
        End If

        ' If no sales records, proceed with normal deletion
        Try
            Using conn As New SqlConnection(GetConnectionString())
                conn.Open()

                ' Start a transaction to ensure all deletions succeed or all fail
                Using transaction As SqlTransaction = conn.BeginTransaction()
                    Try
                        ' Step 1: Delete related product discounts
                        Dim deleteDiscountsQuery As String = "DELETE FROM ProductDiscounts WHERE ProductID = @ProductID"
                        Using cmd As New SqlCommand(deleteDiscountsQuery, conn, transaction)
                            cmd.Parameters.AddWithValue("@ProductID", productID)
                            Dim discountsDeleted As Integer = cmd.ExecuteNonQuery()
                            Debug.WriteLine($"Deleted {discountsDeleted} discount records for ProductID {productID}")
                        End Using

                        ' Step 2: Delete the product itself
                        Dim deleteProductQuery As String = "DELETE FROM wholesaleProducts WHERE ProductID = @ProductID"
                        Using cmd As New SqlCommand(deleteProductQuery, conn, transaction)
                            cmd.Parameters.AddWithValue("@ProductID", productID)
                            Dim rowsAffected As Integer = cmd.ExecuteNonQuery()

                            If rowsAffected > 0 Then
                                ' Commit the transaction if everything succeeded
                                transaction.Commit()

                                MessageBox.Show($"Product '{productName}' has been successfully deleted." & vbCrLf +
                                              "All associated data has been removed from the system.",
                                              "✓ Deletion Successful",
                                              MessageBoxButtons.OK,
                                              MessageBoxIcon.Information)

                                ' Reset form controls after successful deletion
                                productDropDown.SelectedIndex = -1
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
                                newDateText.Checked = False

                                ' Reload the products list
                                loadProducts()
                                If parentForm IsNot Nothing Then
                                    parentForm.LoadProducts()
                                End If
                            Else
                                ' Rollback if product not found
                                transaction.Rollback()
                                MessageBox.Show("No product found with that ID.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                            End If
                        End Using
                    Catch ex As Exception
                        ' Rollback transaction on error
                        transaction.Rollback()
                        Throw ' Re-throw to outer catch block
                    End Try
                End Using
            End Using
        Catch ex As Exception
            ' Check for specific constraint violations
            If ex.Message.Contains("FK_") OrElse ex.Message.Contains("REFERENCE constraint") Then
                ' Find which table is causing the issue
                Dim tableName As String = "related records"
                If ex.Message.Contains("SalesReport") Then
                    tableName = "sales records (SalesReport)"
                ElseIf ex.Message.Contains("RetailSalesReport") Then
                    tableName = "retail sales records"
                ElseIf ex.Message.Contains("ProductDiscounts") Then
                    tableName = "product discounts"
                End If

                MessageBox.Show($"Cannot delete product '{productName}'!" & vbCrLf & vbCrLf &
                              $"This product has {tableName} that reference it." & vbCrLf & vbCrLf &
                              "The system should have offered to archive this product." & vbCrLf &
                              "Please contact system administrator if this problem persists." & vbCrLf & vbCrLf &
                              $"Technical details: {ex.Message}",
                              "❌ Cannot Delete - Related Data Exists",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error)
            Else
                MessageBox.Show($"Error deleting product:{vbCrLf}{vbCrLf}" &
                              $"Error: {ex.Message}" & vbCrLf & vbCrLf &
                              "Please contact system administrator if this problem persists.",
                              "Database Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error)
            End If
        End Try
    End Sub

    ''' <summary>
    ''' Archive a product instead of deleting it - Archives ALL batches with the same ProductName
    ''' </summary>
    Private Sub ArchiveProduct(productID As Integer, productName As String)
        ' Get archive reason from user
        Dim reason As String = InputBox("Please provide a reason for archiving this product:",
                                       "Archive Reason",
                                       "Product no longer available")

        If String.IsNullOrWhiteSpace(reason) Then
            MessageBox.Show("Archive reason is required.", "Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            Dim currentUserID As Integer = GetCurrentUserID()
            If currentUserID = 0 Then Return

            Using conn As New SqlConnection(GetConnectionString())
                conn.Open()
                ' Archive ALL batches with the same ProductName (not just one ProductID)
                Dim query As String = "UPDATE wholesaleProducts
                                      SET IsArchived = 1,
                                          ArchivedDate = GETDATE(),
                                          ArchivedBy = @ArchivedBy,
                                          ArchiveReason = @ArchiveReason
                                      WHERE ProductName = @ProductName"

                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@ProductName", productName)
                    cmd.Parameters.AddWithValue("@ArchivedBy", currentUserID)
                    cmd.Parameters.AddWithValue("@ArchiveReason", reason)

                    Dim rowsAffected As Integer = cmd.ExecuteNonQuery()

                    If rowsAffected > 0 Then
                        MessageBox.Show($"✓ Product '{productName}' has been archived successfully!" & vbCrLf & vbCrLf &
                                      $"Archived {rowsAffected} batch(es)" & vbCrLf &
                                      "Reason: " & reason & vbCrLf & vbCrLf &
                                      "All batches of this product are now hidden from active inventory." & vbCrLf &
                                      "You can restore them later from the Archived Products view.",
                                      "Archive Successful",
                                      MessageBoxButtons.OK,
                                      MessageBoxIcon.Information)

                        ' Reset form and reload
                        productDropDown.SelectedIndex = -1
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
                        newDateText.Checked = False

                        loadProducts()
                        If parentForm IsNot Nothing Then
                            parentForm.LoadProducts()
                        End If
                    Else
                        MessageBox.Show("Product not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show($"Error archiving product: {ex.Message}",
                          "Database Error",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' Get current stock quantity for a product
    ''' </summary>
    Private Function GetCurrentStockQuantity(productID As Integer) As Decimal
        Try
            Using conn As New SqlConnection(GetConnectionString())
                conn.Open()
                Dim query As String = "SELECT StockQuantity FROM wholesaleProducts WHERE ProductID = @ProductID"
                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@ProductID", productID)
                    Dim result = cmd.ExecuteScalar()
                    If result IsNot Nothing AndAlso Not IsDBNull(result) Then
                        Return Convert.ToDecimal(result)
                    End If
                    Return 0
                End Using
            End Using
        Catch ex As Exception
            Debug.WriteLine($"Error getting stock quantity: {ex.Message}")
            Return 0
        End Try
    End Function

    ''' <summary>
    ''' Get current unit for a product
    ''' </summary>
    Private Function GetCurrentUnit(productID As Integer) As String
        Try
            Using conn As New SqlConnection(GetConnectionString())
                conn.Open()
                Dim query As String = "SELECT Unit FROM wholesaleProducts WHERE ProductID = @ProductID"
                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@ProductID", productID)
                    Dim result = cmd.ExecuteScalar()
                    If result IsNot Nothing AndAlso Not IsDBNull(result) Then
                        Return result.ToString()
                    End If
                    Return ""
                End Using
            End Using
        Catch ex As Exception
            Debug.WriteLine($"Error getting unit: {ex.Message}")
            Return ""
        End Try
    End Function

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