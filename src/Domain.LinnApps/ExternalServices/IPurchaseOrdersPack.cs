namespace Linn.Purchasing.Domain.LinnApps.ExternalServices
{
    public interface IPurchaseOrdersPack
    {
        bool OrderIsCompleteSql(int orderNumber, int lineNumber);
    }
}
