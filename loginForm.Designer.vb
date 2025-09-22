<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class loginForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
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
        components = New ComponentModel.Container()
        BindingSource1 = New BindingSource(components)
        brownPanel = New Panel()
        formPanel = New Panel()
        loginButton = New Button()
        passwordLabel = New Label()
        psswrdTxtBx = New TextBox()
        userLabel = New Label()
        usrnmTxtBx = New TextBox()
        Panel1 = New Panel()
        projectTitle = New Label()
        projectTitle2 = New Label()
        pawPrintPictureBox = New PictureBox()
        logoPictureBox = New PictureBox()
        CType(BindingSource1, ComponentModel.ISupportInitialize).BeginInit()
        brownPanel.SuspendLayout()
        formPanel.SuspendLayout()
        Panel1.SuspendLayout()
        CType(pawPrintPictureBox, ComponentModel.ISupportInitialize).BeginInit()
        CType(logoPictureBox, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' brownPanel
        ' 
        brownPanel.Controls.Add(formPanel)
        brownPanel.Controls.Add(pawPrintPictureBox)
        brownPanel.Location = New Point(0, 0)
        brownPanel.Margin = New Padding(3, 4, 3, 4)
        brownPanel.Name = "brownPanel"
        brownPanel.Size = New Size(736, 972)
        brownPanel.TabIndex = 0
        ' 
        ' formPanel
        ' 
        formPanel.Anchor = AnchorStyles.None
        formPanel.Controls.Add(loginButton)
        formPanel.Controls.Add(passwordLabel)
        formPanel.Controls.Add(psswrdTxtBx)
        formPanel.Controls.Add(userLabel)
        formPanel.Controls.Add(usrnmTxtBx)
        formPanel.Controls.Add(Panel1)
        formPanel.Location = New Point(113, 133)
        formPanel.Margin = New Padding(3, 4, 3, 4)
        formPanel.Name = "formPanel"
        formPanel.Size = New Size(507, 707)
        formPanel.TabIndex = 1
        ' 
        ' loginButton
        ' 
        loginButton.Anchor = AnchorStyles.None
        loginButton.AutoSize = True
        loginButton.FlatStyle = FlatStyle.Flat
        loginButton.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        loginButton.Location = New Point(203, 557)
        loginButton.Margin = New Padding(3, 4, 3, 4)
        loginButton.Name = "loginButton"
        loginButton.Size = New Size(102, 53)
        loginButton.TabIndex = 2
        loginButton.Text = "Login"
        loginButton.UseVisualStyleBackColor = True
        ' 
        ' passwordLabel
        ' 
        passwordLabel.AutoSize = True
        passwordLabel.Font = New Font("Segoe UI Semibold", 12F, FontStyle.Bold)
        passwordLabel.Location = New Point(121, 453)
        passwordLabel.Name = "passwordLabel"
        passwordLabel.Size = New Size(97, 28)
        passwordLabel.TabIndex = 6
        passwordLabel.Text = "Password"
        ' 
        ' psswrdTxtBx
        ' 
        psswrdTxtBx.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        psswrdTxtBx.Location = New Point(121, 491)
        psswrdTxtBx.Margin = New Padding(3, 4, 3, 4)
        psswrdTxtBx.Name = "psswrdTxtBx"
        psswrdTxtBx.Size = New Size(263, 27)
        psswrdTxtBx.TabIndex = 5
        ' 
        ' userLabel
        ' 
        userLabel.AutoSize = True
        userLabel.Font = New Font("Segoe UI Semibold", 12F, FontStyle.Bold)
        userLabel.Location = New Point(120, 352)
        userLabel.Name = "userLabel"
        userLabel.Size = New Size(104, 28)
        userLabel.TabIndex = 4
        userLabel.Text = "Username"
        ' 
        ' usrnmTxtBx
        ' 
        usrnmTxtBx.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        usrnmTxtBx.Location = New Point(120, 389)
        usrnmTxtBx.Margin = New Padding(3, 4, 3, 4)
        usrnmTxtBx.Name = "usrnmTxtBx"
        usrnmTxtBx.Size = New Size(263, 27)
        usrnmTxtBx.TabIndex = 3
        ' 
        ' Panel1
        ' 
        Panel1.Controls.Add(projectTitle)
        Panel1.Controls.Add(projectTitle2)
        Panel1.Dock = DockStyle.Top
        Panel1.Location = New Point(0, 0)
        Panel1.Name = "Panel1"
        Panel1.Size = New Size(507, 254)
        Panel1.TabIndex = 7
        ' 
        ' projectTitle
        ' 
        projectTitle.Anchor = AnchorStyles.None
        projectTitle.AutoSize = True
        projectTitle.BackColor = Color.Transparent
        projectTitle.Font = New Font("Segoe UI", 20F, FontStyle.Bold)
        projectTitle.ForeColor = Color.White
        projectTitle.Location = New Point(71, 78)
        projectTitle.Margin = New Padding(0)
        projectTitle.Name = "projectTitle"
        projectTitle.Size = New Size(397, 46)
        projectTitle.TabIndex = 1
        projectTitle.Text = "Inventory Management"
        projectTitle.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' projectTitle2
        ' 
        projectTitle2.Anchor = AnchorStyles.None
        projectTitle2.AutoSize = True
        projectTitle2.BackColor = Color.Transparent
        projectTitle2.Font = New Font("Segoe UI", 20F, FontStyle.Bold)
        projectTitle2.ForeColor = Color.White
        projectTitle2.Location = New Point(191, 144)
        projectTitle2.Margin = New Padding(0)
        projectTitle2.Name = "projectTitle2"
        projectTitle2.Size = New Size(133, 46)
        projectTitle2.TabIndex = 2
        projectTitle2.Text = "System"
        projectTitle2.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' pawPrintPictureBox
        ' 
        pawPrintPictureBox.BackgroundImage = My.Resources.Resources.luxa_org_opacity_changed__opacityPawPrints
        pawPrintPictureBox.BackgroundImageLayout = ImageLayout.Stretch
        pawPrintPictureBox.Location = New Point(-175, -179)
        pawPrintPictureBox.Margin = New Padding(3, 4, 3, 4)
        pawPrintPictureBox.Name = "pawPrintPictureBox"
        pawPrintPictureBox.Size = New Size(1064, 1333)
        pawPrintPictureBox.TabIndex = 0
        pawPrintPictureBox.TabStop = False
        ' 
        ' logoPictureBox
        ' 
        logoPictureBox.BackgroundImage = My.Resources.Resources.Untitled_design
        logoPictureBox.BackgroundImageLayout = ImageLayout.Stretch
        logoPictureBox.Location = New Point(739, 0)
        logoPictureBox.Margin = New Padding(3, 4, 3, 4)
        logoPictureBox.Name = "logoPictureBox"
        logoPictureBox.Size = New Size(773, 972)
        logoPictureBox.SizeMode = PictureBoxSizeMode.StretchImage
        logoPictureBox.TabIndex = 10
        logoPictureBox.TabStop = False
        ' 
        ' LoginForm
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1501, 969)
        Controls.Add(logoPictureBox)
        Controls.Add(brownPanel)
        FormBorderStyle = FormBorderStyle.FixedToolWindow
        Margin = New Padding(3, 4, 3, 4)
        Name = "LoginForm"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Login"
        CType(BindingSource1, ComponentModel.ISupportInitialize).EndInit()
        brownPanel.ResumeLayout(False)
        formPanel.ResumeLayout(False)
        formPanel.PerformLayout()
        Panel1.ResumeLayout(False)
        Panel1.PerformLayout()
        CType(pawPrintPictureBox, ComponentModel.ISupportInitialize).EndInit()
        CType(logoPictureBox, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub

    Friend WithEvents BindingSource1 As BindingSource
    Friend WithEvents brownPanel As Panel
    Friend WithEvents projectTitle As Label
    Friend WithEvents pawPrintPictureBox As PictureBox
    Friend WithEvents formPanel As Panel
    Friend WithEvents Panel21 As Panel
    Friend WithEvents Panel22 As Panel
    Friend WithEvents projectTitle2 As Label
    Friend WithEvents passwordLabel As Label
    Friend WithEvents psswrdTxtBx As TextBox
    Friend WithEvents userLabel As Label
    Friend WithEvents usrnmTxtBx As TextBox
    Friend WithEvents loginButton As Button
    Friend WithEvents logoPictureBox As PictureBox
    Friend WithEvents Panel1 As Panel

End Class
