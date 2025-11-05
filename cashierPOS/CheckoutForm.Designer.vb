<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class CheckoutForm
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
        Me.lblTotalAmount = New System.Windows.Forms.Label()
        Me.lblSelectedPayment = New System.Windows.Forms.Label()
        Me.btnCash = New System.Windows.Forms.Button()
        Me.btnGCash = New System.Windows.Forms.Button()
        Me.btnBankTransaction = New System.Windows.Forms.Button()
        Me.btnConfirm = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'lblTotalAmount
        '
        Me.lblTotalAmount.AutoSize = True
        Me.lblTotalAmount.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.lblTotalAmount.Location = New System.Drawing.Point(30, 20)
        Me.lblTotalAmount.Name = "lblTotalAmount"
        Me.lblTotalAmount.Size = New System.Drawing.Size(185, 25)
        Me.lblTotalAmount.TabIndex = 0
        Me.lblTotalAmount.Text = "Total Amount: ₱0.00"
        '
        'lblSelectedPayment
        '
        Me.lblSelectedPayment.AutoSize = True
        Me.lblSelectedPayment.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point)
        Me.lblSelectedPayment.Location = New System.Drawing.Point(30, 290)
        Me.lblSelectedPayment.Name = "lblSelectedPayment"
        Me.lblSelectedPayment.Size = New System.Drawing.Size(205, 19)
        Me.lblSelectedPayment.TabIndex = 1
        Me.lblSelectedPayment.Text = "Please select a payment method"
        '
        'btnCash
        '
        Me.btnCash.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnCash.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCash.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.btnCash.Location = New System.Drawing.Point(30, 100)
        Me.btnCash.Name = "btnCash"
        Me.btnCash.Size = New System.Drawing.Size(340, 50)
        Me.btnCash.TabIndex = 2
        Me.btnCash.Text = "Cash"
        Me.btnCash.UseVisualStyleBackColor = True
        '
        'btnGCash
        '
        Me.btnGCash.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnGCash.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnGCash.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.btnGCash.Location = New System.Drawing.Point(30, 160)
        Me.btnGCash.Name = "btnGCash"
        Me.btnGCash.Size = New System.Drawing.Size(340, 50)
        Me.btnGCash.TabIndex = 3
        Me.btnGCash.Text = "GCash"
        Me.btnGCash.UseVisualStyleBackColor = True
        '
        'btnBankTransaction
        '
        Me.btnBankTransaction.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnBankTransaction.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnBankTransaction.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.btnBankTransaction.Location = New System.Drawing.Point(30, 220)
        Me.btnBankTransaction.Name = "btnBankTransaction"
        Me.btnBankTransaction.Size = New System.Drawing.Size(340, 50)
        Me.btnBankTransaction.TabIndex = 4
        Me.btnBankTransaction.Text = "Bank Transaction"
        Me.btnBankTransaction.UseVisualStyleBackColor = True
        '
        'btnConfirm
        '
        Me.btnConfirm.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnConfirm.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnConfirm.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.btnConfirm.Location = New System.Drawing.Point(30, 330)
        Me.btnConfirm.Name = "btnConfirm"
        Me.btnConfirm.Size = New System.Drawing.Size(160, 50)
        Me.btnConfirm.TabIndex = 5
        Me.btnConfirm.Text = "Confirm Checkout"
        Me.btnConfirm.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancel.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.btnCancel.Location = New System.Drawing.Point(210, 330)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(160, 50)
        Me.btnCancel.TabIndex = 6
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 11.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.Label1.Location = New System.Drawing.Point(30, 65)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(192, 20)
        Me.Label1.TabIndex = 7
        Me.Label1.Text = "Select Payment Method:"
        '
        'CheckoutForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(400, 410)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnConfirm)
        Me.Controls.Add(Me.btnBankTransaction)
        Me.Controls.Add(Me.btnGCash)
        Me.Controls.Add(Me.btnCash)
        Me.Controls.Add(Me.lblSelectedPayment)
        Me.Controls.Add(Me.lblTotalAmount)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "CheckoutForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Checkout"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblTotalAmount As Label
    Friend WithEvents lblSelectedPayment As Label
    Friend WithEvents btnCash As Button
    Friend WithEvents btnGCash As Button
    Friend WithEvents btnBankTransaction As Button
    Friend WithEvents btnConfirm As Button
    Friend WithEvents btnCancel As Button
    Friend WithEvents Label1 As Label
End Class
