Imports System.Drawing.Drawing2D
Imports System.IO
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports Microsoft.Data.SqlClient
Imports Microsoft.Office.Interop
Imports Document = iTextSharp.text.Document

Public Class retailSalesReport
    Dim topPanel As New topPanelControl()
    Friend WithEvents sidePanel As sidePanelControl2
    Dim colorUnclicked As Color = Color.FromArgb(191, 181, 147)
    Dim colorClicked As Color = Color.FromArgb(102, 66, 52)
    Dim dt As New DataTable()
    Dim dv As New DataView()
    Dim bs As New BindingSource()
    ' Dictionary to store placeholder texts for each TextBox
    Private placeholders As New Dictionary(Of TextBox, String)

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
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
                Dim form = ShowSingleForm(Of wholeSaleStockEditLogs)()
                form.loadStockEditLogs()
            Case "Button10"
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
        ORDER BY sr.SaleDate DESC
    "

        Try
            Using conn As New SqlConnection(connStr)
                Using da As New SqlDataAdapter(query, conn)
                    ' Ensure dt exists
                    If dt Is Nothing Then
                        dt = New DataTable()
                    Else
                        dt.Clear()
                    End If

                    da.Fill(dt)


                    ' Optional formatting
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

        ' Example data
        ' Initialize
        dt = New DataTable()
        dv = New DataView(dt)
        bs = New BindingSource()

        ' Bindings
        bs.DataSource = dv
        tableDataGridView.DataSource = bs


        ' Load data
        loadSalesReport()
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

        ' Top edge
        path.AddLine(rect.X + radius, rect.Y, rect.Right - radius, rect.Y)
        ' Top-right corner
        path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90)
        ' Right edge
        path.AddLine(rect.Right, rect.Y + radius, rect.Right, rect.Bottom - radius)
        ' Bottom-right corner
        path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90)
        ' Bottom edge
        path.AddLine(rect.Right - radius, rect.Bottom, rect.X + radius, rect.Bottom)
        ' Bottom-left corner
        path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90)
        ' Left edge
        path.AddLine(rect.X, rect.Bottom - radius, rect.X, rect.Y + radius)
        ' Top-left corner
        path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90)

        path.CloseFigure()
        Return path
    End Function


    Private Sub SetPlaceholder(tb As TextBox, text As String)
        placeholders(tb) = text
        tb.Text = text
        tb.ForeColor = Color.Gray

        ' Attach shared events
        AddHandler tb.GotFocus, AddressOf RemovePlaceholder
        AddHandler tb.LostFocus, AddressOf RestorePlaceholder
    End Sub

    ' When the user clicks/focuses the TextBox
    Private Sub RemovePlaceholder(sender As Object, e As EventArgs)
        Dim tb As TextBox = DirectCast(sender, TextBox)
        If tb.Text = placeholders(tb) Then
            tb.Text = ""
            tb.ForeColor = Color.Black
        End If
    End Sub

    ' When the TextBox loses focus
    Private Sub RestorePlaceholder(sender As Object, e As EventArgs)
        Dim tb As TextBox = DirectCast(sender, TextBox)
        If String.IsNullOrWhiteSpace(tb.Text) Then
            tb.Text = placeholders(tb)
            tb.ForeColor = Color.Gray
        End If
    End Sub

    Private Sub TextBoxSearch_TextChanged(sender As Object, e As EventArgs) Handles TextBoxSearch.TextChanged
        'reset place holder if focused to not interfere with search
        Dim placeholder = ""
        If placeholders.ContainsKey(TextBoxSearch) Then
            placeholder = placeholders(TextBoxSearch)
        End If

        If String.IsNullOrWhiteSpace(TextBoxSearch.Text) OrElse TextBoxSearch.Text = placeholder Then
            bs.Filter = ""   ' Show all rows
        Else
            bs.Filter = String.Format("ProductName LIKE '%{0}%'", TextBoxSearch.Text.Replace("'", "''"))
        End If
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Using con As New SqlConnection(GetConnectionString())
            Dim query As String = "
            SELECT 
                sr.SaleID,
                sr.SaleDate,
                rp.ProductName,
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
            WHERE sr.SaleDate BETWEEN @FromDate AND @ToDate
            ORDER BY sr.SaleDate DESC"

            Using cmd As New SqlCommand(query, con)
                cmd.Parameters.AddWithValue("@FromDate", DateTimePickerFrom.Value.Date)
                cmd.Parameters.AddWithValue("@ToDate", DateTimePickerTo.Value.Date)

                Dim da As New SqlDataAdapter(cmd)
                dt.Clear()                ' reuse class-level dt (mybase load) to keep the same binding
                da.Fill(dt)               ' fills the same dt bound to dv/bs/grid
            End Using
        End Using
    End Sub

    ' Reset button (show all records + reset pickers)
    Private Sub btnReset_Click(sender As Object, e As EventArgs) Handles resetButton.Click
        ' Reset the DateTimePickers to today's date
        DateTimePickerFrom.Value = DateTime.Today
        DateTimePickerTo.Value = DateTime.Today

        ' Reload all records
        loadSalesReport()
    End Sub

    Private Sub ExportToExcel()
        Try
            Dim excelApp As New Excel.Application
            Dim workbook As Excel.Workbook = excelApp.Workbooks.Add()
            Dim worksheet As Excel.Worksheet = workbook.Sheets(1)

            ' Export column headers
            For col As Integer = 0 To tableDataGridView.Columns.Count - 1
                worksheet.Cells(1, col + 1) = tableDataGridView.Columns(col).HeaderText
            Next

            ' Format header row
            Dim headerRange As Excel.Range = worksheet.Range("A1", worksheet.Cells(1, tableDataGridView.Columns.Count))
            headerRange.Font.Bold = True
            headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
            headerRange.Interior.Color = RGB(230, 216, 177)
            ' Export rows
            For row As Integer = 0 To tableDataGridView.Rows.Count - 1
                For col As Integer = 0 To tableDataGridView.Columns.Count - 1
                    worksheet.Cells(row + 2, col + 1) = tableDataGridView.Rows(row).Cells(col).Value
                Next
            Next

            ' Save file
            Dim saveDialog As New SaveFileDialog()
            saveDialog.Filter = "Excel Files|*.xlsx"
            saveDialog.FileName = "RetailSalesReport.xlsx"

            If saveDialog.ShowDialog() = DialogResult.OK Then
                ' Auto fit columns and rows before saving
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

                ' Set equal column widths
                Dim widths(tableDataGridView.Columns.Count - 1) As Single
                For i As Integer = 0 To widths.Length - 1
                    widths(i) = 1 ' all equal weight
                Next
                pdfTable.SetWidths(widths)

                ' Add styled headers
                Dim headerFont As Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, BaseColor.BLACK)
                For Each column As DataGridViewColumn In tableDataGridView.Columns
                    Dim headerCell As New PdfPCell(New Phrase(column.HeaderText, headerFont))
                    headerCell.BackgroundColor = BaseColor.LIGHT_GRAY
                    headerCell.HorizontalAlignment = Element.ALIGN_CENTER
                    pdfTable.AddCell(headerCell)
                Next

                ' Add data rows
                Dim cellFont As Font = FontFactory.GetFont(FontFactory.HELVETICA, 9, BaseColor.BLACK)
                For Each row As DataGridViewRow In tableDataGridView.Rows
                    If Not row.IsNewRow Then
                        For Each cell As DataGridViewCell In row.Cells
                            Dim pdfCell As New PdfPCell(New Phrase(If(cell.Value IsNot Nothing, cell.Value.ToString(), ""), cellFont))
                            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER
                            pdfTable.AddCell(pdfCell)
                        Next
                    End If
                Next


                ' Write file
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
End Class