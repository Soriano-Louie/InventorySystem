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
        TextBoxSearch = New TextBox()
        addProductPanel = New Panel()
        TableLayoutPanel1 = New TableLayoutPanel()
        Button2 = New Button()
        Button1 = New Button()
        tableDataGridView = New DataGridView()
        dataGridViewPanel.SuspendLayout()
        addProductPanel.SuspendLayout()
        TableLayoutPanel1.SuspendLayout()
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
        dataGridViewPanel.Size = New Size(1810, 945)
        dataGridViewPanel.TabIndex = 0
        ' 
        ' TextBoxSearch
        ' 
        TextBoxSearch.Anchor = AnchorStyles.None
        TableLayoutPanel1.SetColumnSpan(TextBoxSearch, 2)
        TextBoxSearch.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        TextBoxSearch.Location = New Point(505, 64)
        TextBoxSearch.MaximumSize = New Size(165, 29)
        TextBoxSearch.MinimumSize = New Size(165, 29)
        TextBoxSearch.Name = "TextBoxSearch"
        TextBoxSearch.Size = New Size(165, 29)
        TextBoxSearch.TabIndex = 3
        ' 
        ' addProductPanel
        ' 
        addProductPanel.Anchor = AnchorStyles.Top
        addProductPanel.Controls.Add(TableLayoutPanel1)
        addProductPanel.Location = New Point(0, 4)
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
        TableLayoutPanel1.Controls.Add(TextBoxSearch, 0, 1)
        TableLayoutPanel1.Controls.Add(Button2, 1, 0)
        TableLayoutPanel1.Controls.Add(Button1, 0, 0)
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
        Button2.Location = New Point(773, 12)
        Button2.Name = "Button2"
        Button2.Size = New Size(216, 35)
        Button2.TabIndex = 1
        Button2.Text = "EDIT ITEM"
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
        Button1.Location = New Point(185, 12)
        Button1.Name = "Button1"
        Button1.Size = New Size(216, 35)
        Button1.TabIndex = 0
        Button1.Text = "ADD ITEM"
        Button1.UseVisualStyleBackColor = True
        ' 
        ' tableDataGridView
        ' 
        tableDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        tableDataGridView.CellBorderStyle = DataGridViewCellBorderStyle.Raised
        tableDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        tableDataGridView.Dock = DockStyle.Fill
        tableDataGridView.Location = New Point(50, 120)
        tableDataGridView.Name = "tableDataGridView"
        tableDataGridView.Size = New Size(1710, 805)
        tableDataGridView.TabIndex = 0
        ' 
        ' InventoryForm
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1810, 945)
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
        CType(tableDataGridView, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub

    Friend WithEvents dataGridViewPanel As Panel
    Friend WithEvents tableDataGridView As DataGridView
    Friend WithEvents addProductPanel As Panel
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents TextBoxSearch As TextBox
    Friend WithEvents Button1 As Button
    Friend WithEvents Button2 As Button
End Class
