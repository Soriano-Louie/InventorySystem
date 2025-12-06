Imports Microsoft.Data.SqlClient

Public Class DailyTransactionsForm
    Private parentForm As posForm
    Private dt As New DataTable()
    Private dv As New DataView()
    Private bs As New BindingSource()

    ' Track view state
    Private isDetailView As Boolean = False

    Private currentBatchDate As DateTime
    Private currentBatchCashier As String
    Private currentBatchType As String

    Public Sub New(parent As posForm)
        InitializeComponent()
        Me.parentForm = parent
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.StartPosition = FormStartPosition.CenterParent
        Me.BackColor = Color.FromArgb(230, 216, 177)
        Me.Size = New Size(1000, 600)
    End Sub

    Private Sub DailyTransactionsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Setup UI colors
        lblTitle.Text = $"Daily Transactions - {DateTime.Today:dddd, MMMM dd, yyyy}"
        lblTitle.Font = New Font("Segoe UI", 16, FontStyle.Bold)
        lblTitle.ForeColor = Color.FromArgb(79, 51, 40)
        lblTitle.TextAlign = ContentAlignment.MiddleCenter
        lblTitle.Dock = DockStyle.Top
        lblTitle.Height = 50

        ' Setup summary labels
        lblTotalTransactions.Font = New Font("Segoe UI", 11, FontStyle.Bold)
        lblTotalTransactions.ForeColor = Color.FromArgb(79, 51, 40)

        lblTotalRevenue.Font = New Font("Segoe UI", 11, FontStyle.Bold)
        lblTotalRevenue.ForeColor = Color.FromArgb(79, 51, 40)

        lblRetailCount.Font = New Font("Segoe UI", 10, FontStyle.Regular)
        lblRetailCount.ForeColor = Color.FromArgb(79, 51, 40)

        lblWholesaleCount.Font = New Font("Segoe UI", 10, FontStyle.Regular)
        lblWholesaleCount.ForeColor = Color.FromArgb(79, 51, 40)

        ' Setup DataGridView
        transactionsDataGridView.BackgroundColor = Color.FromArgb(230, 216, 177)
        transactionsDataGridView.GridColor = Color.FromArgb(79, 51, 40)
        transactionsDataGridView.EnableHeadersVisualStyles = False
        transactionsDataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(79, 51, 40)
        transactionsDataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(230, 216, 177)
        transactionsDataGridView.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        transactionsDataGridView.DefaultCellStyle.SelectionBackColor = Color.FromArgb(79, 51, 40)
        transactionsDataGridView.DefaultCellStyle.SelectionForeColor = Color.FromArgb(230, 216, 177)
        transactionsDataGridView.ReadOnly = True
        transactionsDataGridView.AllowUserToAddRows = False
        transactionsDataGridView.AllowUserToDeleteRows = False
        transactionsDataGridView.RowHeadersVisible = False
        transactionsDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect

        ' Make columns and rows non-resizable
        transactionsDataGridView.AllowUserToResizeColumns = False
        transactionsDataGridView.AllowUserToResizeRows = False

        ' Add single-click event handler for batch view
        AddHandler transactionsDataGridView.CellClick, AddressOf TransactionsDataGridView_CellClick
        ' Add double-click event handler for detail view (receipts)
        AddHandler transactionsDataGridView.CellDoubleClick, AddressOf TransactionsDataGridView_CellDoubleClick

        ' Add tooltip
        Dim toolTip As New ToolTip()
        toolTip.SetToolTip(transactionsDataGridView, "Click a batch to view details | Double-click an item to view receipt")

        ' Setup buttons - initially hidden except Refresh and Close
        btnRefresh.BackColor = Color.FromArgb(147, 53, 53)
        btnRefresh.ForeColor = Color.FromArgb(230, 216, 177)
        btnRefresh.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        btnRefresh.FlatStyle = FlatStyle.Flat
        btnRefresh.Cursor = Cursors.Hand

        btnRefund.BackColor = Color.FromArgb(147, 53, 53)
        btnRefund.ForeColor = Color.FromArgb(230, 216, 177)
        btnRefund.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        btnRefund.FlatStyle = FlatStyle.Flat
        btnRefund.Cursor = Cursors.Hand
        btnRefund.Visible = False ' Hidden in batch view

        btnViewReceipt.BackColor = Color.FromArgb(147, 53, 53)
        btnViewReceipt.ForeColor = Color.FromArgb(230, 216, 177)
        btnViewReceipt.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        btnViewReceipt.FlatStyle = FlatStyle.Flat
        btnViewReceipt.Cursor = Cursors.Hand
        btnViewReceipt.Visible = False ' Hidden in batch view

        btnClose.BackColor = Color.FromArgb(79, 51, 40)
        btnClose.ForeColor = Color.FromArgb(230, 216, 177)
        btnClose.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        btnClose.FlatStyle = FlatStyle.Flat
        btnClose.Cursor = Cursors.Hand

        ' Setup filter ComboBox
        cboSalesType.BackColor = Color.FromArgb(230, 216, 177)
        cboSalesType.ForeColor = Color.FromArgb(79, 51, 40)
        cboSalesType.DropDownStyle = ComboBoxStyle.DropDownList
        cboSalesType.Items.Clear()
        cboSalesType.Items.Add("All Transactions")
        cboSalesType.Items.Add("Retail Only")
        cboSalesType.Items.Add("Wholesale Only")
        cboSalesType.Items.Add("Refunded Only")
        cboSalesType.Items.Add("Non-Refunded Only")
        cboSalesType.SelectedIndex = 0

        ' Initialize data binding
        dt = New DataTable()
        dv = New DataView(dt)
        bs = New BindingSource()
        bs.DataSource = dv
        transactionsDataGridView.DataSource = bs

        ' Load batch transactions
        LoadBatchTransactions()

        ' Enable form to receive key events before child controls
        Me.KeyPreview = True
        AddHandler Me.KeyDown, AddressOf DailyTransactionsForm_KeyDown
    End Sub

    ' Close form when Escape is pressed
    Private Sub DailyTransactionsForm_KeyDown(sender As Object, e As KeyEventArgs)
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub

    ''' <summary>
    ''' Load grouped batch transactions (summary view)
    ''' Groups by SaleDate (minute precision) and HandledBy
    ''' </summary>
    Private Sub LoadBatchTransactions()
        Try
            Dim connStr As String = GetConnectionString()
            Dim today As Date = DateTime.Today

            ' Query to group transactions by time and cashier
            Dim query As String = "
                WITH AllTransactions AS (
                    -- Retail transactions
                    SELECT
                        'RETAIL' AS SaleType,
                        sr.SaleDate,
                        u.username AS HandledBy,
                        sr.SaleID,
                        sr.TotalAmount,
                        ISNULL(sr.IsRefunded, 0) AS IsRefunded
                    FROM RetailSalesReport sr
                    INNER JOIN Users u ON sr.HandledBy = u.userID
                    WHERE CAST(sr.SaleDate AS DATE) = @Today

                    UNION ALL

                    -- Wholesale transactions
                    SELECT
                        'WHOLESALE' AS SaleType,
                        sr.SaleDate,
                        u.username AS HandledBy,
                        sr.SaleID,
                        sr.TotalAmount,
                        ISNULL(sr.IsRefunded, 0) AS IsRefunded
                    FROM SalesReport sr
                    INNER JOIN Users u ON sr.HandledBy = u.userID
                    WHERE CAST(sr.SaleDate AS DATE) = @Today
                )
                SELECT
                    MIN(SaleDate) AS BatchTime,
                    HandledBy AS Cashier,
                    COUNT(*) AS ItemCount,
                    SUM(CASE WHEN IsRefunded = 0 THEN TotalAmount ELSE 0 END) AS TotalAmount,
                    CASE WHEN MAX(CAST(IsRefunded AS INT)) = 1 THEN 1 ELSE 0 END AS HasRefunds,
                    STRING_AGG(CAST(SaleID AS VARCHAR), ',') AS SaleIDs
                FROM AllTransactions
                GROUP BY
                    DATEADD(MINUTE, DATEDIFF(MINUTE, 0, SaleDate), 0),
                    HandledBy
                ORDER BY BatchTime DESC"

            Using conn As New SqlConnection(connStr)
                Using da As New SqlDataAdapter(query, conn)
                    da.SelectCommand.Parameters.AddWithValue("@Today", today)

                    dt.Clear()
                    da.Fill(dt)

                    ' Add a computed column for display status BEFORE binding
                    If Not dt.Columns.Contains("StatusDisplay") Then
                        dt.Columns.Add("StatusDisplay", GetType(String))
                    End If

                    ' Populate the display column
                    For Each row As DataRow In dt.Rows
                        Dim hasRefundsInt As Integer = Convert.ToInt32(row("HasRefunds"))
                        row("StatusDisplay") = If(hasRefundsInt = 1, "HAS REFUNDS", "Active")
                    Next

                    ' Mark as batch view
                    isDetailView = False

                    ' Hide detail-only buttons
                    btnRefund.Visible = False
                    btnViewReceipt.Visible = False

                    ' Ensure any existing filters from detail view are cleared so rows are visible
                    If bs IsNot Nothing Then
                        bs.Filter = String.Empty
                    End If

                    ' Re-bind to update structure
                    dv = New DataView(dt)
                    bs.DataSource = dv
                    transactionsDataGridView.DataSource = bs

                    ' Format columns for batch view
                    If transactionsDataGridView.Columns.Count > 0 Then
                        ' First, hide all detail view columns if they exist
                        If transactionsDataGridView.Columns.Contains("SaleType") Then
                            transactionsDataGridView.Columns("SaleType").Visible = False
                        End If
                        If transactionsDataGridView.Columns.Contains("TransactionType") Then
                            transactionsDataGridView.Columns("TransactionType").Visible = False
                        End If
                        If transactionsDataGridView.Columns.Contains("SaleID") Then
                            transactionsDataGridView.Columns("SaleID").Visible = False
                        End If
                        If transactionsDataGridView.Columns.Contains("SaleDate") Then
                            transactionsDataGridView.Columns("SaleDate").Visible = False
                        End If
                        If transactionsDataGridView.Columns.Contains("ProductName") Then
                            transactionsDataGridView.Columns("ProductName").Visible = False
                        End If
                        If transactionsDataGridView.Columns.Contains("ProductID") Then
                            transactionsDataGridView.Columns("ProductID").Visible = False
                        End If
                        If transactionsDataGridView.Columns.Contains("Unit") Then
                            transactionsDataGridView.Columns("Unit").Visible = False
                        End If
                        If transactionsDataGridView.Columns.Contains("CategoryName") Then
                            transactionsDataGridView.Columns("CategoryName").Visible = False
                        End If
                        If transactionsDataGridView.Columns.Contains("QuantitySold") Then
                            transactionsDataGridView.Columns("QuantitySold").Visible = False
                        End If
                        If transactionsDataGridView.Columns.Contains("UnitPrice") Then
                            transactionsDataGridView.Columns("UnitPrice").Visible = False
                        End If
                        If transactionsDataGridView.Columns.Contains("PaymentMethod") Then
                            transactionsDataGridView.Columns("PaymentMethod").Visible = False
                        End If
                        If transactionsDataGridView.Columns.Contains("HandledBy") Then
                            transactionsDataGridView.Columns("HandledBy").Visible = False
                        End If
                        If transactionsDataGridView.Columns.Contains("IsRefunded") Then
                            transactionsDataGridView.Columns("IsRefunded").Visible = False
                        End If
                        If transactionsDataGridView.Columns.Contains("RefundStatus") Then
                            transactionsDataGridView.Columns("RefundStatus").Visible = False
                        End If
                        If transactionsDataGridView.Columns.Contains("RefundDate") Then
                            transactionsDataGridView.Columns("RefundDate").Visible = False
                        End If
                        If transactionsDataGridView.Columns.Contains("RefundReason") Then
                            transactionsDataGridView.Columns("RefundReason").Visible = False
                        End If
                        If transactionsDataGridView.Columns.Contains("PayerName") Then
                            transactionsDataGridView.Columns("PayerName").Visible = False
                        End If
                        If transactionsDataGridView.Columns.Contains("ReferenceNumber") Then
                            transactionsDataGridView.Columns("ReferenceNumber").Visible = False
                        End If
                        If transactionsDataGridView.Columns.Contains("BankName") Then
                            transactionsDataGridView.Columns("BankName").Visible = False
                        End If

                        With transactionsDataGridView
                            ' Show and configure batch view columns
                            .Columns("BatchTime").Visible = True
                            .Columns("BatchTime").HeaderText = "Transaction Time (Click to view details)"
                            .Columns("BatchTime").DefaultCellStyle.Format = "hh:mm:ss tt"
                            .Columns("BatchTime").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                            .Columns("BatchTime").FillWeight = 35

                            .Columns("Cashier").Visible = True
                            .Columns("Cashier").HeaderText = "Cashier"
                            .Columns("Cashier").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                            .Columns("Cashier").FillWeight = 25

                            .Columns("ItemCount").Visible = True
                            .Columns("ItemCount").HeaderText = "Items"
                            .Columns("ItemCount").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                            .Columns("ItemCount").FillWeight = 10

                            .Columns("TotalAmount").Visible = True
                            .Columns("TotalAmount").HeaderText = "Total Amount"
                            .Columns("TotalAmount").DefaultCellStyle.Format = "₱#,##0.00"
                            .Columns("TotalAmount").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                            .Columns("TotalAmount").FillWeight = 20

                            ' Use StatusDisplay for showing, hide HasRefunds
                            .Columns("StatusDisplay").Visible = True
                            .Columns("StatusDisplay").HeaderText = "Status"
                            .Columns("StatusDisplay").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                            .Columns("StatusDisplay").FillWeight = 10
                            .Columns("HasRefunds").Visible = False

                            ' Hide SaleIDs column (used internally)
                            .Columns("SaleIDs").Visible = False
                        End With

                        ' Color-code rows based on StatusDisplay
                        For Each row As DataGridViewRow In transactionsDataGridView.Rows
                            Dim statusDisplay As String = row.Cells("StatusDisplay").Value.ToString()

                            If statusDisplay = "HAS REFUNDS" Then
                                row.Cells("StatusDisplay").Style.BackColor = Color.FromArgb(255, 180, 180)
                                row.Cells("StatusDisplay").Style.ForeColor = Color.DarkRed
                                row.Cells("StatusDisplay").Style.Font = New Font("Segoe UI", 9, FontStyle.Bold)
                            Else
                                row.Cells("StatusDisplay").Style.BackColor = Color.FromArgb(200, 255, 200)
                                row.Cells("StatusDisplay").Style.ForeColor = Color.DarkGreen
                            End If
                        Next
                    End If

                    ' Update summary
                    UpdateSummary()
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show($"Error loading transactions: {ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' Load detailed view of products in a specific batch
    ''' </summary>
    Private Sub LoadBatchDetails(batchTime As DateTime, cashier As String)
        Try
            Dim connStr As String = GetConnectionString()

            ' Store current batch info
            currentBatchDate = batchTime
            currentBatchCashier = cashier

            ' Query to get all products in this batch (within same minute, same cashier)
            Dim query As String = "
                SELECT
                    'RETAIL' AS SaleType,
                    'In-Store' AS TransactionType,
                    sr.SaleID,
                    sr.SaleDate,
                    rp.ProductName,
                    rp.ProductID,
                    rp.unit AS Unit,
                    c.CategoryName,
                    sr.QuantitySold,
                    sr.UnitPrice,
                    sr.TotalAmount,
                    sr.PaymentMethod,
                    u.username AS HandledBy,
                    ISNULL(sr.IsRefunded, 0) AS IsRefunded,
                    sr.RefundDate,
                    sr.RefundReason,
                    sr.PayerName,
                    sr.ReferenceNumber,
                    sr.BankName
                FROM RetailSalesReport sr
                INNER JOIN retailProducts rp ON sr.ProductID = rp.ProductID
                INNER JOIN Categories c ON sr.CategoryID = c.CategoryID
                INNER JOIN Users u ON sr.HandledBy = u.userID
                WHERE DATEADD(MINUTE, DATEDIFF(MINUTE, 0, sr.SaleDate), 0) = DATEADD(MINUTE, DATEDIFF(MINUTE, 0, @BatchTime), 0)
                AND u.username = @Cashier

                UNION ALL

                SELECT
                    'WHOLESALE' AS SaleType,
                    CASE
                        WHEN sr.IsDelivery = 1 THEN 'Delivery'
                        WHEN sr.IsDelivery = 0 THEN 'Pickup'
                        ELSE 'In-Store'
                    END AS TransactionType,
                    sr.SaleID,
                    sr.SaleDate,
                    wp.ProductName,
                    wp.ProductID,
                    wp.unit AS Unit,
                    c.CategoryName,
                    sr.QuantitySold,
                    sr.UnitPrice,
                    sr.TotalAmount,
                    sr.PaymentMethod,
                    u.username AS HandledBy,
                    ISNULL(sr.IsRefunded, 0) AS IsRefunded,
                    sr.RefundDate,
                    sr.RefundReason,
                    sr.PayerName,
                    sr.ReferenceNumber,
                    sr.BankName
                FROM SalesReport sr
                INNER JOIN wholesaleProducts wp ON sr.ProductID = wp.ProductID
                INNER JOIN Categories c ON sr.CategoryID = c.CategoryID
                INNER JOIN Users u ON sr.HandledBy = u.userID
                WHERE DATEADD(MINUTE, DATEDIFF(MINUTE, 0, sr.SaleDate), 0) = DATEADD(MINUTE, DATEDIFF(MINUTE, 0, @BatchTime), 0)
                AND u.username = @Cashier

                ORDER BY SaleDate, SaleType"

            Using conn As New SqlConnection(connStr)
                Using da As New SqlDataAdapter(query, conn)
                    da.SelectCommand.Parameters.AddWithValue("@BatchTime", batchTime)
                    da.SelectCommand.Parameters.AddWithValue("@Cashier", cashier)

                    dt.Clear()
                    da.Fill(dt)

                    ' Add computed column for display status
                    If Not dt.Columns.Contains("RefundStatus") Then
                        dt.Columns.Add("RefundStatus", GetType(String))
                    End If

                    For Each row As DataRow In dt.Rows
                        Dim isRefunded As Boolean = If(IsDBNull(row("IsRefunded")), False, Convert.ToBoolean(row("IsRefunded")))
                        row("RefundStatus") = If(isRefunded, "REFUNDED", "Active")
                    Next

                    ' Mark as detail view
                    isDetailView = True

                    ' Show detail-only buttons
                    btnRefund.Visible = True
                    btnViewReceipt.Visible = True

                    ' Re-bind
                    dv = New DataView(dt)
                    bs.DataSource = dv
                    transactionsDataGridView.DataSource = bs

                    ' Format columns for detail view
                    If transactionsDataGridView.Columns.Count > 0 Then
                        With transactionsDataGridView
                            ' Show detail view columns
                            .Columns("SaleType").Visible = True
                            .Columns("SaleType").HeaderText = "Type"
                            .Columns("SaleType").Width = 80

                            .Columns("TransactionType").Visible = True
                            .Columns("TransactionType").HeaderText = "Mode"
                            .Columns("TransactionType").Width = 80

                            .Columns("SaleID").Visible = True
                            .Columns("SaleID").HeaderText = "Sale ID"
                            .Columns("SaleID").Width = 60

                            .Columns("SaleDate").Visible = True
                            .Columns("SaleDate").HeaderText = "Time"
                            .Columns("SaleDate").DefaultCellStyle.Format = "hh:mm:ss tt"
                            .Columns("SaleDate").Width = 90

                            .Columns("ProductName").Visible = True
                            .Columns("ProductName").HeaderText = "Product"
                            .Columns("ProductName").Width = 180

                            .Columns("ProductID").Visible = False
                            .Columns("IsRefunded").Visible = False

                            .Columns("Unit").Visible = True
                            .Columns("Unit").HeaderText = "Unit"
                            .Columns("Unit").Width = 60

                            .Columns("CategoryName").Visible = True
                            .Columns("CategoryName").HeaderText = "Category"
                            .Columns("CategoryName").Width = 90

                            .Columns("QuantitySold").Visible = True
                            .Columns("QuantitySold").HeaderText = "Qty"
                            .Columns("QuantitySold").Width = 50

                            .Columns("UnitPrice").Visible = True
                            .Columns("UnitPrice").HeaderText = "Unit Price"
                            .Columns("UnitPrice").DefaultCellStyle.Format = "₱#,##0.00"
                            .Columns("UnitPrice").Width = 90

                            .Columns("TotalAmount").Visible = True
                            .Columns("TotalAmount").HeaderText = "Total"
                            .Columns("TotalAmount").DefaultCellStyle.Format = "₱#,##0.00"
                            .Columns("TotalAmount").Width = 90

                            .Columns("PaymentMethod").Visible = True
                            .Columns("PaymentMethod").HeaderText = "Payment"
                            .Columns("PaymentMethod").Width = 90

                            .Columns("HandledBy").Visible = True
                            .Columns("HandledBy").HeaderText = "Cashier"
                            .Columns("HandledBy").Width = 90

                            .Columns("RefundStatus").Visible = True
                            .Columns("RefundStatus").HeaderText = "Status"
                            .Columns("RefundStatus").Width = 80

                            .Columns("RefundDate").Visible = True
                            .Columns("RefundDate").HeaderText = "Refund Date"
                            .Columns("RefundDate").DefaultCellStyle.Format = "MM/dd/yyyy hh:mm tt"
                            .Columns("RefundDate").Width = 130

                            .Columns("RefundReason").Visible = True
                            .Columns("RefundReason").HeaderText = "Refund Reason"
                            .Columns("RefundReason").Width = 150

                            .Columns("PayerName").Visible = False
                            .Columns("ReferenceNumber").Visible = False
                            .Columns("BankName").Visible = False

                            ' Hide batch view columns that don't apply to detail view
                            If .Columns.Contains("BatchTime") Then
                                .Columns("BatchTime").Visible = False
                            End If
                            If .Columns.Contains("Cashier") Then
                                .Columns("Cashier").Visible = False
                            End If
                            If .Columns.Contains("ItemCount") Then
                                .Columns("ItemCount").Visible = False
                            End If
                            If .Columns.Contains("StatusDisplay") Then
                                .Columns("StatusDisplay").Visible = False
                            End If
                            If .Columns.Contains("SaleIDs") Then
                                .Columns("SaleIDs").Visible = False
                            End If
                        End With

                        ' Color-code the rows
                        For Each row As DataGridViewRow In transactionsDataGridView.Rows
                            ' Color-code SaleType
                            If row.Cells("SaleType").Value IsNot Nothing Then
                                If row.Cells("SaleType").Value.ToString() = "RETAIL" Then
                                    row.Cells("SaleType").Style.BackColor = Color.LightBlue
                                    row.Cells("SaleType").Style.ForeColor = Color.DarkBlue
                                Else
                                    row.Cells("SaleType").Style.BackColor = Color.LightGreen
                                    row.Cells("SaleType").Style.ForeColor = Color.DarkGreen
                                End If
                            End If

                            ' Color-code TransactionType
                            If row.Cells("TransactionType").Value IsNot Nothing Then
                                Dim transType As String = row.Cells("TransactionType").Value.ToString()
                                Select Case transType
                                    Case "Delivery"
                                        row.Cells("TransactionType").Style.BackColor = Color.FromArgb(255, 220, 180)
                                        row.Cells("TransactionType").Style.ForeColor = Color.DarkOrange
                                    Case "Pickup"
                                        row.Cells("TransactionType").Style.BackColor = Color.FromArgb(255, 255, 200)
                                        row.Cells("TransactionType").Style.ForeColor = Color.DarkGoldenrod
                                    Case "In-Store"
                                        row.Cells("TransactionType").Style.BackColor = Color.FromArgb(200, 255, 200)
                                        row.Cells("TransactionType").Style.ForeColor = Color.DarkGreen
                                End Select
                            End If

                            ' Color-code RefundStatus
                            If row.Cells("RefundStatus").Value IsNot Nothing Then
                                Dim refundStatus As String = row.Cells("RefundStatus").Value.ToString()
                                If refundStatus = "REFUNDED" Then
                                    row.Cells("RefundStatus").Style.BackColor = Color.FromArgb(255, 180, 180)
                                    row.Cells("RefundStatus").Style.ForeColor = Color.DarkRed
                                    row.Cells("RefundStatus").Style.Font = New Font("Segoe UI", 9, FontStyle.Bold)

                                    For Each cell As DataGridViewCell In row.Cells
                                        cell.Style.Font = New Font("Segoe UI", 9, FontStyle.Strikeout)
                                        cell.Style.ForeColor = Color.Gray
                                    Next
                                Else
                                    row.Cells("RefundStatus").Style.BackColor = Color.FromArgb(200, 255, 200)
                                    row.Cells("RefundStatus").Style.ForeColor = Color.DarkGreen
                                End If
                            End If
                        Next
                    End If

                    ' Update title to show batch info
                    lblTitle.Text = $"Batch Details - {batchTime:hh:mm:ss tt} by {cashier} (Click Refresh to go back)"
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show($"Error loading batch details: {ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub UpdateSummary()
        ' Summary logic differs based on view
        If Not isDetailView Then
            ' Batch view summary
            Try
                Dim totalBatches As Integer = dt.Rows.Count
                Dim totalRevenue As Decimal = 0D
                Dim totalItems As Integer = 0

                For Each row As DataRow In dt.Rows
                    If Not IsDBNull(row("TotalAmount")) Then
                        totalRevenue += Convert.ToDecimal(row("TotalAmount"))
                    End If
                    If Not IsDBNull(row("ItemCount")) Then
                        totalItems += Convert.ToInt32(row("ItemCount"))
                    End If
                Next

                lblTotalTransactions.Text = $"Total Batches: {totalBatches}"
                lblTotalRevenue.Text = $"Total Sales: ₱{totalRevenue:N2}"
                lblRetailCount.Text = $"Total Items: {totalItems}"
                lblWholesaleCount.Text = "Click a batch to view details"
            Catch ex As Exception
                Console.WriteLine($"Error updating summary: {ex.Message}")
            End Try
        Else
            ' Detail view summary
            Try
                Dim totalItems As Integer = dt.Rows.Count
                Dim totalRevenue As Decimal = 0D
                Dim retailCount As Integer = 0
                Dim wholesaleCount As Integer = 0
                Dim refundedCount As Integer = 0

                For Each row As DataRow In dt.Rows
                    Dim isRefunded As Boolean = If(IsDBNull(row("IsRefunded")), False, Convert.ToBoolean(row("IsRefunded")))

                    If Not isRefunded Then
                        If Not IsDBNull(row("TotalAmount")) Then
                            totalRevenue += Convert.ToDecimal(row("TotalAmount"))
                        End If

                        If Not IsDBNull(row("SaleType")) Then
                            If row("SaleType").ToString() = "RETAIL" Then
                                retailCount += 1
                            Else
                                wholesaleCount += 1
                            End If
                        End If
                    Else
                        refundedCount += 1
                    End If
                Next

                Dim activeItems As Integer = totalItems - refundedCount
                lblTotalTransactions.Text = $"Items in Batch: {activeItems}"
                lblTotalRevenue.Text = $"Batch Total: ₱{totalRevenue:N2}"
                lblRetailCount.Text = $"Retail: {retailCount} items"
                lblWholesaleCount.Text = $"Wholesale: {wholesaleCount} items"
            Catch ex As Exception
                Console.WriteLine($"Error updating summary: {ex.Message}")
            End Try
        End If
    End Sub

    Private Function GetConnectionString() As String
        Return SharedUtilities.GetConnectionString()
    End Function

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        ' Refresh always goes back to batch view
        LoadBatchTransactions()
        lblTitle.Text = $"Daily Transactions - {DateTime.Today:dddd, MMMM dd, yyyy}"

        ' Ensure filters/reset UI state do not hide data after refresh
        If bs IsNot Nothing Then
            bs.Filter = String.Empty
        End If
        If cboSalesType IsNot Nothing Then
            cboSalesType.SelectedIndex = 0
        End If

        ' Update summary after refresh
        UpdateSummary()

        MessageBox.Show("Transactions refreshed!", "Refresh",
            MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnViewReceipt_Click(sender As Object, e As EventArgs) Handles btnViewReceipt.Click
        If Not isDetailView Then
            MessageBox.Show("Please select a batch first to view receipts.", "No Batch Selected",
                MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        ' Show receipt for entire batch
        ShowBatchReceipt(currentBatchDate, currentBatchCashier)
    End Sub

    Private Sub cboSalesType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboSalesType.SelectedIndexChanged
        ' Filtering only works in detail view
        If Not isDetailView Then
            Return
        End If

        Select Case cboSalesType.SelectedIndex
            Case 0 ' All Transactions
                bs.Filter = ""
            Case 1 ' Retail Only
                bs.Filter = "SaleType = 'RETAIL'"
            Case 2 ' Wholesale Only
                bs.Filter = "SaleType = 'WHOLESALE'"
            Case 3 ' Refunded Only
                bs.Filter = "IsRefunded = True"
            Case 4 ' Non-Refunded Only
                bs.Filter = "IsRefunded = False"
        End Select

        UpdateSummary()
    End Sub

    Private Sub btnRefund_Click(sender As Object, e As EventArgs) Handles btnRefund.Click
        If Not isDetailView Then
            MessageBox.Show("Please select a batch first, then select an item to refund.", "No Batch Selected",
                MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If transactionsDataGridView.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select a transaction to refund.", "No Selection",
                MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim selectedRow As DataGridViewRow = transactionsDataGridView.SelectedRows(0)

        Dim saleType As String = selectedRow.Cells("SaleType").Value.ToString()
        Dim saleID As Integer = Convert.ToInt32(selectedRow.Cells("SaleID").Value)
        Dim productID As Integer = Convert.ToInt32(selectedRow.Cells("ProductID").Value)
        Dim productName As String = selectedRow.Cells("ProductName").Value.ToString()
        Dim quantitySold As Integer = Convert.ToInt32(selectedRow.Cells("QuantitySold").Value)
        Dim totalAmount As Decimal = Convert.ToDecimal(selectedRow.Cells("TotalAmount").Value)

        Dim isRefunded As Boolean = If(IsDBNull(selectedRow.Cells("IsRefunded").Value), False,
            Convert.ToBoolean(selectedRow.Cells("IsRefunded").Value))

        If isRefunded Then
            MessageBox.Show("This transaction has already been refunded.", "Already Refunded",
                MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim confirmMsg As String = $"Refund Transaction Details:" & vbCrLf & vbCrLf &
            $"Type: {saleType}" & vbCrLf &
            $"Product: {productName}" & vbCrLf &
            $"Quantity: {quantitySold}" & vbCrLf &
            $"Amount: ₱{totalAmount:N2}" & vbCrLf & vbCrLf &
            "This will:" & vbCrLf &
            $"1. Mark the transaction as refunded" & vbCrLf &
            $"2. Restore {quantitySold} units to inventory" & vbCrLf &
            $"3. Update revenue calculations" & vbCrLf & vbCrLf &
            "Do you want to proceed with the refund?"

        If MessageBox.Show(confirmMsg, "Confirm Refund", MessageBoxButtons.YesNo,
            MessageBoxIcon.Question) = DialogResult.No Then
            Return
        End If

        Dim refundReason As String = InputBox("Please provide a reason for this refund:",
            "Refund Reason", "Customer request")

        If String.IsNullOrWhiteSpace(refundReason) Then
            MessageBox.Show("Refund reason is required.", "Required Field",
                MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If ProcessRefund(saleType, saleID, productID, quantitySold, refundReason) Then
            MessageBox.Show("Refund processed successfully!" & vbCrLf &
                $"Product quantity restored: +{quantitySold}" & vbCrLf &
                $"Amount refunded: ₱{totalAmount:N2}",
                "Refund Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)

            ' Reload batch details
            LoadBatchDetails(currentBatchDate, currentBatchCashier)
        Else
            MessageBox.Show("Failed to process refund. Please try again.", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    Private Function ProcessRefund(saleType As String, saleID As Integer, productID As Integer,
             quantitySold As Integer, refundReason As String) As Boolean
        Try
            Dim connStr As String = GetConnectionString()

            Using conn As New SqlConnection(connStr)
                conn.Open()

                Using transaction As SqlTransaction = conn.BeginTransaction()
                    Try
                        Dim tableName As String = If(saleType = "RETAIL", "RetailSalesReport", "SalesReport")

                        ' For wholesale deliveries, also set DeliveryStatus to 'Cancelled'
                        Dim updateQuery As String
                        If saleType = "WHOLESALE" Then
                            updateQuery = $"
                                UPDATE {tableName}
                                SET IsRefunded = 1,
                                    RefundDate = GETDATE(),
                                    RefundReason = @RefundReason,
                                    DeliveryStatus = CASE WHEN IsDelivery = 1 THEN 'Cancelled' ELSE DeliveryStatus END
                                WHERE SaleID = @SaleID"
                        Else
                            updateQuery = $"
                                UPDATE {tableName}
                                SET IsRefunded = 1,
                                    RefundDate = GETDATE(),
                                    RefundReason = @RefundReason
                                WHERE SaleID = @SaleID"
                        End If

                        Using cmd As New SqlCommand(updateQuery, conn, transaction)
                            cmd.Parameters.AddWithValue("@RefundReason", refundReason)
                            cmd.Parameters.AddWithValue("@SaleID", saleID)
                            cmd.ExecuteNonQuery()
                        End Using

                        Dim productTableName As String = If(saleType = "RETAIL", "retailProducts", "wholesaleProducts")
                        Dim restoreQuery As String = $"
                            UPDATE {productTableName}
                            SET StockQuantity = StockQuantity + @QuantityToRestore
                            WHERE ProductID = @ProductID"

                        Using cmd As New SqlCommand(restoreQuery, conn, transaction)
                            cmd.Parameters.AddWithValue("@QuantityToRestore", quantitySold)
                            cmd.Parameters.AddWithValue("@ProductID", productID)
                            cmd.ExecuteNonQuery()
                        End Using

                        transaction.Commit()
                        Return True
                    Catch ex As Exception
                        transaction.Rollback()
                        Console.WriteLine($"Refund transaction error: {ex.Message}")
                        Return False
                    End Try
                End Using
            End Using
        Catch ex As Exception
            Console.WriteLine($"Refund processing error: {ex.Message}")
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Handles single-click event on DataGridView for batch selection
    ''' </summary>
    Private Sub TransactionsDataGridView_CellClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex < 0 Then
            Return
        End If

        ' Only handle single-click in batch view to load details
        If Not isDetailView Then
            Dim selectedRow As DataGridViewRow = transactionsDataGridView.Rows(e.RowIndex)
            Dim batchTime As DateTime = Convert.ToDateTime(selectedRow.Cells("BatchTime").Value)
            Dim cashier As String = selectedRow.Cells("Cashier").Value.ToString()

            LoadBatchDetails(batchTime, cashier)
        End If
    End Sub

    ''' <summary>
    ''' Handles double-click event on DataGridView to show receipt
    ''' </summary>
    Private Sub TransactionsDataGridView_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex < 0 Then
            Return
        End If

        ' Only handle double-click in detail view to show single item receipt
        If isDetailView Then
            Dim selectedRow As DataGridViewRow = transactionsDataGridView.Rows(e.RowIndex)
            Dim saleType As String = selectedRow.Cells("SaleType").Value.ToString()
            Dim saleID As Integer = Convert.ToInt32(selectedRow.Cells("SaleID").Value)

            ShowTransactionReceipt(saleType, saleID)
        End If
    End Sub

    ''' <summary>
    ''' Shows receipt for entire batch (all products purchased together)
    ''' </summary>
    Private Sub ShowBatchReceipt(batchTime As DateTime, cashier As String)
        Try
            Dim connStr As String = GetConnectionString()
            Dim receiptContent As New System.Text.StringBuilder()

            ' Get all transactions in this batch
            Dim batchItems As New List(Of Dictionary(Of String, Object))

            Using conn As New SqlConnection(connStr)
                conn.Open()

                ' Get retail items
                Dim retailQuery As String = "
                    SELECT
                        'RETAIL' AS SaleType,
                        sr.SaleID,
                        sr.SaleDate,
                        rp.ProductName,
                        rp.unit AS Unit,
                        c.CategoryName,
                        sr.QuantitySold,
                        sr.UnitPrice,
                        sr.TotalAmount,
                        sr.PaymentMethod,
                        ISNULL(sr.IsRefunded, 0) AS IsRefunded,
                        sr.RefundDate,
                        sr.RefundReason,
                        sr.PayerName,
                        sr.ReferenceNumber,
                        sr.BankName
                    FROM RetailSalesReport sr
                    INNER JOIN retailProducts rp ON sr.ProductID = rp.ProductID
                    INNER JOIN Categories c ON sr.CategoryID = c.CategoryID
                    WHERE DATEADD(MINUTE, DATEDIFF(MINUTE, 0, sr.SaleDate), 0) = DATEADD(MINUTE, DATEDIFF(MINUTE, 0, @BatchTime), 0)
                    AND sr.HandledBy = (SELECT userID FROM Users WHERE username = @Cashier)"

                Dim wholesaleQuery As String = "
                    SELECT
                        'WHOLESALE' AS SaleType,
                        sr.SaleID,
                        sr.SaleDate,
                        wp.ProductName,
                        wp.unit AS Unit,
                        c.CategoryName,
                        sr.QuantitySold,
                        sr.UnitPrice,
                        sr.TotalAmount,
                        sr.PaymentMethod,
                        ISNULL(sr.IsRefunded, 0) AS IsRefunded,
                        sr.RefundDate,
                        sr.RefundReason,
                        CASE
                            WHEN sr.IsDelivery = 1 THEN 'Delivery'
                            WHEN sr.IsDelivery = 0 THEN 'Pickup'
                            ELSE 'In-Store'
                        END AS TransactionType,
                        sr.DeliveryAddress,
                        sr.PayerName,
                        sr.ReferenceNumber,
                        sr.BankName
                    FROM SalesReport sr
                    INNER JOIN wholesaleProducts wp ON sr.ProductID = wp.ProductID
                    INNER JOIN Categories c ON sr.CategoryID = c.CategoryID
                    WHERE DATEADD(MINUTE, DATEDIFF(MINUTE, 0, sr.SaleDate), 0) = DATEADD(MINUTE, DATEDIFF(MINUTE, 0, @BatchTime), 0)
                    AND sr.HandledBy = (SELECT userID FROM Users WHERE username = @Cashier)"

                ' Collect all items
                Using cmd As New SqlCommand(retailQuery, conn)
                    cmd.Parameters.AddWithValue("@BatchTime", batchTime)
                    cmd.Parameters.AddWithValue("@Cashier", cashier)

                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            Dim item As New Dictionary(Of String, Object)
                            For i As Integer = 0 To reader.FieldCount - 1
                                item(reader.GetName(i)) = reader.GetValue(i)
                            Next
                            batchItems.Add(item)
                        End While
                    End Using
                End Using

                Using cmd As New SqlCommand(wholesaleQuery, conn)
                    cmd.Parameters.AddWithValue("@BatchTime", batchTime)
                    cmd.Parameters.AddWithValue("@Cashier", cashier)

                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            Dim item As New Dictionary(Of String, Object)
                            For i As Integer = 0 To reader.FieldCount - 1
                                item(reader.GetName(i)) = reader.GetValue(i)
                            Next
                            batchItems.Add(item)
                        End While
                    End Using
                End Using
            End Using

            ' Build receipt
            receiptContent.AppendLine("================================")
            receiptContent.AppendLine("   INVENTORY SYSTEM POS")
            receiptContent.AppendLine("   BATCH TRANSACTION RECEIPT")
            receiptContent.AppendLine("================================")
            receiptContent.AppendLine()
            receiptContent.AppendLine($"Date: {batchTime:MMM dd, yyyy}")
            receiptContent.AppendLine($"Time: {batchTime:hh:mm:ss tt}")
            receiptContent.AppendLine($"Cashier: {cashier}")
            receiptContent.AppendLine($"Total Items: {batchItems.Count}")
            receiptContent.AppendLine()
            receiptContent.AppendLine("--------------------------------")
            receiptContent.AppendLine("ITEMS PURCHASED")
            receiptContent.AppendLine("--------------------------------")
            receiptContent.AppendLine()

            Dim grandTotal As Decimal = 0D
            Dim hasRefunds As Boolean = False

            For Each item In batchItems
                Dim productName As String = item("ProductName").ToString()
                If productName.Length > 28 Then
                    productName = productName.Substring(0, 25) & "..."
                End If

                Dim isRefunded As Boolean = Convert.ToBoolean(item("IsRefunded"))
                Dim saleType As String = item("SaleType").ToString()
                Dim qty As Integer = Convert.ToInt32(item("QuantitySold"))
                Dim unitPrice As Decimal = Convert.ToDecimal(item("UnitPrice"))
                Dim totalAmount As Decimal = Convert.ToDecimal(item("TotalAmount"))

                If isRefunded Then
                    hasRefunds = True
                    receiptContent.AppendLine($"[REFUNDED] {productName}")
                Else
                    receiptContent.AppendLine($"{productName} ({saleType})")
                    grandTotal += totalAmount
                End If

                receiptContent.AppendLine($"  {qty} x ₱{unitPrice:N2} = ₱{totalAmount:N2}")
                receiptContent.AppendLine()
            Next

            receiptContent.AppendLine("================================")
            receiptContent.AppendLine($"TOTAL AMOUNT: ₱{grandTotal:N2}")
            receiptContent.AppendLine("================================")

            If hasRefunds Then
                receiptContent.AppendLine()
                receiptContent.AppendLine("⚠ Some items in this batch have been refunded.")
            End If

            receiptContent.AppendLine()
            receiptContent.AppendLine("================================")
            receiptContent.AppendLine("  Thank you for your purchase!")
            receiptContent.AppendLine("================================")

            MessageBox.Show(receiptContent.ToString(), $"Batch Receipt - {batchTime:hh:mm:ss tt}",
                MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show($"Error displaying batch receipt: {ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' Shows receipt for single item (used when double-clicking in detail view)
    ''' </summary>
    Private Sub ShowTransactionReceipt(saleType As String, saleID As Integer)
        Try
            Dim connStr As String = GetConnectionString()
            Dim receiptContent As New System.Text.StringBuilder()

            Using conn As New SqlConnection(connStr)
                conn.Open()

                Dim query As String = ""
                If saleType = "RETAIL" Then
                    query = "
                        SELECT
                            sr.SaleID,
                            sr.SaleDate,
                            rp.ProductName,
                            rp.unit AS Unit,
                            c.CategoryName,
                            sr.QuantitySold,
                            sr.UnitPrice,
                            sr.TotalAmount,
                            sr.PaymentMethod,
                            u.username AS HandledBy,
                            ISNULL(sr.IsRefunded, 0) AS IsRefunded,
                            sr.RefundDate,
                            sr.RefundReason,
                            sr.PayerName,
                            sr.ReferenceNumber,
                            sr.BankName
                        FROM RetailSalesReport sr
                        INNER JOIN retailProducts rp ON sr.ProductID = rp.ProductID
                        INNER JOIN Categories c ON sr.CategoryID = c.CategoryID
                        INNER JOIN Users u ON sr.HandledBy = u.userID
                        WHERE sr.SaleID = @SaleID"
                Else
                    query = "
                        SELECT
                            sr.SaleID,
                            sr.SaleDate,
                            wp.ProductName,
                            wp.unit AS Unit,
                            c.CategoryName,
                            sr.QuantitySold,
                            sr.UnitPrice,
                            sr.TotalAmount,
                            sr.PaymentMethod,
                            u.username AS HandledBy,
                            ISNULL(sr.IsRefunded, 0) AS IsRefunded,
                            sr.RefundDate,
                            sr.RefundReason,
                            CASE
                                WHEN sr.IsDelivery = 1 THEN 'Delivery'
                                WHEN sr.IsDelivery = 0 THEN 'Pickup'
                                ELSE 'In-Store'
                            END AS TransactionType,
                            sr.DeliveryAddress,
                            sr.PayerName,
                            sr.ReferenceNumber,
                            sr.BankName
                        FROM SalesReport sr
                        INNER JOIN wholesaleProducts wp ON sr.ProductID = wp.ProductID
                        INNER JOIN Categories c ON sr.CategoryID = c.CategoryID
                        INNER JOIN Users u ON sr.HandledBy = u.userID
                        WHERE sr.SaleID = @SaleID"
                End If

                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@SaleID", saleID)

                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            receiptContent.AppendLine("================================")
                            receiptContent.AppendLine("   INVENTORY SYSTEM POS")
                            receiptContent.AppendLine("   TRANSACTION RECEIPT")
                            receiptContent.AppendLine("================================")
                            receiptContent.AppendLine()

                            Dim isRefunded As Boolean = Convert.ToBoolean(reader("IsRefunded"))
                            If isRefunded Then
                                receiptContent.AppendLine("*** REFUNDED TRANSACTION ***")
                                receiptContent.AppendLine()
                            End If

                            receiptContent.AppendLine($"Type: {saleType}")
                            receiptContent.AppendLine($"Receipt #: {reader("SaleID")}")
                            receiptContent.AppendLine($"Date: {Convert.ToDateTime(reader("SaleDate")):MMM dd, yyyy}")
                            receiptContent.AppendLine($"Time: {Convert.ToDateTime(reader("SaleDate")):hh:mm:ss tt}")
                            receiptContent.AppendLine($"Cashier: {reader("HandledBy")}")

                            If saleType = "WHOLESALE" Then
                                receiptContent.AppendLine($"Mode: {reader("TransactionType")}")
                                If Not IsDBNull(reader("DeliveryAddress")) Then
                                    receiptContent.AppendLine($"Delivery Address: {reader("DeliveryAddress")}")
                                End If
                            End If

                            receiptContent.AppendLine()
                            receiptContent.AppendLine("--------------------------------")
                            receiptContent.AppendLine("ITEM DETAILS")
                            receiptContent.AppendLine("--------------------------------")
                            receiptContent.AppendLine()

                            Dim productName As String = reader("ProductName").ToString()
                            If productName.Length > 30 Then
                                productName = productName.Substring(0, 27) & "..."
                            End If
                            receiptContent.AppendLine($"Product: {productName}")
                            receiptContent.AppendLine($"Category: {reader("CategoryName")}")
                            receiptContent.AppendLine($"Unit: {reader("Unit")}")
                            receiptContent.AppendLine()

                            Dim quantity As Integer = Convert.ToInt32(reader("QuantitySold"))
                            Dim unitPrice As Decimal = Convert.ToDecimal(reader("UnitPrice"))
                            Dim totalAmount As Decimal = Convert.ToDecimal(reader("TotalAmount"))

                            receiptContent.AppendLine($"Quantity: {quantity}")
                            receiptContent.AppendLine($"Unit Price: ₱{unitPrice:N2}")
                            receiptContent.AppendLine()
                            receiptContent.AppendLine("--------------------------------")
                            receiptContent.AppendLine($"TOTAL AMOUNT: ₱{totalAmount:N2}")
                            receiptContent.AppendLine("--------------------------------")
                            receiptContent.AppendLine()
                            receiptContent.AppendLine($"Payment Method: {reader("PaymentMethod")}")

                            Dim paymentMethod As String = reader("PaymentMethod").ToString()
                            If paymentMethod = "GCash" OrElse paymentMethod = "Bank Transaction" Then
                                If Not IsDBNull(reader("PayerName")) OrElse Not IsDBNull(reader("ReferenceNumber")) Then
                                    receiptContent.AppendLine()
                                    receiptContent.AppendLine("--------------------------------")
                                    receiptContent.AppendLine("PAYMENT DETAILS")
                                    receiptContent.AppendLine("--------------------------------")

                                    If Not IsDBNull(reader("PayerName")) Then
                                        receiptContent.AppendLine($"Payer Name: {reader("PayerName")}")
                                    End If

                                    If Not IsDBNull(reader("ReferenceNumber")) Then
                                        receiptContent.AppendLine($"Reference #: {reader("ReferenceNumber")}")
                                    End If

                                    If paymentMethod = "Bank Transaction" AndAlso Not IsDBNull(reader("BankName")) Then
                                        receiptContent.AppendLine($"Bank: {reader("BankName")}")
                                    End If
                                End If
                            End If

                            If isRefunded Then
                                receiptContent.AppendLine()
                                receiptContent.AppendLine("================================")
                                receiptContent.AppendLine("REFUND INFORMATION")
                                receiptContent.AppendLine("================================")
                                If Not IsDBNull(reader("RefundDate")) Then
                                    receiptContent.AppendLine($"Refund Date: {Convert.ToDateTime(reader("RefundDate")):MMM dd, yyyy hh:mm tt}")
                                End If
                                If Not IsDBNull(reader("RefundReason")) Then
                                    receiptContent.AppendLine($"Reason: {reader("RefundReason")}")
                                End If
                                receiptContent.AppendLine($"Refunded Amount: ₱{totalAmount:N2}")
                            End If

                            receiptContent.AppendLine()
                            receiptContent.AppendLine("================================")
                            receiptContent.AppendLine("  Thank you for your purchase!")
                            receiptContent.AppendLine("================================")

                            Dim receiptTitle As String = If(isRefunded,
                                $"Transaction Receipt (REFUNDED) - #{saleID}",
                                $"Transaction Receipt - #{saleID}")

                            MessageBox.Show(receiptContent.ToString(), receiptTitle,
                                MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Else
                            MessageBox.Show("Transaction not found.", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show($"Error displaying receipt: {ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' UpdateFilteredSummary method closing
    ''' <summary>
    ''' Updates the summary labels based on the current filter
    ''' </summary>
    Private Sub UpdateFilteredSummary()
        ' Summary logic for filtered view (detail view only)
        If isDetailView Then
            Try
                Dim totalRevenue As Decimal = 0D
                Dim retailCount As Integer = 0
                Dim wholesaleCount As Integer = 0
                Dim refundedCount As Integer = 0
                Dim totalTransactions As Integer = dt.Rows.Count

                For Each row As DataRow In dt.Rows
                    Dim isRefunded As Boolean = If(IsDBNull(row("IsRefunded")), False, Convert.ToBoolean(row("IsRefunded")))

                    If Not isRefunded Then
                        If Not IsDBNull(row("TotalAmount")) Then
                            totalRevenue += Convert.ToDecimal(row("TotalAmount"))
                        End If

                        If Not IsDBNull(row("SaleType")) Then
                            If row("SaleType").ToString() = "RETAIL" Then
                                retailCount += 1
                            Else
                                wholesaleCount += 1
                            End If
                        End If
                    Else
                        refundedCount += 1
                    End If
                Next

                Dim activeItems As Integer = totalTransactions - refundedCount
                lblTotalTransactions.Text = $"Items in Batch: {activeItems}"
                lblTotalRevenue.Text = $"Batch Total: ₱{totalRevenue:N2}"
                lblRetailCount.Text = $"Retail: {retailCount} items"
                lblWholesaleCount.Text = $"Wholesale: {wholesaleCount} items"

                ' Update labels based on filter
                If cboSalesType.SelectedIndex = 3 Then
                    ' Showing refunded only
                    lblTotalTransactions.Text = $"Refunded Transactions: {refundedCount}"
                    lblTotalRevenue.Text = $"Refunded Amount: ₱{totalRevenue:N2}"
                    lblRetailCount.Text = ""
                    lblWholesaleCount.Text = ""
                Else
                    ' Showing active transactions
                    lblTotalTransactions.Text = $"Total Transactions: {totalTransactions}"
                    lblTotalRevenue.Text = $"Total Sales: ₱{totalRevenue:N2}"
                    lblRetailCount.Text = $"Retail: {retailCount} transactions"
                    lblWholesaleCount.Text = $"Wholesale: {wholesaleCount} transactions"
                End If
            Catch ex As Exception
                Console.WriteLine($"Error updating filtered summary: {ex.Message}")
            End Try
        End If
    End Sub

End Class