namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Resources;

    public class ChangeRequestFacadeService : FacadeResourceService<ChangeRequest, int, ChangeRequestResource, ChangeRequestResource>
    {
        public ChangeRequestFacadeService(IRepository<ChangeRequest, int> repository, ITransactionManager transactionManager, IBuilder<ChangeRequest> resourceBuilder)
            : base(repository, transactionManager, resourceBuilder)
        {
        }

        protected override ChangeRequest CreateFromResource(ChangeRequestResource resource, IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override void UpdateFromResource(ChangeRequest entity, ChangeRequestResource updateResource, IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<ChangeRequest, bool>> SearchExpression(string searchTerm)
        {
            throw new NotImplementedException();
        }

        protected override void SaveToLogTable(
            string actionType,
            int userNumber,
            ChangeRequest entity,
            ChangeRequestResource resource,
            ChangeRequestResource updateResource)
        {
            throw new NotImplementedException();
        }

        protected override void DeleteOrObsoleteResource(ChangeRequest entity, IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }
    }
}
