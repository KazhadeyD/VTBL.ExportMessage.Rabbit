using System.Threading;
using System.Threading.Tasks;
using VTBL.ExportMessage.Rabbit.Context.Entities;
using VTBL.ExportMessage.Rabbit.UI.Models;

namespace VTBL.ExportMessage.Rabbit.UI.Services
{
    public interface INovaMessageService : IIntegrationMessageService<ExportMessageRabbitNova>
    {
        Task<NovaMessagesPageResult> GetMessagesPageAsync(
            int page,
            int pageSize,
            NovaMessageFilter filter = null,
            CancellationToken cancellationToken = default);
    }
}
