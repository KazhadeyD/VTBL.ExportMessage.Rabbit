namespace VTBL.ExportMessage.Rabbit.Context.Entities
{
    public interface IExportMessageRabbitStatus
    {
        string ErrorMessage { get; }

        string SendMessage { get; }
    }
}
