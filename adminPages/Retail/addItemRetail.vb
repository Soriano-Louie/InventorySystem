Imports System.IO
Imports Microsoft.Data.SqlClient
Imports QRCoder

Public Class addItemRetail
    Private parentForm As inventoryRetail

    Public Sub New(parent As inventoryRetail)
        InitializeComponent()
        Me.MaximizeBox = False
        Me.BackColor = Color.FromArgb(230, 216, 177)

        DateTimePicker1.MinDate = DateTime.Now.AddDays(1)

        Label1.ForeColor = Color.FromArgb(79, 51, 40)

        topPanel.BackColor = Color.FromArgb(224, 166, 109)
        mainPanel.BackColor = Color.FromArgb(230, 216, 177)

        Me.parentForm = parent

        categoryDropDown.DropDownStyle = ComboBoxStyle.DropDown
        categoryDropDown.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        categoryDropDown.AutoCompleteSource = AutoCompleteSource.ListItems

        addButton.BackColor = Color.FromArgb(224, 166, 109)
        cancelButton.BackColor = Color.FromArgb(224, 166, 109)
        addButton.ForeColor = Color.FromArgb(79, 51, 40)
        cancelButton.ForeColor = Color.FromArgb(79, 51, 40)

        DateTimePicker1.ShowCheckBox = True
        DateTimePicker1.Checked = False

        ' Initialize VAT checkbox
        VATCheckBox.Checked = False

        ' Make SKU textbox read-only (auto-generated)
        skuTextBox.ReadOnly = True
        skuTextBox.BackColor = Color.FromArgb(240, 240, 240)
        skuTextBox.ForeColor = Color.Gray
        skuTextBox.Text = "Auto-generated"
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

    Private Function GetConnectionString() As String
        Return "Server=DESKTOP-3AKTMEV;Database=inventorySystem;User Id=sa;Password=24@Hakaaii07;TrustServerCertificate=True;"
    End Function

    ''' <summary>
    ''' Auto-generates SKU based on product name and current date
    ''' Format: First 3 letters of product name + YYMMDD + random 2-digit number
    ''' Example: "COF-250119-47" for "Coffee" on Jan 19, 2025
    ''' </summary>
    Private Function GenerateSKU(productName As String) As String
        If String.IsNullOrWhiteSpace(productName) Then
            Return ""
        End If

        ' Get first 3 letters of product name (uppercase, remove spaces)
        Dim cleanName As String = productName.Trim().Replace(" ", "").ToUpper()
        Dim prefix As String = If(cleanName.Length >= 3, cleanName.Substring(0, 3), cleanName.PadRight(3, "X"c))

        ' Get current date in YYMMDD format
        Dim dateCode As String = DateTime.Now.ToString("yyMMdd")

        ' Generate random 2-digit number for uniqueness
        Dim random As New Random()
        Dim randomCode As String = random.Next(10, 99).ToString()

        ' Combine to create SKU: ABC-YYMMDD-RR
        Dim sku As String = $"{prefix}-{dateCode}-{randomCode}"

        ' Check if SKU already exists, if so, regenerate random part
        While SKUExists(sku)
            randomCode = random.Next(10, 99).ToString()
            sku = $"{prefix}-{dateCode}-{randomCode}"
        End While

        Return sku
    End Function

    ''' <summary>
    ''' Checks if SKU already exists in database
    ''' </summary>
    Private Function SKUExists(sku As String) As Boolean
        Dim connString As String = GetConnectionString()
        Try
            Using conn As New SqlConnection(connString)
                conn.Open()
                Dim query As String = "SELECT COUNT(*) FROM retailProducts WHERE SKU = @SKU"
                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@SKU", sku)
                    Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())
                    Return count > 0
                End Using
            End Using
        Catch ex As Exception
            ' If error checking, assume doesn't exist
            Return False
        End Try
    End Function

    Private Function GetVATRate() As Decimal
        Try
            Return SharedUtilities.GetCurrentVATRate()
        Catch ex As Exception
            MessageBox.Show("Error retrieving VAT rate: " & ex.Message & vbCrLf & "Using default VAT rate of 12%.",
                          "VAT Rate Error",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Warning)
            Return 0.12D ' Default 12% VAT rate
        End Try
    End Function

    Private Sub LoadCategories()
        Dim query As String = "SELECT CategoryID, CategoryName FROM Categories ORDER BY CategoryName"
        Dim connString As String = GetConnectionString()

        Try
            Using conn As New SqlConnection(connString)
                Using da As New SqlDataAdapter(query, conn)
                    Dim dt As New DataTable()
                    da.Fill(dt)

                    With categoryDropDown
                        .DataSource = dt
                        .DisplayMember = "CategoryName"   ' visible to user
                        .ValueMember = "CategoryID"       ' hidden value
                        .SelectedIndex = -1               ' optional: no default selection
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

    Private Sub InsertProductWithQRCode(sku As String, productName As String, unit As String, retail As Decimal, cost As Decimal,
        qty As Integer, reorder As Integer, expirationDate As Object,
  categoryID As Integer, isVATApplicable As Boolean)

        Dim connString As String = GetConnectionString()
        Dim newProductID As Integer = -1

        Using conn As New SqlConnection(connString)
            conn.Open()

            ' Updated query with CategoryID, ExpirationDate, and IsVATApplicable
            Dim query As String = "INSERT INTO retailProducts
   (SKU, ProductName, Unit, RetailPrice, Cost, StockQuantity, ReorderLevel, ExpirationDate, CategoryID, QRCodeImage, IsVATApplicable)
        OUTPUT INSERTED.ProductID
         VALUES
          (@SKU, @ProductName, @Unit, @RetailPrice, @Cost, @StockQuantity, @ReorderLevel, @ExpirationDate, @CategoryID, @QRCodeImage, @IsVATApplicable)"

            Using cmd As New SqlCommand(query, conn)

                ' 1️⃣ Generate QR code from SKU (or any unique identifier)
                Dim qrData As Byte()
                Using qrGen As New QRCodeGenerator()
                    Using qrCodeData = qrGen.CreateQrCode(sku, QRCodeGenerator.ECCLevel.Q)
                        Using qrCode As New QRCode(qrCodeData)
                            Using qrImage As Bitmap = qrCode.GetGraphic(20)
                                Using ms As New MemoryStream()
                                    qrImage.Save(ms, Imaging.ImageFormat.Png)
                                    qrData = ms.ToArray()
                                End Using
                            End Using
                        End Using
                    End Using
                End Using

                ' 2️⃣ Add parameters
                cmd.Parameters.AddWithValue("@SKU", sku)
                cmd.Parameters.AddWithValue("@ProductName", productName)
                cmd.Parameters.AddWithValue("@Unit", unit)
                cmd.Parameters.AddWithValue("@RetailPrice", retail)
                cmd.Parameters.AddWithValue("@Cost", cost)
                cmd.Parameters.AddWithValue("@StockQuantity", qty)
                cmd.Parameters.AddWithValue("@ReorderLevel", reorder)
                If expirationDate Is DBNull.Value Then
                    cmd.Parameters.AddWithValue("@ExpirationDate", DBNull.Value)
                Else
                    cmd.Parameters.AddWithValue("@ExpirationDate", CType(expirationDate, Date))
                End If
                cmd.Parameters.AddWithValue("@CategoryID", categoryID)
                cmd.Parameters.Add("@QRCodeImage", SqlDbType.VarBinary).Value = qrData
                cmd.Parameters.AddWithValue("@IsVATApplicable", isVATApplicable)

                ' This executes insert and gets the new ProductID
                newProductID = Convert.ToInt32(cmd.ExecuteScalar())
            End Using
        End Using

        Dim vatMessage As String = ""
        If isVATApplicable Then
            Dim vatRate As Decimal = GetVATRate()
            vatMessage = $" (VAT {(vatRate / 100):P} applicable)"
        Else
            vatMessage = " (VAT not applicable)"
        End If

        MessageBox.Show("Product inserted with QR Code successfully!" & vatMessage)

        ' Refresh parent form's product list
        If parentForm IsNot Nothing Then
            parentForm.LoadProducts()
        End If
    End Sub

    Private Sub ResetControl(ctrl As Control)
        If TypeOf ctrl Is TextBox Then
            DirectCast(ctrl, TextBox).Clear()
        ElseIf TypeOf ctrl Is ComboBox Then
            DirectCast(ctrl, ComboBox).SelectedIndex = -1
        ElseIf TypeOf ctrl Is DateTimePicker Then
            Dim picker As DateTimePicker = DirectCast(ctrl, DateTimePicker)
            picker.Checked = False

            ' Use today's date but respect MinDate/MaxDate
            Dim today As Date = DateTime.Today
            If today < picker.MinDate Then
                picker.Value = picker.MinDate
            ElseIf today > picker.MaxDate Then
                picker.Value = picker.MaxDate
            Else
                picker.Value = today
            End If
        ElseIf TypeOf ctrl Is CheckBox Then
            DirectCast(ctrl, CheckBox).Checked = False
        End If
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.BeginInvoke(Sub() productTextBox.Focus())  ' Focus on product name instead of SKU
        LoadCategories()
    End Sub

    Private Sub cancelButton_Click(sender As Object, e As EventArgs) Handles cancelButton.Click
        Me.Close()
    End Sub

    ''' <summary>
    ''' Event handler when product name changes - auto-generates SKU
    ''' </summary>
    Private Sub productTextBox_TextChanged(sender As Object, e As EventArgs) Handles productTextBox.TextChanged
        ' Only generate if product name has at least 1 character
        If Not String.IsNullOrWhiteSpace(productTextBox.Text) Then
            skuTextBox.Text = GenerateSKU(productTextBox.Text)
            skuTextBox.ForeColor = Color.Black
        Else
            skuTextBox.Text = "Auto-generated"
            skuTextBox.ForeColor = Color.Gray
        End If
    End Sub

    Private Sub saveButton_Click(sender As Object, e As EventArgs) Handles addButton.Click
        If String.IsNullOrWhiteSpace(productTextBox.Text) OrElse
        String.IsNullOrWhiteSpace(unitTextBox.Text) OrElse
        String.IsNullOrWhiteSpace(retailTextBox.Text) OrElse
        String.IsNullOrWhiteSpace(costTextBox.Text) OrElse
        String.IsNullOrWhiteSpace(quantityTextBox.Text) OrElse
        String.IsNullOrWhiteSpace(reorderTextBox.Text) OrElse
        categoryDropDown.SelectedValue Is Nothing Then

            MessageBox.Show("Please fill in all required fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        ' Check if product name already exists (warn user about batch implications)
        If ProductNameExists(productTextBox.Text.Trim()) Then
            Dim result = MessageBox.Show(
                "⚠️ Product Name Already Exists" & vbCrLf & vbCrLf &
                $"A product named '{productTextBox.Text.Trim()}' already exists in the system." & vbCrLf & vbCrLf &
                "IMPORTANT: If you continue:" & vbCrLf &
                "• This will create a NEW BATCH of the same product" & vbCrLf &
                "• Both batches will have the SAME product name" & vbCrLf &
                "• Archiving one will ARCHIVE ALL batches with this name" & vbCrLf & vbCrLf &
                "Do you want to continue and create a new batch?" & vbCrLf & vbCrLf &
                "Click YES to create batch | NO to cancel",
                "Duplicate Product Name Warning",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2)

            If result = DialogResult.No Then
                MessageBox.Show("Operation cancelled. Please use a different product name or edit the existing product to add stock.",
                              "Cancelled",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Information)
                Return
            End If
        End If

        ' Generate final SKU before saving (in case user hasn't typed in product name yet)
        Dim finalSKU As String = GenerateSKU(productTextBox.Text)
        skuTextBox.Text = finalSKU

        Dim expDate As Object
        If DateTimePicker1.Checked Then
            expDate = DateTimePicker1.Value
        Else
            expDate = DBNull.Value   ' No expiration date
        End If

        Try
            InsertProductWithQRCode(
     finalSKU,      ' SKU (auto-generated)
          productTextBox.Text,' Product Name
            unitTextBox.Text,     ' Unit
     Decimal.Parse(retailTextBox.Text),      ' Retail Price
     Decimal.Parse(costTextBox.Text),        ' Cost
       Integer.Parse(quantityTextBox.Text),    ' Stock Quantity
     Integer.Parse(reorderTextBox.Text),     ' Reorder Level
 expDate,  ' Expiration Date or NULL
         Convert.ToInt32(categoryDropDown.SelectedValue), ' CategoryID
            VATCheckBox.Checked      ' IsVATApplicable
        )
            MessageBox.Show("Product inserted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As FormatException
            MessageBox.Show("Please enter valid numeric values for price, cost, quantity, and reorder level.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception
            MessageBox.Show("Error inserting product: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        ' Clear inputs
        For Each ctrl As Control In Panel1.Controls
            ResetControl(ctrl)
        Next
        For Each ctrl As Control In Panel2.Controls
            ResetControl(ctrl)
        Next
        For Each ctrl As Control In Panel3.Controls
            ResetControl(ctrl)
        Next
        For Each ctrl As Control In Panel4.Controls
            ResetControl(ctrl)
        Next
        For Each ctrl As Control In Panel6.Controls
            ResetControl(ctrl)
        Next
        For Each ctrl As Control In Panel7.Controls
            ResetControl(ctrl)
        Next
        For Each ctrl As Control In Panel8.Controls
            ResetControl(ctrl)
        Next
        For Each ctrl As Control In Panel10.Controls
            ResetControl(ctrl)
        Next
        For Each ctrl As Control In Panel11.Controls
            ResetControl(ctrl)
        Next
        ResetControl(DateTimePicker1)

        ' Reset VAT checkbox
        VATCheckBox.Checked = False

        ' Reset SKU display
        skuTextBox.Text = "Auto-generated"
        skuTextBox.ForeColor = Color.Gray

        Me.BeginInvoke(Sub() productTextBox.Focus())

        ' Refresh the DataGridView
        parentForm.LoadProducts()
    End Sub

    ''' <summary>
    ''' Check if a product name already exists in the database
    ''' </summary>
    Private Function ProductNameExists(productName As String) As Boolean
        Try
            Using conn As New SqlConnection(GetConnectionString())
                conn.Open()
                Dim query As String = "SELECT COUNT(*) FROM retailProducts WHERE ProductName = @ProductName AND (IsArchived = 0 OR IsArchived IS NULL)"
                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@ProductName", productName)
                    Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())
                    Return count > 0
                End Using
            End Using
        Catch ex As Exception
            ' If error checking, assume doesn't exist
            Return False
        End Try
    End Function

End Class