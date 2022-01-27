namespace Linn.Purchasing.Persistence.LinnApps.Repositories
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps;

    using Microsoft.EntityFrameworkCore;

    public class VendorManagerRepository : IRepository<VendorManager, string>
    {
        private readonly ServiceDbContext serviceDbContext;

        public VendorManagerRepository(ServiceDbContext serviceDbContext)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public void Add(VendorManager entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<VendorManager> FilterBy(Expression<Func<VendorManager, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public IQueryable<VendorManager> FindAll()
        {
            return this.serviceDbContext.VendorManagers.AsNoTracking().Include(x => x.Employee);
        }

        public VendorManager FindBy(Expression<Func<VendorManager, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public VendorManager FindById(string key)
        {
            throw new NotImplementedException();
        }

        public void Remove(VendorManager entity)
        {
            throw new NotImplementedException();
        }
    }
}
