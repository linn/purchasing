namespace Linn.Purchasing.Persistence.LinnApps.Repositories
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Reports.Models;

    public class SuppliersWithUnacknowledgedOrdersRepository : IQueryRepository<SuppliersWithUnacknowledgedOrders>
    {
        private readonly ServiceDbContext serviceDbContext;

        public SuppliersWithUnacknowledgedOrdersRepository(ServiceDbContext serviceDbContext)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public SuppliersWithUnacknowledgedOrders FindBy(Expression<Func<SuppliersWithUnacknowledgedOrders, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public IQueryable<SuppliersWithUnacknowledgedOrders> FilterBy(Expression<Func<SuppliersWithUnacknowledgedOrders, bool>> expression)
        {
            return this.serviceDbContext.SuppliersWithUnacknowledgedOrders.Where(expression);
        }

        public IQueryable<SuppliersWithUnacknowledgedOrders> FindAll()
        {
            return this.serviceDbContext.SuppliersWithUnacknowledgedOrders; 
        }
    }
}
