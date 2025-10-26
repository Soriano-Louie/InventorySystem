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
        cancelButton = New Button()
        loginButton = New Button()
        passwordLabel = New Label()
        psswrdTxtBx = New TextBox()
        userLabel = New Label()
        usrnmTxtBx = New TextBox()
        Panel1 = New Panel()
        Label1 = New Label()
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
        brownPanel.Name = "brownPanel"
        brownPanel.Size = New Size(644, 729)
        brownPanel.TabIndex = 0
        ' 
        ' formPanel
        ' 
        formPanel.Anchor = AnchorStyles.None
        formPanel.Controls.Add(cancelButton)
        formPanel.Controls.Add(loginButton)
        formPanel.Controls.Add(passwordLabel)
        formPanel.Controls.Add(psswrdTxtBx)
        formPanel.Controls.Add(userLabel)
        formPanel.Controls.Add(usrnmTxtBx)
        formPanel.Controls.Add(Panel1)
        formPanel.Location = New Point(99, 100)
        formPanel.Name = "formPanel"
        formPanel.Size = New Size(444, 530)
        formPanel.TabIndex = 1
        ' 
        ' cancelButton
        ' 
        cancelButton.Anchor = AnchorStyles.None
        cancelButton.AutoSize = True
        cancelButton.FlatStyle = FlatStyle.Flat
        cancelButton.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        cancelButton.Location = New Point(238, 410)
        cancelButton.Name = "cancelButton"
        cancelButton.Size = New Size(78, 33)
        cancelButton.TabIndex = 4
        cancelButton.Text = "Cancel"
        cancelButton.UseVisualStyleBackColor = True
        ' 
        ' loginButton
        ' 
        loginButton.Anchor = AnchorStyles.None
        loginButton.AutoSize = True
        loginButton.FlatStyle = FlatStyle.Flat
        loginButton.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        loginButton.Location = New Point(128, 410)
        loginButton.Name = "loginButton"
        loginButton.Size = New Size(78, 33)
        loginButton.TabIndex = 3
        loginButton.Text = "Login"
        loginButton.UseVisualStyleBackColor = True
        ' 
        ' passwordLabel
        ' 
        passwordLabel.AutoSize = True
        passwordLabel.Font = New Font("Segoe UI Semibold", 12F, FontStyle.Bold)
        passwordLabel.Location = New Point(106, 340)
        passwordLabel.Name = "passwordLabel"
        passwordLabel.Size = New Size(79, 21)
        passwordLabel.TabIndex = 6
        passwordLabel.Text = "Password"
        ' 
        ' psswrdTxtBx
        ' 
        psswrdTxtBx.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        psswrdTxtBx.Location = New Point(106, 368)
        psswrdTxtBx.Name = "psswrdTxtBx"
        psswrdTxtBx.Size = New Size(231, 23)
        psswrdTxtBx.TabIndex = 2
        ' 
        ' userLabel
        ' 
        userLabel.AutoSize = True
        userLabel.Font = New Font("Segoe UI Semibold", 12F, FontStyle.Bold)
        userLabel.Location = New Point(105, 264)
        userLabel.Name = "userLabel"
        userLabel.Size = New Size(83, 21)
        userLabel.TabIndex = 4
        userLabel.Text = "Username"
        ' 
        ' usrnmTxtBx
        ' 
        usrnmTxtBx.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        usrnmTxtBx.Location = New Point(105, 292)
        usrnmTxtBx.Name = "usrnmTxtBx"
        usrnmTxtBx.Size = New Size(231, 23)
        usrnmTxtBx.TabIndex = 1
        ' 
        ' Panel1
        ' 
        Panel1.Controls.Add(Label1)
        Panel1.Controls.Add(projectTitle)
        Panel1.Controls.Add(projectTitle2)
        Panel1.Dock = DockStyle.Top
        Panel1.Location = New Point(0, 0)
        Panel1.Margin = New Padding(3, 2, 3, 2)
        Panel1.Name = "Panel1"
        Panel1.Size = New Size(444, 227)
        Panel1.TabIndex = 7
        ' 
        ' Label1
        ' 
        Label1.Anchor = AnchorStyles.None
        Label1.AutoSize = True
        Label1.BackColor = Color.Transparent
        Label1.Font = New Font("Segoe UI", 20F, FontStyle.Bold)
        Label1.ForeColor = Color.White
        Label1.Location = New Point(65, 125)
        Label1.Margin = New Padding(0)
        Label1.Name = "Label1"
        Label1.Size = New Size(337, 37)
        Label1.TabIndex = 3
        Label1.Text = "Operations Management"
        Label1.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' projectTitle
        ' 
        projectTitle.Anchor = AnchorStyles.None
        projectTitle.AutoSize = True
        projectTitle.BackColor = Color.Transparent
        projectTitle.Font = New Font("Segoe UI", 20F, FontStyle.Bold)
        projectTitle.ForeColor = Color.White
        projectTitle.Location = New Point(95, 80)
        projectTitle.Margin = New Padding(0)
        projectTitle.Name = "projectTitle"
        projectTitle.Size = New Size(255, 37)
        projectTitle.TabIndex = 1
        projectTitle.Text = "Baldo Pet Supplies"
        projectTitle.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' projectTitle2
        ' 
        projectTitle2.Anchor = AnchorStyles.None
        projectTitle2.AutoSize = True
        projectTitle2.BackColor = Color.Transparent
        projectTitle2.Font = New Font("Segoe UI", 20F, FontStyle.Bold)
        projectTitle2.ForeColor = Color.White
        projectTitle2.Location = New Point(169, 171)
        projectTitle2.Margin = New Padding(0)
        projectTitle2.Name = "projectTitle2"
        projectTitle2.Size = New Size(109, 37)
        projectTitle2.TabIndex = 2
        projectTitle2.Text = "System"
        projectTitle2.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' pawPrintPictureBox
        ' 
        pawPrintPictureBox.BackgroundImage = My.Resources.Resources.luxa_org_opacity_changed__opacityPawPrints
        pawPrintPictureBox.BackgroundImageLayout = ImageLayout.Stretch
        pawPrintPictureBox.Location = New Point(-153, -134)
        pawPrintPictureBox.Name = "pawPrintPictureBox"
        pawPrintPictureBox.Size = New Size(931, 1000)
        pawPrintPictureBox.TabIndex = 0
        pawPrintPictureBox.TabStop = False
        ' 
        ' logoPictureBox
        ' 
        logoPictureBox.BackgroundImage = My.Resources.Resources.Untitled_design
        logoPictureBox.BackgroundImageLayout = ImageLayout.Stretch
        logoPictureBox.Location = New Point(647, 0)
        logoPictureBox.Name = "logoPictureBox"
        logoPictureBox.Size = New Size(676, 729)
        logoPictureBox.SizeMode = PictureBoxSizeMode.StretchImage
        logoPictureBox.TabIndex = 10
        logoPictureBox.TabStop = False
        ' 
        ' LoginForm
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1313, 727)
        Controls.Add(logoPictureBox)
        Controls.Add(brownPanel)
        FormBorderStyle = FormBorderStyle.FixedToolWindow
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
    Friend WithEvents cancelButton As Button
    Friend WithEvents Label1 As Label

End Class
