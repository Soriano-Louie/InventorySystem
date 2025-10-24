Module GlobalUserSession
    ' Store the currently logged-in user's information
    Public CurrentUserID As Integer = 0
    Public CurrentUsername As String = ""
    Public CurrentUserRole As String = ""

    ' Method to set user session after login
    Public Sub SetUserSession(userID As Integer, username As String, role As String)
        CurrentUserID = userID
        CurrentUsername = username
        CurrentUserRole = role
    End Sub

    ' Method to clear user session on logout
    Public Sub ClearUserSession()
        CurrentUserID = 0
        CurrentUsername = ""
        CurrentUserRole = ""
    End Sub

    ' Method to check if a user is logged in
    Public Function IsUserLoggedIn() As Boolean
        Return CurrentUserID > 0
    End Function
End Module
