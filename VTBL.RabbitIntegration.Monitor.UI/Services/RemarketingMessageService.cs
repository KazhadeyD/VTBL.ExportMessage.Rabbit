using System.Linq;
using Microsoft.EntityFrameworkCore;
using VTBL.RabbitIntegration.Monitor.Context;
using VTBL.RabbitIntegration.Monitor.Context.Entities;

namespace VTBL.RabbitIntegration.Monitor.UI.Services
{
    /// <summary>
    /// Реализация сервиса сообщений Remarketing.
    /// </summary>
    public class RemarketingMessageService
        : IntegrationMessageServiceBase<ExportMessageRabbitRemarketing, ExportMessageRabbitRemarketingStatus>,
            IRemarketingMessageService
    {
        /// <summary>
        /// Инициализирует сервис сообщений Remarketing.
        /// </summary>
        /// <param name="dbContext">Контекст базы данных.</param>
        public RemarketingMessageService(MscrmExtDbContext dbContext)
            : base(dbContext, ctx => ctx.ExportMessageRabbitRemarketings
                .AsNoTracking()
                .Include(m => m.StatusHistory.OrderBy(s => s.RowVersion))
                .Include(m => m.OperationConfiguration))
        {
        }
    }
}
