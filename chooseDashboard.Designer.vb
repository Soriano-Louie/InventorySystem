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
        choosePanel = New Panel()
        Label1 = New Label()
        chooseTableLayoutPanel1 = New TableLayoutPanel()
        retailButton = New Button()
        wholeSaleButton = New Button()
        choosePanel.SuspendLayout()
        chooseTableLayoutPanel1.SuspendLayout()
        SuspendLayout()
        ' 
        ' choosePanel
        ' 
        choosePanel.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        choosePanel.AutoSize = True
        choosePanel.Controls.Add(Label1)
        choosePanel.Controls.Add(chooseTableLayoutPanel1)
        choosePanel.Location = New Point(228, 87)
        choosePanel.Name = "choosePanel"
        choosePanel.Padding = New Padding(20, 60, 20, 20)
        choosePanel.Size = New Size(1011, 636)
        choosePanel.TabIndex = 2
        ' 
        ' Label1
        ' 
        Label1.Dock = DockStyle.Top
        Label1.Font = New Font("Segoe UI", 26.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Label1.Location = New Point(20, 60)
        Label1.Name = "Label1"
        Label1.Size = New Size(971, 47)
        Label1.TabIndex = 3
        Label1.Text = "Where do you want to go?"
        Label1.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' chooseTableLayoutPanel1
        ' 
        chooseTableLayoutPanel1.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        chooseTableLayoutPanel1.ColumnCount = 3
        chooseTableLayoutPanel1.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 40F))
        chooseTableLayoutPanel1.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 20F))
        chooseTableLayoutPanel1.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 40F))
        chooseTableLayoutPanel1.Controls.Add(retailButton, 2, 0)
        chooseTableLayoutPanel1.Controls.Add(wholeSaleButton, 0, 0)
        chooseTableLayoutPanel1.Location = New Point(51, 110)
        chooseTableLayoutPanel1.MaximumSize = New Size(900, 500)
        chooseTableLayoutPanel1.MinimumSize = New Size(900, 500)
        chooseTableLayoutPanel1.Name = "chooseTableLayoutPanel1"
        chooseTableLayoutPanel1.RowCount = 1
        chooseTableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Percent, 100F))
        chooseTableLayoutPanel1.Size = New Size(900, 500)
        chooseTableLayoutPanel1.TabIndex = 2
        ' 
        ' retailButton
        ' 
        retailButton.Anchor = AnchorStyles.Right
        retailButton.FlatAppearance.BorderSize = 0
        retailButton.FlatStyle = FlatStyle.Popup
        retailButton.Font = New Font("Segoe UI", 20.25F, FontStyle.Bold)
        retailButton.Image = My.Resources.Resources.store_8771926
        retailButton.Location = New Point(545, 67)
        retailButton.Margin = New Padding(3, 10, 10, 10)
        retailButton.MaximumSize = New Size(800, 400)
        retailButton.Name = "retailButton"
        retailButton.Padding = New Padding(0, 0, 0, 10)
        retailButton.Size = New Size(345, 365)
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
        wholeSaleButton.Image = My.Resources.Resources.wholesaler_160899371
        wholeSaleButton.Location = New Point(10, 67)
        wholeSaleButton.Margin = New Padding(10, 10, 3, 10)
        wholeSaleButton.MaximumSize = New Size(800, 400)
        wholeSaleButton.Name = "wholeSaleButton"
        wholeSaleButton.Padding = New Padding(0, 0, 0, 10)
        wholeSaleButton.Size = New Size(345, 365)
        wholeSaleButton.TabIndex = 0
        wholeSaleButton.Text = "Wholesale"
        wholeSaleButton.TextAlign = ContentAlignment.BottomCenter
        wholeSaleButton.UseVisualStyleBackColor = True
        ' 
        ' ChooseDashboard
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        BackgroundImage = My.Resources.Resources.brownWdPawPrints
        ClientSize = New Size(1469, 810)
        Controls.Add(choosePanel)
        FormBorderStyle = FormBorderStyle.FixedToolWindow
        Name = "ChooseDashboard"
        StartPosition = FormStartPosition.Manual
        Text = "Admin"
        WindowState = FormWindowState.Maximized
        choosePanel.ResumeLayout(False)
        chooseTableLayoutPanel1.ResumeLayout(False)
        ResumeLayout(False)
        PerformLayout()
    End Sub
    Friend WithEvents choosePanel As Panel
    Friend WithEvents retailButton As Button
    Friend WithEvents wholeSaleButton As Button
    Friend WithEvents chooseTableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents Label1 As Label
End Class
