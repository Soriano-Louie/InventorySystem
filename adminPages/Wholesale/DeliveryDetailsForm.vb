Imports Microsoft.Data.SqlClient
Imports Microsoft.Web.WebView2.Core
Imports Microsoft.Web.WebView2.WinForms
Imports System.Drawing.Printing

''' Form to display delivery details with map location
''' Shows delivery information and exact location on an interactive map using WebView2
Public Class DeliveryDetailsForm
    Private deliveryData As deliveryLogsForm.DeliveryInfo
    Private mapView As WebView2
    Private isMapReady As Boolean = False
    Private WithEvents printDocument As New PrintDocument()
    Private printContent As String = ""

    Public Sub New(delivery As deliveryLogsForm.DeliveryInfo)
        InitializeComponent()
        Me.deliveryData = delivery
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.StartPosition = FormStartPosition.CenterParent
        Me.BackColor = Color.FromArgb(230, 216, 177)
        Me.Size = New Size(1000, 700)
        Me.Text = $"Delivery Details - Sale #{delivery.SaleID}"

        InitializeUI()
    End Sub

    Private Sub InitializeUI()
        ' Create main panel
        Dim mainPanel As New Panel()
        mainPanel.Dock = DockStyle.Fill
        mainPanel.BackColor = Color.FromArgb(230, 216, 177)
        mainPanel.Padding = New Padding(20)
        Me.Controls.Add(mainPanel)

        ' Title label
        Dim titleLabel As New Label()
        titleLabel.Text = $"Delivery Information - Sale #{deliveryData.SaleID}"
        titleLabel.Font = New Font("Segoe UI", 16, FontStyle.Bold)
        titleLabel.ForeColor = Color.FromArgb(79, 51, 40)
        titleLabel.AutoSize = True
        titleLabel.Location = New Point(20, 10)
        mainPanel.Controls.Add(titleLabel)

        ' Left panel for delivery details
        Dim leftPanel As New Panel()
        leftPanel.Location = New Point(20, 50)
        leftPanel.Size = New Size(420, 550)
        leftPanel.BackColor = Color.White
        leftPanel.BorderStyle = BorderStyle.FixedSingle
        mainPanel.Controls.Add(leftPanel)

        ' Create details labels
        Dim yPos As Integer = 15

        ' Status with color coding
        Dim statusPanel As New Panel()
        statusPanel.Location = New Point(15, yPos)
        statusPanel.Size = New Size(390, 50)
        statusPanel.BorderStyle = BorderStyle.FixedSingle

        Select Case deliveryData.DeliveryStatus
            Case "Pending"
                statusPanel.BackColor = Color.FromArgb(255, 200, 200)
            Case "In Transit"
                statusPanel.BackColor = Color.FromArgb(255, 220, 180)
            Case "Delivered"
                statusPanel.BackColor = Color.FromArgb(200, 255, 200)
            Case "Cancelled"
                statusPanel.BackColor = Color.LightGray
        End Select

        Dim statusLabel As New Label()
        statusLabel.Text = $"Status: {deliveryData.DeliveryStatus}"
        statusLabel.Font = New Font("Segoe UI", 14, FontStyle.Bold)
        statusLabel.ForeColor = Color.FromArgb(79, 51, 40)
        statusLabel.AutoSize = True
        statusLabel.Location = New Point(10, 12)
        statusPanel.Controls.Add(statusLabel)
        leftPanel.Controls.Add(statusPanel)
        yPos += 65

        ' Product Information
        AddDetailLabel(leftPanel, "Product Information", yPos, True)
        yPos += 30
        AddDetailLabel(leftPanel, $"Product: {deliveryData.ProductName}", yPos)
        yPos += 25
        AddDetailLabel(leftPanel, $"Quantity: {deliveryData.QuantitySold} units", yPos)
        yPos += 25
        AddDetailLabel(leftPanel, $"Amount: {deliveryData.TotalAmount:C2}", yPos)
        yPos += 35

        ' Delivery Information
        AddDetailLabel(leftPanel, "Delivery Information", yPos, True)
        yPos += 30
        AddDetailLabel(leftPanel, $"Date: {deliveryData.SaleDate:MMMM dd, yyyy}", yPos)
        yPos += 25
        AddDetailLabel(leftPanel, $"Time: {deliveryData.SaleDate:hh:mm tt}", yPos)
        yPos += 35

        ' Address Section
        AddDetailLabel(leftPanel, "Delivery Address", yPos, True)
        yPos += 30

        Dim addressBox As New TextBox()
        addressBox.Text = deliveryData.DeliveryAddress
        addressBox.Location = New Point(15, yPos)
        addressBox.Size = New Size(390, 80)
        addressBox.Font = New Font("Segoe UI", 10)
        addressBox.Multiline = True
        addressBox.ReadOnly = True
        addressBox.BackColor = Color.FromArgb(250, 250, 250)
        addressBox.ScrollBars = ScrollBars.Vertical
        addressBox.BorderStyle = BorderStyle.FixedSingle
        ' Prevent text selection/highlighting
        addressBox.SelectionStart = 0
        addressBox.SelectionLength = 0
        addressBox.TabStop = False
        ' Add event handler to prevent selection when clicking
        AddHandler addressBox.GotFocus, Sub(s, e)
                                            DirectCast(s, TextBox).SelectionStart = 0
                                            DirectCast(s, TextBox).SelectionLength = 0
                                        End Sub
        AddHandler addressBox.Enter, Sub(s, e)
                                         DirectCast(s, TextBox).SelectionStart = 0
                                         DirectCast(s, TextBox).SelectionLength = 0
                                     End Sub
        leftPanel.Controls.Add(addressBox)
        yPos += 95

        ' Coordinates
        Dim coordsLabel As New Label()
        coordsLabel.Text = $"Coordinates: {deliveryData.Latitude:F6}, {deliveryData.Longitude:F6}"
        coordsLabel.Font = New Font("Segoe UI", 8)
        coordsLabel.ForeColor = Color.Gray
        coordsLabel.AutoSize = True
        coordsLabel.Location = New Point(15, yPos)
        leftPanel.Controls.Add(coordsLabel)
        yPos += 30

        ' Payment Method (if available)
        If Not String.IsNullOrEmpty(deliveryData.PaymentMethod) Then
            AddDetailLabel(leftPanel, "Payment Information", yPos, True)
            yPos += 30
            AddDetailLabel(leftPanel, $"Payment Method: {deliveryData.PaymentMethod}", yPos)
        End If

        ' Right panel for map
        Dim rightPanel As New Panel()
        rightPanel.Location = New Point(450, 50)
        rightPanel.Size = New Size(510, 550)
        rightPanel.BackColor = Color.White
        rightPanel.BorderStyle = BorderStyle.FixedSingle
        mainPanel.Controls.Add(rightPanel)

        ' Map label
        Dim mapLabel As New Label()
        mapLabel.Text = "Delivery Location Map"
        mapLabel.Font = New Font("Segoe UI", 12, FontStyle.Bold)
        mapLabel.ForeColor = Color.FromArgb(79, 51, 40)
        mapLabel.AutoSize = True
        mapLabel.Location = New Point(10, 10)
        rightPanel.Controls.Add(mapLabel)

        ' WebView2 for map
        mapView = New WebView2()
        mapView.Name = "mapView"
        mapView.Location = New Point(10, 40)
        mapView.Size = New Size(490, 500)
        rightPanel.Controls.Add(mapView)

        ' Initialize WebView2 asynchronously
        InitializeMapAsync()

        ' Close button
        Dim closeButton As New Button()
        closeButton.Text = "Close"
        closeButton.Size = New Size(120, 45)
        closeButton.Location = New Point(840, 615)
        closeButton.BackColor = Color.FromArgb(102, 66, 52)
        closeButton.ForeColor = Color.FromArgb(230, 216, 177)
        closeButton.FlatStyle = FlatStyle.Flat
        closeButton.Font = New Font("Segoe UI", 12, FontStyle.Bold)
        AddHandler closeButton.Click, Sub()
                                          Me.Close()
                                      End Sub
        mainPanel.Controls.Add(closeButton)

        ' Print/Export button - NOW FUNCTIONAL
        Dim printButton As New Button()
        printButton.Text = "Print Details"
        printButton.Size = New Size(140, 45)
        printButton.Location = New Point(690, 615)
        printButton.BackColor = Color.FromArgb(147, 53, 53)
        printButton.ForeColor = Color.FromArgb(230, 216, 177)
        printButton.FlatStyle = FlatStyle.Flat
        printButton.Font = New Font("Segoe UI", 12, FontStyle.Bold)
        AddHandler printButton.Click, AddressOf PrintButton_Click
        mainPanel.Controls.Add(printButton)
    End Sub

    Private Sub AddDetailLabel(parent As Panel, text As String, yPos As Integer, Optional isHeader As Boolean = False)
        Dim label As New Label()
        label.Text = text
        label.Location = New Point(15, yPos)
        label.AutoSize = True
        label.MaximumSize = New Size(390, 0)
        label.ForeColor = Color.FromArgb(79, 51, 40)

        If isHeader Then
            label.Font = New Font("Segoe UI", 11, FontStyle.Bold)
        Else
            label.Font = New Font("Segoe UI", 10)
        End If

        parent.Controls.Add(label)
    End Sub

    Private Async Sub InitializeMapAsync()
        Try
            Await mapView.EnsureCoreWebView2Async(Nothing)
            isMapReady = True
            ' Load map with delivery location
            LoadDeliveryMap()
            Debug.WriteLine("WebView2 initialized successfully for delivery details")
        Catch ex As Exception
            Debug.WriteLine($"WebView2 initialization error: {ex.Message}")
            MessageBox.Show("Map preview requires Microsoft Edge WebView2 Runtime." & vbCrLf &
            "The map cannot be displayed, but you can still view the delivery details." & vbCrLf & vbCrLf &
            "To enable maps, install WebView2 from: https://developer.microsoft.com/microsoft-edge/webview2/",
        "Map Unavailable",
               MessageBoxButtons.OK,
            MessageBoxIcon.Information)
            ' Hide map if WebView2 fails
            mapView.Visible = False
        End Try
    End Sub

    Private Sub LoadDeliveryMap()
        If Not isMapReady OrElse mapView Is Nothing OrElse mapView.CoreWebView2 Is Nothing Then
            Debug.WriteLine("Map not ready, skipping LoadDeliveryMap")
            Return
        End If

        Try
            Dim latitude As Double = deliveryData.Latitude
            Dim longitude As Double = deliveryData.Longitude
            Dim addressSafe As String = deliveryData.DeliveryAddress.Replace("'", "\'").Replace(vbCrLf, " ").Replace(vbLf, " ")

            ' Build marker with delivery status color
            Dim markerColor As String = "red"
            Select Case deliveryData.DeliveryStatus
                Case "Pending"
                    markerColor = "red"
                Case "In Transit"
                    markerColor = "orange"
                Case "Delivered"
                    markerColor = "green"
                Case "Cancelled"
                    markerColor = "gray"
            End Select

            Dim htmlContent As String = "<!DOCTYPE html>" & vbCrLf &
         "<html>" & vbCrLf &
         "<head>" & vbCrLf &
         "    <meta charset='utf-8'>" & vbCrLf &
         "    <meta name='viewport' content='width=device-width, initial-scale=1.0'>" & vbCrLf &
         "    <title>Delivery Location</title>" & vbCrLf &
         "    <link rel='stylesheet' href='https://unpkg.com/leaflet@1.9.4/dist/leaflet.css' integrity='sha256-p4NxAoJBhIIN+hmNHrzRCf9tD/miZyoHS5obTRR9BMY=' crossorigin='' />" & vbCrLf &
         "<script src='https://unpkg.com/leaflet@1.9.4/dist/leaflet.js' integrity='sha256-20nQCchB9co0qIjJZRGuk2/Z9VM+kNiyxNV1lvTlZBo=' crossorigin=''></script>" & vbCrLf &
         "    <style>" & vbCrLf &
         "   body { margin: 0; padding: 0; font-family: Arial, sans-serif; }" & vbCrLf &
         "      #map { height: 100vh; width: 100%; }" & vbCrLf &
         "        #loading { position: absolute; top: 50%; left: 50%; transform: translate(-50%, -50%); background: white; padding: 20px; border-radius: 5px; box-shadow: 0 2px 10px rgba(0,0,0,0.2); z-index: 1000; text-align: center; }" & vbCrLf &
         "     .hidden { display: none; }" & vbCrLf &
         "        .delivery-popup { font-family: 'Segoe UI', Arial, sans-serif; max-width: 300px; }" & vbCrLf &
         "        .delivery-popup h3 { margin: 0 0 10px 0; color: #4f3328; }" & vbCrLf &
         ".delivery-popup p { margin: 5px 0; font-size: 13px; }" & vbCrLf &
         "        .delivery-popup .status { font-weight: bold; padding: 3px 8px; border-radius: 3px; display: inline-block; }" & vbCrLf &
         "        .delivery-popup .status-pending { background-color: #ffc8c8; color: #8b0000; }" & vbCrLf &
         "        .delivery-popup .status-intransit { background-color: #ffdcb4; color: #ff8c00; }" & vbCrLf &
         "        .delivery-popup .status-delivered { background-color: #c8ffc8; color: #006400; }" & vbCrLf &
         " .delivery-popup .status-cancelled { background-color: #d3d3d3; color: #808080; }" & vbCrLf &
         "    </style>" & vbCrLf &
         "</head>" & vbCrLf &
         "<body>" & vbCrLf &
         "    <div id='loading'><div style='font-size: 18px; margin-bottom: 10px;'>Loading map...</div><div style='font-size: 12px; color: #666;'>Please wait</div></div>" & vbCrLf &
         "    <div id='map'></div>" & vbCrLf &
         "    <script>" & vbCrLf &
         "        console.log('Starting delivery map initialization...');" & vbCrLf &
         "        setTimeout(function() { var loading = document.getElementById('loading'); if (loading) loading.className = 'hidden'; }, 5000);" & vbCrLf &
         "        try {" & vbCrLf &
         "            var map = L.map('map').setView([" & latitude & ", " & longitude & "], 16);" & vbCrLf &
         "          console.log('Map created successfully');" & vbCrLf &
         "         L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', { maxZoom: 19, attribution: '&copy; OpenStreetMap contributors' }).addTo(map);" & vbCrLf &
         "   console.log('Tiles added successfully');" & vbCrLf &
         "            " & vbCrLf &
         "            // Create custom marker icon based on status" & vbCrLf &
         "            var markerIcon = L.icon({" & vbCrLf &
         "        iconUrl: 'https://raw.githubusercontent.com/pointhi/leaflet-color-markers/master/img/marker-icon-2x-" & markerColor & ".png'," & vbCrLf &
         "     shadowUrl: 'https://cdnjs.cloudflare.com/ajax/libs/leaflet/1.9.4/images/marker-shadow.png'," & vbCrLf &
         "    iconSize: [25, 41]," & vbCrLf &
         "       iconAnchor: [12, 41]," & vbCrLf &
         "        popupAnchor: [1, -34]," & vbCrLf &
         "              shadowSize: [41, 41]" & vbCrLf &
         "         });" & vbCrLf &
         "            " & vbCrLf &
         "       var statusClass = 'status-" & deliveryData.DeliveryStatus.ToLower().Replace(" ", "") & "';" & vbCrLf &
         "            var popupContent = '<div class=""delivery-popup"">' +" & vbCrLf &
         "      '<h3>Delivery Location</h3>' +" & vbCrLf &
         "       '<p><span class=""status ' + statusClass + '"">" & deliveryData.DeliveryStatus & "</span></p>' +" & vbCrLf &
         "          '<p><strong>Product:</strong> " & deliveryData.ProductName.Replace("'", "\'") & "</p>' +" & vbCrLf &
         "     '<p><strong>Quantity:</strong> " & deliveryData.QuantitySold & " units</p>' +" & vbCrLf &
         "   '<p><strong>Address:</strong><br>" & addressSafe & "</p>' +" & vbCrLf &
         "     '<p style=""font-size: 11px; color: #666;"">" & deliveryData.SaleDate.ToString("MMM dd, yyyy hh:mm tt") & "</p>' +" & vbCrLf &
         "      '</div>';" & vbCrLf &
         " " & vbCrLf &
         "          var marker = L.marker([" & latitude & ", " & longitude & "], {icon: markerIcon}).addTo(map);" & vbCrLf &
         "            marker.bindPopup(popupContent, {maxWidth: 350}).openPopup();" & vbCrLf &
         "          " & vbCrLf &
         "// Add circle to show delivery area" & vbCrLf &
         "            var circle = L.circle([" & latitude & ", " & longitude & "], {" & vbCrLf &
         "   color: '" & markerColor & "'," & vbCrLf &
         "           fillColor: '" & markerColor & "'," & vbCrLf &
         "    fillOpacity: 0.1," & vbCrLf &
         "    radius: 100" & vbCrLf &
         "        }).addTo(map);" & vbCrLf &
         "     " & vbCrLf &
         "    map.whenReady(function() { " & vbCrLf &
         "                console.log('Delivery map is ready'); " & vbCrLf &
         "          var loading = document.getElementById('loading'); " & vbCrLf &
         "if (loading) loading.className = 'hidden'; " & vbCrLf &
         "            });" & vbCrLf &
         "        } catch(error) {" & vbCrLf &
         "            console.error('Error initializing delivery map:', error);" & vbCrLf &
         "         var loading = document.getElementById('loading');" & vbCrLf &
         "      if (loading) { loading.innerHTML = '<div style=""color: red;"">Error loading map</div><div style=""font-size: 12px; margin-top: 10px;"">' + error.message + '</div><div style=""font-size: 11px; color: #999; margin-top: 5px;"">Check internet connection</div>'; }" & vbCrLf &
         "        }" & vbCrLf &
         "    </script>" & vbCrLf &
         "</body>" & vbCrLf &
         "</html>"

            mapView.CoreWebView2.NavigateToString(htmlContent)
            Debug.WriteLine($"Delivery map HTML loaded - Latitude: {latitude}, Longitude: {longitude}, Status: {deliveryData.DeliveryStatus}")
        Catch ex As Exception
            Debug.WriteLine("Error loading delivery map: " & ex.Message)
            MessageBox.Show("Error loading map: " & ex.Message & vbCrLf & vbCrLf &
      "Please ensure you have an active internet connection.",
             "Map Load Error",
       MessageBoxButtons.OK,
     MessageBoxIcon.Warning)
        End Try
    End Sub

    ''' Handle print button click - generates and prints delivery details
    Private Sub PrintButton_Click(sender As Object, e As EventArgs)
        Try
            ' Build print content
            printContent = BuildPrintContent()

            ' Show print preview dialog
            Dim printPreviewDialog As New PrintPreviewDialog()
            printPreviewDialog.Document = printDocument
            printPreviewDialog.Width = 800
            printPreviewDialog.Height = 600
            printPreviewDialog.StartPosition = FormStartPosition.CenterParent

            If printPreviewDialog.ShowDialog() = DialogResult.OK Then
                ' User can print from the preview dialog
            End If
        Catch ex As Exception
            MessageBox.Show("Error preparing print: " & ex.Message, "Print Error",
       MessageBoxButtons.OK, MessageBoxIcon.Error)
            Debug.WriteLine($"Print error: {ex.ToString()}")
        End Try
    End Sub

    ' Build formatted text content for printing
    Private Function BuildPrintContent() As String
        Dim content As New System.Text.StringBuilder()

        content.AppendLine("═════════════════════════════════════════════════════════════")
        content.AppendLine("     DELIVERY DETAILS REPORT")
        content.AppendLine("═════════════════════════════════════════════════════════════")
        content.AppendLine()
        content.AppendLine($"Sale #: {deliveryData.SaleID}")
        content.AppendLine($"Print Date: {Date.Now:MMMM dd, yyyy hh:mm tt}")
        content.AppendLine()
        content.AppendLine("─────────────────────────────────────────────────────────────")
        content.AppendLine("  DELIVERY STATUS")
        content.AppendLine("─────────────────────────────────────────────────────────────")
        content.AppendLine()
        content.AppendLine($"Status: {deliveryData.DeliveryStatus}")
        content.AppendLine()
        content.AppendLine("─────────────────────────────────────────────────────────────")
        content.AppendLine("  PRODUCT INFORMATION")
        content.AppendLine("─────────────────────────────────────────────────────────────")
        content.AppendLine()
        content.AppendLine($"Product: {deliveryData.ProductName}")
        content.AppendLine($"Quantity: {deliveryData.QuantitySold} units")
        content.AppendLine($"Amount: {deliveryData.TotalAmount:C2}")
        content.AppendLine()
        content.AppendLine("─────────────────────────────────────────────────────────────")
        content.AppendLine("  DELIVERY INFORMATION")
        content.AppendLine("─────────────────────────────────────────────────────────────")
        content.AppendLine()
        content.AppendLine($"Date: {deliveryData.SaleDate:MMMM dd, yyyy}")
        content.AppendLine($"Time: {deliveryData.SaleDate:hh:mm tt}")
        content.AppendLine()
        content.AppendLine("─────────────────────────────────────────────────────────────")
        content.AppendLine("  DELIVERY ADDRESS")
        content.AppendLine("─────────────────────────────────────────────────────────────")
        content.AppendLine()
        content.AppendLine(deliveryData.DeliveryAddress)
        content.AppendLine()
        content.AppendLine($"Coordinates: {deliveryData.Latitude:F6}, {deliveryData.Longitude:F6}")
        content.AppendLine()

        If Not String.IsNullOrEmpty(deliveryData.PaymentMethod) AndAlso deliveryData.PaymentMethod <> "N/A" Then
            content.AppendLine("─────────────────────────────────────────────────────────────")
            content.AppendLine("PAYMENT INFORMATION")
            content.AppendLine("─────────────────────────────────────────────────────────────")
            content.AppendLine()
            content.AppendLine($"Payment Method: {deliveryData.PaymentMethod}")
            content.AppendLine()
        End If

        content.AppendLine("═════════════════════════════════════════════════════════════")
        content.AppendLine("         End of Delivery Details Report")
        content.AppendLine("═════════════════════════════════════════════════════════════")

        Return content.ToString()
    End Function

    ''' Handle the actual printing - called when PrintDocument.Print() is invoked
    Private Sub PrintDocument_PrintPage(sender As Object, e As PrintPageEventArgs) Handles printDocument.PrintPage
        Try
            ' Set up fonts
            Dim titleFont As New Font("Courier New", 12, FontStyle.Bold)
            Dim contentFont As New Font("Courier New", 10, FontStyle.Regular)

            ' Set up brushes
            Dim blackBrush As New SolidBrush(Color.Black)

            ' Set margins
            Dim leftMargin As Single = e.MarginBounds.Left
            Dim topMargin As Single = e.MarginBounds.Top
            Dim yPosition As Single = topMargin

            ' Line height
            Dim lineHeight As Single = contentFont.GetHeight(e.Graphics)

            ' Split content into lines
            Dim lines() As String = printContent.Split(New String() {vbCrLf, vbLf}, StringSplitOptions.None)

            ' Print each line
            For Each line As String In lines
                ' Check if we need a new page
                If yPosition + lineHeight > e.MarginBounds.Bottom Then
                    e.HasMorePages = True
                    Return
                End If

                ' Use title font for header lines
                Dim currentFont As Font = contentFont
                If line.Contains("DELIVERY DETAILS REPORT") OrElse
                 line.Contains("DELIVERY STATUS") OrElse
                       line.Contains("PRODUCT INFORMATION") OrElse
                      line.Contains("DELIVERY INFORMATION") OrElse
                     line.Contains("DELIVERY ADDRESS") OrElse
                        line.Contains("PAYMENT INFORMATION") Then
                    currentFont = titleFont
                End If

                ' Draw the line
                e.Graphics.DrawString(line, currentFont, blackBrush, leftMargin, yPosition)
                yPosition += lineHeight
            Next

            ' No more pages
            e.HasMorePages = False
        Catch ex As Exception
        End Try
    End Sub
End Class