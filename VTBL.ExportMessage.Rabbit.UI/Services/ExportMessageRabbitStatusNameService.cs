using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VTBL.ExportMessage.Rabbit.Context;
using VTBL.ExportMessage.Rabbit.Context.Entities;

namespace VTBL.ExportMessage.Rabbit.UI.Services
{
    /// <summary>
    /// Реализация сервиса чтения справочника статусов из базы <c>MSCRM_EXT</c>.
    /// </summary>
    public class ExportMessageRabbitStatusNameService : IExportMessageRabbitStatusNameService
    {
        private readonly MscrmExtDbContext _dbContext;

        /// <summary>
        /// Инициализирует сервис справочника статусов.
        /// </summary>
        /// <param name="dbContext">Контекст базы данных.</param>
        public ExportMessageRabbitStatusNameService(MscrmExtDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Возвращает статусы, отсортированные по идентификатору.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Список записей справочника статусов.</returns>
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
