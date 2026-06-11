using System;
using System.Collections.Generic;

namespace VTBL.ExportMessage.Rabbit.Context.Entities
{
    public interface IExportMessageRabbitMessage<TStatus>
        where TStatus : IExportMessageRabbitStatus
    {
        Guid Id { get; }

        string OperationKey { get; }

        DateTime Created { get; }

        ICollection<TStatus> StatusHistory { get; }
    }
}
