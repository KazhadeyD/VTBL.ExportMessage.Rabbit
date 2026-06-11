using System;

namespace VTBL.ExportMessage.Rabbit.UI.Models
{
    public class IntegrationDashboardEntry
    {
        public IntegrationSystemDescriptor System { get; init; }

        public string PageRoute { get; init; }

        public IntegrationDashboardStats Stats { get; init; } = new IntegrationDashboardStats();

        public string LoadError { get; init; }

        public bool IsLoaded => string.IsNullOrEmpty(LoadError);

        public bool HasFailedPercent => Stats.Total > 0;

        public int FailedPercent =>
            Stats.Total > 0
                ? (int)Math.Round(100.0 * Stats.Failed / Stats.Total)
                : 0;
    }
}
