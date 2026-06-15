using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VTBL.ExportMessage.Rabbit.Context.Entities;

namespace VTBL.ExportMessage.Rabbit.UI.Services
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
