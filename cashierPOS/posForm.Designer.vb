<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class posForm
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
        TableLayoutPanel1 = New TableLayoutPanel()
        DataGridView1 = New DataGridView()
        Panel2 = New Panel()
        bottomPanel = New Panel()
        TableLayoutPanel3 = New TableLayoutPanel()
        Panel4 = New Panel()
        dateYearLabel = New Label()
        Panel3 = New Panel()
        timeLabel = New Label()
        Panel5 = New Panel()
        vatableLabel = New Label()
        VATLabel = New Label()
        totalDiscountLabel = New Label()
        totalSalesLabel = New Label()
        Label4 = New Label()
        Label3 = New Label()
        Label2 = New Label()
        Label1 = New Label()
        featuresPanel = New Panel()
        TableLayoutPanel2 = New TableLayoutPanel()
        Button6 = New Button()
        Button1 = New Button()
        Button5 = New Button()
        Button4 = New Button()
        Button3 = New Button()
        Button2 = New Button()
        Panel1.SuspendLayout()
        TableLayoutPanel1.SuspendLayout()
        CType(DataGridView1, ComponentModel.ISupportInitialize).BeginInit()
        Panel2.SuspendLayout()
        bottomPanel.SuspendLayout()
        TableLayoutPanel3.SuspendLayout()
        Panel4.SuspendLayout()
        Panel3.SuspendLayout()
        Panel5.SuspendLayout()
        featuresPanel.SuspendLayout()
        TableLayoutPanel2.SuspendLayout()
        SuspendLayout()
        ' 
        ' Panel1
        ' 
        Panel1.Controls.Add(TableLayoutPanel1)
        Panel1.Dock = DockStyle.Fill
        Panel1.Location = New Point(0, 0)
        Panel1.Name = "Panel1"
        Panel1.Size = New Size(1717, 1020)
        Panel1.TabIndex = 0
        ' 
        ' TableLayoutPanel1
        ' 
        TableLayoutPanel1.ColumnCount = 2
        TableLayoutPanel1.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 70F))
        TableLayoutPanel1.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 30F))
        TableLayoutPanel1.Controls.Add(DataGridView1, 0, 0)
        TableLayoutPanel1.Controls.Add(Panel2, 0, 1)
        TableLayoutPanel1.Controls.Add(featuresPanel, 1, 0)
        TableLayoutPanel1.Dock = DockStyle.Fill
        TableLayoutPanel1.Location = New Point(0, 0)
        TableLayoutPanel1.Name = "TableLayoutPanel1"
        TableLayoutPanel1.Padding = New Padding(30)
        TableLayoutPanel1.RowCount = 2
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Percent, 70F))
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Percent, 30F))
        TableLayoutPanel1.Size = New Size(1717, 1020)
        TableLayoutPanel1.TabIndex = 1
        ' 
        ' DataGridView1
        ' 
        DataGridView1.AllowUserToAddRows = False
        DataGridView1.AllowUserToDeleteRows = False
        DataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridView1.Dock = DockStyle.Fill
        DataGridView1.Location = New Point(33, 33)
        DataGridView1.Name = "DataGridView1"
        DataGridView1.ReadOnly = True
        DataGridView1.Size = New Size(1153, 666)
        DataGridView1.TabIndex = 0
        ' 
        ' Panel2
        ' 
        TableLayoutPanel1.SetColumnSpan(Panel2, 2)
        Panel2.Controls.Add(bottomPanel)
        Panel2.Dock = DockStyle.Fill
        Panel2.Location = New Point(33, 705)
        Panel2.Name = "Panel2"
        Panel2.Padding = New Padding(0, 80, 0, 0)
        Panel2.Size = New Size(1651, 282)
        Panel2.TabIndex = 1
        ' 
        ' bottomPanel
        ' 
        bottomPanel.Controls.Add(TableLayoutPanel3)
        bottomPanel.Dock = DockStyle.Fill
        bottomPanel.Location = New Point(0, 80)
        bottomPanel.Name = "bottomPanel"
        bottomPanel.Size = New Size(1651, 202)
        bottomPanel.TabIndex = 0
        ' 
        ' TableLayoutPanel3
        ' 
        TableLayoutPanel3.ColumnCount = 2
        TableLayoutPanel3.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50F))
        TableLayoutPanel3.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50F))
        TableLayoutPanel3.Controls.Add(Panel4, 0, 1)
        TableLayoutPanel3.Controls.Add(Panel3, 0, 0)
        TableLayoutPanel3.Controls.Add(Panel5, 1, 0)
        TableLayoutPanel3.Dock = DockStyle.Fill
        TableLayoutPanel3.Location = New Point(0, 0)
        TableLayoutPanel3.Name = "TableLayoutPanel3"
        TableLayoutPanel3.RowCount = 2
        TableLayoutPanel3.RowStyles.Add(New RowStyle(SizeType.Percent, 50F))
        TableLayoutPanel3.RowStyles.Add(New RowStyle(SizeType.Percent, 50F))
        TableLayoutPanel3.Size = New Size(1651, 202)
        TableLayoutPanel3.TabIndex = 3
        ' 
        ' Panel4
        ' 
        Panel4.Controls.Add(dateYearLabel)
        Panel4.Dock = DockStyle.Fill
        Panel4.Location = New Point(3, 104)
        Panel4.Name = "Panel4"
        Panel4.Padding = New Padding(50, 0, 0, 0)
        Panel4.Size = New Size(819, 95)
        Panel4.TabIndex = 4
        ' 
        ' dateYearLabel
        ' 
        dateYearLabel.Dock = DockStyle.Fill
        dateYearLabel.Font = New Font("Segoe UI", 22F)
        dateYearLabel.Location = New Point(50, 0)
        dateYearLabel.Name = "dateYearLabel"
        dateYearLabel.Size = New Size(769, 95)
        dateYearLabel.TabIndex = 2
        dateYearLabel.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' Panel3
        ' 
        Panel3.Controls.Add(timeLabel)
        Panel3.Dock = DockStyle.Fill
        Panel3.Location = New Point(3, 3)
        Panel3.Name = "Panel3"
        Panel3.Padding = New Padding(50, 0, 0, 0)
        Panel3.Size = New Size(819, 95)
        Panel3.TabIndex = 3
        ' 
        ' timeLabel
        ' 
        timeLabel.Dock = DockStyle.Fill
        timeLabel.Font = New Font("Segoe UI", 35F)
        timeLabel.Location = New Point(50, 0)
        timeLabel.Name = "timeLabel"
        timeLabel.Size = New Size(769, 95)
        timeLabel.TabIndex = 1
        timeLabel.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' Panel5
        ' 
        Panel5.Controls.Add(vatableLabel)
        Panel5.Controls.Add(VATLabel)
        Panel5.Controls.Add(totalDiscountLabel)
        Panel5.Controls.Add(totalSalesLabel)
        Panel5.Controls.Add(Label4)
        Panel5.Controls.Add(Label3)
        Panel5.Controls.Add(Label2)
        Panel5.Controls.Add(Label1)
        Panel5.Dock = DockStyle.Fill
        Panel5.Location = New Point(828, 3)
        Panel5.Name = "Panel5"
        Panel5.Padding = New Padding(20)
        TableLayoutPanel3.SetRowSpan(Panel5, 2)
        Panel5.Size = New Size(820, 196)
        Panel5.TabIndex = 5
        ' 
        ' vatableLabel
        ' 
        vatableLabel.Anchor = AnchorStyles.None
        vatableLabel.Font = New Font("Segoe UI", 14F)
        vatableLabel.Location = New Point(571, 110)
        vatableLabel.Name = "vatableLabel"
        vatableLabel.Size = New Size(237, 23)
        vatableLabel.TabIndex = 7
        ' 
        ' VATLabel
        ' 
        VATLabel.Anchor = AnchorStyles.None
        VATLabel.Font = New Font("Segoe UI", 14F)
        VATLabel.Location = New Point(530, 70)
        VATLabel.Name = "VATLabel"
        VATLabel.Size = New Size(278, 23)
        VATLabel.TabIndex = 6
        ' 
        ' totalDiscountLabel
        ' 
        totalDiscountLabel.Anchor = AnchorStyles.None
        totalDiscountLabel.Font = New Font("Segoe UI", 14F)
        totalDiscountLabel.Location = New Point(178, 110)
        totalDiscountLabel.Name = "totalDiscountLabel"
        totalDiscountLabel.Size = New Size(245, 23)
        totalDiscountLabel.TabIndex = 5
        ' 
        ' totalSalesLabel
        ' 
        totalSalesLabel.Anchor = AnchorStyles.None
        totalSalesLabel.Font = New Font("Segoe UI", 14F)
        totalSalesLabel.Location = New Point(192, 70)
        totalSalesLabel.Name = "totalSalesLabel"
        totalSalesLabel.Size = New Size(245, 23)
        totalSalesLabel.TabIndex = 4
        ' 
        ' Label4
        ' 
        Label4.Anchor = AnchorStyles.None
        Label4.AutoSize = True
        Label4.Font = New Font("Segoe UI", 14F)
        Label4.Location = New Point(476, 108)
        Label4.Name = "Label4"
        Label4.Size = New Size(89, 25)
        Label4.TabIndex = 3
        Label4.Text = "VATABLE:"
        ' 
        ' Label3
        ' 
        Label3.Anchor = AnchorStyles.None
        Label3.AutoSize = True
        Label3.Font = New Font("Segoe UI", 14F)
        Label3.Location = New Point(476, 70)
        Label3.Name = "Label3"
        Label3.Size = New Size(48, 25)
        Label3.TabIndex = 2
        Label3.Text = "VAT:"
        ' 
        ' Label2
        ' 
        Label2.Anchor = AnchorStyles.None
        Label2.AutoSize = True
        Label2.Font = New Font("Segoe UI", 14F)
        Label2.Location = New Point(66, 108)
        Label2.Name = "Label2"
        Label2.Size = New Size(106, 25)
        Label2.TabIndex = 1
        Label2.Text = "DISCOUNT:"
        ' 
        ' Label1
        ' 
        Label1.Anchor = AnchorStyles.None
        Label1.AutoSize = True
        Label1.Font = New Font("Segoe UI", 14F)
        Label1.Location = New Point(66, 70)
        Label1.Name = "Label1"
        Label1.Size = New Size(124, 25)
        Label1.TabIndex = 0
        Label1.Text = "SALES TOTAL:"
        ' 
        ' featuresPanel
        ' 
        featuresPanel.Controls.Add(TableLayoutPanel2)
        featuresPanel.Dock = DockStyle.Fill
        featuresPanel.Location = New Point(1192, 33)
        featuresPanel.Name = "featuresPanel"
        featuresPanel.Padding = New Padding(50)
        featuresPanel.Size = New Size(492, 666)
        featuresPanel.TabIndex = 2
        ' 
        ' TableLayoutPanel2
        ' 
        TableLayoutPanel2.ColumnCount = 1
        TableLayoutPanel2.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100F))
        TableLayoutPanel2.Controls.Add(Button6, 0, 1)
        TableLayoutPanel2.Controls.Add(Button1, 0, 0)
        TableLayoutPanel2.Controls.Add(Button5, 0, 5)
        TableLayoutPanel2.Controls.Add(Button4, 0, 4)
        TableLayoutPanel2.Controls.Add(Button3, 0, 3)
        TableLayoutPanel2.Controls.Add(Button2, 0, 2)
        TableLayoutPanel2.Dock = DockStyle.Fill
        TableLayoutPanel2.Location = New Point(50, 50)
        TableLayoutPanel2.Name = "TableLayoutPanel2"
        TableLayoutPanel2.RowCount = 6
        TableLayoutPanel2.RowStyles.Add(New RowStyle(SizeType.Percent, 16.666666F))
        TableLayoutPanel2.RowStyles.Add(New RowStyle(SizeType.Percent, 16.666666F))
        TableLayoutPanel2.RowStyles.Add(New RowStyle(SizeType.Percent, 16.666666F))
        TableLayoutPanel2.RowStyles.Add(New RowStyle(SizeType.Percent, 16.666666F))
        TableLayoutPanel2.RowStyles.Add(New RowStyle(SizeType.Percent, 16.666666F))
        TableLayoutPanel2.RowStyles.Add(New RowStyle(SizeType.Percent, 16.666666F))
        TableLayoutPanel2.Size = New Size(392, 566)
        TableLayoutPanel2.TabIndex = 0
        ' 
        ' Button6
        ' 
        Button6.Anchor = AnchorStyles.None
        Button6.Cursor = Cursors.Hand
        Button6.FlatAppearance.BorderSize = 0
        Button6.FlatStyle = FlatStyle.Popup
        Button6.Font = New Font("Segoe UI", 14F)
        Button6.Location = New Point(106, 124)
        Button6.Name = "Button6"
        Button6.Size = New Size(180, 34)
        Button6.TabIndex = 5
        Button6.Text = "[F2] Scan Product"
        Button6.UseVisualStyleBackColor = True
        ' 
        ' Button1
        ' 
        Button1.Anchor = AnchorStyles.None
        Button1.Cursor = Cursors.Hand
        Button1.FlatAppearance.BorderSize = 0
        Button1.FlatStyle = FlatStyle.Popup
        Button1.Font = New Font("Segoe UI", 14F)
        Button1.Location = New Point(106, 30)
        Button1.Name = "Button1"
        Button1.Size = New Size(180, 34)
        Button1.TabIndex = 0
        Button1.Text = "[F1] Seach Product"
        Button1.UseVisualStyleBackColor = True
        ' 
        ' Button5
        ' 
        Button5.Anchor = AnchorStyles.None
        Button5.Cursor = Cursors.Hand
        Button5.FlatAppearance.BorderSize = 0
        Button5.FlatStyle = FlatStyle.Popup
        Button5.Font = New Font("Segoe UI", 14F)
        Button5.Location = New Point(106, 501)
        Button5.Name = "Button5"
        Button5.Size = New Size(180, 34)
        Button5.TabIndex = 4
        Button5.Text = "Logout"
        Button5.UseVisualStyleBackColor = True
        ' 
        ' Button4
        ' 
        Button4.Anchor = AnchorStyles.None
        Button4.Cursor = Cursors.Hand
        Button4.FlatAppearance.BorderSize = 0
        Button4.FlatStyle = FlatStyle.Popup
        Button4.Font = New Font("Segoe UI", 14F)
        Button4.Location = New Point(106, 406)
        Button4.Name = "Button4"
        Button4.Size = New Size(180, 34)
        Button4.TabIndex = 3
        Button4.Text = "[F5] Transactions"
        Button4.UseVisualStyleBackColor = True
        ' 
        ' Button3
        ' 
        Button3.Anchor = AnchorStyles.None
        Button3.Cursor = Cursors.Hand
        Button3.FlatAppearance.BorderSize = 0
        Button3.FlatStyle = FlatStyle.Popup
        Button3.Font = New Font("Segoe UI", 14F)
        Button3.Location = New Point(106, 312)
        Button3.Name = "Button3"
        Button3.Size = New Size(180, 34)
        Button3.TabIndex = 2
        Button3.Text = "[F4] Clear Cart"
        Button3.UseVisualStyleBackColor = True
        ' 
        ' Button2
        ' 
        Button2.Anchor = AnchorStyles.None
        Button2.Cursor = Cursors.Hand
        Button2.FlatAppearance.BorderSize = 0
        Button2.FlatStyle = FlatStyle.Popup
        Button2.Font = New Font("Segoe UI", 14F)
        Button2.Location = New Point(106, 218)
        Button2.Name = "Button2"
        Button2.Size = New Size(180, 34)
        Button2.TabIndex = 1
        Button2.Text = "[F3] Checkout"
        Button2.UseVisualStyleBackColor = True
        ' 
        ' posForm
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1717, 1020)
        Controls.Add(Panel1)
        FormBorderStyle = FormBorderStyle.FixedToolWindow
        Name = "posForm"
        StartPosition = FormStartPosition.CenterScreen
        Text = "a"
        WindowState = FormWindowState.Maximized
        Panel1.ResumeLayout(False)
        TableLayoutPanel1.ResumeLayout(False)
        CType(DataGridView1, ComponentModel.ISupportInitialize).EndInit()
        Panel2.ResumeLayout(False)
        bottomPanel.ResumeLayout(False)
        TableLayoutPanel3.ResumeLayout(False)
        Panel4.ResumeLayout(False)
        Panel3.ResumeLayout(False)
        Panel5.ResumeLayout(False)
        Panel5.PerformLayout()
        featuresPanel.ResumeLayout(False)
        TableLayoutPanel2.ResumeLayout(False)
        ResumeLayout(False)
    End Sub

    Friend WithEvents Panel1 As Panel
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents Panel2 As Panel
    Friend WithEvents bottomPanel As Panel
    Friend WithEvents featuresPanel As Panel
    Friend WithEvents TableLayoutPanel2 As TableLayoutPanel
    Friend WithEvents Button1 As Button
    Friend WithEvents Button3 As Button
    Friend WithEvents Button2 As Button
    Friend WithEvents Button4 As Button
    Friend WithEvents Button5 As Button
    Friend WithEvents dateYearLabel As Label
    Friend WithEvents timeLabel As Label
    Friend WithEvents TableLayoutPanel3 As TableLayoutPanel
    Friend WithEvents Panel4 As Panel
    Friend WithEvents Panel3 As Panel
    Friend WithEvents Panel5 As Panel
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents VATLabel As Label
    Friend WithEvents totalDiscountLabel As Label
    Friend WithEvents totalSalesLabel As Label
    Friend WithEvents vatableLabel As Label
    Friend WithEvents Button6 As Button
End Class
