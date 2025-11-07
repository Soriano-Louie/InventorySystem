Imports Microsoft.Data.SqlClient
Imports System.Drawing.Drawing2D

Public Class DailyTransactionsForm
    Private parentForm As posForm
    Private dt As New DataTable()
    Private dv As New DataView()
    Private bs As New BindingSource()

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

        ' Setup buttons
        btnRefresh.BackColor = Color.FromArgb(147, 53, 53)
        btnRefresh.ForeColor = Color.FromArgb(230, 216, 177)
        btnRefresh.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        btnRefresh.FlatStyle = FlatStyle.Flat
        btnRefresh.Cursor = Cursors.Hand

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
        cboSalesType.SelectedIndex = 0

        ' Initialize data binding
        dt = New DataTable()
        dv = New DataView(dt)
        bs = New BindingSource()
        bs.DataSource = dv
        transactionsDataGridView.DataSource = bs

        ' Load transactions
        LoadDailyTransactions()
    End Sub

    Private Sub LoadDailyTransactions()
        Try
            Dim connStr As String = GetConnectionString()
            Dim today As Date = DateTime.Today

            ' Combined query for both retail and wholesale transactions
            Dim query As String = "
            -- Retail Sales
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
                u.username AS HandledBy
            FROM RetailSalesReport sr
            INNER JOIN retailProducts rp ON sr.ProductID = rp.ProductID
            INNER JOIN Categories c ON sr.CategoryID = c.CategoryID
            INNER JOIN Users u ON sr.HandledBy = u.userID
            WHERE CAST(sr.SaleDate AS DATE) = @Today

            UNION ALL

            -- Wholesale Sales
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
                u.username AS HandledBy
            FROM SalesReport sr
            INNER JOIN wholesaleProducts wp ON sr.ProductID = wp.ProductID
            INNER JOIN Categories c ON sr.CategoryID = c.CategoryID
            INNER JOIN Users u ON sr.HandledBy = u.userID
            WHERE CAST(sr.SaleDate AS DATE) = @Today

            ORDER BY SaleDate DESC"

            Using conn As New SqlConnection(connStr)
                Using da As New SqlDataAdapter(query, conn)
                    da.SelectCommand.Parameters.AddWithValue("@Today", today)

                    dt.Clear()
                    da.Fill(dt)

                    ' Format columns
                    If transactionsDataGridView.Columns.Count > 0 Then
                        With transactionsDataGridView
                            .Columns("SaleType").HeaderText = "Type"
                            .Columns("SaleType").Width = 80
                            .Columns("SaleID").HeaderText = "Sale ID"
                            .Columns("SaleID").Width = 60
                            .Columns("SaleDate").HeaderText = "Time"
                            .Columns("SaleDate").DefaultCellStyle.Format = "hh:mm:ss tt"
                            .Columns("SaleDate").Width = 90
                            .Columns("ProductName").HeaderText = "Product"
                            .Columns("ProductName").Width = 200
                            .Columns("ProductName").AutoSizeMode = DataGridViewAutoSizeColumnMode.None
                            .Columns("ProductName").DefaultCellStyle.WrapMode = DataGridViewTriState.False
                            .Columns("Unit").HeaderText = "Unit"
                            .Columns("Unit").Width = 60
                            .Columns("CategoryName").HeaderText = "Category"
                            .Columns("CategoryName").Width = 100
                            .Columns("QuantitySold").HeaderText = "Qty"
                            .Columns("QuantitySold").Width = 50
                            .Columns("UnitPrice").HeaderText = "Unit Price"
                            .Columns("UnitPrice").DefaultCellStyle.Format = "₱#,##0.00"
                            .Columns("UnitPrice").Width = 90
                            .Columns("TotalAmount").HeaderText = "Total"
                            .Columns("TotalAmount").DefaultCellStyle.Format = "₱#,##0.00"
                            .Columns("TotalAmount").Width = 90
                            .Columns("PaymentMethod").HeaderText = "Payment"
                            .Columns("PaymentMethod").Width = 100
                            .Columns("HandledBy").HeaderText = "Cashier"
                            .Columns("HandledBy").Width = 100
                        End With

                        ' Color-code the SaleType column
                        For Each row As DataGridViewRow In transactionsDataGridView.Rows
                            If row.Cells("SaleType").Value IsNot Nothing Then
                                If row.Cells("SaleType").Value.ToString() = "RETAIL" Then
                                    row.Cells("SaleType").Style.BackColor = Color.LightBlue
                                    row.Cells("SaleType").Style.ForeColor = Color.DarkBlue
                                Else ' WHOLESALE
                                    row.Cells("SaleType").Style.BackColor = Color.LightGreen
                                    row.Cells("SaleType").Style.ForeColor = Color.DarkGreen
                                End If
                            End If
                        Next
                    End If

                    ' Update summary labels
                    UpdateSummary()
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show($"Error loading transactions: {ex.Message}", "Error",
       MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub UpdateSummary()
        Try
            Dim totalTransactions As Integer = dt.Rows.Count
            Dim totalRevenue As Decimal = 0D
            Dim retailCount As Integer = 0
            Dim wholesaleCount As Integer = 0

            For Each row As DataRow In dt.Rows
                ' Sum up total revenue
                If Not IsDBNull(row("TotalAmount")) Then
                    totalRevenue += Convert.ToDecimal(row("TotalAmount"))
                End If

                ' Count by type
                If Not IsDBNull(row("SaleType")) Then
                    If row("SaleType").ToString() = "RETAIL" Then
                        retailCount += 1
                    Else
                        wholesaleCount += 1
                    End If
                End If
            Next

            ' Update labels
            lblTotalTransactions.Text = $"Total Transactions: {totalTransactions}"
            lblTotalRevenue.Text = $"Total Revenue: ₱{totalRevenue:N2}"
            lblRetailCount.Text = $"Retail: {retailCount} transactions"
            lblWholesaleCount.Text = $"Wholesale: {wholesaleCount} transactions"
        Catch ex As Exception
            Console.WriteLine($"Error updating summary: {ex.Message}")
        End Try
    End Sub

    Private Function GetConnectionString() As String
        Return SharedUtilities.GetConnectionString()
    End Function

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        LoadDailyTransactions()
        MessageBox.Show("Transactions refreshed!", "Refresh",
        MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub cboSalesType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboSalesType.SelectedIndexChanged
        ' Filter based on selection
        Select Case cboSalesType.SelectedIndex
            Case 0 ' All Transactions
                bs.Filter = ""
            Case 1 ' Retail Only
                bs.Filter = "SaleType = 'RETAIL'"
            Case 2 ' Wholesale Only
                bs.Filter = "SaleType = 'WHOLESALE'"
        End Select

        ' Update summary after filtering
        UpdateFilteredSummary()
    End Sub

    Private Sub UpdateFilteredSummary()
        Try
            ' Use the DataView (dv) instead of bs.DataSource
            Dim totalTransactions As Integer = dv.Count
            Dim totalRevenue As Decimal = 0D
            Dim retailCount As Integer = 0
            Dim wholesaleCount As Integer = 0

            For Each rowView As DataRowView In dv
                Dim row As DataRow = rowView.Row

                ' Sum up total revenue
                If Not IsDBNull(row("TotalAmount")) Then
                    totalRevenue += Convert.ToDecimal(row("TotalAmount"))
                End If

                ' Count by type
                If Not IsDBNull(row("SaleType")) Then
                    If row("SaleType").ToString() = "RETAIL" Then
                        retailCount += 1
                    Else
                        wholesaleCount += 1
                    End If
                End If
            Next

            ' Update labels
            lblTotalTransactions.Text = $"Total Transactions: {totalTransactions}"
            lblTotalRevenue.Text = $"Total Revenue: ₱{totalRevenue:N2}"
            lblRetailCount.Text = $"Retail: {retailCount} transactions"
            lblWholesaleCount.Text = $"Wholesale: {wholesaleCount} transactions"
        Catch ex As Exception
            Console.WriteLine($"Error updating filtered summary: {ex.Message}")
        End Try
    End Sub

End Class