using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VTBL.ExportMessage.Rabbit.UI.Models;

namespace VTBL.ExportMessage.Rabbit.UI.Services
{
    public class ExportMessageRabbitStatusNameService : IExportMessageRabbitStatusNameService
    {
        private readonly string _connectionString;

        public ExportMessageRabbitStatusNameService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MSCRM_EXT")
                ?? throw new InvalidOperationException("Connection string 'MSCRM_EXT' is not configured.");
        }

        public async Task<IReadOnlyList<ExportMessageRabbitStatusName>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var items = new List<ExportMessageRabbitStatusName>();

            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken).ConfigureAwait(false);

            const string sql = @"
SELECT [Id], [StatusName]
FROM [dbo].[ExportMessageRabbitStatusName]
ORDER BY [Id];";

            await using var command = new SqlCommand(sql, connection);
            await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);

            while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
            {
                items.Add(new ExportMessageRabbitStatusName
                {
                    Id = reader.IsDBNull(0) ? null : reader.GetInt32(0),
                    StatusName = reader.GetString(1),
                });
            }

            return items;
        }
    }
}
