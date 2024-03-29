﻿namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Authorisation;
    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Resources;

    public class SupplierResourceBuilder : IBuilder<Supplier>
    {
        private readonly IBuilder<Address> addressResourceBuilder;

        private readonly IAuthorisationService authService;

        private readonly IBuilder<SupplierContact> supplierContactResourceBuilder;

        public SupplierResourceBuilder(
            IAuthorisationService authService,
            IBuilder<SupplierContact> supplierContactResourceBuilder,
            IBuilder<Address> addressResourceBuilder)
        {
            this.authService = authService;
            this.addressResourceBuilder = addressResourceBuilder;
            this.supplierContactResourceBuilder = supplierContactResourceBuilder;
        }

        public SupplierResource Build(Supplier entity, IEnumerable<string> claims)
        {
            if (entity == null)
            {
                return new SupplierResource { Links = this.BuildLinks(null, claims).ToArray() };
            }

            return new SupplierResource
                       {
                           Id = entity.SupplierId,
                           Name = entity.Name,
                           PhoneNumber = entity.PhoneNumber,
                           InvoiceContactMethod = entity.InvoiceContactMethod,
                           SuppliersReference = entity.SuppliersReference,
                           WebAddress = entity.WebAddress,
                           OrderContactMethod = entity.OrderContactMethod,
                           VendorManagerId = entity.VendorManagerId,
                           ApprovedCarrier = entity.ApprovedCarrier,
                           CurrencyCode = entity.Currency?.Code,
                           CurrencyName = entity.Currency?.Name,
                           ExpenseAccount = entity.ExpenseAccount,
                           InvoiceGoesToId = entity.InvoiceGoesTo?.SupplierId,
                           InvoiceGoesToName = entity.InvoiceGoesTo?.Name,
                           PaymentDays = entity.PaymentDays,
                           PaymentMethod = entity.PaymentMethod,
                           PaysInFc = entity.PaysInFc,
                           AccountingCompany = entity.AccountingCompany,
                           VatNumber = entity.VatNumber,
                           PartCategory = entity.PartCategory?.Category,
                           PartCategoryDescription = entity.PartCategory?.Description,
                           OrderHold = entity.OrderHold,
                           NotesForBuyer = entity.NotesForBuyer,
                           DeliveryDay = entity.DeliveryDay,
                           RefersToFcId = entity.RefersToFc?.SupplierId,
                           RefersToFcName = entity.RefersToFc?.Name,
                           PmDeliveryDaysGrace = entity.PmDeliveryDaysGrace,
                           InvoiceAddressId = entity.InvoiceFullAddress?.Id,
                           InvoiceFullAddress = entity.InvoiceFullAddress?.AddressString,
                           OrderAddressId = entity.OrderAddress?.AddressId,
                           OrderFullAddress = entity.OrderAddress?.FullAddress?.AddressString,
                           PlannerId = entity.Planner?.Id,
                           AccountControllerId = entity.AccountController?.Id,
                           AccountControllerName = entity.AccountController?.FullName,
                           OpenedById = entity.OpenedBy?.Id,
                           DateOpened = entity.DateOpened.ToString("o"),
                           OpenedByName = entity.OpenedBy?.FullName,
                           ClosedById = entity.ClosedBy?.Id,
                           ClosedByName = entity.ClosedBy?.FullName,
                           DateClosed = entity.DateClosed?.ToString("o"),
                           ReasonClosed = entity.ReasonClosed,
                           Notes = entity.Notes,
                           OrganisationId = entity.OrganisationId,
                           SupplierContacts =
                               entity.SupplierContacts?.Where(c => !c.DateInvalid.HasValue).Select(
                                   c => (SupplierContactResource)this.supplierContactResourceBuilder.Build(c, null)),
                           GroupId = entity.Group?.Id,
                           Country = entity.Country,
                           OrderAddress =
                               entity.OrderAddress != null
                                   ? (AddressResource)this.addressResourceBuilder.Build(entity.OrderAddress, new List<string>())
                                   : null,
                           ReceivesPurchaseOrderReminders = entity.ReceivesOrderReminders == "Y",
                           PrintTerms = entity.PrintTerms == "Y",
                           Links = this.BuildLinks(entity, claims).ToArray()
                       };
        }

        public string GetLocation(Supplier p)
        {
            return $"/purchasing/suppliers/{p.SupplierId}";
        }

        object IBuilder<Supplier>.Build(Supplier entity, IEnumerable<string> claims)
        {
            return this.Build(entity, claims);
        }

        private IEnumerable<LinkResource> BuildLinks(Supplier model, IEnumerable<string> claims)
        {
            if (model != null)
            {
                yield return new LinkResource { Rel = "self", Href = this.GetLocation(model) };

                if (!string.IsNullOrEmpty(model.VendorManagerId))
                {
                    yield return new LinkResource { Rel = "vendor-manager", Href = $"/purchasing/vendor-managers/{model.VendorManagerId}" };
                }
            }

            var privileges = claims?.ToList();

            if (model != null && this.authService.HasPermissionFor(AuthorisedAction.SupplierUpdate, privileges))
            {
                yield return new LinkResource { Rel = "edit", Href = $"{this.GetLocation(model)}/edit" };
            }

            if (model != null && this.authService.HasPermissionFor(AuthorisedAction.PartSupplierUpdate, privileges))
            {
                yield return new LinkResource
                                 {
                                     Rel = "bulk-update-lead-times",
                                     Href = $"/purchasing/suppliers/bulk-lead-times?supplierId={model.SupplierId}"
                                 };
            }

            if (this.authService.HasPermissionFor(AuthorisedAction.SupplierCreate, privileges))
            {
                yield return new LinkResource { Rel = "create", Href = "/purchasing/suppliers/create" };
            }

            if (this.authService.HasPermissionFor(AuthorisedAction.SupplierHoldChange, privileges))
            {
                yield return new LinkResource { Rel = "hold", Href = "/purchasing/suppliers/hold" };
            }


            if (this.authService.HasPermissionFor(AuthorisedAction.SendEdi, privileges))
            {
                yield return new LinkResource { Rel = "edi", Href = "/purchasing/edi/orders" };
            }
        }
    }
}
