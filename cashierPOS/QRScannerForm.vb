Imports AForge.Video
Imports AForge.Video.DirectShow
Imports Microsoft.Data.SqlClient
Imports ZXing
Imports ZXing.Windows.Compatibility

Public Class QRScannerForm
    Inherits Form

    Private videoSource As VideoCaptureDevice
    Private reader As BarcodeReader
    Private isScanning As Boolean = True
    Private isProcessing As Boolean = False
    Private parentForm As posForm
    Private lastScannedCode As String = ""
    Private lastScanTime As DateTime = DateTime.MinValue
    Private scanCooldownSeconds As Integer = 1 ' Reduced cooldown for faster continuous scanning

    ' Controls
    Private WithEvents pictureBoxCamera As PictureBox

    Private WithEvents labelStatus As Label
    Private WithEvents btnClose As Button

    Public Sub New(parent As posForm)
        InitializeComponent()
        Me.parentForm = parent

        ' Initialize the barcode reader with optimized settings
        reader = New BarcodeReader()
        reader.AutoRotate = True
        reader.Options = New ZXing.Common.DecodingOptions()
        reader.Options.TryHarder = True
        reader.Options.PossibleFormats = New List(Of BarcodeFormat) From {BarcodeFormat.QR_CODE}

        ' Additional options for better QR code detection across different cameras
        reader.Options.TryInverted = True
        reader.TryInverted = True

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

        ' Close Button (centered since no resume button)
        btnClose = New Button()
        btnClose.Text = "Close (ESC)"
        btnClose.Location = New Point(340, 555)
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
            Dim videoDevices As New FilterInfoCollection(FilterCategory.VideoInputDevice)

            If videoDevices.Count = 0 Then
                MessageBox.Show("No camera detected on this device.", "Camera Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error)
                Me.Close()
                Return
            End If

            Console.WriteLine($"Found {videoDevices.Count} camera device(s)")
            For i As Integer = 0 To videoDevices.Count - 1
                Console.WriteLine($"  [{i}] {videoDevices(i).Name}")
            Next

            Dim cameraInitialized As Boolean = False
            Dim lastError As Exception = Nothing

            For cameraIndex As Integer = 0 To Math.Min(videoDevices.Count - 1, 2)
                Try
                    Console.WriteLine($"Trying camera {cameraIndex}: {videoDevices(cameraIndex).Name}")

                    videoSource = New VideoCaptureDevice(videoDevices(cameraIndex).MonikerString)

                    Dim capabilities = videoSource.VideoCapabilities

                    If capabilities IsNot Nothing AndAlso capabilities.Length > 0 Then
                        Console.WriteLine($"  Available resolutions: {capabilities.Length}")

                        Dim preferredSizes As New List(Of Size) From {
                            New Size(640, 480),
                            New Size(800, 600),
                            New Size(1280, 720),
                            New Size(1024, 768),
                            New Size(320, 240)
                        }

                        Dim bestCap As VideoCapabilities = Nothing

                        For Each prefSize In preferredSizes
                            For Each cap In capabilities
                                If cap.FrameSize.Equals(prefSize) Then
                                    bestCap = cap
                                    Console.WriteLine($"  Using resolution: {cap.FrameSize.Width}x{cap.FrameSize.Height} @ {cap.AverageFrameRate} fps")
                                    Exit For
                                End If
                            Next
                            If bestCap IsNot Nothing Then Exit For
                        Next

                        If bestCap Is Nothing Then
                            bestCap = capabilities(0)
                            Console.WriteLine($"  Using default: {bestCap.FrameSize.Width}x{bestCap.FrameSize.Height}")
                        End If

                        videoSource.VideoResolution = bestCap
                    End If

                    AddHandler videoSource.NewFrame, AddressOf VideoSource_NewFrame
                    videoSource.Start()

                    System.Threading.Thread.Sleep(500)

                    If videoSource.IsRunning Then
                        cameraInitialized = True
                        Console.WriteLine($"Camera {cameraIndex} started successfully")
                        labelStatus.Text = "Ready to scan QR code..."
                        Exit For
                    Else
                        Console.WriteLine($"Camera {cameraIndex} failed to start")
                        RemoveHandler videoSource.NewFrame, AddressOf VideoSource_NewFrame
                        videoSource = Nothing
                    End If
                Catch ex As Exception
                    Console.WriteLine($"Camera {cameraIndex} error: {ex.Message}")
                    lastError = ex
                    If videoSource IsNot Nothing Then
                        Try
                            RemoveHandler videoSource.NewFrame, AddressOf VideoSource_NewFrame
                            If videoSource.IsRunning Then videoSource.SignalToStop()
                        Catch
                        End Try
                        videoSource = Nothing
                    End If
                End Try
            Next

            If Not cameraInitialized Then
                Dim msg As String = "Failed to initialize camera." & vbCrLf & vbCrLf

                If lastError IsNot Nothing Then
                    msg &= $"Error: {lastError.Message}" & vbCrLf & vbCrLf
                End If

                msg &= "Troubleshooting:" & vbCrLf &
                       "• Close other apps using the camera" & vbCrLf +
                       "• Check Windows camera permissions" & vbCrLf +
                       "• Update camera drivers" & vbCrLf +
                       "• Restart the application"

                MessageBox.Show(msg, "Camera Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Me.Close()
            End If
        Catch ex As Exception
            MessageBox.Show($"Camera initialization error: {ex.Message}" & vbCrLf & vbCrLf &
                          "Please check:" & vbCrLf &
                          "- Camera is connected" & vbCrLf &
                          "- Drivers are installed" & vbCrLf &
                          "- No other app is using camera",
                          "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
            ' Stop scanning temporarily during processing
            isScanning = False

            labelStatus.Text = "QR Code detected! Validating..."
            labelStatus.ForeColor = Color.Blue

            ' Validate QR code against database
            Dim productInfo = ValidateQRCodeInDatabase(scannedData)

            If productInfo.IsValid Then
                ' Add to cart with quantity of 1 and product type
                parentForm.AddProductToCart(
     productInfo.ProductID,
  productInfo.ProductName,
         1, ' Always add quantity of 1 per scan
   productInfo.UnitPrice,
          productInfo.CategoryID,
     productInfo.IsVATApplicable,
       productInfo.ProductType ' Pass the product type
  )

                ' Play success beep sound
                Console.Beep(800, 100) ' 800 Hz frequency, 100 ms duration

                ' Show brief success feedback
                labelStatus.Text = $"Added: {productInfo.ProductName}"
                labelStatus.ForeColor = Color.Green

                ' Brief pause before resuming (gives visual feedback)
                System.Threading.Thread.Sleep(300)

                ' Automatically resume scanning for next product
                labelStatus.Text = "Ready to scan next QR code..."
                labelStatus.ForeColor = Color.FromArgb(79, 51, 40)
                isScanning = True
                isProcessing = False
            Else
                ' Invalid QR code - no beep for errors
                labelStatus.Text = "Invalid QR Code! Scan again..."
                labelStatus.ForeColor = Color.Red

                ' Brief pause to show error
                System.Threading.Thread.Sleep(500)

                ' Resume scanning after invalid code
                labelStatus.Text = "Ready to scan QR code..."
                labelStatus.ForeColor = Color.FromArgb(79, 51, 40)
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

    Private Function ValidateQRCodeInDatabase(scannedData As String) As ProductInfo
        Dim result As New ProductInfo()

        Try
            Using conn As New SqlConnection(SharedUtilities.GetConnectionString())
                conn.Open()

                ' Query wholesale products first with product type
                Dim wholesaleQuery As String = "
         SELECT ProductID, ProductName, RetailPrice, CategoryID,
           ISNULL(IsVATApplicable, 0) AS IsVATApplicable, QRCodeImage, 'Wholesale' AS ProductType
        FROM wholesaleProducts
               WHERE QRCodeImage IS NOT NULL"

                Using cmd As New SqlCommand(wholesaleQuery, conn)
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
                                            ' Match found in wholesale!
                                            result.IsValid = True
                                            result.ProductID = reader.GetInt32(0)
                                            result.ProductName = reader.GetString(1)
                                            result.UnitPrice = reader.GetDecimal(2)
                                            result.CategoryID = reader.GetInt32(3)
                                            result.IsVATApplicable = reader.GetBoolean(4)
                                            result.ProductType = "Wholesale"
                                            Exit While
                                        End If
                                    End Using
                                End Using
                            End If
                        End While
                    End Using
                End Using

                ' If not found in wholesale, check retail
                If Not result.IsValid Then
                    Dim retailQuery As String = "
  SELECT ProductID, ProductName, RetailPrice, CategoryID,
      ISNULL(IsVATApplicable, 0) AS IsVATApplicable, QRCodeImage, 'Retail' AS ProductType
  FROM retailProducts
           WHERE QRCodeImage IS NOT NULL"

                    Using cmd As New SqlCommand(retailQuery, conn)
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
                                                ' Match found in retail!
                                                result.IsValid = True
                                                result.ProductID = reader.GetInt32(0)
                                                result.ProductName = reader.GetString(1)
                                                result.UnitPrice = reader.GetDecimal(2)
                                                result.CategoryID = reader.GetInt32(3)
                                                result.IsVATApplicable = reader.GetBoolean(4)
                                                result.ProductType = "Retail"
                                                Exit While
                                            End If
                                        End Using
                                    End Using
                                End If
                            End While
                        End Using
                    End Using
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("Database error: " & ex.Message, "Error",
          MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        Return result
    End Function

    Private Sub BtnClose_Click(sender As Object, e As EventArgs)
        ' Stop scanning first
        isScanning = False
        Me.Close()
    End Sub

    Protected Overrides Sub OnKeyDown(e As KeyEventArgs)
        If e.KeyCode = Keys.Escape Then
            ' Stop scanning first
            isScanning = False
            Me.Close()
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
        Public Property ProductType As String = "" ' "Wholesale" or "Retail"
    End Class

End Class