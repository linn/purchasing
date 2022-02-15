namespace Linn.Purchasing.Persistence.LinnApps.Repositories
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.Reports.Models;

    using Microsoft.EntityFrameworkCore;

    public class SupplierSpendRepository : IQueryRepository<SupplierSpend>
    {
        private readonly ServiceDbContext serviceDbContext;

        public SupplierSpendRepository(ServiceDbContext serviceDbContext)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public IQueryable<SupplierSpend> FilterBy(Expression<Func<SupplierSpend, bool>> expression)
        {
            return this.serviceDbContext.SupplierSpends.Include(s => s.Supplier).Where(expression);
        }

        IQueryable<SupplierSpend> IQueryRepository<SupplierSpend>.FindAll()
        {
            throw new NotImplementedException();
        }

        SupplierSpend IQueryRepository<SupplierSpend>.FindBy(Expression<Func<SupplierSpend, bool>> expression)
        {
            throw new NotImplementedException();
        }
    }
}
