Imports System.Net.Http
Imports System.Text.Json
Imports Microsoft.Web.WebView2.WinForms

''' Enhanced delivery address form with place search and map preview using WebView2
''' Users search for places, see all matching locations, and confirm the correct one
Public Class DeliveryAddressFormSimple
    Private parentCheckoutForm As CheckoutForm
    Private selectedLatitude As Double = 14.5995 ' Default Manila coordinates
    Private selectedLongitude As Double = 120.9842
    Private selectedAddress As String = ""
    Private mapView As WebView2
    Private isMapReady As Boolean = False
    Private searchResults As New List(Of SearchResult)

    ' Class to hold search results
    Private Class SearchResult
        Public Property DisplayName As String
        Public Property Latitude As Double
        Public Property Longitude As Double
        Public Property PlaceId As String
    End Class

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
        Me.Size = New Size(1200, 750)
        Me.Text = "Select Delivery Location"

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
        titleLabel.Text = "Search Delivery Location"
        titleLabel.Font = New Font("Segoe UI", 16, FontStyle.Bold)
        titleLabel.ForeColor = Color.FromArgb(79, 51, 40)
        titleLabel.AutoSize = True
        titleLabel.Location = New Point(20, 10)
        mainPanel.Controls.Add(titleLabel)

        ' Instructions label
        Dim instructionsLabel As New Label()
        instructionsLabel.Text = "Search for a place (e.g., Taguig City University, SM Manila, Makati City Hall):"
        instructionsLabel.Font = New Font("Segoe UI", 10)
        instructionsLabel.ForeColor = Color.FromArgb(79, 51, 40)
        instructionsLabel.AutoSize = True
        instructionsLabel.Location = New Point(20, 45)
        mainPanel.Controls.Add(instructionsLabel)

        ' Left panel for search
        Dim leftPanel As New Panel()
        leftPanel.Location = New Point(20, 75)
        leftPanel.Size = New Size(540, 550)
        leftPanel.BackColor = Color.White
        leftPanel.BorderStyle = BorderStyle.FixedSingle
        mainPanel.Controls.Add(leftPanel)

        ' Search label
        Dim searchLabelTop As New Label()
        searchLabelTop.Text = "Search for Place:"
        searchLabelTop.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        searchLabelTop.ForeColor = Color.FromArgb(79, 51, 40)
        searchLabelTop.AutoSize = True
        searchLabelTop.Location = New Point(10, 10)
        leftPanel.Controls.Add(searchLabelTop)

        ' Search textbox
        Dim searchTextBox As New TextBox()
        searchTextBox.Name = "searchTextBox"
        searchTextBox.Size = New Size(520, 35)
        searchTextBox.Location = New Point(10, 40)
        searchTextBox.Font = New Font("Segoe UI", 12)
        searchTextBox.PlaceholderText = "e.g., Taguig City University"
        ' Make Enter key trigger search button
        AddHandler searchTextBox.KeyDown, Sub(s, ke)
                                              If ke.KeyCode = Keys.Enter Then
                                                  ke.SuppressKeyPress = True ' Prevent the beep sound
                                                  ke.Handled = True ' Mark as handled
                                                  ' Find and click the search button
                                                  Dim searchBtn As Button = Me.Controls.Find("searchButton", True).FirstOrDefault()
                                                  If searchBtn IsNot Nothing AndAlso searchBtn.Enabled Then
                                                      searchBtn.PerformClick()
                                                  End If
                                              End If
                                          End Sub
        leftPanel.Controls.Add(searchTextBox)

        ' Search button
        Dim searchButton As New Button()
        searchButton.Text = "Search Location"
        searchButton.Size = New Size(520, 45)
        searchButton.Location = New Point(10, 85)
        searchButton.BackColor = Color.FromArgb(102, 66, 52)
        searchButton.ForeColor = Color.FromArgb(230, 216, 177)
        searchButton.FlatStyle = FlatStyle.Flat
        searchButton.Font = New Font("Segoe UI", 11, FontStyle.Bold)
        searchButton.Name = "searchButton"
        AddHandler searchButton.Click, AddressOf SearchLocation_Click
        leftPanel.Controls.Add(searchButton)

        ' Results label
        Dim resultsLabel As New Label()
        resultsLabel.Name = "resultsLabel"
        resultsLabel.Text = "Search Results:"
        resultsLabel.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        resultsLabel.ForeColor = Color.FromArgb(79, 51, 40)
        resultsLabel.AutoSize = True
        resultsLabel.Location = New Point(10, 145)
        leftPanel.Controls.Add(resultsLabel)

        ' Results ListBox
        Dim resultsListBox As New ListBox()
        resultsListBox.Name = "resultsListBox"
        resultsListBox.Size = New Size(520, 200)
        resultsListBox.Location = New Point(10, 175)
        resultsListBox.Font = New Font("Segoe UI", 10)
        resultsListBox.BackColor = Color.FromArgb(250, 250, 250)
        resultsListBox.ForeColor = Color.FromArgb(79, 51, 40)
        resultsListBox.ItemHeight = 35 ' Increase item height for better spacing
        resultsListBox.DrawMode = DrawMode.OwnerDrawFixed ' Enable custom drawing

        ' Add custom drawing for better visual distinction
        AddHandler resultsListBox.DrawItem, Sub(sender As Object, e As DrawItemEventArgs)
                                                If e.Index < 0 Then Return

                                                e.DrawBackground()

                                                ' Alternate row colors for better distinction
                                                Dim bgColor As Color
                                                If (e.State And DrawItemState.Selected) = DrawItemState.Selected Then
                                                    ' Selected item - darker brown
                                                    bgColor = Color.FromArgb(147, 53, 53)
                                                    e.Graphics.FillRectangle(New SolidBrush(bgColor), e.Bounds)
                                                ElseIf e.Index Mod 2 = 0 Then
                                                    ' Even rows - light background
                                                    bgColor = Color.FromArgb(255, 255, 255)
                                                    e.Graphics.FillRectangle(New SolidBrush(bgColor), e.Bounds)
                                                Else
                                                    ' Odd rows - slightly darker background
                                                    bgColor = Color.FromArgb(240, 240, 240)
                                                    e.Graphics.FillRectangle(New SolidBrush(bgColor), e.Bounds)
                                                End If

                                                ' Draw text with proper color
                                                Dim textColor As Color
                                                If (e.State And DrawItemState.Selected) = DrawItemState.Selected Then
                                                    textColor = Color.FromArgb(230, 216, 177) ' Light cream for selected
                                                Else
                                                    textColor = Color.FromArgb(79, 51, 40) ' Dark brown for normal
                                                End If

                                                ' Draw the item text with padding
                                                Dim itemText As String = DirectCast(sender, ListBox).Items(e.Index).ToString()
                                                Dim textBounds As New Rectangle(e.Bounds.X + 10, e.Bounds.Y + 5, e.Bounds.Width - 20, e.Bounds.Height - 10)

                                                Using textBrush As New SolidBrush(textColor)
                                                    e.Graphics.DrawString(itemText, e.Font, textBrush, textBounds, StringFormat.GenericDefault)
                                                End Using

                                                ' Draw separator line between items
                                                If e.Index < DirectCast(sender, ListBox).Items.Count - 1 Then
                                                    Using linePen As New Pen(Color.FromArgb(200, 200, 200), 1)
                                                        e.Graphics.DrawLine(linePen, e.Bounds.Left + 5, e.Bounds.Bottom - 1, e.Bounds.Right - 5, e.Bounds.Bottom - 1)
                                                    End Using
                                                End If

                                                e.DrawFocusRectangle()
                                            End Sub

        AddHandler resultsListBox.SelectedIndexChanged, AddressOf ResultsListBox_SelectedIndexChanged
        leftPanel.Controls.Add(resultsListBox)

        ' Selected address label
        Dim addressLabel As New Label()
        addressLabel.Text = "Selected Address:"
        addressLabel.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        addressLabel.ForeColor = Color.FromArgb(79, 51, 40)
        addressLabel.AutoSize = True
        addressLabel.Location = New Point(10, 390)
        leftPanel.Controls.Add(addressLabel)

        ' Selected address display
        Dim addressDisplay As New TextBox()
        addressDisplay.Name = "addressDisplay"
        addressDisplay.Size = New Size(520, 80)
        addressDisplay.Location = New Point(10, 420)
        addressDisplay.Font = New Font("Segoe UI", 9)
        addressDisplay.Multiline = True
        addressDisplay.ReadOnly = True
        addressDisplay.BackColor = Color.White
        addressDisplay.ScrollBars = ScrollBars.Vertical
        leftPanel.Controls.Add(addressDisplay)

        ' Coordinates display
        Dim coordsLabel As New Label()
        coordsLabel.Name = "coordsLabel"
        coordsLabel.Text = String.Format("Coordinates: {0:F6}, {1:F6}", selectedLatitude, selectedLongitude)
        coordsLabel.Font = New Font("Segoe UI", 8)
        coordsLabel.ForeColor = Color.Gray
        coordsLabel.AutoSize = True
        coordsLabel.Location = New Point(10, 510)
        leftPanel.Controls.Add(coordsLabel)

        ' Right panel for map
        Dim rightPanel As New Panel()
        rightPanel.Location = New Point(570, 75)
        rightPanel.Size = New Size(590, 550)
        rightPanel.BackColor = Color.White
        rightPanel.BorderStyle = BorderStyle.FixedSingle
        mainPanel.Controls.Add(rightPanel)

        ' Map label
        Dim mapLabel As New Label()
        mapLabel.Text = "Map Preview"
        mapLabel.Font = New Font("Segoe UI", 12, FontStyle.Bold)
        mapLabel.ForeColor = Color.FromArgb(79, 51, 40)
        mapLabel.AutoSize = True
        mapLabel.Location = New Point(10, 10)
        rightPanel.Controls.Add(mapLabel)

        ' WebView2 for map
        mapView = New WebView2()
        mapView.Name = "mapView"
        mapView.Location = New Point(10, 40)
        mapView.Size = New Size(570, 500)
        rightPanel.Controls.Add(mapView)

        ' Initialize WebView2 asynchronously
        InitializeMapAsync()

        ' Confirm button
        Dim confirmButton As New Button()
        confirmButton.Text = "Confirm Location"
        confirmButton.Size = New Size(180, 45)
        confirmButton.Location = New Point(900, 640)
        confirmButton.BackColor = Color.FromArgb(147, 53, 53)
        confirmButton.ForeColor = Color.FromArgb(230, 216, 177)
        confirmButton.FlatStyle = FlatStyle.Flat
        confirmButton.Font = New Font("Segoe UI", 12, FontStyle.Bold)
        confirmButton.Name = "confirmButton"
        confirmButton.Enabled = False
        AddHandler confirmButton.Click, AddressOf ConfirmButton_Click
        mainPanel.Controls.Add(confirmButton)

        ' Cancel button
        Dim cancelButton As New Button()
        cancelButton.Text = "Cancel"
        cancelButton.Size = New Size(120, 45)
        cancelButton.Location = New Point(760, 640)
        cancelButton.BackColor = Color.FromArgb(102, 66, 52)
        cancelButton.ForeColor = Color.FromArgb(230, 216, 177)
        cancelButton.FlatStyle = FlatStyle.Flat
        cancelButton.Font = New Font("Segoe UI", 12, FontStyle.Bold)
        AddHandler cancelButton.Click, Sub()
                                           Me.DialogResult = DialogResult.Cancel
                                           Me.Close()
                                       End Sub
        mainPanel.Controls.Add(cancelButton)
    End Sub

    Private Async Sub InitializeMapAsync()
        Try
            Await mapView.EnsureCoreWebView2Async(Nothing)
            isMapReady = True
            ' Load initial map after WebView2 is ready
            LoadMap(selectedLatitude, selectedLongitude, New List(Of SearchResult)())
            Debug.WriteLine("WebView2 initialized successfully")
        Catch ex As Exception
            Debug.WriteLine($"WebView2 initialization error: {ex.Message}")
            MessageBox.Show("Map preview requires Microsoft Edge WebView2 Runtime." & vbCrLf &
"To use this feature, please install or update WebView2." & vbCrLf & vbCrLf &
"Download WebView2 from: https://developer.microsoft.com/microsoft-edge/webview2/",
  "Map Preview Unavailable",
MessageBoxButtons.OK,
    MessageBoxIcon.Information)
            ' Hide map panel if WebView2 fails
            mapView.Visible = False
        End Try
    End Sub

    Private Sub LoadMap(latitude As Double, longitude As Double, results As List(Of SearchResult))
        If Not isMapReady OrElse mapView Is Nothing OrElse mapView.CoreWebView2 Is Nothing Then
            Debug.WriteLine("Map not ready, skipping LoadMap")
            Return
        End If

        Try
            ' Build markers JavaScript
            Dim markersJS As String = ""
            Dim zoomLevel As Integer = 13 ' Default zoom

            If results.Count > 0 Then
                If results.Count = 1 Then
                    ' Single marker - zoom in closer
                    zoomLevel = 16
                    Dim safeName = results(0).DisplayName.Replace("'", "\'").Replace(vbCrLf, " ").Replace(vbLf, " ")
                    markersJS = String.Format("var marker = L.marker([{0}, {1}]).addTo(map).bindPopup('<b>Selected Location</b><br>{2}').openPopup();",
           results(0).Latitude, results(0).Longitude, safeName)
                Else
                    ' Multiple markers - show all
                    For i As Integer = 0 To results.Count - 1
                        Dim result = results(i)
                        Dim safeName = result.DisplayName.Replace("'", "\'").Replace(vbCrLf, " ").Replace(vbLf, " ")
                        markersJS &= String.Format("var marker{0} = L.marker([{1}, {2}]).addTo(map).bindPopup('<b>Option {3}</b><br>{4}');{5}",
   i, result.Latitude, result.Longitude, i + 1, safeName, vbCrLf)
                    Next

                    ' Fit bounds to show all markers
                    Dim boundsPoints As New List(Of String)
                    For Each r In results
                        boundsPoints.Add(String.Format("[{0}, {1}]", r.Latitude, r.Longitude))
                    Next
                    Dim bounds = String.Join(", ", boundsPoints)
                    markersJS &= String.Format("var bounds = L.latLngBounds([{0}]); map.fitBounds(bounds, {{padding: [50, 50]}});{1}", bounds, vbCrLf)
                End If
            Else
                ' Single default marker
                markersJS = String.Format("var marker = L.marker([{0}, {1}]).addTo(map).bindPopup('<b>Default Location</b><br>Search for your delivery location').openPopup();",
        latitude, longitude)
            End If

            Dim htmlContent As String = "<!DOCTYPE html>" & vbCrLf &
"<html>" & vbCrLf &
   "<head>" & vbCrLf &
   "    <meta charset='utf-8'>" & vbCrLf &
"    <meta name='viewport' content='width=device-width, initial-scale=1.0'>" & vbCrLf &
   "    <title>Delivery Map</title>" & vbCrLf &
   "    <link rel='stylesheet' href='https://unpkg.com/leaflet@1.9.4/dist/leaflet.css' integrity='sha256-p4NxAoJBhIIN+hmNHrzRCf9tD/miZyoHS5obTRR9BMY=' crossorigin='' />" & vbCrLf &
   "    <script src='https://unpkg.com/leaflet@1.9.4/dist/leaflet.js' integrity='sha256-20nQCchB9co0qIjJZRGuk2/Z9VM+kNiyxNV1lvTlZBo=' crossorigin=''></script>" & vbCrLf &
   "    <style>" & vbCrLf &
   "        body { margin: 0; padding: 0; font-family: Arial, sans-serif; }" & vbCrLf &
   "        #map { height: 100vh; width: 100%; }" & vbCrLf &
   "        #loading { position: absolute; top: 50%; left: 50%; transform: translate(-50%, -50%); background: white; padding: 20px; border-radius: 5px; box-shadow: 0 2px 10px rgba(0,0,0,0.2); z-index: 1000; text-align: center; }" & vbCrLf &
   "        .hidden { display: none; }" & vbCrLf &
   "</style>" & vbCrLf &
   "</head>" & vbCrLf &
   "<body>" & vbCrLf &
   "    <div id='loading'><div style='font-size: 18px; margin-bottom: 10px;'>Loading map...</div><div style='font-size: 12px; color: #666;'>Please wait</div></div>" & vbCrLf &
   "    <div id='map'></div>" & vbCrLf &
   "    <script>" & vbCrLf &
   "        console.log('Starting map initialization...');" & vbCrLf &
   "   setTimeout(function() { var loading = document.getElementById('loading'); if (loading) loading.className = 'hidden'; }, 5000);" & vbCrLf &
   "        try {" & vbCrLf &
"            var map = L.map('map').setView([" & latitude & ", " & longitude & "], " & zoomLevel & ");" & vbCrLf &
   "            console.log('Map created successfully');" & vbCrLf &
   "            L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', { maxZoom: 19, attribution: '&copy; OpenStreetMap contributors' }).addTo(map);" & vbCrLf &
   "      console.log('Tiles added successfully');" & vbCrLf &
   "     " & markersJS & vbCrLf &
   "            map.whenReady(function() { console.log('Map is ready'); var loading = document.getElementById('loading'); if (loading) loading.className = 'hidden'; });" & vbCrLf &
   "    } catch(error) {" & vbCrLf &
   "    console.error('Error initializing map:', error);" & vbCrLf &
   "          var loading = document.getElementById('loading');" & vbCrLf &
   "  if (loading) { loading.innerHTML = '<div style=""color: red;"">Error loading map</div><div style=""font-size: 12px; margin-top: 10px;"">' + error.message + '</div><div style=""font-size: 11px; color: #999; margin-top: 5px;"">Check internet connection</div>'; }" & vbCrLf &
   "}" & vbCrLf &
   "    </script>" & vbCrLf &
   "</body>" & vbCrLf &
   "</html>"

            mapView.CoreWebView2.NavigateToString(htmlContent)
            Debug.WriteLine($"Map HTML loaded successfully - Latitude: {latitude}, Longitude: {longitude}, Results: {results.Count}, Zoom: {zoomLevel}")
        Catch ex As Exception
            Debug.WriteLine("Error loading map: " & ex.Message)
            MessageBox.Show("Error loading map: " & ex.Message & vbCrLf & vbCrLf &
   "Please ensure you have an active internet connection.",
 "Map Load Error",
       MessageBoxButtons.OK,
   MessageBoxIcon.Warning)
        End Try
    End Sub

    Private Sub MapBrowser_DocumentCompleted(sender As Object, e As WebBrowserDocumentCompletedEventArgs)
        ' Deprecated - not used with WebView2
    End Sub

    Private Async Sub SearchLocation_Click(sender As Object, e As EventArgs)
        Dim searchTextBox As TextBox = Me.Controls.Find("searchTextBox", True).FirstOrDefault()
        Dim resultsListBox As ListBox = Me.Controls.Find("resultsListBox", True).FirstOrDefault()
        Dim searchButton As Button = Me.Controls.Find("searchButton", True).FirstOrDefault()

        If searchTextBox Is Nothing OrElse String.IsNullOrWhiteSpace(searchTextBox.Text) Then
            MessageBox.Show("Please enter a place name to search.", "No Search Term", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        ' Disable button during search
        If searchButton IsNot Nothing Then
            searchButton.Enabled = False
            searchButton.Text = "Searching..."
        End If

        Try
            searchResults.Clear()
            If resultsListBox IsNot Nothing Then
                resultsListBox.Items.Clear()
            End If

            Using client As New HttpClient()
                client.DefaultRequestHeaders.Add("User-Agent", "InventorySystem/1.0")

                ' Search in Philippines by default
                Dim searchQuery As String = searchTextBox.Text.Trim() & ", Philippines"
                Dim url As String = String.Format("https://nominatim.openstreetmap.org/search?format=json&q={0}&limit=10&countrycodes=ph",
  Uri.EscapeDataString(searchQuery))

                Dim response = Await client.GetStringAsync(url)
                Debug.WriteLine("Search response: " & response)

                ' Parse JSON response
                Using doc = JsonDocument.Parse(response)
                    Dim root = doc.RootElement

                    If root.GetArrayLength() = 0 Then
                        MessageBox.Show("No locations found. Try a different search term.", "No Results", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Return
                    End If

                    For Each item In root.EnumerateArray()
                        ' Handle lat/lon as either string or number (API returns both formats)
                        Dim lat As Double
                        Dim lon As Double

                        ' Try to get lat as double first, if that fails try as string
                        Dim latProp = item.GetProperty("lat")
                        If latProp.ValueKind = JsonValueKind.Number Then
                            lat = latProp.GetDouble()
                        Else
                            lat = Double.Parse(latProp.GetString())
                        End If

                        ' Try to get lon as double first, if that fails try as string
                        Dim lonProp = item.GetProperty("lon")
                        If lonProp.ValueKind = JsonValueKind.Number Then
                            lon = lonProp.GetDouble()
                        Else
                            lon = Double.Parse(lonProp.GetString())
                        End If

                        ' Get place_id (can be number or missing)
                        Dim placeId As String = "0"
                        Dim placeIdProp As JsonElement
                        If item.TryGetProperty("place_id", placeIdProp) Then
                            If placeIdProp.ValueKind = JsonValueKind.Number Then
                                placeId = placeIdProp.GetInt64().ToString()
                            Else
                                placeId = placeIdProp.GetString()
                            End If
                        End If

                        Dim result As New SearchResult() With {
                            .DisplayName = item.GetProperty("display_name").GetString(),
                            .Latitude = lat,
                            .Longitude = lon,
                            .PlaceId = placeId
                        }
                        searchResults.Add(result)
                        If resultsListBox IsNot Nothing Then
                            ' Format with number and truncate very long addresses
                            Dim displayText As String = result.DisplayName
                            If displayText.Length > 80 Then
                                displayText = displayText.Substring(0, 77) & "..."
                            End If
                            resultsListBox.Items.Add($"{searchResults.Count}. {displayText}")
                        End If
                    Next

                    ' Update results label with count
                    Dim resultsLabel As Label = Me.Controls.Find("resultsLabel", True).FirstOrDefault()
                    If resultsLabel IsNot Nothing Then
                        resultsLabel.Text = $"Search Results ({searchResults.Count} found):"
                    End If

                    ' Show all results on map
                    LoadMap(searchResults(0).Latitude, searchResults(0).Longitude, searchResults)

                    MessageBox.Show(String.Format("Found {0} location(s). Click on a result to select it.", searchResults.Count),
      "Search Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error searching for location: " & ex.Message, "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Debug.WriteLine("Search error: " & ex.ToString())
        Finally
            ' Re-enable button
            If searchButton IsNot Nothing Then
                searchButton.Enabled = True
                searchButton.Text = "Search Location"
            End If
        End Try
    End Sub

    Private Sub ResultsListBox_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim resultsListBox As ListBox = DirectCast(sender, ListBox)
        If resultsListBox.SelectedIndex < 0 OrElse resultsListBox.SelectedIndex >= searchResults.Count Then
            Return
        End If

        Dim selectedResult = searchResults(resultsListBox.SelectedIndex)

        ' Update selected coordinates
        selectedLatitude = selectedResult.Latitude
        selectedLongitude = selectedResult.Longitude
        selectedAddress = selectedResult.DisplayName

        ' Update UI
        Dim addressDisplay As TextBox = Me.Controls.Find("addressDisplay", True).FirstOrDefault()
        If addressDisplay IsNot Nothing Then
            addressDisplay.Text = selectedAddress
        End If

        Dim coordsLabel As Label = Me.Controls.Find("coordsLabel", True).FirstOrDefault()
        If coordsLabel IsNot Nothing Then
            coordsLabel.Text = String.Format("Coordinates: {0:F6}, {1:F6}", selectedLatitude, selectedLongitude)
        End If

        ' Enable confirm button
        Dim confirmButton As Button = Me.Controls.Find("confirmButton", True).FirstOrDefault()
        If confirmButton IsNot Nothing Then
            confirmButton.Enabled = True
        End If

        ' Center map on selected location with a single marker
        Dim singleResult As New List(Of SearchResult) From {selectedResult}
        LoadMap(selectedLatitude, selectedLongitude, singleResult)

        Debug.WriteLine(String.Format("Selected: {0} at {1}, {2}", selectedAddress, selectedLatitude, selectedLongitude))
    End Sub

    Private Sub ConfirmButton_Click(sender As Object, e As EventArgs)
        If String.IsNullOrWhiteSpace(selectedAddress) Then
            MessageBox.Show("Please select a location from the search results.", "No Location Selected",
     MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Show confirmation prompt with address details
        Dim confirmMessage As String = "Please confirm your delivery address:" & vbCrLf & vbCrLf &
      selectedAddress & vbCrLf & vbCrLf &
  String.Format("Coordinates: {0:F6}, {1:F6}", selectedLatitude, selectedLongitude) & vbCrLf & vbCrLf &
        "Is this the correct delivery location?"

        Dim result As DialogResult = MessageBox.Show(confirmMessage,
            "Confirm Delivery Address",
MessageBoxButtons.YesNo,
            MessageBoxIcon.Question)

        If result = DialogResult.No Then
            ' User wants to select a different address
            MessageBox.Show("Please select a different location from the search results.",
    "Address Not Confirmed",
           MessageBoxButtons.OK,
          MessageBoxIcon.Information)
            Return
        End If

        Debug.WriteLine("=== CONFIRM BUTTON CLICKED (Simple Form) ===")
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