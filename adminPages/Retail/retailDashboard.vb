Imports System.Drawing.Drawing2D
Imports Microsoft.Data.SqlClient


Public Class retailDashboard
    Dim topPanel As New topPanelControl()
    Friend WithEvents sidePanel As sidePanelControl2
    Dim colorUnclicked As Color = Color.FromArgb(191, 181, 147)
    Dim colorClicked As Color = Color.FromArgb(102, 66, 52)

    ' Chart data storage for daily sales
    Private chartData As New List(Of KeyValuePair(Of Date, Decimal))()
    Private currentMonth As Date = Date.Today ' Track which month we're viewing
    Private chartPanel As Panel ' Reference to the chart panel for mouse events
    Private tooltipLabel As Label ' Tooltip for hover display

    ' Top Products Chart data storage
    Private topProductsData As New List(Of ProductDailyMetrics)()
    Private topProductsCurrentMonth As Date = Date.Today
    Private topProductsChartPanel As Panel
    Private topProductsTooltipLabel As Label
    Private topProducts As New List(Of TopProductInfo)() ' Store top 10 products info
    Private selectedProductIndex As Integer = 0 ' Track which product line to display

    ' Class to store product metrics per day
    Private Class ProductDailyMetrics
        Public Property SaleDate As Date
        Public Property ProductID As Integer
        Public Property ProductName As String
        Public Property TotalAmount As Decimal
        Public Property QuantitySold As Integer
        Public Property SalesCount As Integer ' Number of times sold that day
    End Class

    ' Class to store top product information
    Private Class TopProductInfo
        Public Property ProductID As Integer
        Public Property ProductName As String
        Public Property TotalRevenue As Decimal
        Public Property TotalQuantity As Integer
        Public Property TotalSalesCount As Integer
        Public Property Color As Color
    End Class

    Public Sub New()
        InitializeComponent()
        sidePanel = New sidePanelControl2()
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

        ' Set label colors for consistent styling - all existing labels
        Label1.ForeColor = Color.FromArgb(230, 216, 177)
        Label2.ForeColor = Color.FromArgb(230, 216, 177)
        Label3.ForeColor = Color.FromArgb(79, 51, 40)
        Label4.ForeColor = Color.FromArgb(79, 51, 40)
        Label5.ForeColor = Color.FromArgb(79, 51, 40)
        Label6.ForeColor = Color.FromArgb(79, 51, 40)
        Label7.ForeColor = Color.FromArgb(79, 51, 40)
        Label8.ForeColor = Color.FromArgb(79, 51, 40)
        Label9.ForeColor = Color.FromArgb(79, 51, 40)
        Label10.ForeColor = Color.FromArgb(79, 51, 40)

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

    Private Function ShowSingleForm(Of T As {Form, New})() As T
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

        Return DirectCast(formToShow, T)
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
                ShowSingleForm(Of retailDashboard)()
            Case "Button2"
                ShowSingleForm(Of inventoryRetail)()
            Case "Button3"
                Dim form = ShowSingleForm(Of categoriesForm)()
                form.loadCategories()
            Case "Button4"

            Case "Button5"
                ShowSingleForm(Of retailSalesReport)()
            Case "Button6"
                Dim form = ShowSingleForm(Of loginRecordsForm)()
                form.LoadLoginHistory()
            Case "Button7"
                Dim form = ShowSingleForm(Of userManagementForm)()
                form.LoadUsers()
        End Select
    End Sub

    Private Sub retailDashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Initialize current month to this month
        currentMonth = New Date(Date.Today.Year, Date.Today.Month, 1)
        topProductsCurrentMonth = New Date(Date.Today.Year, Date.Today.Month, 1)
        LoadSalesData()
        LoadTopProductsData()
        HighlightButton("Button1")

        ' Load dashboard data
        LoadDashboardData()

        ' Example: Make Panel1 have rounded corners with radius 30
        SetRoundedRegion2(Panel1, 30)
        SetRoundedRegion2(Panel2, 30)
        SetRoundedRegion2(Panel3, 30)
        SetRoundedRegion2(Panel4, 30)
        SetRoundedRegion2(Panel5, 30)
        SetRoundedRegion2(Panel6, 30)
        SetRoundedRegion2(Panel7, 30)
    End Sub

    Public Sub LoadDashboardData()
        LoadTotalOrdersTodayForLabel8()
        LoadTotalUsersForLabel1()
        LoadStockLevelStatus()
        LoadTotalProductsForLabel9()
        LoadTotalCategoriesForLabel10()
        LoadTopProductsData()
    End Sub

    Private Sub LoadTotalOrdersTodayForLabel8()
        Try
            Dim connStr As String = GetConnectionString()
            Dim sql As String = "SELECT COUNT(*) FROM RetailSalesReport WHERE CAST(SaleDate AS date) = CAST(GETDATE() AS date)"

            Using conn As New SqlConnection(connStr)
                Using cmd As New SqlCommand(sql, conn)
                    conn.Open()
                    Dim totalOrders As Integer = Convert.ToInt32(cmd.ExecuteScalar())
                    ' Direct assignment to Label8 for total orders today
                    Try
                        Label8.Text = totalOrders.ToString()
                    Catch ex As Exception
                        Console.WriteLine("Label8 not found: " & ex.Message)
                    End Try
                End Using
            End Using
        Catch ex As Exception
            Try
                Label8.Text = "Error"
            Catch
                Console.WriteLine("Error loading total orders for Label8: " & ex.Message)
            End Try
        End Try
    End Sub

    Private Sub LoadTotalUsersForLabel1()
        Try
            Dim connStr As String = GetConnectionString()
            Dim sql As String = "SELECT COUNT(*) FROM Users"

            Using conn As New SqlConnection(connStr)
                Using cmd As New SqlCommand(sql, conn)
                    conn.Open()
                    Dim totalUsers As Integer = Convert.ToInt32(cmd.ExecuteScalar())
                    ' Direct assignment to Label1 for total users
                    Try
                        Label1.Text = totalUsers.ToString()
                    Catch ex As Exception
                        Console.WriteLine("Label1 not found: " & ex.Message)
                    End Try
                End Using
            End Using
        Catch ex As Exception
            Try
                Label1.Text = "Error"
            Catch
                Console.WriteLine("Error loading total users for Label1: " & ex.Message)
            End Try
        End Try
    End Sub

    Private Sub LoadTotalProductsForLabel9()
        Try
            Dim connStr As String = GetConnectionString()
            ' Use wholesaleProducts table as it's the main products table
            Dim sql As String = "SELECT COUNT(*) FROM wholesaleProducts"

            Using conn As New SqlConnection(connStr)
                Using cmd As New SqlCommand(sql, conn)
                    conn.Open()
                    Dim totalProducts As Integer = Convert.ToInt32(cmd.ExecuteScalar())
                    ' Direct assignment to Label9 for total products
                    Try
                        Label9.Text = totalProducts.ToString()
                    Catch ex As Exception
                        Console.WriteLine("Label9 not found: " & ex.Message)
                    End Try
                End Using
            End Using
        Catch ex As Exception
            Try
                Label9.Text = "Error"
            Catch
                Console.WriteLine("Error loading total products for Label9: " & ex.Message)
            End Try
        End Try
    End Sub

    Private Sub LoadTotalCategoriesForLabel10()
        Try
            Dim connStr As String = GetConnectionString()
            Dim sql As String = "SELECT COUNT(*) FROM Categories"

            Using conn As New SqlConnection(connStr)
                Using cmd As New SqlCommand(sql, conn)
                    conn.Open()
                    Dim totalCategories As Integer = Convert.ToInt32(cmd.ExecuteScalar())
                    ' Direct assignment to Label10 for total categories
                    Try
                        Label10.Text = totalCategories.ToString()
                    Catch ex As Exception
                        Console.WriteLine("Label10 not found: " & ex.Message)
                    End Try
                End Using
            End Using
        Catch ex As Exception
            Try
                Label10.Text = "Error"
            Catch
                Console.WriteLine("Error loading total categories for Label10: " & ex.Message)
            End Try
        End Try
    End Sub

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
        FROM RetailSalesReport
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

    Private Sub LoadStockLevelStatus()
        Try
            Dim connStr As String = GetConnectionString()
            ' Use wholesaleProducts table for stock level checking
            Dim sql As String = "SELECT ProductName, StockQuantity, ReorderLevel FROM RetailProducts WHERE StockQuantity <= ReorderLevel"

            Using conn As New SqlConnection(connStr)
                Using cmd As New SqlCommand(sql, conn)
                    conn.Open()
                    Dim criticalProducts As New List(Of String)()

                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            Dim productName As String = reader.GetString("ProductName")
                            criticalProducts.Add(productName)
                        End While
                    End Using

                    ' Clear existing dynamic controls in Panel4 (keep original labels)
                    Dim controlsToRemove As New List(Of Control)()
                    For Each ctrl As Control In Panel4.Controls
                        If TypeOf ctrl Is Label AndAlso (DirectCast(ctrl, Label).Name.StartsWith("lblStatus") OrElse
                                                        DirectCast(ctrl, Label).Name.StartsWith("lblCritical") OrElse
                                                        DirectCast(ctrl, Label).Name.StartsWith("lblError")) Then
                            controlsToRemove.Add(ctrl)
                        ElseIf TypeOf ctrl Is Panel AndAlso DirectCast(ctrl, Panel).Name.StartsWith("containerPanel") Then
                            controlsToRemove.Add(ctrl)
                        End If
                    Next
                    For Each ctrl As Control In controlsToRemove
                        Panel4.Controls.Remove(ctrl)
                    Next

                    If criticalProducts.Count = 0 Then
                        ' All stock levels are normal
                        Dim lblStatus As New Label()
                        lblStatus.Name = "lblStatusNormal"
                        lblStatus.Text = "All Normal Level"
                        lblStatus.Font = New Font("Segoe UI", 18, FontStyle.Bold)
                        lblStatus.ForeColor = Color.Green
                        lblStatus.AutoSize = False
                        lblStatus.Size = New Size(Panel4.Width - 20, 70)  ' Increased from 50 to 70
                        lblStatus.TextAlign = ContentAlignment.MiddleCenter
                        lblStatus.Location = New Point(10, (Panel4.Height - 70) \ 2)  ' Adjusted position for new height
                        Panel4.Controls.Add(lblStatus)
                    Else
                        ' There are critical stock levels - create a container panel for scrolling if needed
                        Dim containerPanel As New Panel()
                        containerPanel.Name = "containerPanelCritical"
                        containerPanel.AutoScroll = True
                        containerPanel.Dock = DockStyle.Fill
                        containerPanel.BackColor = Color.FromArgb(230, 216, 177)
                        Panel4.Controls.Add(containerPanel)

                        ' Calculate appropriate label height based on number of critical products (keep font sizes the same)
                        Dim fontSize As Integer = 18
                        Dim labelHeight As Integer = 50  ' Increased from 35 to 50

                        If criticalProducts.Count > 3 Then
                            fontSize = 12
                            labelHeight = 40  ' Increased from 25 to 40
                        ElseIf criticalProducts.Count > 6 Then
                            fontSize = 10
                            labelHeight = 35  ' Increased from 22 to 35
                        End If

                        Dim yPosition As Integer = 15  ' Increased top margin from 10 to 15

                        For i As Integer = 0 To criticalProducts.Count - 1
                            Dim product As String = criticalProducts(i)
                            Dim lblCritical As New Label()
                            lblCritical.Name = $"lblCritical{i}"
                            lblCritical.Text = $"Critical: {product}"
                            lblCritical.Font = New Font("Segoe UI", fontSize, FontStyle.Bold)
                            lblCritical.ForeColor = Color.Red
                            lblCritical.AutoSize = False
                            lblCritical.Size = New Size(containerPanel.Width - 40, labelHeight)
                            lblCritical.TextAlign = ContentAlignment.MiddleCenter
                            lblCritical.Location = New Point(20, yPosition)

                            ' Handle text wrapping for long product names
                            If product.Length > 20 Then
                                lblCritical.Text = $"Critical: {If(product.Length > 25, product.Substring(0, 22) + "...", product)}"
                            End If

                            containerPanel.Controls.Add(lblCritical)

                            yPosition += labelHeight + 8 ' Increased spacing from 5 to 8 pixels between labels
                        Next

                        ' Ensure container panel can scroll if content exceeds panel height
                        containerPanel.AutoScrollMinSize = New Size(0, yPosition + 15)  ' Increased bottom margin
                    End If
                End Using
            End Using
        Catch ex As Exception
            ' Clear existing dynamic controls in Panel4
            Dim controlsToRemove As New List(Of Control)()
            For Each ctrl As Control In Panel4.Controls
                If TypeOf ctrl Is Label AndAlso (DirectCast(ctrl, Label).Name.StartsWith("lblStatus") OrElse
                                                DirectCast(ctrl, Label).Name.StartsWith("lblCritical") OrElse
                                                DirectCast(ctrl, Label).Name.StartsWith("lblError")) Then
                    controlsToRemove.Add(ctrl)
                ElseIf TypeOf ctrl Is Panel AndAlso DirectCast(ctrl, Panel).Name.StartsWith("containerPanel") Then
                    controlsToRemove.Add(ctrl)
                End If
            Next
            For Each ctrl As Control In controlsToRemove
                Panel4.Controls.Remove(ctrl)
            Next

            ' Show error in panel
            Dim lblError As New Label()
            lblError.Name = "lblErrorStock"
            lblError.Text = "Error loading stock data"
            lblError.Font = New Font("Segoe UI", 12, FontStyle.Bold)
            lblError.ForeColor = Color.Red
            lblError.AutoSize = False
            lblError.Size = New Size(Panel4.Width - 20, 60)  ' Increased from 50 to 60
            lblError.TextAlign = ContentAlignment.MiddleCenter
            lblError.Location = New Point(10, (Panel4.Height - 60) \ 2)  ' Adjusted position for new height
            Panel4.Controls.Add(lblError)
            Console.WriteLine("Error loading stock level status: " & ex.Message)
        End Try
    End Sub

    Private Sub LoadTopProductsData()
        ' Load top 10 products data for the current month (using RetailSalesReport)
        Dim connStr As String = GetConnectionString()
        Dim firstDayOfMonth As Date = New Date(topProductsCurrentMonth.Year, topProductsCurrentMonth.Month, 1)
        Dim lastDayOfMonth As Date = firstDayOfMonth.AddMonths(1).AddDays(-1)

        ' Get the top 10 products for the month from RetailSalesReport
        Dim topProductsSql As String = "
        SELECT TOP 10
            sr.ProductID,
            p.ProductName,
            SUM(sr.TotalAmount) AS TotalRevenue,
            SUM(sr.QuantitySold) AS TotalQuantity,
            COUNT(*) AS SalesCount
        FROM RetailSalesReport sr
        INNER JOIN retailProducts p ON sr.ProductID = p.ProductID
        WHERE sr.SaleDate >= @StartDate AND sr.SaleDate <= @EndDate
        GROUP BY sr.ProductID, p.ProductName
        ORDER BY TotalRevenue DESC, TotalQuantity DESC, SalesCount DESC
        "

        topProducts.Clear()

        Try
            Using conn As New SqlConnection(connStr)
                conn.Open()

                ' Get top 10 products
                Using cmd As New SqlCommand(topProductsSql, conn)
                    cmd.Parameters.AddWithValue("@StartDate", firstDayOfMonth)
                    cmd.Parameters.AddWithValue("@EndDate", lastDayOfMonth)

                    Dim productColors As Color() = {
                        Color.FromArgb(147, 53, 53),
                        Color.FromArgb(79, 51, 40),
                        Color.FromArgb(102, 66, 52),
                        Color.FromArgb(191, 181, 147),
                        Color.FromArgb(224, 166, 109),
                        Color.FromArgb(180, 100, 80),
                        Color.FromArgb(120, 80, 60),
                        Color.FromArgb(160, 140, 120),
                        Color.FromArgb(200, 150, 100),
                        Color.FromArgb(140, 90, 70)
                    }

                    Dim colorIndex As Integer = 0
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            Dim productInfo As New TopProductInfo() With {
                                .ProductID = reader.GetInt32(0),
                                .ProductName = reader.GetString(1),
                                .TotalRevenue = reader.GetDecimal(2),
                                .TotalQuantity = reader.GetInt32(3),
                                .TotalSalesCount = reader.GetInt32(4),
                                .Color = productColors(colorIndex Mod productColors.Length)
                            }
                            topProducts.Add(productInfo)
                            colorIndex += 1
                        End While
                    End Using
                End Using
            End Using

            ' Setup the chart panel
            SetupTopProductsChartPanel()

        Catch ex As Exception
            topProducts.Clear()
            SetupTopProductsChartPanel()
        End Try
    End Sub

    Private Sub SetupTopProductsChartPanel()
        ' Clear existing controls
        Panel5.Controls.Clear()

        ' Create navigation panel at the top
        Dim navPanel As New Panel()
        navPanel.Height = 50
        navPanel.Dock = DockStyle.Top
        navPanel.BackColor = Color.FromArgb(230, 216, 177)

        Dim navWidth As Integer = 280

        ' Center the navigation buttons
        Dim startX As Integer = (navPanel.Width - navWidth) \ 2
        If navPanel.Width <= 0 Then
            startX = (Panel5.Width - navWidth) \ 2
        End If

        ' Previous month button
        Dim btnPrevMonth As New Button()
        btnPrevMonth.Text = "◀"
        btnPrevMonth.Size = New Size(80, 30)
        btnPrevMonth.Location = New Point(startX, 10)
        btnPrevMonth.BackColor = Color.FromArgb(147, 53, 53)
        btnPrevMonth.ForeColor = Color.FromArgb(230, 216, 177)
        btnPrevMonth.FlatStyle = FlatStyle.Flat
        btnPrevMonth.Cursor = Cursors.Hand
        AddHandler btnPrevMonth.Click, AddressOf TopProductsPrevMonth_Click

        ' Current month label
        Dim lblCurrentMonth As New Label()
        lblCurrentMonth.Text = topProductsCurrentMonth.ToString("MMMM yyyy")
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
        btnNextMonth.Cursor = Cursors.Hand
        AddHandler btnNextMonth.Click, AddressOf TopProductsNextMonth_Click

        ' Disable next month if viewing current month
        If topProductsCurrentMonth >= New Date(Date.Today.Year, Date.Today.Month, 1) Then
            btnNextMonth.Enabled = False
            btnNextMonth.BackColor = Color.Gray
        End If

        ' Add resize handler to recenter buttons when panel is resized
        AddHandler navPanel.Resize, Sub()
                                        If navPanel.Width > 0 Then
                                            Dim newStartX As Integer = (navPanel.Width - navWidth) \ 2
                                            btnPrevMonth.Location = New Point(newStartX, 10)
                                            lblCurrentMonth.Location = New Point(newStartX + 90, 10)
                                            btnNextMonth.Location = New Point(newStartX + 200, 10)
                                        End If
                                    End Sub

        navPanel.Controls.Add(btnPrevMonth)
        navPanel.Controls.Add(lblCurrentMonth)
        navPanel.Controls.Add(btnNextMonth)

        ' Create scrollable container for chart
        Dim scrollContainer As New Panel()
        scrollContainer.Dock = DockStyle.Fill
        scrollContainer.AutoScroll = True
        scrollContainer.BackColor = Color.FromArgb(230, 216, 177)

        ' Create chart panel with fixed height based on number of products
        topProductsChartPanel = New Panel()
        topProductsChartPanel.BackColor = Color.FromArgb(230, 216, 177)
        topProductsChartPanel.Dock = DockStyle.Top

        ' Calculate required height: bars + updated margins (60 top + 20 bottom)
        Dim barHeight As Integer = 50
        Dim barSpacing As Integer = 10
        Dim topMargin As Integer = 60
        Dim bottomMargin As Integer = 20
        Dim requiredHeight As Integer = topMargin + bottomMargin + (topProducts.Count * (barHeight + barSpacing))
        topProductsChartPanel.Height = requiredHeight

        AddHandler topProductsChartPanel.Paint, AddressOf DrawTopProductsChart
        AddHandler topProductsChartPanel.MouseMove, AddressOf TopProductsChart_MouseMove
        AddHandler topProductsChartPanel.MouseLeave, AddressOf TopProductsChart_MouseLeave

        ' Create tooltip
        topProductsTooltipLabel = New Label()
        topProductsTooltipLabel.AutoSize = True
        topProductsTooltipLabel.BackColor = Color.FromArgb(255, 255, 200)
        topProductsTooltipLabel.BorderStyle = BorderStyle.FixedSingle
        topProductsTooltipLabel.Font = New Font("Arial", 9, FontStyle.Bold)
        topProductsTooltipLabel.ForeColor = Color.Black
        topProductsTooltipLabel.Visible = False
        topProductsChartPanel.Controls.Add(topProductsTooltipLabel)

        scrollContainer.Controls.Add(topProductsChartPanel)

        Panel5.Controls.Add(navPanel)
        Panel5.Controls.Add(scrollContainer)
    End Sub

    Private Sub TopProductsPrevMonth_Click(sender As Object, e As EventArgs)
        topProductsCurrentMonth = topProductsCurrentMonth.AddMonths(-1)
        LoadTopProductsData()
    End Sub

    Private Sub TopProductsNextMonth_Click(sender As Object, e As EventArgs)
        If topProductsCurrentMonth < New Date(Date.Today.Year, Date.Today.Month, 1) Then
            topProductsCurrentMonth = topProductsCurrentMonth.AddMonths(1)
            LoadTopProductsData()
        End If
    End Sub

    Private Sub TopProductsChart_MouseMove(sender As Object, e As MouseEventArgs)
        If topProducts.Count = 0 Then Return

        Dim panel As Panel = DirectCast(sender, Panel)
        Dim leftMargin As Integer = 150
        Dim rightMargin As Integer = 50
        Dim topMargin As Integer = 40
        Dim bottomMargin As Integer = 40

        Dim chartWidth As Integer = panel.Width - leftMargin - rightMargin
        Dim chartHeight As Integer = panel.Height - topMargin - bottomMargin
        Dim chartRect As New Rectangle(leftMargin, topMargin, chartWidth, chartHeight)

        ' Calculate bar dimensions
        Dim barHeight As Integer = (chartHeight \ topProducts.Count) - 10
        Dim barSpacing As Integer = 10

        ' Check if mouse is over any bar
        For i As Integer = 0 To topProducts.Count - 1
            Dim yPosition As Integer = chartRect.Top + (i * (barHeight + barSpacing))
            Dim barRect As New Rectangle(chartRect.Left, yPosition, chartWidth, barHeight)

            If barRect.Contains(e.Location) Then
                Dim product = topProducts(i)
                topProductsTooltipLabel.Text = $"{product.ProductName}" & vbCrLf &
                                              $"Revenue: ₱{product.TotalRevenue:N2}" & vbCrLf &
                                              $"Qty Sold: {product.TotalQuantity}" & vbCrLf &
                                              $"Sales Count: {product.TotalSalesCount}"
                topProductsTooltipLabel.Location = New Point(e.X + 10, e.Y - 50)
                topProductsTooltipLabel.Visible = True
                topProductsTooltipLabel.BringToFront()
                Return
            End If
        Next

        topProductsTooltipLabel.Visible = False
    End Sub

    Private Sub TopProductsChart_MouseLeave(sender As Object, e As EventArgs)
        topProductsTooltipLabel.Visible = False
    End Sub

    Private Sub DrawTopProductsChart(sender As Object, e As PaintEventArgs)
        If topProducts.Count = 0 Then
            DrawNoTopProductsDataMessage(e.Graphics, DirectCast(sender, Panel))
            Return
        End If

        Dim panel As Panel = DirectCast(sender, Panel)
        Dim g As Graphics = e.Graphics
        g.SmoothingMode = SmoothingMode.AntiAlias

        ' Margins for horizontal bar chart
        Dim leftMargin As Integer = 150 ' Space for product names
        Dim rightMargin As Integer = 100 ' Increased to 100 to accommodate text outside bars
        Dim topMargin As Integer = 60 ' Increased from 40 to 60 to give more space for the title
        Dim bottomMargin As Integer = 20 ' Reduced bottom margin

        ' Use full panel width (accounting for potential scrollbar)
        Dim availableWidth As Integer = panel.Width - 20 ' Account for scrollbar
        Dim chartWidth As Integer = availableWidth - leftMargin - rightMargin

        ' Calculate total height needed for all products
        Dim barHeight As Integer = 50
        Dim barSpacing As Integer = 10
        Dim totalHeight As Integer = topMargin + bottomMargin + (topProducts.Count * (barHeight + barSpacing))

        Dim chartRect As New Rectangle(leftMargin, topMargin, chartWidth, totalHeight - topMargin - bottomMargin)

        ' Find max revenue for scaling
        Dim maxRevenue As Decimal = If(topProducts.Max(Function(x) x.TotalRevenue) > 0, topProducts.Max(Function(x) x.TotalRevenue), 100)
        maxRevenue = maxRevenue * 1.1D ' Add 10% padding

        ' Draw title with more top spacing
        Dim titleFont As New Font("Arial", 12, FontStyle.Bold)
        Dim titleBrush As New SolidBrush(Color.FromArgb(79, 51, 40))
        g.DrawString($"Top {topProducts.Count} Products - {topProductsCurrentMonth.ToString("MMMM yyyy")}", titleFont, titleBrush, leftMargin, 15)

        ' Draw bars for all products
        For i As Integer = 0 To topProducts.Count - 1
            Dim product = topProducts(i)
            Dim yPosition As Integer = topMargin + (i * (barHeight + barSpacing))

            ' Calculate bar width based on revenue (scale to 90% of available width to leave room for labels)
            Dim maxBarWidth As Integer = CInt(chartWidth * 0.9)
            Dim barWidth As Integer = CInt((product.TotalRevenue / maxRevenue) * maxBarWidth)

            ' Draw product name on the left
            Dim nameFont As New Font("Arial", 9, FontStyle.Bold)
            Dim nameBrush As New SolidBrush(Color.FromArgb(79, 51, 40))
            Dim productName As String = product.ProductName

            ' Truncate name if too long
            If productName.Length > 18 Then
                productName = productName.Substring(0, 15) & "..."
            End If

            Dim nameSize = g.MeasureString(productName, nameFont)
            g.DrawString(productName, nameFont, nameBrush,
                        leftMargin - nameSize.Width - 10,
                        yPosition + (barHeight - nameSize.Height) / 2)

            ' Draw bar with solid color
            Dim barRect As New Rectangle(leftMargin, yPosition, barWidth, barHeight)
            Using barBrush As New SolidBrush(product.Color)
                g.FillRectangle(barBrush, barRect)
            End Using

            ' Draw bar border
            Using borderPen As New Pen(Color.FromArgb(79, 51, 40), 2)
                g.DrawRectangle(borderPen, barRect)
            End Using

            ' Draw revenue text on the bar
            Dim revenueText As String = $"₱{product.TotalRevenue:N0}"
            Dim revenueFont As New Font("Arial", 8, FontStyle.Bold)
            Dim revenueSize = g.MeasureString(revenueText, revenueFont)
            Dim revenueBrush As New SolidBrush(Color.White)

            ' Position text inside bar if it fits, otherwise outside
            If barWidth > revenueSize.Width + 10 Then
                g.DrawString(revenueText, revenueFont, revenueBrush,
                           barRect.Left + 5,
                           yPosition + (barHeight - revenueSize.Height) / 2)
            Else
                Dim darkBrush As New SolidBrush(Color.FromArgb(79, 51, 40))
                g.DrawString(revenueText, revenueFont, darkBrush,
                           barRect.Right + 5,
                           yPosition + (barHeight - revenueSize.Height) / 2)
            End If
        Next

        ' Draw vertical axis line (extended for full height)
        Using axisPen As New Pen(Color.FromArgb(79, 51, 40), 2)
            g.DrawLine(axisPen, leftMargin, topMargin, leftMargin, topMargin + (topProducts.Count * (barHeight + barSpacing)))
            ' Draw horizontal axis line (adjusted to not extend too far)
            g.DrawLine(axisPen, leftMargin, topMargin + (topProducts.Count * (barHeight + barSpacing)),
                      leftMargin + CInt(chartWidth * 0.9), topMargin + (topProducts.Count * (barHeight + barSpacing)))
        End Using
    End Sub

    Private Sub DrawNoTopProductsDataMessage(g As Graphics, panel As Panel)
        Dim messageFont As New Font("Arial", 16, FontStyle.Bold)
        Dim messageBrush As New SolidBrush(Color.FromArgb(79, 51, 40))
        Dim message As String = $"No Product Data for {topProductsCurrentMonth.ToString("MMMM yyyy")}"
        Dim messageSize = g.MeasureString(message, messageFont)

        g.DrawString(message, messageFont, messageBrush,
                    (panel.Width - messageSize.Width) / 2,
                    (panel.Height - messageSize.Height) / 2)
    End Sub
End Class
