Imports System.Drawing.Drawing2D
Imports Microsoft.Data.SqlClient


Public Class wholesaleDashboard
    Dim topPanel As New topPanelControl()
    Friend WithEvents sidePanel As sidePanelControl
    Dim colorUnclicked As Color = Color.FromArgb(191, 181, 147)
    Dim colorClicked As Color = Color.FromArgb(102, 66, 52)

    ' Chart data storage
    Private chartData As New List(Of KeyValuePair(Of Date, Decimal))()
    Private currentMonth As Date = Date.Today ' Track which month we're viewing
    Private chartPanel As Panel ' Reference to the chart panel for mouse events
    Private tooltipLabel As Label ' Tooltip for hover display

    Public Sub New()
        InitializeComponent()
        sidePanel = New sidePanelControl()
        sidePanel.Dock = DockStyle.Left
        topPanel.Dock = DockStyle.Top
        Me.Controls.Add(topPanel)
        Me.Controls.Add(sidePanel)
        Me.MaximizeBox = False
        Me.FormBorderStyle = FormBorderStyle.None
        Me.BackColor = Color.FromArgb(224, 166, 109)

        Panel1.BackColor = Color.FromArgb(230, 216, 177)
        Panel3.BackColor = Color.FromArgb(230, 216, 177)
        Panel2.BackColor = Color.FromArgb(147, 53, 53)
        Panel2.ForeColor = Color.FromArgb(230, 216, 177)
        Panel4.BackColor = Color.FromArgb(230, 216, 177)
        Panel5.BackColor = Color.FromArgb(230, 216, 177)
        Panel6.BackColor = Color.FromArgb(230, 216, 177)
        Panel7.BackColor = Color.FromArgb(230, 216, 177)

        Label1.ForeColor = Color.FromArgb(79, 51, 40)
        Label3.ForeColor = Color.FromArgb(79, 51, 40)
        Label2.ForeColor = Color.FromArgb(230, 216, 177)
        Label4.ForeColor = Color.FromArgb(79, 51, 40)
        Label5.ForeColor = Color.FromArgb(79, 51, 40)
        Label6.ForeColor = Color.FromArgb(79, 51, 40)

        ' Enable double buffering for smooth chart drawing
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or ControlStyles.DoubleBuffer, True)
    End Sub

    Protected Overrides Sub WndProc(ByRef m As Message)
        Const WM_SYSCOMMAND As Integer = &H112
        Const SC_RESTORE As Integer = &HF120
        Const SC_MOVE As Integer = &HF010

        If m.Msg = WM_SYSCOMMAND Then
            Dim command As Integer = (m.WParam.ToInt32() And &HFFF0)

            ' Block restore
            If command = SC_RESTORE Then
                Return
            End If

            ' Block moving
            If command = SC_MOVE Then
                Return
            End If
        End If

        MyBase.WndProc(m)
    End Sub

    Private Function ShowSingleForm(Of T As {Form, New})() As Form
        Dim formToShow As Form = Nothing

        For Each frm As Form In Application.OpenForms.Cast(Of Form).ToList()
            If TypeOf frm Is T Then
                formToShow = frm
            ElseIf frm IsNot Me Then
                frm.Hide()
            End If
        Next

        If formToShow Is Nothing Then
            formToShow = New T()
            AddHandler formToShow.FormClosed, AddressOf ChildFormClosed
        End If

        formToShow.Show()
        formToShow.BringToFront()

        ' Hide this form if switching to another
        If formToShow IsNot Me Then
            Me.Hide()
        End If

        Return formToShow ' return the form instance
    End Function

    Private Sub ChildFormClosed(sender As Object, e As FormClosedEventArgs)

    End Sub

    'Private Sub WholesaleDashboard_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
    '    Application.Exit()
    'End Sub

    Private Sub HighlightButton(buttonName As String)
        ' Reset all buttons
        For Each ctrl As Control In sidePanel.Controls
            If TypeOf ctrl Is Button Then
                ctrl.BackColor = colorClicked
                ctrl.ForeColor = colorUnclicked
            End If
        Next

        ' Highlight the selected one
        Dim btn As Button = sidePanel.Controls.OfType(Of Button)().FirstOrDefault(Function(b) b.Name = buttonName)
        If btn IsNot Nothing Then
            btn.BackColor = colorUnclicked
            btn.ForeColor = Color.FromArgb(79, 51, 40)
        End If
    End Sub

    Private Sub SidePanel_ButtonClicked(sender As Object, btnName As String) Handles sidePanel.ButtonClicked
        Select Case btnName
            Case "Button1"
                ShowSingleForm(Of wholesaleDashboard)()
            Case "Button2"
                Dim form = ShowSingleForm(Of InventoryForm)()
                DirectCast(form, InventoryForm).LoadProducts()
            Case "Button3"
                Dim form = ShowSingleForm(Of categoriesForm)()
                DirectCast(form, categoriesForm).loadCategories()
            Case "Button4"
                ShowSingleForm(Of deliveryLogsForm)()
            Case "Button5"
                Dim form = ShowSingleForm(Of salesReport)()
                DirectCast(form, salesReport).loadSalesReport()
            Case "Button6"
                Dim form = ShowSingleForm(Of loginRecordsForm)()
                DirectCast(form, loginRecordsForm).LoadLoginHistory()
            Case "Button7"
                Dim form = ShowSingleForm(Of userManagementForm)()
                DirectCast(form, userManagementForm).LoadUsers()
        End Select
    End Sub

    Private Sub WholesaleDashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Initialize current month to this month
        currentMonth = New Date(Date.Today.Year, Date.Today.Month, 1)
        LoadSalesData()
        HighlightButton("Button1")


        SetRoundedRegion2(Panel1, 30)
        SetRoundedRegion2(Panel2, 30)
        SetRoundedRegion2(Panel3, 30)
        SetRoundedRegion2(Panel4, 30)
        SetRoundedRegion2(Panel5, 30)
        SetRoundedRegion2(Panel6, 30)
        SetRoundedRegion2(Panel7, 30)
    End Sub

    Private Sub SetRoundedRegion2(ctrl As Control, radius As Integer)
        Dim rect As New Rectangle(0, 0, ctrl.Width, ctrl.Height)
        Using path As GraphicsPath = GetRoundedRectanglePath2(rect, radius)
            ctrl.Region = New Region(path)
        End Using
    End Sub

    Private Function GetRoundedRectanglePath2(rect As Rectangle, radius As Integer) As GraphicsPath
        Dim path As New GraphicsPath()
        Dim diameter As Integer = radius * 2

        path.StartFigure()

        ' Top edge
        path.AddLine(rect.X + radius, rect.Y, rect.Right - radius, rect.Y)
        ' Top-right corner
        path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90)
        ' Right edge
        path.AddLine(rect.Right, rect.Y + radius, rect.Right, rect.Bottom - radius)
        ' Bottom-right corner
        path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90)
        ' Bottom edge
        path.AddLine(rect.Right - radius, rect.Bottom, rect.X + radius, rect.Bottom)
        ' Bottom-left corner
        path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90)
        ' Left edge
        path.AddLine(rect.X, rect.Bottom - radius, rect.X, rect.Y + radius)
        ' Top-left corner
        path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90)

        path.CloseFigure()
        Return path
    End Function

    Private Function GetConnectionString() As String
        Return "Server=DESKTOP-3AKTMEV;Database=inventorySystem;User Id=sa;Password=24@Hakaaii07;TrustServerCertificate=True;"
    End Function

    Private Sub LoadSalesData()
        ' Load sales data for the current month being viewed
        Dim connStr As String = GetConnectionString()

        ' Get first and last day of the current month
        Dim firstDayOfMonth As Date = New Date(currentMonth.Year, currentMonth.Month, 1)
        Dim lastDayOfMonth As Date = firstDayOfMonth.AddMonths(1).AddDays(-1)

        Dim sql As String = "
        SELECT
            CAST(SaleDate AS date) AS SaleDate,
            ISNULL(SUM(TotalAmount), 0) AS TotalSales
        FROM SalesReport
        WHERE SaleDate >= @StartDate AND SaleDate <= @EndDate
        GROUP BY CAST(SaleDate AS date)
        ORDER BY SaleDate ASC;
    "

        chartData.Clear()

        Try
            Using conn As New SqlConnection(connStr)
                Using cmd As New SqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@StartDate", firstDayOfMonth)
                    cmd.Parameters.AddWithValue("@EndDate", lastDayOfMonth)

                    conn.Open()
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            Dim saleDate As Date = reader.GetDateTime("SaleDate")
                            Dim totalSales As Decimal = reader.GetDecimal("TotalSales")
                            chartData.Add(New KeyValuePair(Of Date, Decimal)(saleDate, totalSales))
                        End While
                    End Using
                End Using
            End Using

            ' Fill in missing days with zero sales for the entire month
            FillMissingDaysForMonth()

            ' Create and setup the chart panel
            SetupChartPanel()

        Catch ex As Exception
            ' Handle error - show empty chart
            chartData.Clear()
            SetupChartPanel()
        End Try
    End Sub

    Private Sub FillMissingDaysForMonth()
        ' Ensure we have data for every day of the current month, filling gaps with 0
        Dim firstDayOfMonth As Date = New Date(currentMonth.Year, currentMonth.Month, 1)
        Dim daysInMonth As Integer = Date.DaysInMonth(currentMonth.Year, currentMonth.Month)
        Dim allDays As New List(Of KeyValuePair(Of Date, Decimal))()

        For i As Integer = 0 To daysInMonth - 1
            Dim currentDate As Date = firstDayOfMonth.AddDays(i)
            Dim existingData = chartData.FirstOrDefault(Function(x) x.Key.Date = currentDate.Date)

            If existingData.Key = Date.MinValue Then
                allDays.Add(New KeyValuePair(Of Date, Decimal)(currentDate, 0))
            Else
                allDays.Add(existingData)
            End If
        Next

        chartData = allDays
    End Sub

    Private Sub SetupChartPanel()
        ' Clear existing controls and set up the chart panel
        Panel1.Controls.Clear()

        ' Create navigation panel at the top
        Dim navPanel As New Panel()
        navPanel.Height = 50  ' Keep height for both navigation and title
        navPanel.Dock = DockStyle.Top
        navPanel.BackColor = Color.FromArgb(230, 216, 177)

        ' Calculate positions for navigation elements and title side by side
        Dim navWidth As Integer = 280 ' Width of navigation elements (80 + 120 + 80)
        Dim titleWidth As Integer = 300 ' Approximate width for title
        Dim totalWidth As Integer = navWidth + titleWidth + 20 ' Total width with spacing
        Dim startX As Integer = (navPanel.Width - totalWidth) \ 2

        ' If panel width is not available yet, use a default centered position
        If navPanel.Width <= 0 Then
            startX = 100 ' Default starting position
        End If

        ' Previous month button
        Dim btnPrevMonth As New Button()
        btnPrevMonth.Text = "◀"
        btnPrevMonth.Size = New Size(80, 30)
        btnPrevMonth.Location = New Point(startX, 10)
        btnPrevMonth.BackColor = Color.FromArgb(147, 53, 53)
        btnPrevMonth.ForeColor = Color.FromArgb(230, 216, 177)
        btnPrevMonth.FlatStyle = FlatStyle.Flat
        btnPrevMonth.Cursor = Cursors.Hand  ' Hand cursor on hover
        AddHandler btnPrevMonth.Click, AddressOf PrevMonth_Click

        ' Current month label
        Dim lblCurrentMonth As New Label()
        lblCurrentMonth.Text = currentMonth.ToString("MMMM yyyy")
        lblCurrentMonth.Font = New Font("Arial", 10, FontStyle.Bold)
        lblCurrentMonth.ForeColor = Color.FromArgb(79, 51, 40)
        lblCurrentMonth.Size = New Size(120, 30)
        lblCurrentMonth.Location = New Point(startX + 90, 10)
        lblCurrentMonth.TextAlign = ContentAlignment.MiddleCenter

        ' Next month button
        Dim btnNextMonth As New Button()
        btnNextMonth.Text = "▶"
        btnNextMonth.Size = New Size(80, 30)
        btnNextMonth.Location = New Point(startX + 200, 10)
        btnNextMonth.BackColor = Color.FromArgb(147, 53, 53)
        btnNextMonth.ForeColor = Color.FromArgb(230, 216, 177)
        btnNextMonth.FlatStyle = FlatStyle.Flat
        btnNextMonth.Cursor = Cursors.Hand  ' Hand cursor on hover
        AddHandler btnNextMonth.Click, AddressOf NextMonth_Click

        ' Chart title positioned to the right of navigation buttons
        Dim lblChartTitle As New Label()
        lblChartTitle.Text = $"Daily Sales Chart - {currentMonth.ToString("MMMM yyyy")}"
        lblChartTitle.Font = New Font("Arial", 12, FontStyle.Bold)  ' Adjusted size to fit alongside
        lblChartTitle.ForeColor = Color.FromArgb(79, 51, 40)
        lblChartTitle.AutoSize = True
        lblChartTitle.Location = New Point(startX + navWidth + 20, 15) ' Positioned to the right with spacing

        ' Disable next month button if current month is this month or future
        If currentMonth >= New Date(Date.Today.Year, Date.Today.Month, 1) Then
            btnNextMonth.Enabled = False
            btnNextMonth.BackColor = Color.Gray
        End If

        ' Add resize handler to recenter elements when panel is resized
        AddHandler navPanel.Resize, Sub()
                                        If navPanel.Width > 0 Then
                                            Dim newTotalWidth As Integer = navWidth + lblChartTitle.Width + 20
                                            Dim newStartX As Integer = (navPanel.Width - newTotalWidth) \ 2
                                            btnPrevMonth.Location = New Point(newStartX, 10)
                                            lblCurrentMonth.Location = New Point(newStartX + 90, 10)
                                            btnNextMonth.Location = New Point(newStartX + 200, 10)
                                            lblChartTitle.Location = New Point(newStartX + navWidth + 20, 15)
                                        End If
                                        lblChartTitle.Text = $"Daily Sales Chart - {currentMonth.ToString("MMMM yyyy")}"
                                    End Sub

        navPanel.Controls.Add(btnPrevMonth)
        navPanel.Controls.Add(lblCurrentMonth)
        navPanel.Controls.Add(btnNextMonth)
        navPanel.Controls.Add(lblChartTitle)

        ' Create a custom panel for drawing the chart
        chartPanel = New Panel()
        chartPanel.Dock = DockStyle.Fill
        chartPanel.BackColor = Color.FromArgb(230, 216, 177)
        AddHandler chartPanel.Paint, AddressOf DrawChart
        AddHandler chartPanel.MouseMove, AddressOf ChartPanel_MouseMove
        AddHandler chartPanel.MouseLeave, AddressOf ChartPanel_MouseLeave

        ' Create tooltip label with larger font
        tooltipLabel = New Label()
        tooltipLabel.AutoSize = True
        tooltipLabel.BackColor = Color.FromArgb(255, 255, 200)
        tooltipLabel.BorderStyle = BorderStyle.FixedSingle
        tooltipLabel.Font = New Font("Arial", 10, FontStyle.Bold)
        tooltipLabel.ForeColor = Color.Black
        tooltipLabel.Visible = False
        chartPanel.Controls.Add(tooltipLabel)

        Panel1.Controls.Add(navPanel)
        Panel1.Controls.Add(chartPanel)
    End Sub

    Private Sub ChartPanel_MouseMove(sender As Object, e As MouseEventArgs)
        If chartData.Count = 0 Then Return

        Dim panel As Panel = DirectCast(sender, Panel)
        Dim margin As Integer = 50
        Dim chartWidth As Integer = panel.Width - (margin * 2)
        Dim chartHeight As Integer = panel.Height - (margin * 2) - 30  ' Back to original dimensions
        Dim chartRect As New Rectangle(margin, margin + 30, chartWidth, chartHeight)  ' Back to original positioning

        ' Check if mouse is within chart area
        If e.X >= chartRect.Left AndAlso e.X <= chartRect.Right AndAlso
           e.Y >= chartRect.Top AndAlso e.Y <= chartRect.Bottom Then

            ' Find the closest data point
            Dim closestIndex As Integer = -1
            Dim minDistance As Double = Double.MaxValue

            For i As Integer = 0 To chartData.Count - 1
                Dim x As Integer = chartRect.Left + (i * chartRect.Width \ (chartData.Count - 1))
                Dim distance As Double = Math.Abs(e.X - x)

                If distance < minDistance Then
                    minDistance = distance
                    closestIndex = i
                End If
            Next

            If closestIndex >= 0 AndAlso minDistance <= 20 Then ' Within 20 pixels
                Dim data = chartData(closestIndex)
                tooltipLabel.Text = $"{data.Key.ToString("MMM dd")}: ₱{data.Value:N2}"
                tooltipLabel.Location = New Point(e.X + 10, e.Y - 30)
                tooltipLabel.Visible = True
                tooltipLabel.BringToFront()
            Else
                tooltipLabel.Visible = False
            End If
        Else
            tooltipLabel.Visible = False
        End If
    End Sub

    Private Sub ChartPanel_MouseLeave(sender As Object, e As EventArgs)
        tooltipLabel.Visible = False
    End Sub

    Private Sub PrevMonth_Click(sender As Object, e As EventArgs)
        currentMonth = currentMonth.AddMonths(-1)
        LoadSalesData()
    End Sub

    Private Sub NextMonth_Click(sender As Object, e As EventArgs)
        ' Only allow going to next month if it's not the current month or future
        If currentMonth < New Date(Date.Today.Year, Date.Today.Month, 1) Then
            currentMonth = currentMonth.AddMonths(1)
            LoadSalesData()
        End If
    End Sub

    Private Sub DrawChart(sender As Object, e As PaintEventArgs)
        If chartData.Count = 0 Then
            DrawNoDataMessage(e.Graphics, DirectCast(sender, Panel))
            Return
        End If

        Dim panel As Panel = DirectCast(sender, Panel)
        Dim g As Graphics = e.Graphics
        g.SmoothingMode = SmoothingMode.AntiAlias

        ' Chart dimensions - back to normal since title is now in the navigation panel
        Dim margin As Integer = 50
        Dim chartWidth As Integer = panel.Width - (margin * 2)
        Dim chartHeight As Integer = panel.Height - (margin * 2) - 30  ' Back to original spacing
        Dim chartRect As New Rectangle(margin, margin + 30, chartWidth, chartHeight)  ' Back to original positioning

        ' Find min and max values for accurate scaling
        Dim maxValue As Decimal = If(chartData.Max(Function(x) x.Value) > 0, chartData.Max(Function(x) x.Value), 100)
        Dim minValue As Decimal = 0

        ' No title drawing here anymore - it's in the navigation panel alongside buttons

        ' Draw axes with original thickness
        Using axisPen As New Pen(Color.FromArgb(79, 51, 40), 3)
            ' Y-axis
            g.DrawLine(axisPen, margin, margin + 30, margin, margin + 30 + chartHeight)
            ' X-axis
            g.DrawLine(axisPen, margin, margin + 30 + chartHeight, margin + chartWidth, margin + 30 + chartHeight)
        End Using

        ' Draw grid lines and labels
        DrawGridAndLabels(g, chartRect, minValue, maxValue)

        ' Draw the line chart with accurate data representation
        If chartData.Count > 1 Then
            DrawLineChart(g, chartRect, minValue, maxValue)
        End If

        ' Draw data points
        DrawDataPoints(g, chartRect, minValue, maxValue)
    End Sub

    Private Sub DrawGridAndLabels(g As Graphics, chartRect As Rectangle, minValue As Decimal, maxValue As Decimal)
        Dim labelFont As New Font("Arial", 12)  ' Increased to 12 for better number visibility
        Dim labelBrush As New SolidBrush(Color.FromArgb(79, 51, 40))
        Dim gridPen As New Pen(Color.FromArgb(200, 200, 200), 1)  ' Reverted back to 1

        ' Draw horizontal grid lines only (removed Y-axis labels)
        For i As Integer = 0 To 5
            Dim y As Integer = chartRect.Bottom - (i * chartRect.Height \ 5)

            ' Grid line only - no labels
            g.DrawLine(gridPen, chartRect.Left, y, chartRect.Right, y)
        Next

        ' Draw vertical grid lines and X-axis labels (show every 5th day to avoid crowding)
        For i As Integer = 0 To chartData.Count - 1
            If i Mod 5 = 0 OrElse i = chartData.Count - 1 Then ' Show every 5th day and the last day
                Dim x As Integer = chartRect.Left + (i * chartRect.Width \ (chartData.Count - 1))

                ' Grid line
                g.DrawLine(gridPen, x, chartRect.Top, x, chartRect.Bottom)

                ' Label
                Dim labelText As String = chartData(i).Key.Day.ToString()
                Dim labelSize = g.MeasureString(labelText, labelFont)
                g.DrawString(labelText, labelFont, labelBrush, x - labelSize.Width / 2, chartRect.Bottom + 5)
            End If
        Next
    End Sub

    Private Sub DrawLineChart(g As Graphics, chartRect As Rectangle, minValue As Decimal, maxValue As Decimal)
        Dim linePen As New Pen(Color.FromArgb(147, 53, 53), 2)
        Dim points As New List(Of Point)()

        ' Calculate points for the line with accurate data representation
        For i As Integer = 0 To chartData.Count - 1
            Dim x As Integer = chartRect.Left + (i * chartRect.Width \ Math.Max(chartData.Count - 1, 1))
            Dim valueRange As Decimal = maxValue - minValue
            Dim normalizedValue As Single = If(valueRange > 0, CSng((chartData(i).Value - minValue) / valueRange), 0)
            Dim y As Integer = chartRect.Bottom - CInt(normalizedValue * chartRect.Height)

            points.Add(New Point(x, y))
        Next

        ' Draw the line connecting all points for accurate representation
        If points.Count > 1 Then
            g.DrawLines(linePen, points.ToArray())
        End If
    End Sub

    Private Sub DrawDataPoints(g As Graphics, chartRect As Rectangle, minValue As Decimal, maxValue As Decimal)
        Dim pointBrush As New SolidBrush(Color.FromArgb(147, 53, 53))
        Dim zeroPointBrush As New SolidBrush(Color.Gray)
        Dim pointSize As Integer = 12

        For i As Integer = 0 To chartData.Count - 1
            Dim x As Integer = chartRect.Left + (i * chartRect.Width \ Math.Max(chartData.Count - 1, 1))
            Dim valueRange As Decimal = maxValue - minValue
            Dim normalizedValue As Single = If(valueRange > 0, CSng((chartData(i).Value - minValue) / valueRange), 0)
            Dim y As Integer = chartRect.Bottom - CInt(normalizedValue * chartRect.Height)

            ' Use different color for zero values
            Dim brush As SolidBrush = If(chartData(i).Value = 0, zeroPointBrush, pointBrush)

            ' Draw data point - much larger and more noticeable
            g.FillEllipse(brush, x - CInt(pointSize / 2), y - CInt(pointSize / 2), pointSize, pointSize)

            ' Add a white border around each dot to make it even more noticeable
            Using borderPen As New Pen(Color.White, 2)
                g.DrawEllipse(borderPen, x - CInt(pointSize / 2), y - CInt(pointSize / 2), pointSize, pointSize)
            End Using

            ' Removed the zero labels - no more "0" text showing on the chart
        Next
    End Sub

    Private Sub DrawNoDataMessage(g As Graphics, panel As Panel)
        Dim messageFont As New Font("Arial", 18, FontStyle.Bold)  ' Increased from 14 to 18
        Dim messageBrush As New SolidBrush(Color.FromArgb(79, 51, 40))
        Dim message As String = $"No Sales Data Available for {currentMonth.ToString("MMMM yyyy")}"
        Dim messageSize = g.MeasureString(message, messageFont)

        g.DrawString(message, messageFont, messageBrush,
                    (panel.Width - messageSize.Width) / 2,
                    (panel.Height - messageSize.Height) / 2)
    End Sub

End Class
