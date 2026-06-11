using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VTBL.ExportMessage.Rabbit.Context;
using VTBL.ExportMessage.Rabbit.UI.Models;

namespace VTBL.ExportMessage.Rabbit.UI.Services
{
    public abstract class IntegrationMessageServiceBase<TMessage, TStatus> : IIntegrationMessageService<TMessage>
        where TMessage : class
    {
        protected readonly MscrmExtDbContext DbContext;

        protected IntegrationMessageServiceBase(MscrmExtDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<IntegrationMessagesPageResult<TMessage>> GetMessagesPageAsync(
            int page,
            int pageSize,
            IntegrationMessageFilter filter = null,
            CancellationToken cancellationToken = default)
        {
            var query = BuildBaseQuery();

            if (filter != null)
            {
                if (filter.HasId)
                {
                    query = ApplyIdFilter(query, filter.Id.Value);
                }

                if (filter.HasOperationKey)
                {
                    query = ApplyOperationKeyFilter(query, filter.OperationKey);
                }

                if (filter.WithError)
                {
                    query = ApplyWithErrorFilter(query);
                }

                if (filter.WithSendMessage)
                {
                    query = ApplyWithSendMessageFilter(query);
                }

                if (filter.HasCreatedFrom)
                {
                    query = ApplyCreatedFromFilter(query, filter.CreatedFrom.Value);
                }

                if (filter.HasCreatedTo)
                {
                    query = ApplyCreatedToFilter(query, filter.CreatedTo.Value);
                }
            }

            var totalCount = await query.CountAsync(cancellationToken).ConfigureAwait(false);

            var messages = await ApplyMessageOrdering(query)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            foreach (var message in messages)
            {
                SortMessageStatuses(message);
            }

            return new IntegrationMessagesPageResult<TMessage>
            {
                Items = messages,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
            };
        }

        public async Task<IReadOnlyDictionary<int, string>> GetStatusNameMapAsync(
            CancellationToken cancellationToken = default)
        {
            var statusNames = await DbContext.ExportMessageRabbitStatusNames
                .AsNoTracking()
                .Where(s => s.Id != null)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            return statusNames.ToDictionary(s => s.Id.Value, s => s.StatusName);
        }

        public async Task<IReadOnlyList<string>> GetOperationKeysAsync(
            CancellationToken cancellationToken = default)
        {
            return await DbContext.RabbitIntegrationOperationKeysConfigurations
                .AsNoTracking()
                .OrderBy(c => c.Key)
                .Select(c => c.Key)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<IntegrationDashboardStats> GetDashboardStatsAsync(
            CancellationToken cancellationToken = default)
        {
            var baseQuery = BuildBaseQuery();

            var total = await baseQuery
                .CountAsync(cancellationToken)
                .ConfigureAwait(false);

            var failed = await ApplyWithErrorFilter(baseQuery)
                .CountAsync(cancellationToken)
                .ConfigureAwait(false);

            return new IntegrationDashboardStats
            {
                Total = total,
                Failed = failed,
                Successful = total - failed,
            };
        }

        protected abstract IQueryable<TMessage> BuildBaseQuery();

        protected abstract IQueryable<TMessage> ApplyMessageOrdering(IQueryable<TMessage> query);

        protected abstract IQueryable<TMessage> ApplyIdFilter(IQueryable<TMessage> query, System.Guid id);

        protected abstract IQueryable<TMessage> ApplyOperationKeyFilter(IQueryable<TMessage> query, string operationKey);

        protected abstract IQueryable<TMessage> ApplyWithErrorFilter(IQueryable<TMessage> query);

        protected abstract IQueryable<TMessage> ApplyWithSendMessageFilter(IQueryable<TMessage> query);

        protected abstract IQueryable<TMessage> ApplyCreatedFromFilter(IQueryable<TMessage> query, System.DateTime createdFrom);

        protected abstract IQueryable<TMessage> ApplyCreatedToFilter(IQueryable<TMessage> query, System.DateTime createdTo);

        protected abstract void SortMessageStatuses(TMessage message);
    }
}
