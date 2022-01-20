namespace Linn.Purchasing.Domain.LinnApps.PartSuppliers
{
    public interface IUpdatePartPreferredSupplierService
    {
        void UpdatePart(int partId, int newPreferredSupplierId);
    }
}
