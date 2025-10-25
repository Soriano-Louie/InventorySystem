Imports Microsoft.Data.SqlClient

Public Class editCategoryForm
    Private parentForm As categoriesForm
    Public Sub New(parent As categoriesForm)
        InitializeComponent()
        Me.parentForm = parent
        Me.MaximizeBox = False
        Me.BackColor = Color.FromArgb(230, 216, 177)

        ' Add any initialization after the InitializeComponent() call.

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

    Private Function GetConnectionString() As String
        Return "Server=DESKTOP-3AKTMEV;Database=inventorySystem;User Id=sa;Password=24@Hakaaii07;TrustServerCertificate=True;"
    End Function

    Private Sub loadCategories()
        ' Implementation for loading categories if needed
        Dim query As String = ("SELECT CategoryID, CategoryName FROM Categories ORDER BY CategoryName")
        Dim connString As String = GetConnectionString()

        Try
            Using conn As New SqlConnection(connString)
                Using da As New SqlDataAdapter(query, conn)
                    Dim dt As New DataTable()
                    da.Fill(dt)
                    With categoryDropdown
                        .DataSource = dt
                        .DisplayMember = "CategoryName"
                        .ValueMember = "CategoryID"
                        .SelectedIndex = -1 ' No selection by default
                        .DropDownStyle = ComboBoxStyle.DropDown
                        .AutoCompleteMode = AutoCompleteMode.SuggestAppend
                        .AutoCompleteSource = AutoCompleteSource.ListItems
                    End With
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading categories: " & ex.Message)
        End Try
    End Sub

    Private Sub editCategoryForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.BeginInvoke(Sub() categoryDropdown.Focus())
        loadCategories()
    End Sub

    Private Sub updateCategory()
        Try
            Dim query As String = "
                UPDATE Categories
                SET 
                    CategoryName = COALESCE(@CategoryName, CategoryName), 
                    Description = COALESCE(@Description, Description)
                WHERE CategoryID = @CategoryID;
            "

            Using conn As New SqlConnection(GetConnectionString())
                Using cmd As New SqlCommand(query, conn)
                    If String.IsNullOrWhiteSpace(categoryText.Text) Then
                        cmd.Parameters.AddWithValue("@CategoryName", DBNull.Value)
                    Else
                        cmd.Parameters.AddWithValue("@CategoryName", categoryText.Text.Trim())
                    End If
                    ' Optional: keep value if textbox empty
                    If String.IsNullOrWhiteSpace(descriptionText.Text) Then
                        cmd.Parameters.AddWithValue("@Description", DBNull.Value)
                    Else
                        cmd.Parameters.AddWithValue("@Description", descriptionText.Text.Trim())
                    End If
                    ' The selected CategoryID is required
                    cmd.Parameters.AddWithValue("@CategoryID", categoryDropdown.SelectedValue)

                    conn.Open()
                    Dim rowsAffected As Integer = cmd.ExecuteNonQuery()
                    If rowsAffected > 0 Then
                        MessageBox.Show("Category updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

                        loadCategories() ' Refresh this form's category list
                        If parentForm IsNot Nothing Then
                            parentForm.loadCategories() ' Refresh main form's category list
                        End If

                        ' Clear inputs for next entry
                        ResetControl(categoryText)
                        ResetControl(descriptionText)
                        ResetControl(categoryDropdown)
                        categoryDropdown.Focus()
                    Else
                        MessageBox.Show("No category was updated. Please check the details and try again.", "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error updating category: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub saveButton_Click(sender As Object, e As EventArgs) Handles saveButton.Click
        If String.IsNullOrWhiteSpace(categoryDropdown.SelectedValue) Then
            MessageBox.Show("Please select a category.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        updateCategory()

    End Sub

    Private Sub cancelButton_Click(sender As Object, e As EventArgs) Handles cancelButton.Click
        Me.Close()
    End Sub

    Private Sub updateButton_Click(sender As Object, e As EventArgs) Handles updateButton.Click
        Dim query As String = "DELETE FROM Categories WHERE CategoryID = @CategoryID"
        If String.IsNullOrWhiteSpace(categoryDropdown.SelectedValue) Then
            MessageBox.Show("Please select a category to delete.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        Try
            Using conn As New SqlConnection(GetConnectionString())
                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@CategoryID", categoryDropdown.SelectedValue)
                    conn.Open()
                    Dim rowsAffected As Integer = cmd.ExecuteNonQuery()
                    If rowsAffected > 0 Then
                        MessageBox.Show("Category deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        loadCategories() ' Refresh this form's category list
                        If parentForm IsNot Nothing Then
                            ParentForm.loadCategories() ' Refresh main form's category list
                        End If
                        ' Clear inputs for next entry
                        ResetControl(categoryText)
                        ResetControl(descriptionText)
                        ResetControl(categoryDropdown)
                        categoryDropdown.Focus()
                    Else
                        MessageBox.Show("No category was deleted. Please check the details and try again.", "Delete Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error deleting category: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class