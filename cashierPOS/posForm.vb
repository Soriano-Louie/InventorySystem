Public Class posForm
    Dim topPanel As New topControlCashier()
    Private WithEvents updateTimer As New Timer()

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.Controls.Add(topPanel)
        topPanel.Dock = DockStyle.Top
        Me.MaximizeBox = False
        Me.FormBorderStyle = FormBorderStyle.None
        Me.BackColor = Color.FromArgb(224, 166, 109)

        TableLayoutPanel1.BackColor = Color.FromArgb(224, 166, 109)
        bottomPanel.BackColor = Color.FromArgb(230, 216, 177)
        featuresPanel.BackColor = Color.FromArgb(79, 51, 40)

        Button1.BackColor = Color.FromArgb(230, 216, 177)
        Button2.BackColor = Color.FromArgb(230, 216, 177)
        Button3.BackColor = Color.FromArgb(230, 216, 177)
        Button4.BackColor = Color.FromArgb(230, 216, 177)
        Button5.BackColor = Color.FromArgb(230, 216, 177)

        Button1.ForeColor = Color.FromArgb(79, 51, 40)
        Button2.ForeColor = Color.FromArgb(79, 51, 40)
        Button3.ForeColor = Color.FromArgb(79, 51, 40)
        Button4.ForeColor = Color.FromArgb(79, 51, 40)
        Button5.ForeColor = Color.FromArgb(79, 51, 40)

        DataGridView1.BackgroundColor = Color.FromArgb(224, 166, 109)

        ' Configure DataGridView1
        ConfigureDataGridView()

        ' Set label colors
        timeLabel.ForeColor = Color.FromArgb(79, 51, 40)
        dateYearLabel.ForeColor = Color.FromArgb(79, 51, 40)

        ' Initialize and start the timer for updating time and date
        updateTimer.Interval = 1000 ' Update every 1 second (1000 milliseconds)
        updateTimer.Start()

        ' Update time and date immediately
        UpdateTimeAndDate()
    End Sub

    Private Sub UpdateTimeAndDate()
        ' Get current date and time
        Dim currentDateTime As DateTime = DateTime.Now

        ' Update time label (format: HH:mm:ss)
        timeLabel.Text = currentDateTime.ToString("hh:mm:ss tt")

        ' Update date and year label (format: Day, Month Date, Year - e.g., "Friday, January 10, 2025")
        dateYearLabel.Text = currentDateTime.ToString("dddd, MMMM dd, yyyy")
    End Sub

    Private Sub UpdateTimer_Tick(sender As Object, e As EventArgs) Handles updateTimer.Tick
        ' This event fires every second to update the time and date
        UpdateTimeAndDate()
    End Sub

    Private Sub ConfigureDataGridView()
        ' Clear any existing columns
        DataGridView1.Columns.Clear()

        ' Set header style
        DataGridView1.EnableHeadersVisualStyles = False
        DataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(79, 51, 40)
        DataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(230, 216, 177)
        DataGridView1.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        DataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

        ' Add columns
        ' Column 1: #
        DataGridView1.Columns.Add("ColNumber", "#")
        DataGridView1.Columns("ColNumber").ReadOnly = True
        DataGridView1.Columns("ColNumber").FillWeight = 8

        ' Column 2: PRODUCT NAME
        DataGridView1.Columns.Add("ColProductName", "PRODUCT NAME")
        DataGridView1.Columns("ColProductName").ReadOnly = True
        DataGridView1.Columns("ColProductName").FillWeight = 30

        ' Column 3: QTY
        DataGridView1.Columns.Add("ColQty", "QTY")
        DataGridView1.Columns("ColQty").ReadOnly = True
        DataGridView1.Columns("ColQty").FillWeight = 12

        ' Column 4: PRICE
        DataGridView1.Columns.Add("ColPrice", "PRICE")
        DataGridView1.Columns("ColPrice").ReadOnly = True
        DataGridView1.Columns("ColPrice").DefaultCellStyle.Format = "₱#,##0.00"
        DataGridView1.Columns("ColPrice").FillWeight = 15

        ' Column 5: TOTAL
        DataGridView1.Columns.Add("ColTotal", "TOTAL")
        DataGridView1.Columns("ColTotal").ReadOnly = True
        DataGridView1.Columns("ColTotal").DefaultCellStyle.Format = "₱#,##0.00"
        DataGridView1.Columns("ColTotal").FillWeight = 18

        ' Column 6: ADD (Button)
        Dim btnAdd As New DataGridViewButtonColumn()
        btnAdd.Name = "ColAdd"
        btnAdd.HeaderText = "ADD"
        btnAdd.Text = "+"
        btnAdd.UseColumnTextForButtonValue = True
        btnAdd.FillWeight = 9
        DataGridView1.Columns.Add(btnAdd)

        ' Column 7: SUBTRACT (Button)
        Dim btnSubtract As New DataGridViewButtonColumn()
        btnSubtract.Name = "ColSubtract"
        btnSubtract.HeaderText = "SUBTRACT"
        btnSubtract.Text = "-"
        btnSubtract.UseColumnTextForButtonValue = True
        btnSubtract.FillWeight = 12
        DataGridView1.Columns.Add(btnSubtract)

        ' Column 8: DELETE (Button)
        Dim btnDelete As New DataGridViewButtonColumn()
        btnDelete.Name = "ColDelete"
        btnDelete.HeaderText = "DELETE"
        btnDelete.Text = "X"
        btnDelete.UseColumnTextForButtonValue = True
        btnDelete.FillWeight = 10
        DataGridView1.Columns.Add(btnDelete)

        ' Additional DataGridView settings
        DataGridView1.AllowUserToAddRows = False
        DataGridView1.AllowUserToDeleteRows = False
        DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridView1.MultiSelect = False
        DataGridView1.RowHeadersVisible = False
        DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
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
End Class