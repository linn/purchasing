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
        public AddressRepository(ServiceDbContext serviceDbContext)
            : base(serviceDbContext.Addresses)
        {
        }

        public override Address FindById(int key)
        {
            return this.FilterBy(x => x.AddressId == key).FirstOrDefault();
        }

        public override IQueryable<Address> FilterBy(Expression<Func<Address, bool>> expression)
        {
            return base.FilterBy(expression).Include(x => x.Country);
        }
    }
}
