namespace Linn.Purchasing.Persistence.LinnApps.Repositories
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Reports.Models;

    public class SupplierGroupsWithUnacknowledgedOrdersRepository : IQueryRepository<SupplierGroupsWithUnacknowledgedOrders>
    {
        private readonly ServiceDbContext serviceDbContext;

        public SupplierGroupsWithUnacknowledgedOrdersRepository(ServiceDbContext serviceDbContext)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public SupplierGroupsWithUnacknowledgedOrders FindBy(Expression<Func<SupplierGroupsWithUnacknowledgedOrders, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public IQueryable<SupplierGroupsWithUnacknowledgedOrders> FilterBy(Expression<Func<SupplierGroupsWithUnacknowledgedOrders, bool>> expression)
        {
            return this.serviceDbContext.SupplierGroupsWithUnacknowledgedOrders.Where(expression);
        }

        public IQueryable<SupplierGroupsWithUnacknowledgedOrders> FindAll()
        {
            return this.serviceDbContext.SupplierGroupsWithUnacknowledgedOrders; 
        }
    }
}
