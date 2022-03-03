namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Authorisation;
    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Resources;

    public class PurchaseOrderReqResourceBuilder : IBuilder<PurchaseOrderReq>
    {
        private readonly IAuthorisationService authService;

        public PurchaseOrderReqResourceBuilder(IAuthorisationService authService)
        {
            this.authService = authService;
        }

        public PurchaseOrderReqResource Build(PurchaseOrderReq entity, IEnumerable<string> claims)
        {
            if (entity == null)
            {
                return new PurchaseOrderReqResource { Links = this.BuildLinks(null, claims).ToArray() };
            }

            return new PurchaseOrderReqResource
                       {
                           ReqNumber = entity.ReqNumber,
                           State = entity.State,
                           ReqDate = entity.ReqDate.ToString("o"),
                           OrderNumber = entity.OrderNumber,
                           PartNumber = entity.PartNumber,
                           PartDescription = entity.PartDescription,
                           Qty = entity.Qty,
                           UnitPrice = entity.UnitPrice,
                           Carriage = entity.Carriage,
                           TotalReqPrice = entity.TotalReqPrice,
                           Currency = entity.Currency != null ? new CurrencyResource { Code = entity.Currency?.Code } : null,
                           Supplier = entity.Supplier != null ? new SupplierResource { Id = entity.Supplier.SupplierId, Name = entity.Supplier?.Name } : null,
                           SupplierContact = entity.SupplierContact,
                           AddressLine1 = entity.AddressLine1,
                           AddressLine2 = entity.AddressLine2,
                           AddressLine3 = entity.AddressLine3,
                           AddressLine4 = entity.AddressLine4,
                           PostCode = entity.PostCode,
                           Country = entity.Country != null ? new CountryResource { CountryCode = entity.Country.CountryCode } : null,
                           PhoneNumber = entity.PhoneNumber,
                           QuoteRef = entity.QuoteRef,
                           Email = entity.Email,
                           DateRequired = entity.DateRequired.HasValue ? entity.DateRequired.Value.ToString("o") : null,
                           RequestedBy = new EmployeeResource { Id = entity.RequestedBy.Id },
                           AuthorisedBy = entity.AuthorisedBy != null ? new EmployeeResource { Id = entity.AuthorisedBy.Id } : null,
                           SecondAuthBy = entity.SecondAuthBy != null ? new EmployeeResource { Id = entity.SecondAuthBy.Id } : null,
                           FinanceCheckBy = entity.FinanceCheckBy != null ? new EmployeeResource { Id = entity.FinanceCheckBy.Id } : null,
                           TurnedIntoOrderBy = entity.TurnedIntoOrderBy != null ? new EmployeeResource { Id = entity.TurnedIntoOrderBy.Id } : null,
                           Nominal = entity.Nominal,
                           RemarksForOrder = entity.RemarksForOrder,
                           InternalNotes = entity.InternalNotes,
                           Department = entity.Department,
                           Links = this.BuildLinks(entity, claims).ToArray()
                       };
        }

        public string GetLocation(PurchaseOrderReq p)
        {
            return $"/purchasing/purchase-orders/reqs/{p.ReqNumber}";
        }

        object IBuilder<PurchaseOrderReq>.Build(PurchaseOrderReq entity, IEnumerable<string> claims)
        {
            return this.Build(entity, claims);
        }

        private IEnumerable<LinkResource> BuildLinks(PurchaseOrderReq model, IEnumerable<string> claims)
        {
            var privileges = claims as string[] ?? claims.ToArray();

            if (model != null)
            {
                yield return new LinkResource { Rel = "self", Href = this.GetLocation(model) };

                if (this.authService.HasPermissionFor(AuthorisedAction.PurchaseOrderReqUpdate, privileges))
                {
                    yield return new LinkResource { Rel = "edit", Href = this.GetLocation(model) };
                }

                if (this.authService.HasPermissionFor(AuthorisedAction.PurchaseOrderReqCreate, privileges))
                {
                    yield return new LinkResource { Rel = "create", Href = this.GetLocation(model) };
                }
            }
        }
    }
}
