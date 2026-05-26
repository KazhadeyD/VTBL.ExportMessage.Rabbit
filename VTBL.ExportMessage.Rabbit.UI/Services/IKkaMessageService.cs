using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VTBL.ExportMessage.Rabbit.Context.Entities;

namespace VTBL.ExportMessage.Rabbit.UI.Services
{
    public interface IKkaMessageService
    {
        Task<IReadOnlyList<ExportMessageRabbitKka>> GetMessagesGroupedByIdAsync(
            CancellationToken cancellationToken = default);

        Task<IReadOnlyDictionary<int, string>> GetStatusNameMapAsync(
            CancellationToken cancellationToken = default);
    }
}
