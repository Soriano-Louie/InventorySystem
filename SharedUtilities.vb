Imports Microsoft.Data.SqlClient

Public Module SharedUtilities
    ''' <summary>
    ''' Gets the database connection string
    ''' </summary>
    Public Function GetConnectionString() As String
        Return "Server=DESKTOP-3AKTMEV;Database=inventorySystem;User Id=sa;Password=24@Hakaaii07;TrustServerCertificate=True;"
    End Function

    ''' <summary>
    ''' Gets the current VAT rate from the database
    ''' </summary>
    ''' <returns>The VAT rate as a decimal, or 0 if not found</returns>
    Public Function GetCurrentVATRate() As Decimal
        Try
            Dim connStr As String = GetConnectionString()
            Dim sql As String = "SELECT TOP 1 vatRate FROM settings ORDER BY id DESC"

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

    ''' <summary>
    ''' Saves or updates the VAT rate in the database
    ''' </summary>
    ''' <param name="vatRate">The VAT rate to save</param>
    ''' <returns>True if successful, False otherwise</returns>
    Public Function SaveVATRate(vatRate As Decimal) As Boolean
        Try
            Dim connStr As String = GetConnectionString()

            Using conn As New SqlConnection(connStr)
                conn.Open()

                ' Check if a settings record exists
                Dim checkSql As String = "SELECT COUNT(*) FROM settings"
                Dim recordExists As Boolean = False

                Using checkCmd As New SqlCommand(checkSql, conn)
                    recordExists = Convert.ToInt32(checkCmd.ExecuteScalar()) > 0
                End Using

                ' Update existing record or insert new one
                Dim sql As String
                If recordExists Then
                    ' Update the first/only record
                    sql = "UPDATE settings SET vatRate = @VATRate"
                Else
                    ' Insert new record
                    sql = "INSERT INTO settings (vatRate) VALUES (@VATRate)"
                End If

                Using cmd As New SqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@VATRate", vatRate)
                    Dim rowsAffected As Integer = cmd.ExecuteNonQuery()

                    If rowsAffected = 0 Then
                        Console.WriteLine("Warning: No rows were affected")
                        Return False
                    End If
                End Using
            End Using

            Return True
        Catch ex As Exception
            Console.WriteLine("Error saving VAT rate: " & ex.Message)
            Return False
        End Try
    End Function
End Module
