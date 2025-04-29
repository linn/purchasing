namespace Linn.Purchasing.Persistence.LinnApps.Repositories
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Reports.Models;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

    public class SupplierSpendRepository : IQueryRepository<SupplierSpend>
    {
        private readonly ServiceDbContext serviceDbContext;

        public SupplierSpendRepository(ServiceDbContext serviceDbContext)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public IQueryable<SupplierSpend> FilterBy(Expression<Func<SupplierSpend, bool>> expression)
        {
           return this.serviceDbContext.SupplierSpends.AsNoTracking().Where(expression);
        }

        IQueryable<SupplierSpend> IQueryRepository<SupplierSpend>.FindAll()
        {
            return this.serviceDbContext.SupplierSpends.AsNoTracking();
        }

        SupplierSpend IQueryRepository<SupplierSpend>.FindBy(Expression<Func<SupplierSpend, bool>> expression)
        {
            throw new NotImplementedException();
        }
    }
}
