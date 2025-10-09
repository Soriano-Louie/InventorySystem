Imports System.Text
Imports Microsoft.Data.SqlClient
Imports System.Security.Cryptography

Public Class addUserForm
    Private parentForm As userManagementForm
    Public Sub New(parent As userManagementForm)
        InitializeComponent()
        Me.parentForm = parent
        Me.MaximizeBox = False
        Me.BackColor = Color.FromArgb(230, 216, 177)

        Label1.ForeColor = Color.FromArgb(79, 51, 40)

        Label1.BackColor = Color.FromArgb(224, 166, 109)


        saveButton.BackColor = Color.FromArgb(224, 166, 109)
        cancelButton.BackColor = Color.FromArgb(224, 166, 109)
        saveButton.ForeColor = Color.FromArgb(79, 51, 40)
        cancelButton.ForeColor = Color.FromArgb(79, 51, 40)
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

    Private Sub cancelButton_Click(sender As Object, e As EventArgs) Handles cancelButton.Click
        Me.Close()
    End Sub

    Private Sub LoadRoles()
        Using con As New SqlConnection(GetConnectionString())
            Dim query As String = "SELECT RoleID, RoleName FROM Roles"
            Dim da As New SqlDataAdapter(query, con)
            Dim dt As New DataTable()
            da.Fill(dt)

            ' Bind RoleName to display, RoleID as value
            userRoleDropdown.DataSource = dt
            userRoleDropdown.DisplayMember = "RoleName"
            userRoleDropdown.ValueMember = "RoleID"

            ' Enable autocomplete
            userRoleDropdown.AutoCompleteMode = AutoCompleteMode.SuggestAppend
            userRoleDropdown.AutoCompleteSource = AutoCompleteSource.ListItems
        End Using
    End Sub

    Private Sub ResetControl(ctrl As Control)
        If TypeOf ctrl Is TextBox Then
            DirectCast(ctrl, TextBox).Clear()
        ElseIf TypeOf ctrl Is ComboBox Then
            DirectCast(ctrl, ComboBox).SelectedIndex = -1
        ElseIf TypeOf ctrl Is DateTimePicker Then
            Dim picker As DateTimePicker = DirectCast(ctrl, DateTimePicker)

            ' Use today's date but respect MinDate/MaxDate
            Dim today As Date = DateTime.Today
            If today < picker.MinDate Then
                picker.Value = picker.MinDate
            ElseIf today > picker.MaxDate Then
                picker.Value = picker.MaxDate
            Else
                picker.Value = today
            End If
        End If
    End Sub

    Private Function HashSHA256Base64(password As String) As String
        Using sha256 As SHA256 = SHA256.Create()
            Dim b = Encoding.UTF8.GetBytes(password)
            Dim h = sha256.ComputeHash(b)
            Return Convert.ToBase64String(h)
        End Using
    End Function

    Private Function GetConnectionString() As String
        Return "Server=DESKTOP-3AKTMEV;Database=inventorySystem;User Id=sa;Password=24@Hakaaii07;TrustServerCertificate=True;"
    End Function

    Private Sub saveButton_Click(sender As Object, e As EventArgs) Handles saveButton.Click
        If String.IsNullOrWhiteSpace(userNameText.Text) OrElse String.IsNullOrWhiteSpace(passwordText.Text) Then
            MessageBox.Show("Please enter both username and password.")
            Return
        End If

        If userRoleDropdown.SelectedValue Is Nothing Then
            MessageBox.Show("Please select a valid role from the list.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Try
            Dim connectionString As String = GetConnectionString()
            Using connection As New SqlConnection(connectionString)
                Dim command As New SqlCommand("INSERT INTO Users (Username, PasswordHash, RoleID) VALUES (@Username, @PasswordHash, @RoleID)", connection)
                command.Parameters.AddWithValue("@Username", userNameText.Text.Trim())
                command.Parameters.AddWithValue("@PasswordHash", HashSHA256Base64(passwordText.Text.Trim()))
                command.Parameters.AddWithValue("@RoleID", Convert.ToInt32(userRoleDropdown.SelectedValue)) ' Save RoleID

                connection.Open()
                command.ExecuteNonQuery()
            End Using

            MessageBox.Show("User created successfully!")
            If parentForm IsNot Nothing Then
                parentForm.LoadUsers() ' Refresh user list in parent form
            End If

            ' Reset input fields
            ResetControl(userNameText)
            ResetControl(passwordText)
            ResetControl(userRoleDropdown)
            userNameText.Focus() ' Set focus back to username field
        Catch ex As Exception
            MessageBox.Show("Error creating user: " & ex.Message)
        End Try

    End Sub

    Private Sub UserCreationForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadRoles() ' Load roles when the form opens
    End Sub
End Class