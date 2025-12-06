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

    Private frameCounter As Integer = 0
    Private lastFrameTime As DateTime = DateTime.Now

    Public Sub New(parent As posForm)
        InitializeComponent()
        Me.parentForm = parent

        ' Initialize the barcode reader with optimized settings
        reader = New BarcodeReader()
        reader.AutoRotate = True
        reader.Options = New ZXing.Common.DecodingOptions()
        reader.Options.TryHarder = True
        reader.Options.PossibleFormats = New List(Of BarcodeFormat) From {BarcodeFormat.QR_CODE}
        reader.Options.TryInverted = True
        reader.TryInverted = True
    End Sub

    Protected Overrides Sub OnShown(e As EventArgs)
        MyBase.OnShown(e)
        ' Start camera after form is shown so handle is created
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
        pictureBoxCamera.BackColor = Color.Black
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

        ' Add a timer to update status with FPS
        Dim statusTimer As New Timer()
        statusTimer.Interval = 1000
        AddHandler statusTimer.Tick, Sub()
                                         If videoSource IsNot Nothing AndAlso videoSource.IsRunning Then
                                             Dim timeSinceLastFrame = (DateTime.Now - lastFrameTime).TotalSeconds
                                             If timeSinceLastFrame > 2 Then
                                                 labelStatus.Text = "Camera running but NO FRAMES received! Check camera permissions."
                                                 labelStatus.ForeColor = Color.Red
                                             ElseIf frameCounter > 0 Then
                                                 Dim fpsApprox = Math.Max(0, frameCounter / Math.Max(1, (DateTime.Now - lastFrameTime).TotalSeconds))
                                                 'labelStatus.Text = $"Ready to scan QR code... (FPS: ~{fpsApprox:F0})"
                                                 labelStatus.Text = $"Ready to scan QR code..."
                                                 labelStatus.ForeColor = Color.FromArgb(79, 51, 40)
                                             End If
                                         End If
                                     End Sub
        statusTimer.Start()

        ' Close Button
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
                MessageBox.Show("No camera detected on this device.", "Camera Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Me.Close()
                Return
            End If

            Console.WriteLine($"Found {videoDevices.Count} camera device(s)")
            For i As Integer = 0 To videoDevices.Count - 1
                Console.WriteLine($"  [{i}] {videoDevices(i).Name}")
            Next

            Dim cameraInitialized As Boolean = False
            Dim lastError As Exception = Nothing

            ' Build try order: prefer index 1 then 0 if multiple, then remaining indices
            Dim tryOrder As New List(Of Integer)
            If videoDevices.Count > 1 Then
                tryOrder.Add(1)
                tryOrder.Add(0)
                For i As Integer = 2 To videoDevices.Count - 1
                    tryOrder.Add(i)
                Next
            Else
                tryOrder.Add(0)
            End If

            Dim preferredSizes As New List(Of Size) From {
                New Size(1280, 720),
                New Size(1024, 768),
                New Size(800, 600),
                New Size(640, 480),
                New Size(320, 240)
            }

            For Each cameraIndex As Integer In tryOrder
                Try
                    Console.WriteLine($"Trying camera {cameraIndex}: {videoDevices(cameraIndex).Name}")

                    videoSource = New VideoCaptureDevice(videoDevices(cameraIndex).MonikerString)

                    Dim capabilities = videoSource.VideoCapabilities

                    If capabilities IsNot Nothing AndAlso capabilities.Length > 0 Then
                        Dim orderedCaps = capabilities.OrderByDescending(Function(c) c.FrameSize.Width * c.FrameSize.Height).ToList()
                        Dim bestCap As VideoCapabilities = Nothing

                        For Each prefSize In preferredSizes
                            For Each cap In orderedCaps
                                If cap.FrameSize.Equals(prefSize) Then
                                    bestCap = cap
                                    Exit For
                                End If
                            Next
                            If bestCap IsNot Nothing Then Exit For
                        Next

                        If bestCap Is Nothing Then
                            bestCap = orderedCaps.First()
                        End If

                        videoSource.VideoResolution = bestCap
                    End If

                    AddHandler videoSource.NewFrame, AddressOf VideoSource_NewFrame
                    videoSource.DesiredFrameRate = 30
                    videoSource.Start()

                    ' Wait up to 2 seconds for camera to start
                    Dim sw As New System.Diagnostics.Stopwatch()
                    sw.Start()
                    While Not videoSource.IsRunning AndAlso sw.ElapsedMilliseconds < 2000
                        System.Threading.Thread.Sleep(100)
                    End While

                    If videoSource.IsRunning Then
                        cameraInitialized = True
                        Console.WriteLine($"Camera {cameraIndex} started successfully")
                        labelStatus.Text = "Ready to scan QR code..."
                        labelStatus.ForeColor = Color.FromArgb(79, 51, 40)
                        Exit For
                    Else
                        Console.WriteLine($"Camera {cameraIndex} failed to start")
                        RemoveHandler videoSource.NewFrame, AddressOf VideoSource_NewFrame
                        If videoSource.IsRunning Then
                            Try
                                videoSource.SignalToStop()
                            Catch
                            End Try
                        End If
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
                msg &= "Troubleshooting:" & vbCrLf & "• Close other apps using the camera" & vbCrLf & "• Check Windows camera permissions" & vbCrLf & "• Update camera drivers" & vbCrLf & "• Restart the application"
                MessageBox.Show(msg, "Camera Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Me.Close()
            End If
        Catch ex As Exception
            MessageBox.Show($"Camera initialization error: {ex.Message}" & vbCrLf & vbCrLf & "Please check:" & vbCrLf & "- Camera is connected" & vbCrLf & "- Drivers are installed" & vbCrLf & "- No other app is using camera", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Me.Close()
        End Try
    End Sub

    Private Sub VideoSource_NewFrame(sender As Object, eventArgs As NewFrameEventArgs)
        Try
            frameCounter += 1
            lastFrameTime = DateTime.Now

            ' Clone frame for display and decoding
            Dim displayBmp As Bitmap = Nothing
            Dim decodeBmp As Bitmap = Nothing

            Try
                displayBmp = DirectCast(eventArgs.Frame.Clone(), Bitmap)
                decodeBmp = DirectCast(eventArgs.Frame.Clone(), Bitmap)
            Catch ex As Exception
                If displayBmp IsNot Nothing Then displayBmp.Dispose()
                If decodeBmp IsNot Nothing Then decodeBmp.Dispose()
                Console.WriteLine($"Frame clone error: {ex.Message}")
                Return
            End Try

            ' Update PictureBox on UI thread
            Try
                Dim setImage = New Action(Sub()
                                              Try
                                                  If pictureBoxCamera Is Nothing OrElse pictureBoxCamera.IsDisposed Then Return
                                                  Dim oldImg = pictureBoxCamera.Image
                                                  pictureBoxCamera.Image = displayBmp
                                                  pictureBoxCamera.Invalidate()
                                                  If oldImg IsNot Nothing Then
                                                      Try
                                                          oldImg.Dispose()
                                                      Catch
                                                      End Try
                                                  End If
                                              Catch exInner As Exception
                                                  Console.WriteLine($"UI update error: {exInner.Message}")
                                                  If displayBmp IsNot Nothing Then displayBmp.Dispose()
                                              End Try
                                          End Sub)

                If Me.IsHandleCreated AndAlso Me.InvokeRequired Then
                    Me.BeginInvoke(setImage)
                Else
                    setImage()
                End If
            Catch ex As Exception
                If displayBmp IsNot Nothing Then displayBmp.Dispose()
                Console.WriteLine($"Display update error: {ex.Message}")
            End Try

            ' Decode on background thread to avoid blocking frame handler
            If isScanning AndAlso Not isProcessing Then
                Task.Run(Sub()
                             Try
                                 Dim result = reader.Decode(decodeBmp)
                                 If result IsNot Nothing Then
                                     If result.Text = lastScannedCode AndAlso (DateTime.Now - lastScanTime).TotalSeconds < scanCooldownSeconds Then
                                         decodeBmp.Dispose()
                                         Return
                                     End If

                                     lastScannedCode = result.Text
                                     lastScanTime = DateTime.Now
                                     isProcessing = True

                                     Try
                                         Me.BeginInvoke(New Action(Sub() ProcessScannedQRCode(result.Text)))
                                     Catch
                                         isProcessing = False
                                     End Try
                                 End If
                             Catch exDecode As Exception
                                 Console.WriteLine($"Decode error: {exDecode.Message}")
                             Finally
                                 If decodeBmp IsNot Nothing Then decodeBmp.Dispose()
                             End Try
                         End Sub)
            Else
                If decodeBmp IsNot Nothing Then decodeBmp.Dispose()
            End If
        Catch ex As Exception
            Console.WriteLine($"Frame processing error: {ex.Message}")
        End Try
    End Sub

    Private Sub ProcessScannedQRCode(scannedData As String)
        Try
            isScanning = False
            labelStatus.Text = "QR Code detected! Validating..."
            labelStatus.ForeColor = Color.Blue

            Dim productInfo = ValidateQRCodeInDatabase(scannedData)

            If productInfo.IsValid Then
                parentForm.AddProductToCart(productInfo.ProductID, productInfo.ProductName, 1, productInfo.UnitPrice, productInfo.CategoryID, productInfo.IsVATApplicable, productInfo.ProductType)

                Console.Beep(800, 100)
                labelStatus.Text = $"Added: {productInfo.ProductName}"
                labelStatus.ForeColor = Color.Green

                System.Threading.Thread.Sleep(300)

                labelStatus.Text = "Ready to scan next QR code..."
                labelStatus.ForeColor = Color.FromArgb(79, 51, 40)
                isScanning = True
                isProcessing = False
            Else
                labelStatus.Text = "Invalid QR Code! Scan again..."
                labelStatus.ForeColor = Color.Red
                System.Threading.Thread.Sleep(500)
                labelStatus.Text = "Ready to scan QR code..."
                labelStatus.ForeColor = Color.FromArgb(79, 51, 40)
                isScanning = True
                isProcessing = False
            End If
        Catch ex As Exception
            MessageBox.Show("Error processing QR code: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            labelStatus.Text = "Error occurred. Ready to scan QR code..."
            labelStatus.ForeColor = Color.FromArgb(79, 51, 40)
            isScanning = True
            isProcessing = False
        End Try
    End Sub

    Private Function ValidateQRCodeInDatabase(scannedData As String) As ProductInfo
        Dim result As New ProductInfo()

        Try
            Using conn As New SqlConnection(SharedUtilities.GetConnectionString())
                conn.Open()

                Dim wholesaleQuery As String = "
                    SELECT ProductID, ProductName, RetailPrice, CategoryID,
                           ISNULL(IsVATApplicable, 0) AS IsVATApplicable, QRCodeImage, 'Wholesale' AS ProductType
                    FROM wholesaleProducts
                    WHERE QRCodeImage IS NOT NULL"

                Using cmd As New SqlCommand(wholesaleQuery, conn)
                    Using rdr = cmd.ExecuteReader()
                        While rdr.Read()
                            If Not rdr.IsDBNull(5) Then
                                Dim qrImageBytes As Byte() = DirectCast(rdr("QRCodeImage"), Byte())
                                Using ms As New IO.MemoryStream(qrImageBytes)
                                    Using qrImage As Bitmap = New Bitmap(ms)
                                        Dim qrReader As New BarcodeReader()
                                        qrReader.AutoRotate = True
                                        qrReader.Options = New ZXing.Common.DecodingOptions()
                                        qrReader.Options.TryHarder = True

                                        Dim qrResult = qrReader.Decode(qrImage)
                                        If qrResult IsNot Nothing AndAlso qrResult.Text = scannedData Then
                                            result.IsValid = True
                                            result.ProductID = rdr.GetInt32(0)
                                            result.ProductName = rdr.GetString(1)
                                            result.UnitPrice = rdr.GetDecimal(2)
                                            result.CategoryID = rdr.GetInt32(3)
                                            result.IsVATApplicable = rdr.GetBoolean(4)
                                            result.ProductType = "Wholesale"
                                            Exit While
                                        End If
                                    End Using
                                End Using
                            End If
                        End While
                    End Using
                End Using

                If Not result.IsValid Then
                    Dim retailQuery As String = "
                        SELECT ProductID, ProductName, RetailPrice, CategoryID,
                               ISNULL(IsVATApplicable, 0) AS IsVATApplicable, QRCodeImage, 'Retail' AS ProductType
                        FROM retailProducts
                        WHERE QRCodeImage IS NOT NULL"

                    Using cmd As New SqlCommand(retailQuery, conn)
                        Using rdr = cmd.ExecuteReader()
                            While rdr.Read()
                                If Not rdr.IsDBNull(5) Then
                                    Dim qrImageBytes As Byte() = DirectCast(rdr("QRCodeImage"), Byte())
                                    Using ms As New IO.MemoryStream(qrImageBytes)
                                        Using qrImage As Bitmap = New Bitmap(ms)
                                            Dim qrReader As New BarcodeReader()
                                            qrReader.AutoRotate = True
                                            qrReader.Options = New ZXing.Common.DecodingOptions()
                                            qrReader.Options.TryHarder = True

                                            Dim qrResult = qrReader.Decode(qrImage)
                                            If qrResult IsNot Nothing AndAlso qrResult.Text = scannedData Then
                                                result.IsValid = True
                                                result.ProductID = rdr.GetInt32(0)
                                                result.ProductName = rdr.GetString(1)
                                                result.UnitPrice = rdr.GetDecimal(2)
                                                result.CategoryID = rdr.GetInt32(3)
                                                result.IsVATApplicable = rdr.GetBoolean(4)
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
            MessageBox.Show("Database error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        Return result
    End Function

    Private Sub BtnClose_Click(sender As Object, e As EventArgs)
        isScanning = False
        Me.Close()
    End Sub

    Protected Overrides Sub OnKeyDown(e As KeyEventArgs)
        If e.KeyCode = Keys.Escape Then
            isScanning = False
            Me.Close()
        End If
        MyBase.OnKeyDown(e)
    End Sub

    Protected Overrides Sub OnFormClosing(e As FormClosingEventArgs)
        Try
            isScanning = False
            If videoSource IsNot Nothing Then
                Try
                    RemoveHandler videoSource.NewFrame, AddressOf VideoSource_NewFrame
                Catch
                End Try

                If videoSource.IsRunning Then
                    Try
                        videoSource.SignalToStop()
                    Catch
                    End Try

                    Dim sw As New Stopwatch()
                    sw.Start()
                    While videoSource.IsRunning AndAlso sw.ElapsedMilliseconds < 2000
                        System.Threading.Thread.Sleep(50)
                    End While
                End If

                Try
                    If videoSource.IsRunning Then
                        videoSource.SignalToStop()
                    End If
                Catch
                End Try
            End If

            If pictureBoxCamera IsNot Nothing AndAlso pictureBoxCamera.Image IsNot Nothing Then
                Try
                    pictureBoxCamera.Image.Dispose()
                    pictureBoxCamera.Image = Nothing
                Catch
                End Try
            End If
        Catch ex As Exception
            Console.WriteLine($"Error during form closing: {ex.Message}")
        End Try

        MyBase.OnFormClosing(e)
    End Sub

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