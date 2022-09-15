namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Authorisation;
    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Resources;

    public class PurchaseOrderResourceBuilder : IBuilder<PurchaseOrder>
    {
        private readonly IBuilder<Address> addressResourceBuilder;

        private readonly IAuthorisationService authService;

        private readonly IBuilder<LinnDeliveryAddress> deliveryAddressResourceBuilder;

        private readonly IBuilder<PurchaseOrderDetail> detailResourceBuilder;

        public PurchaseOrderResourceBuilder(
            IAuthorisationService authService,
            IBuilder<PurchaseOrderDetail> detailResourceBuilder,
            IBuilder<LinnDeliveryAddress> deliveryAddressResourceBuilder,
            IBuilder<Address> addressResourceBuilder)
        {
            this.authService = authService;
            this.detailResourceBuilder = detailResourceBuilder;
            this.deliveryAddressResourceBuilder = deliveryAddressResourceBuilder;
            this.addressResourceBuilder = addressResourceBuilder;
        }

        public PurchaseOrderResource Build(PurchaseOrder entity, IEnumerable<string> claims)
        {
            if (entity == null)
            {
                return new PurchaseOrderResource { Links = this.BuildLinks(null, claims).ToArray() };
            }

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
                                   d => (PurchaseOrderDetailResource) this.detailResourceBuilder.Build(d, claims)),
                           OrderContactName = entity.OrderContactName,
                           ExchangeRate = entity.ExchangeRate,
                           IssuePartsToSupplier = entity.IssuePartsToSupplier,
                           DeliveryAddress =
                               entity.DeliveryAddress != null
                                   ? (LinnDeliveryAddressResource) this.deliveryAddressResourceBuilder.Build(
                                       entity.DeliveryAddress,
                                       claims)
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
                                             Id = (int) entity.AuthorisedById, FullName = entity.AuthorisedBy?.FullName
                                         }
                                   : null,
                           SentByMethod = entity.SentByMethod,
                           FilCancelled = entity.FilCancelled,
                           Remarks = entity.Remarks,
                           DateFilCancelled = entity.DateFilCancelled?.ToString("O"),
                           PeriodFilCancelled = entity.PeriodFilCancelled,
                           Supplier =
                               new SupplierResource
                                   {
                                       Id = entity.Supplier.SupplierId,
                                       Name = entity.Supplier.Name,
                                       VendorManagerId = entity.Supplier.VendorManagerId
                                   },
                           OrderAddress =
                               entity.OrderAddress != null
                                   ? (AddressResource) this.addressResourceBuilder.Build(entity.OrderAddress, claims)
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
                           Links = this.BuildLinks(entity, claims).ToArray()
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

            if (this.authService.HasPermissionFor(AuthorisedAction.PurchaseOrderUpdate, privileges))
            {
                yield return new LinkResource
                                 {
                                     Rel = "allow-over-book-search",
                                     Href = "/purchasing/purchase-orders/allow-over-book"
                                 };
            }

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

            if (model != null)
            {
                yield return new LinkResource { Rel = "self", Href = this.GetLocation(model) };
                yield return new LinkResource { Rel = "html", Href = $"{this.GetLocation(model)}/html" };

                if (this.authService.HasPermissionFor(AuthorisedAction.PurchaseOrderUpdate, privileges))
                {
                    yield return new LinkResource { Rel = "edit", Href = this.GetLocation(model) };
                    yield return new LinkResource
                                     {
                                         Rel = "allow-over-book", Href = $"{this.GetLocation(model)}/allow-over-book"
                                     };
                }

                if (this.authService.HasPermissionFor(AuthorisedAction.PurchaseOrderAuthorise, privileges))
                {
                    yield return new LinkResource { Rel = "authorise", Href = $"{this.GetLocation(model)}/authorise" };
                }
            }
        }
    }
}
