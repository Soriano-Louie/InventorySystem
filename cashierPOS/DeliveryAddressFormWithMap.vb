Imports Microsoft.Web.WebView2.Core
Imports Microsoft.Web.WebView2.WinForms

''' <summary>
''' Enhanced delivery address form with interactive map for location selection
''' Requires Microsoft Edge WebView2 Runtime to be installed
''' </summary>
Public Class DeliveryAddressFormWithMap
    Private parentCheckoutForm As CheckoutForm
    Private selectedLatitude As Double = 14.5995 ' Default Manila coordinates
    Private selectedLongitude As Double = 120.9842
    Private selectedAddress As String = ""
    Private webView As WebView2
    Private isMapReady As Boolean = False

    Public Property DeliveryLatitude As Double
        Get
            Return selectedLatitude
        End Get
        Set(value As Double)
            selectedLatitude = value
        End Set
    End Property

    Public Property DeliveryLongitude As Double
        Get
            Return selectedLongitude
        End Get
        Set(value As Double)
            selectedLongitude = value
        End Set
    End Property

    Public Property DeliveryAddress As String
        Get
            Return selectedAddress
        End Get
        Set(value As String)
            selectedAddress = value
        End Set
    End Property

    Public Sub New(parent As CheckoutForm)
        InitializeComponent()
        Me.parentCheckoutForm = parent
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.StartPosition = FormStartPosition.CenterParent
        Me.BackColor = Color.FromArgb(230, 216, 177)
        Me.Size = New Size(1000, 700)
        Me.Text = "Select Delivery Address"

        InitializeUIAsync()
    End Sub

    Private Async Sub InitializeUIAsync()
        ' Create main panel
        Dim mainPanel As New Panel()
        mainPanel.Dock = DockStyle.Fill
        mainPanel.BackColor = Color.FromArgb(230, 216, 177)
        mainPanel.Padding = New Padding(20)
        Me.Controls.Add(mainPanel)

        ' Title label
        Dim titleLabel As New Label()
        titleLabel.Text = "Select Delivery Location"
        titleLabel.Font = New Font("Segoe UI", 16, FontStyle.Bold)
        titleLabel.ForeColor = Color.FromArgb(79, 51, 40)
        titleLabel.AutoSize = True
        titleLabel.Location = New Point(20, 10)
        mainPanel.Controls.Add(titleLabel)

        ' Instructions label
        Dim instructionsLabel As New Label()
        instructionsLabel.Text = "Click on the map to select delivery location or enter address below:"
        instructionsLabel.Font = New Font("Segoe UI", 10)
        instructionsLabel.ForeColor = Color.FromArgb(79, 51, 40)
        instructionsLabel.AutoSize = True
        instructionsLabel.Location = New Point(20, 45)
        mainPanel.Controls.Add(instructionsLabel)

        ' Map container panel
        Dim mapPanel As New Panel()
        mapPanel.Location = New Point(20, 75)
        mapPanel.Size = New Size(940, 400)
        mapPanel.BackColor = Color.White
        mapPanel.BorderStyle = BorderStyle.FixedSingle
        mainPanel.Controls.Add(mapPanel)

        ' Initialize WebView2
        webView = New WebView2()
        webView.Dock = DockStyle.Fill
        mapPanel.Controls.Add(webView)

        ' Address info panel
        Dim addressPanel As New Panel()
        addressPanel.Location = New Point(20, 485)
        addressPanel.Size = New Size(940, 120)
        mainPanel.Controls.Add(addressPanel)

        ' Address label
        Dim addressLabel As New Label()
        addressLabel.Text = "Selected Address:"
        addressLabel.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        addressLabel.ForeColor = Color.FromArgb(79, 51, 40)
        addressLabel.AutoSize = True
        addressLabel.Location = New Point(0, 5)
        addressPanel.Controls.Add(addressLabel)

        ' Address textbox
        Dim addressTextBox As New TextBox()
        addressTextBox.Name = "addressTextBox"
        addressTextBox.Size = New Size(940, 60)
        addressTextBox.Location = New Point(0, 30)
        addressTextBox.Font = New Font("Segoe UI", 11)
        addressTextBox.Multiline = True
        addressTextBox.ScrollBars = ScrollBars.Vertical
        addressTextBox.ReadOnly = True
        addressTextBox.BackColor = Color.White
        addressPanel.Controls.Add(addressTextBox)

        ' Coordinates label
        Dim coordsLabel As New Label()
        coordsLabel.Name = "coordsLabel"
        coordsLabel.Text = $"Coordinates: {selectedLatitude:F6}, {selectedLongitude:F6}"
        coordsLabel.Font = New Font("Segoe UI", 9)
        coordsLabel.ForeColor = Color.Gray
        coordsLabel.AutoSize = True
        coordsLabel.Location = New Point(0, 95)
        addressPanel.Controls.Add(coordsLabel)

        ' Manual entry button
        Dim manualEntryButton As New Button()
        manualEntryButton.Text = "Enter Address Manually"
        manualEntryButton.Size = New Size(180, 35)
        manualEntryButton.Location = New Point(20, 615)
        manualEntryButton.BackColor = Color.FromArgb(102, 66, 52)
        manualEntryButton.ForeColor = Color.FromArgb(230, 216, 177)
        manualEntryButton.FlatStyle = FlatStyle.Flat
        manualEntryButton.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        AddHandler manualEntryButton.Click, AddressOf ManualEntryButton_Click
        mainPanel.Controls.Add(manualEntryButton)

        ' Confirm button
        Dim confirmButton As New Button()
        confirmButton.Text = "Confirm Location"
        confirmButton.Size = New Size(150, 35)
        confirmButton.Location = New Point(690, 615)
        confirmButton.BackColor = Color.FromArgb(147, 53, 53)
        confirmButton.ForeColor = Color.FromArgb(230, 216, 177)
        confirmButton.FlatStyle = FlatStyle.Flat
        confirmButton.Font = New Font("Segoe UI", 12, FontStyle.Bold)
        confirmButton.Name = "confirmButton"
        AddHandler confirmButton.Click, AddressOf ConfirmButton_Click
        mainPanel.Controls.Add(confirmButton)

        ' Cancel button
        Dim cancelButton As New Button()
        cancelButton.Text = "Cancel"
        cancelButton.Size = New Size(100, 35)
        cancelButton.Location = New Point(850, 615)
        cancelButton.BackColor = Color.FromArgb(102, 66, 52)
        cancelButton.ForeColor = Color.FromArgb(230, 216, 177)
        cancelButton.FlatStyle = FlatStyle.Flat
        cancelButton.Font = New Font("Segoe UI", 12, FontStyle.Bold)
        AddHandler cancelButton.Click, Sub()
                                           Me.DialogResult = DialogResult.Cancel
                                           Me.Close()
                                       End Sub
        mainPanel.Controls.Add(cancelButton)

        ' Initialize WebView2
        Try
            Await webView.EnsureCoreWebView2Async(Nothing)
            AddHandler webView.CoreWebView2.WebMessageReceived, AddressOf WebView_MessageReceived
            LoadMapHTML()
        Catch ex As Exception
            MessageBox.Show("Error initializing map. Please ensure Microsoft Edge WebView2 Runtime is installed." & vbCrLf & vbCrLf &
                  "You can download it from: https://developer.microsoft.com/en-us/microsoft-edge/webview2/" & vbCrLf & vbCrLf &
              "Falling back to manual address entry.",
                  "Map Initialization Error",
           MessageBoxButtons.OK,
                MessageBoxIcon.Warning)
            webView.Visible = False
            ManualEntryButton_Click(Nothing, Nothing)
        End Try
    End Sub

    Private Sub LoadMapHTML()
        Dim htmlContent As String = $"
<!DOCTYPE html>
<html>
<head>
  <meta charset='utf-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Delivery Location Selector</title>
    <link rel='stylesheet' href='https://unpkg.com/leaflet@1.9.4/dist/leaflet.css'
          integrity='sha256-p4NxAoJBhIIN+hmNHrzRCf9tD/miZyoHS5obTRR9BMY='
          crossorigin=''/>
    <script src='https://unpkg.com/leaflet@1.9.4/dist/leaflet.js'
  integrity='sha256-20nQCchB9co0qIjJZRGuk2/Z9VM+kNiyxNV1lvTlZBo='
 crossorigin=''></script>
    <style>
body {{
   margin: 0;
            padding: 0;
       font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
        }}
        #map {{
            height: 100vh;
 width: 100%;
        }}
        .info-box {{
       position: absolute;
      top: 10px;
            right: 10px;
            background: white;
    padding: 10px;
            border-radius: 5px;
            box-shadow: 0 2px 5px rgba(0,0,0,0.3);
            z-index: 1000;
            font-size: 12px;
        }}
    </style>
</head>
<body>
    <div id='map'></div>
    <div class='info-box'>
        <strong>How to use:</strong><br>
        • Click anywhere on map to select location<br>
        • Drag marker to adjust position<br>
        • Search box to find specific address
    </div>

    <script>
   // Initialize map centered on Manila
 var map = L.map('map').setView([{selectedLatitude}, {selectedLongitude}], 13);

        // Add OpenStreetMap tiles
        L.tileLayer('https://{{s}}.tile.openstreetmap.org/{{z}}/{{x}}/{{y}}.png', {{
            maxZoom: 19,
     attribution: '© OpenStreetMap contributors'
        }}).addTo(map);

        // Current marker
   var marker = L.marker([{selectedLatitude}, {selectedLongitude}], {{
            draggable: true
     }}).addTo(map);

        // Update coordinates when marker is dragged
        marker.on('dragend', function(e) {{
         var position = marker.getLatLng();
      updateLocation(position.lat, position.lng);
        }});

        // Update coordinates when map is clicked
        map.on('click', function(e) {{
    marker.setLatLng(e.latlng);
            updateLocation(e.latlng.lat, e.latlng.lng);
        }});

        // Function to update location and get address
        async function updateLocation(lat, lng) {{
  // Reverse geocoding using Nominatim (OpenStreetMap)
   try {{
    const response = await fetch(
     `https://nominatim.openstreetmap.org/reverse?format=json&lat=${{lat}}&lon=${{lng}}&zoom=18&addressdetails=1`
   );
const data = await response.json();

      var address = data.display_name || 'Address not found';

      // Send data to C# application
         window.chrome.webview.postMessage({{
      latitude: lat,
         longitude: lng,
         address: address
       }});

            marker.bindPopup(`<b>Selected Location</b><br>${{address}}`).openPopup();
        }} catch (error) {{
      console.error('Geocoding error:', error);
window.chrome.webview.postMessage({{
     latitude: lat,
       longitude: lng,
   address: `Location: ${{lat.toFixed(6)}}, ${{lng.toFixed(6)}}`
    }});
    }}
     }}

        // Initialize with default location
      updateLocation({selectedLatitude}, {selectedLongitude});
    </script>
</body>
</html>"

        webView.CoreWebView2.NavigateToString(htmlContent)
        isMapReady = True
    End Sub

    Private Sub WebView_MessageReceived(sender As Object, e As CoreWebView2WebMessageReceivedEventArgs)
        Try
            Dim json As String = e.WebMessageAsJson
            ' Parse the JSON message from JavaScript
            ' Format: {"latitude":14.5995,"longitude":120.9842,"address":"..."}

            Dim latMatch = System.Text.RegularExpressions.Regex.Match(json, """latitude"":([-\d.]+)")
            Dim lngMatch = System.Text.RegularExpressions.Regex.Match(json, """longitude"":([-\d.]+)")
            Dim addrMatch = System.Text.RegularExpressions.Regex.Match(json, """address"":""([^""]+)""")

            If latMatch.Success AndAlso lngMatch.Success Then
                selectedLatitude = Double.Parse(latMatch.Groups(1).Value)
                selectedLongitude = Double.Parse(lngMatch.Groups(1).Value)

                If addrMatch.Success Then
                    selectedAddress = addrMatch.Groups(1).Value
                    ' Unescape JSON string
                    selectedAddress = selectedAddress.Replace("\n", vbCrLf)
                End If

                ' Update UI controls
                Dim addressTextBox As TextBox = Me.Controls.Find("addressTextBox", True).FirstOrDefault()
                Dim coordsLabel As Label = Me.Controls.Find("coordsLabel", True).FirstOrDefault()

                If addressTextBox IsNot Nothing Then
                    addressTextBox.Text = selectedAddress
                End If

                If coordsLabel IsNot Nothing Then
                    coordsLabel.Text = $"Coordinates: {selectedLatitude:F6}, {selectedLongitude:F6}"
                End If
            End If
        Catch ex As Exception
            Debug.WriteLine("Error processing map message: " & ex.Message)
        End Try
    End Sub

    Private Sub ManualEntryButton_Click(sender As Object, e As EventArgs)
        ' Show simple address entry form
        Dim simpleForm As New DeliveryAddressFormSimple(parentCheckoutForm)

        If simpleForm.ShowDialog() = DialogResult.OK Then
            selectedAddress = simpleForm.DeliveryAddress
            selectedLatitude = simpleForm.DeliveryLatitude
            selectedLongitude = simpleForm.DeliveryLongitude

            Me.DialogResult = DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub ConfirmButton_Click(sender As Object, e As EventArgs)
        If String.IsNullOrWhiteSpace(selectedAddress) Then
            MessageBox.Show("Please select a location on the map or enter an address manually.",
                    "No Location Selected",
                MessageBoxButtons.OK,
               MessageBoxIcon.Warning)
            Return
        End If

        Debug.WriteLine("=== CONFIRM BUTTON CLICKED (WithMap Form) ===")
        Debug.WriteLine($"BEFORE ASSIGNMENT - selectedAddress: '{selectedAddress}'")
        Debug.WriteLine($"BEFORE ASSIGNMENT - selectedLatitude: {selectedLatitude}")
        Debug.WriteLine($"BEFORE ASSIGNMENT - selectedLongitude: {selectedLongitude}")

        ' CRITICAL FIX: Explicitly assign private fields to public properties before closing
        Me.DeliveryAddress = selectedAddress
        Me.DeliveryLatitude = selectedLatitude
        Me.DeliveryLongitude = selectedLongitude

        Debug.WriteLine($"AFTER ASSIGNMENT - DeliveryAddress property: '{Me.DeliveryAddress}'")
        Debug.WriteLine($"AFTER ASSIGNMENT - DeliveryLatitude property: {Me.DeliveryLatitude}")
        Debug.WriteLine($"AFTER ASSIGNMENT - DeliveryLongitude property: {Me.DeliveryLongitude}")

        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

End Class