namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System;
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Resources;

    public class PlannerResourceBuilder : IBuilder<Planner>
    {
        public PlannerResource Build(Planner entity, IEnumerable<string> claims)
        {
            return new PlannerResource
                       {
                           Id = entity.Id,
                           EmployeeName = entity.Employee.FullName
                       };
        }

        public string GetLocation(Planner v)
        {
            throw new NotImplementedException();
        }

        object IBuilder<Planner>.Build(Planner entity, IEnumerable<string> claims)
        {
            return this.Build(entity, claims);
        }
    }
}
