Imports System.Drawing.Drawing2D
Imports Microsoft.Data.SqlClient

Public Class userManagementForm
    Dim topPanel As New topPanelControl()
    Friend WithEvents sidePanel As sidePanelControl
    Dim colorUnclicked As Color = Color.FromArgb(191, 181, 147)
    Dim colorClicked As Color = Color.FromArgb(102, 66, 52)
    ' Dictionary to store placeholder texts for each TextBox
    Private placeholders As New Dictionary(Of TextBox, String)
    Dim bs As New BindingSource()

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

        addUserButton.BackColor = Color.FromArgb(147, 53, 53)
        addUserButton.ForeColor = Color.FromArgb(230, 216, 177)
        editUserButton.BackColor = Color.FromArgb(147, 53, 53)
        editUserButton.ForeColor = Color.FromArgb(230, 216, 177)

        TextBoxSearch.BackColor = Color.FromArgb(230, 216, 177)
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

    Private Sub ShowSingleForm(Of T As {Form, New})()
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
    End Sub

    Private Sub ChildFormClosed(sender As Object, e As FormClosedEventArgs)

        ' No need to call HighlightButton here

    End Sub

    Private Sub userSettingsForm_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        Application.Exit()
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
                ShowSingleForm(Of WholesaleDashboard)()
            Case "Button2"
                ShowSingleForm(Of InventoryForm)()
            Case "Button3"
                ShowSingleForm(Of categoriesForm)()
            Case "Button4"
                ShowSingleForm(Of deliveryLogsForm)()
            Case "Button5"
                ShowSingleForm(Of salesReport)()
            Case "Button6"
                ShowSingleForm(Of loginRecordsForm)()
            Case "Button7"
                ShowSingleForm(Of userManagementForm)()
        End Select
    End Sub

    Private Sub userManagementForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        HighlightButton("Button7")
        SetPlaceholder(TextBoxSearch, "Search...")
        SetRoundedRegion2(addUserButton, 20)
        SetRoundedRegion2(editUserButton, 20)

        LoadUsers()
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
            bs.Filter = String.Format("Name LIKE '%{0}%'", TextBoxSearch.Text.Replace("'", "''"))
        End If
    End Sub

    Private Function GetConnectionString() As String
        Return "Server=DESKTOP-3AKTMEV;Database=inventorySystem;User Id=sa;Password=24@Hakaaii07;TrustServerCertificate=True;"
    End Function

    Public Sub LoadUsers()
        Dim dt As New DataTable()
        Dim query As String = "
        SELECT u.UserID, 
               u.Username, 
               r.RoleName AS [Role], 
               u.LastLogin
        FROM Users u
        INNER JOIN Roles r ON u.RoleID = r.RoleID
        ORDER BY u.UserID
    "

        Using con As New SqlConnection(GetConnectionString())
            Using cmd As New SqlCommand(query, con)
                Using da As New SqlDataAdapter(cmd)
                    da.Fill(dt)
                End Using
            End Using
        End Using

        tableDataGridView.DataSource = dt

        ' Optional: set custom headers
        tableDataGridView.Columns("UserID").HeaderText = "User ID"
        tableDataGridView.Columns("Username").HeaderText = "Username"
        tableDataGridView.Columns("Role").HeaderText = "User Role"
        tableDataGridView.Columns("LastLogin").HeaderText = "Last Login"
    End Sub


    Private Sub addUserButton_Click(sender As Object, e As EventArgs) Handles addUserButton.Click
        Dim popup As New addUserForm
        popup.ShowDialog(Me)
    End Sub

    Private Sub editUserButton_Click(sender As Object, e As EventArgs) Handles editUserButton.Click
        Dim popup As New editUserForm
        popup.ShowDialog(Me)
    End Sub
End Class