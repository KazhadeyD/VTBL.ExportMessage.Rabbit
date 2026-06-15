using System;

namespace VTBL.RabbitIntegration.Monitor.UI.Models
{
    /// <summary>
    /// Строка дашборда по одной интеграционной системе.
    /// </summary>
    public class IntegrationDashboardEntry
    {
        /// <summary>
        /// Описание интеграционной системы.
        /// </summary>
        public IntegrationSystemDescriptor System { get; init; }

        /// <summary>
        /// Имя Razor Page для перехода в раздел системы.
        /// </summary>
        public string PageRoute { get; init; }

        /// <summary>
        /// Агрегированная статистика по системе.
        /// </summary>
        public IntegrationDashboardStats Stats { get; init; } = new IntegrationDashboardStats();

        /// <summary>
        /// Текст ошибки загрузки статистики, если загрузка не удалась.
        /// </summary>
        public string LoadError { get; init; }

        /// <summary>
        /// Признак успешной загрузки статистики.
        /// </summary>
        public bool IsLoaded => string.IsNullOrEmpty(LoadError);

        /// <summary>
        /// Признак того, что можно вычислить процент ошибок.
        /// </summary>
        public bool HasFailedPercent => Stats.Total > 0;

        /// <summary>
        /// Доля сообщений с ошибками в процентах.
        /// </summary>
        public int FailedPercent =>
            Stats.Total > 0
                ? (int)Math.Round(100.0 * Stats.Failed / Stats.Total)
                : 0;
    }
}
