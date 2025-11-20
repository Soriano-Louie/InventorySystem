-- ========================================================
-- CHECK AND FIX PAYMENT DETAILS TRIGGERS
-- ========================================================
-- This script checks for triggers that might be interfering
-- with payment details storage
-- ========================================================

USE inventorySystem;
GO

PRINT '========================================================';
PRINT 'Checking for Triggers on Sales Tables';
PRINT '========================================================';
PRINT '';

-- ========================================================
-- CHECK FOR TRIGGERS ON RetailSalesReport
-- ========================================================

PRINT 'Triggers on RetailSalesReport:';
PRINT '-------------------------------';

SELECT 
    t.name AS TriggerName,
    OBJECT_NAME(t.parent_id) AS TableName,
    t.is_disabled AS IsDisabled,
    t.create_date AS CreateDate,
    t.modify_date AS ModifyDate
FROM sys.triggers t
WHERE OBJECT_NAME(t.parent_id) = 'RetailSalesReport';

PRINT '';

-- ========================================================
-- CHECK FOR TRIGGERS ON SalesReport
-- ========================================================

PRINT 'Triggers on SalesReport:';
PRINT '------------------------';

SELECT 
    t.name AS TriggerName,
    OBJECT_NAME(t.parent_id) AS TableName,
    t.is_disabled AS IsDisabled,
    t.create_date AS CreateDate,
    t.modify_date AS ModifyDate
FROM sys.triggers t
WHERE OBJECT_NAME(t.parent_id) = 'SalesReport';

PRINT '';
PRINT '========================================================';
PRINT 'Trigger Details and Recommendations';
PRINT '========================================================';
PRINT '';

-- Get trigger definitions
DECLARE @TriggerCount INT;

SELECT @TriggerCount = COUNT(*)
FROM sys.triggers t
WHERE OBJECT_NAME(t.parent_id) IN ('RetailSalesReport', 'SalesReport');

IF @TriggerCount > 0
BEGIN
    PRINT 'WARNING: Triggers found that may interfere with payment details!';
    PRINT '';
    PRINT 'Trigger Definitions:';
    PRINT '-------------------';
    
    -- Show trigger text for RetailSalesReport
    SELECT 
t.name AS TriggerName,
        OBJECT_DEFINITION(t.object_id) AS TriggerDefinition
    FROM sys.triggers t
    WHERE OBJECT_NAME(t.parent_id) = 'RetailSalesReport';
    
    -- Show trigger text for SalesReport
    SELECT 
 t.name AS TriggerName,
        OBJECT_DEFINITION(t.object_id) AS TriggerDefinition
    FROM sys.triggers t
    WHERE OBJECT_NAME(t.parent_id) = 'SalesReport';
    
    PRINT '';
    PRINT 'RECOMMENDED ACTION:';
    PRINT 'If a trigger is updating these columns to NULL, you need to:';
    PRINT '1. Either DROP the trigger, or';
  PRINT '2. Modify the trigger to preserve PayerName, ReferenceNumber, and BankName values';
    PRINT '';
END
ELSE
BEGIN
    PRINT '? No triggers found on sales tables';
    PRINT '  The issue may be elsewhere in the application code.';
END

PRINT '';
PRINT '========================================================';
PRINT 'Test Insert Query';
PRINT '========================================================';
PRINT '';
PRINT 'To test if triggers are the issue, try this INSERT:';
PRINT '';
PRINT 'INSERT INTO RetailSalesReport';
PRINT '(SaleDate, ProductID, CategoryID, QuantitySold, UnitPrice, TotalAmount,';
PRINT ' PaymentMethod, HandledBy, PayerName, ReferenceNumber, BankName)';
PRINT 'VALUES';
PRINT '(GETDATE(), 1, 1, 1, 100.00, 100.00, ''GCash'', 1,';
PRINT ' ''Test User'', ''TEST-12345'', NULL)';
PRINT '';
PRINT 'Then check if the values were preserved:';
PRINT '';
PRINT 'SELECT TOP 1 PayerName, ReferenceNumber, BankName, PaymentMethod';
PRINT 'FROM RetailSalesReport';
PRINT 'WHERE PaymentMethod = ''GCash''';
PRINT 'ORDER BY SaleDate DESC;';
PRINT '';

GO
