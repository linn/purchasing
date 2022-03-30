namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.MaterialRequirements;
    using Linn.Purchasing.Resources.MaterialRequirements;
    using Linn.Purchasing.Resources.SearchResources;

    public class MrpRunLogFacadeService : FacadeFilterResourceService<MrpRunLog, int, MrpRunLogResource, MrpRunLogResource, MaterialRequirementsSearchResource>
    {
        public MrpRunLogFacadeService(
            IRepository<MrpRunLog, int> repository,
            ITransactionManager transactionManager,
            IBuilder<MrpRunLog> resourceBuilder)
            : base(repository, transactionManager, resourceBuilder)
        {
        }

        protected override MrpRunLog CreateFromResource(
            MrpRunLogResource resource,
            IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override void DeleteOrObsoleteResource(MrpRunLog entity, IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override void SaveToLogTable(
            string actionType,
            int userNumber,
            MrpRunLog entity,
            MrpRunLogResource resource,
            MrpRunLogResource updateResource)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<MrpRunLog, bool>> SearchExpression(string searchTerm)
        {
            return a => a.JobRef == searchTerm;
        }

        protected override void UpdateFromResource(
            MrpRunLog entity,
            MrpRunLogResource updateResource,
            IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<MrpRunLog, bool>> FilterExpression(MaterialRequirementsSearchResource searchResource)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<MrpRunLog, bool>> FindExpression(MaterialRequirementsSearchResource searchResource)
        {
            return a => a.JobRef == searchResource.JobRef;
        }
    }
}
