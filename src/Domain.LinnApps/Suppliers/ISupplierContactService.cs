namespace Linn.Purchasing.Domain.LinnApps.Suppliers
{
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    public interface ISupplierContactService
    {
        SupplierContact GetMainContactForSupplier(int supplier);
    }
}
