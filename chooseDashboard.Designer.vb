<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class chooseDashboard
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
        PictureBox1 = New PictureBox()
        PictureBox2 = New PictureBox()
        choosePanel = New Panel()
        Label1 = New Label()
        chooseTableLayoutPanel1 = New TableLayoutPanel()
        retailButton = New Button()
        wholeSaleButton = New Button()
        CType(PictureBox1, ComponentModel.ISupportInitialize).BeginInit()
        CType(PictureBox2, ComponentModel.ISupportInitialize).BeginInit()
        choosePanel.SuspendLayout()
        chooseTableLayoutPanel1.SuspendLayout()
        SuspendLayout()
        ' 
        ' PictureBox1
        ' 
        PictureBox1.BackColor = Color.Transparent
        PictureBox1.BackgroundImage = My.Resources.Resources.opacityPawPrints
        PictureBox1.BackgroundImageLayout = ImageLayout.Stretch
        PictureBox1.Location = New Point(-92, -82)
        PictureBox1.Name = "PictureBox1"
        PictureBox1.Size = New Size(638, 651)
        PictureBox1.TabIndex = 0
        PictureBox1.TabStop = False
        ' 
        ' PictureBox2
        ' 
        PictureBox2.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        PictureBox2.BackColor = Color.Transparent
        PictureBox2.BackgroundImage = My.Resources.Resources.opacityPawPrints
        PictureBox2.BackgroundImageLayout = ImageLayout.Stretch
        PictureBox2.Location = New Point(913, 265)
        PictureBox2.Name = "PictureBox2"
        PictureBox2.Size = New Size(638, 651)
        PictureBox2.TabIndex = 1
        PictureBox2.TabStop = False
        ' 
        ' choosePanel
        ' 
        choosePanel.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        choosePanel.Controls.Add(Label1)
        choosePanel.Controls.Add(chooseTableLayoutPanel1)
        choosePanel.Location = New Point(318, 88)
        choosePanel.Name = "choosePanel"
        choosePanel.Padding = New Padding(20)
        choosePanel.Size = New Size(1011, 626)
        choosePanel.TabIndex = 2
        ' 
        ' Label1
        ' 
        Label1.Anchor = AnchorStyles.None
        Label1.AutoSize = True
        Label1.Font = New Font("Segoe UI", 26.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Label1.Location = New Point(315, 35)
        Label1.Name = "Label1"
        Label1.Size = New Size(457, 47)
        Label1.TabIndex = 3
        Label1.Text = "Where do you want to go?"
        Label1.TextAlign = ContentAlignment.TopCenter
        ' 
        ' chooseTableLayoutPanel1
        ' 
        chooseTableLayoutPanel1.Anchor = AnchorStyles.None
        chooseTableLayoutPanel1.ColumnCount = 3
        chooseTableLayoutPanel1.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 40.0F))
        chooseTableLayoutPanel1.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 20.0F))
        chooseTableLayoutPanel1.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 40.0F))
        chooseTableLayoutPanel1.Controls.Add(retailButton, 2, 0)
        chooseTableLayoutPanel1.Controls.Add(wholeSaleButton, 0, 0)
        chooseTableLayoutPanel1.Location = New Point(39, 103)
        chooseTableLayoutPanel1.MaximumSize = New Size(900, 500)
        chooseTableLayoutPanel1.MinimumSize = New Size(900, 500)
        chooseTableLayoutPanel1.Name = "chooseTableLayoutPanel1"
        chooseTableLayoutPanel1.RowCount = 1
        chooseTableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        chooseTableLayoutPanel1.Size = New Size(900, 500)
        chooseTableLayoutPanel1.TabIndex = 2
        ' 
        ' retailButton
        ' 
        retailButton.Anchor = AnchorStyles.Right
        retailButton.FlatAppearance.BorderSize = 0
        retailButton.FlatStyle = FlatStyle.Popup
        retailButton.Font = New Font("Segoe UI", 20.25F, FontStyle.Bold)
        retailButton.Image = My.Resources.Resources.retail1
        retailButton.ImageAlign = ContentAlignment.TopCenter
        retailButton.Location = New Point(545, 88)
        retailButton.Margin = New Padding(3, 10, 10, 10)
        retailButton.MaximumSize = New Size(800, 400)
        retailButton.Name = "retailButton"
        retailButton.Padding = New Padding(0, 0, 0, 10)
        retailButton.Size = New Size(345, 324)
        retailButton.TabIndex = 1
        retailButton.Text = "Retail"
        retailButton.TextAlign = ContentAlignment.BottomCenter
        retailButton.UseVisualStyleBackColor = True
        ' 
        ' wholeSaleButton
        ' 
        wholeSaleButton.Anchor = AnchorStyles.Left
        wholeSaleButton.FlatAppearance.BorderSize = 0
        wholeSaleButton.FlatStyle = FlatStyle.Popup
        wholeSaleButton.Font = New Font("Segoe UI", 20.25F, FontStyle.Bold)
        wholeSaleButton.Image = My.Resources.Resources.wholsale
        wholeSaleButton.ImageAlign = ContentAlignment.TopCenter
        wholeSaleButton.Location = New Point(10, 88)
        wholeSaleButton.Margin = New Padding(10, 10, 3, 10)
        wholeSaleButton.MaximumSize = New Size(800, 400)
        wholeSaleButton.Name = "wholeSaleButton"
        wholeSaleButton.Padding = New Padding(0, 0, 0, 10)
        wholeSaleButton.Size = New Size(345, 324)
        wholeSaleButton.TabIndex = 0
        wholeSaleButton.Text = "Wholesale"
        wholeSaleButton.TextAlign = ContentAlignment.BottomCenter
        wholeSaleButton.UseVisualStyleBackColor = True
        ' 
        ' chooseDashboard
        ' 
        AutoScaleDimensions = New SizeF(7.0F, 15.0F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1469, 810)
        Controls.Add(choosePanel)
        Controls.Add(PictureBox2)
        Controls.Add(PictureBox1)
        FormBorderStyle = FormBorderStyle.FixedToolWindow
        Name = "chooseDashboard"
        StartPosition = FormStartPosition.Manual
        Text = "Admin"
        WindowState = FormWindowState.Maximized
        CType(PictureBox1, ComponentModel.ISupportInitialize).EndInit()
        CType(PictureBox2, ComponentModel.ISupportInitialize).EndInit()
        choosePanel.ResumeLayout(False)
        choosePanel.PerformLayout()
        chooseTableLayoutPanel1.ResumeLayout(False)
        ResumeLayout(False)
    End Sub

    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents PictureBox2 As PictureBox
    Friend WithEvents choosePanel As Panel
    Friend WithEvents retailButton As Button
    Friend WithEvents wholeSaleButton As Button
    Friend WithEvents chooseTableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents Label1 As Label
End Class
