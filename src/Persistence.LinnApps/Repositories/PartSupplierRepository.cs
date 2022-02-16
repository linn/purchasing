namespace Linn.Purchasing.Persistence.LinnApps.Repositories
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Purchasing.Domain.LinnApps.Keys;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;

    using Microsoft.EntityFrameworkCore;

    public class PartSupplierRepository : EntityFrameworkRepository<PartSupplier, PartSupplierKey>
    {
        private readonly DbSet<PartSupplier> partSuppliers;

        public PartSupplierRepository(ServiceDbContext serviceDbContext) 
            : base(serviceDbContext.PartSuppliers)
        {
            this.partSuppliers = serviceDbContext.PartSuppliers;
        }

        public override PartSupplier FindById(PartSupplierKey key)
        {
            return this.partSuppliers
                .Include(p => p.Part)
                .Include(p => p.Supplier)
                .ThenInclude(p => p.Planner)
                .Include(p => p.Supplier)
                .ThenInclude(p => p.VendorManager)
                .Include(p => p.PackagingGroup)
                .Include(p => p.Supplier)
                .ThenInclude(p => p.AccountController)
                .Include(p => p.CreatedBy)
                .Include(p => p.MadeInvalidBy)
                .Include(p => p.DeliveryFullAddress)
                .Include(p => p.Manufacturer)
                .Include(p => p.Tariff)
                .Include(p => p.OrderMethod)
                .Include(p => p.Currency)
                .SingleOrDefault(
                p => p.PartNumber == key.PartNumber && p.SupplierId == key.SupplierId);
        }

        public override IQueryable<PartSupplier> FilterBy(Expression<Func<PartSupplier, bool>> expression)
        {
            return this.partSuppliers
                .Include(p => p.Supplier)
                .Include(p => p.Part)
                .Include(p => p.Currency)
                .AsNoTracking().Where(expression);
        }
    }
}
