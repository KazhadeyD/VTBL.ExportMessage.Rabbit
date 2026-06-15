using System;
using System.Collections.Generic;

namespace VTBL.RabbitIntegration.Monitor.Context.Entities
{
    /// <summary>
    /// Минимальный контракт сообщения интеграции для универсальных запросов в сервисах UI.
    /// </summary>
    /// <typeparam name="TStatus">Тип статуса сообщения.</typeparam>
    public interface IExportMessageRabbitMessage<TStatus>
        where TStatus : IExportMessageRabbitStatus
    {
        /// <summary>
        /// Идентификатор записи сообщения.
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Ключ операции интеграции.
        /// </summary>
        string OperationKey { get; }

        /// <summary>
        /// Дата и время создания записи.
        /// </summary>
        DateTime Created { get; }

        /// <summary>
        /// История статусов сообщения.
        /// </summary>
        ICollection<TStatus> StatusHistory { get; }
    }
}
