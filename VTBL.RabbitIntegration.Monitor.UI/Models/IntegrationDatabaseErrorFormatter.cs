using System;

namespace VTBL.RabbitIntegration.Monitor.UI.Models
{
    /// <summary>
    /// Преобразует технические ошибки доступа к БД в пользовательские сообщения.
    /// </summary>
    public static class IntegrationDatabaseErrorFormatter
    {
        /// <summary>
        /// Формирует текст ошибки для интерфейса в контексте конкретной интеграционной системы.
        /// </summary>
        /// <param name="exception">Исходное исключение.</param>
        /// <param name="system">Описание интеграционной системы.</param>
        /// <returns>Пользовательское сообщение об ошибке.</returns>
        public static string ToUserMessage(Exception exception, IntegrationSystemDescriptor system)
        {
            if (exception == null)
            {
                return "Неизвестная ошибка при обращении к базе данных.";
            }

            if (IsMissingTableError(exception))
            {
                var tables = string.Join(", ", system.MessageTables);
                return $"Таблицы системы {system.DisplayName} не найдены в базе MSCRM_EXT ({tables}). "
                    + "Проверьте наличие таблиц в базе данных.";
            }

            if (IsDatabaseUnavailableError(exception))
            {
                return "Не удалось подключиться к базе MSCRM_EXT. "
                    + "Проверьте, что SQL Server запущен и строка подключения в appsettings корректна.";
            }

            return $"Не удалось загрузить данные системы {system.DisplayName}. "
                + "Проверьте доступность базы данных и наличие необходимых таблиц.";
        }

        private static bool IsMissingTableError(Exception exception)
        {
            for (var current = exception; current != null; current = current.InnerException)
            {
                if (TryGetSqlErrorNumber(current, out var number) && number == 208)
                {
                    return true;
                }

                if (current.Message.IndexOf("Invalid object name", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsDatabaseUnavailableError(Exception exception)
        {
            for (var current = exception; current != null; current = current.InnerException)
            {
                if (TryGetSqlErrorNumber(current, out var number)
                    && (number == 4060 || number == 18456 || number == 53 || number == -1))
                {
                    return true;
                }

                if (current.Message.IndexOf("network-related", StringComparison.OrdinalIgnoreCase) >= 0
                    || current.Message.IndexOf("Cannot open database", StringComparison.OrdinalIgnoreCase) >= 0
                    || current.Message.IndexOf("Login failed", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool TryGetSqlErrorNumber(Exception exception, out int number)
        {
            number = 0;

            if (exception == null || exception.GetType().Name != "SqlException")
            {
                return false;
            }

            var numberProperty = exception.GetType().GetProperty("Number");
            if (numberProperty?.GetValue(exception) is int sqlNumber)
            {
                number = sqlNumber;
                return true;
            }

            return false;
        }
    }
}
