using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VTBL.RabbitIntegration.Monitor.Context.Entities;

namespace VTBL.RabbitIntegration.Monitor.UI.Services
{
    /// <summary>
    /// Сервис доступа к справочнику наименований статусов Rabbit.
    /// </summary>
    public interface IExportMessageRabbitStatusNameService
    {
        /// <summary>
        /// Возвращает полный список наименований статусов.
        /// </summary>
        Task<IReadOnlyList<ExportMessageRabbitStatusName>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
