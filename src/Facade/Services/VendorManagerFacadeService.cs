namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Resources;

    public class VendorManagerFacadeService : FacadeResourceService<VendorManager, string, VendorManagerResource,
        VendorManagerResource>
    {
        public VendorManagerFacadeService(
            IRepository<VendorManager, string> repository,
            ITransactionManager transactionManager,
            IBuilder<VendorManager> resourceBuilder)
            : base(repository, transactionManager, resourceBuilder)
        {
        }

        protected override VendorManager CreateFromResource(
            VendorManagerResource resource,
            IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override void DeleteOrObsoleteResource(VendorManager entity, IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override void SaveToLogTable(
            string actionType,
            int userNumber,
            VendorManager entity,
            VendorManagerResource resource,
            VendorManagerResource updateResource)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<VendorManager, bool>> SearchExpression(string searchTerm)
        {
            throw new NotImplementedException();
        }

        protected override void UpdateFromResource(
            VendorManager entity,
            VendorManagerResource updateResource,
            IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }
    }
}
