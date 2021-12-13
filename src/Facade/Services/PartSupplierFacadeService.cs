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
    using Linn.Purchasing.Resources.SearchResources;

    public class PartSupplierFacadeService 
        : FacadeFilterResourceService<PartSupplier, PartSupplierKey, PartSupplierResource, PartSupplierResource, PartSupplierSearchResource>,
          IApplicationStateService<PartSupplier, PartSupplierKey, PartSupplierResource, PartSupplierResource, PartSupplierSearchResource>
    {
        public PartSupplierFacadeService(IRepository<PartSupplier, PartSupplierKey> repository, ITransactionManager transactionManager, IBuilder<PartSupplier> resourceBuilder)
            : base(repository, transactionManager, resourceBuilder)
        {
        }

        public IResult<PartSupplierResource> GetApplicationState(IEnumerable<string> claims)
        {
            return new SuccessResult<PartSupplierResource>(this.BuildResource(null, claims));
        }

        protected override PartSupplier CreateFromResource(PartSupplierResource resource)
        {
            throw new NotImplementedException();
        }

        protected override void DeleteOrObsoleteResource(PartSupplier entity)
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

        protected override Expression<Func<PartSupplier, bool>> FilterExpression(PartSupplierSearchResource searchResource)
        {
            return x => (x.PartNumber.Contains(searchResource.PartNumberSearchTerm.ToUpper()) 
                        || string.IsNullOrEmpty(searchResource.PartNumberSearchTerm))
                        && 
                        (x.Supplier.Name.Contains(searchResource.SupplierNameSearchTerm.ToUpper())
                         || string.IsNullOrEmpty(searchResource.SupplierNameSearchTerm));
        }
    }
}
