Imports System.Drawing.Drawing2D
Imports System.IO
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports Microsoft.Data.SqlClient
Imports Microsoft.Office.Interop
Imports Document = iTextSharp.text.Document
Imports ITextFont = iTextSharp.text.Font

Public Class retailSalesReport
    Dim topPanel As New topPanelControl()
    Friend WithEvents sidePanel As sidePanelControl2
    Dim colorUnclicked As Color = Color.FromArgb(191, 181, 147)
    Dim colorClicked As Color = Color.FromArgb(102, 66, 52)
    Dim dt As New DataTable()
    Dim dv As New DataView()
    Dim bs As New BindingSource()
    Private placeholders As New Dictionary(Of TextBox, String)

    Public Sub New()
        InitializeComponent()

        sidePanel = New sidePanelControl2()
        sidePanel.Dock = DockStyle.Left
        topPanel.Dock = DockStyle.Top
        Me.Controls.Add(topPanel)
        Me.Controls.Add(sidePanel)
        Me.MaximizeBox = False
        Me.FormBorderStyle = FormBorderStyle.None
        Me.BackColor = Color.FromArgb(224, 166, 109)

        tableDataGridView.BackgroundColor = Color.FromArgb(230, 216, 177)
        tableDataGridView.GridColor = Color.FromArgb(79, 51, 40)
        DateTimePickerFrom.BackColor = Color.FromArgb(230, 216, 177)
        DateTimePickerTo.BackColor = Color.FromArgb(230, 216, 177)
        TextBoxSearch.BackColor = Color.FromArgb(230, 216, 177)

        Button1.BackColor = Color.FromArgb(147, 53, 53)
        Button1.ForeColor = Color.FromArgb(230, 216, 177)
        Button2.BackColor = Color.FromArgb(147, 53, 53)
        Button2.ForeColor = Color.FromArgb(230, 216, 177)
        btnSearch.BackColor = Color.FromArgb(147, 53, 53)
        btnSearch.ForeColor = Color.FromArgb(230, 216, 177)
        resetButton.BackColor = Color.FromArgb(147, 53, 53)
        resetButton.ForeColor = Color.FromArgb(230, 216, 177)

        tableDataGridView.ReadOnly = True
        tableDataGridView.AllowUserToAddRows = False
        tableDataGridView.AllowUserToDeleteRows = False
        tableDataGridView.RowHeadersVisible = False
        Me.KeyPreview = True

        AddHandler tableDataGridView.CellClick, AddressOf TableDataGridView_CellClick
    End Sub

    Protected Overrides Sub WndProc(ByRef m As Message)
        Const WM_SYSCOMMAND As Integer = &H112
        Const SC_RESTORE As Integer = &HF120
        Const SC_MOVE As Integer = &HF010

        If m.Msg = WM_SYSCOMMAND Then
            Dim command As Integer = (m.WParam.ToInt32() And &HFFF0)
            If command = SC_RESTORE Then Return
            If command = SC_MOVE Then Return
        End If

        MyBase.WndProc(m)
    End Sub

    Private Function ShowSingleForm(Of T As {Form, New})() As T
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

        If formToShow IsNot Me Then Me.Hide()
        Return DirectCast(formToShow, T)
    End Function

    Private Sub ChildFormClosed(sender As Object, e As FormClosedEventArgs)
    End Sub

    Private Sub HighlightButton(buttonName As String)
        For Each ctrl As Control In sidePanel.Controls
            If TypeOf ctrl Is Button Then
                ctrl.BackColor = colorClicked
                ctrl.ForeColor = colorUnclicked
            End If
        Next

        Dim btn As Button = sidePanel.Controls.OfType(Of Button)().FirstOrDefault(Function(b) b.Name = buttonName)
        If btn IsNot Nothing Then
            btn.BackColor = colorUnclicked
            btn.ForeColor = Color.FromArgb(79, 51, 40)
        End If
    End Sub

    Private Sub SidePanel_ButtonClicked(sender As Object, btnName As String) Handles sidePanel.ButtonClicked
        Select Case btnName
            Case "Button1"
                Dim form = ShowSingleForm(Of retailDashboard)()
                form.LoadDashboardData()
            Case "Button2"
                ShowSingleForm(Of inventoryRetail)()
            Case "Button3"
                Dim form = ShowSingleForm(Of categoriesForm)()
                form.loadCategories()
            Case "Button4"
                Dim form = ShowSingleForm(Of retailStockEditLogs)()
                form.loadStockEditLogs()
            Case "Button5"
                ShowSingleForm(Of retailSalesReport)()
            Case "Button6"
                ShowSingleForm(Of loginRecordsForm)()
            Case "Button7"
                Dim form = ShowSingleForm(Of userManagementForm)()
                form.LoadUsers()
            Case "Button11"
                SetVATRate()
        End Select
    End Sub

    Private Function GetConnectionString() As String
        Return SharedUtilities.GetConnectionString()
    End Function

    Public Sub loadSalesReport()
        Dim connStr As String = GetConnectionString()
        Dim query As String = "
        SELECT
            sr.SaleID,
     sr.SaleDate,
        rp.ProductName,
rp.unit,
    c.CategoryName,
       sr.QuantitySold,
            sr.UnitPrice,
       sr.TotalAmount,
    sr.PaymentMethod,
            u.username AS HandledBy
FROM RetailSalesReport sr
      INNER JOIN retailProducts rp ON sr.ProductID = rp.ProductID
    INNER JOIN Categories c ON sr.CategoryID = c.CategoryID
        INNER JOIN Users u ON sr.HandledBy = u.userID
        ORDER BY sr.SaleDate DESC"

        Try
            Using conn As New SqlConnection(connStr)
                Using da As New SqlDataAdapter(query, conn)
                    If dt Is Nothing Then
                        dt = New DataTable()
                    Else
                        dt.Clear()
                    End If

                    da.Fill(dt)

                    With tableDataGridView
                        .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
                        .Columns("SaleID").HeaderText = "Sale ID"
                        .Columns("SaleDate").HeaderText = "Date"
                        .Columns("ProductName").HeaderText = "Product"
                        .Columns("unit").HeaderText = "Unit"
                        .Columns("CategoryName").HeaderText = "Category"
                        .Columns("QuantitySold").HeaderText = "Quantity"
                        .Columns("UnitPrice").HeaderText = "Unit Price"
                        .Columns("TotalAmount").HeaderText = "Total"
                        .Columns("PaymentMethod").HeaderText = "Payment"
                        .Columns("HandledBy").HeaderText = "Handled By"
                    End With
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading sales report: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub salesReport_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        HighlightButton("Button5")
        SetPlaceholder(TextBoxSearch, "Search...")
        SetRoundedRegion2(Button1, 20)
        SetRoundedRegion2(Button2, 20)

        dt = New DataTable()
        dv = New DataView(dt)
        bs = New BindingSource()
        bs.DataSource = dv
        tableDataGridView.DataSource = bs
        loadSalesReport()
    End Sub

    Private Sub retailSalesReport_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        Select Case e.KeyCode
            Case Keys.Enter
                If btnSearch.Enabled Then
                    btnSearch.PerformClick()
                    e.Handled = True
                    e.SuppressKeyPress = True
                End If
            Case Keys.F5
                If resetButton.Enabled Then
                    resetButton.PerformClick()
                    e.Handled = True
                    e.SuppressKeyPress = True
                End If
        End Select
    End Sub

    Private Sub TableDataGridView_CellClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex < 0 Then Return

        Try
            Dim row As DataGridViewRow = tableDataGridView.Rows(e.RowIndex)
            Dim saleID As Integer = Convert.ToInt32(row.Cells("SaleID").Value)
            ShowSaleDetails(saleID)
        Catch ex As Exception
            MessageBox.Show($"Error displaying sale details: {ex.Message}", "Error",
              MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub ShowSaleDetails(saleID As Integer)
        Try
            Dim connStr As String = GetConnectionString()
            Dim detailsText As New System.Text.StringBuilder()

            Using conn As New SqlConnection(connStr)
                conn.Open()

                Dim query As String = "
                  SELECT
                          sr.SaleID, sr.SaleDate, rp.ProductName, rp.SKU, rp.unit AS Unit,
                    c.CategoryName, sr.QuantitySold, sr.UnitPrice, sr.TotalAmount,
                         sr.PaymentMethod, u.username AS HandledBy, sr.PayerName,
                  sr.ReferenceNumber, sr.BankName, ISNULL(sr.IsRefunded, 0) AS IsRefunded,
                      sr.RefundDate, sr.RefundReason
                            FROM RetailSalesReport sr
                       INNER JOIN retailProducts rp ON sr.ProductID = rp.ProductID
                 INNER JOIN Categories c ON sr.CategoryID = c.CategoryID
                            INNER JOIN Users u ON sr.HandledBy = u.userID
                  WHERE sr.SaleID = @SaleID"

                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@SaleID", saleID)

                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            Dim isRefunded As Boolean = Convert.ToBoolean(reader("IsRefunded"))

                            detailsText.AppendLine("=======================================")
                            detailsText.AppendLine(vbTab & vbTab & "RETAIL SALE DETAILS")
                            detailsText.AppendLine("=======================================")
                            detailsText.AppendLine()

                            If isRefunded Then
                                detailsText.AppendLine("*** REFUNDED TRANSACTION ***")
                                detailsText.AppendLine()
                            End If

                            detailsText.AppendLine("Transaction Information:")
                            detailsText.AppendLine("----------------------------------------")
                            detailsText.AppendLine($"Sale ID: {reader("SaleID")}")
                            detailsText.AppendLine($"Date: {Convert.ToDateTime(reader("SaleDate")):MMMM dd, yyyy}")
                            detailsText.AppendLine($"Time: {Convert.ToDateTime(reader("SaleDate")):hh:mm:ss tt}")
                            detailsText.AppendLine($"Handled By: {reader("HandledBy")}")
                            detailsText.AppendLine()

                            detailsText.AppendLine("Product Information:")
                            detailsText.AppendLine("----------------------------------------")
                            detailsText.AppendLine($"Product: {reader("ProductName")}")
                            detailsText.AppendLine($"SKU: {reader("SKU")}")
                            detailsText.AppendLine($"Category: {reader("CategoryName")}")
                            detailsText.AppendLine($"Unit: {reader("Unit")}")
                            detailsText.AppendLine()

                            detailsText.AppendLine("Pricing Information:")
                            detailsText.AppendLine("----------------------------------------")
                            detailsText.AppendLine($"Quantity Sold: {reader("QuantitySold")}")
                            detailsText.AppendLine($"Unit Price: P{Convert.ToDecimal(reader("UnitPrice")):N2}")
                            detailsText.AppendLine($"Total Amount: P{Convert.ToDecimal(reader("TotalAmount")):N2}")
                            detailsText.AppendLine()

                            detailsText.AppendLine("Payment Information:")
                            detailsText.AppendLine("----------------------------------------")
                            Dim paymentMethod As String = reader("PaymentMethod").ToString()
                            detailsText.AppendLine($"Payment Method: {paymentMethod}")

                            If paymentMethod = "GCash" OrElse paymentMethod = "Bank Transaction" Then
                                If Not IsDBNull(reader("PayerName")) Then
                                    detailsText.AppendLine($"Payer Name: {reader("PayerName")}")
                                End If
                                If Not IsDBNull(reader("ReferenceNumber")) Then
                                    detailsText.AppendLine($"Reference Number: {reader("ReferenceNumber")}")
                                End If
                                If paymentMethod = "Bank Transaction" AndAlso Not IsDBNull(reader("BankName")) Then
                                    detailsText.AppendLine($"Bank Name: {reader("BankName")}")
                                End If
                            End If
                            detailsText.AppendLine()

                            If isRefunded Then
                                detailsText.AppendLine("Refund Information:")
                                detailsText.AppendLine("----------------------------------------")
                                If Not IsDBNull(reader("RefundDate")) Then
                                    detailsText.AppendLine($"Refund Date: {Convert.ToDateTime(reader("RefundDate")):MMMM dd, yyyy hh:mm tt}")
                                End If
                                If Not IsDBNull(reader("RefundReason")) Then
                                    detailsText.AppendLine($"Refund Reason: {reader("RefundReason")}")
                                End If
                                detailsText.AppendLine($"Refunded Amount: P{Convert.ToDecimal(reader("TotalAmount")):N2}")
                                detailsText.AppendLine()
                            End If

                            detailsText.AppendLine("=======================================")

                            Dim title As String = If(isRefunded,
   $"Sale Details (REFUNDED) - #{saleID}",
 $"Sale Details - #{saleID}")

                            MessageBox.Show(detailsText.ToString(), title,
         MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Else
                            MessageBox.Show("Sale record not found.", "Not Found",
         MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show($"Error loading sale details: {ex.Message}", "Error",
        MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub SetRoundedRegion2(ctrl As Control, radius As Integer)
        Dim rect As New System.Drawing.Rectangle(0, 0, ctrl.Width, ctrl.Height)
        Using path As GraphicsPath = GetRoundedRectanglePath2(rect, radius)
            ctrl.Region = New Region(path)
        End Using
    End Sub

    Private Function GetRoundedRectanglePath2(rect As System.Drawing.Rectangle, radius As Integer) As GraphicsPath
        Dim path As New GraphicsPath()
        Dim diameter As Integer = radius * 2

        path.StartFigure()
        path.AddLine(rect.X + radius, rect.Y, rect.Right - radius, rect.Y)
        path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90)
        path.AddLine(rect.Right, rect.Y + radius, rect.Right, rect.Bottom - radius)
        path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90)
        path.AddLine(rect.Right - radius, rect.Bottom, rect.X + radius, rect.Bottom)
        path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90)
        path.AddLine(rect.X, rect.Bottom - radius, rect.X, rect.Y + radius)
        path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90)
        path.CloseFigure()

        Return path
    End Function

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

    Private Sub TextBoxSearch_TextChanged(sender As Object, e As EventArgs) Handles TextBoxSearch.TextChanged
        Dim placeholder = ""
        If placeholders.ContainsKey(TextBoxSearch) Then
            placeholder = placeholders(TextBoxSearch)
        End If

        If String.IsNullOrWhiteSpace(TextBoxSearch.Text) OrElse TextBoxSearch.Text = placeholder Then
            bs.Filter = ""
        Else
            bs.Filter = String.Format("ProductName LIKE '%{0}%'", TextBoxSearch.Text.Replace("'", "''"))
        End If
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Using con As New SqlConnection(GetConnectionString())
            Dim query As String = "
    SELECT
     sr.SaleID, sr.SaleDate, rp.ProductName, rp.unit, c.CategoryName,
             sr.QuantitySold, sr.UnitPrice, sr.TotalAmount,
  sr.PaymentMethod, u.username AS HandledBy
      FROM RetailSalesReport sr
 INNER JOIN retailProducts rp ON sr.ProductID = rp.ProductID
        INNER JOIN Categories c ON sr.CategoryID = c.CategoryID
    INNER JOIN Users u ON sr.HandledBy = u.userID
   WHERE sr.SaleDate >= @FromDate AND sr.SaleDate <= @ToDate
       ORDER BY sr.SaleDate DESC"

            Using cmd As New SqlCommand(query, con)
                ' Set FromDate to start of day (00:00:00)
                cmd.Parameters.AddWithValue("@FromDate", DateTimePickerFrom.Value.Date)
                ' Set ToDate to end of day (23:59:59)
                cmd.Parameters.AddWithValue("@ToDate", DateTimePickerTo.Value.Date.AddDays(1).AddSeconds(-1))

                Dim da As New SqlDataAdapter(cmd)
                dt.Clear()
                da.Fill(dt)
            End Using
        End Using
    End Sub

    Private Sub btnReset_Click(sender As Object, e As EventArgs) Handles resetButton.Click
        DateTimePickerFrom.Value = DateTime.Today
        DateTimePickerTo.Value = DateTime.Today
        tableDataGridView.ClearSelection()
        loadSalesReport()
    End Sub

    Private Sub ExportToExcel()
        Try
            Dim excelApp As New Excel.Application
            Dim workbook As Excel.Workbook = excelApp.Workbooks.Add()
            Dim worksheet As Excel.Worksheet = workbook.Sheets(1)

            For col As Integer = 0 To tableDataGridView.Columns.Count - 1
                worksheet.Cells(1, col + 1) = tableDataGridView.Columns(col).HeaderText
            Next

            Dim headerRange As Excel.Range = worksheet.Range("A1", worksheet.Cells(1, tableDataGridView.Columns.Count))
            headerRange.Font.Bold = True
            headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
            headerRange.Interior.Color = RGB(230, 216, 177)

            For row As Integer = 0 To tableDataGridView.Rows.Count - 1
                For col As Integer = 0 To tableDataGridView.Columns.Count - 1
                    worksheet.Cells(row + 2, col + 1) = tableDataGridView.Rows(row).Cells(col).Value
                Next
            Next

            Dim saveDialog As New SaveFileDialog()
            saveDialog.Filter = "Excel Files|*.xlsx"
            saveDialog.FileName = "RetailSalesReport.xlsx"

            If saveDialog.ShowDialog() = DialogResult.OK Then
                worksheet.Columns.AutoFit()
                worksheet.Rows.AutoFit()
                workbook.SaveAs(saveDialog.FileName)
                workbook.Close()
                excelApp.Quit()
                MessageBox.Show("Exported to Excel successfully!")
            End If
        Catch ex As Exception
            MessageBox.Show("Error exporting to Excel: " & ex.Message)
        End Try
    End Sub

    Private Sub ExportToPDF()
        Try
            Dim saveDialog As New SaveFileDialog()
            saveDialog.Filter = "PDF Files|*.pdf"
            saveDialog.FileName = "RetailSalesReport.pdf"

            If saveDialog.ShowDialog() = DialogResult.OK Then
                Dim pdfTable As New PdfPTable(tableDataGridView.Columns.Count)
                pdfTable.WidthPercentage = 100

                Dim widths(tableDataGridView.Columns.Count - 1) As Single
                For i As Integer = 0 To widths.Length - 1
                    widths(i) = 1
                Next
                pdfTable.SetWidths(widths)

                Dim headerFont As ITextFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, BaseColor.BLACK)
                For Each column As DataGridViewColumn In tableDataGridView.Columns
                    Dim headerCell As New PdfPCell(New Phrase(column.HeaderText, headerFont))
                    headerCell.BackgroundColor = BaseColor.LIGHT_GRAY
                    headerCell.HorizontalAlignment = Element.ALIGN_CENTER
                    pdfTable.AddCell(headerCell)
                Next

                Dim cellFont As ITextFont = FontFactory.GetFont(FontFactory.HELVETICA, 9, BaseColor.BLACK)
                For Each row As DataGridViewRow In tableDataGridView.Rows
                    If Not row.IsNewRow Then
                        For Each cell As DataGridViewCell In row.Cells
                            Dim pdfCell As New PdfPCell(New Phrase(If(cell.Value IsNot Nothing, cell.Value.ToString(), ""), cellFont))
                            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER
                            pdfTable.AddCell(pdfCell)
                        Next
                    End If
                Next

                Using stream As New FileStream(saveDialog.FileName, FileMode.Create)
                    Dim pdfDoc As New Document(PageSize.A4, 10, 10, 10, 10)
                    PdfWriter.GetInstance(pdfDoc, stream)
                    pdfDoc.Open()
                    pdfDoc.Add(pdfTable)
                    pdfDoc.Close()
                    stream.Close()
                End Using

                MessageBox.Show("Exported to PDF successfully!")
            End If
        Catch ex As Exception
            MessageBox.Show("Error exporting to PDF: " & ex.Message)
        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ExportToExcel()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        ExportToPDF()
    End Sub

    Private Sub SetVATRate()
        Try
            Dim currentVAT As Decimal = GetCurrentVATRate()
            Dim input As String = InputBox($"Enter VAT Rate (%):{vbCrLf}Current VAT: {currentVAT:N2}%",
                "Set VAT Rate", currentVAT.ToString())

            If String.IsNullOrWhiteSpace(input) Then Return

            Dim newVATRate As Decimal
            If Not Decimal.TryParse(input, newVATRate) Then
                MessageBox.Show("Please enter a valid numeric value for VAT rate.",
          "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            If newVATRate < 0 OrElse newVATRate > 100 Then
                MessageBox.Show("VAT rate must be between 0 and 100.",
   "Invalid Range", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            If SaveVATRate(newVATRate) Then
                MessageBox.Show($"VAT rate successfully set to {newVATRate:N2}%",
     "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            MessageBox.Show($"Error setting VAT rate: {ex.Message}",
    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Function GetCurrentVATRate() As Decimal
        Return SharedUtilities.GetCurrentVATRate()
    End Function

    Private Function SaveVATRate(vatRate As Decimal) As Boolean
        Try
            Dim errorMessage As String = String.Empty
            Dim success As Boolean = SharedUtilities.SaveVATRate(vatRate, errorMessage)

            If Not success Then
                If Not String.IsNullOrEmpty(errorMessage) Then
                    MessageBox.Show($"Failed to save VAT rate: {errorMessage}",
            "Error Details", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Else
                    MessageBox.Show("Failed to save VAT rate. No specific error message was returned from the database operation.",
       "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End If

            Return success
        Catch ex As Exception
            MessageBox.Show($"Unexpected error in SaveVATRate wrapper: {ex.Message}{vbCrLf}{vbCrLf}Stack Trace:{vbCrLf}{ex.StackTrace}",
        "Critical Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

End Class