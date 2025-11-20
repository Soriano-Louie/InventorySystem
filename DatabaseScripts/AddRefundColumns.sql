-- ========================================================
-- REFUND TRACKING DATABASE SCHEMA SETUP
-- ========================================================
-- IMPORTANT: Run this script BEFORE using the refund feature
-- This adds necessary columns to track refunded transactions
-- ========================================================

-- SQL Script to add refund tracking columns to sales report tables
-- Run this script to enable refund functionality

USE inventorySystem;
GO

-- ========================================================
-- RETAIL SALES REPORT TABLE - Add Refund Columns
-- ========================================================

-- Add refund columns to RetailSalesReport table
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'RetailSalesReport') AND name = 'IsRefunded')
BEGIN
    ALTER TABLE RetailSalesReport
    ADD IsRefunded BIT DEFAULT 0 NOT NULL;
    
    PRINT 'Added IsRefunded column to RetailSalesReport';
END
ELSE
BEGIN
    PRINT 'IsRefunded column already exists in RetailSalesReport';
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'RetailSalesReport') AND name = 'RefundDate')
BEGIN
    ALTER TABLE RetailSalesReport
    ADD RefundDate DATETIME NULL;
    
    PRINT 'Added RefundDate column to RetailSalesReport';
END
ELSE
BEGIN
    PRINT 'RefundDate column already exists in RetailSalesReport';
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'RetailSalesReport') AND name = 'RefundReason')
BEGIN
    ALTER TABLE RetailSalesReport
    ADD RefundReason NVARCHAR(500) NULL;

  PRINT 'Added RefundReason column to RetailSalesReport';
END
ELSE
BEGIN
    PRINT 'RefundReason column already exists in RetailSalesReport';
END
GO

-- ========================================================
-- WHOLESALE SALES REPORT TABLE - Add Refund Columns
-- ========================================================

-- Add refund columns to SalesReport table (Wholesale)
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'SalesReport') AND name = 'IsRefunded')
BEGIN
    ALTER TABLE SalesReport
    ADD IsRefunded BIT DEFAULT 0 NOT NULL;
    
    PRINT 'Added IsRefunded column to SalesReport';
END
ELSE
BEGIN
    PRINT 'IsRefunded column already exists in SalesReport';
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'SalesReport') AND name = 'RefundDate')
BEGIN
    ALTER TABLE SalesReport
    ADD RefundDate DATETIME NULL;
    
    PRINT 'Added RefundDate column to SalesReport';
END
ELSE
BEGIN
    PRINT 'RefundDate column already exists in SalesReport';
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'SalesReport') AND name = 'RefundReason')
BEGIN
    ALTER TABLE SalesReport
    ADD RefundReason NVARCHAR(500) NULL;
    
    PRINT 'Added RefundReason column to SalesReport';
END
ELSE
BEGIN
    PRINT 'RefundReason column already exists in SalesReport';
END
GO

-- ========================================================
-- PERFORMANCE OPTIMIZATION - Create Indexes
-- ========================================================

-- Create an index on IsRefunded for better query performance
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_RetailSalesReport_IsRefunded')
BEGIN
    CREATE INDEX IX_RetailSalesReport_IsRefunded 
    ON RetailSalesReport(IsRefunded);
    
    PRINT 'Created index IX_RetailSalesReport_IsRefunded';
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_SalesReport_IsRefunded')
BEGIN
    CREATE INDEX IX_SalesReport_IsRefunded 
ON SalesReport(IsRefunded);
    
    PRINT 'Created index IX_SalesReport_IsRefunded';
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
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'RetailSalesReport') AND name = 'IsRefunded')
    AND EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'RetailSalesReport') AND name = 'RefundDate')
    AND EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'RetailSalesReport') AND name = 'RefundReason')
BEGIN
    PRINT '? RetailSalesReport: All refund columns present';
END
ELSE
BEGIN
    PRINT '? RetailSalesReport: Missing refund columns';
END

-- Check SalesReport (Wholesale)
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'SalesReport') AND name = 'IsRefunded')
    AND EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'SalesReport') AND name = 'RefundDate')
    AND EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'SalesReport') AND name = 'RefundReason')
BEGIN
    PRINT '? SalesReport (Wholesale): All refund columns present';
END
ELSE
BEGIN
    PRINT '? SalesReport (Wholesale): Missing refund columns';
END

PRINT '';
PRINT '========================================================';
PRINT 'Refund columns setup completed successfully!';
PRINT 'You can now use the refund feature in the application.';
PRINT '========================================================';
GO
