namespace Linn.Purchasing.Persistence.LinnApps.Repositories
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Persistence.LinnApps.Keys;

    using Microsoft.EntityFrameworkCore;

    public class PartSupplierRepository : IRepository<PartSupplier, PartSupplierKey>
    {
        private readonly ServiceDbContext serviceDbContext;

        public PartSupplierRepository(ServiceDbContext serviceDbContext)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public PartSupplier FindById(PartSupplierKey key)
        {
            return this.serviceDbContext.PartSuppliers
                .Include(p => p.Part)
                .SingleOrDefault(
                p => p.PartNumber == key.PartNumber && p.SupplierId == key.SupplierId);
        }

        public IQueryable<PartSupplier> FindAll()
        {
            throw new NotImplementedException();
        }

        public void Add(PartSupplier entity)
        {
            throw new NotImplementedException();
        }

        public void Remove(PartSupplier entity)
        {
            throw new NotImplementedException();
        }

        public PartSupplier FindBy(Expression<Func<PartSupplier, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public IQueryable<PartSupplier> FilterBy(Expression<Func<PartSupplier, bool>> expression)
        {
            throw new NotImplementedException();
        }
    }
}
