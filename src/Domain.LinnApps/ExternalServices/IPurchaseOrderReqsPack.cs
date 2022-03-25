namespace Linn.Purchasing.Domain.LinnApps.ExternalServices
{
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;

    public interface IPurchaseOrderReqsPack
    {
        bool StateChangeAllowed(string fromState, string toState);

        AllowedToAuthoriseReqResult AllowedToAuthorise(string stage, int userNumber, decimal value, string dept, string state);
    }
}
