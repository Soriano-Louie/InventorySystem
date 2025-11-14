Imports System.Drawing.Drawing2D
Imports InventorySystem.sidePanelControl
Imports InventorySystem.topPanelControl
Imports Microsoft.Data.SqlClient

Public Class deliveryLogsForm
    Dim topPanel As New topPanelControl()
    Friend WithEvents sidePanel As sidePanelControl
    Dim colorUnclicked As Color = Color.FromArgb(191, 181, 147)
    Dim colorClicked As Color = Color.FromArgb(102, 66, 52)
    Dim dt As New DataTable()
    Dim dv As New DataView()
    Dim bs As New BindingSource()

    ' Class to store delivery information - NOW PUBLIC so DeliveryDetailsForm can use it
    Public Class DeliveryInfo
        Public Property SaleID As Integer
        Public Property ProductName As String
        Public Property DeliveryAddress As String
        Public Property Latitude As Double
        Public Property Longitude As Double
        Public Property DeliveryStatus As String
        Public Property SaleDate As Date
        Public Property TotalAmount As Decimal
        Public Property QuantitySold As Integer
        Public Property PaymentMethod As String ' Added for details form
    End Class

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        sidePanel = New sidePanelControl()
        sidePanel.Dock = DockStyle.Left
        topPanel.Dock = DockStyle.Top
        Me.Controls.Add(topPanel)
        Me.Controls.Add(sidePanel)
        Me.MaximizeBox = False
        Me.FormBorderStyle = FormBorderStyle.None
        Me.BackColor = Color.FromArgb(224, 166, 109)
        tableDataGridView.BackgroundColor = Color.FromArgb(230, 216, 177)
        tableDataGridView.GridColor = Color.FromArgb(79, 51, 40)
        Label1.ForeColor = Color.FromArgb(79, 51, 40)

        tableDataGridView.ReadOnly = True
        tableDataGridView.AllowUserToAddRows = False
        tableDataGridView.AllowUserToDeleteRows = False
        tableDataGridView.RowHeadersVisible = False

        ' Enable full row selection for better click experience
        tableDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        tableDataGridView.MultiSelect = False

        resetButton.BackColor = Color.FromArgb(147, 53, 53)
        resetButton.ForeColor = Color.FromArgb(230, 216, 177)
        filterButton.BackColor = Color.FromArgb(147, 53, 53)
        filterButton.ForeColor = Color.FromArgb(230, 216, 177)

        ' Enable keyboard shortcuts
        Me.KeyPreview = True
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

    ''' Handle keyboard shortcuts for delivery logs form
    ''' Enter = Filter deliveries by date range
    Private Sub deliveryLogsForm_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        Select Case e.KeyCode
            Case Keys.Enter
                ' Press Enter to filter deliveries
                If filterButton.Enabled Then
                    filterButton.PerformClick()
                    e.Handled = True
                    e.SuppressKeyPress = True
                End If
            Case Keys.F5
                ' Press F5 to reset
                If resetButton.Enabled Then
                    resetButton.PerformClick()
                    e.Handled = True
                    e.SuppressKeyPress = True
                End If
        End Select
    End Sub

    Private Function ShowSingleForm(Of T As {Form, New})() As T
        ' Hide all forms except the one to show
        Dim formToShow As Form = Nothing
        For Each frm As Form In Application.OpenForms.Cast(Of Form).ToList()
            If TypeOf frm Is T Then
                formToShow = frm
            ElseIf frm IsNot Me Then
                frm.Hide()
            End If
        Next

        If formToShow Is Nothing Then
            formToShow = New T()
            AddHandler formToShow.FormClosed, AddressOf ChildFormClosed
        End If

        formToShow.Show()
        formToShow.BringToFront()

        ' Hide this form if switching to another
        If formToShow IsNot Me Then
            Me.Hide()
        End If

        Return DirectCast(formToShow, T)
    End Function

    Private Sub ChildFormClosed(sender As Object, e As FormClosedEventArgs)
        ' No need to call HighlightButton here
    End Sub

    'Private Sub deliveryForm_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
    '    Application.Exit()
    'End Sub

    Private Sub HighlightButton(buttonName As String)
        ' Reset all buttons
        For Each ctrl As Control In sidePanel.Controls
            If TypeOf ctrl Is Button Then
                ctrl.BackColor = colorClicked
                ctrl.ForeColor = colorUnclicked
            End If
        Next

        ' Highlight the selected one
        Dim btn As Button = sidePanel.Controls.OfType(Of Button)().FirstOrDefault(Function(b) b.Name = buttonName)
        If btn IsNot Nothing Then
            btn.BackColor = colorUnclicked
            btn.ForeColor = Color.FromArgb(79, 51, 40)
        End If
    End Sub

    Private Sub SidePanel_ButtonClicked(sender As Object, btnName As String) Handles sidePanel.ButtonClicked
        Select Case btnName
            Case "Button1"
                Dim form = ShowSingleForm(Of wholesaleDashboard)()
                form.LoadDashboardData()
            Case "Button2"
                Dim form = ShowSingleForm(Of InventoryForm)()
                form.LoadProducts()
            Case "Button3"
                Dim form = ShowSingleForm(Of categoriesForm)()
                form.loadCategories()
            Case "Button4"
                ShowSingleForm(Of deliveryLogsForm)()
            Case "Button5"
                Dim form = ShowSingleForm(Of wholeSaleStockEditLogs)()
                form.loadStockEditLogs()
            Case "Button10"
                Dim form = ShowSingleForm(Of salesReport)()
                form.loadSalesReport()
            Case "Button6"
                Dim form = ShowSingleForm(Of loginRecordsForm)()
                form.LoadLoginHistory()
            Case "Button7"
                Dim form = ShowSingleForm(Of userManagementForm)()
                form.LoadUsers()
            Case "Button11"
                SetVATRate()
        End Select
    End Sub

    Private Sub deliveryForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        HighlightButton("Button4")

        ' Set default date range (today only)
        DateTimePickerFrom.Value = Date.Today
        DateTimePickerTo.Value = Date.Today
        DateTimePickerFrom.Format = DateTimePickerFormat.Short
        DateTimePickerTo.Format = DateTimePickerFormat.Short

        SetupDeliveryGrid()
        LoadTodaysDeliveries()
    End Sub

    Private Sub SetupDeliveryGrid()
        ' Configure DataGridView for delivery tracking
        tableDataGridView.Columns.Clear()

        ' Set header style
        tableDataGridView.EnableHeadersVisualStyles = False
        tableDataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(79, 51, 40)
        tableDataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(230, 216, 177)
        tableDataGridView.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        tableDataGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        tableDataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(79, 51, 40)
        tableDataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.FromArgb(230, 216, 177)

        ' Set default cell colors
        tableDataGridView.DefaultCellStyle.BackColor = Color.White
        tableDataGridView.DefaultCellStyle.ForeColor = Color.Black
        tableDataGridView.DefaultCellStyle.SelectionBackColor = Color.FromArgb(79, 51, 40)
        tableDataGridView.DefaultCellStyle.SelectionForeColor = Color.FromArgb(230, 216, 177)

        ' Add columns for delivery tracking
        tableDataGridView.Columns.Add("ColSaleID", "Sale #")
        tableDataGridView.Columns.Add("ColTime", "Time")
        tableDataGridView.Columns.Add("ColProduct", "Product")
        tableDataGridView.Columns.Add("ColQuantity", "Qty")
        tableDataGridView.Columns.Add("ColAmount", "Amount")
        tableDataGridView.Columns.Add("ColAddress", "Delivery Address")
        tableDataGridView.Columns.Add("ColStatus", "Status")

        ' Add status update button column
        Dim btnUpdateStatus As New DataGridViewButtonColumn()
        btnUpdateStatus.Name = "ColUpdateStatus"
        btnUpdateStatus.HeaderText = "Update"
        btnUpdateStatus.Text = "Change Status"
        btnUpdateStatus.UseColumnTextForButtonValue = True
        tableDataGridView.Columns.Add(btnUpdateStatus)

        ' Add view details button column
        Dim btnViewDetails As New DataGridViewButtonColumn()
        btnViewDetails.Name = "ColViewDetails"
        btnViewDetails.HeaderText = "Details"
        btnViewDetails.Text = "View Details"
        btnViewDetails.UseColumnTextForButtonValue = True
        tableDataGridView.Columns.Add(btnViewDetails)

        ' Set column widths
        tableDataGridView.Columns("ColSaleID").Width = 70
        tableDataGridView.Columns("ColTime").Width = 100
        tableDataGridView.Columns("ColProduct").Width = 180
        tableDataGridView.Columns("ColQuantity").Width = 60
        tableDataGridView.Columns("ColAmount").Width = 100
        tableDataGridView.Columns("ColAddress").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        tableDataGridView.Columns("ColStatus").Width = 100
        tableDataGridView.Columns("ColUpdateStatus").Width = 120
        tableDataGridView.Columns("ColViewDetails").Width = 120

        ' Handle button clicks and row clicks
        AddHandler tableDataGridView.CellContentClick, AddressOf DeliveryGrid_CellContentClick
        AddHandler tableDataGridView.CellFormatting, AddressOf DeliveryGrid_CellFormatting
        AddHandler tableDataGridView.CellClick, AddressOf DeliveryGrid_CellClick
        AddHandler tableDataGridView.CellDoubleClick, AddressOf DeliveryGrid_CellDoubleClick
    End Sub

    Private Sub LoadTodaysDeliveries()
        tableDataGridView.Rows.Clear()

        Try
            Using conn As New SqlConnection(GetConnectionString())
                conn.Open()
                Dim query As String = "
     SELECT sr.SaleID, p.ProductName, sr.DeliveryAddress,
    sr.DeliveryLatitude, sr.DeliveryLongitude, sr.DeliveryStatus,
         sr.SaleDate, sr.TotalAmount, sr.QuantitySold, sr.PaymentMethod
     FROM SalesReport sr
  INNER JOIN wholesaleProducts p ON sr.ProductID = p.ProductID
                    WHERE sr.IsDelivery = 1
      AND CAST(sr.SaleDate AS DATE) = CAST(GETDATE() AS DATE)
    AND sr.DeliveryStatus IS NOT NULL
      ORDER BY sr.SaleDate DESC"

                Using cmd As New SqlCommand(query, conn)
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            Dim rowIndex As Integer = tableDataGridView.Rows.Add()
                            Dim row As DataGridViewRow = tableDataGridView.Rows(rowIndex)

                            row.Cells("ColSaleID").Value = reader.GetInt32(0)
                            row.Cells("ColTime").Value = reader.GetDateTime(6).ToString("hh:mm tt")
                            row.Cells("ColProduct").Value = reader.GetString(1)
                            row.Cells("ColQuantity").Value = reader.GetInt32(8)
                            row.Cells("ColAmount").Value = reader.GetDecimal(7).ToString("C2")
                            row.Cells("ColAddress").Value = reader.GetString(2)
                            row.Cells("ColStatus").Value = reader.GetString(5)

                            ' Store delivery info in tag for updates and details view
                            row.Tag = New DeliveryInfo() With {
.SaleID = reader.GetInt32(0),
       .ProductName = reader.GetString(1),
        .DeliveryAddress = reader.GetString(2),
               .Latitude = reader.GetDouble(3),
          .Longitude = reader.GetDouble(4),
     .DeliveryStatus = reader.GetString(5),
           .SaleDate = reader.GetDateTime(6),
     .TotalAmount = reader.GetDecimal(7),
        .QuantitySold = reader.GetInt32(8),
     .PaymentMethod = If(reader.IsDBNull(9), "N/A", reader.GetString(9))
             }
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading deliveries: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        ' Update title
        Label1.Text = $"Today's Deliveries ({tableDataGridView.Rows.Count})"
    End Sub

    Private Sub DeliveryGrid_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs)
        If e.ColumnIndex = tableDataGridView.Columns("ColStatus").Index AndAlso e.RowIndex >= 0 Then
            Dim status As String = Convert.ToString(e.Value)

            ' Color-code status cells
            Select Case status
                Case "Pending"
                    e.CellStyle.BackColor = Color.FromArgb(255, 200, 200) ' Light red
                    e.CellStyle.ForeColor = Color.DarkRed
                Case "In Transit"
                    e.CellStyle.BackColor = Color.FromArgb(255, 220, 180) ' Light orange
                    e.CellStyle.ForeColor = Color.DarkOrange
                Case "Delivered"
                    e.CellStyle.BackColor = Color.FromArgb(200, 255, 200) ' Light green
                    e.CellStyle.ForeColor = Color.DarkGreen
                Case "Cancelled"
                    e.CellStyle.BackColor = Color.LightGray
                    e.CellStyle.ForeColor = Color.Gray
            End Select
        End If
    End Sub

    Private Sub DeliveryGrid_CellContentClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex < 0 Then Return

        If e.ColumnIndex = tableDataGridView.Columns("ColUpdateStatus").Index Then
            UpdateDeliveryStatus(e.RowIndex)
        ElseIf e.ColumnIndex = tableDataGridView.Columns("ColViewDetails").Index Then
            ShowDeliveryDetails(e.RowIndex)
        End If
    End Sub

    ''' <summary>
    ''' Handle single click on any cell (except buttons) to show details
    ''' </summary>
    Private Sub DeliveryGrid_CellClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex < 0 Then Return

        ' Don't trigger on button columns
        If e.ColumnIndex = tableDataGridView.Columns("ColUpdateStatus").Index OrElse
      e.ColumnIndex = tableDataGridView.Columns("ColViewDetails").Index Then
            Return
        End If

        ' Show details when clicking on any other cell
        ShowDeliveryDetails(e.RowIndex)
    End Sub

    ''' Handle double click on row to show details
    Private Sub DeliveryGrid_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex < 0 Then Return
        ShowDeliveryDetails(e.RowIndex)
    End Sub

    ''' Show detailed delivery information with map in a popup form
    Private Sub ShowDeliveryDetails(rowIndex As Integer)
        Try
            Dim row As DataGridViewRow = tableDataGridView.Rows(rowIndex)
            Dim deliveryInfo As DeliveryInfo = DirectCast(row.Tag, DeliveryInfo)

            If deliveryInfo Is Nothing Then
                MessageBox.Show("No delivery information found for this row.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' Create and show the delivery details form with map
            Dim detailsForm As New DeliveryDetailsForm(deliveryInfo)
            detailsForm.ShowDialog()
        Catch ex As Exception
            MessageBox.Show("Error showing delivery details: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Debug.WriteLine($"Error showing delivery details: {ex.ToString()}")
        End Try
    End Sub

    Private Sub UpdateDeliveryStatus(rowIndex As Integer)
        Dim row As DataGridViewRow = tableDataGridView.Rows(rowIndex)
        Dim deliveryInfo As DeliveryInfo = DirectCast(row.Tag, DeliveryInfo)

        If deliveryInfo Is Nothing Then Return

        ' Show status selection dialog
        Dim statusForm As New Form()
        statusForm.Text = $"Update Delivery Status - Sale #{deliveryInfo.SaleID}"
        statusForm.Size = New Size(400, 300)
        statusForm.StartPosition = FormStartPosition.CenterParent
        statusForm.FormBorderStyle = FormBorderStyle.FixedDialog
        statusForm.MaximizeBox = False
        statusForm.MinimizeBox = False
        statusForm.BackColor = Color.FromArgb(230, 216, 177)

        Dim currentLabel As New Label()
        currentLabel.Text = $"Current Status: {deliveryInfo.DeliveryStatus}"
        currentLabel.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        currentLabel.ForeColor = Color.FromArgb(79, 51, 40)
        currentLabel.Location = New Point(20, 20)
        currentLabel.AutoSize = True
        statusForm.Controls.Add(currentLabel)

        ' Add product info label
        Dim productLabel As New Label()
        productLabel.Text = $"Product: {deliveryInfo.ProductName}{vbCrLf}Quantity: {deliveryInfo.QuantitySold}"
        productLabel.Font = New Font("Segoe UI", 9)
        productLabel.ForeColor = Color.FromArgb(79, 51, 40)
        productLabel.Location = New Point(20, 45)
        productLabel.AutoSize = True
        statusForm.Controls.Add(productLabel)

        Dim selectLabel As New Label()
        selectLabel.Text = "Select New Status:"
        selectLabel.Font = New Font("Segoe UI", 10)
        selectLabel.ForeColor = Color.FromArgb(79, 51, 40)
        selectLabel.Location = New Point(20, 80)
        selectLabel.AutoSize = True
        statusForm.Controls.Add(selectLabel)

        Dim statusCombo As New ComboBox()
        statusCombo.Items.AddRange(New String() {"Pending", "In Transit", "Delivered", "Cancelled"})
        statusCombo.SelectedItem = deliveryInfo.DeliveryStatus
        statusCombo.Location = New Point(20, 105)
        statusCombo.Size = New Size(340, 30)
        statusCombo.Font = New Font("Segoe UI", 11)
        statusCombo.DropDownStyle = ComboBoxStyle.DropDownList
        statusForm.Controls.Add(statusCombo)

        ' Add warning label for cancellation
        Dim warningLabel As New Label()
        warningLabel.Text = "⚠ Cancelling will restore stock to inventory"
        warningLabel.Font = New Font("Segoe UI", 8, FontStyle.Italic)
        warningLabel.ForeColor = Color.DarkRed
        warningLabel.Location = New Point(20, 140)
        warningLabel.Size = New Size(340, 30)
        warningLabel.Visible = False
        statusForm.Controls.Add(warningLabel)

        ' Show warning when Cancelled is selected
        AddHandler statusCombo.SelectedIndexChanged, Sub()
                                                         warningLabel.Visible = (statusCombo.SelectedItem?.ToString() = "Cancelled")
                                                     End Sub

        Dim updateBtn As New Button()
        updateBtn.Text = "Update Status"
        updateBtn.Location = New Point(180, 200)
        updateBtn.Size = New Size(120, 35)
        updateBtn.BackColor = Color.FromArgb(147, 53, 53)
        updateBtn.ForeColor = Color.FromArgb(230, 216, 177)
        updateBtn.FlatStyle = FlatStyle.Flat
        AddHandler updateBtn.Click, Sub()
                                        Dim newStatus As String = statusCombo.SelectedItem.ToString()

                                        ' Show additional confirmation for cancellation
                                        If newStatus = "Cancelled" AndAlso deliveryInfo.DeliveryStatus <> "Cancelled" Then
                                            Dim confirmResult = MessageBox.Show(
                                             $"Are you sure you want to CANCEL this delivery?{vbCrLf}{vbCrLf}" &
                                                $"Product: {deliveryInfo.ProductName}{vbCrLf}" &
                                                    $"Quantity: {deliveryInfo.QuantitySold}{vbCrLf}{vbCrLf}" &
                                           $"The stock will be restored to inventory.",
                                                     "Confirm Cancellation",
                                                MessageBoxButtons.YesNo,
                                                MessageBoxIcon.Warning)

                                            If confirmResult <> DialogResult.Yes Then
                                                Return
                                            End If
                                        End If

                                        If UpdateStatusInDatabase(deliveryInfo.SaleID, newStatus) Then
                                            statusForm.DialogResult = DialogResult.OK
                                            statusForm.Close()
                                        End If
                                    End Sub
        statusForm.Controls.Add(updateBtn)

        Dim cancelBtn As New Button()
        cancelBtn.Text = "Cancel"
        cancelBtn.Location = New Point(310, 200)
        cancelBtn.Size = New Size(70, 35)
        cancelBtn.BackColor = Color.FromArgb(102, 66, 52)
        cancelBtn.ForeColor = Color.FromArgb(230, 216, 177)
        cancelBtn.FlatStyle = FlatStyle.Flat
        AddHandler cancelBtn.Click, Sub()
                                        statusForm.DialogResult = DialogResult.Cancel
                                        statusForm.Close()
                                    End Sub
        statusForm.Controls.Add(cancelBtn)

        If statusForm.ShowDialog() = DialogResult.OK Then
            LoadTodaysDeliveries() ' Refresh grid
        End If
    End Sub

    Private Function UpdateStatusInDatabase(saleID As Integer, newStatus As String) As Boolean
        Try
            Using conn As New SqlConnection(GetConnectionString())
                conn.Open()

                ' If status is being changed to Cancelled, restore the stock first
                If newStatus = "Cancelled" Then
                    ' Get the product details and quantity from the sale
                    Dim getProductQuery As String = "
                       SELECT ProductID, QuantitySold, DeliveryStatus
                            FROM SalesReport
                       WHERE SaleID = @SaleID"

                    Dim productID As Integer = 0
                    Dim quantitySold As Integer = 0
                    Dim currentStatus As String = ""

                    Using cmd As New SqlCommand(getProductQuery, conn)
                        cmd.Parameters.AddWithValue("@SaleID", saleID)

                        Using reader As SqlDataReader = cmd.ExecuteReader()
                            If reader.Read() Then
                                productID = reader.GetInt32(0)
                                quantitySold = reader.GetInt32(1)
                                currentStatus = reader.GetString(2)
                            Else
                                MessageBox.Show("Sale record not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                Return False
                            End If
                        End Using
                    End Using

                    ' Only restore stock if the current status is NOT already Cancelled
                    If currentStatus <> "Cancelled" Then
                        ' Restore stock quantity (add back the quantity that was sold)
                        Dim restoreStockQuery As String = "
                              UPDATE wholesaleProducts
                         SET StockQuantity = StockQuantity + @QuantitySold
                              WHERE ProductID = @ProductID"

                        Using cmd As New SqlCommand(restoreStockQuery, conn)
                            cmd.Parameters.AddWithValue("@QuantitySold", quantitySold)
                            cmd.Parameters.AddWithValue("@ProductID", productID)
                            Dim rowsAffected As Integer = cmd.ExecuteNonQuery()

                            If rowsAffected = 0 Then
                                MessageBox.Show("Product not found in inventory. Stock was not restored.", "Warning",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning)
                            End If
                        End Using

                        Debug.WriteLine($"Stock restored: Added {quantitySold} units back to ProductID {productID}")
                    Else
                        Debug.WriteLine($"Sale #{saleID} is already Cancelled. Stock not restored again.")
                    End If
                End If

                ' Update the delivery status
                Dim query As String = "UPDATE SalesReport SET DeliveryStatus = @Status WHERE SaleID = @SaleID"

                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@Status", newStatus)
                    cmd.Parameters.AddWithValue("@SaleID", saleID)
                    cmd.ExecuteNonQuery()
                End Using
            End Using

            ' Show success message with stock restoration info if applicable
            If newStatus = "Cancelled" Then
                MessageBox.Show($"Delivery cancelled successfully!{vbCrLf}{vbCrLf}Stock has been restored to inventory.",
       "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                MessageBox.Show("Status updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

            Return True
        Catch ex As Exception
            MessageBox.Show("Error updating status: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    Private Function GetConnectionString() As String
        Return SharedUtilities.GetConnectionString()
    End Function

    Private Sub SetVATRate()
        Try
            ' Get current VAT rate from database
            Dim currentVAT As Decimal = GetCurrentVATRate()

            ' Show input box with current VAT rate
            Dim input As String = InputBox($"Enter VAT Rate (%):{vbCrLf}Current VAT: {currentVAT:N2}%",
                                          "Set VAT Rate",
                                          currentVAT.ToString())

            ' Check if user cancelled
            If String.IsNullOrWhiteSpace(input) Then
                Return
            End If

            ' Validate input
            Dim newVATRate As Decimal
            If Not Decimal.TryParse(input, newVATRate) Then
                MessageBox.Show("Please enter a valid numeric value for VAT rate.",
                              "Invalid Input",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Warning)
                Return
            End If

            ' Validate range (0 to 100)
            If newVATRate < 0 OrElse newVATRate > 100 Then
                MessageBox.Show("VAT rate must be between 0 and 100.",
                              "Invalid Range",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Warning)
                Return
            End If

            ' Save to database
            If SaveVATRate(newVATRate) Then
                MessageBox.Show($"VAT rate successfully set to {newVATRate:N2}%",
                              "Success",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Information)
            Else
                MessageBox.Show("Failed to save VAT rate. Please try again.",
                              "Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error)
            End If
        Catch ex As Exception
            MessageBox.Show($"Error setting VAT rate: {ex.Message}",
                          "Error",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Error)
        End Try
    End Sub

    Private Function GetCurrentVATRate() As Decimal
        Return SharedUtilities.GetCurrentVATRate()
    End Function

    Private Function SaveVATRate(vatRate As Decimal) As Boolean
        Return SharedUtilities.SaveVATRate(vatRate)
    End Function

    Private Sub filterButton_Click(sender As Object, e As EventArgs) Handles filterButton.Click
        ' Clear existing rows
        tableDataGridView.Rows.Clear()

        Try
            Using conn As New SqlConnection(GetConnectionString())
                conn.Open()

                ' Query to get deliveries within date range
                Dim query As String = "
            SELECT sr.SaleID, p.ProductName, sr.DeliveryAddress,
        sr.DeliveryLatitude, sr.DeliveryLongitude, sr.DeliveryStatus,
         sr.SaleDate, sr.TotalAmount, sr.QuantitySold, sr.PaymentMethod
            FROM SalesReport sr
         INNER JOIN wholesaleProducts p ON sr.ProductID = p.ProductID
           WHERE sr.IsDelivery = 1
  AND CAST(sr.SaleDate AS DATE) BETWEEN @FromDate AND @ToDate
          AND sr.DeliveryStatus IS NOT NULL
        ORDER BY sr.SaleDate DESC"

                Using cmd As New SqlCommand(query, conn)
                    ' Get dates from DateTimePickers (use .Value.Date to get just the date part)
                    Dim fromDate As Date = DateTimePickerFrom.Value.Date
                    Dim toDate As Date = DateTimePickerTo.Value.Date

                    ' Validate date range
                    If fromDate > toDate Then
                        MessageBox.Show("'From' date cannot be later than 'To' date.", "Invalid Date Range",
              MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Return
                    End If

                    cmd.Parameters.AddWithValue("@FromDate", fromDate)
                    cmd.Parameters.AddWithValue("@ToDate", toDate)

                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            Dim rowIndex As Integer = tableDataGridView.Rows.Add()
                            Dim row As DataGridViewRow = tableDataGridView.Rows(rowIndex)

                            row.Cells("ColSaleID").Value = reader.GetInt32(0)
                            row.Cells("ColTime").Value = reader.GetDateTime(6).ToString("hh:mm tt")
                            row.Cells("ColProduct").Value = reader.GetString(1)
                            row.Cells("ColQuantity").Value = reader.GetInt32(8)
                            row.Cells("ColAmount").Value = reader.GetDecimal(7).ToString("C2")
                            row.Cells("ColAddress").Value = reader.GetString(2)
                            row.Cells("ColStatus").Value = reader.GetString(5)

                            ' Store delivery info in tag for updates and details view
                            row.Tag = New DeliveryInfo() With {
     .SaleID = reader.GetInt32(0),
.ProductName = reader.GetString(1),
     .DeliveryAddress = reader.GetString(2),
                .Latitude = reader.GetDouble(3),
      .Longitude = reader.GetDouble(4),
             .DeliveryStatus = reader.GetString(5),
              .SaleDate = reader.GetDateTime(6),
               .TotalAmount = reader.GetDecimal(7),
  .QuantitySold = reader.GetInt32(8),
            .PaymentMethod = If(reader.IsDBNull(9), "N/A", reader.GetString(9))
        }
                        End While
                    End Using
                End Using
            End Using

            ' Update title with date range and count
            Label1.Text = $"Deliveries ({tableDataGridView.Rows.Count}) - {DateTimePickerFrom.Value:MMM dd, yyyy} to {DateTimePickerTo.Value:MMM dd, yyyy}"
        Catch ex As Exception
            MessageBox.Show("Error filtering deliveries: " & ex.Message, "Error",
         MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub resetButton_Click(sender As Object, e As EventArgs) Handles resetButton.Click
        'reset datetimepickers to today
        DateTimePickerFrom.Value = Date.Today
        DateTimePickerTo.Value = Date.Today

        ' Clear any selected cells
        tableDataGridView.ClearSelection()

        LoadTodaysDeliveries()

    End Sub

End Class