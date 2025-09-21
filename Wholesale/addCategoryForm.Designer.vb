<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class addCategoryForm
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
        TableLayoutPanel1 = New TableLayoutPanel()
        Panel1 = New Panel()
        Label3 = New Label()
        TextBox2 = New TextBox()
        Panel10 = New Panel()
        Label2 = New Label()
        TextBox1 = New TextBox()
        TableLayoutPanel2 = New TableLayoutPanel()
        cancelButton = New Button()
        saveButton = New Button()
        TableLayoutPanel1.SuspendLayout()
        Panel1.SuspendLayout()
        Panel10.SuspendLayout()
        TableLayoutPanel2.SuspendLayout()
        SuspendLayout()
        ' 
        ' Label1
        ' 
        Label1.Dock = DockStyle.Top
        Label1.Font = New Font("Segoe UI", 15.75F, FontStyle.Bold)
        Label1.Location = New Point(0, 0)
        Label1.Name = "Label1"
        Label1.Size = New Size(364, 69)
        Label1.TabIndex = 1
        Label1.Text = "Add Category"
        Label1.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' TableLayoutPanel1
        ' 
        TableLayoutPanel1.ColumnCount = 1
        TableLayoutPanel1.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100F))
        TableLayoutPanel1.Controls.Add(Panel1, 0, 1)
        TableLayoutPanel1.Controls.Add(Panel10, 0, 0)
        TableLayoutPanel1.Controls.Add(TableLayoutPanel2, 0, 2)
        TableLayoutPanel1.Dock = DockStyle.Fill
        TableLayoutPanel1.Location = New Point(0, 69)
        TableLayoutPanel1.Name = "TableLayoutPanel1"
        TableLayoutPanel1.RowCount = 3
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Percent, 33.3333321F))
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Percent, 33.3333321F))
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Percent, 33.3333321F))
        TableLayoutPanel1.Size = New Size(364, 239)
        TableLayoutPanel1.TabIndex = 2
        ' 
        ' Panel1
        ' 
        Panel1.Controls.Add(Label3)
        Panel1.Controls.Add(TextBox2)
        Panel1.Dock = DockStyle.Fill
        Panel1.Location = New Point(3, 82)
        Panel1.Name = "Panel1"
        Panel1.Size = New Size(358, 73)
        Panel1.TabIndex = 6
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(3, 10)
        Label3.Name = "Label3"
        Label3.Size = New Size(122, 15)
        Label3.TabIndex = 1
        Label3.Text = "Description (optional)"
        Label3.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' TextBox2
        ' 
        TextBox2.Font = New Font("Segoe UI", 11F)
        TextBox2.Location = New Point(3, 31)
        TextBox2.Name = "TextBox2"
        TextBox2.Size = New Size(350, 27)
        TextBox2.TabIndex = 2
        ' 
        ' Panel10
        ' 
        Panel10.Controls.Add(Label2)
        Panel10.Controls.Add(TextBox1)
        Panel10.Dock = DockStyle.Fill
        Panel10.Location = New Point(3, 3)
        Panel10.Name = "Panel10"
        Panel10.Size = New Size(358, 73)
        Panel10.TabIndex = 5
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(3, 10)
        Label2.Name = "Label2"
        Label2.Size = New Size(90, 15)
        Label2.TabIndex = 1
        Label2.Text = "Category Name"
        Label2.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' TextBox1
        ' 
        TextBox1.Font = New Font("Segoe UI", 11F)
        TextBox1.Location = New Point(3, 31)
        TextBox1.Name = "TextBox1"
        TextBox1.Size = New Size(350, 27)
        TextBox1.TabIndex = 2
        ' 
        ' TableLayoutPanel2
        ' 
        TableLayoutPanel2.ColumnCount = 2
        TableLayoutPanel2.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50F))
        TableLayoutPanel2.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50F))
        TableLayoutPanel2.Controls.Add(cancelButton, 1, 0)
        TableLayoutPanel2.Controls.Add(saveButton, 0, 0)
        TableLayoutPanel2.Dock = DockStyle.Fill
        TableLayoutPanel2.Location = New Point(3, 161)
        TableLayoutPanel2.Name = "TableLayoutPanel2"
        TableLayoutPanel2.RowCount = 1
        TableLayoutPanel2.RowStyles.Add(New RowStyle(SizeType.Percent, 50F))
        TableLayoutPanel2.Size = New Size(358, 75)
        TableLayoutPanel2.TabIndex = 0
        ' 
        ' cancelButton
        ' 
        cancelButton.Anchor = AnchorStyles.None
        cancelButton.AutoSize = True
        cancelButton.Cursor = Cursors.Hand
        cancelButton.FlatAppearance.BorderSize = 0
        cancelButton.FlatStyle = FlatStyle.Popup
        cancelButton.Font = New Font("Segoe UI Semibold", 12F, FontStyle.Bold)
        cancelButton.Location = New Point(223, 22)
        cancelButton.Name = "cancelButton"
        cancelButton.Size = New Size(91, 31)
        cancelButton.TabIndex = 16
        cancelButton.Text = "CANCEL"
        cancelButton.UseVisualStyleBackColor = True
        ' 
        ' saveButton
        ' 
        saveButton.Anchor = AnchorStyles.None
        saveButton.AutoSize = True
        saveButton.Cursor = Cursors.Hand
        saveButton.FlatAppearance.BorderSize = 0
        saveButton.FlatStyle = FlatStyle.Popup
        saveButton.Font = New Font("Segoe UI Semibold", 12F, FontStyle.Bold)
        saveButton.Location = New Point(52, 22)
        saveButton.Name = "saveButton"
        saveButton.Size = New Size(75, 31)
        saveButton.TabIndex = 15
        saveButton.Text = "SAVE"
        saveButton.UseVisualStyleBackColor = True
        ' 
        ' addCategoryForm
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(364, 308)
        Controls.Add(TableLayoutPanel1)
        Controls.Add(Label1)
        FormBorderStyle = FormBorderStyle.FixedToolWindow
        Name = "addCategoryForm"
        StartPosition = FormStartPosition.CenterParent
        TableLayoutPanel1.ResumeLayout(False)
        Panel1.ResumeLayout(False)
        Panel1.PerformLayout()
        Panel10.ResumeLayout(False)
        Panel10.PerformLayout()
        TableLayoutPanel2.ResumeLayout(False)
        TableLayoutPanel2.PerformLayout()
        ResumeLayout(False)
    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents TableLayoutPanel2 As TableLayoutPanel
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Label3 As Label
    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents Panel10 As Panel
    Friend WithEvents Label2 As Label
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents saveButton As Button
    Friend WithEvents cancelButton As Button
End Class
