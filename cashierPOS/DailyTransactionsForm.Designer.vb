<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class DailyTransactionsForm
    Inherits System.Windows.Forms.Form

 'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
Me.lblTitle = New System.Windows.Forms.Label()
     Me.transactionsDataGridView = New System.Windows.Forms.DataGridView()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.lblWholesaleCount = New System.Windows.Forms.Label()
        Me.lblRetailCount = New System.Windows.Forms.Label()
        Me.lblTotalRevenue = New System.Windows.Forms.Label()
  Me.lblTotalTransactions = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnRefresh = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
Me.cboSalesType = New System.Windows.Forms.ComboBox()
    CType(Me.transactionsDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblTitle
'
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblTitle.Font = New System.Drawing.Font("Segoe UI", 16.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
    Me.lblTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblTitle.Name = "lblTitle"
   Me.lblTitle.Size = New System.Drawing.Size(984, 50)
        Me.lblTitle.TabIndex = 0
  Me.lblTitle.Text = "Daily Transactions"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'transactionsDataGridView
        '
        Me.transactionsDataGridView.AllowUserToAddRows = False
 Me.transactionsDataGridView.AllowUserToDeleteRows = False
        Me.transactionsDataGridView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
        Or System.Windows.Forms.AnchorStyles.Left) _
     Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
     Me.transactionsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.transactionsDataGridView.Location = New System.Drawing.Point(10, 150)
        Me.transactionsDataGridView.Name = "transactionsDataGridView"
   Me.transactionsDataGridView.ReadOnly = True
     Me.transactionsDataGridView.RowTemplate.Height = 25
        Me.transactionsDataGridView.Size = New System.Drawing.Size(964, 360)
  Me.transactionsDataGridView.TabIndex = 1
        '
        'Panel1
        '
     Me.Panel1.Controls.Add(Me.lblWholesaleCount)
        Me.Panel1.Controls.Add(Me.lblRetailCount)
     Me.Panel1.Controls.Add(Me.lblTotalRevenue)
   Me.Panel1.Controls.Add(Me.lblTotalTransactions)
      Me.Panel1.Location = New System.Drawing.Point(10, 60)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(600, 80)
        Me.Panel1.TabIndex = 2
   '
        'lblWholesaleCount
        '
        Me.lblWholesaleCount.AutoSize = True
 Me.lblWholesaleCount.Location = New System.Drawing.Point(300, 45)
     Me.lblWholesaleCount.Name = "lblWholesaleCount"
   Me.lblWholesaleCount.Size = New System.Drawing.Size(149, 15)
        Me.lblWholesaleCount.TabIndex = 3
        Me.lblWholesaleCount.Text = "Wholesale: 0 transactions"
        '
    'lblRetailCount
  '
        Me.lblRetailCount.AutoSize = True
   Me.lblRetailCount.Location = New System.Drawing.Point(300, 15)
        Me.lblRetailCount.Name = "lblRetailCount"
 Me.lblRetailCount.Size = New System.Drawing.Size(122, 15)
        Me.lblRetailCount.TabIndex = 2
        Me.lblRetailCount.Text = "Retail: 0 transactions"
        '
        'lblTotalRevenue
        '
        Me.lblTotalRevenue.AutoSize = True
    Me.lblTotalRevenue.Font = New System.Drawing.Font("Segoe UI", 11.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
     Me.lblTotalRevenue.Location = New System.Drawing.Point(15, 45)
        Me.lblTotalRevenue.Name = "lblTotalRevenue"
        Me.lblTotalRevenue.Size = New System.Drawing.Size(148, 20)
    Me.lblTotalRevenue.TabIndex = 1
  Me.lblTotalRevenue.Text = "Total Revenue: ?0.00"
        '
  'lblTotalTransactions
        '
     Me.lblTotalTransactions.AutoSize = True
        Me.lblTotalTransactions.Font = New System.Drawing.Font("Segoe UI", 11.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.lblTotalTransactions.Location = New System.Drawing.Point(15, 15)
   Me.lblTotalTransactions.Name = "lblTotalTransactions"
    Me.lblTotalTransactions.Size = New System.Drawing.Size(162, 20)
        Me.lblTotalTransactions.TabIndex = 0
   Me.lblTotalTransactions.Text = "Total Transactions: 0"
        '
'Panel2
      '
        Me.Panel2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.Panel2.Controls.Add(Me.btnClose)
        Me.Panel2.Controls.Add(Me.btnRefresh)
     Me.Panel2.Location = New System.Drawing.Point(714, 520)
        Me.Panel2.Name = "Panel2"
   Me.Panel2.Size = New System.Drawing.Size(260, 50)
        Me.Panel2.TabIndex = 3
   '
        'btnClose
   '
        Me.btnClose.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Location = New System.Drawing.Point(135, 10)
        Me.btnClose.Name = "btnClose"
 Me.btnClose.Size = New System.Drawing.Size(120, 35)
    Me.btnClose.TabIndex = 1
        Me.btnClose.Text = "Close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'btnRefresh
        '
        Me.btnRefresh.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRefresh.Location = New System.Drawing.Point(5, 10)
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(120, 35)
   Me.btnRefresh.TabIndex = 0
        Me.btnRefresh.Text = "Refresh"
        Me.btnRefresh.UseVisualStyleBackColor = True
  '
        'Label1
        '
        Me.Label1.AutoSize = True
      Me.Label1.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.Label1.Location = New System.Drawing.Point(630, 75)
        Me.Label1.Name = "Label1"
     Me.Label1.Size = New System.Drawing.Size(83, 19)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Filter by:"
 '
  'cboSalesType
      '
        Me.cboSalesType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSalesType.FormattingEnabled = True
        Me.cboSalesType.Location = New System.Drawing.Point(630, 100)
        Me.cboSalesType.Name = "cboSalesType"
        Me.cboSalesType.Size = New System.Drawing.Size(200, 23)
        Me.cboSalesType.TabIndex = 5
        '
        'DailyTransactionsForm
     '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(984, 581)
        Me.Controls.Add(Me.cboSalesType)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
   Me.Controls.Add(Me.transactionsDataGridView)
        Me.Controls.Add(Me.lblTitle)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
      Me.Name = "DailyTransactionsForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
    Me.Text = "Daily Transactions"
        CType(Me.transactionsDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
  Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
   Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblTitle As Label
    Friend WithEvents transactionsDataGridView As DataGridView
 Friend WithEvents Panel1 As Panel
    Friend WithEvents lblTotalTransactions As Label
    Friend WithEvents lblTotalRevenue As Label
 Friend WithEvents lblRetailCount As Label
    Friend WithEvents lblWholesaleCount As Label
    Friend WithEvents Panel2 As Panel
    Friend WithEvents btnRefresh As Button
    Friend WithEvents btnClose As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents cboSalesType As ComboBox
End Class
