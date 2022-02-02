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
        public SupplierFacadeService(IRepository<Supplier, int> repository, ITransactionManager transactionManager, IBuilder<Supplier> resourceBuilder)
            : base(repository, transactionManager, resourceBuilder)
        {
        }

        protected override Supplier CreateFromResource(
            SupplierResource resource,
            IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
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
            var updated = new Supplier
                              {
                                  SupplierId = entity.SupplierId,
                                  Name = updateResource.Name,
                                  Currency = updateResource.Currency,
                                  WebAddress = updateResource.WebAddress,
                                  VendorManager = updateResource.VendorManager,
                                  Planner = updateResource.Planner,
                                  InvoiceContactMethod = updateResource.InvoiceContactMethod,
                                  PhoneNumber = updateResource.PhoneNumber,
                                  OrderContactMethod = updateResource.OrderContactMethod,
                                  SuppliersReference = updateResource.SuppliersReference,
                                  LiveOnOracle = updateResource.LiveOnOracle,
                                  LedgerStream = updateResource.LedgerStream
                              };
        }

        protected override Expression<Func<Supplier, bool>> SearchExpression(string searchTerm)
        {
            return s => s.SupplierId.ToString().Contains(searchTerm) || s.Name.Contains(searchTerm.ToUpper());
        }
    }
}
