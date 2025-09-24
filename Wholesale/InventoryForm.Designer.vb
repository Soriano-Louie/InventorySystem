<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class InventoryForm
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
        dataGridViewPanel = New Panel()
        addProductPanel = New Panel()
        TableLayoutPanel1 = New TableLayoutPanel()
        Button2 = New Button()
        Button1 = New Button()
        Panel1 = New Panel()
        TextBoxSearch = New TextBox()
        tableDataGridView = New DataGridView()
        dataGridViewPanel.SuspendLayout()
        addProductPanel.SuspendLayout()
        TableLayoutPanel1.SuspendLayout()
        Panel1.SuspendLayout()
        CType(tableDataGridView, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' dataGridViewPanel
        ' 
        dataGridViewPanel.Controls.Add(addProductPanel)
        dataGridViewPanel.Controls.Add(tableDataGridView)
        dataGridViewPanel.Dock = DockStyle.Fill
        dataGridViewPanel.Location = New Point(0, 0)
        dataGridViewPanel.Name = "dataGridViewPanel"
        dataGridViewPanel.Padding = New Padding(50, 120, 50, 20)
        dataGridViewPanel.Size = New Size(1684, 791)
        dataGridViewPanel.TabIndex = 0
        ' 
        ' addProductPanel
        ' 
        addProductPanel.Anchor = AnchorStyles.Top
        addProductPanel.Controls.Add(TableLayoutPanel1)
        addProductPanel.Location = New Point(-63, 4)
        addProductPanel.Margin = New Padding(3, 3, 3, 20)
        addProductPanel.Name = "addProductPanel"
        addProductPanel.Size = New Size(1810, 116)
        addProductPanel.TabIndex = 2
        ' 
        ' TableLayoutPanel1
        ' 
        TableLayoutPanel1.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        TableLayoutPanel1.ColumnCount = 2
        TableLayoutPanel1.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.00001F))
        TableLayoutPanel1.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 49.99999F))
        TableLayoutPanel1.Controls.Add(Button2, 1, 0)
        TableLayoutPanel1.Controls.Add(Button1, 0, 0)
        TableLayoutPanel1.Controls.Add(Panel1, 0, 1)
        TableLayoutPanel1.Location = New Point(301, 14)
        TableLayoutPanel1.Name = "TableLayoutPanel1"
        TableLayoutPanel1.RowCount = 2
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Percent, 60F))
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Percent, 40F))
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Absolute, 20F))
        TableLayoutPanel1.Size = New Size(1175, 99)
        TableLayoutPanel1.TabIndex = 4
        ' 
        ' Button2
        ' 
        Button2.Anchor = AnchorStyles.None
        Button2.AutoSize = True
        Button2.Cursor = Cursors.Hand
        Button2.FlatAppearance.BorderSize = 0
        Button2.FlatStyle = FlatStyle.Popup
        Button2.Font = New Font("Segoe UI", 14F)
        Button2.Location = New Point(781, 8)
        Button2.Name = "Button2"
        Button2.Size = New Size(199, 42)
        Button2.TabIndex = 1
        Button2.Text = "Edit Item"
        Button2.UseVisualStyleBackColor = True
        ' 
        ' Button1
        ' 
        Button1.Anchor = AnchorStyles.None
        Button1.AutoSize = True
        Button1.Cursor = Cursors.Hand
        Button1.FlatAppearance.BorderSize = 0
        Button1.FlatStyle = FlatStyle.Popup
        Button1.Font = New Font("Segoe UI", 14F)
        Button1.Location = New Point(194, 8)
        Button1.Name = "Button1"
        Button1.Size = New Size(199, 42)
        Button1.TabIndex = 0
        Button1.Text = "Add Item"
        Button1.UseVisualStyleBackColor = True
        ' 
        ' Panel1
        ' 
        Panel1.Anchor = AnchorStyles.None
        TableLayoutPanel1.SetColumnSpan(Panel1, 2)
        Panel1.Controls.Add(TextBoxSearch)
        Panel1.Location = New Point(487, 62)
        Panel1.Name = "Panel1"
        Panel1.Size = New Size(200, 34)
        Panel1.TabIndex = 2
        ' 
        ' TextBoxSearch
        ' 
        TextBoxSearch.BorderStyle = BorderStyle.FixedSingle
        TextBoxSearch.Dock = DockStyle.Fill
        TextBoxSearch.Font = New Font("Segoe UI", 13F)
        TextBoxSearch.Location = New Point(0, 0)
        TextBoxSearch.Name = "TextBoxSearch"
        TextBoxSearch.Size = New Size(200, 31)
        TextBoxSearch.TabIndex = 0
        ' 
        ' tableDataGridView
        ' 
        tableDataGridView.AllowUserToAddRows = False
        tableDataGridView.AllowUserToDeleteRows = False
        tableDataGridView.AllowUserToResizeColumns = False
        tableDataGridView.AllowUserToResizeRows = False
        tableDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        tableDataGridView.CellBorderStyle = DataGridViewCellBorderStyle.Raised
        tableDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        tableDataGridView.Dock = DockStyle.Fill
        tableDataGridView.Location = New Point(50, 120)
        tableDataGridView.Name = "tableDataGridView"
        tableDataGridView.RowHeadersWidth = 51
        tableDataGridView.Size = New Size(1584, 651)
        tableDataGridView.TabIndex = 0
        ' 
        ' InventoryForm
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1684, 791)
        Controls.Add(dataGridViewPanel)
        FormBorderStyle = FormBorderStyle.FixedToolWindow
        Name = "InventoryForm"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Inventory"
        WindowState = FormWindowState.Maximized
        dataGridViewPanel.ResumeLayout(False)
        addProductPanel.ResumeLayout(False)
        TableLayoutPanel1.ResumeLayout(False)
        TableLayoutPanel1.PerformLayout()
        Panel1.ResumeLayout(False)
        Panel1.PerformLayout()
        CType(tableDataGridView, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub

    Friend WithEvents dataGridViewPanel As Panel
    Friend WithEvents tableDataGridView As DataGridView
    Friend WithEvents addProductPanel As Panel
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents Button1 As Button
    Friend WithEvents Button2 As Button
    Friend WithEvents Panel1 As Panel
    Friend WithEvents TextBoxSearch As TextBox
End Class
