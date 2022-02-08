namespace Linn.Purchasing.Domain.LinnApps.Suppliers
{
    using System.Collections.Generic;

    public interface ISupplierService
    {
        void UpdateSupplier(Supplier current, Supplier updated, IEnumerable<string> privileges);

        Supplier CreateSupplier(Supplier candidate, IEnumerable<string> privileges);

        Supplier ChangeSupplierHoldStatus(
            SupplierOrderHoldHistoryEntry data, 
            IEnumerable<string> privileges);
    }
}
