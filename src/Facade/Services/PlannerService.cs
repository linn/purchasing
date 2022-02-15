namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Resources;

    public class PlannerService : FacadeResourceService<Planner, int, PlannerResource, PlannerResource>
    {
        public PlannerService(IRepository<Planner, int> repository, ITransactionManager transactionManager, IBuilder<Planner> resourceBuilder)
            : base(repository, transactionManager, resourceBuilder)
        {
        }

        protected override Planner CreateFromResource(PlannerResource resource, IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override void UpdateFromResource(Planner entity, PlannerResource updateResource, IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<Planner, bool>> SearchExpression(string searchTerm)
        {
            throw new NotImplementedException();
        }

        protected override void SaveToLogTable(
            string actionType,
            int userNumber,
            Planner entity,
            PlannerResource resource,
            PlannerResource updateResource)
        {
            throw new NotImplementedException();
        }

        protected override void DeleteOrObsoleteResource(Planner entity, IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }
    }
}
