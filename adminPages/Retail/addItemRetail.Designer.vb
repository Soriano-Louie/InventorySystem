<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class addItemRetail
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        mainPanel = New Panel()
        TableLayoutPanel1 = New TableLayoutPanel()
        Panel7 = New Panel()
        Label8 = New Label()
        reorderTextBox = New TextBox()
        Panel11 = New Panel()
        Label11 = New Label()
        costTextBox = New TextBox()
        Panel10 = New Panel()
        Label10 = New Label()
        skuTextBox = New TextBox()
        Panel9 = New Panel()
        Panel8 = New Panel()
        DateTimePicker1 = New DateTimePicker()
        Label9 = New Label()
        Panel4 = New Panel()
        Label5 = New Label()
        unitTextBox = New TextBox()
        Panel3 = New Panel()
        Label4 = New Label()
        quantityTextBox = New TextBox()
        Panel2 = New Panel()
        categoryDropDown = New ComboBox()
        Label3 = New Label()
        Panel1 = New Panel()
        productTextBox = New TextBox()
        Label2 = New Label()
        Panel6 = New Panel()
        Label7 = New Label()
        retailTextBox = New TextBox()
        TableLayoutPanel2 = New TableLayoutPanel()
        cancelButton = New Button()
        addButton = New Button()
        topPanel = New Panel()
        Label1 = New Label()
        Panel5 = New Panel()
        Label6 = New Label()
        VATCheckBox = New CheckBox()
        mainPanel.SuspendLayout()
        TableLayoutPanel1.SuspendLayout()
        Panel7.SuspendLayout()
        Panel11.SuspendLayout()
        Panel10.SuspendLayout()
        Panel9.SuspendLayout()
        Panel8.SuspendLayout()
        Panel4.SuspendLayout()
        Panel3.SuspendLayout()
        Panel2.SuspendLayout()
        Panel1.SuspendLayout()
        Panel6.SuspendLayout()
        TableLayoutPanel2.SuspendLayout()
        topPanel.SuspendLayout()
        Panel5.SuspendLayout()
        SuspendLayout()
        ' 
        ' mainPanel
        ' 
        mainPanel.Controls.Add(TableLayoutPanel1)
        mainPanel.Controls.Add(topPanel)
        mainPanel.Dock = DockStyle.Fill
        mainPanel.Location = New Point(0, 0)
        mainPanel.Name = "mainPanel"
        mainPanel.Size = New Size(363, 735)
        mainPanel.TabIndex = 0
        ' 
        ' TableLayoutPanel1
        ' 
        TableLayoutPanel1.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left
        TableLayoutPanel1.ColumnCount = 1
        TableLayoutPanel1.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100F))
        TableLayoutPanel1.Controls.Add(Panel7, 0, 7)
        TableLayoutPanel1.Controls.Add(Panel11, 0, 5)
        TableLayoutPanel1.Controls.Add(Panel10, 0, 0)
        TableLayoutPanel1.Controls.Add(Panel9, 0, 8)
        TableLayoutPanel1.Controls.Add(Panel4, 0, 4)
        TableLayoutPanel1.Controls.Add(Panel3, 0, 3)
        TableLayoutPanel1.Controls.Add(Panel2, 0, 2)
        TableLayoutPanel1.Controls.Add(Panel1, 0, 1)
        TableLayoutPanel1.Controls.Add(Panel6, 0, 6)
        TableLayoutPanel1.Controls.Add(TableLayoutPanel2, 0, 10)
        TableLayoutPanel1.Controls.Add(Panel5, 0, 9)
        TableLayoutPanel1.Location = New Point(0, 69)
        TableLayoutPanel1.Name = "TableLayoutPanel1"
        TableLayoutPanel1.RowCount = 11
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Percent, 9.090909F))
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Percent, 9.090909F))
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Percent, 9.090909F))
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Percent, 9.090909F))
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Percent, 9.090909F))
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Percent, 9.090909F))
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Percent, 9.090909F))
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Percent, 9.090909F))
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Percent, 9.090909F))
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Percent, 9.090909F))
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Percent, 9.090909F))
        TableLayoutPanel1.Size = New Size(362, 666)
        TableLayoutPanel1.TabIndex = 3
        ' 
        ' Panel7
        ' 
        Panel7.Controls.Add(Label8)
        Panel7.Controls.Add(reorderTextBox)
        Panel7.Dock = DockStyle.Fill
        Panel7.Location = New Point(3, 423)
        Panel7.Name = "Panel7"
        Panel7.Size = New Size(356, 54)
        Panel7.TabIndex = 9
        ' 
        ' Label8
        ' 
        Label8.AutoSize = True
        Label8.Location = New Point(3, 10)
        Label8.Name = "Label8"
        Label8.Size = New Size(78, 15)
        Label8.TabIndex = 1
        Label8.Text = "Reorder Level"
        Label8.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' reorderTextBox
        ' 
        reorderTextBox.Font = New Font("Segoe UI", 11F)
        reorderTextBox.Location = New Point(3, 31)
        reorderTextBox.Name = "reorderTextBox"
        reorderTextBox.Size = New Size(350, 27)
        reorderTextBox.TabIndex = 9
        ' 
        ' Panel11
        ' 
        Panel11.Controls.Add(Label11)
        Panel11.Controls.Add(costTextBox)
        Panel11.Dock = DockStyle.Fill
        Panel11.Location = New Point(3, 303)
        Panel11.Name = "Panel11"
        Panel11.Size = New Size(356, 54)
        Panel11.TabIndex = 6
        ' 
        ' Label11
        ' 
        Label11.AutoSize = True
        Label11.Location = New Point(3, 10)
        Label11.Name = "Label11"
        Label11.Size = New Size(31, 15)
        Label11.TabIndex = 1
        Label11.Text = "Cost"
        Label11.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' costTextBox
        ' 
        costTextBox.Font = New Font("Segoe UI", 11F)
        costTextBox.Location = New Point(3, 31)
        costTextBox.Name = "costTextBox"
        costTextBox.Size = New Size(350, 27)
        costTextBox.TabIndex = 6
        ' 
        ' Panel10
        ' 
        Panel10.Controls.Add(Label10)
        Panel10.Controls.Add(skuTextBox)
        Panel10.Dock = DockStyle.Fill
        Panel10.Location = New Point(3, 3)
        Panel10.Name = "Panel10"
        Panel10.Size = New Size(356, 54)
        Panel10.TabIndex = 15
        ' 
        ' Label10
        ' 
        Label10.AutoSize = True
        Label10.Location = New Point(3, 10)
        Label10.Name = "Label10"
        Label10.Size = New Size(80, 15)
        Label10.TabIndex = 1
        Label10.Text = "Product Code"
        Label10.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' skuTextBox
        ' 
        skuTextBox.Font = New Font("Segoe UI", 11F)
        skuTextBox.Location = New Point(3, 31)
        skuTextBox.Name = "skuTextBox"
        skuTextBox.Size = New Size(350, 27)
        skuTextBox.TabIndex = 1
        ' 
        ' Panel9
        ' 
        Panel9.Controls.Add(Panel8)
        Panel9.Dock = DockStyle.Fill
        Panel9.Location = New Point(3, 483)
        Panel9.Name = "Panel9"
        Panel9.Size = New Size(356, 54)
        Panel9.TabIndex = 12
        ' 
        ' Panel8
        ' 
        Panel8.Controls.Add(DateTimePicker1)
        Panel8.Controls.Add(Label9)
        Panel8.Dock = DockStyle.Fill
        Panel8.Location = New Point(0, 0)
        Panel8.Name = "Panel8"
        Panel8.Size = New Size(356, 54)
        Panel8.TabIndex = 10
        ' 
        ' DateTimePicker1
        ' 
        DateTimePicker1.Location = New Point(3, 31)
        DateTimePicker1.MinDate = New Date(2025, 9, 24, 0, 0, 0, 0)
        DateTimePicker1.Name = "DateTimePicker1"
        DateTimePicker1.Size = New Size(200, 23)
        DateTimePicker1.TabIndex = 10
        ' 
        ' Label9
        ' 
        Label9.AutoSize = True
        Label9.Location = New Point(3, 10)
        Label9.Name = "Label9"
        Label9.Size = New Size(151, 15)
        Label9.TabIndex = 1
        Label9.Text = "Expiration Date (REQUIRED)"
        Label9.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' Panel4
        ' 
        Panel4.Controls.Add(Label5)
        Panel4.Controls.Add(unitTextBox)
        Panel4.Dock = DockStyle.Fill
        Panel4.Location = New Point(3, 243)
        Panel4.Name = "Panel4"
        Panel4.Size = New Size(356, 54)
        Panel4.TabIndex = 5
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.Location = New Point(3, 10)
        Label5.Name = "Label5"
        Label5.Size = New Size(125, 15)
        Label5.TabIndex = 1
        Label5.Text = "Unit (kg, pc, pack, etc)"
        Label5.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' unitTextBox
        ' 
        unitTextBox.Font = New Font("Segoe UI", 11F)
        unitTextBox.Location = New Point(3, 31)
        unitTextBox.Name = "unitTextBox"
        unitTextBox.Size = New Size(350, 27)
        unitTextBox.TabIndex = 5
        ' 
        ' Panel3
        ' 
        Panel3.Controls.Add(Label4)
        Panel3.Controls.Add(quantityTextBox)
        Panel3.Dock = DockStyle.Fill
        Panel3.Location = New Point(3, 183)
        Panel3.Name = "Panel3"
        Panel3.Size = New Size(356, 54)
        Panel3.TabIndex = 4
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Location = New Point(3, 10)
        Label4.Name = "Label4"
        Label4.Size = New Size(53, 15)
        Label4.TabIndex = 1
        Label4.Text = "Quantity"
        Label4.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' quantityTextBox
        ' 
        quantityTextBox.Font = New Font("Segoe UI", 11F)
        quantityTextBox.Location = New Point(3, 31)
        quantityTextBox.Name = "quantityTextBox"
        quantityTextBox.Size = New Size(350, 27)
        quantityTextBox.TabIndex = 4
        ' 
        ' Panel2
        ' 
        Panel2.Controls.Add(categoryDropDown)
        Panel2.Controls.Add(Label3)
        Panel2.Dock = DockStyle.Fill
        Panel2.Location = New Point(3, 123)
        Panel2.Name = "Panel2"
        Panel2.Size = New Size(356, 54)
        Panel2.TabIndex = 3
        ' 
        ' categoryDropDown
        ' 
        categoryDropDown.Font = New Font("Segoe UI", 11F)
        categoryDropDown.FormattingEnabled = True
        categoryDropDown.Location = New Point(3, 31)
        categoryDropDown.Name = "categoryDropDown"
        categoryDropDown.Size = New Size(350, 28)
        categoryDropDown.TabIndex = 3
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(3, 10)
        Label3.Name = "Label3"
        Label3.Size = New Size(55, 15)
        Label3.TabIndex = 1
        Label3.Text = "Category"
        Label3.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' Panel1
        ' 
        Panel1.Controls.Add(productTextBox)
        Panel1.Controls.Add(Label2)
        Panel1.Dock = DockStyle.Fill
        Panel1.Location = New Point(3, 63)
        Panel1.Name = "Panel1"
        Panel1.Size = New Size(356, 54)
        Panel1.TabIndex = 2
        ' 
        ' productTextBox
        ' 
        productTextBox.Font = New Font("Segoe UI", 11F)
        productTextBox.Location = New Point(3, 27)
        productTextBox.Name = "productTextBox"
        productTextBox.Size = New Size(350, 27)
        productTextBox.TabIndex = 2
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(3, 10)
        Label2.Name = "Label2"
        Label2.Size = New Size(84, 15)
        Label2.TabIndex = 1
        Label2.Text = "Product Name"
        Label2.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' Panel6
        ' 
        Panel6.Controls.Add(Label7)
        Panel6.Controls.Add(retailTextBox)
        Panel6.Dock = DockStyle.Fill
        Panel6.Location = New Point(3, 363)
        Panel6.Name = "Panel6"
        Panel6.Size = New Size(356, 54)
        Panel6.TabIndex = 8
        ' 
        ' Label7
        ' 
        Label7.AutoSize = True
        Label7.Location = New Point(3, 10)
        Label7.Name = "Label7"
        Label7.Size = New Size(65, 15)
        Label7.TabIndex = 1
        Label7.Text = "Retail Price"
        Label7.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' retailTextBox
        ' 
        retailTextBox.Font = New Font("Segoe UI", 11F)
        retailTextBox.Location = New Point(3, 31)
        retailTextBox.Name = "retailTextBox"
        retailTextBox.Size = New Size(350, 27)
        retailTextBox.TabIndex = 8
        ' 
        ' TableLayoutPanel2
        ' 
        TableLayoutPanel2.ColumnCount = 2
        TableLayoutPanel2.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50F))
        TableLayoutPanel2.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50F))
        TableLayoutPanel2.Controls.Add(cancelButton, 1, 0)
        TableLayoutPanel2.Controls.Add(addButton, 0, 0)
        TableLayoutPanel2.Location = New Point(3, 603)
        TableLayoutPanel2.Name = "TableLayoutPanel2"
        TableLayoutPanel2.RowCount = 1
        TableLayoutPanel2.RowStyles.Add(New RowStyle(SizeType.Percent, 100F))
        TableLayoutPanel2.Size = New Size(356, 60)
        TableLayoutPanel2.TabIndex = 11
        ' 
        ' cancelButton
        ' 
        cancelButton.Anchor = AnchorStyles.None
        cancelButton.AutoSize = True
        cancelButton.Cursor = Cursors.Hand
        cancelButton.FlatAppearance.BorderSize = 0
        cancelButton.FlatStyle = FlatStyle.Popup
        cancelButton.Font = New Font("Segoe UI Semibold", 12F, FontStyle.Bold)
        cancelButton.Location = New Point(220, 14)
        cancelButton.Name = "cancelButton"
        cancelButton.Size = New Size(93, 31)
        cancelButton.TabIndex = 12
        cancelButton.Text = "CANCEL"
        cancelButton.UseVisualStyleBackColor = True
        ' 
        ' addButton
        ' 
        addButton.Anchor = AnchorStyles.None
        addButton.AutoSize = True
        addButton.Cursor = Cursors.Hand
        addButton.FlatAppearance.BorderSize = 0
        addButton.FlatStyle = FlatStyle.Popup
        addButton.Font = New Font("Segoe UI Semibold", 12F, FontStyle.Bold)
        addButton.Location = New Point(51, 14)
        addButton.Name = "addButton"
        addButton.Size = New Size(75, 31)
        addButton.TabIndex = 11
        addButton.Text = "ADD"
        addButton.UseVisualStyleBackColor = True
        ' 
        ' topPanel
        ' 
        topPanel.Controls.Add(Label1)
        topPanel.Dock = DockStyle.Top
        topPanel.Location = New Point(0, 0)
        topPanel.Name = "topPanel"
        topPanel.Size = New Size(363, 69)
        topPanel.TabIndex = 0
        ' 
        ' Label1
        ' 
        Label1.Dock = DockStyle.Fill
        Label1.Font = New Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Label1.Location = New Point(0, 0)
        Label1.Name = "Label1"
        Label1.Size = New Size(363, 69)
        Label1.TabIndex = 0
        Label1.Text = "Add Item to Inventory"
        Label1.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' Panel5
        ' 
        Panel5.Controls.Add(Label6)
        Panel5.Controls.Add(VATCheckBox)
        Panel5.Dock = DockStyle.Fill
        Panel5.Location = New Point(3, 543)
        Panel5.Name = "Panel5"
        Panel5.Size = New Size(356, 54)
        Panel5.TabIndex = 16
        ' 
        ' Label6
        ' 
        Label6.AutoSize = True
        Label6.Location = New Point(3, 11)
        Label6.Name = "Label6"
        Label6.Size = New Size(220, 15)
        Label6.TabIndex = 19
        Label6.Text = "Check if VAT is applicable to the product"
        Label6.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' VATCheckBox
        ' 
        VATCheckBox.Anchor = AnchorStyles.Left
        VATCheckBox.AutoSize = True
        VATCheckBox.Font = New Font("Segoe UI", 9F)
        VATCheckBox.Location = New Point(8, 29)
        VATCheckBox.Name = "VATCheckBox"
        VATCheckBox.Size = New Size(80, 19)
        VATCheckBox.TabIndex = 18
        VATCheckBox.Text = "Apply VAT"
        VATCheckBox.UseVisualStyleBackColor = True
        ' 
        ' addItemRetail
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(363, 735)
        Controls.Add(mainPanel)
        FormBorderStyle = FormBorderStyle.FixedToolWindow
        Name = "addItemRetail"
        StartPosition = FormStartPosition.CenterParent
        mainPanel.ResumeLayout(False)
        TableLayoutPanel1.ResumeLayout(False)
        Panel7.ResumeLayout(False)
        Panel7.PerformLayout()
        Panel11.ResumeLayout(False)
        Panel11.PerformLayout()
        Panel10.ResumeLayout(False)
        Panel10.PerformLayout()
        Panel9.ResumeLayout(False)
        Panel8.ResumeLayout(False)
        Panel8.PerformLayout()
        Panel4.ResumeLayout(False)
        Panel4.PerformLayout()
        Panel3.ResumeLayout(False)
        Panel3.PerformLayout()
        Panel2.ResumeLayout(False)
        Panel2.PerformLayout()
        Panel1.ResumeLayout(False)
        Panel1.PerformLayout()
        Panel6.ResumeLayout(False)
        Panel6.PerformLayout()
        TableLayoutPanel2.ResumeLayout(False)
        TableLayoutPanel2.PerformLayout()
        topPanel.ResumeLayout(False)
        Panel5.ResumeLayout(False)
        Panel5.PerformLayout()
        ResumeLayout(False)
    End Sub

    Friend WithEvents mainPanel As Panel
    Friend WithEvents topPanel As Panel
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Panel8 As Panel
    Friend WithEvents Label9 As Label
    Friend WithEvents Panel7 As Panel
    Friend WithEvents Label8 As Label
    Friend WithEvents reorderTextBox As TextBox
    Friend WithEvents Panel6 As Panel
    Friend WithEvents Label7 As Label
    Friend WithEvents retailTextBox As TextBox
    Friend WithEvents Panel4 As Panel
    Friend WithEvents Label5 As Label
    Friend WithEvents unitTextBox As TextBox
    Friend WithEvents Panel3 As Panel
    Friend WithEvents Label4 As Label
    Friend WithEvents quantityTextBox As TextBox
    Friend WithEvents Panel2 As Panel
    Friend WithEvents Label3 As Label
    Friend WithEvents categoryDropDown As ComboBox
    Friend WithEvents TableLayoutPanel2 As TableLayoutPanel
    Friend WithEvents cancelButton As Button
    Friend WithEvents addButton As Button
    Friend WithEvents Panel9 As Panel
    Friend WithEvents DateTimePicker1 As DateTimePicker
    Friend WithEvents Panel11 As Panel
    Friend WithEvents Label11 As Label
    Friend WithEvents costTextBox As TextBox
    Friend WithEvents Panel10 As Panel
    Friend WithEvents Label10 As Label
    Friend WithEvents skuTextBox As TextBox
    Friend WithEvents productTextBox As TextBox
    Friend WithEvents Panel5 As Panel
    Friend WithEvents Label6 As Label
    Friend WithEvents VATCheckBox As CheckBox
End Class
