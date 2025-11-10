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
        ' Ensure a category is selected
        If String.IsNullOrWhiteSpace(categoryDropdown.SelectedValue) Then
            MessageBox.Show("Please select a category to delete.", "No Category Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim categoryID As Integer = Convert.ToInt32(categoryDropdown.SelectedValue)
        Dim categoryName As String = categoryDropdown.Text

        ' Check if category is being used before attempting deletion
        Dim usageInfo As CategoryUsageInfo = CheckCategoryUsage(categoryID)

        If usageInfo.IsInUse Then
            ' Category is being used, show detailed error message
            Dim usageMessage As String = $"Cannot delete category '{categoryName}'!" & vbCrLf & vbCrLf &
       "This category is currently being used by:" & vbCrLf

            If usageInfo.WholesaleProductCount > 0 Then
                usageMessage &= $"  • {usageInfo.WholesaleProductCount} Wholesale Product(s)" & vbCrLf
            End If

            If usageInfo.RetailProductCount > 0 Then
                usageMessage &= $"  • {usageInfo.RetailProductCount} Retail Product(s)" & vbCrLf
            End If

            If usageInfo.SalesReportCount > 0 Then
                usageMessage &= $"  • {usageInfo.SalesReportCount} Sales Record(s)" & vbCrLf
            End If

            If usageInfo.RetailSalesReportCount > 0 Then
                usageMessage &= $"  • {usageInfo.RetailSalesReportCount} Retail Sales Record(s)" & vbCrLf
            End If

            usageMessage &= vbCrLf &
     "To delete this category, you must first:" & vbCrLf &
   "1. Remove or reassign all products in this category" & vbCrLf &
       "2. Update historical sales records to a different category" & vbCrLf & vbCrLf &
    "Alternatively, consider creating a new category instead."

            MessageBox.Show(usageMessage,
      "Category In Use - Cannot Delete",
      MessageBoxButtons.OK,
   MessageBoxIcon.Warning)
            Exit Sub
        End If

        ' Show deletion warning with category details
        Dim categoryDetails As String = GetCategoryDetailsForDeletion(categoryID)

        Dim warningMessage As String = $"⚠️ WARNING: You are about to permanently delete this category!" & vbCrLf & vbCrLf &
          $"Category: {categoryName}" & vbCrLf &
    categoryDetails & vbCrLf &
 "This action CANNOT be undone!" & vbCrLf & vbCrLf &
  "Are you sure you want to DELETE this category?"

        ' Confirm deletion
        Dim confirm = MessageBox.Show(warningMessage,
   "⚠️ CONFIRM CATEGORY DELETION",
     MessageBoxButtons.YesNo,
      MessageBoxIcon.Warning,
     MessageBoxDefaultButton.Button2) ' Default to "No"

        If confirm <> DialogResult.Yes Then
            MessageBox.Show("Deletion cancelled.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        ' Proceed with deletion
        Dim query As String = "DELETE FROM Categories WHERE CategoryID = @CategoryID"
        Try
            Using conn As New SqlConnection(GetConnectionString())
                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@CategoryID", categoryID)
                    conn.Open()
                    Dim rowsAffected As Integer = cmd.ExecuteNonQuery()

                    If rowsAffected > 0 Then
                        MessageBox.Show($"Category '{categoryName}' has been successfully deleted.",
      "✓ Deletion Successful",
 MessageBoxButtons.OK,
        MessageBoxIcon.Information)

                        ' Reset form controls and refresh data
                        ResetControl(categoryText)
                        ResetControl(descriptionText)
                        ResetControl(categoryDropdown)
                        categoryDropdown.Focus()

                        loadCategories()
                        If parentForm IsNot Nothing Then
                            parentForm.loadCategories()
                        End If
                    Else
                        MessageBox.Show("No category was deleted. Please check the details and try again.",
        "Delete Failed",
     MessageBoxButtons.OK,
  MessageBoxIcon.Warning)
                    End If
                End Using
            End Using
        Catch ex As SqlException
            ' Handle specific SQL errors
            If ex.Number = 547 Then ' Foreign key constraint violation
                MessageBox.Show($"Cannot delete category '{categoryName}' because it is being used by products or sales records." & vbCrLf & vbCrLf &
     "Please remove or reassign all associated items before deleting this category.",
     "Category In Use",
       MessageBoxButtons.OK,
   MessageBoxIcon.Error)
            Else
                MessageBox.Show($"Database error deleting category:{vbCrLf}{vbCrLf}" &
           $"Error: {ex.Message}" & vbCrLf & vbCrLf &
     "Please contact system administrator if this problem persists.",
       "Database Error",
    MessageBoxButtons.OK,
        MessageBoxIcon.Error)
            End If
        Catch ex As Exception
            MessageBox.Show($"Error deleting category:{vbCrLf}{vbCrLf}" &
 $"Error: {ex.Message}",
     "Error",
   MessageBoxButtons.OK,
       MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' Class to store category usage information
    ''' </summary>
    Private Class CategoryUsageInfo
        Public Property IsInUse As Boolean
        Public Property WholesaleProductCount As Integer
        Public Property RetailProductCount As Integer
        Public Property SalesReportCount As Integer
        Public Property RetailSalesReportCount As Integer
    End Class

    ''' <summary>
    ''' Checks if a category is being used in any tables
    ''' </summary>
    Private Function CheckCategoryUsage(categoryID As Integer) As CategoryUsageInfo
        Dim usageInfo As New CategoryUsageInfo()

        Try
            Using conn As New SqlConnection(GetConnectionString())
                conn.Open()

                ' Check wholesale products
                Dim wholesaleQuery As String = "SELECT COUNT(*) FROM wholesaleProducts WHERE CategoryID = @CategoryID"
                Using cmd As New SqlCommand(wholesaleQuery, conn)
                    cmd.Parameters.AddWithValue("@CategoryID", categoryID)
                    usageInfo.WholesaleProductCount = Convert.ToInt32(cmd.ExecuteScalar())
                End Using

                ' Check retail products
                Dim retailQuery As String = "SELECT COUNT(*) FROM retailProducts WHERE CategoryID = @CategoryID"
                Using cmd As New SqlCommand(retailQuery, conn)
                    cmd.Parameters.AddWithValue("@CategoryID", categoryID)
                    usageInfo.RetailProductCount = Convert.ToInt32(cmd.ExecuteScalar())
                End Using

                ' Check sales report
                Dim salesQuery As String = "SELECT COUNT(*) FROM SalesReport WHERE CategoryID = @CategoryID"
                Using cmd As New SqlCommand(salesQuery, conn)
                    cmd.Parameters.AddWithValue("@CategoryID", categoryID)
                    usageInfo.SalesReportCount = Convert.ToInt32(cmd.ExecuteScalar())
                End Using

                ' Check retail sales report
                Dim retailSalesQuery As String = "SELECT COUNT(*) FROM RetailSalesReport WHERE CategoryID = @CategoryID"
                Using cmd As New SqlCommand(retailSalesQuery, conn)
                    cmd.Parameters.AddWithValue("@CategoryID", categoryID)
                    usageInfo.RetailSalesReportCount = Convert.ToInt32(cmd.ExecuteScalar())
                End Using
            End Using

            ' Determine if category is in use
            usageInfo.IsInUse = (usageInfo.WholesaleProductCount > 0 OrElse
    usageInfo.RetailProductCount > 0 OrElse
     usageInfo.SalesReportCount > 0 OrElse
 usageInfo.RetailSalesReportCount > 0)
        Catch ex As Exception
            MessageBox.Show("Error checking category usage: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            usageInfo.IsInUse = True ' Assume in use to prevent deletion on error
        End Try

        Return usageInfo
    End Function

    ''' <summary>
    ''' Gets category details for the deletion warning message
    ''' </summary>
    Private Function GetCategoryDetailsForDeletion(categoryID As Integer) As String
        Dim details As String = ""

        Try
            Using conn As New SqlConnection(GetConnectionString())
                conn.Open()
                Dim query As String = "SELECT Description FROM Categories WHERE CategoryID = @CategoryID"

                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@CategoryID", categoryID)

                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            Dim description As String = If(IsDBNull(reader("Description")), "No description", reader("Description").ToString())
                            details = $"Description: {description}"
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            details = "(Unable to load category details)"
        End Try

        Return details
    End Function

End Class