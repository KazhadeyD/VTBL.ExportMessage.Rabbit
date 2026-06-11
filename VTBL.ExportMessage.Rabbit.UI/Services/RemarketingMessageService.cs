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
    public class RemarketingMessageService
        : IntegrationMessageServiceBase<ExportMessageRabbitRemarketing, ExportMessageRabbitRemarketingStatus>,
            IRemarketingMessageService
    {
        public RemarketingMessageService(MscrmExtDbContext dbContext)
            : base(dbContext)
        {
        }

        public async Task<RemarketingMessagesPageResult> GetMessagesPageAsync(
            int page,
            int pageSize,
            RemarketingMessageFilter filter = null,
            CancellationToken cancellationToken = default)
        {
            var result = await ((IIntegrationMessageService<ExportMessageRabbitRemarketing>)this)
                .GetMessagesPageAsync(page, pageSize, filter, cancellationToken)
                .ConfigureAwait(false);

            return new RemarketingMessagesPageResult
            {
                Items = result.Items,
                TotalCount = result.TotalCount,
                Page = result.Page,
                PageSize = result.PageSize,
            };
        }

        protected override IQueryable<ExportMessageRabbitRemarketing> BuildBaseQuery()
        {
            return DbContext.ExportMessageRabbitRemarketings
                .AsNoTracking()
                .Include(k => k.StatusHistory)
                .Include(k => k.OperationConfiguration);
        }

        protected override IQueryable<ExportMessageRabbitRemarketing> ApplyMessageOrdering(
            IQueryable<ExportMessageRabbitRemarketing> query)
        {
            return query.OrderByDescending(k => k.Created);
        }

        protected override IQueryable<ExportMessageRabbitRemarketing> ApplyIdFilter(
            IQueryable<ExportMessageRabbitRemarketing> query,
            Guid id)
        {
            return query.Where(k => k.Id == id);
        }

        protected override IQueryable<ExportMessageRabbitRemarketing> ApplyOperationKeyFilter(
            IQueryable<ExportMessageRabbitRemarketing> query,
            string operationKey)
        {
            return query.Where(k => k.OperationKey == operationKey);
        }

        protected override IQueryable<ExportMessageRabbitRemarketing> ApplyWithErrorFilter(
            IQueryable<ExportMessageRabbitRemarketing> query)
        {
            return query.Where(k => k.StatusHistory.Any(s =>
                s.ErrorMessage != null && s.ErrorMessage != string.Empty));
        }

        protected override IQueryable<ExportMessageRabbitRemarketing> ApplyWithSendMessageFilter(
            IQueryable<ExportMessageRabbitRemarketing> query)
        {
            return query.Where(k => k.StatusHistory.Any(s =>
                s.SendMessage != null && s.SendMessage != string.Empty));
        }

        protected override IQueryable<ExportMessageRabbitRemarketing> ApplyCreatedFromFilter(
            IQueryable<ExportMessageRabbitRemarketing> query,
            DateTime createdFrom)
        {
            return query.Where(k => k.Created >= createdFrom);
        }

        protected override IQueryable<ExportMessageRabbitRemarketing> ApplyCreatedToFilter(
            IQueryable<ExportMessageRabbitRemarketing> query,
            DateTime createdTo)
        {
            return query.Where(k => k.Created <= createdTo);
        }

        protected override void SortMessageStatuses(ExportMessageRabbitRemarketing message)
        {
            message.StatusHistory = message.StatusHistory
                .OrderBy(s => s.Created)
                .ToList();
        }
    }
}
