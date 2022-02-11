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
                .Include(s => s.OrderFullAddress)
                .First(x => x.SupplierId == key);
        }

        public IQueryable<Supplier> FindAll()
        {
            throw new NotImplementedException();
        }

        public void Add(Supplier entity)
        {
            throw new NotImplementedException();
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
            return this.serviceDbContext.Suppliers
                .AsNoTracking().Where(expression);
        }
    }
}
