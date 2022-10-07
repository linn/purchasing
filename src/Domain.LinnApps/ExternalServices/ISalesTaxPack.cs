namespace Linn.Purchasing.Domain.LinnApps.ExternalServices
{
    public interface ISalesTaxPack
    {
        decimal GetVatRateSupplier(int supplierId);
    }
}
