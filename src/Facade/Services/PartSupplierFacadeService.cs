namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Linq.Expressions;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
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

        protected override void SaveToLogTable(
            string actionType,
            int userNumber,
            PartSupplier entity,
            PartSupplierResource resource,
            PartSupplierResource updateResource)
        {
            throw new NotImplementedException();
        }

        protected override void DeleteOrObsoleteResource(PartSupplier entity)
        {
            throw new NotImplementedException();
        }

        protected override void UpdateFromResource(PartSupplier entity, PartSupplierResource updateResource)
        {
            var updated = new PartSupplier
            {
                SupplierId = entity.SupplierId,
                PartNumber = entity.PartNumber,
                SupplierDesignation = updateResource.Designation,
                OrderMethod = new OrderMethod { Name = updateResource.OrderMethodName },
                Currency = new Currency { Code = updateResource.CurrencyCode },
                CurrencyUnitPrice = updateResource.CurrencyUnitPrice,
                OurCurrencyPriceToShowOnOrder = updateResource.OurCurrencyPriceToShowOnOrder,
                BaseOurUnitPrice = updateResource.BaseOurUnitPrice,
                MinimumOrderQty = updateResource.MinimumOrderQty,
                MinimumDeliveryQty = updateResource.MinimumDeliveryQty,
                OrderIncrement = updateResource.OrderIncrement,
                ReelOrBoxQty = updateResource.ReelOrBoxQty,
                DeliveryAddress = updateResource.AddressId == null ? null :
                                                            new Address { Id = (int)updateResource.AddressId },
                LeadTimeWeeks = updateResource.LeadTimeWeeks,
                ContractLeadTimeWeeks = updateResource.ContractLeadTimeWeeks,
                OverbookingAllowed = updateResource.OverbookingAllowed,
                DamagesPercent = updateResource.DamagesPercent,
                WebAddress = updateResource.WebAddress,
                DeliveryInstructions = updateResource.DeliveryInstructions,
                NotesForBuyer = updateResource.NotesForBuyer,
                DutyPercent = updateResource.DutyPercent,
                Tariff = updateResource.TariffId == null
                                                   ? null : new Tariff
                                                   {
                                                       Id = (int)updateResource.TariffId
                                                   },
                PackagingGroup = updateResource.PackagingGroupId == null
                                                           ? null : new PackagingGroup
                                                           {
                                                               Id = (int)updateResource.PackagingGroupId
                                                           }
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
