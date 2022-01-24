namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.Keys;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
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
            var candidate = new PreferredSupplierChange
                                {
                                    ChangedBy = resource.ChangedById.HasValue
                                    ? new Employee { Id = (int)resource.ChangedById } : null,
                                    PartNumber = resource.PartNumber,
                                    OldSupplier = resource.OldSupplierId.HasValue
                                                      ? new Supplier { SupplierId = (int)resource.OldSupplierId }
                                                      : null,
                                    OldPrice = resource.OldPrice,
                                    BaseOldPrice = resource.BaseOldPrice,
                                    OldCurrency = !string.IsNullOrEmpty(resource.OldCurrencyCode) 
                                                      ? new Currency { Code = resource.OldCurrencyCode }
                                                      : null,
                                    NewSupplier = resource.NewSupplierId.HasValue 
                                                        ? new Supplier { SupplierId = (int)resource.NewSupplierId }
                                                        : null,
                                    ChangeReason = new PriceChangeReason { ReasonCode = resource.ChangeReasonCode },
                                    NewCurrency = !string.IsNullOrEmpty(resource.NewCurrency)
                                                      ? new Currency { Code = resource.NewCurrency } : null,
                                    NewPrice = resource.BaseNewPrice,
                                    BaseNewPrice = resource.BaseNewPrice
                                };
            return this.domainService.CreatePreferredSupplierChange(candidate, privileges);
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
