namespace VTBL.ExportMessage.Rabbit.UI.Models
{
    public interface IIntegrationCreatedFilterFields
    {
        string FilterCreatedFrom { get; }

        string FilterCreatedTo { get; }

        string FilterCreatedFromError { get; }

        string FilterCreatedToError { get; }

        string FilterCreatedRangeError { get; }
    }
}
