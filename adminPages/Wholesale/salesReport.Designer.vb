<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class salesReport
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
        Button2 = New Button()
        Panel2 = New Panel()
        TextBoxSearch = New TextBox()
        Button1 = New Button()
        resetButton = New Button()
        btnSearch = New Button()
        Panel3 = New Panel()
        Panel5 = New Panel()
        DateTimePickerTo = New DateTimePicker()
        Label2 = New Label()
        Panel4 = New Panel()
        DateTimePickerFrom = New DateTimePicker()
        Label1 = New Label()
        tableDataGridView = New DataGridView()
        mainPanel.SuspendLayout()
        Panel1.SuspendLayout()
        TableLayoutPanel1.SuspendLayout()
        Panel2.SuspendLayout()
        Panel3.SuspendLayout()
        Panel5.SuspendLayout()
        Panel4.SuspendLayout()
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
        TableLayoutPanel1.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        TableLayoutPanel1.ColumnCount = 5
        TableLayoutPanel1.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 20.563036F))
        TableLayoutPanel1.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 18.72705F))
        TableLayoutPanel1.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 41.1260719F))
        TableLayoutPanel1.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 10.8935127F))
        TableLayoutPanel1.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 8.445533F))
        TableLayoutPanel1.Controls.Add(Button2, 1, 0)
        TableLayoutPanel1.Controls.Add(Panel2, 0, 1)
        TableLayoutPanel1.Controls.Add(Button1, 0, 0)
        TableLayoutPanel1.Controls.Add(resetButton, 4, 0)
        TableLayoutPanel1.Controls.Add(btnSearch, 3, 0)
        TableLayoutPanel1.Controls.Add(Panel3, 2, 0)
        TableLayoutPanel1.Location = New Point(503, 12)
        TableLayoutPanel1.Name = "TableLayoutPanel1"
        TableLayoutPanel1.RowCount = 2
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Percent, 57.5757561F))
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Percent, 42.4242439F))
        TableLayoutPanel1.Size = New Size(817, 99)
        TableLayoutPanel1.TabIndex = 6
        ' 
        ' Button2
        ' 
        Button2.Anchor = AnchorStyles.None
        Button2.AutoSize = True
        Button2.Cursor = Cursors.Hand
        Button2.FlatAppearance.BorderSize = 0
        Button2.FlatStyle = FlatStyle.Popup
        Button2.Font = New Font("Segoe UI", 14F)
        Button2.Location = New Point(175, 11)
        Button2.Name = "Button2"
        Button2.Size = New Size(138, 35)
        Button2.TabIndex = 5
        Button2.Text = "Export To PDF"
        Button2.UseVisualStyleBackColor = True
        ' 
        ' Panel2
        ' 
        Panel2.Anchor = AnchorStyles.None
        TableLayoutPanel1.SetColumnSpan(Panel2, 55)
        Panel2.Controls.Add(TextBoxSearch)
        Panel2.Location = New Point(308, 61)
        Panel2.Name = "Panel2"
        Panel2.Size = New Size(200, 34)
        Panel2.TabIndex = 2
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
        ' Button1
        ' 
        Button1.Anchor = AnchorStyles.None
        Button1.AutoSize = True
        Button1.Cursor = Cursors.Hand
        Button1.FlatAppearance.BorderSize = 0
        Button1.FlatStyle = FlatStyle.Popup
        Button1.Font = New Font("Segoe UI", 14F)
        Button1.Location = New Point(10, 11)
        Button1.Name = "Button1"
        Button1.Size = New Size(148, 35)
        Button1.TabIndex = 0
        Button1.Text = "Export To Excel"
        Button1.UseVisualStyleBackColor = True
        ' 
        ' resetButton
        ' 
        resetButton.Anchor = AnchorStyles.None
        resetButton.Cursor = Cursors.Hand
        resetButton.FlatAppearance.BorderSize = 3
        resetButton.FlatStyle = FlatStyle.Popup
        resetButton.Location = New Point(754, 17)
        resetButton.Name = "resetButton"
        resetButton.Size = New Size(55, 23)
        resetButton.TabIndex = 4
        resetButton.Text = "Reset"
        resetButton.UseVisualStyleBackColor = True
        ' 
        ' btnSearch
        ' 
        btnSearch.Anchor = AnchorStyles.None
        btnSearch.Cursor = Cursors.Hand
        btnSearch.FlatAppearance.BorderSize = 3
        btnSearch.FlatStyle = FlatStyle.Popup
        btnSearch.Location = New Point(670, 17)
        btnSearch.Name = "btnSearch"
        btnSearch.Size = New Size(63, 23)
        btnSearch.TabIndex = 1
        btnSearch.Text = "Search"
        btnSearch.UseVisualStyleBackColor = True
        ' 
        ' Panel3
        ' 
        Panel3.Controls.Add(Panel5)
        Panel3.Controls.Add(Panel4)
        Panel3.Dock = DockStyle.Fill
        Panel3.Location = New Point(324, 3)
        Panel3.Name = "Panel3"
        Panel3.Size = New Size(330, 51)
        Panel3.TabIndex = 3
        ' 
        ' Panel5
        ' 
        Panel5.Controls.Add(DateTimePickerTo)
        Panel5.Controls.Add(Label2)
        Panel5.Dock = DockStyle.Fill
        Panel5.Location = New Point(173, 0)
        Panel5.Name = "Panel5"
        Panel5.Size = New Size(157, 51)
        Panel5.TabIndex = 6
        ' 
        ' DateTimePickerTo
        ' 
        DateTimePickerTo.Format = DateTimePickerFormat.Custom
        DateTimePickerTo.Location = New Point(43, 13)
        DateTimePickerTo.Name = "DateTimePickerTo"
        DateTimePickerTo.Size = New Size(98, 23)
        DateTimePickerTo.TabIndex = 8
        ' 
        ' Label2
        ' 
        Label2.Anchor = AnchorStyles.None
        Label2.AutoSize = True
        Label2.Font = New Font("Segoe UI", 10F)
        Label2.Location = New Point(11, 16)
        Label2.Name = "Label2"
        Label2.Size = New Size(26, 19)
        Label2.TabIndex = 6
        Label2.Text = "To:"
        Label2.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' Panel4
        ' 
        Panel4.Controls.Add(DateTimePickerFrom)
        Panel4.Controls.Add(Label1)
        Panel4.Dock = DockStyle.Left
        Panel4.Location = New Point(0, 0)
        Panel4.Name = "Panel4"
        Panel4.Size = New Size(173, 51)
        Panel4.TabIndex = 2
        ' 
        ' DateTimePickerFrom
        ' 
        DateTimePickerFrom.Format = DateTimePickerFormat.Custom
        DateTimePickerFrom.Location = New Point(57, 16)
        DateTimePickerFrom.Name = "DateTimePickerFrom"
        DateTimePickerFrom.Size = New Size(98, 23)
        DateTimePickerFrom.TabIndex = 7
        ' 
        ' Label1
        ' 
        Label1.Anchor = AnchorStyles.None
        Label1.AutoSize = True
        Label1.Font = New Font("Segoe UI", 10F)
        Label1.Location = New Point(7, 16)
        Label1.Name = "Label1"
        Label1.Size = New Size(44, 19)
        Label1.TabIndex = 2
        Label1.Text = "From:"
        Label1.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' tableDataGridView
        ' 
        tableDataGridView.AllowUserToAddRows = False
        tableDataGridView.AllowUserToDeleteRows = False
        tableDataGridView.AllowUserToResizeColumns = False
        tableDataGridView.AllowUserToResizeRows = False
        tableDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        tableDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        tableDataGridView.Dock = DockStyle.Fill
        tableDataGridView.Location = New Point(50, 120)
        tableDataGridView.Name = "tableDataGridView"
        tableDataGridView.ReadOnly = True
        tableDataGridView.Size = New Size(1710, 652)
        tableDataGridView.TabIndex = 0
        ' 
        ' salesReport
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1810, 792)
        Controls.Add(mainPanel)
        Name = "salesReport"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Sales Report"
        WindowState = FormWindowState.Maximized
        mainPanel.ResumeLayout(False)
        Panel1.ResumeLayout(False)
        TableLayoutPanel1.ResumeLayout(False)
        TableLayoutPanel1.PerformLayout()
        Panel2.ResumeLayout(False)
        Panel2.PerformLayout()
        Panel3.ResumeLayout(False)
        Panel5.ResumeLayout(False)
        Panel5.PerformLayout()
        Panel4.ResumeLayout(False)
        Panel4.PerformLayout()
        CType(tableDataGridView, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub

    Friend WithEvents mainPanel As Panel
    Friend WithEvents tableDataGridView As DataGridView
    Friend WithEvents Panel1 As Panel
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents Button1 As Button
    Friend WithEvents Panel2 As Panel
    Friend WithEvents TextBoxSearch As TextBox
    Friend WithEvents Panel3 As Panel
    Friend WithEvents Panel4 As Panel
    Friend WithEvents Label1 As Label
    Friend WithEvents Panel5 As Panel
    Friend WithEvents Label2 As Label
    Friend WithEvents DateTimePickerTo As DateTimePicker
    Friend WithEvents DateTimePickerFrom As DateTimePicker
    Friend WithEvents btnSearch As Button
    Friend WithEvents resetButton As Button
    Friend WithEvents Button2 As Button
End Class
