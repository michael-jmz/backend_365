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

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240707162053_v1'
)
BEGIN
    CREATE TABLE [Usuario] (
        [Id] int NOT NULL IDENTITY,
        [Nombres] varchar(50) NOT NULL,
        [Apellidos] varchar(50) NOT NULL,
        [Cedula] varchar(50) NOT NULL,
        [CodigoDactilar] varchar(50) NOT NULL,
        [Celular] varchar(50) NOT NULL,
        [Email] varchar(50) NOT NULL,
        [Password] varchar(50) NOT NULL,
        [Provincia] varchar(50) NOT NULL,
        [FotoRostroURL] varchar(2048) NOT NULL,
        [SituacionLaboral] varchar(50) NOT NULL,
        [Empresa] varchar(50) NULL,
        [PaisPagoImpuestos] varchar(50) NOT NULL,
        [AceptoTerminosYConcidiones] bit NOT NULL,
        [Inactivo] bit NOT NULL,
        [FechaCreacion] datetime2 NOT NULL,
        CONSTRAINT [PK_Usuario] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240707162053_v1'
)
BEGIN
    CREATE TABLE [CodigoVerificacion] (
        [Id] int NOT NULL IDENTITY,
        [Codigo] varchar(50) NOT NULL,
        [FechaExpiracion] datetime2 NOT NULL,
        [IdUsuario] int NOT NULL,
        [FechaCreacion] datetime2 NOT NULL,
        CONSTRAINT [PK_CodigoVerificacion] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_CodigoVerificacion_Usuario_IdUsuario] FOREIGN KEY ([IdUsuario]) REFERENCES [Usuario] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240707162053_v1'
)
BEGIN
    CREATE TABLE [Cuenta] (
        [Id] int NOT NULL IDENTITY,
        [Numero] varchar(50) NOT NULL,
        [Saldo] float NOT NULL,
        [IdUsuario] int NOT NULL,
        [Inactivo] bit NOT NULL,
        [FechaCreacion] datetime2 NOT NULL,
        CONSTRAINT [PK_Cuenta] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Cuenta_Usuario_IdUsuario] FOREIGN KEY ([IdUsuario]) REFERENCES [Usuario] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240707162053_v1'
)
BEGIN
    CREATE INDEX [IX_CodigoVerificacion_IdUsuario] ON [CodigoVerificacion] ([IdUsuario]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240707162053_v1'
)
BEGIN
    CREATE INDEX [IX_Cuenta_IdUsuario] ON [Cuenta] ([IdUsuario]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240707162053_v1'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240707162053_v1', N'8.0.6');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240707171655_v2'
)
BEGIN
    ALTER TABLE [Usuario] ADD [CodigoVerificado] bit NOT NULL DEFAULT CAST(0 AS bit);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240707171655_v2'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240707171655_v2', N'8.0.6');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240711044341_v3'
)
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Usuario]') AND [c].[name] = N'Password');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Usuario] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [Usuario] ALTER COLUMN [Password] varchar(2048) NOT NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240711044341_v3'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240711044341_v3', N'8.0.6');
END;
GO

COMMIT;
GO