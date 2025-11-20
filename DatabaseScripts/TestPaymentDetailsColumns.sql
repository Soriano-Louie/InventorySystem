-- ========================================================
-- TEST PAYMENT DETAILS COLUMNS
-- ========================================================
-- This script tests if payment details columns are working
-- ========================================================

USE inventorySystem;
GO

PRINT '========================================================';
PRINT 'Testing Payment Details Columns';
PRINT '========================================================';
PRINT '';

-- First, verify columns exist
PRINT 'Step 1: Verifying columns exist...';
PRINT '-----------------------------------';

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'RetailSalesReport') AND name = 'PayerName')
BEGIN
    PRINT '? PayerName column exists in RetailSalesReport';
END
ELSE
BEGIN
    PRINT '? PayerName column MISSING in RetailSalesReport';
END

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'RetailSalesReport') AND name = 'ReferenceNumber')
BEGIN
 PRINT '? ReferenceNumber column exists in RetailSalesReport';
END
ELSE
BEGIN
    PRINT '? ReferenceNumber column MISSING in RetailSalesReport';
END

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'RetailSalesReport') AND name = 'BankName')
BEGIN
    PRINT '? BankName column exists in RetailSalesReport';
END
ELSE
BEGIN
    PRINT '? BankName column MISSING in RetailSalesReport';
END

PRINT '';
PRINT '========================================================';
PRINT 'Step 2: Testing INSERT with payment details...';
PRINT '========================================================';
PRINT '';

-- Test INSERT (using a transaction so we can rollback)
BEGIN TRANSACTION TestInsert;

BEGIN TRY
    -- Insert a test record
    INSERT INTO RetailSalesReport
    (SaleDate, ProductID, CategoryID, QuantitySold, UnitPrice, TotalAmount,
     PaymentMethod, HandledBy, PayerName, ReferenceNumber, BankName)
    VALUES
    (GETDATE(), 1, 1, 1, 999.99, 999.99, 'GCash', 1,
  'TEST USER - LOUIE', 'TEST-REF-12345', NULL);
    
    PRINT '? Test INSERT executed successfully';
    PRINT '';
    
  -- Check what was actually stored
    PRINT 'Checking what was stored:';
    PRINT '-------------------------';
    
    SELECT TOP 1
        SaleID,
        PaymentMethod,
        PayerName,
 ReferenceNumber,
        BankName,
 TotalAmount,
        CASE 
  WHEN PayerName IS NULL THEN '? PayerName is NULL (PROBLEM!)'
 ELSE '? PayerName stored: ' + PayerName
   END AS PayerNameStatus,
        CASE 
        WHEN ReferenceNumber IS NULL THEN '? ReferenceNumber is NULL (PROBLEM!)'
     ELSE '? ReferenceNumber stored: ' + ReferenceNumber
        END AS ReferenceNumberStatus
    FROM RetailSalesReport
    WHERE PaymentMethod = 'GCash'
    AND TotalAmount = 999.99
    ORDER BY SaleDate DESC;

    PRINT '';
    
    -- Rollback the test insert
    ROLLBACK TRANSACTION TestInsert;
    PRINT '? Test transaction rolled back (no data was actually saved)';
    
END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION TestInsert;
    PRINT '? ERROR during test INSERT:';
    PRINT ERROR_MESSAGE();
END CATCH

PRINT '';
PRINT '========================================================';
PRINT 'Step 3: Diagnosis';
PRINT '========================================================';
PRINT '';

-- Check for triggers
DECLARE @TriggerCount INT;
SELECT @TriggerCount = COUNT(*)
FROM sys.triggers
WHERE OBJECT_NAME(parent_id) = 'RetailSalesReport';

IF @TriggerCount > 0
BEGIN
    PRINT '? WARNING: ' + CAST(@TriggerCount AS VARCHAR) + ' trigger(s) found on RetailSalesReport';
    PRINT '  Triggers may be overwriting payment details!';
    PRINT '';
    PRINT '  Run this to see trigger code:';
    PRINT '  SELECT name, OBJECT_DEFINITION(object_id) FROM sys.triggers';
    PRINT '  WHERE OBJECT_NAME(parent_id) = ''RetailSalesReport'';';
END
ELSE
BEGIN
    PRINT '? No triggers found on RetailSalesReport';
END

PRINT '';
PRINT '========================================================';
PRINT 'Test completed';
PRINT '========================================================';

GO
