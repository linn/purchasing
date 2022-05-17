namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Authorisation;
    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrderReqs;
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
                           StateDescription = entity.ReqState?.Description,
                           ReqDate = entity.ReqDate.ToString("o"),
                           OrderNumber = entity.OrderNumber,
                           PartNumber = entity.PartNumber,
                           Description = entity.Description,
                           Qty = entity.Qty,
                           UnitPrice = entity.UnitPrice,
                           Carriage = entity.Carriage,
                           TotalReqPrice = entity.TotalReqPrice,
                           Currency =
                               entity.Currency != null
                                   ? new CurrencyResource { Code = entity.Currency.Code, Name = entity.Currency.Name }
                                   : null,
                           Supplier =
                               entity.Supplier != null
                                   ? new SupplierResource
                                         {
                                             Id = entity.Supplier.SupplierId, Name = entity.Supplier?.Name
                                         }
                                   : null,
                           SupplierContact = entity.SupplierContact,
                           AddressLine1 = entity.AddressLine1,
                           AddressLine2 = entity.AddressLine2,
                           AddressLine3 = entity.AddressLine3,
                           AddressLine4 = entity.AddressLine4,
                           PostCode = entity.PostCode,
                           Country =
                               entity.Country != null
                                   ? new CountryResource
                                         {
                                             CountryCode = entity.Country.CountryCode, CountryName = entity.Country.Name
                                         }
                                   : null,
                           PhoneNumber = entity.PhoneNumber,
                           QuoteRef = entity.QuoteRef,
                           Email = entity.Email,
                           DateRequired = entity.DateRequired.HasValue ? entity.DateRequired.Value.ToString("o") : null,
                           RequestedBy =
                               entity.RequestedBy != null
                                   ? new EmployeeResource
                                         {
                                             Id = entity.RequestedBy.Id, FullName = entity.RequestedBy.FullName
                                         }
                                   : null,
                           AuthorisedBy =
                               entity.AuthorisedBy != null
                                   ? new EmployeeResource
                                         {
                                             Id = entity.AuthorisedBy.Id, FullName = entity.AuthorisedBy.FullName
                                         }
                                   : null,
                           SecondAuthBy =
                               entity.SecondAuthBy != null
                                   ? new EmployeeResource
                                         {
                                             Id = entity.SecondAuthBy.Id, FullName = entity.SecondAuthBy.FullName
                                         }
                                   : null,
                           FinanceCheckBy =
                               entity.FinanceCheckBy != null
                                   ? new EmployeeResource
                                         {
                                             Id = entity.FinanceCheckBy.Id, FullName = entity.FinanceCheckBy.FullName
                                         }
                                   : null,
                           TurnedIntoOrderBy =
                               entity.TurnedIntoOrderBy != null
                                   ? new EmployeeResource
                                         {
                                             Id = entity.TurnedIntoOrderBy.Id,
                                             FullName = entity.TurnedIntoOrderBy.FullName
                                         }
                                   : null,
                           Nominal =
                               entity.Nominal != null
                                   ? new NominalResource
                                         {
                                             NominalCode = entity.Nominal.NominalCode,
                                             Description = entity.Nominal.Description
                                         }
                                   : null,
                           RemarksForOrder = entity.RemarksForOrder,
                           InternalNotes = entity.InternalNotes,
                           Department = entity.Department != null
                                            ? new DepartmentResource
                                                  {
                                                      DepartmentCode = entity.Department.DepartmentCode,
                                                      Description = entity.Department.Description
                                                  }
                                            : null,
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
                yield return new LinkResource { Rel = "print", Href = $"{this.GetLocation(model)}/print" };
                yield return new LinkResource { Rel = "edit", Href = this.GetLocation(model) };
                yield return new LinkResource { Rel = "cancel", Href = $"{this.GetLocation(model)}/cancel" };
                yield return new LinkResource { Rel = "authorise", Href = $"{this.GetLocation(model)}/authorise" };
                yield return new LinkResource { Rel = "check-signing-limit-covers", Href = "/purchasing/purchase-orders/check-signing-limit-covers-po-auth" };

                if (this.authService.HasPermissionFor(AuthorisedAction.PurchaseOrderReqFinanceCheck, privileges))
                {
                    yield return new LinkResource
                                     {
                                         Rel = "finance-check", Href = $"{this.GetLocation(model)}/finance-authorise"
                                     };
                }

                if (this.authService.HasPermissionFor(AuthorisedAction.PurchaseOrderCreate, privileges))
                {
                    yield return new LinkResource { Rel = "turn-req-into-purchase-order", Href = $"{this.GetLocation(model)}/turn-into-order" };
                }

                if (model.OrderNumber.HasValue)
                {
                    yield return new LinkResource { Rel = "view-purchase-order", Href = $"/purchasing/purchase-orders/{model.OrderNumber.Value}" };
                }
            }

            yield return new LinkResource { Rel = "create", Href = "/purchasing/purchase-orders/reqs/create" };
        }
    }
}
