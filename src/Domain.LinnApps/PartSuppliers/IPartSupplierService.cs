namespace Linn.Purchasing.Domain.LinnApps.PartSuppliers
{
    using System.Collections.Generic;

    public interface IPartSupplierService
    {
        public void UpdatePartSupplier(PartSupplier current, PartSupplier updated, IEnumerable<string> privileges);

        public PartSupplier CreatePartSupplier(PartSupplier candidate, IEnumerable<string> privileges);

        public PreferredSupplierChange CreatePreferredSupplierChange(PreferredSupplierChange candidate, IEnumerable<string> privileges);

        public ProcessResult BulkUpdateLeadTimes(
            int supplierId,
            IEnumerable<LeadTimeUpdateModel> changes, 
            IEnumerable<string> privileges,
            int? organisationId = null);
    }
}
