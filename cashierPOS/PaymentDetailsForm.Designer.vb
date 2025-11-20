<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class PaymentDetailsForm
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
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.lblPayerName = New System.Windows.Forms.Label()
        Me.txtPayerName = New System.Windows.Forms.TextBox()
        Me.lblReferenceNumber = New System.Windows.Forms.Label()
        Me.txtReferenceNumber = New System.Windows.Forms.TextBox()
        Me.lblBankName = New System.Windows.Forms.Label()
        Me.txtBankName = New System.Windows.Forms.TextBox()
        Me.btnConfirm = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'lblTitle
        '
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(434, 50)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "Payment Information"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblPayerName
        '
        Me.lblPayerName.AutoSize = True
        Me.lblPayerName.Location = New System.Drawing.Point(30, 70)
        Me.lblPayerName.Name = "lblPayerName"
        Me.lblPayerName.Size = New System.Drawing.Size(78, 15)
        Me.lblPayerName.TabIndex = 1
        Me.lblPayerName.Text = "Payer Name:"
        '
        'txtPayerName
        '
        Me.txtPayerName.Location = New System.Drawing.Point(30, 90)
        Me.txtPayerName.Name = "txtPayerName"
        Me.txtPayerName.Size = New System.Drawing.Size(374, 23)
        Me.txtPayerName.TabIndex = 2
        '
        'lblReferenceNumber
        '
        Me.lblReferenceNumber.AutoSize = True
        Me.lblReferenceNumber.Location = New System.Drawing.Point(30, 130)
        Me.lblReferenceNumber.Name = "lblReferenceNumber"
        Me.lblReferenceNumber.Size = New System.Drawing.Size(112, 15)
        Me.lblReferenceNumber.TabIndex = 3
        Me.lblReferenceNumber.Text = "Reference Number:"
        '
        'txtReferenceNumber
        '
        Me.txtReferenceNumber.Location = New System.Drawing.Point(30, 150)
        Me.txtReferenceNumber.Name = "txtReferenceNumber"
        Me.txtReferenceNumber.Size = New System.Drawing.Size(374, 23)
        Me.txtReferenceNumber.TabIndex = 4
        '
        'lblBankName
        '
        Me.lblBankName.AutoSize = True
        Me.lblBankName.Location = New System.Drawing.Point(30, 190)
        Me.lblBankName.Name = "lblBankName"
        Me.lblBankName.Size = New System.Drawing.Size(73, 15)
        Me.lblBankName.TabIndex = 5
        Me.lblBankName.Text = "Bank Name:"
        '
        'txtBankName
        '
        Me.txtBankName.Location = New System.Drawing.Point(30, 210)
        Me.txtBankName.Name = "txtBankName"
        Me.txtBankName.Size = New System.Drawing.Size(374, 23)
        Me.txtBankName.TabIndex = 6
        '
        'btnConfirm
        '
        Me.btnConfirm.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnConfirm.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnConfirm.Location = New System.Drawing.Point(30, 260)
        Me.btnConfirm.Name = "btnConfirm"
        Me.btnConfirm.Size = New System.Drawing.Size(180, 35)
        Me.btnConfirm.TabIndex = 7
        Me.btnConfirm.Text = "Confirm"
        Me.btnConfirm.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancel.Location = New System.Drawing.Point(224, 260)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(180, 35)
        Me.btnCancel.TabIndex = 8
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'PaymentDetailsForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(434, 311)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnConfirm)
        Me.Controls.Add(Me.txtBankName)
        Me.Controls.Add(Me.lblBankName)
        Me.Controls.Add(Me.txtReferenceNumber)
        Me.Controls.Add(Me.lblReferenceNumber)
        Me.Controls.Add(Me.txtPayerName)
        Me.Controls.Add(Me.lblPayerName)
        Me.Controls.Add(Me.lblTitle)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "PaymentDetailsForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Payment Details"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblTitle As Label
    Friend WithEvents lblPayerName As Label
    Friend WithEvents txtPayerName As TextBox
    Friend WithEvents lblReferenceNumber As Label
    Friend WithEvents txtReferenceNumber As TextBox
    Friend WithEvents lblBankName As Label
    Friend WithEvents txtBankName As TextBox
    Friend WithEvents btnConfirm As Button
    Friend WithEvents btnCancel As Button
End Class
