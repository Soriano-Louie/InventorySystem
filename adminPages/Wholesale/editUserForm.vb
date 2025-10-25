Imports System.Text
Imports Microsoft.Data.SqlClient
Imports System.Security.Cryptography

Public Class editUserForm
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
        Const SC_MAXIMIZE As Integer = &HF030
        Const SC_RESTORE As Integer = &HF120

        If m.Msg = WM_SYSCOMMAND Then
            Dim command As Integer = (m.WParam.ToInt32() And &HFFF0)

            ' Block maximize
            If command = SC_MAXIMIZE Then
                Return
            End If

            ' Block restore (prevents double-click maximize)
            If command = SC_RESTORE Then
                Return
            End If
        End If

        MyBase.WndProc(m)
    End Sub

    Private Sub cancelButton_Click(sender As Object, e As EventArgs) Handles cancelButton.Click
        Close()
    End Sub

    Private Sub editUserForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        loadUserDetails()
    End Sub

    Private Function GetConnectionString() As String
        Return "Server=DESKTOP-3AKTMEV;Database=inventorySystem;User Id=sa;Password=24@Hakaaii07;TrustServerCertificate=True;"
    End Function

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

    Private Sub loadUserDetails()
        Dim query As String = "SELECT Username, userID FROM Users ORDER BY Username"
        Try
            Using conn As New SqlConnection(GetConnectionString())
                Using da As New SqlDataAdapter(query, conn)
                    Dim dt As New DataTable()
                    da.Fill(dt)

                    With userDropDown
                        .DataSource = dt
                        .DisplayMember = "Username"   ' visible to user
                        .ValueMember = "userID"   ' hidden value
                        .SelectedIndex = -1               ' optional: no default selection
                        .DropDownStyle = ComboBoxStyle.DropDown
                        .AutoCompleteMode = AutoCompleteMode.SuggestAppend
                        .AutoCompleteSource = AutoCompleteSource.ListItems
                    End With
                End Using
            End Using

        Catch ex As Exception
            MessageBox.Show("Error loading users: " & ex.Message)
        End Try
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
                    cmd.Parameters.AddWithValue("@userID", GlobalUserSession.CurrentUserID)
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


    Private Sub saveButton_Click(sender As Object, e As EventArgs) Handles saveButton.Click
        ' Prompt for admin password
        Dim adminPassword As String = InputBoxPassword("Enter admin password to confirm changes:", "Verify Password")

        ' Cancel if empty
        If String.IsNullOrWhiteSpace(adminPassword) Then
            MessageBox.Show("Update cancelled. No password entered.")
            Return
        End If

        ' Verify password
        If Not verifyPassword(adminPassword) Then
            MessageBox.Show("Invalid admin password. Update cancelled.")
            Return
        End If

        Try
            Dim query As New StringBuilder("UPDATE Users SET ")
            Dim updates As New List(Of String)
            Dim parameters As New List(Of SqlParameter)

            ' Only update Username if provided
            If Not String.IsNullOrWhiteSpace(newUserText.Text) Then
                updates.Add("Username = @Username")
                parameters.Add(New SqlParameter("@Username", newUserText.Text))
            End If

            ' Only update Password if provided
            If Not String.IsNullOrWhiteSpace(newPasswordText.Text) Then
                updates.Add("PasswordHash = @Password")
                parameters.Add(New SqlParameter("@Password", HashSHA256Base64(newPasswordText.Text)))
            End If

            ' Only update Role if a role is chosen
            If newRoleDropdown.SelectedValue IsNot Nothing Then
                updates.Add("RoleID = @RoleID")
                parameters.Add(New SqlParameter("@RoleID", newRoleDropdown.SelectedValue))
            End If

            ' Nothing to update
            If updates.Count = 0 Then
                MessageBox.Show("No changes to update.")
                Return
            End If

            ' Build final SQL
            query.Append(String.Join(", ", updates))
            query.Append(" WHERE UserID = @UserID")

            Using connection As New SqlConnection(GetConnectionString())
                Using command As New SqlCommand(query.ToString(), connection)
                    command.Parameters.AddRange(parameters.ToArray())
                    command.Parameters.AddWithValue("@UserID", userDropDown.SelectedValue)

                    connection.Open()
                    command.ExecuteNonQuery()
                    MessageBox.Show("User updated successfully.")
                End Using
            End Using

            ResetControl(userDropDown)
            ResetControl(newUserText)
            ResetControl(newPasswordText)
            ResetControl(newRoleDropdown)
            userDropDown.Focus()
            loadUserDetails() ' Refresh user list
            If parentForm IsNot Nothing Then
                parentForm.LoadUsers()
            End If
        Catch ex As Exception
            MessageBox.Show("Error updating user: " & ex.Message)
        End Try
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

    Private Sub deleteButton_Click(sender As Object, e As EventArgs) Handles deleteButton.Click
        ' prompt confirmation on deleting
        Dim result = MessageBox.Show("Are you sure you want to delete this user? This action cannot be undone.", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
        If result <> DialogResult.Yes Then
            Return
        End If
        ' Prompt for admin password
        Dim adminPassword As String = InputBoxPassword("Enter admin password to confirm changes:", "Verify Password")

        ' Cancel if empty
        If String.IsNullOrWhiteSpace(adminPassword) Then
            MessageBox.Show("Update cancelled. No password entered.")
            Return
        End If

        ' Verify password
        If Not verifyPassword(adminPassword) Then
            MessageBox.Show("Invalid admin password. Update cancelled.")
            Return
        End If
        Try
            Dim query As String = "DELETE FROM Users WHERE UserID = @UserID"
            Using connection As New SqlConnection(GetConnectionString())
                Using command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@UserID", userDropDown.SelectedValue)
                    connection.Open()
                    command.ExecuteNonQuery()
                    MessageBox.Show("User deleted successfully.")
                    If parentForm IsNot Nothing Then
                        parentForm.LoadUsers() ' Refresh user list in parent form
                    End If
                End Using
            End Using
            ResetControl(userDropDown)
            ResetControl(newUserText)
            ResetControl(newPasswordText)
            ResetControl(newRoleDropdown)
            userDropDown.Focus()
            loadUserDetails() ' Refresh user list
            If parentForm IsNot Nothing Then
                parentForm.LoadUsers()
            End If
        Catch ex As Exception
            MessageBox.Show("Error deleting user: " & ex.Message)
        End Try
    End Sub
End Class