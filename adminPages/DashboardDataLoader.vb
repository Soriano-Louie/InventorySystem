Imports Microsoft.Data.SqlClient

''' <summary>
''' Polymorphic data loader for dashboards using Strategy Pattern
''' Demonstrates polymorphism through abstract methods and inheritance
''' </summary>
Public MustInherit Class DashboardDataLoader

    ' ============================================
    ' ABSTRACT METHODS (Polymorphism - Must Override)
    ' ============================================

    ''' <summary>
    ''' Gets the product table name - Polymorphic behavior
    ''' </summary>
    Protected MustOverride Function GetProductTableName() As String

    ''' <summary>
    ''' Gets the sales report table name - Polymorphic behavior
    ''' </summary>
    Protected MustOverride Function GetSalesReportTableName() As String

    ' ============================================
    ' TEMPLATE METHOD PATTERN (Polymorphism)
    ' ============================================

    ''' <summary>
    ''' Template method that uses polymorphic table names
    ''' </summary>
    Public Function LoadTotalOrdersToday() As Integer
        Try
            Dim connStr As String = GetConnectionString()
            Dim sql As String = $"SELECT COUNT(*) FROM {GetSalesReportTableName()} WHERE CAST(SaleDate AS date) = CAST(GETDATE() AS date)"

            Using conn As New SqlConnection(connStr)
                Using cmd As New SqlCommand(sql, conn)
                    conn.Open()
                    Return Convert.ToInt32(cmd.ExecuteScalar())
                End Using
            End Using
        Catch ex As Exception
            Console.WriteLine($"Error loading total orders: {ex.Message}")
            Return -1 ' Error indicator
        End Try
    End Function

    ''' <summary>
    ''' Loads total products - uses polymorphic table name
    ''' </summary>
    Public Function LoadTotalProducts() As Integer
        Try
            Dim connStr As String = GetConnectionString()
            Dim sql As String = $"SELECT COUNT(*) FROM {GetProductTableName()}"

            Using conn As New SqlConnection(connStr)
                Using cmd As New SqlCommand(sql, conn)
                    conn.Open()
                    Return Convert.ToInt32(cmd.ExecuteScalar())
                End Using
            End Using
        Catch ex As Exception
            Console.WriteLine($"Error loading total products: {ex.Message}")
            Return -1
        End Try
    End Function

    ''' <summary>
    ''' Loads critical stock products - uses polymorphic table name
    ''' </summary>
    Public Function LoadCriticalStockProducts() As List(Of String)
        Try
            Dim connStr As String = GetConnectionString()
            Dim sql As String = $"SELECT ProductName FROM {GetProductTableName()} WHERE StockQuantity <= ReorderLevel"

            Dim criticalProducts As New List(Of String)()

            Using conn As New SqlConnection(connStr)
                Using cmd As New SqlCommand(sql, conn)
                    conn.Open()
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            criticalProducts.Add(reader.GetString(0))
                        End While
                    End Using
                End Using
            End Using

            Return criticalProducts
        Catch ex As Exception
            Console.WriteLine($"Error loading critical stock: {ex.Message}")
            Return New List(Of String)()
        End Try
    End Function

    ''' <summary>
    ''' Loads sales data for chart - uses polymorphic table name
    ''' </summary>
    Public Function LoadSalesDataForMonth(currentMonth As Date) As List(Of KeyValuePair(Of Date, Decimal))
        Dim chartData As New List(Of KeyValuePair(Of Date, Decimal))()

        Try
            Dim connStr As String = GetConnectionString()
            Dim firstDayOfMonth As Date = New Date(currentMonth.Year, currentMonth.Month, 1)
            Dim lastDayOfMonth As Date = firstDayOfMonth.AddMonths(1).AddDays(-1)

            Dim sql As String = $"
   SELECT
            CAST(SaleDate AS date) AS SaleDate,
    ISNULL(SUM(TotalAmount), 0) AS TotalSales
            FROM {GetSalesReportTableName()}
  WHERE SaleDate >= @StartDate AND SaleDate <= @EndDate
         GROUP BY CAST(SaleDate AS date)
        ORDER BY SaleDate ASC"

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
        Catch ex As Exception
            Console.WriteLine($"Error loading sales data: {ex.Message}")
        End Try

        Return chartData
    End Function

    ''' <summary>
    ''' Loads top products - uses polymorphic table names
    ''' </summary>
    Public Function LoadTopProducts(currentMonth As Date) As DataTable
        Dim dt As New DataTable()

        Try
            Dim connStr As String = GetConnectionString()
            Dim firstDayOfMonth As Date = New Date(currentMonth.Year, currentMonth.Month, 1)
            Dim lastDayOfMonth As Date = firstDayOfMonth.AddMonths(1).AddDays(-1)

            Dim sql As String = $"
     SELECT TOP 10
   sr.ProductID,
      p.ProductName,
         SUM(sr.TotalAmount) AS TotalRevenue,
       SUM(sr.QuantitySold) AS TotalQuantity,
  COUNT(*) AS SalesCount
                FROM {GetSalesReportTableName()} sr
  INNER JOIN {GetProductTableName()} p ON sr.ProductID = p.ProductID
      WHERE sr.SaleDate >= @StartDate AND sr.SaleDate <= @EndDate
         GROUP BY sr.ProductID, p.ProductName
     ORDER BY TotalRevenue DESC, TotalQuantity DESC, SalesCount DESC"

            Using conn As New SqlConnection(connStr)
                Using cmd As New SqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@StartDate", firstDayOfMonth)
                    cmd.Parameters.AddWithValue("@EndDate", lastDayOfMonth)

                    Using adapter As New SqlDataAdapter(cmd)
                        adapter.Fill(dt)
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Console.WriteLine($"Error loading top products: {ex.Message}")
        End Try

        Return dt
    End Function

    ' ============================================
    ' COMMON METHODS (Non-polymorphic)
    ' ============================================

    Public Function LoadTotalUsers() As Integer
        Try
            Dim connStr As String = GetConnectionString()
            Dim sql As String = "SELECT COUNT(*) FROM Users"

            Using conn As New SqlConnection(connStr)
                Using cmd As New SqlCommand(sql, conn)
                    conn.Open()
                    Return Convert.ToInt32(cmd.ExecuteScalar())
                End Using
            End Using
        Catch ex As Exception
            Console.WriteLine($"Error loading total users: {ex.Message}")
            Return -1
        End Try
    End Function

    Public Function LoadTotalCategories() As Integer
        Try
            Dim connStr As String = GetConnectionString()
            Dim sql As String = "SELECT COUNT(*) FROM Categories"

            Using conn As New SqlConnection(connStr)
                Using cmd As New SqlCommand(sql, conn)
                    conn.Open()
                    Return Convert.ToInt32(cmd.ExecuteScalar())
                End Using
            End Using
        Catch ex As Exception
            Console.WriteLine($"Error loading total categories: {ex.Message}")
            Return -1
        End Try
    End Function

    Protected Function GetConnectionString() As String
        Return SharedUtilities.GetConnectionString()
    End Function

End Class

''' <summary>
''' Wholesale-specific data loader - Polymorphic implementation
''' </summary>
Public Class WholesaleDashboardDataLoader
    Inherits DashboardDataLoader

    Protected Overrides Function GetProductTableName() As String
        Return "wholesaleProducts"
    End Function

    Protected Overrides Function GetSalesReportTableName() As String
        Return "SalesReport"
    End Function

End Class

''' <summary>
''' Retail-specific data loader - Polymorphic implementation
''' </summary>
Public Class RetailDashboardDataLoader
    Inherits DashboardDataLoader

    Protected Overrides Function GetProductTableName() As String
        Return "retailProducts"
    End Function

    Protected Overrides Function GetSalesReportTableName() As String
        Return "RetailSalesReport"
    End Function

End Class