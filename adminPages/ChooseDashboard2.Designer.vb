<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class chooseDashboard2
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
        chooseTableLayoutPanel1 = New TableLayoutPanel()
        retailButton = New Button()
        wholeSaleButton = New Button()
        TableLayoutPanel1 = New TableLayoutPanel()
        Panel2 = New Panel()
        Label1 = New Label()
        TableLayoutPanel2 = New TableLayoutPanel()
        logoPictureBox = New PictureBox()
        chooseTableLayoutPanel1.SuspendLayout()
        TableLayoutPanel1.SuspendLayout()
        Panel2.SuspendLayout()
        TableLayoutPanel2.SuspendLayout()
        CType(logoPictureBox, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' chooseTableLayoutPanel1
        ' 
        chooseTableLayoutPanel1.ColumnCount = 3
        chooseTableLayoutPanel1.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 45F))
        chooseTableLayoutPanel1.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 10F))
        chooseTableLayoutPanel1.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 45F))
        chooseTableLayoutPanel1.Controls.Add(retailButton, 2, 0)
        chooseTableLayoutPanel1.Controls.Add(wholeSaleButton, 0, 0)
        chooseTableLayoutPanel1.Dock = DockStyle.Fill
        chooseTableLayoutPanel1.Location = New Point(3, 510)
        chooseTableLayoutPanel1.Name = "chooseTableLayoutPanel1"
        chooseTableLayoutPanel1.RowCount = 1
        chooseTableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Percent, 100F))
        chooseTableLayoutPanel1.Size = New Size(895, 501)
        chooseTableLayoutPanel1.TabIndex = 2
        ' 
        ' retailButton
        ' 
        retailButton.Anchor = AnchorStyles.None
        retailButton.BackgroundImageLayout = ImageLayout.Stretch
        retailButton.FlatAppearance.BorderSize = 0
        retailButton.FlatStyle = FlatStyle.Popup
        retailButton.Font = New Font("Segoe UI", 20.25F, FontStyle.Bold)
        retailButton.Image = My.Resources.Resources.store_8771926
        retailButton.ImageAlign = ContentAlignment.TopCenter
        retailButton.Location = New Point(525, 87)
        retailButton.Margin = New Padding(3, 10, 10, 10)
        retailButton.MaximumSize = New Size(800, 400)
        retailButton.Name = "retailButton"
        retailButton.Padding = New Padding(0, 0, 0, 10)
        retailButton.Size = New Size(329, 327)
        retailButton.TabIndex = 1
        retailButton.Text = "Retail"
        retailButton.TextAlign = ContentAlignment.BottomCenter
        retailButton.UseVisualStyleBackColor = True
        ' 
        ' wholeSaleButton
        ' 
        wholeSaleButton.Anchor = AnchorStyles.None
        wholeSaleButton.FlatAppearance.BorderSize = 0
        wholeSaleButton.FlatStyle = FlatStyle.Popup
        wholeSaleButton.Font = New Font("Segoe UI", 20.25F, FontStyle.Bold)
        wholeSaleButton.Image = My.Resources.Resources.wholesaler_160899371
        wholeSaleButton.ImageAlign = ContentAlignment.TopCenter
        wholeSaleButton.Location = New Point(41, 87)
        wholeSaleButton.Margin = New Padding(10, 10, 3, 10)
        wholeSaleButton.MaximumSize = New Size(800, 400)
        wholeSaleButton.Name = "wholeSaleButton"
        wholeSaleButton.Padding = New Padding(0, 0, 0, 10)
        wholeSaleButton.Size = New Size(327, 327)
        wholeSaleButton.TabIndex = 0
        wholeSaleButton.Text = "Wholesale"
        wholeSaleButton.TextAlign = ContentAlignment.BottomCenter
        wholeSaleButton.UseVisualStyleBackColor = True
        ' 
        ' TableLayoutPanel1
        ' 
        TableLayoutPanel1.ColumnCount = 1
        TableLayoutPanel1.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50F))
        TableLayoutPanel1.Controls.Add(Panel2, 0, 0)
        TableLayoutPanel1.Controls.Add(chooseTableLayoutPanel1, 0, 1)
        TableLayoutPanel1.Dock = DockStyle.Fill
        TableLayoutPanel1.Location = New Point(909, 3)
        TableLayoutPanel1.Name = "TableLayoutPanel1"
        TableLayoutPanel1.RowCount = 2
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Percent, 50F))
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Percent, 50F))
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Absolute, 20F))
        TableLayoutPanel1.Size = New Size(901, 1014)
        TableLayoutPanel1.TabIndex = 4
        ' 
        ' Panel2
        ' 
        Panel2.BackgroundImage = My.Resources.Resources.pawprintsoriano
        Panel2.BackgroundImageLayout = ImageLayout.Stretch
        Panel2.Controls.Add(Label1)
        Panel2.Dock = DockStyle.Fill
        Panel2.Location = New Point(3, 3)
        Panel2.Name = "Panel2"
        Panel2.Size = New Size(895, 501)
        Panel2.TabIndex = 12
        ' 
        ' Label1
        ' 
        Label1.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        Label1.Font = New Font("Segoe UI", 80F, FontStyle.Bold)
        Label1.Location = New Point(184, 157)
        Label1.Name = "Label1"
        Label1.Size = New Size(538, 207)
        Label1.TabIndex = 3
        Label1.Text = "B.R.O.S"
        Label1.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' TableLayoutPanel2
        ' 
        TableLayoutPanel2.ColumnCount = 2
        TableLayoutPanel2.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50F))
        TableLayoutPanel2.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50F))
        TableLayoutPanel2.Controls.Add(logoPictureBox, 0, 0)
        TableLayoutPanel2.Controls.Add(TableLayoutPanel1, 1, 0)
        TableLayoutPanel2.Dock = DockStyle.Fill
        TableLayoutPanel2.Location = New Point(0, 0)
        TableLayoutPanel2.Name = "TableLayoutPanel2"
        TableLayoutPanel2.RowCount = 1
        TableLayoutPanel2.RowStyles.Add(New RowStyle(SizeType.Percent, 50F))
        TableLayoutPanel2.Size = New Size(1813, 1020)
        TableLayoutPanel2.TabIndex = 4
        ' 
        ' logoPictureBox
        ' 
        logoPictureBox.BackgroundImage = My.Resources.Resources.Untitled_design
        logoPictureBox.BackgroundImageLayout = ImageLayout.Stretch
        logoPictureBox.Dock = DockStyle.Fill
        logoPictureBox.Location = New Point(3, 3)
        logoPictureBox.Name = "logoPictureBox"
        logoPictureBox.Size = New Size(900, 1014)
        logoPictureBox.SizeMode = PictureBoxSizeMode.StretchImage
        logoPictureBox.TabIndex = 11
        logoPictureBox.TabStop = False
        ' 
        ' chooseDashboard2
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        BackgroundImageLayout = ImageLayout.Stretch
        ClientSize = New Size(1813, 1020)
        Controls.Add(TableLayoutPanel2)
        FormBorderStyle = FormBorderStyle.FixedToolWindow
        Name = "chooseDashboard2"
        StartPosition = FormStartPosition.Manual
        Text = "Admin"
        WindowState = FormWindowState.Maximized
        chooseTableLayoutPanel1.ResumeLayout(False)
        TableLayoutPanel1.ResumeLayout(False)
        Panel2.ResumeLayout(False)
        TableLayoutPanel2.ResumeLayout(False)
        CType(logoPictureBox, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub
    Friend WithEvents retailButton As Button
    Friend WithEvents wholeSaleButton As Button
    Friend WithEvents chooseTableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents TableLayoutPanel2 As TableLayoutPanel
    Friend WithEvents logoPictureBox As PictureBox
    Friend WithEvents Panel2 As Panel
    Friend WithEvents Label1 As Label
End Class
