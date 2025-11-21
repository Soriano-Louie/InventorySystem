Imports Microsoft.Data.SqlClient

Public Class discountForm

    Private _productID As String
    Private _productName As String
    Public Sub New(productID As Integer, productName As String)
        InitializeComponent()
        Me._productID = productID
        Me._productName = productName

        Me.MaximizeBox = False
        Me.BackColor = Color.FromArgb(230, 216, 177)

        Label1.BackColor = Color.FromArgb(224, 166, 109)
        TableLayoutPanel1.BackColor = Color.FromArgb(230, 216, 177)

        saveButton.BackColor = Color.FromArgb(224, 166, 109)
        saveButton.ForeColor = Color.FromArgb(79, 51, 40)
        dgvDiscounts.BackgroundColor = Color.FromArgb(230, 216, 177)
        dgvDiscounts.Columns.Add("MinSacks", "Min Sacks")
        dgvDiscounts.Columns.Add("MaxSacks", "Max Sacks")
        dgvDiscounts.Columns.Add("DiscountPrice", "Discount Price")

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

    Private Sub discountForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Show product name on form title or label
        Dim title As String = "Manage Discounts - " & _productName

        Label1.Text = title

        ' Load existing discounts for this product
        LoadDiscounts(_productID)
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

    Private Sub LoadDiscounts(productID As Integer)
        Try
            ' Clear old rows to prevent duplication
            dgvDiscounts.Rows.Clear()

            Dim query As String = "SELECT MinSacks, MaxSacks, DiscountPrice 
                               FROM ProductDiscounts 
                               WHERE ProductID = @ProductID"
            Using conn As New SqlConnection(GetConnectionString())
                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@ProductID", productID)

                    conn.Open()
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            Dim minSacks As Integer = reader("MinSacks")
                            Dim maxSacks As Integer = reader("MaxSacks")
                            Dim discountPrice As Decimal = reader("DiscountPrice")

                            ' Populate DataGridView
                            dgvDiscounts.Rows.Add(minSacks, maxSacks, discountPrice)
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading discounts: " & ex.Message)
        End Try
    End Sub


    Private Sub saveButton_Click(sender As Object, e As EventArgs) Handles saveButton.Click
        If String.IsNullOrWhiteSpace(txtMinSacks.Text) OrElse String.IsNullOrWhiteSpace(txtDiscountPrice.Text) OrElse String.IsNullOrWhiteSpace(txtMaxSacks.Text) Then
            MessageBox.Show("Please fill in all fields.")
            Return
        End If

        Try
            Dim query = "INSERT INTO ProductDiscounts (ProductID, MinSacks, DiscountPrice, MaxSacks) VALUES (@ProductID, @MinSacks, @DiscountPrice, @MaxSacks)"
            Using conn As New SqlConnection(GetConnectionString)
                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@ProductID", _productID)
                    cmd.Parameters.AddWithValue("@MinSacks", Convert.ToInt32(txtMinSacks.Text))
                    cmd.Parameters.AddWithValue("@MaxSacks", Convert.ToInt32(txtMaxSacks.Text))
                    cmd.Parameters.AddWithValue("@DiscountPrice", Convert.ToDecimal(txtDiscountPrice.Text))

                    conn.Open
                    cmd.ExecuteNonQuery
                End Using
            End Using

            MessageBox.Show("Discount added successfully!")
            ' Clear input fields
            ResetControl(txtMinSacks)
            ResetControl(txtMaxSacks)
            ResetControl(txtDiscountPrice)
            LoadDiscounts(_productID) ' refresh DataGridView
        Catch ex As Exception
            MessageBox.Show("Error saving discount: " & ex.Message)
        End Try
    End Sub

    Private Sub deleteButton_Click(sender As Object, e As EventArgs) Handles deleteButton.Click
        ' Check if a row is selected
        If dgvDiscounts.SelectedRows.Count = 0 AndAlso dgvDiscounts.SelectedCells.Count = 0 Then
            MessageBox.Show("Please select a discount to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        ' Get the selected row index
        Dim rowIndex As Integer = -1
        If dgvDiscounts.SelectedRows.Count > 0 Then
            rowIndex = dgvDiscounts.SelectedRows(0).Index
        ElseIf dgvDiscounts.SelectedCells.Count > 0 Then
            rowIndex = dgvDiscounts.SelectedCells(0).RowIndex
        End If

        If rowIndex < 0 OrElse rowIndex >= dgvDiscounts.Rows.Count Then
            MessageBox.Show("Invalid selection.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Get the discount details from the selected row
        Dim minSacks As Integer = Convert.ToInt32(dgvDiscounts.Rows(rowIndex).Cells("MinSacks").Value)
        Dim maxSacks As Integer = Convert.ToInt32(dgvDiscounts.Rows(rowIndex).Cells("MaxSacks").Value)
        Dim discountPrice As Decimal = Convert.ToDecimal(dgvDiscounts.Rows(rowIndex).Cells("DiscountPrice").Value)

        ' Confirm deletion
        Dim result = MessageBox.Show(
            $"Are you sure you want to delete this discount?{vbCrLf}{vbCrLf}" &
            $"Range: {minSacks} - {maxSacks} sacks{vbCrLf}" &
            $"Discount Price: ₱{discountPrice:N2}",
            "Confirm Delete",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question)

        If result = DialogResult.No Then
            Return
        End If

        ' Delete from database
        Try
            Dim query As String = "DELETE FROM ProductDiscounts 
                                  WHERE ProductID = @ProductID 
                                  AND MinSacks = @MinSacks 
                                  AND MaxSacks = @MaxSacks 
                                  AND DiscountPrice = @DiscountPrice"

            Using conn As New SqlConnection(GetConnectionString())
                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@ProductID", _productID)
                    cmd.Parameters.AddWithValue("@MinSacks", minSacks)
                    cmd.Parameters.AddWithValue("@MaxSacks", maxSacks)
                    cmd.Parameters.AddWithValue("@DiscountPrice", discountPrice)

                    conn.Open()
                    Dim rowsAffected As Integer = cmd.ExecuteNonQuery()

                    If rowsAffected > 0 Then
                        MessageBox.Show("Discount deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        LoadDiscounts(_productID) ' Refresh DataGridView
                    Else
                        MessageBox.Show("Discount not found in database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show($"Error deleting discount: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class