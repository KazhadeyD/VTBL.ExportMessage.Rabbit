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
    public class KkaMessageService : IntegrationMessageServiceBase<ExportMessageRabbitKka, ExportMessageRabbitKkaStatus>, IKkaMessageService
    {
        public KkaMessageService(MscrmExtDbContext dbContext)
            : base(dbContext)
        {
        }

        public async Task<KkaMessagesPageResult> GetMessagesPageAsync(
            int page,
            int pageSize,
            KkaMessageFilter filter = null,
            CancellationToken cancellationToken = default)
        {
            var result = await ((IIntegrationMessageService<ExportMessageRabbitKka>)this)
                .GetMessagesPageAsync(page, pageSize, filter, cancellationToken)
                .ConfigureAwait(false);

            return new KkaMessagesPageResult
            {
                Items = result.Items,
                TotalCount = result.TotalCount,
                Page = result.Page,
                PageSize = result.PageSize,
            };
        }

        protected override IQueryable<ExportMessageRabbitKka> BuildBaseQuery()
        {
            return DbContext.ExportMessageRabbitKkas
                .AsNoTracking()
                .Include(k => k.StatusHistory)
                .Include(k => k.OperationConfiguration);
        }

        protected override IQueryable<ExportMessageRabbitKka> ApplyMessageOrdering(IQueryable<ExportMessageRabbitKka> query)
        {
            return query.OrderByDescending(k => k.Created);
        }

        protected override IQueryable<ExportMessageRabbitKka> ApplyIdFilter(IQueryable<ExportMessageRabbitKka> query, Guid id)
        {
            return query.Where(k => k.Id == id);
        }

        protected override IQueryable<ExportMessageRabbitKka> ApplyOperationKeyFilter(IQueryable<ExportMessageRabbitKka> query, string operationKey)
        {
            return query.Where(k => k.OperationKey == operationKey);
        }

        protected override IQueryable<ExportMessageRabbitKka> ApplyWithErrorFilter(IQueryable<ExportMessageRabbitKka> query)
        {
            return query.Where(k => k.StatusHistory.Any(s =>
                s.ErrorMessage != null && s.ErrorMessage != string.Empty));
        }

        protected override IQueryable<ExportMessageRabbitKka> ApplyWithSendMessageFilter(IQueryable<ExportMessageRabbitKka> query)
        {
            return query.Where(k => k.StatusHistory.Any(s =>
                s.SendMessage != null && s.SendMessage != string.Empty));
        }

        protected override IQueryable<ExportMessageRabbitKka> ApplyCreatedFromFilter(
            IQueryable<ExportMessageRabbitKka> query,
            DateTime createdFrom)
        {
            return query.Where(k => k.Created >= createdFrom);
        }

        protected override IQueryable<ExportMessageRabbitKka> ApplyCreatedToFilter(
            IQueryable<ExportMessageRabbitKka> query,
            DateTime createdTo)
        {
            return query.Where(k => k.Created <= createdTo);
        }

        protected override void SortMessageStatuses(ExportMessageRabbitKka message)
        {
            message.StatusHistory = message.StatusHistory
                .OrderBy(s => s.RowVersion, RowVersionByteComparer.Instance)
                .ToList();
        }
    }
}
