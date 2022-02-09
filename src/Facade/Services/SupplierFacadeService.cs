namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Resources;

    public class SupplierFacadeService : FacadeResourceService<Supplier, int, SupplierResource, SupplierResource>
    {
        private readonly ISupplierService domainService;

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
                           Currency = new Currency
                                          {
                                              Code = resource.CurrencyCode
                                          },
                           WebAddress = resource.WebAddress,
                           VendorManager = resource.VendorManager,
                           Planner = resource.Planner,
                           InvoiceContactMethod = resource.InvoiceContactMethod,
                           PhoneNumber = resource.PhoneNumber,
                           OrderContactMethod = resource.OrderContactMethod,
                           SuppliersReference = resource.SuppliersReference,
                           LiveOnOracle = resource.LiveOnOracle,
                           ExpenseAccount = resource.ExpenseAccount,
                           PaymentDays = resource.PaymentDays,
                           PaymentMethod = resource.PaymentMethod,
                           PaysInFc = resource.PaysInFc,
                           AccountingCompany = resource.AccountingCompany,
                           ApprovedCarrier = resource.ApprovedCarrier,
                           VatNumber = resource.VatNumber,
                           InvoiceGoesTo = resource.InvoiceGoesToId.HasValue 
                                               ? new Supplier { SupplierId = (int)resource.InvoiceGoesToId } : null,
                           PartCategory = !string.IsNullOrEmpty(resource.PartCategory)
                                          ? new PartCategory { Category = resource.PartCategory } : null,
                           OrderHold = resource.OrderHold,
                           NotesForBuyer = resource.NotesForBuyer,
                           DeliveryDay = resource.DeliveryDay,
                           RefersToFc = resource.RefersToFcId.HasValue
                            ? new Supplier { SupplierId = (int)resource.RefersToFcId } : null,
                           PmDeliveryDaysGrace = resource.PmDeliveryDaysGrace
                       };
    }
    }
}
