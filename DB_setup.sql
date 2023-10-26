:setvar DB TSD

IF NOT EXISTS(SELECT Name FROM sys.databases WHERE Name = '$(DB)') BEGIN
	CREATE DATABASE $(DB);
END

GO

USE $(DB);

GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Banks') BEGIN
	CREATE TABLE Banks (
		bank_name VARCHAR(255) NOT NULL, 
		password VARCHAR(255) NOT NULL, 
		CONSTRAINT PK_BANKS PRIMARY KEY (bank_name)
	);
END

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Users') BEGIN
	CREATE TABLE Users(
		username VARCHAR(255) NOT NULL,
		password VARCHAR(255) NOT NULL,
		bank_name VARCHAR(255) NOT NULL,
		CONSTRAINT PK_USERS PRIMARY KEY (username),
		CONSTRAINT FK_BANKS_USERS FOREIGN KEY (bank_name) REFERENCES Banks(bank_name) ON DELETE CASCADE ON UPDATE CASCADE
	);
END

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Branches') BEGIN
	CREATE TABLE Branches (
		bank_name VARCHAR(255) NOT NULL, 
		branch_id INT IDENTITY(1, 1) NOT NULL,
		name_en VARCHAR(100) NOT NULL,
		name_ar NVARCHAR(100) NOT NULL,
		active BIT NOT NULL, 
		CONSTRAINT PK_BRANCHES PRIMARY KEY (bank_name, branch_id), 
		CONSTRAINT FK_BANKS_BRANCHES FOREIGN KEY (bank_name) REFERENCES Banks(bank_name) ON DELETE NO ACTION ON UPDATE NO ACTION
	);
END

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'BankServices') BEGIN
	CREATE TABLE BankServices (
		bank_name VARCHAR(255) NOT NULL,
		service_id INT IDENTITY(1, 1) NOT NULL,
		name_en VARCHAR(100) NOT NULL,
		name_ar NVARCHAR(100) NOT NULL,
		active BIT NOT NULL,
		max_daily_tickets INT NOT NULL,
		min_service_time INT DEFAULT 45 NOT NULL,
		max_service_time INT DEFAULT 300 NOT NULL,
		CONSTRAINT PK_BANKSERVICES PRIMARY KEY (bank_name, service_id),
		CONSTRAINT FK_BANKS_BANKSERVICES FOREIGN KEY (bank_name) REFERENCES Banks(bank_name) ON DELETE NO ACTION ON UPDATE NO ACTION,
		CONSTRAINT CHECK_MAX_DAILY_TICKETS CHECK(max_daily_tickets >= 1 AND max_daily_tickets <= 100)
	);
END
ELSE BEGIN
	IF COL_LENGTH('dbo.BankServices', 'min_service_time') IS NULL BEGIN
		ALTER TABLE BankServices ADD min_service_time INT DEFAULT 45 NOT NULL;
	END
	IF COL_LENGTH('dbo.BankServices', 'max_service_time') IS NULL BEGIN
		ALTER TABLE BankServices ADD max_service_time INT DEFAULT 300 NOT NULL;
	END
END

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Counters') BEGIN
	CREATE TABLE Counters (
		bank_name VARCHAR(255) NOT NULL,
		branch_id INT NOT NULL,
		counter_id INT IDENTITY(1, 1) NOT NULL,
		name_en VARCHAR(100) NOT NULL,
		name_ar NVARCHAR(100) NOT NULL,
		active BIT NOT NULL,
		type INT NOT NULL,
		CONSTRAINT PK_COUNTERS PRIMARY KEY (bank_name, branch_id, counter_id),
		CONSTRAINT FK_COUNTERS_BRANCHES FOREIGN KEY (bank_name, branch_id) REFERENCES Branches(bank_name, branch_id) ON DELETE NO ACTION ON UPDATE NO ACTION
	);
END

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'ServicesCounters') BEGIN
	CREATE TABLE ServicesCounters (
		bank_name VARCHAR(255) NOT NULL,
		branch_id INT NOT NULL,
		counter_id INT NOT NULL,
		service_id INT NOT NULL,
		CONSTRAINT PK_SERVICESCOUNTERS PRIMARY KEY (bank_name, branch_id, counter_id, service_id),
		CONSTRAINT FK_BANKSERVICES_SERVICESCOUNTERS FOREIGN KEY (bank_name, service_id) REFERENCES BankServices(bank_name, service_id) ON DELETE NO ACTION ON UPDATE NO ACTION,
		CONSTRAINT FK_COUNTERS_SERVICESCOUNTERS FOREIGN KEY (bank_name, branch_id, counter_id) REFERENCES Counters(bank_name, branch_id, counter_id) ON DELETE NO ACTION ON UPDATE NO ACTION
	);
END

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Sessions') BEGIN
	CREATE TABLE Sessions (
		username VARCHAR(255) NOT NULL,
		session_id INT NOT NULL,
		expires DATETIME NOT NULL,
		user_agent VARCHAR(255) NOT NULL,
		ip_address VARCHAR(39) NOT NULL,
		CONSTRAINT PK_SESSIONS PRIMARY KEY (session_id),
		CONSTRAINT FK_USERS_SESSIONS FOREIGN KEY (username) REFERENCES Users(username) ON DELETE CASCADE ON UPDATE CASCADE,
	);
END

IF EXISTS(SELECT * FROM sys.triggers WHERE name = 'CascadeOnBankDelete') BEGIN
	DROP TRIGGER dbo.CascadeOnBankDelete;
END

IF EXISTS(SELECT * FROM sys.triggers WHERE name = 'CascadeOnBranchDelete') BEGIN
	DROP TRIGGER dbo.CascadeOnBranchDelete;
END

IF EXISTS(SELECT * FROM sys.triggers WHERE name = 'CascadeOnServiceDelete') BEGIN
	DROP TRIGGER dbo.CascadeOnServiceDelete;
END

IF EXISTS(SELECT * FROM sys.triggers WHERE name = 'CascadeOnCounterDelete') BEGIN
	DROP TRIGGER dbo.CascadeOnCounterDelete;
END

GO

CREATE TRIGGER CascadeOnBankDelete
	ON Banks
	AFTER DELETE
AS BEGIN
	DELETE FROM Branches WHERE Branches.bank_name IN (SELECT deleted.bank_name FROM deleted);
	DELETE FROM BankServices WHERE BankServices.bank_name IN (SELECT deleted.bank_name FROM deleted);
END	

GO

CREATE TRIGGER CascadeOnBranchDelete
	ON Branches
	INSTEAD OF DELETE
AS BEGIN
	DELETE FROM ServicesCounters WHERE ServicesCounters.bank_name IN (SELECT deleted.bank_name FROM deleted) AND ServicesCounters.branch_id IN (SELECT deleted.branch_id FROM deleted);
	DELETE FROM Counters WHERE Counters.bank_name IN (SELECT deleted.bank_name FROM deleted) AND Counters.branch_id IN (SELECT deleted.branch_id FROM deleted);
	DELETE FROM Branches WHERE Branches.bank_name IN (SELECT deleted.bank_name FROM deleted) AND Branches.branch_id IN (SELECT deleted.branch_id FROM deleted);
END

GO

CREATE TRIGGER CascadeOnServiceDelete
	ON BankServices
	INSTEAD OF DELETE
AS BEGIN
	DELETE FROM ServicesCounters WHERE ServicesCounters.service_id IN (SELECT deleted.service_id FROM deleted) AND ServicesCounters.bank_name IN (SELECT deleted.bank_name FROM deleted);
	DELETE FROM BankServices WHERE BankServices.bank_name IN (SELECT deleted.bank_name FROM deleted) AND BankServices.service_id IN (SELECT deleted.service_id FROM deleted);
END

GO

CREATE TRIGGER CascadeOnCounterDelete
	ON Counters
	INSTEAD OF DELETE
AS BEGIN
	DELETE FROM ServicesCounters WHERE ServicesCounters.bank_name IN (SELECT deleted.bank_name FROM deleted) AND ServicesCounters.branch_id IN (SELECT deleted.branch_id FROM deleted) AND ServicesCounters.counter_id IN (SELECT deleted.counter_id FROM deleted);
	DELETE FROM Counters WHERE Counters.bank_name IN (SELECT deleted.bank_name FROM deleted) AND Counters.branch_id IN (SELECT deleted.branch_id FROM deleted) AND Counters.counter_id IN (SELECT deleted.counter_id FROM deleted);
END

GO

DROP PROCEDURE IF EXISTS AddServicesToCounters;
DROP TYPE IF EXISTS add_service_counter_parameter;

CREATE TYPE add_service_counter_parameter AS TABLE(
	bank_name VARCHAR(255) NOT NULL,
	branch_id INT NOT NULL,
	counter_id INT NOT NULL,
	service_id INT NOT NULL
);

GO

CREATE PROCEDURE AddServicesToCounters(@service_counters add_service_counter_parameter READONLY) AS BEGIN
	DECLARE @bank_name VARCHAR(255);
	DECLARE @branch_id INT;
	DECLARE @counter_id INT;
	DECLARE @service_id INT;

	DECLARE parameter_cursor CURSOR FOR SELECT * FROM @service_counters;
	OPEN parameter_cursor;

	FETCH NEXT FROM parameter_cursor INTO @bank_name, @branch_id, @counter_id, @service_id;

	WHILE @@FETCH_STATUS = 0 BEGIN
		INSERT INTO ServicesCounters (bank_name, branch_id, counter_id, service_id) VALUES (@bank_name, @branch_id, @counter_id, @service_id);
		FETCH NEXT FROM parameter_cursor INTO @bank_name, @branch_id, @counter_id, @service_id;
	END
END
