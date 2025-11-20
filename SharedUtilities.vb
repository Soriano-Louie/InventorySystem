Imports Microsoft.Data.SqlClient

Public Module SharedUtilities

    ''' Gets the database connection string
    Public Function GetConnectionString() As String
        Return "Server=DESKTOP-3AKTMEV;Database=inventorySystem;User Id=sa;Password=24@Hakaaii07;TrustServerCertificate=True;"
    End Function

    ''' Gets the current VAT rate from the database
    ''' <returns>The VAT rate as a decimal, or 0 if not found</returns>
    Public Function GetCurrentVATRate() As Decimal
        Try
            Dim connStr As String = GetConnectionString()
            Dim sql As String = "
SELECT TOP 1 vatRate
          FROM settings
      ORDER BY id DESC"

            Using conn As New SqlConnection(connStr)
                Using cmd As New SqlCommand(sql, conn)
                    conn.Open()
                    Dim result = cmd.ExecuteScalar()

                    If result IsNot Nothing AndAlso Not IsDBNull(result) Then
                        Return Convert.ToDecimal(result)
                    Else
                        Return 0D ' Default to 0 if no VAT rate is set
                    End If
                End Using
            End Using
        Catch ex As Exception
            Console.WriteLine("Error getting current VAT rate: " & ex.Message)
            Return 0D
        End Try
    End Function

    ''' Saves or updates the VAT rate in the database
    ''' <param name="vatRate">The VAT rate to save</param>
    ''' <param name="errorMessage">Output parameter containing error message if save fails</param>
    ''' <returns>True if successful, False otherwise</returns>
    Public Function SaveVATRate(vatRate As Decimal, ByRef errorMessage As String) As Boolean
        Try
            Dim connStr As String = GetConnectionString()

            Using conn As New SqlConnection(connStr)
                Try
                    conn.Open()
                Catch connEx As Exception
                    errorMessage = $"Failed to connect to database: {connEx.Message}"
                    Console.WriteLine("Connection Error: " & errorMessage)
                    Return False
                End Try

                ' Check if a settings record exists
                Dim checkSql As String = "SELECT COUNT(*) FROM settings"
                Dim recordExists As Boolean = False

                Try
                    Using checkCmd As New SqlCommand(checkSql, conn)
                        recordExists = Convert.ToInt32(checkCmd.ExecuteScalar()) > 0
                    End Using
                Catch checkEx As Exception
                    errorMessage = $"Failed to check settings table: {checkEx.Message}"
                    Console.WriteLine("Check Error: " & errorMessage)
                    Return False
                End Try

                ' Update existing record or insert new one
                Dim sql As String
                If recordExists Then
                    ' Update the first/only record
                    sql = "UPDATE settings SET vatRate = @VATRate"
                Else
                    ' Insert new record
                    sql = "INSERT INTO settings (vatRate) VALUES (@VATRate)"
                End If

                Try
                    Using cmd As New SqlCommand(sql, conn)
                        cmd.Parameters.AddWithValue("@VATRate", vatRate)
                        Dim rowsAffected As Integer = cmd.ExecuteNonQuery()

                        If rowsAffected = 0 Then
                            errorMessage = "No rows were affected. The database may have an issue or the settings table may be empty."
                            Console.WriteLine("Warning: " & errorMessage)
                            Return False
                        End If
                    End Using
                Catch sqlEx As Exception
                    errorMessage = $"SQL execution failed: {sqlEx.Message}"
                    Console.WriteLine("SQL Error: " & errorMessage)
                    Return False
                End Try
            End Using

            errorMessage = String.Empty
            Return True
        Catch ex As Exception
            errorMessage = $"Unexpected error: {ex.Message}"
            Console.WriteLine("Error saving VAT rate: " & errorMessage)
            Return False
        End Try
    End Function

    ''' Saves or updates the VAT rate in the database (backward compatible version
    ''' <param name="vatRate">The VAT rate to save</param>
    ''' <returns>True if successful, False otherwise</returns>
    Public Function SaveVATRate(vatRate As Decimal) As Boolean
        Dim errorMessage As String = String.Empty
        Return SaveVATRate(vatRate, errorMessage)
    End Function

End Module