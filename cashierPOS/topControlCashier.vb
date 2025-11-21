Imports Microsoft.Data.SqlClient

Public Class topControlCashier
    Private WithEvents salesUpdateTimer As New Timer()

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.BackColor = Color.FromArgb(79, 51, 40)
        Label1.ForeColor = Color.FromArgb(94, 175, 209)
        nameLabel.ForeColor = Color.FromArgb(230, 216, 177)
        roleLabel.ForeColor = Color.FromArgb(230, 216, 177)
        totalSalesText.AutoSize = True

        ' Set the user information from the current session
        SetUserInfo()

        ' Initialize and start the timer for updating daily sales
        salesUpdateTimer.Interval = 30000 ' Update every 30 seconds (30000 milliseconds)
        salesUpdateTimer.Start()

        ' Load daily sales immediately
        UpdateDailySales()
    End Sub

    ''' <summary>
    ''' Sets the nameLabel and roleLabel based on the logged-in user's session
    ''' </summary>
    Public Sub SetUserInfo()
        If GlobalUserSession.IsUserLoggedIn() Then
            nameLabel.Text = GlobalUserSession.CurrentUsername
            roleLabel.Text = GlobalUserSession.CurrentUserRole
        Else
            nameLabel.Text = "Not Logged In"
            roleLabel.Text = ""
        End If
    End Sub

    ''' <summary>
    ''' Loads and displays total sales for the current day
    ''' </summary>
    Public Sub UpdateDailySales()
        Try
            Dim totalSales As Decimal = GetTotalDailySales()
            totalSalesText.Text = $"₱{totalSales:N2}"
        Catch ex As Exception
            totalSalesText.Text = "Error"
            Console.WriteLine($"Error updating daily sales: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' Gets total sales for today from both retail and wholesale sales
    ''' </summary>
    Private Function GetTotalDailySales() As Decimal
        Dim connStr As String = SharedUtilities.GetConnectionString()
        Dim today As Date = DateTime.Today
        Dim totalSales As Decimal = 0D

        Try
            Using conn As New SqlConnection(connStr)
                conn.Open()

                ' Query to get total sales from both Retail and Wholesale (excluding refunded transactions)
                Dim query As String = "
                    SELECT
                        ISNULL(SUM(TotalAmount), 0) AS TotalSales
                    FROM (
                        -- Retail Sales
                        SELECT TotalAmount
                        FROM RetailSalesReport
                        WHERE CAST(SaleDate AS DATE) = @Today
                        AND ISNULL(IsRefunded, 0) = 0

                        UNION ALL

                        -- Wholesale Sales
                        SELECT TotalAmount
                        FROM SalesReport
                        WHERE CAST(SaleDate AS DATE) = @Today
                        AND ISNULL(IsRefunded, 0) = 0
                    ) AS AllSales"

                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@Today", today)
                    Dim result = cmd.ExecuteScalar()

                    If result IsNot Nothing AndAlso Not IsDBNull(result) Then
                        totalSales = Convert.ToDecimal(result)
                    End If
                End Using
            End Using
        Catch ex As Exception
            Console.WriteLine($"Error getting daily sales: {ex.Message}")
        End Try

        Return totalSales
    End Function

    ''' <summary>
    ''' Timer tick event to update daily sales periodically
    ''' </summary>
    Private Sub SalesUpdateTimer_Tick(sender As Object, e As EventArgs) Handles salesUpdateTimer.Tick
        UpdateDailySales()
    End Sub

End Class