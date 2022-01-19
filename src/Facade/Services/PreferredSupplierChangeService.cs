namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Persistence.LinnApps.Keys;
    using Linn.Purchasing.Resources;

    public class PreferredSupplierChangeService 
        : FacadeResourceService<PreferredSupplierChange, PreferredSupplierChangeKey, PreferredSupplierChangeResource, PreferredSupplierChangeKey>
    {
        private readonly IPartSupplierService domainService;

        public PreferredSupplierChangeService(
            IRepository<PreferredSupplierChange, PreferredSupplierChangeKey> repository, 
            ITransactionManager transactionManager, 
            IBuilder<PreferredSupplierChange> resourceBuilder,
            IPartSupplierService domainService)
            : base(repository, transactionManager, resourceBuilder)
        {
            this.domainService = domainService;
        }

        protected override PreferredSupplierChange CreateFromResource(
            PreferredSupplierChangeResource resource, 
            IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override void UpdateFromResource(
            PreferredSupplierChange entity,
            PreferredSupplierChangeKey updateResource,
            IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<PreferredSupplierChange, bool>> SearchExpression(string searchTerm)
        {
            throw new NotImplementedException();
        }

        protected override void SaveToLogTable(
            string actionType,
            int userNumber,
            PreferredSupplierChange entity,
            PreferredSupplierChangeResource resource,
            PreferredSupplierChangeKey updateResource)
        {
            throw new NotImplementedException();
        }

        protected override void DeleteOrObsoleteResource(
            PreferredSupplierChange entity, 
            IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }
    }
}