﻿namespace Linn.Purchasing.Facade.Services
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

        protected override PartSupplier CreateFromResource(
            PartSupplierResource resource, 
            IEnumerable<string> privileges = null)        
        {
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

        protected override void DeleteOrObsoleteResource(
            PartSupplier entity, 
            IEnumerable<string> privileges = null)        
        {
            throw new NotImplementedException();
        }

        protected override void UpdateFromResource(
            PartSupplier entity, 
            PartSupplierResource updateResource, 
            IEnumerable<string> privileges = null)        
        {
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
            return x => (string.IsNullOrEmpty(searchResource.PartNumberSearchTerm) 
                         || x.PartNumber.Contains(searchResource.PartNumberSearchTerm.Trim().ToUpper()))
                        &&
                        (string.IsNullOrEmpty(searchResource.SupplierNameSearchTerm) 
                         || x.Supplier.Name.Contains(searchResource.SupplierNameSearchTerm.Trim().ToUpper()));
        }

        protected override Expression<Func<PartSupplier, bool>> FindExpression(PartSupplierSearchResource searchResource)
        {
            throw new NotImplementedException();
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
                DeliveryFullAddress = resource.AddressId == null ? null :
                                                            new FullAddress { Id = (int)resource.AddressId },
                UnitOfMeasure = resource.UnitOfMeasure,
                LeadTimeWeeks = resource.LeadTimeWeeks,
                DamagesPercent = resource.DamagesPercent,
                DeliveryInstructions = resource.DeliveryInstructions,
                NotesForBuyer = resource.NotesForBuyer,
                MadeInvalidBy = resource.MadeInvalidBy.HasValue
                                    ? new Employee { Id = (int)resource.MadeInvalidBy }
                                    : null,
                DateInvalid = string.IsNullOrEmpty(resource.DateInvalid)
                                                        ? null : DateTime.Parse(resource.DateInvalid),
                Manufacturer = string.IsNullOrEmpty(resource.ManufacturerCode) ? null :
                                                         new Manufacturer { Code = resource.ManufacturerCode },
                ManufacturerPartNumber = resource.ManufacturerPartNumber,
                VendorPartNumber = resource.VendorPartNumber
            };
        }
    }
}
