<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class editItemForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Panel1 = New Panel()
        Label1 = New Label()
        TableLayoutPanel1 = New TableLayoutPanel()
        Panel12 = New Panel()
        editDiscountButton = New Button()
        Panel11 = New Panel()
        Label10 = New Label()
        newCostText = New TextBox()
        Panel5 = New Panel()
        Label6 = New Label()
        newProductText = New TextBox()
        Panel9 = New Panel()
        Panel8 = New Panel()
        newDateText = New DateTimePicker()
        Label9 = New Label()
        Panel10 = New Panel()
        productDropDown = New ComboBox()
        Label2 = New Label()
        Panel4 = New Panel()
        Label5 = New Label()
        newUnitText = New TextBox()
        Panel3 = New Panel()
        Label4 = New Label()
        newQuantityText = New TextBox()
        Panel2 = New Panel()
        newCategoryDropdown = New ComboBox()
        Label3 = New Label()
        Panel7 = New Panel()
        Label8 = New Label()
        newReorderText = New TextBox()
        Panel6 = New Panel()
        Label7 = New Label()
        newRetailText = New TextBox()
        TableLayoutPanel2 = New TableLayoutPanel()
        deleteButton = New Button()
        updateButton = New Button()
        cancelButton = New Button()
        Panel1.SuspendLayout()
        TableLayoutPanel1.SuspendLayout()
        Panel12.SuspendLayout()
        Panel11.SuspendLayout()
        Panel5.SuspendLayout()
        Panel9.SuspendLayout()
        Panel8.SuspendLayout()
        Panel10.SuspendLayout()
        Panel4.SuspendLayout()
        Panel3.SuspendLayout()
        Panel2.SuspendLayout()
        Panel7.SuspendLayout()
        Panel6.SuspendLayout()
        TableLayoutPanel2.SuspendLayout()
        SuspendLayout()
        ' 
        ' Panel1
        ' 
        Panel1.Controls.Add(Label1)
        Panel1.Dock = DockStyle.Top
        Panel1.Location = New Point(0, 0)
        Panel1.Name = "Panel1"
        Panel1.Size = New Size(362, 69)
        Panel1.TabIndex = 0
        ' 
        ' Label1
        ' 
        Label1.Dock = DockStyle.Fill
        Label1.Font = New Font("Segoe UI", 15.75F, FontStyle.Bold)
        Label1.Location = New Point(0, 0)
        Label1.Name = "Label1"
        Label1.Size = New Size(362, 69)
        Label1.TabIndex = 0
        Label1.Text = "Edit Item Details"
        Label1.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' TableLayoutPanel1
        ' 
        TableLayoutPanel1.ColumnCount = 1
        TableLayoutPanel1.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TableLayoutPanel1.Controls.Add(Panel12, 0, 9)
        TableLayoutPanel1.Controls.Add(Panel11, 0, 5)
        TableLayoutPanel1.Controls.Add(Panel5, 0, 1)
        TableLayoutPanel1.Controls.Add(Panel9, 0, 8)
        TableLayoutPanel1.Controls.Add(Panel10, 0, 0)
        TableLayoutPanel1.Controls.Add(Panel4, 0, 4)
        TableLayoutPanel1.Controls.Add(Panel3, 0, 3)
        TableLayoutPanel1.Controls.Add(Panel2, 0, 2)
        TableLayoutPanel1.Controls.Add(Panel7, 0, 7)
        TableLayoutPanel1.Controls.Add(Panel6, 0, 6)
        TableLayoutPanel1.Controls.Add(TableLayoutPanel2, 0, 10)
        TableLayoutPanel1.Dock = DockStyle.Fill
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
        TableLayoutPanel1.TabIndex = 4
        ' 
        ' Panel12
        ' 
        Panel12.Controls.Add(editDiscountButton)
        Panel12.Dock = DockStyle.Fill
        Panel12.Location = New Point(3, 543)
        Panel12.Name = "Panel12"
        Panel12.Size = New Size(356, 54)
        Panel12.TabIndex = 13
        ' 
        ' editDiscountButton
        ' 
        editDiscountButton.Anchor = AnchorStyles.None
        editDiscountButton.Cursor = Cursors.Hand
        editDiscountButton.FlatStyle = FlatStyle.Popup
        editDiscountButton.Font = New Font("Segoe UI Semibold", 12.0F, FontStyle.Bold)
        editDiscountButton.Location = New Point(117, 12)
        editDiscountButton.Name = "editDiscountButton"
        editDiscountButton.Size = New Size(124, 31)
        editDiscountButton.TabIndex = 14
        editDiscountButton.Text = "Edit Discount"
        editDiscountButton.UseVisualStyleBackColor = True
        ' 
        ' Panel11
        ' 
        Panel11.Controls.Add(Label10)
        Panel11.Controls.Add(newCostText)
        Panel11.Dock = DockStyle.Fill
        Panel11.Location = New Point(3, 303)
        Panel11.Name = "Panel11"
        Panel11.Size = New Size(356, 54)
        Panel11.TabIndex = 9
        ' 
        ' Label10
        ' 
        Label10.AutoSize = True
        Label10.Location = New Point(3, 10)
        Label10.Name = "Label10"
        Label10.Size = New Size(58, 15)
        Label10.TabIndex = 1
        Label10.Text = "New Cost"
        Label10.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' newCostText
        ' 
        newCostText.Font = New Font("Segoe UI", 11.0F)
        newCostText.Location = New Point(3, 31)
        newCostText.Name = "newCostText"
        newCostText.Size = New Size(350, 27)
        newCostText.TabIndex = 2
        ' 
        ' Panel5
        ' 
        Panel5.Controls.Add(Label6)
        Panel5.Controls.Add(newProductText)
        Panel5.Dock = DockStyle.Fill
        Panel5.Location = New Point(3, 63)
        Panel5.Name = "Panel5"
        Panel5.Size = New Size(356, 54)
        Panel5.TabIndex = 5
        ' 
        ' Label6
        ' 
        Label6.AutoSize = True
        Label6.Location = New Point(3, 10)
        Label6.Name = "Label6"
        Label6.Size = New Size(111, 15)
        Label6.TabIndex = 1
        Label6.Text = "New Product Name"
        Label6.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' newProductText
        ' 
        newProductText.Font = New Font("Segoe UI", 11.0F)
        newProductText.Location = New Point(3, 31)
        newProductText.Name = "newProductText"
        newProductText.Size = New Size(350, 27)
        newProductText.TabIndex = 2
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
        Panel8.Controls.Add(newDateText)
        Panel8.Controls.Add(Label9)
        Panel8.Dock = DockStyle.Fill
        Panel8.Location = New Point(0, 0)
        Panel8.Name = "Panel8"
        Panel8.Size = New Size(356, 54)
        Panel8.TabIndex = 12
        ' 
        ' newDateText
        ' 
        newDateText.Location = New Point(4, 33)
        newDateText.MinDate = New Date(2025, 9, 24, 0, 0, 0, 0)
        newDateText.Name = "newDateText"
        newDateText.Size = New Size(200, 23)
        newDateText.TabIndex = 11
        ' 
        ' Label9
        ' 
        Label9.AutoSize = True
        Label9.Location = New Point(3, 10)
        Label9.Name = "Label9"
        Label9.Size = New Size(113, 15)
        Label9.TabIndex = 1
        Label9.Text = "New Expiration Date"
        Label9.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' Panel10
        ' 
        Panel10.Controls.Add(productDropDown)
        Panel10.Controls.Add(Label2)
        Panel10.Dock = DockStyle.Fill
        Panel10.Location = New Point(3, 3)
        Panel10.Name = "Panel10"
        Panel10.Size = New Size(356, 54)
        Panel10.TabIndex = 4
        ' 
        ' productDropDown
        ' 
        productDropDown.Font = New Font("Segoe UI", 11.0F)
        productDropDown.FormattingEnabled = True
        productDropDown.Location = New Point(3, 32)
        productDropDown.Name = "productDropDown"
        productDropDown.Size = New Size(350, 28)
        productDropDown.TabIndex = 3
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
        ' Panel4
        ' 
        Panel4.Controls.Add(Label5)
        Panel4.Controls.Add(newUnitText)
        Panel4.Dock = DockStyle.Fill
        Panel4.Location = New Point(3, 243)
        Panel4.Name = "Panel4"
        Panel4.Size = New Size(356, 54)
        Panel4.TabIndex = 8
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.Location = New Point(3, 10)
        Label5.Name = "Label5"
        Label5.Size = New Size(56, 15)
        Label5.TabIndex = 1
        Label5.Text = "New Unit"
        Label5.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' newUnitText
        ' 
        newUnitText.Font = New Font("Segoe UI", 11.0F)
        newUnitText.Location = New Point(3, 31)
        newUnitText.Name = "newUnitText"
        newUnitText.Size = New Size(350, 27)
        newUnitText.TabIndex = 2
        ' 
        ' Panel3
        ' 
        Panel3.Controls.Add(Label4)
        Panel3.Controls.Add(newQuantityText)
        Panel3.Dock = DockStyle.Fill
        Panel3.Location = New Point(3, 183)
        Panel3.Name = "Panel3"
        Panel3.Size = New Size(356, 54)
        Panel3.TabIndex = 7
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Location = New Point(3, 10)
        Label4.Name = "Label4"
        Label4.Size = New Size(80, 15)
        Label4.TabIndex = 1
        Label4.Text = "New Quantity"
        Label4.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' newQuantityText
        ' 
        newQuantityText.Font = New Font("Segoe UI", 11.0F)
        newQuantityText.Location = New Point(3, 31)
        newQuantityText.Name = "newQuantityText"
        newQuantityText.Size = New Size(350, 27)
        newQuantityText.TabIndex = 2
        ' 
        ' Panel2
        ' 
        Panel2.Controls.Add(newCategoryDropdown)
        Panel2.Controls.Add(Label3)
        Panel2.Dock = DockStyle.Fill
        Panel2.Location = New Point(3, 123)
        Panel2.Name = "Panel2"
        Panel2.Size = New Size(356, 54)
        Panel2.TabIndex = 6
        ' 
        ' newCategoryDropdown
        ' 
        newCategoryDropdown.Font = New Font("Segoe UI", 11.0F)
        newCategoryDropdown.FormattingEnabled = True
        newCategoryDropdown.Location = New Point(3, 31)
        newCategoryDropdown.Name = "newCategoryDropdown"
        newCategoryDropdown.Size = New Size(350, 28)
        newCategoryDropdown.TabIndex = 2
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(3, 10)
        Label3.Name = "Label3"
        Label3.Size = New Size(82, 15)
        Label3.TabIndex = 1
        Label3.Text = "New Category"
        Label3.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' Panel7
        ' 
        Panel7.Controls.Add(Label8)
        Panel7.Controls.Add(newReorderText)
        Panel7.Dock = DockStyle.Fill
        Panel7.Location = New Point(3, 423)
        Panel7.Name = "Panel7"
        Panel7.Size = New Size(356, 54)
        Panel7.TabIndex = 11
        ' 
        ' Label8
        ' 
        Label8.AutoSize = True
        Label8.Location = New Point(3, 10)
        Label8.Name = "Label8"
        Label8.Size = New Size(105, 15)
        Label8.TabIndex = 1
        Label8.Text = "New Reorder Level"
        Label8.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' newReorderText
        ' 
        newReorderText.Font = New Font("Segoe UI", 11.0F)
        newReorderText.Location = New Point(3, 31)
        newReorderText.Name = "newReorderText"
        newReorderText.Size = New Size(350, 27)
        newReorderText.TabIndex = 2
        ' 
        ' Panel6
        ' 
        Panel6.Controls.Add(Label7)
        Panel6.Controls.Add(newRetailText)
        Panel6.Dock = DockStyle.Fill
        Panel6.ImeMode = ImeMode.NoControl
        Panel6.Location = New Point(3, 363)
        Panel6.Name = "Panel6"
        Panel6.Size = New Size(356, 54)
        Panel6.TabIndex = 10
        ' 
        ' Label7
        ' 
        Label7.AutoSize = True
        Label7.Location = New Point(3, 10)
        Label7.Name = "Label7"
        Label7.Size = New Size(92, 15)
        Label7.TabIndex = 1
        Label7.Text = "New Retail Price"
        Label7.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' newRetailText
        ' 
        newRetailText.Font = New Font("Segoe UI", 11.0F)
        newRetailText.Location = New Point(3, 31)
        newRetailText.Name = "newRetailText"
        newRetailText.Size = New Size(350, 27)
        newRetailText.TabIndex = 2
        ' 
        ' TableLayoutPanel2
        ' 
        TableLayoutPanel2.ColumnCount = 3
        TableLayoutPanel2.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 33.3333321F))
        TableLayoutPanel2.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 33.3333321F))
        TableLayoutPanel2.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 33.3333321F))
        TableLayoutPanel2.Controls.Add(deleteButton, 1, 0)
        TableLayoutPanel2.Controls.Add(updateButton, 0, 0)
        TableLayoutPanel2.Controls.Add(cancelButton, 2, 0)
        TableLayoutPanel2.Dock = DockStyle.Fill
        TableLayoutPanel2.Location = New Point(3, 603)
        TableLayoutPanel2.Name = "TableLayoutPanel2"
        TableLayoutPanel2.RowCount = 1
        TableLayoutPanel2.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        TableLayoutPanel2.Size = New Size(356, 60)
        TableLayoutPanel2.TabIndex = 14
        ' 
        ' deleteButton
        ' 
        deleteButton.Anchor = AnchorStyles.None
        deleteButton.AutoSize = True
        deleteButton.BackColor = Color.Red
        deleteButton.Cursor = Cursors.Hand
        deleteButton.FlatAppearance.BorderSize = 0
        deleteButton.FlatStyle = FlatStyle.Popup
        deleteButton.Font = New Font("Segoe UI Semibold", 12.0F, FontStyle.Bold)
        deleteButton.Location = New Point(131, 14)
        deleteButton.Name = "deleteButton"
        deleteButton.Size = New Size(91, 31)
        deleteButton.TabIndex = 16
        deleteButton.Text = "DELETE"
        deleteButton.UseVisualStyleBackColor = False
        ' 
        ' updateButton
        ' 
        updateButton.Anchor = AnchorStyles.None
        updateButton.AutoSize = True
        updateButton.Cursor = Cursors.Hand
        updateButton.FlatAppearance.BorderSize = 0
        updateButton.FlatStyle = FlatStyle.Popup
        updateButton.Font = New Font("Segoe UI Semibold", 12.0F, FontStyle.Bold)
        updateButton.Location = New Point(20, 14)
        updateButton.Name = "updateButton"
        updateButton.Size = New Size(78, 31)
        updateButton.TabIndex = 15
        updateButton.Text = "UPDATE"
        updateButton.UseVisualStyleBackColor = True
        ' 
        ' cancelButton
        ' 
        cancelButton.Anchor = AnchorStyles.None
        cancelButton.AutoSize = True
        cancelButton.Cursor = Cursors.Hand
        cancelButton.FlatAppearance.BorderSize = 0
        cancelButton.FlatStyle = FlatStyle.Popup
        cancelButton.Font = New Font("Segoe UI Semibold", 12.0F, FontStyle.Bold)
        cancelButton.Location = New Point(250, 14)
        cancelButton.Name = "cancelButton"
        cancelButton.Size = New Size(91, 31)
        cancelButton.TabIndex = 17
        cancelButton.Text = "CANCEL"
        cancelButton.UseVisualStyleBackColor = True
        ' 
        ' editItemForm
        ' 
        AutoScaleDimensions = New SizeF(7.0F, 15.0F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(362, 735)
        Controls.Add(TableLayoutPanel1)
        Controls.Add(Panel1)
        FormBorderStyle = FormBorderStyle.FixedToolWindow
        Name = "editItemForm"
        StartPosition = FormStartPosition.CenterParent
        Panel1.ResumeLayout(False)
        TableLayoutPanel1.ResumeLayout(False)
        Panel12.ResumeLayout(False)
        Panel11.ResumeLayout(False)
        Panel11.PerformLayout()
        Panel5.ResumeLayout(False)
        Panel5.PerformLayout()
        Panel9.ResumeLayout(False)
        Panel8.ResumeLayout(False)
        Panel8.PerformLayout()
        Panel10.ResumeLayout(False)
        Panel10.PerformLayout()
        Panel4.ResumeLayout(False)
        Panel4.PerformLayout()
        Panel3.ResumeLayout(False)
        Panel3.PerformLayout()
        Panel2.ResumeLayout(False)
        Panel2.PerformLayout()
        Panel7.ResumeLayout(False)
        Panel7.PerformLayout()
        Panel6.ResumeLayout(False)
        Panel6.PerformLayout()
        TableLayoutPanel2.ResumeLayout(False)
        TableLayoutPanel2.PerformLayout()
        ResumeLayout(False)
    End Sub

    Friend WithEvents Panel1 As Panel
    Friend WithEvents Label1 As Label
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents TableLayoutPanel2 As TableLayoutPanel
    Friend WithEvents cancelButton As Button
    Friend WithEvents updateButton As Button
    Friend WithEvents Panel8 As Panel
    Friend WithEvents Label9 As Label
    Friend WithEvents Panel7 As Panel
    Friend WithEvents Label8 As Label
    Friend WithEvents newReorderText As TextBox
    Friend WithEvents Panel6 As Panel
    Friend WithEvents Label7 As Label
    Friend WithEvents newRetailText As TextBox
    Friend WithEvents Panel4 As Panel
    Friend WithEvents Label5 As Label
    Friend WithEvents newUnitText As TextBox
    Friend WithEvents Panel3 As Panel
    Friend WithEvents Label4 As Label
    Friend WithEvents newQuantityText As TextBox
    Friend WithEvents Panel2 As Panel
    Friend WithEvents newCategoryDropdown As ComboBox
    Friend WithEvents Label3 As Label
    Friend WithEvents Panel10 As Panel
    Friend WithEvents Label2 As Label
    Friend WithEvents deleteButton As Button
    Friend WithEvents Panel5 As Panel
    Friend WithEvents Label6 As Label
    Friend WithEvents newProductText As TextBox
    Friend WithEvents Panel9 As Panel
    Friend WithEvents productDropDown As ComboBox
    Friend WithEvents newDateText As DateTimePicker
    Friend WithEvents Panel11 As Panel
    Friend WithEvents Label10 As Label
    Friend WithEvents newCostText As TextBox
    Friend WithEvents Panel12 As Panel
    Friend WithEvents editDiscountButton As Button
End Class
