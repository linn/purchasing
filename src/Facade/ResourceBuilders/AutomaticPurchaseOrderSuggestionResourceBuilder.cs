namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.AutomaticPurchaseOrders;
    using Linn.Purchasing.Resources;

    public class AutomaticPurchaseOrderSuggestionResourceBuilder : IBuilder<AutomaticPurchaseOrderSuggestion>
    {
        public object Build(AutomaticPurchaseOrderSuggestion model, IEnumerable<string> claims)
        {
            return new AutomaticPurchaseOrderSuggestionResource
                       {
                           PartNumber = model.PartNumber,
                           PreferredSupplierId = model.PreferredSupplierId,
                           RecommendedQuantity = model.RecommendedQuantity,
                           RecommendedDate = model.RecommendedDate.ToString("o"),
                           RecommendationCode = model.RecommendationCode,
                           CurrencyCode = model.CurrencyCode,
                           OurPrice = model.OurPrice,
                           SupplierName = model.SupplierName,
                           OrderMethod = model.OrderMethod,
                           JitReorderNumber = model.JitReorderNumber,
                           VendorManager = model.VendorManager,
                           Planner = model.Planner,
                           JobRef = model.JobRef
                       };
        }

        public string GetLocation(AutomaticPurchaseOrderSuggestion model)
        {
            throw new System.NotImplementedException();
        }
    }
}
