namespace Linn.Purchasing.Persistence.LinnApps.Repositories
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;

    using Microsoft.EntityFrameworkCore;

    public class LinnDeliveryAddressRepository : IQueryRepository<LinnDeliveryAddress>
    {
        private readonly ServiceDbContext serviceDbContext;

        public LinnDeliveryAddressRepository(ServiceDbContext serviceDbContext)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public LinnDeliveryAddress FindBy(Expression<Func<LinnDeliveryAddress, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public IQueryable<LinnDeliveryAddress> FilterBy(Expression<Func<LinnDeliveryAddress, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public IQueryable<LinnDeliveryAddress> FindAll()
        {
            return this.serviceDbContext.LinnDeliveryAddresses
                .AsNoTracking().Include(x => x.Address);
        }
    }
}
