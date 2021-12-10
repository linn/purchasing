namespace Linn.Purchasing.Domain.LinnApps.PartSuppliers
{
    using System.Collections.Generic;

    public interface IPartSupplierService
    {
        public void UpdatePartSupplier(PartSupplier current, PartSupplier updated, IEnumerable<string> privileges);

        public PartSupplier CreatePartSupplier(PartSupplier candidate, IEnumerable<string> privileges);
    }
}
