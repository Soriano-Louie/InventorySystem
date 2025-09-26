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
        dgvDiscounts.Columns.Add("MinSacks", "Min Sacks")
        dgvDiscounts.Columns.Add("MaxSacks", "Max Sacks")
        dgvDiscounts.Columns.Add("DiscountPrice", "Discount Price")

    End Sub

    Private Sub discountForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Show product name on form title or label
        Dim title As String = "Manage Discounts - " & _productName

        Label1.Text = title

        ' Load existing discounts for this product
        LoadDiscounts(_productID)
    End Sub

    Private Function GetConnectionString() As String
        Return "Server=DESKTOP-3AKTMEV;Database=inventorySystem;User Id=sa;Password=24@Hakaaii07;TrustServerCertificate=True;"
    End Function

    Private Sub LoadDiscounts(productID As Integer)

        Try
            Dim query As String = "SELECT MinSacks, MaxSacks, DiscountPrice FROM ProductDiscounts WHERE ProductID = @ProductID"
            Using conn As New SqlConnection(GetConnectionString())
                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@ProductID", productID)

                    conn.Open()
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            Dim minSacks As Integer = reader("MinSacks")
                            Dim maxSacks As Integer = reader("MaxSacks")
                            Dim discountPrice As Decimal = reader("DiscountPrice")

                            ' populate DataGridView
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
            Dim query As String = "INSERT INTO ProductDiscounts (ProductID, MinSacks, DiscountPrice, MaxSacks) VALUES (@ProductID, @MinSacks, @DiscountPrice, @MaxSacks)"
            Using conn As New SqlConnection(GetConnectionString())
                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@ProductID", _productID)
                    cmd.Parameters.AddWithValue("@MinSacks", Convert.ToInt32(txtMinSacks.Text))
                    cmd.Parameters.AddWithValue("@MaxSacks", Convert.ToInt32(txtMaxSacks.Text))
                    cmd.Parameters.AddWithValue("@DiscountPrice", Convert.ToDecimal(txtDiscountPrice.Text))

                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using

            MessageBox.Show("Discount added successfully!")
            LoadDiscounts(_productID) ' refresh DataGridView
        Catch ex As Exception
            MessageBox.Show("Error saving discount: " & ex.Message)
        End Try
    End Sub
End Class