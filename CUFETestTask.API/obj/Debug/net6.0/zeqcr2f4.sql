IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Users] (
    [UserId] int NOT NULL IDENTITY,
    [UserName] nvarchar(150) NOT NULL,
    [Password] nvarchar(max) NOT NULL,
    [FirstName] nvarchar(50) NOT NULL,
    [FatherName] nvarchar(50) NOT NULL,
    [FamilyName] nvarchar(50) NOT NULL,
    [Occupation] nvarchar(max) NOT NULL,
    [Address] nvarchar(max) NOT NULL,
    [BirthDate] datetime2 NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([UserId])
);
GO

CREATE UNIQUE INDEX [IX_Users_UserName] ON [Users] ([UserName]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220520154350_UserDbMigration', N'6.0.5');
GO

COMMIT;
GO

