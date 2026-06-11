namespace VTBL.ExportMessage.Rabbit.UI.Models
{
    public static class IntegrationPageSize
    {
        public const int Default = 20;

        public static readonly int[] AllowedSizes = { 20, 50, 100 };

        public static int Normalize(int? pageSize)
        {
            if (!pageSize.HasValue)
            {
                return Default;
            }

            foreach (var allowed in AllowedSizes)
            {
                if (pageSize.Value == allowed)
                {
                    return allowed;
                }
            }

            return Default;
        }
    }
}
