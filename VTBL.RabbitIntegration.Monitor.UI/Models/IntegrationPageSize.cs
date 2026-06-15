namespace VTBL.RabbitIntegration.Monitor.UI.Models
{
    /// <summary>
    /// Политика допустимых размеров страницы результатов.
    /// </summary>
    public static class IntegrationPageSize
    {
        /// <summary>
        /// Размер страницы по умолчанию.
        /// </summary>
        public const int Default = 20;

        /// <summary>
        /// Допустимые значения размера страницы.
        /// </summary>
        public static readonly int[] AllowedSizes = { 20, 50, 100 };

        /// <summary>
        /// Нормализует пользовательское значение размера страницы до разрешенного.
        /// </summary>
        /// <param name="pageSize">Значение из query string.</param>
        /// <returns>Разрешенный размер страницы или <see cref="Default"/>.</returns>
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
