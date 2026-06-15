using System.Linq;
using Microsoft.EntityFrameworkCore;
using VTBL.ExportMessage.Rabbit.Context;
using VTBL.ExportMessage.Rabbit.Context.Entities;

namespace VTBL.ExportMessage.Rabbit.UI.Services
{
    /// <summary>
    /// Реализация сервиса сообщений ККА.
    /// </summary>
    public class KkaMessageService
        : IntegrationMessageServiceBase<ExportMessageRabbitKka, ExportMessageRabbitKkaStatus>,
            IKkaMessageService
    {
        /// <summary>
        /// Инициализирует сервис сообщений ККА.
        /// </summary>
        /// <param name="dbContext">Контекст базы данных.</param>
        public KkaMessageService(MscrmExtDbContext dbContext)
            : base(dbContext, ctx => ctx.ExportMessageRabbitKkas
                .AsNoTracking()
                .Include(m => m.StatusHistory.OrderBy(s => s.RowVersion))
                .Include(m => m.OperationConfiguration))
        {
        }
    }
}
