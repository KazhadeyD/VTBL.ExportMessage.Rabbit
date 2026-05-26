using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VTBL.ExportMessage.Rabbit.Context;
using VTBL.ExportMessage.Rabbit.UI.Models;

namespace VTBL.ExportMessage.Rabbit.UI.Services
{
    public class KkaMessageService : IKkaMessageService
    {
        private readonly MscrmExtDbContext _dbContext;

        public KkaMessageService(MscrmExtDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<KkaMessagesPageResult> GetMessagesPageAsync(
            int page,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            var query = _dbContext.ExportMessageRabbitKkas.AsNoTracking();

            var totalCount = await query.CountAsync(cancellationToken).ConfigureAwait(false);

            var messages = await query
                .Include(k => k.StatusHistory)
                .Include(k => k.OperationConfiguration)
                .OrderByDescending(k => k.Created)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            foreach (var message in messages)
            {
                message.StatusHistory = message.StatusHistory
                    .OrderBy(s => s.Created)
                    .ToList();
            }

            return new KkaMessagesPageResult
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
            var statusNames = await _dbContext.ExportMessageRabbitStatusNames
                .AsNoTracking()
                .Where(s => s.Id != null)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            return statusNames.ToDictionary(s => s.Id.Value, s => s.StatusName);
        }
    }
}
