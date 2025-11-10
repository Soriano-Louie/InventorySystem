-- Simple Manual Fix: Just insert a row with explicit ID
-- Run this if the FIX_SETTINGS_TABLE.sql seems too complex

-- Option 1: If table allows IDENTITY_INSERT
SET IDENTITY_INSERT settings ON
INSERT INTO settings (id, vatRate) VALUES (1, 12.00)
SET IDENTITY_INSERT settings OFF

-- Option 2: If id is NOT an identity column, just insert directly
-- INSERT INTO settings (id, vatRate) VALUES (1, 12.00)

-- Verify
SELECT * FROM settings
