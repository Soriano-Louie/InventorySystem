<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class topControlCashier
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Label1 = New Label()
        nameLabel = New Label()
        roleLabel = New Label()
        totalSalesText = New Label()
        CType(PictureBox1, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' PictureBox1
        ' 
        PictureBox1.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left
        PictureBox1.BackColor = Color.Transparent
        PictureBox1.BackgroundImage = My.Resources.Resources.malinawLogo
        PictureBox1.BackgroundImageLayout = ImageLayout.Stretch
        PictureBox1.Location = New Point(46, 0)
        PictureBox1.Name = "PictureBox1"
        PictureBox1.Size = New Size(202, 150)
        PictureBox1.TabIndex = 0
        PictureBox1.TabStop = False
        ' 
        ' Label1
        ' 
        Label1.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left
        Label1.Font = New Font("Segoe UI", 18F, FontStyle.Bold)
        Label1.Location = New Point(279, 35)
        Label1.Name = "Label1"
        Label1.Size = New Size(212, 31)
        Label1.TabIndex = 1
        Label1.Text = " POINT OF SALES"
        ' 
        ' nameLabel
        ' 
        nameLabel.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left
        nameLabel.Font = New Font("Segoe UI Semibold", 14.25F, FontStyle.Bold)
        nameLabel.Location = New Point(292, 75)
        nameLabel.Name = "nameLabel"
        nameLabel.Size = New Size(183, 23)
        nameLabel.TabIndex = 2
        nameLabel.Text = "Louie"
        nameLabel.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' roleLabel
        ' 
        roleLabel.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left
        roleLabel.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        roleLabel.Location = New Point(353, 105)
        roleLabel.Name = "roleLabel"
        roleLabel.Size = New Size(62, 23)
        roleLabel.TabIndex = 4
        roleLabel.Text = "Cashier"
        ' 
        ' totalSalesText
        ' 
        totalSalesText.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Right
        totalSalesText.Font = New Font("Segoe UI", 50F, FontStyle.Bold)
        totalSalesText.ForeColor = Color.LimeGreen
        totalSalesText.Location = New Point(1009, 29)
        totalSalesText.Name = "totalSalesText"
        totalSalesText.Size = New Size(541, 97)
        totalSalesText.TabIndex = 5
        ' 
        ' topControlCashier
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        Controls.Add(totalSalesText)
        Controls.Add(roleLabel)
        Controls.Add(nameLabel)
        Controls.Add(Label1)
        Controls.Add(PictureBox1)
        Name = "topControlCashier"
        Size = New Size(1582, 150)
        CType(PictureBox1, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub

    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents Label1 As Label
    Friend WithEvents nameLabel As Label
    Friend WithEvents roleLabel As Label
    Friend WithEvents totalSalesText As Label

End Class
