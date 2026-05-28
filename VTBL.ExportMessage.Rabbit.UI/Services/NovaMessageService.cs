using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VTBL.ExportMessage.Rabbit.Context;
using VTBL.ExportMessage.Rabbit.Context.Entities;
using VTBL.ExportMessage.Rabbit.UI.Models;

namespace VTBL.ExportMessage.Rabbit.UI.Services
{
    public class NovaMessageService : IntegrationMessageServiceBase<ExportMessageRabbitNova, ExportMessageRabbitNovaStatus>, INovaMessageService
    {
        public NovaMessageService(MscrmExtDbContext dbContext)
            : base(dbContext)
        {
        }

        public async Task<NovaMessagesPageResult> GetMessagesPageAsync(
            int page,
            int pageSize,
            NovaMessageFilter filter = null,
            CancellationToken cancellationToken = default)
        {
            var result = await ((IIntegrationMessageService<ExportMessageRabbitNova>)this)
                .GetMessagesPageAsync(page, pageSize, filter, cancellationToken)
                .ConfigureAwait(false);

            return new NovaMessagesPageResult
            {
                Items = result.Items,
                TotalCount = result.TotalCount,
                Page = result.Page,
                PageSize = result.PageSize,
            };
        }

        protected override IQueryable<ExportMessageRabbitNova> BuildBaseQuery()
        {
            return DbContext.ExportMessageRabbitNovas
                .AsNoTracking()
                .Include(k => k.StatusHistory)
                .Include(k => k.OperationConfiguration);
        }

        protected override IQueryable<ExportMessageRabbitNova> ApplyMessageOrdering(IQueryable<ExportMessageRabbitNova> query)
        {
            return query.OrderByDescending(k => k.Created);
        }

        protected override IQueryable<ExportMessageRabbitNova> ApplyIdFilter(IQueryable<ExportMessageRabbitNova> query, Guid id)
        {
            return query.Where(k => k.Id == id);
        }

        protected override IQueryable<ExportMessageRabbitNova> ApplyOperationKeyFilter(IQueryable<ExportMessageRabbitNova> query, string operationKey)
        {
            return query.Where(k => k.OperationKey == operationKey);
        }

        protected override IQueryable<ExportMessageRabbitNova> ApplyWithErrorFilter(IQueryable<ExportMessageRabbitNova> query)
        {
            return query.Where(k => k.StatusHistory.Any(s =>
                s.ErrorMessage != null && s.ErrorMessage != string.Empty));
        }

        protected override IQueryable<ExportMessageRabbitNova> ApplyWithSendMessageFilter(IQueryable<ExportMessageRabbitNova> query)
        {
            return query.Where(k => k.StatusHistory.Any(s =>
                s.SendMessage != null && s.SendMessage != string.Empty));
        }

        protected override void SortMessageStatuses(ExportMessageRabbitNova message)
        {
            message.StatusHistory = message.StatusHistory
                .OrderBy(s => s.Created)
                .ToList();
        }
    }
}
