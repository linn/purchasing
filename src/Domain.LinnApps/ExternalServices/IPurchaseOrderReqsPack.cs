namespace Linn.Purchasing.Domain.LinnApps.ExternalServices
{
    public interface IPurchaseOrderReqsPack
    {
        bool StateChangeAllowed(string fromState, string toState);

        string AllowedToAuthorise(string stage, int userNumber, decimal value, string dept, string state);
    }
}
