namespace Linn.Purchasing.Domain.LinnApps.ExternalServices
{
    public interface IPurchaseOrderReqsPack
    {
        bool StateChangeAllowed(string fromState, string toState);
    }
}
