using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VTBL.ExportMessage.Rabbit.Context;
using VTBL.ExportMessage.Rabbit.Context.Entities;

namespace VTBL.ExportMessage.Rabbit.UI.Services
{
    public class ExportMessageRabbitStatusNameService : IExportMessageRabbitStatusNameService
    {
        private readonly MscrmExtDbContext _dbContext;

        public ExportMessageRabbitStatusNameService(MscrmExtDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyList<ExportMessageRabbitStatusName>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.ExportMessageRabbitStatusNames
                .AsNoTracking()
                .OrderBy(x => x.Id)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
