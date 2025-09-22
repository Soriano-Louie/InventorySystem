<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class editUserForm
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
        TableLayoutPanel2 = New TableLayoutPanel()
        cancelButton = New Button()
        saveButton = New Button()
        Panel1 = New Panel()
        Label3 = New Label()
        TextBox2 = New TextBox()
        Panel10 = New Panel()
        Label2 = New Label()
        ComboBox2 = New ComboBox()
        Label4 = New Label()
        ComboBox1 = New ComboBox()
        Panel2 = New Panel()
        TableLayoutPanel1.SuspendLayout()
        TableLayoutPanel2.SuspendLayout()
        Panel1.SuspendLayout()
        Panel10.SuspendLayout()
        Panel2.SuspendLayout()
        SuspendLayout()
        ' 
        ' Label1
        ' 
        Label1.Dock = DockStyle.Top
        Label1.Font = New Font("Segoe UI", 15.75F, FontStyle.Bold)
        Label1.Location = New Point(0, 0)
        Label1.Name = "Label1"
        Label1.Size = New Size(364, 69)
        Label1.TabIndex = 3
        Label1.Text = "Edit User"
        Label1.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' TableLayoutPanel1
        ' 
        TableLayoutPanel1.ColumnCount = 1
        TableLayoutPanel1.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100F))
        TableLayoutPanel1.Controls.Add(TableLayoutPanel2, 0, 3)
        TableLayoutPanel1.Controls.Add(Panel2, 0, 2)
        TableLayoutPanel1.Controls.Add(Panel1, 0, 1)
        TableLayoutPanel1.Controls.Add(Panel10, 0, 0)
        TableLayoutPanel1.Dock = DockStyle.Fill
        TableLayoutPanel1.Location = New Point(0, 69)
        TableLayoutPanel1.Name = "TableLayoutPanel1"
        TableLayoutPanel1.RowCount = 4
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Percent, 25F))
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Percent, 25F))
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Percent, 25F))
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Percent, 25F))
        TableLayoutPanel1.Size = New Size(364, 313)
        TableLayoutPanel1.TabIndex = 4
        ' 
        ' TableLayoutPanel2
        ' 
        TableLayoutPanel2.ColumnCount = 2
        TableLayoutPanel2.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50F))
        TableLayoutPanel2.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50F))
        TableLayoutPanel2.Controls.Add(cancelButton, 1, 0)
        TableLayoutPanel2.Controls.Add(saveButton, 0, 0)
        TableLayoutPanel2.Dock = DockStyle.Fill
        TableLayoutPanel2.Location = New Point(3, 237)
        TableLayoutPanel2.Name = "TableLayoutPanel2"
        TableLayoutPanel2.RowCount = 1
        TableLayoutPanel2.RowStyles.Add(New RowStyle(SizeType.Percent, 50F))
        TableLayoutPanel2.Size = New Size(358, 73)
        TableLayoutPanel2.TabIndex = 9
        ' 
        ' cancelButton
        ' 
        cancelButton.Anchor = AnchorStyles.None
        cancelButton.AutoSize = True
        cancelButton.Cursor = Cursors.Hand
        cancelButton.FlatAppearance.BorderSize = 0
        cancelButton.FlatStyle = FlatStyle.Popup
        cancelButton.Font = New Font("Segoe UI Semibold", 12F, FontStyle.Bold)
        cancelButton.Location = New Point(223, 21)
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
        saveButton.Location = New Point(52, 21)
        saveButton.Name = "saveButton"
        saveButton.Size = New Size(75, 31)
        saveButton.TabIndex = 15
        saveButton.Text = "SAVE"
        saveButton.UseVisualStyleBackColor = True
        ' 
        ' Panel1
        ' 
        Panel1.Controls.Add(Label3)
        Panel1.Controls.Add(TextBox2)
        Panel1.Dock = DockStyle.Fill
        Panel1.Location = New Point(3, 81)
        Panel1.Name = "Panel1"
        Panel1.Size = New Size(358, 72)
        Panel1.TabIndex = 7
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(3, 10)
        Label3.Name = "Label3"
        Label3.Size = New Size(57, 15)
        Label3.TabIndex = 1
        Label3.Text = "Password"
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
        Panel10.Controls.Add(ComboBox2)
        Panel10.Controls.Add(Label2)
        Panel10.Dock = DockStyle.Fill
        Panel10.Location = New Point(3, 3)
        Panel10.Name = "Panel10"
        Panel10.Size = New Size(358, 72)
        Panel10.TabIndex = 6
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(3, 10)
        Label2.Name = "Label2"
        Label2.Size = New Size(60, 15)
        Label2.TabIndex = 1
        Label2.Text = "Username"
        Label2.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' ComboBox2
        ' 
        ComboBox2.FormattingEnabled = True
        ComboBox2.Location = New Point(4, 35)
        ComboBox2.Name = "ComboBox2"
        ComboBox2.Size = New Size(350, 23)
        ComboBox2.TabIndex = 3
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Location = New Point(3, 10)
        Label4.Name = "Label4"
        Label4.Size = New Size(30, 15)
        Label4.TabIndex = 1
        Label4.Text = "Role"
        Label4.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' ComboBox1
        ' 
        ComboBox1.FormattingEnabled = True
        ComboBox1.Location = New Point(3, 31)
        ComboBox1.Name = "ComboBox1"
        ComboBox1.Size = New Size(350, 23)
        ComboBox1.TabIndex = 2
        ' 
        ' Panel2
        ' 
        Panel2.Controls.Add(ComboBox1)
        Panel2.Controls.Add(Label4)
        Panel2.Dock = DockStyle.Fill
        Panel2.Location = New Point(3, 159)
        Panel2.Name = "Panel2"
        Panel2.Size = New Size(358, 72)
        Panel2.TabIndex = 8
        ' 
        ' editUserForm
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(364, 382)
        Controls.Add(TableLayoutPanel1)
        Controls.Add(Label1)
        FormBorderStyle = FormBorderStyle.FixedToolWindow
        Name = "editUserForm"
        StartPosition = FormStartPosition.CenterParent
        TableLayoutPanel1.ResumeLayout(False)
        TableLayoutPanel2.ResumeLayout(False)
        TableLayoutPanel2.PerformLayout()
        Panel1.ResumeLayout(False)
        Panel1.PerformLayout()
        Panel10.ResumeLayout(False)
        Panel10.PerformLayout()
        Panel2.ResumeLayout(False)
        Panel2.PerformLayout()
        ResumeLayout(False)
    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents TableLayoutPanel2 As TableLayoutPanel
    Friend WithEvents cancelButton As Button
    Friend WithEvents saveButton As Button
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Label3 As Label
    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents Panel10 As Panel
    Friend WithEvents Label2 As Label
    Friend WithEvents ComboBox2 As ComboBox
    Friend WithEvents Panel2 As Panel
    Friend WithEvents ComboBox1 As ComboBox
    Friend WithEvents Label4 As Label
End Class
