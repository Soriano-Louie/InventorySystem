Imports Microsoft.Data.SqlClient

Public Class ArchivedProductsForm
    Inherits Form

    Private Const WHOLESALE_TYPE As String = "Wholesale"
    Private Const RETAIL_TYPE As String = "Retail"

    Private currentProductType As String
    Private dt As New DataTable()

    ' Store references to radio buttons for keyboard access
    Private WithEvents radioWholesale As RadioButton

    Private WithEvents radioRetail As RadioButton

    ' Constructor with optional product type parameter
    Public Sub New(Optional initialProductType As String = WHOLESALE_TYPE)
        MyBase.New()

        ' Set the initial product type
        currentProductType = initialProductType

        Me.Text = "Archived Products Management"
        Me.Size = New Size(1200, 700)
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.BackColor = Color.FromArgb(230, 216, 177)
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False

        ' Enable keyboard shortcuts
        Me.KeyPreview = True

        InitializeControls()
        LoadArchivedProducts()
    End Sub

    Private Sub InitializeControls()
        ' Title Label
        Dim lblTitle As New Label With {
            .Text = "📦 Archived Products",
            .Font = New Font("Segoe UI", 20, FontStyle.Bold),
            .ForeColor = Color.FromArgb(79, 51, 40),
            .Location = New Point(20, 20),
            .AutoSize = True
        }
        Me.Controls.Add(lblTitle)

        ' Product Type Toggle
        radioWholesale = New RadioButton With {
            .Text = "Wholesale Products",
            .Location = New Point(20, 70),
            .AutoSize = True,
            .Checked = (currentProductType = WHOLESALE_TYPE),
            .Font = New Font("Segoe UI", 10),
            .ForeColor = Color.FromArgb(79, 51, 40)
        }
        AddHandler radioWholesale.CheckedChanged, Sub()
                                                      If radioWholesale.Checked Then
                                                          currentProductType = WHOLESALE_TYPE
                                                          LoadArchivedProducts()
                                                      End If
                                                  End Sub
        Me.Controls.Add(radioWholesale)

        radioRetail = New RadioButton With {
            .Text = "Retail Products",
            .Location = New Point(200, 70),
            .AutoSize = True,
            .Checked = (currentProductType = RETAIL_TYPE),
            .Font = New Font("Segoe UI", 10),
            .ForeColor = Color.FromArgb(79, 51, 40)
        }
        AddHandler radioRetail.CheckedChanged, Sub()
                                                   If radioRetail.Checked Then
                                                       currentProductType = RETAIL_TYPE
                                                       LoadArchivedProducts()
                                                   End If
                                               End Sub
        Me.Controls.Add(radioRetail)

        ' Keyboard Shortcuts Info Label
        Dim lblShortcuts As New Label With {
            .Text = "Shortcuts: R = Restore | ESC = Close",
            .Font = New Font("Segoe UI", 9, FontStyle.Italic),
            .ForeColor = Color.FromArgb(79, 51, 40),
            .Location = New Point(400, 73),
            .AutoSize = True
        }
        Me.Controls.Add(lblShortcuts)

        ' DataGridView
        Dim dgv As New DataGridView With {
            .Name = "dgvArchivedProducts",
            .Location = New Point(20, 110),
            .Size = New Size(1140, 450),
            .BackgroundColor = Color.FromArgb(230, 216, 177),
            .GridColor = Color.FromArgb(79, 51, 40),
            .ReadOnly = True,
            .AllowUserToAddRows = False,
            .AllowUserToDeleteRows = False,
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            .MultiSelect = False,
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            .RowHeadersVisible = False,
            .AllowUserToResizeColumns = False,
            .AllowUserToResizeRows = False
        }
        Me.Controls.Add(dgv)

        ' Restore Button
        Dim btnRestore As New Button With {
            .Name = "btnRestore",
            .Text = "🔄 Restore Selected Product (R)",
            .Location = New Point(20, 580),
            .Size = New Size(250, 50),
            .BackColor = Color.FromArgb(76, 175, 80),
            .ForeColor = Color.White,
            .Font = New Font("Segoe UI", 12, FontStyle.Bold),
            .Cursor = Cursors.Hand,
            .FlatStyle = FlatStyle.Flat
        }
        btnRestore.FlatAppearance.BorderSize = 0
        AddHandler btnRestore.Click, AddressOf RestoreSelectedProduct
        Me.Controls.Add(btnRestore)

        ' Close Button
        Dim btnClose As New Button With {
            .Name = "btnClose",
            .Text = "Close (ESC)",
            .Location = New Point(290, 580),
            .Size = New Size(100, 50),
            .BackColor = Color.FromArgb(147, 53, 53),
            .ForeColor = Color.FromArgb(230, 216, 177),
            .Font = New Font("Segoe UI", 12, FontStyle.Bold),
            .Cursor = Cursors.Hand,
            .FlatStyle = FlatStyle.Flat
        }
        btnClose.FlatAppearance.BorderSize = 0
        AddHandler btnClose.Click, Sub() Me.Close()
        Me.Controls.Add(btnClose)
    End Sub

    ''' <summary>
    ''' Handle keyboard shortcuts
    ''' </summary>
    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean
        Select Case keyData
            Case Keys.R
                ' R key - Restore selected product
                RestoreSelectedProduct(Nothing, Nothing)
                Return True
            Case Keys.Escape
                ' ESC key - Close form
                Me.Close()
                Return True
            Case Else
                Return MyBase.ProcessCmdKey(msg, keyData)
        End Select
    End Function

    Private Sub LoadArchivedProducts()
        Dim tableName As String = If(currentProductType = WHOLESALE_TYPE, "wholesaleProducts", "retailProducts")
        Dim query As String = $"
            SELECT
                p.ProductID,
                p.SKU,
                p.ProductName,
                c.CategoryName,
                p.StockQuantity,
                p.Unit,
                p.ArchivedDate,
                u.Username as ArchivedBy,
                p.ArchiveReason
            FROM {tableName} p
            LEFT JOIN Categories c ON p.CategoryID = c.CategoryID
            LEFT JOIN Users u ON p.ArchivedBy = u.UserID
            WHERE p.IsArchived = 1
            ORDER BY p.ArchivedDate DESC"

        Try
            Using conn As New SqlConnection(GetConnectionString())
                Using da As New SqlDataAdapter(query, conn)
                    dt.Clear()
                    da.Fill(dt)
                End Using
            End Using

            Dim dgv As DataGridView = DirectCast(Me.Controls("dgvArchivedProducts"), DataGridView)
            dgv.DataSource = dt

            ' Configure columns
            If dgv.Columns.Contains("ProductID") Then dgv.Columns("ProductID").Visible = False
            If dgv.Columns.Contains("SKU") Then dgv.Columns("SKU").HeaderText = "Product Code"
            If dgv.Columns.Contains("ProductName") Then dgv.Columns("ProductName").HeaderText = "Product Name"
            If dgv.Columns.Contains("CategoryName") Then dgv.Columns("CategoryName").HeaderText = "Category"
            If dgv.Columns.Contains("StockQuantity") Then dgv.Columns("StockQuantity").HeaderText = "Stock"
            If dgv.Columns.Contains("Unit") Then dgv.Columns("Unit").HeaderText = "Unit"
            If dgv.Columns.Contains("ArchivedDate") Then
                dgv.Columns("ArchivedDate").HeaderText = "Archived Date"
                dgv.Columns("ArchivedDate").DefaultCellStyle.Format = "yyyy-MM-dd HH:mm"
            End If
            If dgv.Columns.Contains("ArchivedBy") Then dgv.Columns("ArchivedBy").HeaderText = "Archived By"
            If dgv.Columns.Contains("ArchiveReason") Then dgv.Columns("ArchiveReason").HeaderText = "Reason"
        Catch ex As Exception
            MessageBox.Show($"Error loading archived products: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub RestoreSelectedProduct(sender As Object, e As EventArgs)
        Dim dgv As DataGridView = DirectCast(Me.Controls("dgvArchivedProducts"), DataGridView)

        If dgv.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select a product to restore.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim productID As Integer = Convert.ToInt32(dgv.SelectedRows(0).Cells("ProductID").Value)
        Dim productName As String = dgv.SelectedRows(0).Cells("ProductName").Value.ToString()

        Dim result = MessageBox.Show($"Are you sure you want to restore '{productName}'?" & vbCrLf & vbCrLf &
                                    "This will make it visible in the active inventory again.",
                                    "Confirm Restore",
                                    MessageBoxButtons.YesNo,
                                    MessageBoxIcon.Question)

        If result = DialogResult.Yes Then
            RestoreProduct(productID, productName)
        End If
    End Sub

    Private Sub RestoreProduct(productID As Integer, productName As String)
        Dim tableName As String = If(currentProductType = WHOLESALE_TYPE, "wholesaleProducts", "retailProducts")

        Try
            Using conn As New SqlConnection(GetConnectionString())
                conn.Open()
                Dim query As String = $"UPDATE {tableName}
                                       SET IsArchived = 0,
                                           ArchivedDate = NULL,
                                           ArchivedBy = NULL,
                                           ArchiveReason = NULL
                                       WHERE ProductID = @ProductID"

                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@ProductID", productID)
                    Dim rowsAffected As Integer = cmd.ExecuteNonQuery()

                    If rowsAffected > 0 Then
                        MessageBox.Show($"✓ Product '{productName}' has been restored successfully!" & vbCrLf &
                                      "It is now visible in the active inventory.",
                                      "Restore Successful",
                                      MessageBoxButtons.OK,
                                      MessageBoxIcon.Information)
                        LoadArchivedProducts()
                    Else
                        MessageBox.Show("Product not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show($"Error restoring product: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Function GetConnectionString() As String
        Return SharedUtilities.GetConnectionString()
    End Function

End Class