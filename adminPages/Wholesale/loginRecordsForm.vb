Imports Microsoft.Data.SqlClient
Imports System.Data

Public Class loginRecordsForm
    Dim topPanel As topPanelControl
    Friend WithEvents sidePanel As sidePanelControl
    Friend WithEvents sidePanel2 As sidePanelControl2
    Dim colorUnclicked As Color = Color.FromArgb(191, 181, 147)
    Dim colorClicked As Color = Color.FromArgb(102, 66, 52)
    Dim dt As New DataTable()
    Dim dv As New DataView()
    Dim bs As New BindingSource()

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        'sidePanel = New sidePanelControl()
        'sidePanel.Dock = DockStyle.Left
        'topPanel.Dock = DockStyle.Top
        'Me.Controls.Add(topPanel)
        'Me.Controls.Add(sidePanel)
        Me.MaximizeBox = False
        Me.FormBorderStyle = FormBorderStyle.None
        Me.BackColor = Color.FromArgb(224, 166, 109)

        filterButton.BackColor = Color.FromArgb(147, 53, 53)
        filterButton.ForeColor = Color.FromArgb(230, 216, 177)
        resetButton.BackColor = Color.FromArgb(147, 53, 53)
        resetButton.ForeColor = Color.FromArgb(230, 216, 177)

        tableDataGridView.BackgroundColor = Color.FromArgb(230, 216, 177)
        tableDataGridView.GridColor = Color.FromArgb(79, 51, 40)
        tableDataGridView.ReadOnly = True
        tableDataGridView.AllowUserToAddRows = False
        tableDataGridView.AllowUserToDeleteRows = False
        tableDataGridView.RowHeadersVisible = False
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
        ' Hide all forms except the one to show
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

        ' No need to call HighlightButton here

    End Sub

    Private Sub logsForm_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        chooseDashboard2.globalPage = ""
    End Sub

    Private Sub HighlightButton(buttonName As String)
        ' Reset all buttons
        If sidePanel IsNot Nothing Then
            For Each ctrl As Control In sidePanel.Controls
                If TypeOf ctrl Is Button Then
                    ctrl.BackColor = colorClicked
                    ctrl.ForeColor = colorUnclicked
                End If
            Next
        End If
        If sidePanel2 IsNot Nothing Then
            For Each ctrl As Control In sidePanel2.Controls
                If TypeOf ctrl Is Button Then
                    ctrl.BackColor = colorClicked
                    ctrl.ForeColor = colorUnclicked
                End If
            Next
        End If

        ' Highlight the selected button (for whichever panel is active)
        Dim btn As Button = Nothing
        If sidePanel IsNot Nothing Then
            btn = sidePanel.Controls.OfType(Of Button)().FirstOrDefault(Function(b) b.Name = buttonName)
        End If
        If btn Is Nothing AndAlso sidePanel2 IsNot Nothing Then
            btn = sidePanel2.Controls.OfType(Of Button)().FirstOrDefault(Function(b) b.Name = buttonName)
        End If

        ' Apply highlight colors
        If btn IsNot Nothing Then
            btn.BackColor = colorUnclicked
            btn.ForeColor = Color.FromArgb(79, 51, 40)
        End If
    End Sub

    Private Sub SidePanel_ButtonClicked(sender As Object, btnName As String) Handles sidePanel.ButtonClicked, sidePanel2.ButtonClicked
        If chooseDashboard2.globalPage.ToLower() = "wholesale" Then
            Select Case btnName
                Case "Button1"
                    Dim form = ShowSingleForm(Of wholesaleDashboard)()
                    form.LoadDashboardData()
                Case "Button2"
                    Dim form = ShowSingleForm(Of InventoryForm)()
                    form.LoadProducts()
                Case "Button3"
                    Dim form = ShowSingleForm(Of categoriesForm)()
                    form.loadCategories()
                Case "Button4"
                    ShowSingleForm(Of deliveryLogsForm)()
                Case "Button5"
                    Dim form = ShowSingleForm(Of salesReport)()
                    form.loadSalesReport()
                Case "Button6"
                    ShowSingleForm(Of loginRecordsForm)()
                Case "Button7"
                    Dim form = ShowSingleForm(Of userManagementForm)()
                    form.LoadUsers()
            End Select
        Else
            Select Case btnName
                Case "Button1"
                    Dim form = ShowSingleForm(Of retailDashboard)()
                    form.LoadDashboardData()
                Case "Button2"
                    ShowSingleForm(Of inventoryRetail)()
                Case "Button3"
                    Dim form = ShowSingleForm(Of categoriesForm)()
                    form.loadCategories()
                Case "Button4"
                    Dim form = ShowSingleForm(Of retailStockEditLogs)()
                    form.loadStockEditLogs()
                Case "Button5"
                    Dim form = ShowSingleForm(Of wholeSaleStockEditLogs)()
                    form.loadStockEditLogs()
                Case "Button10"
                    ShowSingleForm(Of retailSalesReport)()
                Case "Button6"
                    ShowSingleForm(Of loginRecordsForm)()
                Case "Button7"
                    Dim form = ShowSingleForm(Of userManagementForm)()
                    form.LoadUsers()
            End Select
        End If
    End Sub

    Private Sub logsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Clear old panels
        For Each ctrl As Control In Me.Controls.OfType(Of Control).ToList()
            If TypeOf ctrl Is topPanelControl OrElse
               TypeOf ctrl Is sidePanelControl OrElse
               TypeOf ctrl Is sidePanelControl2 Then
                Me.Controls.Remove(ctrl)
                ctrl.Dispose()
            End If
        Next

        ' Create and assign to class-level fields
        Me.topPanel = New topPanelControl()
        Me.topPanel.Dock = DockStyle.Top

        If chooseDashboard2.globalPage.ToLower() = "wholesale" Then
            Me.topPanel.Label1.Text = "WHOLESALE"
            Me.sidePanel = New sidePanelControl()
            Me.sidePanel.Dock = DockStyle.Left
            Me.Controls.Add(Me.topPanel)
            Me.Controls.Add(Me.sidePanel)

        ElseIf chooseDashboard2.globalPage.ToLower() = "retail" Then
            Me.topPanel.Label1.Text = "RETAIL"
            Me.sidePanel2 = New sidePanelControl2()
            Me.sidePanel2.Dock = DockStyle.Left
            Me.Controls.Add(Me.topPanel)
            Me.Controls.Add(Me.sidePanel2)
        End If

        HighlightButton("Button6")

        ' Initialize
        dt = New DataTable()
        dv = New DataView(dt)
        bs = New BindingSource()

        ' Bindings
        bs.DataSource = dv
        tableDataGridView.DataSource = bs

        LoadLoginHistory()
    End Sub

    Private Function GetConnectionString() As String
        Return "Server=DESKTOP-3AKTMEV;Database=inventorySystem;User Id=sa;Password=24@Hakaaii07;TrustServerCertificate=True;"
    End Function

    Public Sub LoadLoginHistory()
        Try
            Using conn As New SqlConnection(GetConnectionString())
                conn.Open()

                Dim query As String = "SELECT h.LoginID, u.username, h.LoginTime, h.DeviceInfo " &
                                      "FROM UserLoginHistory h " &
                                      "INNER JOIN Users u ON h.UserID = u.UserID " &
                                      "ORDER BY h.LoginTime DESC"

                Using cmd As New SqlCommand(query, conn)
                    Using da As New SqlDataAdapter(cmd)
                        da.Fill(dt)

                        ' Bind to DataGridView
                        tableDataGridView.DataSource = dt

                        With tableDataGridView
                            .EnableHeadersVisualStyles = False ' allow custom styling
                            .ColumnHeadersDefaultCellStyle.Font = New Font(.Font, FontStyle.Bold)
                            .Columns("LoginID").HeaderText = "Login ID"
                            .Columns("username").HeaderText = "Username"
                            .Columns("LoginTime").HeaderText = "Login Time"
                            .Columns("DeviceInfo").HeaderText = "Device Info"
                        End With
                    End Using
                End Using
            End Using

        Catch ex As Exception
            MessageBox.Show("Error loading login history: " & ex.Message)
        End Try
    End Sub

    Private Sub filterButton_Click(sender As Object, e As EventArgs) Handles filterButton.Click
        Using con As New SqlConnection(GetConnectionString())
            Dim query As String = "
            SELECT h.LoginID, u.username, h.LoginTime, h.DeviceInfo
            FROM UserLoginHistory h
            INNER JOIN Users u ON h.UserID = u.UserID
            WHERE h.LoginTime >= @FromDate AND h.LoginTime < @ToDateNextDay
            ORDER BY h.LoginTime DESC"

            Using cmd As New SqlCommand(query, con)
                cmd.Parameters.AddWithValue("@FromDate", DateTimePickerFrom.Value.Date)
                cmd.Parameters.AddWithValue("@ToDateNextDay", DateTimePickerTo.Value.Date.AddDays(1))

                Dim da As New SqlDataAdapter(cmd)
                dt.Clear()                ' reuse class-level dt (mybase load) to keep the same binding
                da.Fill(dt)               ' fills the same dt bound to dv/bs/grid
            End Using
        End Using
    End Sub

    Private Sub resetButton_Click(sender As Object, e As EventArgs) Handles resetButton.Click
        ' Reset the DateTimePickers to today's date
        DateTimePickerFrom.Value = DateTime.Today
        DateTimePickerTo.Value = DateTime.Today

        ' Reload all records
        LoadLoginHistory()
    End Sub
End Class