-- ========================================================
-- FIX SALES REPORT (WHOLESALE) TRIGGER
-- ========================================================
-- This script fixes the wholesale trigger to include payment details
-- ========================================================

USE inventorySystem;
GO

PRINT '========================================================';
PRINT 'Checking SalesReport (Wholesale) Trigger';
PRINT '========================================================';
PRINT '';

-- Check if there's a similar trigger on SalesReport
DECLARE @TriggerExists INT;
SELECT @TriggerExists = COUNT(*)
FROM sys.triggers
WHERE OBJECT_NAME(parent_id) = 'SalesReport'
AND name LIKE '%Total%';

IF @TriggerExists > 0
BEGIN
    PRINT 'WARNING: Found trigger on SalesReport that may need fixing!';
    PRINT '';
    
    -- Show the trigger
    SELECT 
        name AS TriggerName,
        OBJECT_DEFINITION(object_id) AS TriggerCode
    FROM sys.triggers
    WHERE OBJECT_NAME(parent_id) = 'SalesReport';
    
    PRINT '';
    PRINT 'If this trigger has INSTEAD OF INSERT and does not include';
    PRINT 'PayerName, ReferenceNumber, and BankName columns,';
    PRINT 'it needs to be fixed the same way as the retail trigger.';
    PRINT '';
    PRINT 'Look at the trigger code above and check if it includes:';
    PRINT '  - PayerName';
    PRINT '- ReferenceNumber';
    PRINT '  - BankName';
    PRINT '';
    PRINT 'If NOT included, you need to DROP and recreate it.';
END
ELSE
BEGIN
    PRINT '? No problematic trigger found on SalesReport (Wholesale)';
    PRINT '  The wholesale sales should work correctly.';
END

PRINT '';
PRINT '========================================================';

GO
