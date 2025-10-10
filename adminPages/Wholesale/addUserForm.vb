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

    Private Function verifyPassword(password As String) As Boolean
        Dim query As String = "SELECT passwordHash FROM Users WHERE userID = @userID"
        Try
            Using conn As New SqlConnection(GetConnectionString())
                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@userID", LoginForm.globalUserID)
                    conn.Open()
                    Dim storedHash As String = Convert.ToString(cmd.ExecuteScalar())
                    Dim inputHash As String = HashSHA256Base64(password)
                    Return storedHash = inputHash
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error verifying password: " & ex.Message)
            Return False
        End Try
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

        ' Prompt for admin password
        Dim adminPassword As String = InputBoxPassword("Enter admin password to confirm changes:", "Verify Password")

        ' Cancel if empty
        If String.IsNullOrWhiteSpace(adminPassword) Then
            MessageBox.Show("Creation cancelled. No password entered.")
            Return
        End If

        ' Verify password
        If Not verifyPassword(adminPassword) Then
            MessageBox.Show("Invalid admin password. Creation cancelled.")
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
            If parentForm IsNot Nothing Then
                parentForm.LoadUsers()
            End If
        Catch ex As Exception
            MessageBox.Show("Error creating user: " & ex.Message)
        End Try

    End Sub

    Private Sub UserCreationForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadRoles() ' Load roles when the form opens
    End Sub

    Public Function InputBoxPassword(prompt As String, title As String) As String
        ' Create a new form
        Dim frm As New Form()
        frm.Width = 350
        frm.Height = 150
        frm.Text = title
        frm.FormBorderStyle = FormBorderStyle.FixedDialog
        frm.StartPosition = FormStartPosition.CenterScreen
        frm.MinimizeBox = False
        frm.MaximizeBox = False

        ' Create a label
        Dim lbl As New Label()
        lbl.Text = prompt
        lbl.Left = 10
        lbl.Top = 10
        lbl.Width = 300
        frm.Controls.Add(lbl)

        ' Create a password TextBox
        Dim txt As New TextBox()
        txt.Left = 10
        txt.Top = 40
        txt.Width = 300
        txt.UseSystemPasswordChar = True   ' hides characters
        frm.Controls.Add(txt)

        ' Create OK and Cancel buttons
        Dim btnOK As New Button()
        btnOK.Text = "OK"
        btnOK.Left = 150
        btnOK.Top = 75
        btnOK.DialogResult = DialogResult.OK
        frm.Controls.Add(btnOK)

        Dim btnCancel As New Button()
        btnCancel.Text = "Cancel"
        btnCancel.Left = 235
        btnCancel.Top = 75
        btnCancel.DialogResult = DialogResult.Cancel
        frm.Controls.Add(btnCancel)

        frm.AcceptButton = btnOK
        frm.CancelButton = btnCancel

        ' Show dialog and return password
        If frm.ShowDialog() = DialogResult.OK Then
            Return txt.Text
        Else
            Return Nothing
        End If
    End Function
End Class