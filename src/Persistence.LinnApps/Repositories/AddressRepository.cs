namespace Linn.Purchasing.Persistence.LinnApps.Repositories
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Purchasing.Domain.LinnApps;

    using Microsoft.EntityFrameworkCore;

    public class AddressRepository : EntityFrameworkRepository<Address, int>
    {
        private readonly ServiceDbContext serviceDbContext;

        public AddressRepository(ServiceDbContext serviceDbContext)
            : base(serviceDbContext.Addresses)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public override Address FindById(int key)
        {
            return this.serviceDbContext.Addresses.Include(a => a.Country)
                .SingleOrDefault(a => a.AddressId == key);
        }

        public override IQueryable<Address> FilterBy(Expression<Func<Address, bool>> expression)
        {
            return base.FilterBy(expression).AsNoTracking().Include(x => x.Country);
        }
    }
}
