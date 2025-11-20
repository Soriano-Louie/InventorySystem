-- ========================================================
-- VIEW TRIGGER DEFINITIONS
-- ========================================================
-- Simple script to view trigger code
-- ========================================================

USE inventorySystem;
GO

-- ========================================================
-- SHOW TRIGGERS ON RetailSalesReport
-- ========================================================

PRINT '========================================================';
PRINT 'TRIGGER DEFINITIONS - RetailSalesReport';
PRINT '========================================================';
PRINT '';

SELECT 
    t.name AS TriggerName,
    t.is_disabled AS IsDisabled,
    OBJECT_DEFINITION(t.object_id) AS TriggerCode
FROM sys.triggers t
WHERE OBJECT_NAME(t.parent_id) = 'RetailSalesReport';

GO

PRINT '';
PRINT '========================================================';
PRINT 'TRIGGER DEFINITIONS - SalesReport';
PRINT '========================================================';
PRINT '';

SELECT 
    t.name AS TriggerName,
    t.is_disabled AS IsDisabled,
    OBJECT_DEFINITION(t.object_id) AS TriggerCode
FROM sys.triggers t
WHERE OBJECT_NAME(t.parent_id) = 'SalesReport';

GO
