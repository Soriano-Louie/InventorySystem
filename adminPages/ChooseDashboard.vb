Imports System.Drawing.Drawing2D

Public Class ChooseDashboard
    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.MaximizeBox = False
        Me.BackColor = Color.FromArgb(79, 51, 40)

        Label1.ForeColor = Color.FromArgb(79, 51, 40)

        choosePanel.BackColor = Color.FromArgb(224, 166, 109)
        'chooseTableLayoutPanel1.BackColor = Color.FromArgb(166, 166, 166)
        retailButton.BackColor = Color.FromArgb(79, 51, 40)
        wholeSaleButton.BackColor = Color.FromArgb(79, 51, 40)
        retailButton.Cursor = Cursors.Hand
        wholeSaleButton.Cursor = Cursors.Hand
        retailButton.ForeColor = Color.FromArgb(230, 216, 177)
        wholeSaleButton.ForeColor = Color.FromArgb(230, 216, 177)
        chooseTableLayoutPanel1.BackColor = Color.FromArgb(224, 166, 109)


    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CenterTableLayoutPanel()
        CenterLabelTop()
        ' Example: Make Panel1 have rounded corners with radius 30
        SetRoundedRegion2(choosePanel, 70)
        SetRoundedRegion2(retailButton, 20)
        SetRoundedRegion2(wholeSaleButton, 20)
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


    Private Sub CenterTableLayoutPanel()
        Dim parentPanel As Panel = choosePanel ' your panel
        Dim tlp As TableLayoutPanel = chooseTableLayoutPanel1 ' your TableLayoutPanel

        ' Calculate centered position
        Dim x As Integer = (parentPanel.ClientSize.Width - tlp.Width) \ 2
        Dim y As Integer = (parentPanel.ClientSize.Height - tlp.Height) \ 2

        ' Set location
        tlp.Location = New Point(Math.Max(0, x), Math.Max(0, y))
    End Sub

    Private Sub CenterLabelTop()
        Dim container As Control = Me
        Dim lbl As Label = Label1

        ' Calculate X to center label horizontally
        Dim x As Integer = (container.ClientSize.Width - lbl.Width) \ 3

        ' Y position at top (e.g., 0 or some padding)
        Dim y As Integer = 80 ' or 10 for some top margin

        lbl.Location = New Point(x, y)
    End Sub

    Private Sub wholeSaleButton_Click(sender As Object, e As EventArgs) Handles wholeSaleButton.Click
        Me.Hide()
        WholesaleDashboard.Show()
    End Sub

    Private Sub retailButton_Click(sender As Object, e As EventArgs) Handles retailButton.Click
        Me.Hide()
        retailDashboard.Show()
    End Sub
End Class