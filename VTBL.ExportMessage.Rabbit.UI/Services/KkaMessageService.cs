using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VTBL.ExportMessage.Rabbit.Context;
using VTBL.ExportMessage.Rabbit.Context.Entities;

namespace VTBL.ExportMessage.Rabbit.UI.Services
{
    public class KkaMessageService : IKkaMessageService
    {
        private readonly MscrmExtDbContext _dbContext;

        public KkaMessageService(MscrmExtDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyList<ExportMessageRabbitKka>> GetMessagesGroupedByIdAsync(
            CancellationToken cancellationToken = default)
        {
            var messages = await _dbContext.ExportMessageRabbitKkas
                .AsNoTracking()
                .Include(k => k.StatusHistory)
                .Include(k => k.OperationConfiguration)
                .OrderByDescending(k => k.Created)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            foreach (var message in messages)
            {
                message.StatusHistory = message.StatusHistory
                    .OrderBy(s => s.Created)
                    .ToList();
            }

            return messages;
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
