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
        projectTitle2 = New Label()
        projectTitle = New Label()
        pawPrintPictureBox = New PictureBox()
        logoPictureBox = New PictureBox()
        CType(BindingSource1, ComponentModel.ISupportInitialize).BeginInit()
        brownPanel.SuspendLayout()
        formPanel.SuspendLayout()
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
        formPanel.Controls.Add(loginButton)
        formPanel.Controls.Add(passwordLabel)
        formPanel.Controls.Add(psswrdTxtBx)
        formPanel.Controls.Add(userLabel)
        formPanel.Controls.Add(usrnmTxtBx)
        formPanel.Controls.Add(projectTitle2)
        formPanel.Controls.Add(projectTitle)
        formPanel.Location = New Point(99, 100)
        formPanel.Name = "formPanel"
        formPanel.Size = New Size(444, 530)
        formPanel.TabIndex = 1
        ' 
        ' loginButton
        ' 
        loginButton.FlatStyle = FlatStyle.Flat
        loginButton.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        loginButton.Location = New Point(178, 418)
        loginButton.Name = "loginButton"
        loginButton.Size = New Size(89, 30)
        loginButton.TabIndex = 2
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
        psswrdTxtBx.TabIndex = 5
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
        usrnmTxtBx.TabIndex = 3
        ' 
        ' projectTitle2
        ' 
        projectTitle2.AutoSize = True
        projectTitle2.BackColor = Color.Transparent
        projectTitle2.Font = New Font("Segoe UI", 21.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        projectTitle2.ForeColor = Color.White
        projectTitle2.Location = New Point(167, 129)
        projectTitle2.Margin = New Padding(0)
        projectTitle2.Name = "projectTitle2"
        projectTitle2.Size = New Size(115, 40)
        projectTitle2.TabIndex = 2
        projectTitle2.Text = "System"
        ' 
        ' projectTitle
        ' 
        projectTitle.AutoSize = True
        projectTitle.BackColor = Color.Transparent
        projectTitle.Font = New Font("Segoe UI", 21.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        projectTitle.ForeColor = Color.White
        projectTitle.Location = New Point(49, 78)
        projectTitle.Margin = New Padding(0)
        projectTitle.Name = "projectTitle"
        projectTitle.Size = New Size(344, 40)
        projectTitle.TabIndex = 1
        projectTitle.Text = "Inventory Management"
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
        ' loginForm
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1313, 727)
        Controls.Add(logoPictureBox)
        Controls.Add(brownPanel)
        FormBorderStyle = FormBorderStyle.FixedToolWindow
        Name = "loginForm"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Login"
        CType(BindingSource1, ComponentModel.ISupportInitialize).EndInit()
        brownPanel.ResumeLayout(False)
        formPanel.ResumeLayout(False)
        formPanel.PerformLayout()
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

End Class
