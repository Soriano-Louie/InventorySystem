<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class userManagementForm
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
        mainPanel = New Panel()
        Panel1 = New Panel()
        TableLayoutPanel1 = New TableLayoutPanel()
        Panel2 = New Panel()
        TextBoxSearch = New TextBox()
        editUserButton = New Button()
        addUserButton = New Button()
        tableDataGridView = New DataGridView()
        mainPanel.SuspendLayout()
        Panel1.SuspendLayout()
        TableLayoutPanel1.SuspendLayout()
        Panel2.SuspendLayout()
        CType(tableDataGridView, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' mainPanel
        ' 
        mainPanel.Controls.Add(Panel1)
        mainPanel.Controls.Add(tableDataGridView)
        mainPanel.Dock = DockStyle.Fill
        mainPanel.Location = New Point(0, 0)
        mainPanel.Name = "mainPanel"
        mainPanel.Padding = New Padding(50, 120, 50, 20)
        mainPanel.Size = New Size(1810, 792)
        mainPanel.TabIndex = 0
        ' 
        ' Panel1
        ' 
        Panel1.Anchor = AnchorStyles.Top
        Panel1.Controls.Add(TableLayoutPanel1)
        Panel1.Location = New Point(0, 0)
        Panel1.Name = "Panel1"
        Panel1.Size = New Size(1810, 114)
        Panel1.TabIndex = 1
        ' 
        ' TableLayoutPanel1
        ' 
        TableLayoutPanel1.ColumnCount = 2
        TableLayoutPanel1.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50F))
        TableLayoutPanel1.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50F))
        TableLayoutPanel1.Controls.Add(Panel2, 0, 1)
        TableLayoutPanel1.Controls.Add(editUserButton, 1, 0)
        TableLayoutPanel1.Controls.Add(addUserButton, 0, 0)
        TableLayoutPanel1.Location = New Point(634, 3)
        TableLayoutPanel1.Name = "TableLayoutPanel1"
        TableLayoutPanel1.RowCount = 2
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Percent, 55.5555573F))
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Percent, 44.4444427F))
        TableLayoutPanel1.Size = New Size(510, 108)
        TableLayoutPanel1.TabIndex = 2
        ' 
        ' Panel2
        ' 
        Panel2.Anchor = AnchorStyles.None
        TableLayoutPanel1.SetColumnSpan(Panel2, 2)
        Panel2.Controls.Add(TextBoxSearch)
        Panel2.Location = New Point(155, 67)
        Panel2.Name = "Panel2"
        Panel2.Size = New Size(200, 34)
        Panel2.TabIndex = 3
        ' 
        ' TextBoxSearch
        ' 
        TextBoxSearch.BorderStyle = BorderStyle.FixedSingle
        TextBoxSearch.Dock = DockStyle.Bottom
        TextBoxSearch.Font = New Font("Segoe UI", 13F)
        TextBoxSearch.Location = New Point(0, 3)
        TextBoxSearch.Name = "TextBoxSearch"
        TextBoxSearch.Size = New Size(200, 31)
        TextBoxSearch.TabIndex = 0
        ' 
        ' editUserButton
        ' 
        editUserButton.Anchor = AnchorStyles.None
        editUserButton.Cursor = Cursors.Hand
        editUserButton.FlatAppearance.BorderSize = 0
        editUserButton.FlatStyle = FlatStyle.Popup
        editUserButton.Font = New Font("Segoe UI", 14F)
        editUserButton.Location = New Point(316, 12)
        editUserButton.Name = "editUserButton"
        editUserButton.Size = New Size(132, 36)
        editUserButton.TabIndex = 1
        editUserButton.Text = "Edit User"
        editUserButton.UseVisualStyleBackColor = True
        ' 
        ' addUserButton
        ' 
        addUserButton.Anchor = AnchorStyles.None
        addUserButton.Cursor = Cursors.Hand
        addUserButton.FlatAppearance.BorderSize = 0
        addUserButton.FlatStyle = FlatStyle.Popup
        addUserButton.Font = New Font("Segoe UI", 14F)
        addUserButton.Location = New Point(61, 12)
        addUserButton.Name = "addUserButton"
        addUserButton.Size = New Size(132, 36)
        addUserButton.TabIndex = 0
        addUserButton.Text = "Add User"
        addUserButton.UseVisualStyleBackColor = True
        ' 
        ' tableDataGridView
        ' 
        tableDataGridView.AllowUserToAddRows = False
        tableDataGridView.AllowUserToDeleteRows = False
        tableDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        tableDataGridView.Dock = DockStyle.Fill
        tableDataGridView.Location = New Point(50, 120)
        tableDataGridView.Name = "tableDataGridView"
        tableDataGridView.ReadOnly = True
        tableDataGridView.Size = New Size(1710, 652)
        tableDataGridView.TabIndex = 0
        ' 
        ' userManagementForm
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1810, 792)
        Controls.Add(mainPanel)
        Name = "userManagementForm"
        StartPosition = FormStartPosition.CenterScreen
        Text = "userSettingsForm"
        WindowState = FormWindowState.Maximized
        mainPanel.ResumeLayout(False)
        Panel1.ResumeLayout(False)
        TableLayoutPanel1.ResumeLayout(False)
        Panel2.ResumeLayout(False)
        Panel2.PerformLayout()
        CType(tableDataGridView, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub

    Friend WithEvents mainPanel As Panel
    Friend WithEvents tableDataGridView As DataGridView
    Friend WithEvents Panel1 As Panel
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents addUserButton As Button
    Friend WithEvents editUserButton As Button
    Friend WithEvents Panel2 As Panel
    Friend WithEvents TextBoxSearch As TextBox
End Class
