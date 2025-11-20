-- ========================================================
-- FIX RETAIL SALES REPORT TRIGGER
-- ========================================================
-- This script fixes the trigger to include payment details
-- ========================================================

USE inventorySystem;
GO

PRINT '========================================================';
PRINT 'Fixing RetailSalesReport Trigger';
PRINT '========================================================';
PRINT '';

-- Drop the old trigger
IF EXISTS (SELECT * FROM sys.triggers WHERE name = 'trg_SetRetailTotalAmount')
BEGIN
    DROP TRIGGER [dbo].[trg_SetRetailTotalAmount];
    PRINT '? Dropped old trigger: trg_SetRetailTotalAmount';
END
GO

-- Create the FIXED trigger with payment details columns
CREATE TRIGGER [dbo].[trg_SetRetailTotalAmount]
ON [dbo].[RetailSalesReport]
INSTEAD OF INSERT
AS
BEGIN
    INSERT INTO [dbo].[RetailSalesReport]
        (SaleDate, ProductID, CategoryID, QuantitySold, UnitPrice, TotalAmount, 
         PaymentMethod, HandledBy, 
      PayerName, ReferenceNumber, BankName,  -- ? ADDED THESE!
         IsRefunded, RefundDate, RefundReason)  -- Also preserve refund columns
    SELECT 
     ISNULL(i.SaleDate, GETDATE()) AS SaleDate,
i.ProductID,
   i.CategoryID,
        i.QuantitySold,
  rp.RetailPrice AS UnitPrice,
        rp.RetailPrice * i.QuantitySold AS TotalAmount,
        i.PaymentMethod,
        i.HandledBy,
     i.PayerName,   -- ? ADDED!
i.ReferenceNumber,   -- ? ADDED!
        i.BankName,    -- ? ADDED!
 ISNULL(i.IsRefunded, 0),
        i.RefundDate,
      i.RefundReason
    FROM inserted i
    JOIN retailProducts rp ON i.ProductID = rp.ProductID;
END;
GO

PRINT '? Created FIXED trigger with payment details support';
PRINT '';
PRINT '========================================================';
PRINT 'Testing the Fixed Trigger';
PRINT '========================================================';
PRINT '';

-- Test the fixed trigger
BEGIN TRANSACTION TestFixedTrigger;

    -- Insert test data
    INSERT INTO RetailSalesReport
(SaleDate, ProductID, CategoryID, QuantitySold, UnitPrice, TotalAmount,
     PaymentMethod, HandledBy, PayerName, ReferenceNumber, BankName)
    VALUES
    (GETDATE(), 1, 1, 1, 999.99, 999.99, 'GCash', 1,
     'TEST USER - FIXED', 'TEST-FIXED-12345', NULL);
    
    PRINT 'Test INSERT executed...';
    PRINT '';
    
    -- Check what was stored
    SELECT TOP 1
        SaleID,
    PaymentMethod,
     PayerName,
        ReferenceNumber,
        BankName,
   TotalAmount,
      CASE 
     WHEN PayerName IS NULL THEN '? PayerName is NULL (STILL BROKEN!)'
    ELSE '? PayerName stored: ' + PayerName
    END AS PayerNameStatus,
    CASE 
            WHEN ReferenceNumber IS NULL THEN '? ReferenceNumber is NULL (STILL BROKEN!)'
   ELSE '? ReferenceNumber stored: ' + ReferenceNumber
        END AS ReferenceNumberStatus
    FROM RetailSalesReport
    WHERE PaymentMethod = 'GCash'
    AND TotalAmount = 999.99
    ORDER BY SaleDate DESC;

ROLLBACK TRANSACTION TestFixedTrigger;

PRINT '';
PRINT '? Test transaction rolled back';
PRINT '';
PRINT '========================================================';
PRINT 'Fix Complete!';
PRINT '========================================================';
PRINT '';
PRINT 'The trigger now preserves payment details.';
PRINT 'Restart your application and test again!';
PRINT '';

GO
