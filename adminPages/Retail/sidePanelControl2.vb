Public Class sidePanelControl2
    Dim colorUnclicked As Color = Color.FromArgb(230, 216, 177)
    Dim colorClicked As Color = Color.FromArgb(102, 66, 52)

    Public Sub New()
        InitializeComponent()
        Me.BackColor = colorClicked

    End Sub

    Private Sub sidePanelControl_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        For Each btn As Button In Me.Controls.OfType(Of Button)()
            btn.Padding = New Padding(10, 0, 10, 0)
            btn.BackColor = colorClicked
            btn.ForeColor = colorUnclicked
            btn.Tag = False
            AddHandler btn.Click, AddressOf Button_Click
        Next
    End Sub

    Public Event ButtonClicked As EventHandler(Of String)

    Private Sub Button_Click(sender As Object, e As EventArgs)
        Dim btn As Button = CType(sender, Button)
        RaiseEvent ButtonClicked(Me, btn.Name)
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        chooseDashboard.Show()
        Dim hostForm As Form = Me.FindForm() ' find the form that hosts this UserControl
        If hostForm IsNot Nothing Then
            hostForm.Hide()
        End If
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        ' Confirm logout
        Dim result As DialogResult = MessageBox.Show("Are you sure you want to log out?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If result = DialogResult.Yes Then
            Application.Restart()
        End If
    End Sub

End Class