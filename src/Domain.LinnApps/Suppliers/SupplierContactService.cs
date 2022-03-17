namespace Linn.Purchasing.Domain.LinnApps.Suppliers
{
    using Linn.Common.Persistence;

    public class SupplierContactService : ISupplierContactService
    {
        private readonly IRepository<SupplierContact, int> repository;

        public SupplierContactService(IRepository<SupplierContact, int> repository)
        {
            this.repository = repository;
        }

        public SupplierContact GetMainContactForSupplier(int supplierId)
        {
            return this.repository.FindBy(s => s.SupplierId == supplierId && s.IsMainOrderContact == "Y");
        }
    }
}
