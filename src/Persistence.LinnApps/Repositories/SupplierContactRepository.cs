namespace Linn.Purchasing.Persistence.LinnApps.Repositories
{
    using System.Linq;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using Microsoft.EntityFrameworkCore;

    public class SupplierContactRepository : EntityFrameworkRepository<SupplierContact, int>
    {
        private readonly ServiceDbContext serviceDbContext;

        public SupplierContactRepository(ServiceDbContext serviceDbContext)
            : base(serviceDbContext.SupplierContacts)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public override IQueryable<SupplierContact> FindAll()
        {
            return this.serviceDbContext.SupplierContacts.AsNoTracking();
        }

        public override SupplierContact FindById(int key)
        {
            return this.serviceDbContext.SupplierContacts
                .Include(v => v.Person).SingleOrDefault(v => v.ContactId == key);
        }
    }
}
