<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class discountForm
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
        Label1 = New Label()
        dgvDiscounts = New DataGridView()
        TableLayoutPanel1 = New TableLayoutPanel()
        deleteButton = New Button()
        Panel3 = New Panel()
        txtDiscountPrice = New TextBox()
        Label4 = New Label()
        Panel2 = New Panel()
        txtMaxSacks = New TextBox()
        Label3 = New Label()
        Panel1 = New Panel()
        txtMinSacks = New TextBox()
        Label2 = New Label()
        saveButton = New Button()
        CType(dgvDiscounts, ComponentModel.ISupportInitialize).BeginInit()
        TableLayoutPanel1.SuspendLayout()
        Panel3.SuspendLayout()
        Panel2.SuspendLayout()
        Panel1.SuspendLayout()
        SuspendLayout()
        ' 
        ' Label1
        ' 
        Label1.Dock = DockStyle.Top
        Label1.Font = New Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Label1.Location = New Point(0, 0)
        Label1.Name = "Label1"
        Label1.Size = New Size(595, 69)
        Label1.TabIndex = 1
        Label1.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' dgvDiscounts
        ' 
        dgvDiscounts.AllowUserToAddRows = False
        dgvDiscounts.AllowUserToDeleteRows = False
        dgvDiscounts.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        dgvDiscounts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgvDiscounts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvDiscounts.Location = New Point(0, 200)
        dgvDiscounts.Name = "dgvDiscounts"
        dgvDiscounts.ReadOnly = True
        dgvDiscounts.Size = New Size(595, 263)
        dgvDiscounts.TabIndex = 2
        ' 
        ' TableLayoutPanel1
        ' 
        TableLayoutPanel1.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        TableLayoutPanel1.ColumnCount = 3
        TableLayoutPanel1.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50F))
        TableLayoutPanel1.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50F))
        TableLayoutPanel1.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 100F))
        TableLayoutPanel1.Controls.Add(deleteButton, 2, 0)
        TableLayoutPanel1.Controls.Add(Panel3, 0, 1)
        TableLayoutPanel1.Controls.Add(Panel2, 1, 0)
        TableLayoutPanel1.Controls.Add(Panel1, 0, 0)
        TableLayoutPanel1.Controls.Add(saveButton, 1, 1)
        TableLayoutPanel1.Location = New Point(0, 72)
        TableLayoutPanel1.Name = "TableLayoutPanel1"
        TableLayoutPanel1.RowCount = 2
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Percent, 50F))
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Percent, 50F))
        TableLayoutPanel1.Size = New Size(595, 122)
        TableLayoutPanel1.TabIndex = 3
        ' 
        ' deleteButton
        ' 
        deleteButton.Anchor = AnchorStyles.None
        deleteButton.BackColor = Color.Red
        deleteButton.FlatStyle = FlatStyle.Popup
        deleteButton.Location = New Point(510, 47)
        deleteButton.Name = "deleteButton"
        TableLayoutPanel1.SetRowSpan(deleteButton, 2)
        deleteButton.Size = New Size(68, 27)
        deleteButton.TabIndex = 6
        deleteButton.Text = "DELETE"
        deleteButton.UseVisualStyleBackColor = False
        ' 
        ' Panel3
        ' 
        Panel3.Controls.Add(txtDiscountPrice)
        Panel3.Controls.Add(Label4)
        Panel3.Dock = DockStyle.Fill
        Panel3.Location = New Point(3, 64)
        Panel3.Name = "Panel3"
        Panel3.Size = New Size(241, 55)
        Panel3.TabIndex = 4
        ' 
        ' txtDiscountPrice
        ' 
        txtDiscountPrice.BorderStyle = BorderStyle.FixedSingle
        txtDiscountPrice.Location = New Point(54, 24)
        txtDiscountPrice.Name = "txtDiscountPrice"
        txtDiscountPrice.Size = New Size(100, 23)
        txtDiscountPrice.TabIndex = 1
        ' 
        ' Label4
        ' 
        Label4.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        Label4.AutoSize = True
        Label4.Location = New Point(57, 6)
        Label4.Name = "Label4"
        Label4.Size = New Size(96, 15)
        Label4.TabIndex = 0
        Label4.Text = "Discounted Price"
        Label4.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' Panel2
        ' 
        Panel2.Controls.Add(txtMaxSacks)
        Panel2.Controls.Add(Label3)
        Panel2.Dock = DockStyle.Fill
        Panel2.Location = New Point(250, 3)
        Panel2.Name = "Panel2"
        Panel2.Size = New Size(241, 55)
        Panel2.TabIndex = 3
        ' 
        ' txtMaxSacks
        ' 
        txtMaxSacks.BorderStyle = BorderStyle.FixedSingle
        txtMaxSacks.Location = New Point(70, 24)
        txtMaxSacks.Name = "txtMaxSacks"
        txtMaxSacks.Size = New Size(100, 23)
        txtMaxSacks.TabIndex = 1
        ' 
        ' Label3
        ' 
        Label3.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        Label3.AutoSize = True
        Label3.Location = New Point(51, 6)
        Label3.Name = "Label3"
        Label3.Size = New Size(151, 15)
        Label3.TabIndex = 0
        Label3.Text = "Maximum number of sacks"
        Label3.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' Panel1
        ' 
        Panel1.Controls.Add(txtMinSacks)
        Panel1.Controls.Add(Label2)
        Panel1.Dock = DockStyle.Fill
        Panel1.Location = New Point(3, 3)
        Panel1.Name = "Panel1"
        Panel1.Size = New Size(241, 55)
        Panel1.TabIndex = 0
        ' 
        ' txtMinSacks
        ' 
        txtMinSacks.BorderStyle = BorderStyle.FixedSingle
        txtMinSacks.Location = New Point(55, 24)
        txtMinSacks.Name = "txtMinSacks"
        txtMinSacks.Size = New Size(100, 23)
        txtMinSacks.TabIndex = 1
        ' 
        ' Label2
        ' 
        Label2.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        Label2.AutoSize = True
        Label2.Location = New Point(33, 6)
        Label2.Name = "Label2"
        Label2.Size = New Size(150, 15)
        Label2.TabIndex = 0
        Label2.Text = "Minimum number of sacks"
        Label2.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' saveButton
        ' 
        saveButton.Anchor = AnchorStyles.None
        saveButton.FlatStyle = FlatStyle.Popup
        saveButton.Location = New Point(336, 78)
        saveButton.Name = "saveButton"
        saveButton.Size = New Size(68, 27)
        saveButton.TabIndex = 5
        saveButton.Text = "SAVE"
        saveButton.UseVisualStyleBackColor = True
        ' 
        ' discountForm
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(595, 463)
        Controls.Add(TableLayoutPanel1)
        Controls.Add(dgvDiscounts)
        Controls.Add(Label1)
        FormBorderStyle = FormBorderStyle.FixedToolWindow
        Name = "discountForm"
        StartPosition = FormStartPosition.CenterScreen
        CType(dgvDiscounts, ComponentModel.ISupportInitialize).EndInit()
        TableLayoutPanel1.ResumeLayout(False)
        Panel3.ResumeLayout(False)
        Panel3.PerformLayout()
        Panel2.ResumeLayout(False)
        Panel2.PerformLayout()
        Panel1.ResumeLayout(False)
        Panel1.PerformLayout()
        ResumeLayout(False)
    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents dgvDiscounts As DataGridView
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Panel3 As Panel
    Friend WithEvents txtDiscountPrice As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents Panel2 As Panel
    Friend WithEvents txtMaxSacks As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents txtMinSacks As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents deleteButton As Button
    Friend WithEvents saveButton As Button
End Class
