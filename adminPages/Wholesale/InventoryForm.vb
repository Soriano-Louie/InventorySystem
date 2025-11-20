Imports System.Drawing.Drawing2D
Imports System.Drawing.Printing
Imports System.IO
Imports Microsoft.Data.SqlClient

Public Class InventoryForm
    Dim topPanel As New topPanelControl()
    Friend WithEvents sidePanel As sidePanelControl
    Dim colorUnclicked As Color = Color.FromArgb(191, 181, 147)
    Dim colorClicked As Color = Color.FromArgb(102, 66, 52)
    Dim dt As New DataTable()
    Dim dv As New DataView()
    Dim bs As New BindingSource()

    ' Track if we're showing summary or detail view
    Private isShowingDetailView As Boolean = False

    Private currentSelectedProductName As String = ""

    ' Back to Summary button
    Private WithEvents btnBackToSummary As New Button()

    Private WithEvents printDoc As New PrintDocument
    Private WithEvents printDocAll As New PrintDocument

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        sidePanel = New sidePanelControl()
        sidePanel.Dock = DockStyle.Left
        topPanel.Dock = DockStyle.Top
        topPanel.Margin = New Padding(0, 0, 10, 0)
        Me.Controls.Add(topPanel)
        Me.Controls.Add(sidePanel)
        Me.MaximizeBox = False
        Me.FormBorderStyle = FormBorderStyle.None
        Me.BackColor = Color.FromArgb(224, 166, 109)
        tableDataGridView.BackgroundColor = Color.FromArgb(230, 216, 177)
        tableDataGridView.GridColor = Color.FromArgb(79, 51, 40)

        TextBoxSearch.BackColor = Color.FromArgb(230, 216, 177)

        Button1.BackColor = Color.FromArgb(147, 53, 53)
        Button2.BackColor = Color.FromArgb(147, 53, 53)
        Button1.ForeColor = Color.FromArgb(230, 216, 177)
        Button2.ForeColor = Color.FromArgb(230, 216, 177)
        btnPrintAllQRCodes.BackColor = Color.FromArgb(147, 53, 53)
        btnPrintAllQRCodes.ForeColor = Color.FromArgb(230, 216, 177)

        tableDataGridView.ReadOnly = True
        tableDataGridView.AllowUserToAddRows = False
        tableDataGridView.AllowUserToDeleteRows = False
        tableDataGridView.RowHeadersVisible = False
        tableDataGridView.TabStop = False

        ' Initialize Back to Summary button
        InitializeBackButton()

        ' Add cell click event handler for expanding product batches
        AddHandler tableDataGridView.CellClick, AddressOf TableDataGridView_RowClick
    End Sub

    ''' Initialize the Back to Summary button
    Private Sub InitializeBackButton()
        With btnBackToSummary
            .Text = "← Back to Summary"
            .Size = New Size(160, 40)
            .BackColor = Color.FromArgb(147, 53, 53)
            .ForeColor = Color.FromArgb(230, 216, 177)
            .Font = New Font("Segoe UI", 10, FontStyle.Bold)
            .Cursor = Cursors.Hand
            .Visible = False ' Hidden by default
            .FlatStyle = FlatStyle.Flat
            .FlatAppearance.BorderSize = 0
            .AutoSize = False
        End With

        ' Position it near the top-left of the data grid
        btnBackToSummary.Location = New Point(tableDataGridView.Left + 250, tableDataGridView.Top - -50)

        Me.Controls.Add(btnBackToSummary)
        btnBackToSummary.BringToFront()

        ' Round the corners
        SetRoundedRegion2(btnBackToSummary, 15)
    End Sub

    ''' <summary>
    ''' Handle Back to Summary button click
    ''' </summary>
    Private Sub btnBackToSummary_Click(sender As Object, e As EventArgs) Handles btnBackToSummary.Click
        LoadProducts()
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
                Dim form = ShowSingleForm(Of wholesaleDashboard)()
                form.LoadDashboardData()
            Case "Button2"
                ShowSingleForm(Of InventoryForm)()
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

    Private Function GetConnectionString() As String
        Return SharedUtilities.GetConnectionString()
    End Function

    ''' Load products in aggregated summary view (one row per product name)
    Public Sub LoadProducts()
        Dim connString As String = GetConnectionString()

        ' Query to get aggregated product summary
        Dim query As String = "
        SELECT
        p.ProductName,
        c.CategoryName,
        SUM(p.StockQuantity) AS TotalQuantity,
        COUNT(*) AS BatchCount
        FROM wholesaleProducts p
        INNER JOIN Categories c ON p.CategoryID = c.CategoryID
        GROUP BY p.ProductName, c.CategoryName
        ORDER BY p.ProductName"

        Try
            Using conn As New SqlConnection(connString)
                Using da As New SqlDataAdapter(query, conn)
                    dt.Clear()
                    da.Fill(dt)
                End Using
            End Using

            ' Reset to summary view
            isShowingDetailView = False
            currentSelectedProductName = ""

            ' Hide back button in summary view
            btnBackToSummary.Visible = False

            ' Re-bind DataView and BindingSource
            dv = New DataView(dt)
            bs.DataSource = dv
            tableDataGridView.DataSource = bs

            ' Configure headers for summary view
            With tableDataGridView
                .EnableHeadersVisualStyles = False
                .ColumnHeadersDefaultCellStyle.Font = New Font(.Font, FontStyle.Bold)

                ' Ensure only summary columns are visible
                For Each col As DataGridViewColumn In .Columns
                    ' Hide all columns first
                    col.Visible = False
                Next

                ' Show only summary columns
                If .Columns.Contains("ProductName") Then
                    .Columns("ProductName").HeaderText = "Product Name (Click to view batches)"
                    .Columns("ProductName").SortMode = DataGridViewColumnSortMode.Automatic
                    .Columns("ProductName").Visible = True
                End If

                If .Columns.Contains("CategoryName") Then
                    .Columns("CategoryName").HeaderText = "Category"
                    .Columns("CategoryName").Visible = True
                End If

                If .Columns.Contains("TotalQuantity") Then
                    .Columns("TotalQuantity").HeaderText = "Total Stock"
                    .Columns("TotalQuantity").Visible = True
                End If

                If .Columns.Contains("BatchCount") Then
                    .Columns("BatchCount").HeaderText = "# of Batches"
                    .Columns("BatchCount").Visible = True
                End If
            End With

            tableDataGridView.Refresh()
        Catch ex As Exception
            MessageBox.Show("Error loading data: " & ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' Load detailed batch view for a specific product name
    ''' </summary>
    Private Sub LoadProductBatches(productName As String)
        Dim connString As String = GetConnectionString()

        ' Query to get all batches for the selected product
        Dim query As String = "
          SELECT
        p.SKU,
        p.ProductName,
        c.CategoryName,
        p.unit,
        p.retailPrice,
        p.cost,
        p.StockQuantity,
        p.ReorderLevel,
        p.expirationDate,
        p.QRCodeImage
        FROM wholesaleProducts p
        INNER JOIN Categories c ON p.CategoryID = c.CategoryID
        WHERE p.ProductName = @ProductName
        ORDER BY
        CASE WHEN p.expirationDate IS NULL THEN 1 ELSE 0 END,
        p.expirationDate ASC"

        Try
            Using conn As New SqlConnection(connString)
                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@ProductName", productName)

                    Using da As New SqlDataAdapter(cmd)
                        dt.Clear()
                        da.Fill(dt)
                    End Using
                End Using
            End Using

            ' Add QR Code Hex column
            If Not dt.Columns.Contains("QR Code Hex") Then
                dt.Columns.Add("QR Code Hex", GetType(String))
            End If

            For Each row As DataRow In dt.Rows
                If Not IsDBNull(row("QRCodeImage")) Then
                    Dim bytes As Byte() = CType(row("QRCodeImage"), Byte())
                    row("QR Code Hex") = BitConverter.ToString(bytes).Replace("-", "")
                End If
            Next

            ' Mark as detail view
            isShowingDetailView = True
            currentSelectedProductName = productName

            ' Show back button in detail view
            btnBackToSummary.Visible = True
            btnBackToSummary.BringToFront()

            ' Re-bind DataView and BindingSource
            dv = New DataView(dt)
            bs.DataSource = dv
            tableDataGridView.DataSource = bs

            ' Configure headers for detail view
            With tableDataGridView
                .EnableHeadersVisualStyles = False
                .ColumnHeadersDefaultCellStyle.Font = New Font(.Font, FontStyle.Bold)

                ' Make all detail columns visible
                For Each col As DataGridViewColumn In .Columns
                    col.Visible = True ' Show all columns by default
                Next

                ' Set headers and ensure visibility for each column
                If .Columns.Contains("SKU") Then
                    .Columns("SKU").HeaderText = "Product Code"
                    .Columns("SKU").Visible = True
                End If

                If .Columns.Contains("ProductName") Then
                    .Columns("ProductName").HeaderText = "Product Name"
                    .Columns("ProductName").Visible = True
                End If

                If .Columns.Contains("CategoryName") Then
                    .Columns("CategoryName").HeaderText = "Category"
                    .Columns("CategoryName").Visible = True
                End If

                If .Columns.Contains("unit") Then
                    .Columns("unit").HeaderText = "Unit"
                    .Columns("unit").Visible = True
                End If

                If .Columns.Contains("retailPrice") Then
                    .Columns("retailPrice").HeaderText = "Retail Price"
                    .Columns("retailPrice").Visible = True
                End If

                If .Columns.Contains("cost") Then
                    .Columns("cost").HeaderText = "Cost"
                    .Columns("cost").Visible = True
                End If

                If .Columns.Contains("StockQuantity") Then
                    .Columns("StockQuantity").HeaderText = "Quantity in Stock"
                    .Columns("StockQuantity").Visible = True
                End If

                If .Columns.Contains("ReorderLevel") Then
                    .Columns("ReorderLevel").HeaderText = "Reorder Level"
                    .Columns("ReorderLevel").Visible = True
                End If

                If .Columns.Contains("expirationDate") Then
                    .Columns("expirationDate").HeaderText = "Expiration Date"
                    .Columns("expirationDate").Visible = True
                End If

                If .Columns.Contains("QR Code Hex") Then
                    .Columns("QR Code Hex").HeaderText = "QR Code (Click to view)"
                    .Columns("QR Code Hex").Visible = True
                End If

                ' Hide columns that don't belong in detail view
                If .Columns.Contains("QRCodeImage") Then
                    .Columns("QRCodeImage").Visible = False
                End If

                If .Columns.Contains("TotalQuantity") Then
                    .Columns("TotalQuantity").Visible = False
                End If

                If .Columns.Contains("BatchCount") Then
                    .Columns("BatchCount").Visible = False
                End If
            End With

            tableDataGridView.Refresh()
        Catch ex As Exception
            MessageBox.Show("Error loading product batches: " & ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' Handle row click - toggle between summary and detail view
    ''' </summary>
    Private Sub TableDataGridView_RowClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex < 0 Then Return ' Ignore header clicks

        Try
            If Not isShowingDetailView Then
                ' Currently in summary view - show batches for clicked product
                Dim productName As String = tableDataGridView.Rows(e.RowIndex).Cells("ProductName").Value?.ToString()

                If Not String.IsNullOrWhiteSpace(productName) Then
                    LoadProductBatches(productName)
                End If
            Else
                ' Currently in detail view - handle QR code click if applicable
                If e.ColumnIndex >= 0 AndAlso tableDataGridView.Columns(e.ColumnIndex).Name = "QR Code Hex" Then
                    Dim hexString As String = tableDataGridView.Rows(e.RowIndex).Cells(e.ColumnIndex).Value?.ToString()

                    If Not String.IsNullOrWhiteSpace(hexString) Then
                        ' Convert HEX string back to Byte()
                        Dim bytes As Byte() = Enumerable.Range(0, hexString.Length \ 2) _
                        .Select(Function(i) Convert.ToByte(hexString.Substring(i * 2, 2), 16)).ToArray()

                        ' Create image from Byte()
                        Using ms As New MemoryStream(bytes)
                            Dim qrImage As Image = Image.FromStream(ms)
                            Dim productName As String = tableDataGridView.Rows(e.RowIndex).Cells("ProductName").Value.ToString()

                            ' Show the image in a preview form
                            Dim previewForm As New Form With {
                      .Text = "QR Code Preview",
                .Size = New Size(300, 300),
                       .StartPosition = FormStartPosition.CenterParent
                     }

                            Dim pb As New PictureBox With {
                                            .Dock = DockStyle.Fill,
                                     .Image = qrImage,
                                    .SizeMode = PictureBoxSizeMode.Zoom
                            }
                            previewForm.Controls.Add(pb)

                            ' Add a Print button
                            Dim btnPrint As New Button With {
                          .Text = "Print QR Code",
                       .Dock = DockStyle.Bottom
                        }

                            AddHandler btnPrint.Click, Sub()
                                                           PrintQRCode(qrImage, productName)
                                                       End Sub
                            previewForm.Controls.Add(btnPrint)

                            previewForm.ShowDialog()
                        End Using
                    End If
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Error handling row click: " & ex.Message)
        End Try
    End Sub

    ' colored cells for expiration dates (red, orange, green)
    Private Sub tableDataGridView_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles tableDataGridView.CellFormatting
        If tableDataGridView.Columns(e.ColumnIndex).Name = "expirationDate" AndAlso e.Value IsNot Nothing AndAlso e.Value IsNot DBNull.Value Then
            Dim expDate As DateTime = Convert.ToDateTime(e.Value)
            Dim today As DateTime = DateTime.Today
            Dim daysLeft As Integer = (expDate - today).Days

            ' Apply background color based on expiration status
            If expDate < today Then
                ' Already expired
                e.CellStyle.BackColor = Color.Red
                e.CellStyle.ForeColor = Color.White
            ElseIf daysLeft <= 30 Then
                ' Soon to expire (within 30 days)
                e.CellStyle.BackColor = Color.Orange
                e.CellStyle.ForeColor = Color.Black
            Else
                ' Safe (not expiring soon)
                e.CellStyle.BackColor = Color.LightGreen
                e.CellStyle.ForeColor = Color.Black
            End If
        ElseIf tableDataGridView.Columns(e.ColumnIndex).Name = "expirationDate" Then
            ' If expirationDate is NULL
            e.CellStyle.BackColor = Color.LightGray
            e.CellStyle.ForeColor = Color.Black
        End If
    End Sub

    Private qrImageToPrint As Image
    Private productNameToPrint As String
    Private numberOfCopies As Integer

    Private Sub PrintQRCode(qrImage As Image, productName As String)
        qrImageToPrint = qrImage
        productNameToPrint = productName

        Dim sizeChoice As String = InputBox("Choose paper size (Letter, Legal, A4):", "Paper Size", "Letter")

        If String.IsNullOrWhiteSpace(sizeChoice) Then
            Exit Sub
        End If

        Select Case sizeChoice.ToLower()
            Case "letter"
                printDoc.DefaultPageSettings.PaperSize = New PaperSize("Letter", 850, 1100)
            Case "legal"
                printDoc.DefaultPageSettings.PaperSize = New PaperSize("Legal", 850, 1400)
            Case "a4"
                printDoc.DefaultPageSettings.PaperSize = New PaperSize("A4", 827, 1169)
            Case Else
                MessageBox.Show("Invalid choice. Defaulting to Letter size.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information)
                printDoc.DefaultPageSettings.PaperSize = New PaperSize("Letter", 850, 1100)
        End Select

        Dim input As String = InputBox("How many copies per product?", "Copies", "1")

        If String.IsNullOrWhiteSpace(input) Then
            Exit Sub
        End If

        If Not Integer.TryParse(input, numberOfCopies) OrElse numberOfCopies <= 0 Then
            MessageBox.Show("Invalid number of copies.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Dim settings = printDoc.DefaultPageSettings
        settings.Margins = New Margins(10, 10, 10, 10)
        printDoc.DefaultPageSettings = settings

        Dim printPreview As New PrintPreviewDialog()
        printPreview.Document = printDoc
        printPreview.ShowDialog()
    End Sub

    Private Sub printDoc_PrintPage(sender As Object, e As PrintPageEventArgs) Handles printDoc.PrintPage
        If qrImageToPrint IsNot Nothing Then
            Dim qrSize As Integer = 100
            Dim margin As Integer = 20
            Dim font As New Font("Arial", 10, FontStyle.Bold)

            Dim pageWidth As Integer = e.MarginBounds.Width
            Dim pageHeight As Integer = e.MarginBounds.Height

            Dim qrPerRow As Integer = Math.Floor((pageWidth + margin) / (qrSize + margin))
            Dim qrPerCol As Integer = Math.Floor((pageHeight + margin) / (qrSize + 30 + margin))

            Dim totalPerPage As Integer = qrPerRow * qrPerCol

            Dim copiesPrinted As Integer = 0
            Dim x As Integer = e.MarginBounds.Left
            Dim y As Integer = e.MarginBounds.Top

            While copiesPrinted < numberOfCopies
                e.Graphics.DrawImage(qrImageToPrint, x, y, qrSize, qrSize)

                Dim textY As Integer = y + qrSize + 5
                e.Graphics.DrawString(productNameToPrint, font, Brushes.Black, x, textY)

                copiesPrinted += 1

                x += qrSize + margin

                If (copiesPrinted Mod qrPerRow = 0) Then
                    x = e.MarginBounds.Left
                    y += qrSize + 30 + margin
                End If

                If copiesPrinted < numberOfCopies AndAlso copiesPrinted Mod totalPerPage = 0 Then
                    e.HasMorePages = True
                    Return
                End If
            End While
        End If
    End Sub

    Private qrImagesToPrint As New List(Of Image)
    Private productNamesToPrint As New List(Of String)
    Private copiesPerProduct As Integer = 1

    Private Sub btnPrintAllQRCodes_Click(sender As Object, e As EventArgs) Handles btnPrintAllQRCodes.Click
        Dim sizeChoice As String = InputBox("Choose paper size (Letter, Legal, A4):", "Paper Size", "Letter")

        If String.IsNullOrWhiteSpace(sizeChoice) Then
            Exit Sub
        End If

        Select Case sizeChoice.ToLower()
            Case "letter"
                printDocAll.DefaultPageSettings.PaperSize = New PaperSize("Letter", 850, 1100)
            Case "legal"
                printDocAll.DefaultPageSettings.PaperSize = New PaperSize("Legal", 850, 1400)
            Case "a4"
                printDocAll.DefaultPageSettings.PaperSize = New PaperSize("A4", 827, 1169)
            Case Else
                MessageBox.Show("Invalid choice. Defaulting to Letter size.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information)
                printDocAll.DefaultPageSettings.PaperSize = New PaperSize("Letter", 850, 1100)
        End Select

        Dim input As String = InputBox("How many copies per product?", "Copies", "1")

        If String.IsNullOrWhiteSpace(input) Then
            Exit Sub
        End If

        If Not Integer.TryParse(input, copiesPerProduct) OrElse copiesPerProduct <= 0 Then
            MessageBox.Show("Invalid number of copies.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        qrImagesToPrint.Clear()
        productNamesToPrint.Clear()

        Using conn As New SqlConnection(GetConnectionString())
            conn.Open()
            Dim query As String = "
  SELECT ProductName, QRCodeImage
     FROM wholesaleProducts"

            Using cmd As New SqlCommand(query, conn)
                Using rdr As SqlDataReader = cmd.ExecuteReader()
                    While rdr.Read()
                        Dim productName As String = rdr("ProductName").ToString()

                        Dim qrBytes As Byte() = Nothing

                        If Not IsDBNull(rdr("QRCodeImage")) Then
                            qrBytes = DirectCast(rdr("QRCodeImage"), Byte())
                        End If

                        Dim qrImg As Image = Nothing
                        If qrBytes IsNot Nothing Then
                            Using ms As New MemoryStream(qrBytes)
                                qrImg = Image.FromStream(ms)
                            End Using
                        End If

                        For i As Integer = 1 To copiesPerProduct
                            qrImagesToPrint.Add(qrImg)
                            productNamesToPrint.Add(productName)
                        Next
                    End While
                End Using
            End Using
        End Using

        Dim settings = printDocAll.DefaultPageSettings
        settings.Margins = New Margins(10, 10, 10, 10)
        printDocAll.DefaultPageSettings = settings

        Dim preview As New PrintPreviewDialog()
        preview.Document = printDocAll
        preview.Width = 800
        preview.Height = 600
        preview.ShowDialog()
    End Sub

    Private currentIndex As Integer = 0

    Private Sub printDocAll_PrintPage(sender As Object, e As Printing.PrintPageEventArgs) Handles printDocAll.PrintPage
        Dim margin As Integer = 20
        Dim qrSize As Integer = 100

        Dim qrPerRow As Integer = (e.MarginBounds.Width - margin) \ (qrSize + margin)
        Dim qrPerCol As Integer = (e.MarginBounds.Height - margin) \ (qrSize + margin)
        Dim maxPerPage As Integer = qrPerRow * qrPerCol

        Dim x As Integer = e.MarginBounds.Left
        Dim y As Integer = e.MarginBounds.Top

        Dim count As Integer = 0

        While currentIndex < qrImagesToPrint.Count AndAlso count < maxPerPage
            Dim img As Image = qrImagesToPrint(currentIndex)
            Dim name As String = productNamesToPrint(currentIndex)

            If img IsNot Nothing Then
                e.Graphics.DrawImage(img, x, y, qrSize, qrSize)
            Else
                e.Graphics.DrawRectangle(Pens.Black, x, y, qrSize, qrSize)
                e.Graphics.DrawString("NO QR", New Font("Arial", 8), Brushes.Black, x + 10, y + (qrSize \ 2) - 5)
            End If

            e.Graphics.DrawString(name, New Font("Arial", 8), Brushes.Black, x, y + qrSize + 5)

            count += 1
            currentIndex += 1
            x += qrSize + margin

            If count Mod qrPerRow = 0 Then
                x = e.MarginBounds.Left
                y += qrSize + margin + 20
            End If
        End While

        If currentIndex < qrImagesToPrint.Count Then
            e.HasMorePages = True
        Else
            e.HasMorePages = False
            currentIndex = 0
        End If
    End Sub

    Private placeholders As New Dictionary(Of TextBox, String)

    Private Sub InventoryForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        HighlightButton("Button2")
        SetPlaceholder(TextBoxSearch, "Search Product Name...")
        SetRoundedRegion2(Button1, 20)
        SetRoundedRegion2(Button2, 20)
        SetRoundedRegion2(btnPrintAllQRCodes, 20)

        dt = New DataTable()
        dv = New DataView(dt)
        bs = New BindingSource()

        bs.DataSource = dv
        tableDataGridView.DataSource = bs

        LoadProducts()
    End Sub

    ''' <summary>
    ''' Override KeyDown to add ESC key for returning to summary view
    ''' </summary>
    Protected Overrides Sub OnKeyDown(e As KeyEventArgs)
        If isShowingDetailView AndAlso e.KeyCode = Keys.Escape Then
            LoadProducts()
            e.Handled = True
        End If
        MyBase.OnKeyDown(e)
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

    Private Sub SetRoundedRegion2(ctrl As Control, radius As Integer)
        Dim rect As New Rectangle(0, 0, ctrl.Width, ctrl.Height)
        Using path As GraphicsPath = GetRoundedRectanglePath2(rect, radius)
            ctrl.Region = New Region(path)
        End Using
    End Sub

    Private Function GetRoundedRectanglePath2(rect As Rectangle, radius As Integer) As GraphicsPath
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

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim popup As New addItemForm(Me)
        popup.ShowDialog(Me)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim popup As New editItemForm(Me)
        popup.ShowDialog(Me)
    End Sub

    Private Sub SetVATRate()
        Try
            Dim currentVAT As Decimal = GetCurrentVATRate()

            Dim input As String = InputBox($"Enter VAT Rate (%):{vbCrLf}Current VAT: {currentVAT:N2}%",
                        "Set VAT Rate",
              currentVAT.ToString())

            If String.IsNullOrWhiteSpace(input) Then
                Return
            End If

            Dim newVATRate As Decimal
            If Not Decimal.TryParse(input, newVATRate) Then
                MessageBox.Show("Please enter a valid numeric value for VAT rate.",
                   "Invalid Input",
               MessageBoxButtons.OK,
               MessageBoxIcon.Warning)
                Return
            End If

            If newVATRate < 0 OrElse newVATRate > 100 Then
                MessageBox.Show("VAT rate must be between 0 and 100.",
           "Invalid Range",
         MessageBoxButtons.OK,
         MessageBoxIcon.Warning)
                Return
            End If

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