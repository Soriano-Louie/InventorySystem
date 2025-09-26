﻿Imports System.Data.SqlClient
Imports System.IO
Imports Microsoft.Data.SqlClient
Imports QRCoder

Public Class addItemForm
    Public Sub New()
        InitializeComponent()
        Me.MaximizeBox = False
        Me.BackColor = Color.FromArgb(230, 216, 177)

        DateTimePicker1.MinDate = DateTime.Now.AddDays(1)

        Label1.ForeColor = Color.FromArgb(79, 51, 40)

        topPanel.BackColor = Color.FromArgb(224, 166, 109)
        mainPanel.BackColor = Color.FromArgb(230, 216, 177)

        categoryDropDown.DropDownStyle = ComboBoxStyle.DropDown
        categoryDropDown.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        categoryDropDown.AutoCompleteSource = AutoCompleteSource.ListItems


        addButton.BackColor = Color.FromArgb(224, 166, 109)
        cancelButton.BackColor = Color.FromArgb(224, 166, 109)
        addButton.ForeColor = Color.FromArgb(79, 51, 40)
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

    Private Function GetConnectionString() As String
        Return "Server=DESKTOP-3AKTMEV;Database=inventorySystem;User Id=sa;Password=24@Hakaaii07;TrustServerCertificate=True;"
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
                                    qty As Integer, reorder As Integer, expirationDate As Date,
                                    categoryID As Integer)

        Dim connString As String = GetConnectionString()
        Dim newProductID As Integer = -1

        Using conn As New SqlConnection(connString)
            conn.Open()

            ' Updated query with CategoryID and ExpirationDate
            Dim query As String = "INSERT INTO Products 
                               (SKU, ProductName, Unit, RetailPrice, Cost, StockQuantity, ReorderLevel, ExpirationDate, CategoryID, QRCodeImage) 
                               OUTPUT INSERTED.ProductID
                               VALUES 
                               (@SKU, @ProductName, @Unit, @RetailPrice, @Cost, @StockQuantity, @ReorderLevel, @ExpirationDate, @CategoryID, @QRCodeImage)"

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
                cmd.Parameters.AddWithValue("@ExpirationDate", expirationDate)
                cmd.Parameters.AddWithValue("@CategoryID", categoryID)
                cmd.Parameters.Add("@QRCodeImage", SqlDbType.VarBinary).Value = qrData

                ' This executes insert and gets the new ProductID
                newProductID = Convert.ToInt32(cmd.ExecuteScalar())
            End Using
        End Using

        MessageBox.Show("Product inserted with QR Code successfully!")
        If MessageBox.Show("Do you want to set discounts for this product now?",
                   "Add Discounts", MessageBoxButtons.YesNo) = DialogResult.Yes Then
            Dim discountForm As New discountForm(newProductID, productName)
            discountForm.ShowDialog()
        End If
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


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.BeginInvoke(Sub() skuTextBox.Focus())
        LoadCategories()
    End Sub

    Private Sub cancelButton_Click(sender As Object, e As EventArgs) Handles cancelButton.Click
        Me.Close()
    End Sub

    Private Sub saveButton_Click(sender As Object, e As EventArgs) Handles addButton.Click
        InsertProductWithQRCode(
            skuTextBox.Text,                           ' SKU
            productTextBox.Text,                       ' Product Name
            unitTextBox.Text,                          ' Unit
            Decimal.Parse(retailTextBox.Text),         ' Retail Price
            Decimal.Parse(costTextBox.Text),           ' Cost
            Integer.Parse(quantityTextBox.Text),       ' Stock Quantity
            Integer.Parse(reorderTextBox.Text),        ' Reorder Level
            DateTime.Parse(DateTimePicker1.Text), ' Expiration Date
            Convert.ToInt32(categoryDropDown.SelectedValue) ' CategoryID (hidden ValueMember)
        )

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
        Me.BeginInvoke(Sub() skuTextBox.Focus())


        ' Refresh the DataGridView
        InventoryForm.LoadProducts()
    End Sub

End Class