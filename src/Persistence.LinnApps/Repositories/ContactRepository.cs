namespace Linn.Purchasing.Persistence.LinnApps.Repositories
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Purchasing.Domain.LinnApps;

    using Microsoft.EntityFrameworkCore;

    public class ContactRepository : EntityFrameworkRepository<Contact, int>
    {
        public ContactRepository(ServiceDbContext serviceDbContext)
            : base(serviceDbContext.Contacts)
        {
        }

        public override IQueryable<Contact> FilterBy(Expression<Func<Contact, bool>> expression)
        {
            return base.FilterBy(expression).AsNoTracking().Include(c => c.Person);
        }
    }
}
