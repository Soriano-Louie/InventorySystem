<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class WholesaleDashboard
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        FlowLayoutPanel1 = New FlowLayoutPanel()
        Panel1 = New Panel()
        Label4 = New Label()
        Label1 = New Label()
        Panel2 = New Panel()
        Label5 = New Label()
        Label2 = New Label()
        Panel3 = New Panel()
        Label3 = New Label()
        FlowLayoutPanel1.SuspendLayout()
        Panel1.SuspendLayout()
        Panel2.SuspendLayout()
        Panel3.SuspendLayout()
        SuspendLayout()
        ' 
        ' FlowLayoutPanel1
        ' 
        FlowLayoutPanel1.Controls.Add(Panel1)
        FlowLayoutPanel1.Controls.Add(Panel2)
        FlowLayoutPanel1.Controls.Add(Panel3)
        FlowLayoutPanel1.Dock = DockStyle.Fill
        FlowLayoutPanel1.Location = New Point(0, 0)
        FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        FlowLayoutPanel1.Padding = New Padding(60)
        FlowLayoutPanel1.Size = New Size(1717, 792)
        FlowLayoutPanel1.TabIndex = 0
        ' 
        ' Panel1
        ' 
        Panel1.Controls.Add(Label4)
        Panel1.Controls.Add(Label1)
        Panel1.Location = New Point(63, 63)
        Panel1.Name = "Panel1"
        Panel1.Size = New Size(1218, 234)
        Panel1.TabIndex = 0
        ' 
        ' Label4
        ' 
        Label4.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        Label4.AutoSize = True
        Label4.Font = New Font("Segoe UI", 21.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Label4.Location = New Point(464, 167)
        Label4.Name = "Label4"
        Label4.Size = New Size(286, 40)
        Label4.TabIndex = 0
        Label4.Text = "Total Sales (TODAY)"
        Label4.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' Label1
        ' 
        Label1.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        Label1.AutoSize = True
        Label1.Font = New Font("Segoe UI", 21.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Label1.Location = New Point(468, 174)
        Label1.Name = "Label1"
        Label1.Size = New Size(286, 40)
        Label1.TabIndex = 0
        Label1.Text = "Total Sales (TODAY)"
        Label1.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' Panel2
        ' 
        Panel2.Controls.Add(Label5)
        Panel2.Controls.Add(Label2)
        Panel2.Location = New Point(1287, 63)
        Panel2.Name = "Panel2"
        Panel2.Size = New Size(303, 234)
        Panel2.TabIndex = 1
        ' 
        ' Label5
        ' 
        Label5.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        Label5.AutoSize = True
        Label5.Font = New Font("Segoe UI", 21.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Label5.Location = New Point(62, 178)
        Label5.Name = "Label5"
        Label5.Size = New Size(184, 40)
        Label5.TabIndex = 2
        Label5.Text = "REGISTERED"
        Label5.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' Label2
        ' 
        Label2.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        Label2.AutoSize = True
        Label2.Font = New Font("Segoe UI", 21.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Label2.Location = New Point(99, 138)
        Label2.Name = "Label2"
        Label2.Size = New Size(104, 40)
        Label2.TabIndex = 1
        Label2.Text = "USERS"
        Label2.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' Panel3
        ' 
        Panel3.Controls.Add(Label3)
        Panel3.Location = New Point(63, 303)
        Panel3.Name = "Panel3"
        Panel3.Size = New Size(303, 234)
        Panel3.TabIndex = 2
        ' 
        ' Label3
        ' 
        Label3.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        Label3.AutoSize = True
        Label3.Font = New Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Label3.Location = New Point(32, 198)
        Label3.Name = "Label3"
        Label3.Size = New Size(178, 25)
        Label3.TabIndex = 1
        Label3.Text = "Total Sales (TODAY)"
        Label3.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' WholesaleDashboard
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1717, 792)
        Controls.Add(FlowLayoutPanel1)
        FormBorderStyle = FormBorderStyle.FixedDialog
        Name = "WholesaleDashboard"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Wholesale Dashboard"
        WindowState = FormWindowState.Maximized
        FlowLayoutPanel1.ResumeLayout(False)
        Panel1.ResumeLayout(False)
        Panel1.PerformLayout()
        Panel2.ResumeLayout(False)
        Panel2.PerformLayout()
        Panel3.ResumeLayout(False)
        Panel3.PerformLayout()
        ResumeLayout(False)
    End Sub

    Friend WithEvents FlowLayoutPanel1 As FlowLayoutPanel
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Label1 As Label
    Friend WithEvents Panel2 As Panel
    Friend WithEvents Panel3 As Panel
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
End Class
