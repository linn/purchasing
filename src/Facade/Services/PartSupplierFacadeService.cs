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

        protected override PartSupplier CreateFromResource(PartSupplierResource resource, IEnumerable<string> privileges = null)        {
            var candidate = this.BuildEntityFromResourceHelper(resource);
            candidate.PartNumber = resource.PartNumber;
            candidate.SupplierId = resource.SupplierId;
            candidate.CreatedBy = resource.CreatedBy.HasValue 
                                      ? new Employee { Id = (int)resource.CreatedBy } : null;
            return this.domainService.CreatePartSupplier(candidate, privileges);
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

        protected override void DeleteOrObsoleteResource(PartSupplier entity, IEnumerable<string> privileges = null)        {
            throw new NotImplementedException();
        }

        protected override void UpdateFromResource(PartSupplier entity, PartSupplierResource updateResource, IEnumerable<string> privileges = null)        {
            var updated = this.BuildEntityFromResourceHelper(updateResource);

            updated.PartNumber = entity.PartNumber;
            updated.SupplierId = entity.SupplierId;

            this.domainService.UpdatePartSupplier(entity, updated, privileges);
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

        private PartSupplier BuildEntityFromResourceHelper(PartSupplierResource resource)
        {
            return new PartSupplier
            {
                SupplierDesignation = resource.Designation,
                OrderMethod = new OrderMethod { Name = resource.OrderMethodName },
                Currency = new Currency { Code = resource.CurrencyCode },
                CurrencyUnitPrice = resource.CurrencyUnitPrice,
                OurCurrencyPriceToShowOnOrder = resource.OurCurrencyPriceToShowOnOrder,
                BaseOurUnitPrice = resource.BaseOurUnitPrice,
                MinimumOrderQty = resource.MinimumOrderQty,
                MinimumDeliveryQty = resource.MinimumDeliveryQty,
                OrderIncrement = resource.OrderIncrement,
                ReelOrBoxQty = resource.ReelOrBoxQty,
                DeliveryAddress = resource.AddressId == null ? null :
                                                            new Address { Id = (int)resource.AddressId },
                LeadTimeWeeks = resource.LeadTimeWeeks,
                ContractLeadTimeWeeks = resource.ContractLeadTimeWeeks,
                OverbookingAllowed = resource.OverbookingAllowed,
                DamagesPercent = resource.DamagesPercent,
                WebAddress = resource.WebAddress,
                DeliveryInstructions = resource.DeliveryInstructions,
                NotesForBuyer = resource.NotesForBuyer,
                DutyPercent = resource.DutyPercent,
                Tariff = resource.TariffId == null
                                                   ? null : new Tariff
                                                   {
                                                       Id = (int)resource.TariffId
                                                   },
                PackagingGroup = resource.PackagingGroupId == null
                                                           ? null : new PackagingGroup
                                                           {
                                                               Id = (int)resource.PackagingGroupId
                                                           },
                MadeInvalidBy = resource.MadeInvalidBy.HasValue
                                                          ? new Employee { Id = (int)resource.MadeInvalidBy }
                                                          : null,
                DateInvalid = string.IsNullOrEmpty(resource.DateInvalid)
                                                        ? null : DateTime.Parse(resource.DateInvalid),
                Manufacturer = string.IsNullOrEmpty(resource.ManufacturerCode) ? null :
                                                         new Manufacturer { Code = resource.ManufacturerCode },
                ManufacturerPartNumber = resource.ManufacturerPartNumber,
                VendorPartNumber = resource.VendorPartNumber,
                RohsCategory = resource.RohsCategory,
                DateRohsCompliant = string.IsNullOrEmpty(resource.DateRohsCompliant)
                                                            ? null : DateTime.Parse(resource.DateRohsCompliant),
                RohsComments = resource.RohsComments,
                RohsCompliant = resource.RohsCompliant
            };
        }
    }
}
