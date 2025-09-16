<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class topPanelControl
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
        dateLabel = New Label()
        Label3 = New Label()
        Label1 = New Label()
        Label2 = New Label()
        adminImgPanel = New Panel()
        titlePanel = New Panel()
        titlePanel.SuspendLayout()
        SuspendLayout()
        ' 
        ' dateLabel
        ' 
        dateLabel.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left
        dateLabel.Font = New Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        dateLabel.Location = New Point(142, 34)
        dateLabel.Name = "dateLabel"
        dateLabel.Size = New Size(368, 37)
        dateLabel.TabIndex = 10
        dateLabel.Text = "S"
        dateLabel.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' Label3
        ' 
        Label3.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left
        Label3.Font = New Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label3.Location = New Point(62, 34)
        Label3.Name = "Label3"
        Label3.Size = New Size(98, 37)
        Label3.TabIndex = 9
        Label3.Text = "Date:"
        Label3.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' Label1
        ' 
        Label1.Dock = DockStyle.Fill
        Label1.Font = New Font("Segoe UI", 21.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Label1.Location = New Point(0, 0)
        Label1.Name = "Label1"
        Label1.Size = New Size(432, 42)
        Label1.TabIndex = 2
        Label1.Text = "WHOLESALE"
        Label1.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' Label2
        ' 
        Label2.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Right
        Label2.Font = New Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label2.Location = New Point(1486, 34)
        Label2.Name = "Label2"
        Label2.Padding = New Padding(0, 0, 10, 0)
        Label2.Size = New Size(175, 37)
        Label2.TabIndex = 8
        Label2.Text = "Admin"
        Label2.TextAlign = ContentAlignment.MiddleRight
        ' 
        ' adminImgPanel
        ' 
        adminImgPanel.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Right
        adminImgPanel.BackgroundImage = My.Resources.Resources.user__1_
        adminImgPanel.BackgroundImageLayout = ImageLayout.Stretch
        adminImgPanel.Location = New Point(1667, 11)
        adminImgPanel.Name = "adminImgPanel"
        adminImgPanel.Size = New Size(82, 79)
        adminImgPanel.TabIndex = 7
        ' 
        ' titlePanel
        ' 
        titlePanel.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        titlePanel.AutoSizeMode = AutoSizeMode.GrowAndShrink
        titlePanel.Controls.Add(Label1)
        titlePanel.Location = New Point(670, 29)
        titlePanel.Name = "titlePanel"
        titlePanel.Size = New Size(432, 42)
        titlePanel.TabIndex = 6
        ' 
        ' topPanelControl
        ' 
        AutoScaleDimensions = New SizeF(7.0F, 15.0F)
        AutoScaleMode = AutoScaleMode.Font
        AutoSize = True
        AutoSizeMode = AutoSizeMode.GrowAndShrink
        Controls.Add(dateLabel)
        Controls.Add(Label3)
        Controls.Add(Label2)
        Controls.Add(adminImgPanel)
        Controls.Add(titlePanel)
        Name = "topPanelControl"
        Size = New Size(1810, 100)
        titlePanel.ResumeLayout(False)
        ResumeLayout(False)
    End Sub

    Friend WithEvents dateLabel As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents adminImgPanel As Panel
    Friend WithEvents titlePanel As Panel

End Class
