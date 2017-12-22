IF NOT EXISTS(SELECT * FROM sys.columns 
            WHERE [Name] = N'EnableRegistries' AND Object_ID = Object_ID(N'Organizations'))    
BEGIN
    ALTER TABLE Organizations ADD EnableRegistries bit NOT NULL CONSTRAINT DF_Organizations_EnableRegistries DEFAULT 0
END
GO

IF NOT EXISTS(SELECT NULL FROM MigrationScript WHERE ScriptRun = '082.2013.10.07')
BEGIN
	INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '082.2013.10.07')
END
GO