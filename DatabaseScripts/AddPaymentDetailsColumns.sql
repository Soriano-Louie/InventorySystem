-- ========================================================
-- ADD PAYMENT DETAILS COLUMNS TO SALES REPORT TABLES
-- ========================================================
-- This script adds columns to store payment reference information
-- for GCash and Bank Transaction payments
-- ========================================================

USE inventorySystem;
GO

PRINT '========================================================';
PRINT 'Adding Payment Details Columns';
PRINT '========================================================';
PRINT '';

-- ========================================================
-- RETAIL SALES REPORT TABLE - Add Payment Details Columns
-- ========================================================

-- Add PayerName column
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'RetailSalesReport') AND name = 'PayerName')
BEGIN
    ALTER TABLE RetailSalesReport
    ADD PayerName NVARCHAR(200) NULL;
    
    PRINT '? Added PayerName column to RetailSalesReport';
END
ELSE
BEGIN
    PRINT 'PayerName column already exists in RetailSalesReport';
END
GO

-- Add ReferenceNumber column
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'RetailSalesReport') AND name = 'ReferenceNumber')
BEGIN
    ALTER TABLE RetailSalesReport
    ADD ReferenceNumber NVARCHAR(100) NULL;
    
    PRINT '? Added ReferenceNumber column to RetailSalesReport';
END
ELSE
BEGIN
    PRINT 'ReferenceNumber column already exists in RetailSalesReport';
END
GO

-- Add BankName column
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'RetailSalesReport') AND name = 'BankName')
BEGIN
    ALTER TABLE RetailSalesReport
    ADD BankName NVARCHAR(200) NULL;
    
    PRINT '? Added BankName column to RetailSalesReport';
END
ELSE
BEGIN
  PRINT 'BankName column already exists in RetailSalesReport';
END
GO

-- ========================================================
-- WHOLESALE SALES REPORT TABLE - Add Payment Details Columns
-- ========================================================

-- Add PayerName column
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'SalesReport') AND name = 'PayerName')
BEGIN
    ALTER TABLE SalesReport
  ADD PayerName NVARCHAR(200) NULL;
    
    PRINT '? Added PayerName column to SalesReport';
END
ELSE
BEGIN
    PRINT 'PayerName column already exists in SalesReport';
END
GO

-- Add ReferenceNumber column
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'SalesReport') AND name = 'ReferenceNumber')
BEGIN
    ALTER TABLE SalesReport
    ADD ReferenceNumber NVARCHAR(100) NULL;
    
 PRINT '? Added ReferenceNumber column to SalesReport';
END
ELSE
BEGIN
    PRINT 'ReferenceNumber column already exists in SalesReport';
END
GO

-- Add BankName column
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'SalesReport') AND name = 'BankName')
BEGIN
    ALTER TABLE SalesReport
    ADD BankName NVARCHAR(200) NULL;
    
    PRINT '? Added BankName column to SalesReport';
END
ELSE
BEGIN
    PRINT 'BankName column already exists in SalesReport';
END
GO

-- ========================================================
-- PERFORMANCE OPTIMIZATION - Create Indexes
-- ========================================================

-- Create index on ReferenceNumber for faster lookups
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_RetailSalesReport_ReferenceNumber')
BEGIN
    CREATE INDEX IX_RetailSalesReport_ReferenceNumber 
    ON RetailSalesReport(ReferenceNumber);
    
    PRINT '? Created index IX_RetailSalesReport_ReferenceNumber';
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_SalesReport_ReferenceNumber')
BEGIN
    CREATE INDEX IX_SalesReport_ReferenceNumber 
    ON SalesReport(ReferenceNumber);
    
    PRINT '? Created index IX_SalesReport_ReferenceNumber';
END
GO

-- ========================================================
-- VERIFICATION - Check Column Setup
-- ========================================================

PRINT '';
PRINT '========================================================';
PRINT 'VERIFICATION RESULTS';
PRINT '========================================================';

-- Check RetailSalesReport
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'RetailSalesReport') AND name = 'PayerName')
    AND EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'RetailSalesReport') AND name = 'ReferenceNumber')
    AND EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'RetailSalesReport') AND name = 'BankName')
BEGIN
    PRINT '? RetailSalesReport: All payment details columns present';
END
ELSE
BEGIN
    PRINT '? RetailSalesReport: Missing payment details columns';
END

-- Check SalesReport (Wholesale)
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'SalesReport') AND name = 'PayerName')
    AND EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'SalesReport') AND name = 'ReferenceNumber')
    AND EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'SalesReport') AND name = 'BankName')
BEGIN
    PRINT '? SalesReport (Wholesale): All payment details columns present';
END
ELSE
BEGIN
    PRINT '? SalesReport (Wholesale): Missing payment details columns';
END

PRINT '';
PRINT '========================================================';
PRINT 'Payment details columns setup completed successfully!';
PRINT 'You can now capture payment information for GCash and Bank Transaction payments.';
PRINT '========================================================';
GO
