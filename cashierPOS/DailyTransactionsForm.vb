Imports Microsoft.Data.SqlClient

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

        ' Make columns and rows non-resizable
        transactionsDataGridView.AllowUserToResizeColumns = False
        transactionsDataGridView.AllowUserToResizeRows = False

        ' Add double-click event handler to show receipt
        AddHandler transactionsDataGridView.CellDoubleClick, AddressOf TransactionsDataGridView_CellDoubleClick

        ' Add tooltip to inform users about double-click functionality
        Dim toolTip As New ToolTip()
        toolTip.SetToolTip(transactionsDataGridView, "Double-click a transaction to view its receipt")

        ' Setup buttons
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

        btnViewReceipt.BackColor = Color.FromArgb(147, 53, 53)
        btnViewReceipt.ForeColor = Color.FromArgb(230, 216, 177)
        btnViewReceipt.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        btnViewReceipt.FlatStyle = FlatStyle.Flat
        btnViewReceipt.Cursor = Cursors.Hand

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

        ' Load transactions
        LoadDailyTransactions()
    End Sub

    Private Sub LoadDailyTransactions()
        Try
            Dim connStr As String = GetConnectionString()
            Dim today As Date = DateTime.Today

            ' Combined query for both retail and wholesale transactions with refund status
            Dim query As String = "
    -- Retail Sales (all in-store)
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
      WHERE CAST(sr.SaleDate AS DATE) = @Today

        UNION ALL

         -- Wholesale Sales (can be delivery, pickup, or in-store)
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
      WHERE CAST(sr.SaleDate AS DATE) = @Today

        ORDER BY SaleDate DESC"

            Using conn As New SqlConnection(connStr)
                Using da As New SqlDataAdapter(query, conn)
                    da.SelectCommand.Parameters.AddWithValue("@Today", today)

                    dt.Clear()
                    da.Fill(dt)

                    ' Add a computed column for display status
                    If Not dt.Columns.Contains("RefundStatus") Then
                        dt.Columns.Add("RefundStatus", GetType(String))
                    End If

                    ' Populate the display column
                    For Each row As DataRow In dt.Rows
                        Dim isRefunded As Boolean = If(IsDBNull(row("IsRefunded")), False, Convert.ToBoolean(row("IsRefunded")))
                        row("RefundStatus") = If(isRefunded, "REFUNDED", "Active")
                    Next

                    ' Format columns
                    If transactionsDataGridView.Columns.Count > 0 Then
                        With transactionsDataGridView
                            .Columns("SaleType").HeaderText = "Type"
                            .Columns("SaleType").Width = 80
                            .Columns("TransactionType").HeaderText = "Mode"
                            .Columns("TransactionType").Width = 80
                            .Columns("SaleID").HeaderText = "Sale ID"
                            .Columns("SaleID").Width = 60
                            .Columns("SaleDate").HeaderText = "Time"
                            .Columns("SaleDate").DefaultCellStyle.Format = "hh:mm:ss tt"
                            .Columns("SaleDate").Width = 90
                            .Columns("ProductName").HeaderText = "Product"
                            .Columns("ProductName").Width = 180
                            .Columns("ProductName").AutoSizeMode = DataGridViewAutoSizeColumnMode.None
                            .Columns("ProductName").DefaultCellStyle.WrapMode = DataGridViewTriState.False

                            ' Hide ProductID and IsRefunded columns (needed for refunds but not displayed)
                            .Columns("ProductID").Visible = False
                            .Columns("IsRefunded").Visible = False

                            .Columns("Unit").HeaderText = "Unit"
                            .Columns("Unit").Width = 60
                            .Columns("CategoryName").HeaderText = "Category"
                            .Columns("CategoryName").Width = 90
                            .Columns("QuantitySold").HeaderText = "Qty"
                            .Columns("QuantitySold").Width = 50
                            .Columns("UnitPrice").HeaderText = "Unit Price"
                            .Columns("UnitPrice").DefaultCellStyle.Format = "₱#,##0.00"
                            .Columns("UnitPrice").Width = 90
                            .Columns("TotalAmount").HeaderText = "Total"
                            .Columns("TotalAmount").DefaultCellStyle.Format = "₱#,##0.00"
                            .Columns("TotalAmount").Width = 90
                            .Columns("PaymentMethod").HeaderText = "Payment"
                            .Columns("PaymentMethod").Width = 90
                            .Columns("HandledBy").HeaderText = "Cashier"
                            .Columns("HandledBy").Width = 90

                            ' Refund status columns - use RefundStatus instead of IsRefunded for display
                            .Columns("RefundStatus").HeaderText = "Status"
                            .Columns("RefundStatus").Width = 80
                            .Columns("RefundDate").HeaderText = "Refund Date"
                            .Columns("RefundDate").DefaultCellStyle.Format = "MM/dd/yyyy hh:mm tt"
                            .Columns("RefundDate").Width = 130
                            .Columns("RefundReason").HeaderText = "Refund Reason"
                            .Columns("RefundReason").Width = 150
                        End With

                        ' Color-code the rows
                        For Each row As DataGridViewRow In transactionsDataGridView.Rows
                            ' Color-code the SaleType column
                            If row.Cells("SaleType").Value IsNot Nothing Then
                                If row.Cells("SaleType").Value.ToString() = "RETAIL" Then
                                    row.Cells("SaleType").Style.BackColor = Color.LightBlue
                                    row.Cells("SaleType").Style.ForeColor = Color.DarkBlue
                                Else ' WHOLESALE
                                    row.Cells("SaleType").Style.BackColor = Color.LightGreen
                                    row.Cells("SaleType").Style.ForeColor = Color.DarkGreen
                                End If
                            End If

                            ' Color-code the TransactionType column
                            If row.Cells("TransactionType").Value IsNot Nothing Then
                                Dim transType As String = row.Cells("TransactionType").Value.ToString()
                                Select Case transType
                                    Case "Delivery"
                                        row.Cells("TransactionType").Style.BackColor = Color.FromArgb(255, 220, 180) ' Light orange
                                        row.Cells("TransactionType").Style.ForeColor = Color.DarkOrange
                                    Case "Pickup"
                                        row.Cells("TransactionType").Style.BackColor = Color.FromArgb(255, 255, 200) ' Light yellow
                                        row.Cells("TransactionType").Style.ForeColor = Color.DarkGoldenrod
                                    Case "In-Store"
                                        row.Cells("TransactionType").Style.BackColor = Color.FromArgb(200, 255, 200) ' Light green
                                        row.Cells("TransactionType").Style.ForeColor = Color.DarkGreen
                                End Select
                            End If

                            ' Color-code the Status column and entire row for refunded items
                            If row.Cells("RefundStatus").Value IsNot Nothing Then
                                Dim refundStatus As String = row.Cells("RefundStatus").Value.ToString()
                                If refundStatus = "REFUNDED" Then
                                    row.Cells("RefundStatus").Style.BackColor = Color.FromArgb(255, 180, 180) ' Light red
                                    row.Cells("RefundStatus").Style.ForeColor = Color.DarkRed
                                    row.Cells("RefundStatus").Style.Font = New Font("Segoe UI", 9, FontStyle.Bold)

                                    ' Strike through entire row for refunded transactions
                                    For Each cell As DataGridViewCell In row.Cells
                                        cell.Style.Font = New Font("Segoe UI", 9, FontStyle.Strikeout)
                                        cell.Style.ForeColor = Color.Gray
                                    Next
                                Else
                                    row.Cells("RefundStatus").Style.BackColor = Color.FromArgb(200, 255, 200) ' Light green
                                    row.Cells("RefundStatus").Style.ForeColor = Color.DarkGreen
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
            Dim refundedCount As Integer = 0
            Dim refundedAmount As Decimal = 0D

            For Each row As DataRow In dt.Rows
                ' Check if refunded
                Dim isRefunded As Boolean = If(IsDBNull(row("IsRefunded")), False, Convert.ToBoolean(row("IsRefunded")))

                If isRefunded Then
                    refundedCount += 1
                    ' Track refunded amount but don't add to revenue
                    If Not IsDBNull(row("TotalAmount")) Then
                        refundedAmount += Convert.ToDecimal(row("TotalAmount"))
                    End If
                Else
                    ' Only count non-refunded transactions in revenue
                    If Not IsDBNull(row("TotalAmount")) Then
                        totalRevenue += Convert.ToDecimal(row("TotalAmount"))
                    End If

                    ' Count by type (only non-refunded)
                    If Not IsDBNull(row("SaleType")) Then
                        If row("SaleType").ToString() = "RETAIL" Then
                            retailCount += 1
                        Else
                            wholesaleCount += 1
                        End If
                    End If
                End If
            Next

            ' Update labels - showing active (non-refunded) transactions
            Dim activeTransactions As Integer = totalTransactions - refundedCount
            lblTotalTransactions.Text = $"Total Transactions: {activeTransactions}"
            lblTotalRevenue.Text = $"Total Sales: ₱{totalRevenue:N2}"
            lblRetailCount.Text = $"Retail: {retailCount} transactions"
            lblWholesaleCount.Text = $"Wholesale: {wholesaleCount} transactions"

            ' Optionally show refunded info in debug
            If refundedCount > 0 Then
                Debug.WriteLine($"Refunded Transactions: {refundedCount}, Refunded Amount: ₱{refundedAmount:N2}")
            End If
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

    ''' <summary>
    ''' View Receipt button click handler
    ''' Shows receipt for the currently selected transaction
    ''' Alternative to double-clicking a row
    ''' </summary>
    Private Sub btnViewReceipt_Click(sender As Object, e As EventArgs) Handles btnViewReceipt.Click
        ' Check if a row is selected
        If transactionsDataGridView.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select a transaction to view its receipt.", "No Selection",
         MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim selectedRow As DataGridViewRow = transactionsDataGridView.SelectedRows(0)

        ' Get transaction details
        Dim saleType As String = selectedRow.Cells("SaleType").Value.ToString()
        Dim saleID As Integer = Convert.ToInt32(selectedRow.Cells("SaleID").Value)

        ' Show receipt
        ShowTransactionReceipt(saleType, saleID)
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
            Case 3 ' Refunded Only
                bs.Filter = "IsRefunded = True"
            Case 4 ' Non-Refunded Only
                bs.Filter = "IsRefunded = False"
        End Select

        ' Update summary after filtering
        UpdateFilteredSummary()
    End Sub

    Private Sub UpdateFilteredSummary()
        Try
            Dim totalTransactions As Integer = 0
            Dim totalRevenue As Decimal = 0D
            Dim retailCount As Integer = 0
            Dim wholesaleCount As Integer = 0
            Dim refundedCount As Integer = 0
            Dim refundedAmount As Decimal = 0D

            For Each rowView As DataRowView In dv
                Dim row As DataRow = rowView.Row

                ' Check if refunded
                Dim isRefunded As Boolean = If(IsDBNull(row("IsRefunded")), False, Convert.ToBoolean(row("IsRefunded")))

                If isRefunded Then
                    refundedCount += 1
                    ' Track refunded amount
                    If Not IsDBNull(row("TotalAmount")) Then
                        refundedAmount += Convert.ToDecimal(row("TotalAmount"))
                    End If

                    ' If we're showing "Refunded Only", include in transaction count
                    If cboSalesType.SelectedIndex = 3 Then
                        totalTransactions += 1
                    End If
                Else
                    ' Non-refunded transaction
                    totalTransactions += 1

                    ' Sum up total revenue (only non-refunded)
                    If Not IsDBNull(row("TotalAmount")) Then
                        totalRevenue += Convert.ToDecimal(row("TotalAmount"))
                    End If

                    ' Count by type (only non-refunded)
                    If Not IsDBNull(row("SaleType")) Then
                        If row("SaleType").ToString() = "RETAIL" Then
                            retailCount += 1
                        Else
                            wholesaleCount += 1
                        End If
                    End If
                End If
            Next

            ' Update labels based on filter
            If cboSalesType.SelectedIndex = 3 Then
                ' Showing refunded only
                lblTotalTransactions.Text = $"Refunded Transactions: {refundedCount}"
                lblTotalRevenue.Text = $"Refunded Amount: ₱{refundedAmount:N2}"
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
    End Sub

    ''' <summary>
    ''' Refund button click handler
    '''
    ''' REFUND FEATURE USAGE:
    ''' ====================
    ''' 1. User selects a transaction from the grid
    ''' 2. Clicks the "Refund" button
    ''' 3. System validates the transaction is not already refunded
    ''' 4. Confirmation dialog shows refund details and impact
    ''' 5. User provides refund reason (required)
    ''' 6. System processes refund:
    '''    a. Marks transaction as refunded in database
    '''    b. Restores product quantity to inventory
    '''    c. Updates Total Sales (excludes refunded amount)
    ''' 7. Grid refreshes showing refunded transaction with strikethrough
    '''
    ''' FILTERS AVAILABLE:
    ''' ==================
    ''' - All Transactions: Shows everything
    ''' - Retail Only: Shows only retail transactions
    ''' - Wholesale Only: Shows only wholesale transactions
    ''' - Refunded Only: Shows only refunded transactions
    ''' - Non-Refunded Only: Shows active transactions (default for calculations)
    '''
    ''' NOTES:
    ''' ======
    ''' - Refunds are permanent (cannot undo)
    ''' - Refunded transactions excluded from revenue totals
    ''' - Product quantity is restored immediately
    ''' - Refund reason is required for audit trail
    ''' </summary>
    Private Sub btnRefund_Click(sender As Object, e As EventArgs) Handles btnRefund.Click
        ' Check if a row is selected
        If transactionsDataGridView.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select a transaction to refund.", "No Selection",
   MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim selectedRow As DataGridViewRow = transactionsDataGridView.SelectedRows(0)

        ' Get transaction details
        Dim saleType As String = selectedRow.Cells("SaleType").Value.ToString()
        Dim saleID As Integer = Convert.ToInt32(selectedRow.Cells("SaleID").Value)
        Dim productID As Integer = Convert.ToInt32(selectedRow.Cells("ProductID").Value)
        Dim productName As String = selectedRow.Cells("ProductName").Value.ToString()
        Dim quantitySold As Integer = Convert.ToInt32(selectedRow.Cells("QuantitySold").Value)
        Dim totalAmount As Decimal = Convert.ToDecimal(selectedRow.Cells("TotalAmount").Value)

        ' Get the actual boolean value from the IsRefunded column (not RefundStatus display column)
        Dim isRefunded As Boolean = If(IsDBNull(selectedRow.Cells("IsRefunded").Value), False,
   Convert.ToBoolean(selectedRow.Cells("IsRefunded").Value))

        ' Check if already refunded
        If isRefunded Then
            MessageBox.Show("This transaction has already been refunded.", "Already Refunded",
       MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        ' Confirm refund
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

        ' Ask for refund reason
        Dim refundReason As String = InputBox("Please provide a reason for this refund:",
            "Refund Reason", "Customer request")

        If String.IsNullOrWhiteSpace(refundReason) Then
            MessageBox.Show("Refund reason is required.", "Required Field",
     MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Process the refund
        If ProcessRefund(saleType, saleID, productID, quantitySold, refundReason) Then
            MessageBox.Show("Refund processed successfully!" & vbCrLf &
    $"Product quantity restored: +{quantitySold}" & vbCrLf &
            $"Amount refunded: ₱{totalAmount:N2}",
   "Refund Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)

            ' Reload transactions
            LoadDailyTransactions()
        Else
            MessageBox.Show("Failed to process refund. Please try again.", "Error",
 MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    ''' <summary>
    ''' Processes a refund for a transaction
    ''' This function:
    ''' 1. Marks the sale as refunded in the database
    ''' 2. Restores the product quantity to inventory
    ''' 3. Records the refund reason and timestamp
    ''' 4. Uses database transaction to ensure data consistency
    '''
    ''' Refunded transactions are:
    ''' - Excluded from Total Sales calculations
    ''' - Shown with strikethrough formatting
    ''' - Filterable via "Refunded Only" option
    ''' - Cannot be refunded again (one-time operation)
    ''' </summary>
    ''' <param name="saleType">RETAIL or WHOLESALE</param>
    ''' <param name="saleID">The Sale ID from sales report table</param>
    ''' <param name="productID">Product ID to restore quantity</param>
    ''' <param name="quantitySold">Quantity to restore to inventory</param>
    ''' <param name="refundReason">Reason for refund (required)</param>
    ''' <returns>True if refund successful, False otherwise</returns>
    Private Function ProcessRefund(saleType As String, saleID As Integer, productID As Integer,
             quantitySold As Integer, refundReason As String) As Boolean
        Try
            Dim connStr As String = GetConnectionString()

            Using conn As New SqlConnection(connStr)
                conn.Open()

                Using transaction As SqlTransaction = conn.BeginTransaction()
                    Try
                        ' 1. Update the sales report to mark as refunded
                        Dim tableName As String = If(saleType = "RETAIL", "RetailSalesReport", "SalesReport")
                        Dim updateQuery As String = $"
                          UPDATE {tableName}
                                      SET IsRefunded = 1,
                             RefundDate = GETDATE(),
                           RefundReason = @RefundReason
                               WHERE SaleID = @SaleID"

                        Using cmd As New SqlCommand(updateQuery, conn, transaction)
                            cmd.Parameters.AddWithValue("@RefundReason", refundReason)
                            cmd.Parameters.AddWithValue("@SaleID", saleID)
                            cmd.ExecuteNonQuery()
                        End Using

                        ' 2. Restore product quantity
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

                        ' Commit the transaction
                        transaction.Commit()
                        Return True
                    Catch ex As Exception
                        ' Rollback on error
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
    ''' Handles double-click event on DataGridView to show receipt
    ''' </summary>
    Private Sub TransactionsDataGridView_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs)
        ' Ignore header row clicks
        If e.RowIndex < 0 Then
            Return
        End If

        ' Get the selected row
        Dim selectedRow As DataGridViewRow = transactionsDataGridView.Rows(e.RowIndex)

        ' Get transaction details
        Dim saleType As String = selectedRow.Cells("SaleType").Value.ToString()
        Dim saleID As Integer = Convert.ToInt32(selectedRow.Cells("SaleID").Value)

        ' Show receipt for this transaction
        ShowTransactionReceipt(saleType, saleID)
    End Sub

    ''' <summary>
    ''' Displays a receipt for the selected transaction
    ''' Shows all details including product info, amounts, payment method, and refund status
    ''' </summary>
    Private Sub ShowTransactionReceipt(saleType As String, saleID As Integer)
        Try
            Dim connStr As String = GetConnectionString()
            Dim receiptContent As New System.Text.StringBuilder()

            Using conn As New SqlConnection(connStr)
                conn.Open()

                ' Query to get transaction details
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
                Else ' WHOLESALE
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
                            ' Build receipt header
                            receiptContent.AppendLine("================================")
                            receiptContent.AppendLine("   INVENTORY SYSTEM POS")
                            receiptContent.AppendLine("   TRANSACTION RECEIPT")
                            receiptContent.AppendLine("================================")
                            receiptContent.AppendLine()

                            ' Transaction type and status
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

                            ' Wholesale-specific details
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

                            ' Product details
                            Dim productName As String = reader("ProductName").ToString()
                            If productName.Length > 30 Then
                                productName = productName.Substring(0, 27) & "..."
                            End If
                            receiptContent.AppendLine($"Product: {productName}")
                            receiptContent.AppendLine($"Category: {reader("CategoryName")}")
                            receiptContent.AppendLine($"Unit: {reader("Unit")}")
                            receiptContent.AppendLine()

                            ' Pricing details
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

                            ' Payment details for GCash and Bank Transaction
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

                            ' Refund information
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

                            ' Display receipt in message box
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

End Class