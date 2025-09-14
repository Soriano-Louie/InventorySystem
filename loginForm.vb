Imports System.Drawing.Drawing2D
Imports System.Security.Cryptography
Imports System.Text
Imports System.Text.RegularExpressions
Imports Microsoft.Data.SqlClient

Public Class LoginForm
    'Private Sub loginForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    '    brownPanel.BackColor = Color.FromArgb(79, 51, 40) ' Custom purple color
    'End Sub

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.BackColor = Color.FromArgb(230, 216, 177)
        Me.MaximizeBox = False
        brownPanel.BackColor = Color.FromArgb(79, 51, 40)
        SetRoundedRegion(brownPanel, 30)
        projectTitle.AutoSize = True
        projectTitle.Margin = New Padding(0)
        projectTitle.Padding = New Padding(0)
        projectTitle.BackColor = Color.Transparent
        projectTitle.TextAlign = ContentAlignment.TopLeft
        projectTitle.ForeColor = Color.FromArgb(79, 51, 40)
        projectTitle2.ForeColor = Color.FromArgb(79, 51, 40)
        userLabel.ForeColor = Color.FromArgb(79, 51, 40)
        passwordLabel.ForeColor = Color.FromArgb(79, 51, 40)
        loginButton.BackColor = Color.FromArgb(79, 51, 40)
        loginButton.ForeColor = Color.White
        loginButton.FlatAppearance.BorderSize = 0
        loginButton.Cursor = Cursors.Hand

        psswrdTxtBx.PasswordChar = "*"

        Panel1.BackColor = Color.FromArgb(224, 166, 109)
        Panel2.BackColor = Color.FromArgb(224, 166, 109)
        Panel3.BackColor = Color.FromArgb(224, 166, 109)
        Panel5.BackColor = Color.FromArgb(224, 166, 109)
        Panel8.BackColor = Color.FromArgb(224, 166, 109)
        Panel9.BackColor = Color.FromArgb(224, 166, 109)
        Panel11.BackColor = Color.FromArgb(224, 166, 109)
        Panel14.BackColor = Color.FromArgb(224, 166, 109)
        Panel17.BackColor = Color.FromArgb(224, 166, 109)
        Panel20.BackColor = Color.FromArgb(224, 166, 109)
        Panel23.BackColor = Color.FromArgb(224, 166, 109)
        Panel26.BackColor = Color.FromArgb(224, 166, 109)
        Panel29.BackColor = Color.FromArgb(224, 166, 109)
        Panel32.BackColor = Color.FromArgb(224, 166, 109)
        Panel35.BackColor = Color.FromArgb(224, 166, 109)
        Panel38.BackColor = Color.FromArgb(224, 166, 109)
        Panel41.BackColor = Color.FromArgb(224, 166, 109)
        Panel44.BackColor = Color.FromArgb(224, 166, 109)
        Panel47.BackColor = Color.FromArgb(224, 166, 109)
        Panel50.BackColor = Color.FromArgb(224, 166, 109)
        Panel53.BackColor = Color.FromArgb(224, 166, 109)
        Panel56.BackColor = Color.FromArgb(224, 166, 109)
        Panel59.BackColor = Color.FromArgb(224, 166, 109)
        Panel62.BackColor = Color.FromArgb(224, 166, 109)
        Panel65.BackColor = Color.FromArgb(224, 166, 109)
        Panel68.BackColor = Color.FromArgb(224, 166, 109)
        formPanel.BackColor = Color.FromArgb(224, 166, 109)


    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Example: Make Panel1 have rounded corners with radius 30
        SetRoundedRegion(brownPanel, 70)
        SetRoundedRegion2(formPanel, 70)
    End Sub

    Private Sub SetRoundedRegion(ctrl As Control, radius As Integer)
        Dim rect As New Rectangle(0, 0, ctrl.Width, ctrl.Height)
        Using path As GraphicsPath = GetRoundedRectanglePath(rect, radius)
            ctrl.Region = New Region(path)
        End Using
    End Sub

    Private Sub SetRoundedRegion2(ctrl As Control, radius As Integer)
        Dim rect As New Rectangle(0, 0, ctrl.Width, ctrl.Height)
        Using path As GraphicsPath = GetRoundedRectanglePath2(rect, radius)
            ctrl.Region = New Region(path)
        End Using
    End Sub

    Private Function GetRoundedRectanglePath(rect As Rectangle, radius As Integer) As GraphicsPath
        Dim path As New GraphicsPath()
        Dim diameter As Integer = radius * 2

        path.StartFigure()

        ' Top edge (left to right - straight line)
        path.AddLine(rect.X, rect.Y, rect.Right - radius, rect.Y)

        ' Top-right corner (arc)
        path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90)

        ' Right edge (top to bottom - straight line)
        path.AddLine(rect.Right, rect.Y + radius, rect.Right, rect.Bottom - radius)

        ' Bottom-right corner (arc)
        path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90)

        ' Bottom edge (right to left - straight line)
        path.AddLine(rect.Right - radius, rect.Bottom, rect.X, rect.Bottom)

        ' Left edge (bottom to top - straight line, no rounding)
        path.AddLine(rect.X, rect.Bottom, rect.X, rect.Y)

        path.CloseFigure()
        Return path
    End Function

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


    ' Function to hash password with SHA256
    Private Function GetConnectionString() As String
        Return "Server=DESKTOP-3AKTMEV;Database=inventorySystem;User Id=sa;Password=24@Hakaaii07;TrustServerCertificate=True;"
    End Function


    Private Function HashSHA256Base64(password As String) As String
        Using sha256 As SHA256 = SHA256.Create()
            Dim b = Encoding.UTF8.GetBytes(password)
            Dim h = sha256.ComputeHash(b)
            Return Convert.ToBase64String(h)
        End Using
    End Function

    Private Function IsStoredValueHashed(stored As String) As Boolean
        If String.IsNullOrEmpty(stored) Then Return False
        stored = stored.Trim()

        ' check if it is a valid Base64 string, and its byte length matches what a SHA256 hash is (32 bytes)
        Dim base64Pattern As String = "^[A-Za-z0-9\+/]+={0,2}$"
        If Regex.IsMatch(stored, base64Pattern) Then
            Try
                Dim decoded = Convert.FromBase64String(stored)
                If decoded.Length = 32 Then
                    Return True
                End If
            Catch ex As Exception
                ' not valid Base64 or length wrong
            End Try
        End If

        Return False
    End Function

    Private Sub UpdatePasswordHashInDb(username As String, newHash As String)
        Using conn As New SqlConnection(GetConnectionString())
            conn.Open()
            Dim sql As String = "UPDATE users SET password = @ph WHERE username = @u"
            Using cmd As New SqlCommand(sql, conn)
                cmd.Parameters.Add("@ph", SqlDbType.NVarChar, -1).Value = newHash
                cmd.Parameters.Add("@u", SqlDbType.NVarChar, 50).Value = username
                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Private Sub LoginButton_Click(sender As Object, e As EventArgs) Handles loginButton.Click
        Dim username As String = usrnmTxtBx.Text.Trim()
        Dim password As String = psswrdTxtBx.Text

        If String.IsNullOrEmpty(username) OrElse String.IsNullOrEmpty(password) Then
            MessageBox.Show("Enter username and password.")
            Return
        End If

        Dim connStr = GetConnectionString()
        Dim storedObj As Object = Nothing
        Dim stored As String = ""

        Try
            Using conn As New SqlConnection(connStr)
                conn.Open()
                Using cmd As New SqlCommand("SELECT password FROM users WHERE username = @u", conn)
                    cmd.Parameters.Add("@u", SqlDbType.NVarChar, 50).Value = username
                    storedObj = cmd.ExecuteScalar()
                End Using
            End Using

            If storedObj Is Nothing OrElse storedObj Is DBNull.Value Then
                MessageBox.Show("Username or password incorrect.")
                Return
            End If

            stored = storedObj.ToString()

            ' Case 1: stored is already a hash
            If IsStoredValueHashed(stored) Then
                Dim hashedInput As String = HashSHA256Base64(password)
                If hashedInput = stored Then
                    MessageBox.Show("Login successful!")
                    Me.Hide()
                    chooseDashboard.Show()
                Else
                    MessageBox.Show("Username or password incorrect.")
                End If

            Else
                ' Case 2: stored is plaintext
                If stored = password Then
                    ' successful login → now hash and save
                    Dim newHash = HashSHA256Base64(password)
                    UpdatePasswordHashInDb(username, newHash)
                    MessageBox.Show("Login successful! Password was hashed for security.")
                    Me.Hide()
                    chooseDashboard.Show()
                Else
                    MessageBox.Show("Username or password incorrect.")
                End If
            End If

        Catch ex As Exception
            MessageBox.Show("Error connecting to database: " & ex.Message)
        End Try

    End Sub
End Class
