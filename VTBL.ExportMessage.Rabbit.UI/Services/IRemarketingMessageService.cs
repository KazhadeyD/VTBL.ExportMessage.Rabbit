using System.Threading;
using System.Threading.Tasks;
using VTBL.ExportMessage.Rabbit.Context.Entities;
using VTBL.ExportMessage.Rabbit.UI.Models;

namespace VTBL.ExportMessage.Rabbit.UI.Services
{
    public interface IRemarketingMessageService : IIntegrationMessageService<ExportMessageRabbitRemarketing>
    {
        Task<RemarketingMessagesPageResult> GetMessagesPageAsync(
            int page,
            int pageSize,
            RemarketingMessageFilter filter = null,
            CancellationToken cancellationToken = default);
    }
}
