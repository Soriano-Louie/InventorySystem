Public Class topControlCashier
    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.BackColor = Color.FromArgb(79, 51, 40)
        Label1.ForeColor = Color.FromArgb(94, 175, 209)
        nameLabel.ForeColor = Color.FromArgb(230, 216, 177)
        roleLabel.ForeColor = Color.FromArgb(230, 216, 177)

        ' Set the user information from the current session
        SetUserInfo()
    End Sub

    ''' <summary>
    ''' Sets the nameLabel and roleLabel based on the logged-in user's session
    ''' </summary>
    Public Sub SetUserInfo()
        If GlobalUserSession.IsUserLoggedIn() Then
            nameLabel.Text = GlobalUserSession.CurrentUsername
            roleLabel.Text = GlobalUserSession.CurrentUserRole
        Else
            nameLabel.Text = "Not Logged In"
            roleLabel.Text = ""
        End If
    End Sub
End Class
