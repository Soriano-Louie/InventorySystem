<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class inventoryRetail
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
        Panel1 = New Panel()
        TableLayoutPanel1 = New TableLayoutPanel()
        btnPrintAllQRCodes = New Button()
        Button1 = New Button()
        Panel2 = New Panel()
        TextBoxSearch = New TextBox()
        Button2 = New Button()
        tableDataGridView = New DataGridView()
        dataGridViewPanel.SuspendLayout()
        Panel1.SuspendLayout()
        TableLayoutPanel1.SuspendLayout()
        Panel2.SuspendLayout()
        CType(tableDataGridView, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' dataGridViewPanel
        ' 
        dataGridViewPanel.Controls.Add(Panel1)
        dataGridViewPanel.Controls.Add(tableDataGridView)
        dataGridViewPanel.Dock = DockStyle.Fill
        dataGridViewPanel.Location = New Point(0, 0)
        dataGridViewPanel.Name = "dataGridViewPanel"
        dataGridViewPanel.Padding = New Padding(50, 120, 50, 20)
        dataGridViewPanel.Size = New Size(1684, 791)
        dataGridViewPanel.TabIndex = 0
        ' 
        ' Panel1
        ' 
        Panel1.Anchor = AnchorStyles.Top
        Panel1.Controls.Add(TableLayoutPanel1)
        Panel1.Location = New Point(0, 0)
        Panel1.Name = "Panel1"
        Panel1.Size = New Size(1684, 114)
        Panel1.TabIndex = 2
        ' 
        ' TableLayoutPanel1
        ' 
        TableLayoutPanel1.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        TableLayoutPanel1.ColumnCount = 3
        TableLayoutPanel1.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 33.33334F))
        TableLayoutPanel1.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 33.33333F))
        TableLayoutPanel1.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 33.3333321F))
        TableLayoutPanel1.Controls.Add(btnPrintAllQRCodes, 1, 0)
        TableLayoutPanel1.Controls.Add(Button1, 0, 0)
        TableLayoutPanel1.Controls.Add(Panel2, 0, 1)
        TableLayoutPanel1.Controls.Add(Button2, 2, 0)
        TableLayoutPanel1.Location = New Point(405, 8)
        TableLayoutPanel1.Name = "TableLayoutPanel1"
        TableLayoutPanel1.RowCount = 2
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Percent, 60F))
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Percent, 40F))
        TableLayoutPanel1.Size = New Size(875, 99)
        TableLayoutPanel1.TabIndex = 5
        ' 
        ' btnPrintAllQRCodes
        ' 
        btnPrintAllQRCodes.Anchor = AnchorStyles.None
        btnPrintAllQRCodes.AutoSize = True
        btnPrintAllQRCodes.Cursor = Cursors.Hand
        btnPrintAllQRCodes.FlatAppearance.BorderSize = 0
        btnPrintAllQRCodes.FlatStyle = FlatStyle.Popup
        btnPrintAllQRCodes.Font = New Font("Segoe UI", 14F)
        btnPrintAllQRCodes.Location = New Point(322, 8)
        btnPrintAllQRCodes.Name = "btnPrintAllQRCodes"
        btnPrintAllQRCodes.Size = New Size(229, 42)
        btnPrintAllQRCodes.TabIndex = 3
        btnPrintAllQRCodes.Text = "Print All QR Codes"
        btnPrintAllQRCodes.UseVisualStyleBackColor = True
        ' 
        ' Button1
        ' 
        Button1.Anchor = AnchorStyles.None
        Button1.AutoSize = True
        Button1.Cursor = Cursors.Hand
        Button1.FlatAppearance.BorderSize = 0
        Button1.FlatStyle = FlatStyle.Popup
        Button1.Font = New Font("Segoe UI", 14F)
        Button1.Location = New Point(70, 8)
        Button1.Name = "Button1"
        Button1.Size = New Size(151, 42)
        Button1.TabIndex = 0
        Button1.Text = "Add Item"
        Button1.UseVisualStyleBackColor = True
        ' 
        ' Panel2
        ' 
        Panel2.Anchor = AnchorStyles.None
        TableLayoutPanel1.SetColumnSpan(Panel2, 3)
        Panel2.Controls.Add(TextBoxSearch)
        Panel2.Location = New Point(337, 62)
        Panel2.Name = "Panel2"
        Panel2.Size = New Size(200, 34)
        Panel2.TabIndex = 2
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
        ' Button2
        ' 
        Button2.Anchor = AnchorStyles.None
        Button2.AutoSize = True
        Button2.Cursor = Cursors.Hand
        Button2.FlatAppearance.BorderSize = 0
        Button2.FlatStyle = FlatStyle.Popup
        Button2.Font = New Font("Segoe UI", 14F)
        Button2.Location = New Point(654, 8)
        Button2.Name = "Button2"
        Button2.Size = New Size(149, 42)
        Button2.TabIndex = 1
        Button2.Text = "Edit Item"
        Button2.UseVisualStyleBackColor = True
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
        tableDataGridView.TabIndex = 1
        ' 
        ' inventoryRetail
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1684, 791)
        Controls.Add(dataGridViewPanel)
        FormBorderStyle = FormBorderStyle.FixedToolWindow
        Name = "inventoryRetail"
        StartPosition = FormStartPosition.CenterScreen
        WindowState = FormWindowState.Maximized
        dataGridViewPanel.ResumeLayout(False)
        Panel1.ResumeLayout(False)
        TableLayoutPanel1.ResumeLayout(False)
        TableLayoutPanel1.PerformLayout()
        Panel2.ResumeLayout(False)
        Panel2.PerformLayout()
        CType(tableDataGridView, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub

    Friend WithEvents dataGridViewPanel As Panel
    Friend WithEvents tableDataGridView As DataGridView
    Friend WithEvents Panel1 As Panel
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents btnPrintAllQRCodes As Button
    Friend WithEvents Button1 As Button
    Friend WithEvents Panel2 As Panel
    Friend WithEvents TextBoxSearch As TextBox
    Friend WithEvents Button2 As Button
End Class
