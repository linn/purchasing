namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Authorisation;
    using Linn.Common.Facade;
    using Linn.Common.Reporting.Models;
    using Linn.Common.Resources;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Resources;

    public class PurchaseOrderResourceBuilder : IBuilder<PurchaseOrder>
    {
        private readonly IAuthorisationService authService;

        public PurchaseOrderResourceBuilder(IAuthorisationService authService)
        {
            this.authService = authService;
        }

        public PurchaseOrderResource Build(PurchaseOrder entity, IEnumerable<string> claims)
        {
            if (entity == null)
            {
                return new PurchaseOrderResource
                           {
                               Links = this.BuildLinks(null, claims).ToArray()
                           };
            }

            return new PurchaseOrderResource
                       {
                           OrderNumber = entity.OrderNumber,
                           Cancelled = entity.Cancelled,
                           DocumentType = entity.DocumentType,
                           DateOfOrder = entity.OrderDate,
                           Overbook = entity.Overbook,
                           OverbookQty = entity.OverbookQty,
                           SupplierId = entity.SupplierId,
                           Links = this.BuildLinks(entity, claims).ToArray()
                       };
        }

        public string GetLocation(PurchaseOrder p)
        {
            return $"/purchasing/purchase-orders/{p.OrderNumber}";
        }

        object IBuilder<PurchaseOrder>.Build(PurchaseOrder entity, IEnumerable<string> claims) => this.Build(entity, claims);

        private IEnumerable<LinkResource> BuildLinks(PurchaseOrder model, IEnumerable<string> claims)
        {
            var privileges = claims as string[] ?? claims.ToArray();

            if (model != null)
            {
                yield return new LinkResource { Rel = "self", Href = this.GetLocation(model) };

                if (this.authService.HasPermissionFor(AuthorisedAction.PurchaseOrderUpdate, privileges))
                {
                    yield return new LinkResource { Rel = "edit", Href = this.GetLocation(model) };
                    yield return new LinkResource { Rel = "allow-over-book", Href = $"{this.GetLocation(model)}/allow-over-book" };
                }
            }
        }
    }
}
