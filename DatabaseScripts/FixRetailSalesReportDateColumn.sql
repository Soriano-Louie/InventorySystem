-- ========================================================
-- FIX RETAIL SALES REPORT DATE COLUMN
-- ========================================================
-- This script changes the SaleDate column type from DATE to DATETIME
-- to properly store both date and time information for retail transactions
-- ========================================================

USE inventorySystem;
GO

PRINT '========================================================';
PRINT 'Fixing RetailSalesReport.SaleDate Column Type';
PRINT '========================================================';
PRINT '';

-- Check current data type
DECLARE @CurrentType NVARCHAR(50);
SELECT @CurrentType = DATA_TYPE 
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'RetailSalesReport' 
AND COLUMN_NAME = 'SaleDate';

PRINT 'Current SaleDate column type: ' + ISNULL(@CurrentType, 'NOT FOUND');
PRINT '';

-- Only proceed if the column exists and is DATE type
IF @CurrentType = 'date'
BEGIN
  PRINT 'Column is DATE type - converting to DATETIME...';
    PRINT '';
    
    -- Alter the column to DATETIME
    -- This will preserve existing dates but set time to 00:00:00
    ALTER TABLE RetailSalesReport
    ALTER COLUMN SaleDate DATETIME NOT NULL;
    
    PRINT '? Successfully converted SaleDate to DATETIME';
    PRINT '';
    PRINT 'NOTE: Existing records will have time set to 00:00:00 (midnight)';
    PRINT '      New transactions will store the actual time of sale.';
END
ELSE IF @CurrentType = 'datetime' OR @CurrentType = 'datetime2'
BEGIN
    PRINT '? Column is already DATETIME/DATETIME2 type - no changes needed';
END
ELSE IF @CurrentType IS NULL
BEGIN
    PRINT '? ERROR: SaleDate column not found in RetailSalesReport table';
END
ELSE
BEGIN
    PRINT '? WARNING: SaleDate column has unexpected type: ' + @CurrentType;
    PRINT '  Manual review recommended';
END

PRINT '';
PRINT '========================================================';
PRINT 'Verification';
PRINT '========================================================';

-- Verify the change
SELECT 
 COLUMN_NAME,
 DATA_TYPE,
    IS_NULLABLE,
    COLUMN_DEFAULT
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'RetailSalesReport'
AND COLUMN_NAME = 'SaleDate';

PRINT '';
PRINT '========================================================';
PRINT 'Script completed';
PRINT '========================================================';
GO
