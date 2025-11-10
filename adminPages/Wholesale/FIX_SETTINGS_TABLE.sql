-- Fix Script: Add IDENTITY to settings table and insert initial row
-- Run this in SQL Server Management Studio

PRINT '========================================='
PRINT 'FIX SETTINGS TABLE SCRIPT'
PRINT '========================================='
PRINT ''

-- Check current table structure
PRINT '1. Current table structure:'
SELECT 
    COLUMN_NAME,
    DATA_TYPE,
    IS_NULLABLE,
    COLUMN_DEFAULT,
    COLUMNPROPERTY(OBJECT_ID('settings'), COLUMN_NAME, 'IsIdentity') AS IsIdentity
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'settings'
ORDER BY ORDINAL_POSITION

PRINT ''
PRINT '2. Checking if id column is IDENTITY...'

IF COLUMNPROPERTY(OBJECT_ID('settings'), 'id', 'IsIdentity') = 0
BEGIN
    PRINT '   ? id column is NOT an IDENTITY column'
    PRINT ''
    PRINT '   Fixing: Recreating table with IDENTITY column...'
    
    -- Drop and recreate the table with proper IDENTITY
    BEGIN TRANSACTION
    
    -- Backup any existing data (there is none, but just in case)
    SELECT * INTO settings_backup FROM settings
  
    -- Drop the old table
    DROP TABLE settings
    
  -- Create new table with IDENTITY
    CREATE TABLE settings (
     id INT IDENTITY(1,1) PRIMARY KEY,
        vatRate DECIMAL(5, 2) NOT NULL DEFAULT 0.12,
        CreatedDate DATETIME DEFAULT GETDATE(),
 ModifiedDate DATETIME DEFAULT GETDATE()
    )
    
    -- Insert default VAT rate
    INSERT INTO settings (vatRate) VALUES (12.00)
    
    -- Drop backup table
    DROP TABLE settings_backup
    
    COMMIT TRANSACTION
    
    PRINT '   ? Table recreated with IDENTITY column'
    PRINT '   ? Default VAT rate (12%) inserted'
END
ELSE
BEGIN
    PRINT '   ? id column is already an IDENTITY column'
    PRINT ''
    PRINT '3. Checking for data...'
    
    DECLARE @count INT
    SELECT @count = COUNT(*) FROM settings
  
    IF @count = 0
    BEGIN
        PRINT '   ? No data found'
        PRINT '   Inserting default VAT rate (12%)...'
        
   INSERT INTO settings (vatRate) VALUES (12.00)
        
      PRINT '   ? Default VAT rate inserted'
    END
    ELSE
    BEGIN
        PRINT '   ? Data exists (' + CAST(@count AS VARCHAR) + ' row(s))'
    END
END

PRINT ''
PRINT '4. Final table structure:'
SELECT 
  COLUMN_NAME,
    DATA_TYPE,
    IS_NULLABLE,
    COLUMN_DEFAULT,
    COLUMNPROPERTY(OBJECT_ID('settings'), COLUMN_NAME, 'IsIdentity') AS IsIdentity
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'settings'
ORDER BY ORDINAL_POSITION

PRINT ''
PRINT '5. Current settings data:'
SELECT * FROM settings

PRINT ''
PRINT '========================================='
PRINT 'FIX COMPLETE'
PRINT '========================================='
