-- Test Script to Diagnose VAT Rate Issue
-- Run this in SQL Server Management Studio

PRINT '========================================='
PRINT 'VAT RATE DIAGNOSTIC SCRIPT'
PRINT '========================================='
PRINT ''

-- 1. Check if settings table exists
PRINT '1. Checking if settings table exists...'
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'settings')
BEGIN
    PRINT '   ? Settings table EXISTS'
    
    -- Show table structure
    PRINT ''
    PRINT '2. Settings table structure:'
    SELECT 
        COLUMN_NAME,
        DATA_TYPE,
        IS_NULLABLE,
        COLUMN_DEFAULT
    FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'settings'
    ORDER BY ORDINAL_POSITION
    
    -- Show current data
    PRINT ''
    PRINT '3. Current settings data:'
    SELECT * FROM settings
    
  -- Test COUNT query
    PRINT ''
    PRINT '4. Testing COUNT query...'
    DECLARE @count INT
    SELECT @count = COUNT(*) FROM settings
    PRINT '   Record count: ' + CAST(@count AS VARCHAR)
    
    -- Test INSERT/UPDATE
    IF @count > 0
    BEGIN
        PRINT ''
  PRINT '5. Testing UPDATE query...'
  PRINT '   SQL: UPDATE settings SET vatRate = 12.5'
        -- Don't actually run it, just show what would happen
        PRINT '   This would update ' + CAST(@count AS VARCHAR) + ' row(s)'
    END
    ELSE
    BEGIN
   PRINT ''
        PRINT '5. Testing INSERT query...'
  PRINT '   SQL: INSERT INTO settings (vatRate) VALUES (12.5)'
        PRINT '   This would insert a new row'
    END
END
ELSE
BEGIN
 PRINT '   ? Settings table DOES NOT EXIST'
    PRINT ''
    PRINT 'SOLUTION: Run the CREATE_SETTINGS_TABLE.sql script to create the table'
END

PRINT ''
PRINT '========================================='
PRINT 'DIAGNOSTIC COMPLETE'
PRINT '========================================='
