namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System;
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Resources;

    public class PlannerResourceBuilder : IBuilder<Planner>
    {
        private readonly IRepository<Employee, int> employeeRepository;

        public PlannerResourceBuilder(IRepository<Employee, int> employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }

        public PlannerResource Build(Planner entity, IEnumerable<string> claims)
        {
            return new PlannerResource
                       {
                           Id = entity.Id,
                           EmployeeName = this.employeeRepository.FindById(entity.Id)?.FullName
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
