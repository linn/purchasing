﻿namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Authorisation;
    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Resources;

    public class PurchaseOrderResourceBuilder : IBuilder<PurchaseOrder>
    {
        private readonly IBuilder<Address> addressResourceBuilder;

        private readonly IAuthorisationService authService;

        private readonly IBuilder<LinnDeliveryAddress> deliveryAddressResourceBuilder;

        private readonly IBuilder<PurchaseOrderDetail> detailResourceBuilder;

        private readonly IBuilder<Supplier> supplierResourceBuilder;

        public PurchaseOrderResourceBuilder(
            IAuthorisationService authService,
            IBuilder<PurchaseOrderDetail> detailResourceBuilder,
            IBuilder<LinnDeliveryAddress> deliveryAddressResourceBuilder,
            IBuilder<Address> addressResourceBuilder,
            IBuilder<Supplier> supplierResourceBuilder)
        {
            this.authService = authService;
            this.detailResourceBuilder = detailResourceBuilder;
            this.deliveryAddressResourceBuilder = deliveryAddressResourceBuilder;
            this.addressResourceBuilder = addressResourceBuilder;
            this.supplierResourceBuilder = supplierResourceBuilder;
        }

        public PurchaseOrderResource Build(PurchaseOrder entity, IEnumerable<string> claims)
        {
            var claimsList = claims.ToList();
            if (entity == null)
            {
                return new PurchaseOrderResource { Links = this.BuildLinks(null, claimsList).ToArray() };
            }

            var cancelledDetail = entity.Details?.FirstOrDefault()?.CancelledDetails?.FirstOrDefault(a => a.CancelledById.HasValue);

            return new PurchaseOrderResource
                       {
                           OrderNumber = entity.OrderNumber,
                           Currency = new CurrencyResource { Code = entity.CurrencyCode, Name = entity.Currency?.Name },
                           Cancelled = entity.Cancelled,
                           DocumentType =
                               new DocumentTypeResource
                                   {
                                       Name = entity.DocumentType?.Name, Description = entity.DocumentType?.Description
                                   },
                           OrderDate = entity.OrderDate.ToString("O"),
                           OrderMethod =
                               new OrderMethodResource
                                   {
                                       Description = entity.OrderMethod?.Description, Name = entity.OrderMethodName
                                   },
                           Overbook = entity.Overbook,
                           OverbookQty = entity.OverbookQty,
                           Details =
                               entity.Details?.Select(
                                   d => (PurchaseOrderDetailResource)this.detailResourceBuilder.Build(d, claimsList)),
                           OrderContactName = entity.OrderContactName,
                           ExchangeRate = entity.ExchangeRate,
                           IssuePartsToSupplier = entity.IssuePartsToSupplier,
                           DeliveryAddress =
                               entity.DeliveryAddress != null
                                   ? (LinnDeliveryAddressResource)this.deliveryAddressResourceBuilder.Build(
                                       entity.DeliveryAddress,
                                       claimsList)
                                   : null,
                           RequestedBy =
                               new EmployeeResource
                                   {
                                       Id = entity.RequestedById, FullName = entity.RequestedBy?.FullName
                                   },
                           EnteredBy =
                               new EmployeeResource { Id = entity.EnteredById, FullName = entity.EnteredBy?.FullName },
                           QuotationRef = entity.QuotationRef,
                           AuthorisedBy =
                               entity.AuthorisedById.HasValue
                                   ? new EmployeeResource
                                         {
                                             Id = (int)entity.AuthorisedById, FullName = entity.AuthorisedBy?.FullName
                                         }
                                   : null,
                           SentByMethod = entity.SentByMethod,
                           FilCancelled = entity.FilCancelled,
                           Remarks = entity.Remarks,
                           DateFilCancelled = entity.DateFilCancelled?.ToString("O"),
                           PeriodFilCancelled = entity.PeriodFilCancelled,
                           Supplier = (SupplierResource)this.supplierResourceBuilder.Build(entity.Supplier, null),
                           OrderAddress =
                               entity.OrderAddress != null
                                   ? (AddressResource)this.addressResourceBuilder.Build(entity.OrderAddress, claimsList)
                                   : null,
                           InvoiceAddressId = entity.InvoiceAddressId,
                           SupplierContactEmail =
                               entity.Supplier.SupplierContacts?.FirstOrDefault(c => c.IsMainOrderContact == "Y")
                                   ?.EmailAddress,
                           SupplierContactPhone =
                               entity.Supplier.SupplierContacts?.FirstOrDefault(c => c.IsMainOrderContact == "Y")
                                   ?.PhoneNumber,
                           BaseOrderNetTotal = entity.BaseOrderNetTotal,
                           OrderNetTotal = entity.OrderNetTotal,
                           Links = this.BuildLinks(entity, claimsList).ToArray(),
                           CancelledByName = cancelledDetail?.CancelledBy?.FullName,
                           DateCancelled = cancelledDetail != null && cancelledDetail.DateCancelled.HasValue 
                                                ? cancelledDetail.DateCancelled.Value.ToString("dd/MM/yyyy") 
                                                : string.Empty,
                           ReasonCancelled = cancelledDetail?.ReasonCancelled,
                           LedgerEntries = entity.LedgerEntries?.OrderByDescending(x => x.Pltref).Select(
                               e => new PurchaseLedgerResource
                                        {
                                            TransType = e.PlTransType, 
                                            PlDeliveryRef = e.PlDeliveryRef, 
                                            Qty = e.PlQuantity, 
                                            NetTotal = e.PlNetTotal,
                                            VatTotal = e.PlVat,
                                            InvoiceRef = e.PlInvoiceRef,
                                            BaseVat = e.BaseVatTotal,
                                            InvoiceDate = e.InvoiceDate.ToString("dd/MM/yyyy"),
                                            Tref = e.Pltref
                                        })
                       };
        }

        public string GetLocation(PurchaseOrder p)
        {
            return $"/purchasing/purchase-orders/{p.OrderNumber}";
        }

        object IBuilder<PurchaseOrder>.Build(PurchaseOrder entity, IEnumerable<string> claims)
        {
            return this.Build(entity, claims);
        }

        private IEnumerable<LinkResource> BuildLinks(PurchaseOrder model, IEnumerable<string> claims)
        {
            var privileges = claims as string[] ?? claims.ToArray();

            if (this.authService.HasPermissionFor(AuthorisedAction.PurchaseOrderCreate, privileges))
            {
                yield return new LinkResource { Rel = "create", Href = "/purchasing/purchase-orders/create" };
                yield return new LinkResource
                                 {
                                     Rel = "quick-create", Href = "/purchasing/purchase-orders/quick-create"
                                 };
                yield return new LinkResource
                                 {
                                     Rel = "generate-order-fields",
                                     Href = "/purchasing/purchase-orders/generate-order-from-supplier-id"
                                 };
            }

            if (this.authService.HasPermissionFor(AuthorisedAction.PurchaseOrderCreateForOtherUser, privileges))
            {
                yield return new LinkResource { Rel = "create-for-other-user", Href = "/purchasing/purchase-orders/create" };
            }

            if (model != null)
            {
                yield return new LinkResource { Rel = "self", Href = this.GetLocation(model) };
                yield return new LinkResource { Rel = "html", Href = $"{this.GetLocation(model)}/html" };

                if (this.authService.HasPermissionFor(AuthorisedAction.PurchaseOrderUpdate, privileges))
                {
                    if (model.Cancelled != "Y")
                    {
                        yield return new LinkResource { Rel = "edit", Href = this.GetLocation(model) };
                    }

                    if (this.authService.HasPermissionFor(AuthorisedAction.PurchaseOrderUpdate, privileges))
                    {
                        yield return new LinkResource
                                         {
                                             Rel = "overbook",
                                             Href = this.GetLocation(model)
                                         };
                    }
                }

                if (
                    this.authService.HasPermissionFor(AuthorisedAction.PurchaseOrderAuthorise, privileges)
                    && !model.AuthorisedById.HasValue)
                {
                    yield return new LinkResource
                                     {
                                         Rel = "authorise", Href = $"{this.GetLocation(model)}/authorise"
                                     };
                }

                if (this.authService.HasPermissionFor(AuthorisedAction.PurchaseOrderAuthorise, privileges)
                    && model.AuthorisedById.HasValue)
                {
                    yield return new LinkResource
                                     {
                                         Rel = "email-dept",
                                         Href = $"{this.GetLocation(model)}/email-dept"
                                     };
                }

                if (this.authService.HasPermissionFor(AuthorisedAction.PurchaseOrderFilCancel, privileges))
                {
                    yield return new LinkResource
                                     {
                                         Rel = "fil-cancel",
                                         Href = $"{this.GetLocation(model)}"
                                     };
                }

                if (model.DocumentType?.Name == "RO" && model.ReturnsCreditDebitNotes != null && model.ReturnsCreditDebitNotes.Any())
                {
                    yield return new LinkResource
                    {
                        Rel = "ret-credit-debit-note",
                        Href = $"/purchasing/pl-credit-debit-notes/{model.ReturnsCreditDebitNotes.First().NoteNumber}"
                    };
                }
            }
        }
    }
}
