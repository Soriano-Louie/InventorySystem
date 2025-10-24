Imports System.Drawing.Printing
Imports System.IO
Imports Microsoft.Data.SqlClient
Imports System.Drawing.Drawing2D

Public Class inventoryRetail
    Dim topPanel As New topPanelControl()
    Friend WithEvents sidePanel As sidePanelControl2
    Dim colorUnclicked As Color = Color.FromArgb(191, 181, 147)
    Dim colorClicked As Color = Color.FromArgb(102, 66, 52)
    Dim dt As New DataTable()
    Dim dv As New DataView()
    Dim bs As New BindingSource()

    Private WithEvents printDoc As New PrintDocument
    Private WithEvents printDocAll As New PrintDocument
    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()
        sidePanel = New sidePanelControl2()
        sidePanel.Dock = DockStyle.Left
        topPanel.Dock = DockStyle.Top
        Me.Controls.Add(topPanel)
        Me.Controls.Add(sidePanel)
        Me.MaximizeBox = False
        Me.FormBorderStyle = FormBorderStyle.None
        Me.BackColor = Color.FromArgb(224, 166, 109)
        ' Add any initialization after the InitializeComponent() call.

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

    'Private Sub inventoryRetail_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
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

    Private Sub inventoryRetail_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        HighlightButton("Button2")

        SetPlaceholder(TextBoxSearch, "Search Product Name...")

        SetRoundedRegion2(Button1, 20)
        SetRoundedRegion2(Button2, 20)
        SetRoundedRegion2(btnPrintAllQRCodes, 20)

        ' Initialize
        dt = New DataTable()
        dv = New DataView(dt)
        bs = New BindingSource()

        ' Bindings
        bs.DataSource = dv
        tableDataGridView.DataSource = bs


        ' Load data
        LoadProducts()
    End Sub

    Private Sub SidePanel_ButtonClicked(sender As Object, btnName As String) Handles sidePanel.ButtonClicked
        Select Case btnName
            Case "Button1"
                Dim form = ShowSingleForm(Of retailDashboard)()
                ' Add any load method if needed
            Case "Button2"
                ShowSingleForm(Of inventoryRetail)()
            Case "Button3"
                Dim form = ShowSingleForm(Of categoriesForm)()
                ' Add any load method if needed
            Case "Button4"
                Dim form = ShowSingleForm(Of retailStockEditLogs)()
                form.loadStockEditLogs()
            Case "Button5"
                Dim form = ShowSingleForm(Of retailSalesReport)()
                ' Add any load method if needed
            Case "Button6"
                Dim form = ShowSingleForm(Of loginRecordsForm)()
                ' Add any load method if needed
            Case "Button7"
                Dim form = ShowSingleForm(Of userManagementForm)()
                ' Add any load method if needed
        End Select
    End Sub

    Private Function GetConnectionString() As String
        Return "Server=DESKTOP-3AKTMEV;Database=inventorySystem;User Id=sa;Password=24@Hakaaii07;TrustServerCertificate=True;"
    End Function

    Public Sub LoadProducts()
        Dim connString As String = GetConnectionString()
        Dim query As String = "
            SELECT p.SKU, 
                   p.productName,
                   c.CategoryName,
                   p.unit, 
                   p.retailPrice, 
                   p.cost, 
                   p.StockQuantity, 
                   p.ReorderLevel, 
                   p.expirationDate, 
                   p.QRCodeImage
            FROM retailProducts p
            INNER JOIN Categories c ON p.CategoryID = c.CategoryID
            ORDER BY 
            CASE WHEN p.expirationDate IS NULL THEN 1 ELSE 0 END, 
            p.expirationDate ASC"


        'Using conn As New SqlConnection(connString)
        '    Using cmd As New SqlCommand(query, conn)
        '        ' Create a Data Adapter
        '        Using adapter As New SqlDataAdapter(cmd)
        '            Dim dt As New DataTable()
        '            Try
        '                adapter.Fill(dt)
        '                tableDataGridView.DataSource = dt
        '            Catch ex As Exception
        '                MessageBox.Show("Error retrieving data: " & ex.Message)
        '            End Try
        '        End Using
        '    End Using
        'End Using

        Try
            Using conn As New SqlConnection(connString)
                Using da As New SqlDataAdapter(query, conn)
                    dt.Clear() ' clear old data
                    da.Fill(dt)
                End Using
            End Using

            ' add extra column for QR hex
            If Not dt.Columns.Contains("QR Code Hex (Dbl Click)") Then
                dt.Columns.Add("QR Code Hex (Dbl Click)", GetType(String))
            End If

            For Each row As DataRow In dt.Rows
                If Not IsDBNull(row("QRCodeImage")) Then
                    Dim bytes As Byte() = CType(row("QRCodeImage"), Byte())
                    row("QR Code Hex (Dbl Click)") = BitConverter.ToString(bytes).Replace("-", "")
                End If
            Next

            ' Re-bind DataView and BindingSource
            dv = New DataView(dt)
            bs.DataSource = dv
            tableDataGridView.DataSource = bs

            ' rename headers
            With tableDataGridView
                .EnableHeadersVisualStyles = False
                .ColumnHeadersDefaultCellStyle.Font = New Font(.Font, FontStyle.Bold)
                .Columns("SKU").HeaderText = "Product Code"
                .Columns("ProductName").HeaderText = "Product Name"
                .Columns("ProductName").SortMode = DataGridViewColumnSortMode.Automatic
                .Columns("CategoryName").HeaderText = "Category"
                .Columns("unit").HeaderText = "Unit"
                .Columns("retailPrice").HeaderText = "Retail Price"
                .Columns("cost").HeaderText = "Cost"
                .Columns("StockQuantity").HeaderText = "Quantity in Stock"
                .Columns("ReorderLevel").HeaderText = "Reorder Level"
                .Columns("expirationDate").HeaderText = "Expiration Date"
            End With
            tableDataGridView.Refresh()

            tableDataGridView.Columns("QRCodeImage").Visible = False
        Catch ex As Exception
            MessageBox.Show("Error loading data: " & ex.Message)
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

    'cell double click for previewing and printing QR code
    Private Sub tableDataGridView_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles tableDataGridView.CellDoubleClick
        If e.RowIndex >= 0 AndAlso e.ColumnIndex >= 0 Then
            Dim clickedColumn As String = tableDataGridView.Columns(e.ColumnIndex).Name

            If clickedColumn = "QR Code Hex (Dbl Click)" Then
                Dim hexString As String = tableDataGridView.Rows(e.RowIndex).Cells(e.ColumnIndex).Value?.ToString()

                If String.IsNullOrWhiteSpace(hexString) Then
                    MessageBox.Show("No QR code data available for this product.")
                    Exit Sub
                End If

                Try
                    ' Convert HEX string back to Byte()
                    Dim bytes As Byte() = Enumerable.Range(0, hexString.Length \ 2) _
                    .Select(Function(i) Convert.ToByte(hexString.Substring(i * 2, 2), 16)) _
                    .ToArray()

                    ' Create image from Byte()
                    Using ms As New MemoryStream(bytes)
                        Dim qrImage As Image = Image.FromStream(ms)

                        ' Get product name
                        Dim productName As String = tableDataGridView.Rows(e.RowIndex).Cells("ProductName").Value.ToString()

                        ' Show image in preview
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

                        ' Add Print button
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

                Catch ex As Exception
                    MessageBox.Show("Error decoding QR code: " & ex.Message)
                End Try
            End If
        End If
    End Sub



    Private qrImageToPrint As Image
    Private productNameToPrint As String
    Private numberOfCopies As Integer

    ' Call this when user clicks "Print QR"
    ' Ask user how many copies they want before printing
    Private Sub PrintQRCode(qrImage As Image, productName As String)
        qrImageToPrint = qrImage
        productNameToPrint = productName

        ' Ask user which paper size to use
        Dim sizeChoice As String = InputBox("Choose paper size (Letter, Legal, A4):", "Paper Size", "Letter")

        If String.IsNullOrWhiteSpace(sizeChoice) Then
            ' User cancelled
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

        ' Ask how many copies to print
        Dim input As String = InputBox("How many copies per product?", "Copies", "1")

        If String.IsNullOrWhiteSpace(input) Then
            ' User cancelled
            Exit Sub
        End If

        If Not Integer.TryParse(input, numberOfCopies) OrElse numberOfCopies <= 0 Then
            MessageBox.Show("Invalid number of copies.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' Reduce margins
        Dim settings = printDoc.DefaultPageSettings
        settings.Margins = New Margins(10, 10, 10, 10) ' 0.1 inch on all sides
        printDoc.DefaultPageSettings = settings

        ' show preview before printing
        Dim printPreview As New PrintPreviewDialog()
        printPreview.Document = printDoc
        printPreview.ShowDialog()
    End Sub


    ' Handles actual drawing on paper
    Private Sub printDoc_PrintPage(sender As Object, e As PrintPageEventArgs) Handles printDoc.PrintPage
        If qrImageToPrint IsNot Nothing Then
            Dim qrSize As Integer = 100   ' Size of each QR code
            Dim margin As Integer = 20    ' Space between QR codes
            Dim font As New Font("Arial", 10, FontStyle.Bold)

            Dim pageWidth As Integer = e.MarginBounds.Width
            Dim pageHeight As Integer = e.MarginBounds.Height

            ' Calculate how many QR codes fit per row
            Dim qrPerRow As Integer = Math.Floor((pageWidth + margin) / (qrSize + margin))
            Dim qrPerCol As Integer = Math.Floor((pageHeight + margin) / (qrSize + 30 + margin)) ' 30px for text

            Dim totalPerPage As Integer = qrPerRow * qrPerCol

            Dim copiesPrinted As Integer = 0
            Dim x As Integer = e.MarginBounds.Left
            Dim y As Integer = e.MarginBounds.Top

            While copiesPrinted < numberOfCopies
                ' Draw QR code
                e.Graphics.DrawImage(qrImageToPrint, x, y, qrSize, qrSize)

                ' Draw Product Name below QR
                Dim textY As Integer = y + qrSize + 5
                e.Graphics.DrawString(productNameToPrint, font, Brushes.Black, x, textY)

                copiesPrinted += 1

                ' Move to next position
                x += qrSize + margin

                ' If we reach end of row
                If (copiesPrinted Mod qrPerRow = 0) Then
                    x = e.MarginBounds.Left
                    y += qrSize + 30 + margin ' QR height + text + spacing
                End If

                ' If page is full, tell printer there’s more pages
                If copiesPrinted < numberOfCopies AndAlso copiesPrinted Mod totalPerPage = 0 Then
                    e.HasMorePages = True
                    Return
                End If
            End While
        End If
    End Sub

    ' PRINTING FOR ALL QR CODES 
    ' Store data to print
    Private qrImagesToPrint As New List(Of Image)
    Private productNamesToPrint As New List(Of String)

    ' Copies for each QR
    Private copiesPerProduct As Integer = 1

    Private Sub btnPrintAllQRCodes_Click(sender As Object, e As EventArgs) Handles btnPrintAllQRCodes.Click
        ' 1. Ask paper size
        ' Ask user which paper size to use
        Dim sizeChoice As String = InputBox("Choose paper size (Letter, Legal, A4):", "Paper Size", "Letter")

        If String.IsNullOrWhiteSpace(sizeChoice) Then
            ' User cancelled
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

        ' 2. Ask copies
        Dim input As String = InputBox("How many copies per product?", "Copies", "1")

        If String.IsNullOrWhiteSpace(input) Then
            ' User cancelled
            Exit Sub
        End If

        If Not Integer.TryParse(input, copiesPerProduct) OrElse copiesPerProduct <= 0 Then
            MessageBox.Show("Invalid number of copies.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' 3. Load products & QRs from database
        qrImagesToPrint.Clear()
        productNamesToPrint.Clear()

        Using conn As New SqlConnection(GetConnectionString())
            conn.Open()
            Dim query As String = "SELECT ProductName, QRCodeImage FROM retailProducts"

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

                        ' Store the product name + image (Nothing means no QR)
                        For i As Integer = 1 To copiesPerProduct
                            qrImagesToPrint.Add(qrImg) ' Can be Nothing
                            productNamesToPrint.Add(productName)
                        Next
                    End While
                End Using
            End Using
        End Using

        ' Reduce margins
        Dim settings = printDocAll.DefaultPageSettings
        settings.Margins = New Margins(10, 10, 10, 10) ' 0.1 inch on all sides
        printDocAll.DefaultPageSettings = settings

        ' 4. Show preview before printing
        Dim preview As New PrintPreviewDialog()
        preview.Document = printDocAll
        preview.Width = 800
        preview.Height = 600
        preview.ShowDialog()
    End Sub

    Private printIndex As Integer = 0

    Private currentIndex As Integer = 0 ' Track across pages

    Private Sub printDocAll_PrintPage(sender As Object, e As Printing.PrintPageEventArgs) Handles printDocAll.PrintPage
        Dim margin As Integer = 20
        Dim qrSize As Integer = 100

        ' Grid layout calculation
        Dim qrPerRow As Integer = (e.MarginBounds.Width - margin) \ (qrSize + margin)
        Dim qrPerCol As Integer = (e.MarginBounds.Height - margin) \ (qrSize + margin)
        Dim maxPerPage As Integer = qrPerRow * qrPerCol

        Dim x As Integer = e.MarginBounds.Left
        Dim y As Integer = e.MarginBounds.Top

        Dim count As Integer = 0

        While currentIndex < qrImagesToPrint.Count AndAlso count < maxPerPage
            Dim img As Image = qrImagesToPrint(currentIndex)
            Dim name As String = productNamesToPrint(currentIndex)

            ' Draw QR or placeholder
            If img IsNot Nothing Then
                e.Graphics.DrawImage(img, x, y, qrSize, qrSize)
            Else
                e.Graphics.DrawRectangle(Pens.Black, x, y, qrSize, qrSize)
                e.Graphics.DrawString("NO QR", New Font("Arial", 8), Brushes.Black, x + 10, y + (qrSize \ 2) - 5)
            End If

            ' Print product name
            e.Graphics.DrawString(name, New Font("Arial", 8), Brushes.Black, x, y + qrSize + 5)

            ' Move to next cell
            count += 1
            currentIndex += 1
            x += qrSize + margin

            If count Mod qrPerRow = 0 Then
                x = e.MarginBounds.Left
                y += qrSize + margin + 20
            End If
        End While

        ' Check if more pages are needed
        If currentIndex < qrImagesToPrint.Count Then
            e.HasMorePages = True
        Else
            e.HasMorePages = False
            currentIndex = 0 ' Reset for next print job
        End If
    End Sub

    ' Dictionary to store placeholder texts for each TextBox
    Private placeholders As New Dictionary(Of TextBox, String)

    ' Method to assign placeholder text to a TextBox
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

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim popup As New addItemRetail(Me)
        popup.ShowDialog(Me)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim popup As New editItemRetail(Me)
        popup.ShowDialog(Me)
    End Sub
End Class