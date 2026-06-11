using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace VTBL.ExportMessage.Rabbit.UI.Models
{
    public static class IntegrationFilterBuilder
    {
        public static bool TryBuild<TFilter>(
            IntegrationFilterBinding binding,
            IReadOnlyList<string> operationKeys,
            out TFilter filter,
            out IntegrationFilterValidationErrors errors,
            out bool hasActiveFilter)
            where TFilter : IntegrationMessageFilter, new()
        {
            filter = null;
            errors = new IntegrationFilterValidationErrors();
            hasActiveFilter = false;

            var hasId = !string.IsNullOrWhiteSpace(binding.Id);
            var hasOperationKey = !string.IsNullOrWhiteSpace(binding.OperationKey);
            var hasCreatedFrom = !string.IsNullOrWhiteSpace(binding.CreatedFrom);
            var hasCreatedTo = !string.IsNullOrWhiteSpace(binding.CreatedTo);

            if (!hasId
                && !hasOperationKey
                && !binding.HasError
                && !binding.HasSendMessage
                && !hasCreatedFrom
                && !hasCreatedTo)
            {
                return true;
            }

            hasActiveFilter = true;
            filter = new TFilter
            {
                WithError = binding.HasError,
                WithSendMessage = binding.HasSendMessage,
            };

            if (hasId)
            {
                if (Guid.TryParse(binding.Id.Trim(), out var parsedId))
                {
                    filter.Id = parsedId;
                }
                else
                {
                    errors.Id = "Некорректный формат Id. Укажите GUID, например: CA42A29F-4D8B-4428-9D43-20F9597C615F";
                    return false;
                }
            }

            if (hasOperationKey)
            {
                var key = binding.OperationKey.Trim();
                if (operationKeys.Contains(key, StringComparer.Ordinal))
                {
                    filter.OperationKey = key;
                }
                else
                {
                    errors.OperationKey = "Выберите OperationKey из списка.";
                    return false;
                }
            }

            if (hasCreatedFrom)
            {
                if (TryParseFilterDateTime(binding.CreatedFrom, out var createdFrom))
                {
                    filter.CreatedFrom = createdFrom;
                }
                else
                {
                    errors.CreatedFrom = "Некорректная дата «от». Формат: ГГГГ-ММ-ДДTЧЧ:ММ";
                    return false;
                }
            }

            if (hasCreatedTo)
            {
                if (TryParseFilterDateTime(binding.CreatedTo, out var createdTo))
                {
                    filter.CreatedTo = createdTo;
                }
                else
                {
                    errors.CreatedTo = "Некорректная дата «до». Формат: ГГГГ-ММ-ДДTЧЧ:ММ";
                    return false;
                }
            }

            if (filter.HasCreatedFrom && filter.HasCreatedTo && filter.CreatedFrom > filter.CreatedTo)
            {
                errors.CreatedRange = "Дата «от» не может быть позже даты «до».";
                return false;
            }

            return true;
        }

        public static string FormatForDateTimeLocal(DateTime value)
        {
            return value.ToString("yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture);
        }

        private static bool TryParseFilterDateTime(string value, out DateTime result)
        {
            var trimmed = value.Trim();

            if (DateTime.TryParseExact(
                    trimmed,
                    "yyyy-MM-ddTHH:mm",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.AssumeLocal,
                    out result))
            {
                return true;
            }

            return DateTime.TryParse(
                trimmed,
                CultureInfo.InvariantCulture,
                DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeLocal,
                out result);
        }
    }
}
