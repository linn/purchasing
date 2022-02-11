namespace Linn.Purchasing.Persistence.LinnApps.Repositories
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Reports.Models;

    public class UnacknowledgedOrdersRepository : IQueryRepository<UnacknowledgedOrders>
    {
        private readonly ServiceDbContext serviceDbContext;

        public UnacknowledgedOrdersRepository(ServiceDbContext serviceDbContext)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public UnacknowledgedOrders FindBy(Expression<Func<UnacknowledgedOrders, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public IQueryable<UnacknowledgedOrders> FilterBy(Expression<Func<UnacknowledgedOrders, bool>> expression)
        {
            return this.serviceDbContext.UnacknowledgedOrders.Where(expression);
        }

        public IQueryable<UnacknowledgedOrders> FindAll()
        {
            return this.serviceDbContext.UnacknowledgedOrders; 
        }
    }
}
