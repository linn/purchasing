﻿namespace Linn.Purchasing.Persistence.LinnApps.Repositories
{
    using System.Linq;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using Microsoft.EntityFrameworkCore;

    public class PlannerRepository : EntityFrameworkRepository<Planner, int>
    {
        private readonly ServiceDbContext serviceDbContext;

        public PlannerRepository(ServiceDbContext serviceDbContext)
            : base(serviceDbContext.Planners)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public override IQueryable<Planner> FindAll()
        {
            return this.serviceDbContext.Planners.AsNoTracking();
        }

        public override Planner FindById(int key)
        {
            return this.serviceDbContext.Planners.SingleOrDefault(v => v.Id == key);
        }
    }
}
