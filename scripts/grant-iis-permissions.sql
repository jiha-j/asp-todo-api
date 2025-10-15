-- IIS 애플리케이션 풀 사용자에게 SQL Server 권한 부여
USE [master];
GO

-- IIS 애플리케이션 풀 사용자 로그인 생성
IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = N'IIS APPPOOL\DefaultAppPool')
BEGIN
    CREATE LOGIN [IIS APPPOOL\DefaultAppPool] FROM WINDOWS WITH DEFAULT_DATABASE=[TodoDb];
    PRINT 'Login created for IIS APPPOOL\DefaultAppPool';
END
ELSE
BEGIN
    PRINT 'Login already exists for IIS APPPOOL\DefaultAppPool';
END
GO

-- TodoDb 데이터베이스로 전환
USE [TodoDb];
GO

-- 데이터베이스 사용자 생성
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'IIS APPPOOL\DefaultAppPool')
BEGIN
    CREATE USER [IIS APPPOOL\DefaultAppPool] FOR LOGIN [IIS APPPOOL\DefaultAppPool];
    PRINT 'User created in TodoDb database';
END
ELSE
BEGIN
    PRINT 'User already exists in TodoDb database';
END
GO

-- db_datareader 및 db_datawriter 역할 부여
ALTER ROLE [db_datareader] ADD MEMBER [IIS APPPOOL\DefaultAppPool];
ALTER ROLE [db_datawriter] ADD MEMBER [IIS APPPOOL\DefaultAppPool];
PRINT 'Granted db_datareader and db_datawriter roles';
GO

-- 필요 시 ddladmin 역할 부여 (스키마 변경 권한)
-- ALTER ROLE [db_ddladmin] ADD MEMBER [IIS APPPOOL\DefaultAppPool];
-- PRINT 'Granted db_ddladmin role';
-- GO

PRINT 'Permissions granted successfully!';
GO
