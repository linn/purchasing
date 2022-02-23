namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Resources;

    public class SupplierGroupFacadeService : FacadeResourceService<SupplierGroup, int, SupplierGroupResource, SupplierGroupResource>
    {
        public SupplierGroupFacadeService(
            IRepository<SupplierGroup, int> repository,
            ITransactionManager transactionManager,
            IBuilder<SupplierGroup> resourceBuilder)
            : base(repository, transactionManager, resourceBuilder)
        {
        }

        protected override SupplierGroup CreateFromResource(
            SupplierGroupResource resource,
            IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override void DeleteOrObsoleteResource(SupplierGroup entity, IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override void SaveToLogTable(
            string actionType,
            int userNumber,
            SupplierGroup entity,
            SupplierGroupResource resource,
            SupplierGroupResource updateResource)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<SupplierGroup, bool>> SearchExpression(string searchTerm)
        {
            throw new NotImplementedException();
        }

        protected override void UpdateFromResource(
            SupplierGroup entity,
            SupplierGroupResource updateResource,
            IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }
    }
}
