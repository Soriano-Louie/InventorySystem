<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class deliveryLogsForm
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
        TableLayoutPanel2 = New TableLayoutPanel()
        resetButton = New Button()
        Panel5 = New Panel()
        DateTimePickerTo = New DateTimePicker()
        Label2 = New Label()
        Panel4 = New Panel()
        DateTimePickerFrom = New DateTimePicker()
        Label3 = New Label()
        filterButton = New Button()
        Label1 = New Label()
        tableDataGridView = New DataGridView()
        mainPanel.SuspendLayout()
        Panel1.SuspendLayout()
        TableLayoutPanel1.SuspendLayout()
        TableLayoutPanel2.SuspendLayout()
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
        TableLayoutPanel1.ColumnCount = 1
        TableLayoutPanel1.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100F))
        TableLayoutPanel1.Controls.Add(TableLayoutPanel2, 0, 1)
        TableLayoutPanel1.Controls.Add(Label1, 0, 0)
        TableLayoutPanel1.Dock = DockStyle.Fill
        TableLayoutPanel1.Location = New Point(0, 0)
        TableLayoutPanel1.Name = "TableLayoutPanel1"
        TableLayoutPanel1.RowCount = 2
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Percent, 45.6140366F))
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Percent, 54.3859634F))
        TableLayoutPanel1.Size = New Size(1810, 114)
        TableLayoutPanel1.TabIndex = 0
        ' 
        ' TableLayoutPanel2
        ' 
        TableLayoutPanel2.Anchor = AnchorStyles.None
        TableLayoutPanel2.ColumnCount = 4
        TableLayoutPanel2.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 25F))
        TableLayoutPanel2.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 25F))
        TableLayoutPanel2.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 25F))
        TableLayoutPanel2.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 25F))
        TableLayoutPanel2.Controls.Add(resetButton, 3, 0)
        TableLayoutPanel2.Controls.Add(Panel5, 1, 0)
        TableLayoutPanel2.Controls.Add(Panel4, 0, 0)
        TableLayoutPanel2.Controls.Add(filterButton, 2, 0)
        TableLayoutPanel2.Location = New Point(588, 55)
        TableLayoutPanel2.Name = "TableLayoutPanel2"
        TableLayoutPanel2.RowCount = 1
        TableLayoutPanel2.RowStyles.Add(New RowStyle(SizeType.Percent, 100F))
        TableLayoutPanel2.Size = New Size(633, 56)
        TableLayoutPanel2.TabIndex = 1
        ' 
        ' resetButton
        ' 
        resetButton.Anchor = AnchorStyles.None
        resetButton.FlatStyle = FlatStyle.Flat
        resetButton.Location = New Point(516, 16)
        resetButton.Name = "resetButton"
        resetButton.Size = New Size(75, 23)
        resetButton.TabIndex = 9
        resetButton.Text = "Reset"
        resetButton.UseVisualStyleBackColor = True
        ' 
        ' Panel5
        ' 
        Panel5.Controls.Add(DateTimePickerTo)
        Panel5.Controls.Add(Label2)
        Panel5.Dock = DockStyle.Fill
        Panel5.Location = New Point(161, 3)
        Panel5.Name = "Panel5"
        Panel5.Size = New Size(152, 50)
        Panel5.TabIndex = 7
        ' 
        ' DateTimePickerTo
        ' 
        DateTimePickerTo.Format = DateTimePickerFormat.Custom
        DateTimePickerTo.Location = New Point(34, 12)
        DateTimePickerTo.Name = "DateTimePickerTo"
        DateTimePickerTo.Size = New Size(98, 23)
        DateTimePickerTo.TabIndex = 9
        ' 
        ' Label2
        ' 
        Label2.Anchor = AnchorStyles.None
        Label2.AutoSize = True
        Label2.Font = New Font("Segoe UI", 10F)
        Label2.Location = New Point(5, 16)
        Label2.Name = "Label2"
        Label2.Size = New Size(26, 19)
        Label2.TabIndex = 6
        Label2.Text = "To:"
        Label2.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' Panel4
        ' 
        Panel4.Controls.Add(DateTimePickerFrom)
        Panel4.Controls.Add(Label3)
        Panel4.Dock = DockStyle.Left
        Panel4.Location = New Point(3, 3)
        Panel4.Name = "Panel4"
        Panel4.Size = New Size(152, 50)
        Panel4.TabIndex = 3
        ' 
        ' DateTimePickerFrom
        ' 
        DateTimePickerFrom.Format = DateTimePickerFormat.Custom
        DateTimePickerFrom.Location = New Point(51, 13)
        DateTimePickerFrom.Name = "DateTimePickerFrom"
        DateTimePickerFrom.Size = New Size(98, 23)
        DateTimePickerFrom.TabIndex = 8
        ' 
        ' Label3
        ' 
        Label3.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left
        Label3.AutoSize = True
        Label3.Font = New Font("Segoe UI", 10F)
        Label3.Location = New Point(6, 16)
        Label3.Name = "Label3"
        Label3.Size = New Size(44, 19)
        Label3.TabIndex = 2
        Label3.Text = "From:"
        Label3.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' filterButton
        ' 
        filterButton.Anchor = AnchorStyles.None
        filterButton.FlatStyle = FlatStyle.Flat
        filterButton.Location = New Point(357, 16)
        filterButton.Name = "filterButton"
        filterButton.Size = New Size(75, 23)
        filterButton.TabIndex = 8
        filterButton.Text = "Filter"
        filterButton.UseVisualStyleBackColor = True
        ' 
        ' Label1
        ' 
        Label1.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        Label1.AutoSize = True
        Label1.Font = New Font("Segoe UI Semibold", 20F, FontStyle.Bold)
        Label1.Location = New Point(3, 15)
        Label1.Name = "Label1"
        Label1.Size = New Size(1804, 37)
        Label1.TabIndex = 0
        Label1.Text = "DELIVERY LOGS"
        Label1.TextAlign = ContentAlignment.MiddleCenter
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
        tableDataGridView.Size = New Size(1710, 652)
        tableDataGridView.TabIndex = 0
        ' 
        ' deliveryLogsForm
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1810, 792)
        Controls.Add(mainPanel)
        Name = "deliveryLogsForm"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Delivery"
        WindowState = FormWindowState.Maximized
        mainPanel.ResumeLayout(False)
        Panel1.ResumeLayout(False)
        TableLayoutPanel1.ResumeLayout(False)
        TableLayoutPanel1.PerformLayout()
        TableLayoutPanel2.ResumeLayout(False)
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
    Friend WithEvents Label1 As Label
    Friend WithEvents TableLayoutPanel2 As TableLayoutPanel
    Friend WithEvents Panel5 As Panel
    Friend WithEvents Label2 As Label
    Friend WithEvents Panel4 As Panel
    Friend WithEvents Label3 As Label
    Friend WithEvents filterButton As Button
    Friend WithEvents DateTimePickerFrom As DateTimePicker
    Friend WithEvents DateTimePickerTo As DateTimePicker
    Friend WithEvents resetButton As Button
End Class
