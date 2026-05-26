using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VTBL.ExportMessage.Rabbit.Context.Entities;

namespace VTBL.ExportMessage.Rabbit.UI.Services
{
    public interface IExportMessageRabbitStatusNameService
    {
        Task<IReadOnlyList<ExportMessageRabbitStatusName>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
