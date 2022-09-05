namespace Linn.Purchasing.Persistence.LinnApps.Repositories
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Purchasing.Domain.LinnApps.Forecasting;

    using Microsoft.EntityFrameworkCore;

    public class ForecastWeekChangesRepository 
        : EntityFrameworkQueryRepository<ForecastWeekChange>
    {
        private readonly ServiceDbContext serviceDbContext;

        public ForecastWeekChangesRepository(ServiceDbContext serviceDbContext)
            : base(serviceDbContext.ForecastWeekChanges)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public override IQueryable<ForecastWeekChange> FilterBy(
            Expression<Func<ForecastWeekChange, bool>> expression)
        {
            return base.FilterBy(expression).Include(x => x.LinnWeek);
        }
    }
}
