SET ANSI_NULLS ON;
GO

SET QUOTED_IDENTIFIER ON;
GO

SET NOCOUNT ON;

IF OBJECT_ID(N'dbo.ExportMessageRabbitKKA', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[ExportMessageRabbitKKA](
        [Id] [uniqueidentifier] NOT NULL,
        [MessageId] [uniqueidentifier] NULL,
        [OperationKey] [nvarchar](100) NOT NULL,
        [Endpoint] [nvarchar](100) NOT NULL,
        [Created] [datetime] NOT NULL,
        [Body] [nvarchar](max) NULL,
     CONSTRAINT [PK_ExportMessageRabbitKKA] PRIMARY KEY CLUSTERED
    (
        [Id] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];

    PRINT N'Table dbo.ExportMessageRabbitKKA created.';
END
ELSE
    PRINT N'Table dbo.ExportMessageRabbitKKA already exists.';
GO

IF OBJECT_ID(N'dbo.ExportMessageRabbitKKAStatus', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[ExportMessageRabbitKKAStatus](
        [Id] [uniqueidentifier] NOT NULL,
        [IntegrationId] [uniqueidentifier] NOT NULL,
        [StatusId] [int] NULL,
        [Created] [datetime] NOT NULL,
        [ProcessingId] [uniqueidentifier] NULL,
        [ErrorMessage] [nvarchar](max) NULL,
        [SendMessage] [nvarchar](max) NULL,
        [RowVersion] [timestamp] NOT NULL,
     CONSTRAINT [PK_ExportMessageRabbitKKAStatus] PRIMARY KEY CLUSTERED
    (
        [Id] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];

    ALTER TABLE [dbo].[ExportMessageRabbitKKAStatus] ADD DEFAULT (newid()) FOR [Id];
    ALTER TABLE [dbo].[ExportMessageRabbitKKAStatus] ADD DEFAULT (getdate()) FOR [Created];

    PRINT N'Table dbo.ExportMessageRabbitKKAStatus created.';
END
ELSE
    PRINT N'Table dbo.ExportMessageRabbitKKAStatus already exists.';
GO

IF OBJECT_ID(N'dbo.ExportMessageRabbitNOVA', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[ExportMessageRabbitNOVA](
        [Id] [uniqueidentifier] NOT NULL,
        [MessageId] [uniqueidentifier] NULL,
        [OperationKey] [nvarchar](100) NOT NULL,
        [Endpoint] [nvarchar](100) NOT NULL,
        [Created] [datetime] NOT NULL,
        [Body] [nvarchar](max) NULL,
     CONSTRAINT [PK_ExportMessageRabbitNOVA] PRIMARY KEY CLUSTERED
    (
        [Id] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];

    PRINT N'Table dbo.ExportMessageRabbitNOVA created.';
END
ELSE
    PRINT N'Table dbo.ExportMessageRabbitNOVA already exists.';
GO

IF OBJECT_ID(N'dbo.ExportMessageRabbitNOVAStatus', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[ExportMessageRabbitNOVAStatus](
        [Id] [uniqueidentifier] NOT NULL,
        [IntegrationId] [uniqueidentifier] NOT NULL,
        [StatusId] [int] NULL,
        [Created] [datetime] NOT NULL,
        [ProcessingId] [uniqueidentifier] NULL,
        [ErrorMessage] [nvarchar](max) NULL,
        [SendMessage] [nvarchar](max) NULL,
        [RowVersion] [timestamp] NOT NULL,
     CONSTRAINT [PK_ExportMessageRabbitNOVAStatus] PRIMARY KEY CLUSTERED
    (
        [Id] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];

    ALTER TABLE [dbo].[ExportMessageRabbitNOVAStatus] ADD DEFAULT (newid()) FOR [Id];
    ALTER TABLE [dbo].[ExportMessageRabbitNOVAStatus] ADD DEFAULT (getdate()) FOR [Created];

    PRINT N'Table dbo.ExportMessageRabbitNOVAStatus created.';
END
ELSE
    PRINT N'Table dbo.ExportMessageRabbitNOVAStatus already exists.';
GO

IF OBJECT_ID(N'dbo.ExportMessageRabbitStatusName', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[ExportMessageRabbitStatusName](
        [Id] [int] NULL,
        [StatusName] [nvarchar](100) NOT NULL
    ) ON [PRIMARY];

    PRINT N'Table dbo.ExportMessageRabbitStatusName created.';
END
ELSE
    PRINT N'Table dbo.ExportMessageRabbitStatusName already exists.';
GO

IF OBJECT_ID(N'dbo.RabbitIntegrationOperationKeysConfiguration', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[RabbitIntegrationOperationKeysConfiguration](
        [Id] [uniqueidentifier] NOT NULL,
        [Key] [nvarchar](max) NOT NULL,
        [PackageBuilderURL] [nvarchar](max) NOT NULL,
        [SendPackageRabbitExchange] [nvarchar](max) NOT NULL,
        [SendPackageRabbitRoutingkey] [nvarchar](max) NOT NULL,
     CONSTRAINT [PK_RabbitIntegrationOperationKeysConfiguration] PRIMARY KEY CLUSTERED
    (
        [Id] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];

    ALTER TABLE [dbo].[RabbitIntegrationOperationKeysConfiguration] ADD DEFAULT (newid()) FOR [Id];

    PRINT N'Table dbo.RabbitIntegrationOperationKeysConfiguration created.';
END
ELSE
    PRINT N'Table dbo.RabbitIntegrationOperationKeysConfiguration already exists.';
GO
