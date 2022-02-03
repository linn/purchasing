namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Resources;

    public class SupplierFacadeService : FacadeResourceService<Supplier, int, SupplierResource, SupplierResource>
    {
        private ISupplierService domainService;

        public SupplierFacadeService(
            IRepository<Supplier, int> repository, 
            ITransactionManager transactionManager, 
            IBuilder<Supplier> resourceBuilder,
            ISupplierService domainService)
            : base(repository, transactionManager, resourceBuilder)
        {
            this.domainService = domainService;
        }

        protected override Supplier CreateFromResource(
            SupplierResource resource,
            IEnumerable<string> privileges = null)
        {
            var candidate = this.BuildEntityFromResourceHelper(resource);

            return this.domainService.CreateSupplier(candidate, privileges);
        }

        protected override void SaveToLogTable(
            string actionType,
            int userNumber,
            Supplier entity,
            SupplierResource resource,
            SupplierResource updateResource)
        {
            throw new NotImplementedException();
        }

        protected override void DeleteOrObsoleteResource(
            Supplier entity,
            IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override void UpdateFromResource(
            Supplier entity, 
            SupplierResource updateResource,
            IEnumerable<string> privileges = null)
        {
            var updated = this.BuildEntityFromResourceHelper(updateResource);

            updated.SupplierId = entity.SupplierId;

            this.domainService.UpdateSupplier(entity, updated, privileges);
        }

        protected override Expression<Func<Supplier, bool>> SearchExpression(string searchTerm)
        {
            return s => s.SupplierId.ToString().Contains(searchTerm) || s.Name.Contains(searchTerm.ToUpper());
        }

        private Supplier BuildEntityFromResourceHelper(SupplierResource resource)
        {
            return new Supplier
                       {
                           Name = resource.Name,
                           Currency = resource.Currency,
                           WebAddress = resource.WebAddress,
                           VendorManager = resource.VendorManager,
                           Planner = resource.Planner,
                           InvoiceContactMethod = resource.InvoiceContactMethod,
                           PhoneNumber = resource.PhoneNumber,
                           OrderContactMethod = resource.OrderContactMethod,
                           SuppliersReference = resource.SuppliersReference,
                           LiveOnOracle = resource.LiveOnOracle,
                           LedgerStream = resource.LedgerStream
                       };
        }
    }
}
