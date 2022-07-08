namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Resources;

    public class AutomaticPurchaseOrderResourceBuilder : IBuilder<AutomaticPurchaseOrder>
    {
        public AutomaticPurchaseOrderResource Build(AutomaticPurchaseOrder entity, IEnumerable<string> claims)
        {
            return new AutomaticPurchaseOrderResource
                       {
                           Id = entity.Id,
                           StartedBy = entity.StartedBy,
                           JobRef = entity.JobRef,
                           DateRaised = entity.DateRaised.ToString("o"),
                           SupplierId = entity.SupplierId,
                           Planner = entity.Planner,
                           Details = entity.Details?.Select(d => new AutomaticPurchaseOrderDetailResource
                                                                    {
                                                                        Id = d.Id,
                                                                        Sequence = d.Sequence,
                                                                        PartNumber = d.PartNumber,
                                                                        SupplierId = d.SupplierId,
                                                                        OrderNumber = d.OrderNumber,
                                                                        Quantity = d.Quantity,
                                                                        QuantityRecommended = d.QuantityRecommended,
                                                                        RecommendationCode = d.RecommendationCode,
                                                                        OrderLog = d.OrderLog,
                                                                        CurrencyCode = d.CurrencyCode,
                                                                        CurrencyPrice = d.CurrencyPrice,
                                                                        BasePrice = d.BasePrice,
                                                                        RequestedDate = d.RequestedDate?.ToString("o"),
                                                                        OrderMethod = d.OrderMethod,
                                                                        IssuePartsToSupplier = d.IssuePartsToSupplier,
                                                                        IssueSerialNumbers = d.IssueSerialNumbers
                                                                    }),
                           Links = this.BuildLinks(entity, claims).ToArray()
            };
        }

        public string GetLocation(AutomaticPurchaseOrder entity)
        {
            return $"/purchasing/automatic-purchase-orders/{entity.Id}";
        }

        object IBuilder<AutomaticPurchaseOrder>.Build(AutomaticPurchaseOrder entity, IEnumerable<string> claims)
        {
            return this.Build(entity, claims);
        }

        private IEnumerable<LinkResource> BuildLinks(AutomaticPurchaseOrder model, IEnumerable<string> claims)
        {
            if (model != null)
            {
                yield return new LinkResource { Rel = "self", Href = this.GetLocation(model) };
            }
        }
    }
}
