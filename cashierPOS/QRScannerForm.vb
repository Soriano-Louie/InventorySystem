Imports AForge.Video
Imports AForge.Video.DirectShow
Imports ZXing
Imports ZXing.Windows.Compatibility
Imports Microsoft.Data.SqlClient
Imports System.Drawing.Imaging

Public Class QRScannerForm
    Inherits Form

    Private videoSource As VideoCaptureDevice
    Private reader As BarcodeReader
    Private isScanning As Boolean = True
    Private isProcessing As Boolean = False
    Private parentForm As posForm
    Private lastScannedCode As String = ""
    Private lastScanTime As DateTime = DateTime.MinValue
    Private scanCooldownSeconds As Integer = 3 ' Cooldown after successful scan

    ' Controls
    Private WithEvents pictureBoxCamera As PictureBox
    Private WithEvents labelStatus As Label
    Private WithEvents btnClose As Button
    Private WithEvents btnResumeScan As Button

    Public Sub New(parent As posForm)
        InitializeComponent()
        Me.parentForm = parent

        ' Initialize the barcode reader
        reader = New BarcodeReader()
        reader.AutoRotate = True
        reader.Options = New ZXing.Common.DecodingOptions()
        reader.Options.TryHarder = True
        reader.Options.PossibleFormats = New List(Of BarcodeFormat) From {BarcodeFormat.QR_CODE}

        ConfigureScanner()
    End Sub

    Private Sub InitializeComponent()
        Me.SuspendLayout()

        ' Form properties
        Me.Text = "QR Code Scanner"
        Me.Size = New Size(800, 650)
        Me.StartPosition = FormStartPosition.CenterParent
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False

        ' PictureBox for camera feed
        pictureBoxCamera = New PictureBox()
        pictureBoxCamera.Location = New Point(20, 20)
        pictureBoxCamera.Size = New Size(760, 480)
        pictureBoxCamera.BorderStyle = BorderStyle.FixedSingle
        pictureBoxCamera.SizeMode = PictureBoxSizeMode.Zoom
        Me.Controls.Add(pictureBoxCamera)

        ' Status Label
        labelStatus = New Label()
        labelStatus.Location = New Point(20, 510)
        labelStatus.Size = New Size(760, 30)
        labelStatus.Text = "Initializing camera..."
        labelStatus.Font = New Font("Segoe UI", 12, FontStyle.Bold)
        labelStatus.TextAlign = ContentAlignment.MiddleCenter
        labelStatus.ForeColor = Color.FromArgb(79, 51, 40)
        Me.Controls.Add(labelStatus)

        ' Resume Scan Button
        btnResumeScan = New Button()
        btnResumeScan.Text = "Resume Scanning (SPACE)"
        btnResumeScan.Location = New Point(240, 555)
        btnResumeScan.Size = New Size(200, 40)
        btnResumeScan.BackColor = Color.FromArgb(144, 238, 144) ' Light green
        btnResumeScan.ForeColor = Color.FromArgb(79, 51, 40)
        btnResumeScan.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        btnResumeScan.Visible = False ' Hidden by default
        AddHandler btnResumeScan.Click, AddressOf BtnResumeScan_Click
        Me.Controls.Add(btnResumeScan)

        ' Close Button
        btnClose = New Button()
        btnClose.Text = "Close (ESC)"
        btnClose.Location = New Point(460, 555)
        btnClose.Size = New Size(120, 40)
        btnClose.BackColor = Color.FromArgb(230, 216, 177)
        btnClose.ForeColor = Color.FromArgb(79, 51, 40)
        btnClose.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        AddHandler btnClose.Click, AddressOf BtnClose_Click
        Me.Controls.Add(btnClose)

        Me.BackColor = Color.FromArgb(224, 166, 109)
        Me.KeyPreview = True

        Me.ResumeLayout(False)
    End Sub

    Private Sub ConfigureScanner()
        Try
            ' Get available video devices
            Dim videoDevices As New FilterInfoCollection(FilterCategory.VideoInputDevice)

            If videoDevices.Count = 0 Then
                MessageBox.Show("No camera detected on this device.", "Camera Error",
                  MessageBoxButtons.OK, MessageBoxIcon.Error)
                Me.Close()
                Return
            End If

            ' Use the first available camera
            videoSource = New VideoCaptureDevice(videoDevices(0).MonikerString)
            AddHandler videoSource.NewFrame, AddressOf VideoSource_NewFrame
            videoSource.Start()

            labelStatus.Text = "Ready to scan QR code..."
        Catch ex As Exception
            MessageBox.Show("Error initializing camera: " & ex.Message, "Camera Error",
                     MessageBoxButtons.OK, MessageBoxIcon.Error)
            Me.Close()
        End Try
    End Sub

    Private Sub VideoSource_NewFrame(sender As Object, eventArgs As NewFrameEventArgs)
        Try
            Dim frame As Bitmap = DirectCast(eventArgs.Frame.Clone(), Bitmap)

            ' Update picture box on UI thread
            If Me.InvokeRequired Then
                Try
                    Me.Invoke(New Action(Sub()
                                             If pictureBoxCamera IsNot Nothing AndAlso Not pictureBoxCamera.IsDisposed Then
                                                 If pictureBoxCamera.Image IsNot Nothing Then
                                                     pictureBoxCamera.Image.Dispose()
                                                 End If
                                                 pictureBoxCamera.Image = DirectCast(frame.Clone(), Bitmap)
                                             End If
                                         End Sub))
                Catch ex As ObjectDisposedException
                    ' Form is closing, ignore
                    frame.Dispose()
                    Return
                Catch ex As InvalidOperationException
                    ' Form handle destroyed, ignore
                    frame.Dispose()
                    Return
                End Try
            End If

            ' Try to decode QR code only if scanning is active and not already processing
            If isScanning AndAlso Not isProcessing Then
                Dim result = reader.Decode(frame)
                If result IsNot Nothing Then
                    ' Prevent duplicate scans within cooldown period
                    If result.Text = lastScannedCode AndAlso (DateTime.Now - lastScanTime).TotalSeconds < scanCooldownSeconds Then
                        frame.Dispose()
                        Return
                    End If

                    lastScannedCode = result.Text
                    lastScanTime = DateTime.Now
                    isProcessing = True

                    ' Process the scanned QR code on UI thread
                    Try
                        Me.Invoke(New Action(Sub() ProcessScannedQRCode(result.Text)))
                    Catch ex As ObjectDisposedException
                        ' Form is closing, ignore
                        isProcessing = False
                    Catch ex As InvalidOperationException
                        ' Form handle destroyed, ignore
                        isProcessing = False
                    End Try
                End If
            End If

            frame.Dispose()
        Catch ex As Exception
            ' Ignore frame processing errors
        End Try
    End Sub

    Private Sub ProcessScannedQRCode(scannedData As String)
        Try
            ' Stop scanning immediately
            isScanning = False

            labelStatus.Text = "QR Code detected! Validating..."

            ' Validate QR code against database
            Dim productInfo = ValidateQRCodeInDatabase(scannedData)

            If productInfo.IsValid Then
                ' Show quantity input dialog
                Dim quantity As Integer = ShowQuantityDialog(productInfo.ProductName)

                If quantity > 0 Then
                    ' Add to cart
                    parentForm.AddProductToCart(
     productInfo.ProductID,
  productInfo.ProductName,
         quantity,
   productInfo.UnitPrice,
          productInfo.CategoryID,
     productInfo.IsVATApplicable
  )

                    MessageBox.Show($"Added {quantity} x {productInfo.ProductName} to cart!",
  "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

                    ' Update status and show resume button
                    labelStatus.Text = "Product added! Remove QR code and press SPACE or click Resume to continue..."
                    labelStatus.ForeColor = Color.Green
                    btnResumeScan.Visible = True
                    btnResumeScan.Focus()

                    ' Keep scanning paused until user resumes
                    isProcessing = False
                    ' DON'T set isScanning = True here - wait for user action
                Else
                    ' User cancelled - resume scanning immediately
                    labelStatus.Text = "Cancelled. Ready to scan QR code..."
                    labelStatus.ForeColor = Color.FromArgb(79, 51, 40)
                    isScanning = True
                    isProcessing = False
                End If
            Else
                MessageBox.Show("QR Code not found in database.", "Invalid QR Code",
              MessageBoxButtons.OK, MessageBoxIcon.Warning)
                labelStatus.Text = "Invalid code. Ready to scan QR code..."
                labelStatus.ForeColor = Color.FromArgb(79, 51, 40)

                ' Resume scanning after invalid code
                isScanning = True
                isProcessing = False
            End If

        Catch ex As Exception
            MessageBox.Show("Error processing QR code: " & ex.Message, "Error",
          MessageBoxButtons.OK, MessageBoxIcon.Error)
            labelStatus.Text = "Error occurred. Ready to scan QR code..."
            labelStatus.ForeColor = Color.FromArgb(79, 51, 40)

            ' Resume scanning
            isScanning = True
            isProcessing = False
        End Try
    End Sub

    Private Sub StartScanCooldown()
        ' Disable scanning temporarily
        isScanning = False
        btnResumeScan.Visible = True ' Show the resume button

        ' Change status label to indicate cooldown
        labelStatus.Text = $"Scan successful! Please wait {scanCooldownSeconds} seconds..."

        ' Start a timer for the cooldown period
        Dim cooldownEndTime As DateTime = DateTime.Now.AddSeconds(scanCooldownSeconds)
        Dim cooldownTimer As New System.Windows.Forms.Timer()
        AddHandler cooldownTimer.Tick,
         Sub()
             ' Check if cooldown period has ended
             If DateTime.Now >= cooldownEndTime Then
                 ' Stop the timer
                 cooldownTimer.Stop()
                 btnResumeScan.Visible = False ' Hide the resume button
                 isScanning = True ' Resume scanning
                 labelStatus.Text = "Ready to scan QR code..."
             End If
         End Sub
        cooldownTimer.Interval = 1000 ' 1 second interval
        cooldownTimer.Start()
    End Sub

    Private Function ValidateQRCodeInDatabase(scannedData As String) As ProductInfo
        Dim result As New ProductInfo()

        Try
            Using conn As New SqlConnection(SharedUtilities.GetConnectionString())
                conn.Open()

                ' Query both wholesale and retail products
                Dim query As String = "
                SELECT ProductID, ProductName, RetailPrice, CategoryID, 
                    ISNULL(IsVATApplicable, 0) AS IsVATApplicable, QRCodeImage
                    FROM wholesaleProducts 
                WHERE QRCodeImage IS NOT NULL
                
                UNION
         
                SELECT ProductID, ProductName, RetailPrice, CategoryID,
                ISNULL(IsVATApplicable, 0) AS IsVATApplicable, QRCodeImage
                        FROM retailProducts 
                WHERE QRCodeImage IS NOT NULL"

                Using cmd As New SqlCommand(query, conn)
                    Using reader = cmd.ExecuteReader()
                        While reader.Read()
                            ' Get the QR code image from database
                            If Not reader.IsDBNull(5) Then
                                Dim qrImageBytes As Byte() = DirectCast(reader("QRCodeImage"), Byte())

                                ' Convert binary to image and decode
                                Using ms As New IO.MemoryStream(qrImageBytes)
                                    Using qrImage As Bitmap = New Bitmap(ms)
                                        ' Create a new barcode reader instance
                                        Dim qrReader As New BarcodeReader()
                                        qrReader.AutoRotate = True
                                        qrReader.Options = New ZXing.Common.DecodingOptions()
                                        qrReader.Options.TryHarder = True

                                        Dim qrResult = qrReader.Decode(qrImage)

                                        If qrResult IsNot Nothing AndAlso qrResult.Text = scannedData Then
                                            ' Match found!
                                            result.IsValid = True
                                            result.ProductID = reader.GetInt32(0)
                                            result.ProductName = reader.GetString(1)
                                            result.UnitPrice = reader.GetDecimal(2)
                                            result.CategoryID = reader.GetInt32(3)
                                            result.IsVATApplicable = reader.GetBoolean(4)
                                            Exit While
                                        End If
                                    End Using
                                End Using
                            End If
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Database error: " & ex.Message, "Error",
          MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        Return result
    End Function

    Private Function ShowQuantityDialog(productName As String) As Integer
        Dim inputForm As New Form()
        inputForm.Text = "Enter Quantity"
        inputForm.Size = New Size(400, 200)
        inputForm.StartPosition = FormStartPosition.CenterParent
        inputForm.FormBorderStyle = FormBorderStyle.FixedDialog
        inputForm.MaximizeBox = False
        inputForm.MinimizeBox = False
        inputForm.BackColor = Color.FromArgb(224, 166, 109)

        Dim lblProduct As New Label()
        lblProduct.Text = "Product: " & productName
        lblProduct.Location = New Point(20, 20)
        lblProduct.Size = New Size(360, 25)
        lblProduct.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        lblProduct.ForeColor = Color.FromArgb(79, 51, 40)
        inputForm.Controls.Add(lblProduct)

        Dim lblQuantity As New Label()
        lblQuantity.Text = "Enter Quantity:"
        lblQuantity.Location = New Point(20, 60)
        lblQuantity.Size = New Size(120, 25)
        lblQuantity.Font = New Font("Segoe UI", 10)
        lblQuantity.ForeColor = Color.FromArgb(79, 51, 40)
        inputForm.Controls.Add(lblQuantity)

        Dim txtQuantity As New NumericUpDown()
        txtQuantity.Location = New Point(150, 60)
        txtQuantity.Size = New Size(100, 25)
        txtQuantity.Minimum = 1
        txtQuantity.Maximum = 10000
        txtQuantity.Value = 1
        txtQuantity.Font = New Font("Segoe UI", 10)

        ' Select all text when the control gets focus
        AddHandler txtQuantity.Enter, Sub(s, e)
                                          txtQuantity.Select(0, txtQuantity.Text.Length)
                                      End Sub

        ' Handle when user clicks on the control
        AddHandler txtQuantity.MouseDown, Sub(s, e)
                                              txtQuantity.Select(0, txtQuantity.Text.Length)
                                          End Sub

        inputForm.Controls.Add(txtQuantity)

        Dim btnOK As New Button()
        btnOK.Text = "OK"
        btnOK.DialogResult = DialogResult.OK
        btnOK.Location = New Point(100, 110)
        btnOK.Size = New Size(80, 35)
        btnOK.BackColor = Color.FromArgb(230, 216, 177)
        btnOK.ForeColor = Color.FromArgb(79, 51, 40)
        btnOK.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        inputForm.Controls.Add(btnOK)

        Dim btnCancel As New Button()
        btnCancel.Text = "Cancel"
        btnCancel.DialogResult = DialogResult.Cancel
        btnCancel.Location = New Point(220, 110)
        btnCancel.Size = New Size(80, 35)
        btnCancel.BackColor = Color.FromArgb(230, 216, 177)
        btnCancel.ForeColor = Color.FromArgb(79, 51, 40)
        btnCancel.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        inputForm.Controls.Add(btnCancel)

        inputForm.AcceptButton = btnOK
        inputForm.CancelButton = btnCancel

        ' Set focus to the NumericUpDown and select all text when form is shown
        AddHandler inputForm.Shown, Sub(s, e)
                                        txtQuantity.Focus()
                                        txtQuantity.Select(0, txtQuantity.Text.Length)
                                    End Sub

        If inputForm.ShowDialog() = DialogResult.OK Then
            Return CInt(txtQuantity.Value)
        Else
            Return 0
        End If
    End Function

    Private Sub BtnClose_Click(sender As Object, e As EventArgs)
        ' Stop scanning first
        isScanning = False
        Me.Close()
    End Sub

    Private Sub BtnResumeScan_Click(sender As Object, e As EventArgs)
        ' Resume scanning
        btnResumeScan.Visible = False
        labelStatus.Text = "Ready to scan QR code..."
        labelStatus.ForeColor = Color.FromArgb(79, 51, 40)
        lastScannedCode = "" ' Clear last scanned code
        lastScanTime = DateTime.MinValue
        isScanning = True
    End Sub

    Protected Overrides Sub OnKeyDown(e As KeyEventArgs)
        If e.KeyCode = Keys.Escape Then
            ' Stop scanning first
            isScanning = False
            Me.Close()
        ElseIf e.KeyCode = Keys.Space Then
            ' Resume scanning with Space key
            If btnResumeScan.Visible Then
                BtnResumeScan_Click(Nothing, Nothing)
            End If
        End If
        MyBase.OnKeyDown(e)
    End Sub

    Protected Overrides Sub OnFormClosing(e As FormClosingEventArgs)
        Try
            ' Stop scanning immediately
            isScanning = False

            ' Stop video source with timeout
            If videoSource IsNot Nothing AndAlso videoSource.IsRunning Then
                videoSource.SignalToStop()

                ' Wait with timeout to prevent hanging
                Dim stopTask = Task.Run(Sub()
                                            Try
                                                videoSource.WaitForStop()
                                            Catch
                                                ' Ignore errors during stop
                                            End Try
                                        End Sub)

                ' Wait maximum 2 seconds for video to stop
                If Not stopTask.Wait(2000) Then
                    ' Force continue if timeout
                    Console.WriteLine("Video source stop timeout - continuing with close")
                End If
            End If

            ' Cleanup picture box image
            If pictureBoxCamera IsNot Nothing AndAlso Not pictureBoxCamera.IsDisposed Then
                If pictureBoxCamera.Image IsNot Nothing Then
                    Try
                        pictureBoxCamera.Image.Dispose()
                        pictureBoxCamera.Image = Nothing
                    Catch
                        ' Ignore disposal errors
                    End Try
                End If
            End If
        Catch ex As Exception
            ' Ignore any errors during cleanup
            Console.WriteLine($"Error during form closing: {ex.Message}")
        End Try

        MyBase.OnFormClosing(e)
    End Sub

    ' Helper class to store product information
    Private Class ProductInfo
        Public Property IsValid As Boolean = False
        Public Property ProductID As Integer
        Public Property ProductName As String
        Public Property UnitPrice As Decimal
        Public Property CategoryID As Integer
        Public Property IsVATApplicable As Boolean
    End Class
End Class
