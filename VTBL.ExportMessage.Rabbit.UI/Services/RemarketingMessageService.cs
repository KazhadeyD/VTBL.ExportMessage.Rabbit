using System.Linq;
using Microsoft.EntityFrameworkCore;
using VTBL.ExportMessage.Rabbit.Context;
using VTBL.ExportMessage.Rabbit.Context.Entities;

namespace VTBL.ExportMessage.Rabbit.UI.Services
{
    public class RemarketingMessageService
        : IntegrationMessageServiceBase<ExportMessageRabbitRemarketing, ExportMessageRabbitRemarketingStatus>,
            IRemarketingMessageService
    {
        public RemarketingMessageService(MscrmExtDbContext dbContext)
            : base(dbContext, ctx => ctx.ExportMessageRabbitRemarketings
                .AsNoTracking()
                .Include(m => m.StatusHistory.OrderBy(s => s.RowVersion))
                .Include(m => m.OperationConfiguration))
        {
        }
    }
}
