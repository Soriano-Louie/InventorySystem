Imports Microsoft.Data.SqlClient
Imports System.Data

Public Class loginRecordsForm
    Dim topPanel As New topPanelControl()
    Friend WithEvents sidePanel As sidePanelControl
    Dim colorUnclicked As Color = Color.FromArgb(191, 181, 147)
    Dim colorClicked As Color = Color.FromArgb(102, 66, 52)
    Dim dt As New DataTable()
    Dim dv As New DataView()
    Dim bs As New BindingSource()

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        sidePanel = New sidePanelControl()
        sidePanel.Dock = DockStyle.Left
        topPanel.Dock = DockStyle.Top
        Me.Controls.Add(topPanel)
        Me.Controls.Add(sidePanel)
        Me.MaximizeBox = False
        Me.FormBorderStyle = FormBorderStyle.None
        Me.BackColor = Color.FromArgb(224, 166, 109)

        filterButton.BackColor = Color.FromArgb(147, 53, 53)
        filterButton.ForeColor = Color.FromArgb(230, 216, 177)

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

    Private Sub ShowSingleForm(Of T As {Form, New})()
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
    End Sub

    Private Sub ChildFormClosed(sender As Object, e As FormClosedEventArgs)

        ' No need to call HighlightButton here

    End Sub

    Private Sub logsForm_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        Application.Exit()
    End Sub

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
                ShowSingleForm(Of WholesaleDashboard)()
            Case "Button2"
                ShowSingleForm(Of InventoryForm)()
            Case "Button3"
                ShowSingleForm(Of categoriesForm)()
            Case "Button4"
                ShowSingleForm(Of deliveryLogsForm)()
            Case "Button5"
                ShowSingleForm(Of salesReport)()
            Case "Button6"
                ShowSingleForm(Of loginRecordsForm)()
            Case "Button7"
                ShowSingleForm(Of userManagementForm)()
        End Select
    End Sub

    Private Sub logsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        HighlightButton("Button6")
        ' Example data
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

    Private Sub LoadLoginHistory()
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
End Class