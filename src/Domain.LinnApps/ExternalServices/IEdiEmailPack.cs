namespace Linn.Purchasing.Domain.LinnApps.ExternalServices
{
    public interface IEdiEmailPack
    {
        void GetEdiOrders(int supplierId);

        string SendEdiOrder(int supplierId, string altEmail, string additionalEmail, string additionalText, bool test);
    }
}
