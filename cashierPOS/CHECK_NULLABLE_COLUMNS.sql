-- Check if IsDelivery allows NULL and see the actual column definition
SELECT 
    COLUMN_NAME,
  DATA_TYPE,
    IS_NULLABLE,
    COLUMN_DEFAULT,
    CHARACTER_MAXIMUM_LENGTH
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'SalesReport' 
  AND COLUMN_NAME IN ('IsDelivery', 'DeliveryAddress', 'DeliveryLatitude', 'DeliveryLongitude', 'DeliveryStatus')
ORDER BY ORDINAL_POSITION;

-- Also check if there's a NOT NULL constraint that's causing the issue
PRINT ''
PRINT 'If IsDelivery shows IS_NULLABLE = ''NO'', we need to make it nullable or provide a value'
PRINT 'Run this if IS_NULLABLE = ''NO'':'
PRINT 'ALTER TABLE SalesReport ALTER COLUMN IsDelivery BIT NULL;'
