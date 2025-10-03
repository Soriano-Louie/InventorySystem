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
        Panel3 = New Panel()
        newUserText = New TextBox()
        Label5 = New Label()
        Panel10 = New Panel()
        userDropDown = New ComboBox()
        Label2 = New Label()
        TableLayoutPanel2 = New TableLayoutPanel()
        deleteButton = New Button()
        saveButton = New Button()
        cancelButton = New Button()
        Panel2 = New Panel()
        newRoleDropdown = New ComboBox()
        Label4 = New Label()
        Panel1 = New Panel()
        Label3 = New Label()
        newPasswordText = New TextBox()
        TableLayoutPanel1.SuspendLayout()
        Panel3.SuspendLayout()
        Panel10.SuspendLayout()
        TableLayoutPanel2.SuspendLayout()
        Panel2.SuspendLayout()
        Panel1.SuspendLayout()
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
        TableLayoutPanel1.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TableLayoutPanel1.Controls.Add(Panel3, 0, 1)
        TableLayoutPanel1.Controls.Add(Panel10, 0, 0)
        TableLayoutPanel1.Controls.Add(TableLayoutPanel2, 0, 4)
        TableLayoutPanel1.Controls.Add(Panel2, 0, 3)
        TableLayoutPanel1.Controls.Add(Panel1, 0, 2)
        TableLayoutPanel1.Dock = DockStyle.Fill
        TableLayoutPanel1.Location = New Point(0, 69)
        TableLayoutPanel1.Name = "TableLayoutPanel1"
        TableLayoutPanel1.RowCount = 5
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Percent, 20.0F))
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Percent, 20.0F))
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Percent, 20.0F))
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Percent, 20.0F))
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Percent, 20.0F))
        TableLayoutPanel1.Size = New Size(364, 313)
        TableLayoutPanel1.TabIndex = 4
        ' 
        ' Panel3
        ' 
        Panel3.Controls.Add(newUserText)
        Panel3.Controls.Add(Label5)
        Panel3.Dock = DockStyle.Fill
        Panel3.Location = New Point(3, 65)
        Panel3.Name = "Panel3"
        Panel3.Size = New Size(358, 56)
        Panel3.TabIndex = 2
        ' 
        ' newUserText
        ' 
        newUserText.Font = New Font("Segoe UI", 11.0F)
        newUserText.Location = New Point(4, 33)
        newUserText.Name = "newUserText"
        newUserText.Size = New Size(350, 27)
        newUserText.TabIndex = 2
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.Location = New Point(3, 7)
        Label5.Name = "Label5"
        Label5.Size = New Size(87, 15)
        Label5.TabIndex = 1
        Label5.Text = "New Username"
        Label5.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' Panel10
        ' 
        Panel10.Controls.Add(userDropDown)
        Panel10.Controls.Add(Label2)
        Panel10.Dock = DockStyle.Fill
        Panel10.Location = New Point(3, 3)
        Panel10.Name = "Panel10"
        Panel10.Size = New Size(358, 56)
        Panel10.TabIndex = 1
        ' 
        ' userDropDown
        ' 
        userDropDown.FormattingEnabled = True
        userDropDown.Location = New Point(4, 30)
        userDropDown.Name = "userDropDown"
        userDropDown.Size = New Size(350, 23)
        userDropDown.TabIndex = 1
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(3, 7)
        Label2.Name = "Label2"
        Label2.Size = New Size(60, 15)
        Label2.TabIndex = 1
        Label2.Text = "Username"
        Label2.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' TableLayoutPanel2
        ' 
        TableLayoutPanel2.ColumnCount = 3
        TableLayoutPanel2.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 33.3333321F))
        TableLayoutPanel2.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 33.3333321F))
        TableLayoutPanel2.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 33.3333321F))
        TableLayoutPanel2.Controls.Add(deleteButton, 1, 0)
        TableLayoutPanel2.Controls.Add(saveButton, 0, 0)
        TableLayoutPanel2.Controls.Add(cancelButton, 2, 0)
        TableLayoutPanel2.Dock = DockStyle.Fill
        TableLayoutPanel2.Location = New Point(3, 251)
        TableLayoutPanel2.Name = "TableLayoutPanel2"
        TableLayoutPanel2.RowCount = 1
        TableLayoutPanel2.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        TableLayoutPanel2.Size = New Size(358, 59)
        TableLayoutPanel2.TabIndex = 5
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
        deleteButton.Location = New Point(133, 14)
        deleteButton.Name = "deleteButton"
        deleteButton.Size = New Size(91, 31)
        deleteButton.TabIndex = 6
        deleteButton.Text = "DELETE"
        deleteButton.UseVisualStyleBackColor = False
        ' 
        ' saveButton
        ' 
        saveButton.Anchor = AnchorStyles.None
        saveButton.AutoSize = True
        saveButton.Cursor = Cursors.Hand
        saveButton.FlatAppearance.BorderSize = 0
        saveButton.FlatStyle = FlatStyle.Popup
        saveButton.Font = New Font("Segoe UI Semibold", 12.0F, FontStyle.Bold)
        saveButton.Location = New Point(22, 14)
        saveButton.Name = "saveButton"
        saveButton.Size = New Size(75, 31)
        saveButton.TabIndex = 5
        saveButton.Text = "SAVE"
        saveButton.UseVisualStyleBackColor = True
        ' 
        ' cancelButton
        ' 
        cancelButton.Anchor = AnchorStyles.None
        cancelButton.AutoSize = True
        cancelButton.Cursor = Cursors.Hand
        cancelButton.FlatAppearance.BorderSize = 0
        cancelButton.FlatStyle = FlatStyle.Popup
        cancelButton.Font = New Font("Segoe UI Semibold", 12.0F, FontStyle.Bold)
        cancelButton.Location = New Point(258, 14)
        cancelButton.Name = "cancelButton"
        cancelButton.Size = New Size(79, 31)
        cancelButton.TabIndex = 7
        cancelButton.Text = "CANCEL"
        cancelButton.UseVisualStyleBackColor = True
        ' 
        ' Panel2
        ' 
        Panel2.Controls.Add(newRoleDropdown)
        Panel2.Controls.Add(Label4)
        Panel2.Dock = DockStyle.Fill
        Panel2.Location = New Point(3, 189)
        Panel2.Name = "Panel2"
        Panel2.Size = New Size(358, 56)
        Panel2.TabIndex = 4
        ' 
        ' newRoleDropdown
        ' 
        newRoleDropdown.FormattingEnabled = True
        newRoleDropdown.Location = New Point(3, 27)
        newRoleDropdown.Name = "newRoleDropdown"
        newRoleDropdown.Size = New Size(350, 23)
        newRoleDropdown.TabIndex = 4
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Location = New Point(3, 7)
        Label4.Name = "Label4"
        Label4.Size = New Size(57, 15)
        Label4.TabIndex = 1
        Label4.Text = "New Role"
        Label4.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' Panel1
        ' 
        Panel1.Controls.Add(Label3)
        Panel1.Controls.Add(newPasswordText)
        Panel1.Dock = DockStyle.Fill
        Panel1.Location = New Point(3, 127)
        Panel1.Name = "Panel1"
        Panel1.Size = New Size(358, 56)
        Panel1.TabIndex = 3
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(3, 7)
        Label3.Name = "Label3"
        Label3.Size = New Size(84, 15)
        Label3.TabIndex = 1
        Label3.Text = "New Password"
        Label3.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' newPasswordText
        ' 
        newPasswordText.Font = New Font("Segoe UI", 11.0F)
        newPasswordText.Location = New Point(3, 27)
        newPasswordText.Name = "newPasswordText"
        newPasswordText.Size = New Size(350, 27)
        newPasswordText.TabIndex = 3
        ' 
        ' editUserForm
        ' 
        AutoScaleDimensions = New SizeF(7.0F, 15.0F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(364, 382)
        Controls.Add(TableLayoutPanel1)
        Controls.Add(Label1)
        FormBorderStyle = FormBorderStyle.FixedToolWindow
        Name = "editUserForm"
        StartPosition = FormStartPosition.CenterParent
        TableLayoutPanel1.ResumeLayout(False)
        Panel3.ResumeLayout(False)
        Panel3.PerformLayout()
        Panel10.ResumeLayout(False)
        Panel10.PerformLayout()
        TableLayoutPanel2.ResumeLayout(False)
        TableLayoutPanel2.PerformLayout()
        Panel2.ResumeLayout(False)
        Panel2.PerformLayout()
        Panel1.ResumeLayout(False)
        Panel1.PerformLayout()
        ResumeLayout(False)
    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents TableLayoutPanel2 As TableLayoutPanel
    Friend WithEvents cancelButton As Button
    Friend WithEvents saveButton As Button
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Label3 As Label
    Friend WithEvents newPasswordText As TextBox
    Friend WithEvents Panel10 As Panel
    Friend WithEvents Label2 As Label
    Friend WithEvents userDropDown As ComboBox
    Friend WithEvents Panel2 As Panel
    Friend WithEvents newRoleDropdown As ComboBox
    Friend WithEvents Label4 As Label
    Friend WithEvents Panel3 As Panel
    Friend WithEvents newUserText As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents deleteButton As Button
End Class
