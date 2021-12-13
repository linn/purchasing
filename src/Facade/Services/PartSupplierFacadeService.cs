namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Linq.Expressions;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Persistence.LinnApps.Keys;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.SearchResources;

    public class PartSupplierFacadeService 
        : FacadeFilterResourceService<PartSupplier, PartSupplierKey, PartSupplierResource, PartSupplierResource, PartSupplierSearchResource>
    {
        private readonly IPartSupplierService domainService;

        public PartSupplierFacadeService(
            IRepository<PartSupplier, PartSupplierKey> repository, 
            ITransactionManager transactionManager, 
            IBuilder<PartSupplier> resourceBuilder,
            IPartSupplierService domainService)
            : base(repository, transactionManager, resourceBuilder)
        {
            this.domainService = domainService;
        }

        protected override PartSupplier CreateFromResource(PartSupplierResource resource)
        {
            var candidate = new PartSupplier
                                {
                                    SupplierId = resource.SupplierId,
                                    PartNumber = resource.PartNumber,
                                    SupplierDesignation = resource.Designation
                                };

            return this.domainService.CreatePartSupplier(candidate, resource.Privileges);
        }

        protected override void DeleteOrObsoleteResource(PartSupplier entity)
        {
            throw new NotImplementedException();
        }

        protected override void UpdateFromResource(PartSupplier entity, PartSupplierResource updateResource)
        {
            var updated = new PartSupplier
                                  {
                                      SupplierId = updateResource.SupplierId,
                                      PartNumber = updateResource.PartNumber,
                                      SupplierDesignation = updateResource.Designation
                                  };

            this.domainService.UpdatePartSupplier(entity, updated, updateResource.Privileges);
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
