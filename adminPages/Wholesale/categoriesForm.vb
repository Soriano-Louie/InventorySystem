Imports System.Drawing.Drawing2D
Imports Microsoft.Data.SqlClient

Public Class categoriesForm
    Dim topPanel As New topPanelControl()
    Friend WithEvents sidePanel As sidePanelControl
    Friend WithEvents sidePanel2 As sidePanelControl2
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
        'sidePanel = New sidePanelControl()
        'sidePanel.Dock = DockStyle.Left
        'topPanel.Dock = DockStyle.Top
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

    Private Sub categoriesForm_Closed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        chooseDashboard.globalPage = ""
    End Sub

    Private Sub HighlightButton(buttonName As String)
        ' Reset all buttons
        If sidePanel IsNot Nothing Then
            For Each ctrl As Control In sidePanel.Controls
                If TypeOf ctrl Is Button Then
                    ctrl.BackColor = colorClicked
                    ctrl.ForeColor = colorUnclicked
                End If
            Next
        ElseIf sidePanel2 IsNot Nothing Then
            For Each ctrl As Control In sidePanel2.Controls
                If TypeOf ctrl Is Button Then
                    ctrl.BackColor = colorClicked
                    ctrl.ForeColor = colorUnclicked
                End If
            Next
        End If


        ' Highlight the selected button (for whichever panel is active)
        Dim btn As Button = Nothing
        If sidePanel IsNot Nothing Then
            btn = sidePanel.Controls.OfType(Of Button)().FirstOrDefault(Function(b) b.Name = buttonName)
        ElseIf sidePanel2 IsNot Nothing Then
            btn = sidePanel2.Controls.OfType(Of Button)().FirstOrDefault(Function(b) b.Name = buttonName)
        End If

        ' Apply highlight colors
        If btn IsNot Nothing Then
            btn.BackColor = colorUnclicked
            btn.ForeColor = Color.FromArgb(79, 51, 40)
        End If
    End Sub

    Private Sub SidePanel_ButtonClicked(sender As Object, btnName As String) Handles sidePanel.ButtonClicked
        If chooseDashboard.globalPage.ToLower() = "wholesale" Then
            Select Case btnName
                Case "Button1"
                    Dim form = ShowSingleForm(Of wholesaleDashboard)()
                    form.LoadDashboardData()
                Case "Button2"
                    Dim form = ShowSingleForm(Of InventoryForm)()
                    form.LoadProducts()
                Case "Button3"
                    ShowSingleForm(Of categoriesForm)()
                Case "Button4"
                    ShowSingleForm(Of deliveryLogsForm)()
                Case "Button5"
                    Dim form = ShowSingleForm(Of salesReport)()
                    form.loadSalesReport()
                Case "Button6"
                    Dim form = ShowSingleForm(Of loginRecordsForm)()
                    form.LoadLoginHistory()
                Case "Button7"
                    Dim form = ShowSingleForm(Of userManagementForm)()
                    form.LoadUsers()
            End Select
        ElseIf chooseDashboard.globalPage.ToLower() = "retail" Then
            Select Case btnName
                Case "Button1"
                    Dim form = ShowSingleForm(Of retailDashboard)()
                    form.LoadDashboardData()
                Case "Button2"
                    ShowSingleForm(Of inventoryRetail)()
                Case "Button3"
                    ShowSingleForm(Of categoriesForm)()
                Case "Button4"

                Case "Button5"
                    ShowSingleForm(Of retailSalesReport)()
                Case "Button6"
                    Dim form = ShowSingleForm(Of loginRecordsForm)()
                    form.LoadLoginHistory()
                Case "Button7"
                    Dim form = ShowSingleForm(Of userManagementForm)()
                    form.LoadUsers()
            End Select
        End If
    End Sub

    Private Sub categoriesForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If chooseDashboard.globalPage.ToLower() = "wholesale" Then
            topPanel.Label1.Text = "WHOLESALE"
            sidePanel = New sidePanelControl()
            sidePanel.Dock = DockStyle.Left
            topPanel.Dock = DockStyle.Top
            Me.Controls.Add(topPanel)
            Me.Controls.Add(sidePanel)

            ' Attach event handler  
            AddHandler sidePanel.ButtonClicked, AddressOf SidePanel_ButtonClicked

        ElseIf chooseDashboard.globalPage.ToLower() = "retail" Then
            topPanel.Label1.Text = "RETAIL"
            sidePanel2 = New sidePanelControl2()
            sidePanel2.Dock = DockStyle.Left
            topPanel.Dock = DockStyle.Top
            Me.Controls.Add(topPanel)
            Me.Controls.Add(sidePanel2)

            ' Attach event handler for sidePanel2
            AddHandler sidePanel2.ButtonClicked, AddressOf SidePanel_ButtonClicked
        End If
        HighlightButton("Button3")

        SetPlaceholder(TextBoxSearch, "Search Category Name...")

        SetRoundedRegion2(Button1, 20)
        SetRoundedRegion2(Button2, 20)

        ' Initialize
        dt = New DataTable()
        dv = New DataView(dt)
        bs = New BindingSource()

        ' Bindings
        bs.DataSource = dv
        tableDataGridView.DataSource = bs

        loadCategories()
    End Sub

    Private Function GetConnectionString() As String
        Return "Server=DESKTOP-3AKTMEV;Database=inventorySystem;User Id=sa;Password=24@Hakaaii07;TrustServerCertificate=True;"
    End Function

    Public Sub loadCategories()
        Dim connString As String = GetConnectionString()
        Dim query As String = "
            SELECT CategoryName, Description FROM Categories"
        Try
            Using conn As New SqlConnection(connString)
                Using da As New SqlDataAdapter(query, conn)
                    dt.Clear() ' clear old data
                    da.Fill(dt)
                End Using
            End Using

            tableDataGridView.AutoGenerateColumns = True
            tableDataGridView.Refresh()

            With tableDataGridView
                .EnableHeadersVisualStyles = False ' allow custom styling
                .ColumnHeadersDefaultCellStyle.Font = New Font(.Font, FontStyle.Bold)
                If .Columns.Contains("CategoryName") Then
                    .Columns("CategoryName").HeaderText = "Category"
                End If

                If .Columns.Contains("Description") Then
                    .Columns("Description").HeaderText = "Description"
                End If
            End With
        Catch ex As Exception
            MessageBox.Show("Error loading categories: " & ex.Message)
        End Try
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
            bs.Filter = String.Format("CategoryName LIKE '%{0}%'", TextBoxSearch.Text.Replace("'", "''"))
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim popup As New addCategoryForm(Me)
        popup.ShowDialog(Me)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim popup As New editCategoryForm(Me)
        popup.ShowDialog(Me)
    End Sub

End Class