namespace Linn.Purchasing.Domain.LinnApps.ExternalServices
{
    public interface IPurchaseOrdersPack
    {
        bool OrderCanBeAuthorisedBy(
            int? orderNumber,
            int? lineNumber,
            int userNumber,
            decimal totalValueBase,
            string part,
            string documentType);

        bool OrderIsCompleteSql(int orderNumber, int lineNumber);
    }
}
