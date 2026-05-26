SET NOCOUNT ON;

INSERT INTO [dbo].[RabbitIntegrationOperationKeysConfiguration] (
    [Id],
    [Key],
    [PackageBuilderURL],
    [SendPackageRabbitExchange],
    [SendPackageRabbitRoutingkey]
)
SELECT
    [Id],
    [Key],
    [PackageBuilderURL],
    [SendPackageRabbitExchange],
    [SendPackageRabbitRoutingkey]
FROM (
    VALUES
        (CAST(N'CA42A29F-4D8B-4428-9D43-20F9597C615F' AS uniqueidentifier), N'SendSystemuserToKKA', N'http://als-app-precrm:8100/api/KKASystemuserPackageBuilder', N'CRM-KKA.DE', N'SystemuserToKKA'),
        (CAST(N'A8037714-2E3D-47BF-A405-2E110841752E' AS uniqueidentifier), N'SendAccountCoremanagerToKKA', N'http://als-app-precrm:8100/api/KKAAccountManagerPackageBuilder', N'CRM-KKA.DE', N'SendAccountCoremanagerToKKA'),
        (CAST(N'1F1365C6-FBF0-4257-A0EB-646E7F004A2E' AS uniqueidentifier), N'SendOrderModifyToKKA', N'http://als-app-precrm:8100/api/KKAOrderModifyPackageBuilder', N'CRM-KKA.DE', N'OrderModifyToKKA'),
        (CAST(N'9975C34F-EBB9-438B-81C6-9E88A7920F6D' AS uniqueidentifier), N'SendAddproductToKKA', N'http://als-app-precrm:8100/api/KKAAdditionalProductPackageBuilder', N'CRM-KKA.DE', N'AddproductToKKA'),
        (CAST(N'787D2069-336C-422E-90C9-E019453B86BE' AS uniqueidentifier), N'SendPTSToKKA', N'http://als-app-precrm:8100/api/KKAPTSPackageBuilder', N'CRM-KKA.DE', N'SendPTSToKKA'),
        (CAST(N'1A4C4394-9ED0-43B6-BEC4-ED3481A234D9' AS uniqueidentifier), N'SendAddproductTo1C', N'http://als-app-precrm:8100/api/AdditionalProduct1CPackageBuilder', N'CRM.DE', N'AddproductTo1C'),
        (CAST(N'317F95DC-6C41-46EE-AA87-F32DC984D5B0' AS uniqueidentifier), N'SendCommentCreateToKKA', N'http://als-app-precrm:8100/api/KKAOrderCommentPackageBuilder', N'CRM-KKA.DE', N'CommentCreateToKKA')
) AS [seed] ([Id], [Key], [PackageBuilderURL], [SendPackageRabbitExchange], [SendPackageRabbitRoutingkey])
WHERE NOT EXISTS (
    SELECT 1
    FROM [dbo].[RabbitIntegrationOperationKeysConfiguration] AS [existing]
    WHERE [existing].[Id] = [seed].[Id]
);
GO

PRINT N'RabbitIntegrationOperationKeysConfiguration seed applied.';
GO
