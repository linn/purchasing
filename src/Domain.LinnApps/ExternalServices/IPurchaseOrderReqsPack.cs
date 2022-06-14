namespace Linn.Purchasing.Domain.LinnApps.ExternalServices
{
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrderReqs;

    public interface IPurchaseOrderReqsPack
    {
        AllowedToAuthoriseReqResult AllowedToAuthorise(string stage, int userNumber, decimal value, string dept, string state);
    }
}
