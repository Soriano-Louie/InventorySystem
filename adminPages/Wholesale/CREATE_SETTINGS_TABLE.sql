    -- Create the settings table if it doesn't exist
    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'settings')
    BEGIN
        CREATE TABLE settings (
            id INT IDENTITY(1,1) PRIMARY KEY,
            vatRate DECIMAL(5, 2) NOT NULL DEFAULT 0,
            CreatedDate DATETIME DEFAULT GETDATE(),
            ModifiedDate DATETIME DEFAULT GETDATE()
        )
    
        -- Insert a default VAT rate of 0%
        INSERT INTO settings (vatRate) VALUES (0)
    
        PRINT 'Settings table created successfully'
    END
    ELSE
    BEGIN
        PRINT 'Settings table already exists'
    END

    -- Verify the table structure
    SELECT * FROM settings
