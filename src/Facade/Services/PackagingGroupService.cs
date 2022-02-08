namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Resources;

    public class PackagingGroupService
        : FacadeResourceService<PackagingGroup, int, PackagingGroupResource, PackagingGroupResource>
    {
        public PackagingGroupService(
            IRepository<PackagingGroup, int> repository, 
            ITransactionManager transactionManager, 
            IBuilder<PackagingGroup> resourceBuilder)
            : base(repository, transactionManager, resourceBuilder)
        {
        }

        protected override PackagingGroup CreateFromResource(
            PackagingGroupResource resource, 
            IEnumerable<string> privileges = null)        
        {
            throw new NotImplementedException();
        }

        protected override void SaveToLogTable(
            string actionType,
            int userNumber,
            PackagingGroup entity,
            PackagingGroupResource resource,
            PackagingGroupResource updateResource)
        {
            throw new NotImplementedException();
        }

        protected override void DeleteOrObsoleteResource(
            PackagingGroup entity, 
            IEnumerable<string> privileges = null)        
        {
            throw new NotImplementedException();
        }

        protected override void UpdateFromResource(
            PackagingGroup entity, 
            PackagingGroupResource updateResource, 
            IEnumerable<string> privileges = null)        
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<PackagingGroup, bool>> SearchExpression(string searchTerm)
        {
            throw new NotImplementedException();
        }
    }
}
