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
        Panel9 = New Panel()
        TableLayoutPanel2 = New TableLayoutPanel()
        deleteButton = New Button()
        updateButton = New Button()
        cancelButton = New Button()
        Panel8 = New Panel()
        Label9 = New Label()
        TextBox8 = New TextBox()
        Panel7 = New Panel()
        Label8 = New Label()
        TextBox7 = New TextBox()
        Panel6 = New Panel()
        Label7 = New Label()
        TextBox6 = New TextBox()
        Panel5 = New Panel()
        Label6 = New Label()
        TextBox5 = New TextBox()
        Panel4 = New Panel()
        Label5 = New Label()
        TextBox4 = New TextBox()
        Panel3 = New Panel()
        Label4 = New Label()
        TextBox3 = New TextBox()
        Panel2 = New Panel()
        ComboBox1 = New ComboBox()
        Label3 = New Label()
        Panel10 = New Panel()
        Label2 = New Label()
        TextBox1 = New TextBox()
        Panel1.SuspendLayout()
        TableLayoutPanel1.SuspendLayout()
        Panel9.SuspendLayout()
        TableLayoutPanel2.SuspendLayout()
        Panel8.SuspendLayout()
        Panel7.SuspendLayout()
        Panel6.SuspendLayout()
        Panel5.SuspendLayout()
        Panel4.SuspendLayout()
        Panel3.SuspendLayout()
        Panel2.SuspendLayout()
        Panel10.SuspendLayout()
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
        TableLayoutPanel1.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100F))
        TableLayoutPanel1.Controls.Add(Panel9, 0, 8)
        TableLayoutPanel1.Controls.Add(Panel8, 0, 7)
        TableLayoutPanel1.Controls.Add(Panel7, 0, 6)
        TableLayoutPanel1.Controls.Add(Panel6, 0, 5)
        TableLayoutPanel1.Controls.Add(Panel5, 0, 4)
        TableLayoutPanel1.Controls.Add(Panel4, 0, 3)
        TableLayoutPanel1.Controls.Add(Panel3, 0, 2)
        TableLayoutPanel1.Controls.Add(Panel2, 0, 1)
        TableLayoutPanel1.Controls.Add(Panel10, 0, 0)
        TableLayoutPanel1.Dock = DockStyle.Fill
        TableLayoutPanel1.Location = New Point(0, 69)
        TableLayoutPanel1.Name = "TableLayoutPanel1"
        TableLayoutPanel1.RowCount = 9
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Percent, 11.1111107F))
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Percent, 11.1111107F))
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Percent, 11.1111107F))
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Percent, 11.1111107F))
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Percent, 11.1111107F))
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Percent, 11.1111107F))
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Percent, 11.1111107F))
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Percent, 11.1111107F))
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Percent, 11.1111107F))
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Absolute, 20F))
        TableLayoutPanel1.Size = New Size(362, 666)
        TableLayoutPanel1.TabIndex = 4
        ' 
        ' Panel9
        ' 
        Panel9.Controls.Add(TableLayoutPanel2)
        Panel9.Dock = DockStyle.Fill
        Panel9.Location = New Point(3, 587)
        Panel9.Name = "Panel9"
        Panel9.Size = New Size(356, 76)
        Panel9.TabIndex = 12
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
        TableLayoutPanel2.Location = New Point(0, 0)
        TableLayoutPanel2.Name = "TableLayoutPanel2"
        TableLayoutPanel2.RowCount = 1
        TableLayoutPanel2.RowStyles.Add(New RowStyle(SizeType.Percent, 100F))
        TableLayoutPanel2.Size = New Size(356, 76)
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
        deleteButton.Font = New Font("Segoe UI Semibold", 12F, FontStyle.Bold)
        deleteButton.Location = New Point(131, 22)
        deleteButton.Name = "deleteButton"
        deleteButton.Size = New Size(91, 31)
        deleteButton.TabIndex = 15
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
        updateButton.Font = New Font("Segoe UI Semibold", 12F, FontStyle.Bold)
        updateButton.Location = New Point(20, 22)
        updateButton.Name = "updateButton"
        updateButton.Size = New Size(78, 31)
        updateButton.TabIndex = 13
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
        cancelButton.Font = New Font("Segoe UI Semibold", 12F, FontStyle.Bold)
        cancelButton.Location = New Point(250, 22)
        cancelButton.Name = "cancelButton"
        cancelButton.Size = New Size(91, 31)
        cancelButton.TabIndex = 14
        cancelButton.Text = "CANCEL"
        cancelButton.UseVisualStyleBackColor = True
        ' 
        ' Panel8
        ' 
        Panel8.Controls.Add(Label9)
        Panel8.Controls.Add(TextBox8)
        Panel8.Dock = DockStyle.Fill
        Panel8.Location = New Point(3, 514)
        Panel8.Name = "Panel8"
        Panel8.Size = New Size(356, 67)
        Panel8.TabIndex = 11
        ' 
        ' Label9
        ' 
        Label9.AutoSize = True
        Label9.Location = New Point(3, 10)
        Label9.Name = "Label9"
        Label9.Size = New Size(86, 15)
        Label9.TabIndex = 1
        Label9.Text = "Expiration Date"
        Label9.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' TextBox8
        ' 
        TextBox8.Font = New Font("Segoe UI", 11F)
        TextBox8.Location = New Point(3, 31)
        TextBox8.Name = "TextBox8"
        TextBox8.Size = New Size(350, 27)
        TextBox8.TabIndex = 2
        ' 
        ' Panel7
        ' 
        Panel7.Controls.Add(Label8)
        Panel7.Controls.Add(TextBox7)
        Panel7.Dock = DockStyle.Fill
        Panel7.Location = New Point(3, 441)
        Panel7.Name = "Panel7"
        Panel7.Size = New Size(356, 67)
        Panel7.TabIndex = 10
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
        ' TextBox7
        ' 
        TextBox7.Font = New Font("Segoe UI", 11F)
        TextBox7.Location = New Point(3, 31)
        TextBox7.Name = "TextBox7"
        TextBox7.Size = New Size(350, 27)
        TextBox7.TabIndex = 2
        ' 
        ' Panel6
        ' 
        Panel6.Controls.Add(Label7)
        Panel6.Controls.Add(TextBox6)
        Panel6.Dock = DockStyle.Fill
        Panel6.Location = New Point(3, 368)
        Panel6.Name = "Panel6"
        Panel6.Size = New Size(356, 67)
        Panel6.TabIndex = 9
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
        ' TextBox6
        ' 
        TextBox6.Font = New Font("Segoe UI", 11F)
        TextBox6.Location = New Point(3, 31)
        TextBox6.Name = "TextBox6"
        TextBox6.Size = New Size(350, 27)
        TextBox6.TabIndex = 2
        ' 
        ' Panel5
        ' 
        Panel5.Controls.Add(Label6)
        Panel5.Controls.Add(TextBox5)
        Panel5.Dock = DockStyle.Fill
        Panel5.Location = New Point(3, 295)
        Panel5.Name = "Panel5"
        Panel5.Size = New Size(356, 67)
        Panel5.TabIndex = 8
        ' 
        ' Label6
        ' 
        Label6.AutoSize = True
        Label6.Location = New Point(3, 10)
        Label6.Name = "Label6"
        Label6.Size = New Size(90, 15)
        Label6.TabIndex = 1
        Label6.Text = "Wholesale Price"
        Label6.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' TextBox5
        ' 
        TextBox5.Font = New Font("Segoe UI", 11F)
        TextBox5.Location = New Point(3, 31)
        TextBox5.Name = "TextBox5"
        TextBox5.Size = New Size(350, 27)
        TextBox5.TabIndex = 2
        ' 
        ' Panel4
        ' 
        Panel4.Controls.Add(Label5)
        Panel4.Controls.Add(TextBox4)
        Panel4.Dock = DockStyle.Fill
        Panel4.Location = New Point(3, 222)
        Panel4.Name = "Panel4"
        Panel4.Size = New Size(356, 67)
        Panel4.TabIndex = 7
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.Location = New Point(3, 10)
        Label5.Name = "Label5"
        Label5.Size = New Size(29, 15)
        Label5.TabIndex = 1
        Label5.Text = "Unit"
        Label5.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' TextBox4
        ' 
        TextBox4.Font = New Font("Segoe UI", 11F)
        TextBox4.Location = New Point(3, 31)
        TextBox4.Name = "TextBox4"
        TextBox4.Size = New Size(350, 27)
        TextBox4.TabIndex = 2
        ' 
        ' Panel3
        ' 
        Panel3.Controls.Add(Label4)
        Panel3.Controls.Add(TextBox3)
        Panel3.Dock = DockStyle.Fill
        Panel3.Location = New Point(3, 149)
        Panel3.Name = "Panel3"
        Panel3.Size = New Size(356, 67)
        Panel3.TabIndex = 6
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
        ' TextBox3
        ' 
        TextBox3.Font = New Font("Segoe UI", 11F)
        TextBox3.Location = New Point(3, 31)
        TextBox3.Name = "TextBox3"
        TextBox3.Size = New Size(350, 27)
        TextBox3.TabIndex = 2
        ' 
        ' Panel2
        ' 
        Panel2.Controls.Add(ComboBox1)
        Panel2.Controls.Add(Label3)
        Panel2.Dock = DockStyle.Fill
        Panel2.Location = New Point(3, 76)
        Panel2.Name = "Panel2"
        Panel2.Size = New Size(356, 67)
        Panel2.TabIndex = 5
        ' 
        ' ComboBox1
        ' 
        ComboBox1.Font = New Font("Segoe UI", 11F)
        ComboBox1.FormattingEnabled = True
        ComboBox1.Location = New Point(3, 31)
        ComboBox1.Name = "ComboBox1"
        ComboBox1.Size = New Size(350, 28)
        ComboBox1.TabIndex = 2
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
        ' Panel10
        ' 
        Panel10.Controls.Add(Label2)
        Panel10.Controls.Add(TextBox1)
        Panel10.Dock = DockStyle.Fill
        Panel10.Location = New Point(3, 3)
        Panel10.Name = "Panel10"
        Panel10.Size = New Size(356, 67)
        Panel10.TabIndex = 4
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
        ' TextBox1
        ' 
        TextBox1.Font = New Font("Segoe UI", 11F)
        TextBox1.Location = New Point(3, 31)
        TextBox1.Name = "TextBox1"
        TextBox1.Size = New Size(350, 27)
        TextBox1.TabIndex = 2
        ' 
        ' editItemForm
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(362, 735)
        Controls.Add(TableLayoutPanel1)
        Controls.Add(Panel1)
        FormBorderStyle = FormBorderStyle.FixedToolWindow
        Name = "editItemForm"
        StartPosition = FormStartPosition.CenterParent
        Panel1.ResumeLayout(False)
        TableLayoutPanel1.ResumeLayout(False)
        Panel9.ResumeLayout(False)
        TableLayoutPanel2.ResumeLayout(False)
        TableLayoutPanel2.PerformLayout()
        Panel8.ResumeLayout(False)
        Panel8.PerformLayout()
        Panel7.ResumeLayout(False)
        Panel7.PerformLayout()
        Panel6.ResumeLayout(False)
        Panel6.PerformLayout()
        Panel5.ResumeLayout(False)
        Panel5.PerformLayout()
        Panel4.ResumeLayout(False)
        Panel4.PerformLayout()
        Panel3.ResumeLayout(False)
        Panel3.PerformLayout()
        Panel2.ResumeLayout(False)
        Panel2.PerformLayout()
        Panel10.ResumeLayout(False)
        Panel10.PerformLayout()
        ResumeLayout(False)
    End Sub

    Friend WithEvents Panel1 As Panel
    Friend WithEvents Label1 As Label
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents Panel9 As Panel
    Friend WithEvents TableLayoutPanel2 As TableLayoutPanel
    Friend WithEvents cancelButton As Button
    Friend WithEvents updateButton As Button
    Friend WithEvents Panel8 As Panel
    Friend WithEvents Label9 As Label
    Friend WithEvents TextBox8 As TextBox
    Friend WithEvents Panel7 As Panel
    Friend WithEvents Label8 As Label
    Friend WithEvents TextBox7 As TextBox
    Friend WithEvents Panel6 As Panel
    Friend WithEvents Label7 As Label
    Friend WithEvents TextBox6 As TextBox
    Friend WithEvents Panel5 As Panel
    Friend WithEvents Label6 As Label
    Friend WithEvents TextBox5 As TextBox
    Friend WithEvents Panel4 As Panel
    Friend WithEvents Label5 As Label
    Friend WithEvents TextBox4 As TextBox
    Friend WithEvents Panel3 As Panel
    Friend WithEvents Label4 As Label
    Friend WithEvents TextBox3 As TextBox
    Friend WithEvents Panel2 As Panel
    Friend WithEvents ComboBox1 As ComboBox
    Friend WithEvents Label3 As Label
    Friend WithEvents Panel10 As Panel
    Friend WithEvents Label2 As Label
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents deleteButton As Button
End Class
