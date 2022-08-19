namespace Linn.Purchasing.Persistence.LinnApps.Repositories
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using Microsoft.EntityFrameworkCore;

    public class SupplierRepository : IRepository<Supplier, int>
    {
        private readonly ServiceDbContext serviceDbContext;

        public SupplierRepository(ServiceDbContext serviceDbContext)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public Supplier FindById(int key)
        {
            return this.serviceDbContext.Suppliers
                    .Include(s => s.InvoiceGoesTo)
                    .Include(s => s.Currency)
                    .Include(s => s.PartCategory)
                    .Include(s => s.RefersToFc)
                    .Include(s => s.InvoiceFullAddress)
                    .Include(s => s.AccountController)
                    .Include(s => s.Planner)
                    .Include(s => s.VendorManager)
                    .Include(s => s.OrderAddress)
                    .Include(s => s.OpenedBy)
                    .Include(s => s.ClosedBy)
                    .Include(s => s.SupplierContacts)
                    .ThenInclude(c => c.Person)
                    .Include(s => s.Group)
                    .Include(s => s.OrderAddress).ThenInclude(a => a.FullAddress)
                    .Include(s => s.Currency)
                    .FirstOrDefault(x => x.SupplierId == key);
        }

        public IQueryable<Supplier> FindAll()
        {
            throw new NotImplementedException();
        }

        public void Add(Supplier entity)
        {
            this.serviceDbContext.Suppliers.Add(entity);
        }

        public void Remove(Supplier entity)
        {
            throw new NotImplementedException();
        }

        public Supplier FindBy(Expression<Func<Supplier, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Supplier> FilterBy(Expression<Func<Supplier, bool>> expression)
        {
            return this.serviceDbContext.Suppliers.Include(s => s.OrderAddress).ThenInclude(a => a.FullAddress)
                .Include(s => s.SupplierContacts).ThenInclude(sc => sc.Person)
                .Include(s => s.Currency)
                .AsNoTracking().Where(expression);
        }
    }
}
