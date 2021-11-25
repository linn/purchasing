namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Linq.Expressions;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Persistence.LinnApps.Keys;
    using Linn.Purchasing.Resources;

    public class PartSupplierFacadeService : FacadeResourceService<PartSupplier, PartSupplierKey, PartSupplierResource, PartSupplierResource>
    {
        public PartSupplierFacadeService(IRepository<PartSupplier, PartSupplierKey> repository, ITransactionManager transactionManager, IBuilder<PartSupplier> resourceBuilder)
            : base(repository, transactionManager, resourceBuilder)
        {
        }

        protected override PartSupplier CreateFromResource(PartSupplierResource resource)
        {
            throw new NotImplementedException();
        }

        protected override void UpdateFromResource(PartSupplier entity, PartSupplierResource updateResource)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<PartSupplier, bool>> SearchExpression(string searchTerm)
        {
            throw new NotImplementedException();
        }
    }
}