namespace Linn.Purchasing.Facade.Extensions
{
    using System;
    using System.Linq;

    using Linn.Purchasing.Domain.LinnApps.AutomaticPurchaseOrders;
    using Linn.Purchasing.Resources;

    public static class AutomaticPurchaseOrderResourceExtensions
    {
        public static AutomaticPurchaseOrder ToDomain(this AutomaticPurchaseOrderResource resource)
        {
            return new AutomaticPurchaseOrder
                       {
                           DateRaised = string.IsNullOrEmpty(resource.DateRaised) ? DateTime.Now : DateTime.Parse(resource.DateRaised),
                           SupplierId = resource.SupplierId,
                           Planner = resource.Planner,
                           Id = resource.Id,
                           JobRef = resource.JobRef,
                           StartedBy = resource.StartedBy,
                           Details = resource.Details.Select(d => new AutomaticPurchaseOrderDetail
                                                                      {
                                                                          Id = d.Id,
                                                                          Sequence = d.Sequence,
                                                                          PartNumber = d.PartNumber,
                                                                          SupplierId = d.SupplierId,
                                                                          SupplierName = d.SupplierName,
                                                                          OrderNumber = d.OrderNumber,
                                                                          Quantity = d.Quantity,
                                                                          QuantityRecommended = d.QuantityRecommended,
                                                                          RecommendationCode = d.RecommendationCode,
                                                                          OrderLog = d.OrderLog,
                                                                          CurrencyCode = d.CurrencyCode,
                                                                          CurrencyPrice = d.CurrencyPrice,
                                                                          BasePrice = d.BasePrice,
                                                                          RequestedDate = DateTime.Parse(d.RequestedDate),
                                                                          OrderMethod = d.OrderMethod,
                                                                          IssuePartsToSupplier = d.IssuePartsToSupplier,
                                                                          IssueSerialNumbers = d.IssueSerialNumbers
                                                                      })
                       };
        }
    }
}
