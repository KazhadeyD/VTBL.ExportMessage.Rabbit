SET NOCOUNT ON;

INSERT INTO [dbo].[ExportMessageRabbitStatusName] ([Id], [StatusName])
SELECT [Id], [StatusName]
FROM (
    VALUES
        (1, N'Ready'),
        (2, N'InProcessed'),
        (3, N'Send'),
        (4, N'Close'),
        (5, N'Error')
) AS [seed] ([Id], [StatusName])
WHERE NOT EXISTS (
    SELECT 1
    FROM [dbo].[ExportMessageRabbitStatusName] AS [existing]
    WHERE [existing].[Id] = [seed].[Id]
);
GO

PRINT N'ExportMessageRabbitStatusName seed applied.';
GO
