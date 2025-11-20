Public Class PaymentDetailsForm
    Private _paymentMethod As String
    Private _payerName As String = ""
    Private _referenceNumber As String = ""
    Private _bankName As String = ""

    ''' <summary>
    ''' Gets the payer name entered by the user
    ''' </summary>
    Public ReadOnly Property PayerName As String
        Get
            Return _payerName
        End Get
    End Property

    ''' <summary>
    ''' Gets the reference number entered by the user
    ''' </summary>
    Public ReadOnly Property ReferenceNumber As String
        Get
            Return _referenceNumber
        End Get
    End Property

    ''' <summary>
    ''' Gets the bank name entered by the user (for Bank Transaction only)
    ''' </summary>
    Public ReadOnly Property BankName As String
        Get
            Return _bankName
        End Get
    End Property

    Public Sub New(paymentMethod As String)
        InitializeComponent()
        _paymentMethod = paymentMethod
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.StartPosition = FormStartPosition.CenterParent
        Me.BackColor = Color.FromArgb(230, 216, 177)
        ' Initial size - will be adjusted in Load event based on payment method
        Me.Size = New Size(450, 420)  ' Increased height for better spacing
    End Sub

    Private Sub PaymentDetailsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Set form title based on payment method
        Me.Text = $"{_paymentMethod} Payment Details"

        ' Setup title label
        lblTitle.Text = $"{_paymentMethod} Payment Information"
        lblTitle.Font = New Font("Segoe UI", 14, FontStyle.Bold)
        lblTitle.ForeColor = Color.FromArgb(79, 51, 40)
        lblTitle.TextAlign = ContentAlignment.MiddleCenter
        lblTitle.Dock = DockStyle.Top
        lblTitle.Height = 50

        ' Setup labels
        lblPayerName.Text = "Payer Name:"
        lblPayerName.Font = New Font("Segoe UI", 10, FontStyle.Regular)
        lblPayerName.ForeColor = Color.FromArgb(79, 51, 40)

        lblReferenceNumber.Text = "Reference Number:"
        lblReferenceNumber.Font = New Font("Segoe UI", 10, FontStyle.Regular)
        lblReferenceNumber.ForeColor = Color.FromArgb(79, 51, 40)

        lblBankName.Text = "Bank Name:"
        lblBankName.Font = New Font("Segoe UI", 10, FontStyle.Regular)
        lblBankName.ForeColor = Color.FromArgb(79, 51, 40)

        ' Setup textboxes with white background
        txtPayerName.Font = New Font("Segoe UI", 10, FontStyle.Regular)
        txtPayerName.BackColor = Color.White  ' Changed to white
        txtPayerName.ForeColor = Color.FromArgb(79, 51, 40)

        txtReferenceNumber.Font = New Font("Segoe UI", 10, FontStyle.Regular)
        txtReferenceNumber.BackColor = Color.White  ' Changed to white
        txtReferenceNumber.ForeColor = Color.FromArgb(79, 51, 40)

        txtBankName.Font = New Font("Segoe UI", 10, FontStyle.Regular)
        txtBankName.BackColor = Color.White  ' Changed to white
        txtBankName.ForeColor = Color.FromArgb(79, 51, 40)

        ' Show/hide bank name field based on payment method and adjust layout
        If _paymentMethod = "Bank Transaction" Then
            ' Bank Transaction - show all fields
            lblBankName.Visible = True
            txtBankName.Visible = True
            ' Move buttons down for better spacing (80px below last field)
            btnConfirm.Top = 310  ' Moved down from 260
            btnCancel.Top = 310  ' Moved down from 260
            ' Set form height to accommodate all fields + buttons + margins
            Me.Height = 400  ' Increased for better spacing
        Else
            ' GCash - hide bank name field and move buttons up
            lblBankName.Visible = False
            txtBankName.Visible = False
            ' Move buttons down for better spacing (80px below last field)
            btnConfirm.Top = 240  ' Moved down from 200
            btnCancel.Top = 240  ' Moved down from 200
            ' Adjust form height for GCash (smaller)
            Me.Height = 330  ' Increased from 290 for better spacing
        End If

        ' Setup buttons
        btnConfirm.BackColor = Color.FromArgb(147, 53, 53)
        btnConfirm.ForeColor = Color.FromArgb(230, 216, 177)
        btnConfirm.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        btnConfirm.FlatStyle = FlatStyle.Flat
        btnConfirm.Cursor = Cursors.Hand

        btnCancel.BackColor = Color.FromArgb(79, 51, 40)
        btnCancel.ForeColor = Color.FromArgb(230, 216, 177)
        btnCancel.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        btnCancel.FlatStyle = FlatStyle.Flat
        btnCancel.Cursor = Cursors.Hand

        ' Set focus to payer name
        txtPayerName.Focus()
    End Sub

    Private Sub btnConfirm_Click(sender As Object, e As EventArgs) Handles btnConfirm.Click
        ' Validate payer name
        If String.IsNullOrWhiteSpace(txtPayerName.Text) Then
            MessageBox.Show("Please enter the payer name.", "Required Field",
               MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtPayerName.Focus()
            Return
        End If

        ' Validate reference number
        If String.IsNullOrWhiteSpace(txtReferenceNumber.Text) Then
            MessageBox.Show("Please enter the reference number.", "Required Field",
               MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtReferenceNumber.Focus()
            Return
        End If

        ' Validate bank name if Bank Transaction
        If _paymentMethod = "Bank Transaction" AndAlso String.IsNullOrWhiteSpace(txtBankName.Text) Then
            MessageBox.Show("Please enter the bank name.", "Required Field",
     MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtBankName.Focus()
            Return
        End If

        ' Store values
        _payerName = txtPayerName.Text.Trim()
        _referenceNumber = txtReferenceNumber.Text.Trim()
        _bankName = If(_paymentMethod = "Bank Transaction", txtBankName.Text.Trim(), "")

        ' Set dialog result and close
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

End Class