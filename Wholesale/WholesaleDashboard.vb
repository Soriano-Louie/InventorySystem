Imports System.Drawing.Drawing2D
Imports InventorySystem.sidePanelControl
Imports InventorySystem.topPanelControl


Public Class WholesaleDashboard
    Dim topPanel As New topPanelControl()
    Friend WithEvents sidePanel As sidePanelControl
    Dim colorUnclicked As Color = Color.FromArgb(191, 181, 147)
    Dim colorClicked As Color = Color.FromArgb(102, 66, 52)

    Public Sub New()
        InitializeComponent()
        sidePanel = New sidePanelControl()
        sidePanel.Dock = DockStyle.Left
        topPanel.Dock = DockStyle.Top
        Me.Controls.Add(topPanel)
        Me.Controls.Add(sidePanel)
        Me.MaximizeBox = False
        Me.FormBorderStyle = FormBorderStyle.None
        Me.BackColor = Color.FromArgb(224, 166, 109)

        Panel1.BackColor = Color.FromArgb(230, 216, 177)
        Panel3.BackColor = Color.FromArgb(230, 216, 177)
        Panel2.BackColor = Color.FromArgb(147, 53, 53)
        Panel2.ForeColor = Color.FromArgb(230, 216, 177)
        Panel4.BackColor = Color.FromArgb(230, 216, 177)
        Panel5.BackColor = Color.FromArgb(230, 216, 177)
        Panel6.BackColor = Color.FromArgb(230, 216, 177)
        Panel7.BackColor = Color.FromArgb(230, 216, 177)

        Label1.ForeColor = Color.FromArgb(79, 51, 40)
        Label3.ForeColor = Color.FromArgb(79, 51, 40)
        Label2.ForeColor = Color.FromArgb(230, 216, 177)
        Label4.ForeColor = Color.FromArgb(79, 51, 40)
        Label5.ForeColor = Color.FromArgb(79, 51, 40)
        Label6.ForeColor = Color.FromArgb(79, 51, 40)
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

    Private Sub WholesaleDashboard_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
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
                ShowSingleForm(Of productsForm)()
            Case "Button4"
                ShowSingleForm(Of deliveryForm)()
            Case "Button5"
                ShowSingleForm(Of salesReport)()
            Case "Button6"
                ShowSingleForm(Of logsForm)()
            Case "Button7"
                ShowSingleForm(Of userSettingsForm)()
        End Select
    End Sub

    Private Sub WholesaleDashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        HighlightButton("Button1")

        ' Example: Make Panel1 have rounded corners with radius 30
        SetRoundedRegion2(Panel1, 30)
        SetRoundedRegion2(Panel2, 30)
        SetRoundedRegion2(Panel3, 30)
        SetRoundedRegion2(Panel4, 30)
        SetRoundedRegion2(Panel5, 30)
        SetRoundedRegion2(Panel6, 30)
        SetRoundedRegion2(Panel7, 30)
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




End Class
