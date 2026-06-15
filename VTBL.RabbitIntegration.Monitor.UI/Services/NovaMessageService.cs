using System.Linq;
using Microsoft.EntityFrameworkCore;
using VTBL.RabbitIntegration.Monitor.Context;
using VTBL.RabbitIntegration.Monitor.Context.Entities;

namespace VTBL.RabbitIntegration.Monitor.UI.Services
{
    /// <summary>
    /// Реализация сервиса сообщений NOVA.
    /// </summary>
    public class NovaMessageService
        : IntegrationMessageServiceBase<ExportMessageRabbitNova, ExportMessageRabbitNovaStatus>,
            INovaMessageService
    {
        /// <summary>
        /// Инициализирует сервис сообщений NOVA.
        /// </summary>
        /// <param name="dbContext">Контекст базы данных.</param>
        public NovaMessageService(MscrmExtDbContext dbContext)
            : base(dbContext, ctx => ctx.ExportMessageRabbitNovas
                .AsNoTracking()
                .Include(m => m.StatusHistory.OrderBy(s => s.RowVersion))
                .Include(m => m.OperationConfiguration))
        {
        }
    }
}
