Imports Microsoft.Data.SqlClient

Public Class addCategoryForm
    Private parentForm As categoriesForm
    Public Sub New(parent As categoriesForm)
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

    Private Sub addCategoryForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.BeginInvoke(Sub() categoryText.Focus())
    End Sub

    Private Sub cancelButton_Click(sender As Object, e As EventArgs) Handles cancelButton.Click
        Me.Close()
    End Sub

    Private Function GetConnectionString() As String
        Return "Server=DESKTOP-3AKTMEV;Database=inventorySystem;User Id=sa;Password=24@Hakaaii07;TrustServerCertificate=True;"
    End Function



    Private Sub saveButton_Click(sender As Object, e As EventArgs) Handles saveButton.Click
        If String.IsNullOrWhiteSpace(categoryText.Text) Then
            MessageBox.Show("Please enter a category name.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Try
            Dim query As String = "
                INSERT INTO Categories (CategoryName, Description)
                VALUES (@CategoryName, @Description);
            "

            Using conn As New SqlConnection(GetConnectionString())
                conn.Open()
                Using cmd As New SqlCommand(query, conn)
                    ' Always required
                    cmd.Parameters.AddWithValue("@CategoryName", categoryText.Text.Trim())

                    ' Optional: insert NULL if textbox empty
                    If String.IsNullOrWhiteSpace(descriptionText.Text) Then
                        cmd.Parameters.AddWithValue("@Description", DBNull.Value)
                    Else
                        cmd.Parameters.AddWithValue("@Description", descriptionText.Text.Trim())
                    End If

                    cmd.ExecuteNonQuery()
                    MessageBox.Show("Category added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

                    If parentForm IsNot Nothing Then
                        parentForm.loadCategories() ' Refresh parent form's data
                    End If
                End Using
            End Using
            ' Clear inputs for next entry
            ResetControl(categoryText)
            ResetControl(descriptionText)
            categoryText.Focus()

        Catch ex As Exception
            MessageBox.Show("Error adding category: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class