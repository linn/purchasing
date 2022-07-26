namespace Linn.Purchasing.Domain.LinnApps.AutomaticPurchaseOrders
{
    using System.Collections.Generic;

    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;

    public class AutomaticPurchaseOrderService : IAutomaticPurchaseOrderService
    {
        private readonly IPurchaseOrderAutoOrderPack purchaseOrderAutoOrderPack;

        private readonly IPurchaseOrdersPack purchaseOrdersPack;

        private readonly ICurrencyPack currencyPack;

        private readonly IRepository<SigningLimit, int> signingLimitRepository;

        public AutomaticPurchaseOrderService(
            IPurchaseOrderAutoOrderPack purchaseOrderAutoOrderPack,
            IPurchaseOrdersPack purchaseOrdersPack,
            ICurrencyPack currencyPack,
            IRepository<SigningLimit, int> signingLimitRepository)
        {
            this.purchaseOrderAutoOrderPack = purchaseOrderAutoOrderPack;
            this.purchaseOrdersPack = purchaseOrdersPack;
            this.currencyPack = currencyPack;
            this.signingLimitRepository = signingLimitRepository;
        }

        public AutomaticPurchaseOrder CreateAutomaticPurchaseOrder(AutomaticPurchaseOrder proposedAutomaticPurchaseOrder)
        {
            var automaticPurchaseOrder = new AutomaticPurchaseOrder
                                             {
                                                 StartedBy = proposedAutomaticPurchaseOrder.StartedBy,
                                                 JobRef = proposedAutomaticPurchaseOrder.JobRef,
                                                 DateRaised = proposedAutomaticPurchaseOrder.DateRaised
                                             };

            var details = new List<AutomaticPurchaseOrderDetail>();
            var signingLimit = this.signingLimitRepository.FindById(proposedAutomaticPurchaseOrder.StartedBy);
            var seq = 1;
            foreach (var detail in proposedAutomaticPurchaseOrder.Details)
            {
                if (detail.RequestedDate != null)
                {
                    var basePrice = this.currencyPack.CalculateBaseValueFromCurrencyValue(detail.CurrencyCode, detail.CurrencyPrice);
                    var issueParts = this.purchaseOrdersPack.IssuePartsToSupplier(detail.PartNumber, detail.SupplierId);
                    var canAuthorise = basePrice <= signingLimit?.ProductionLimit;
                    
                    var orderResult = this.purchaseOrderAutoOrderPack.CreateAutoOrder(
                        detail.PartNumber,
                        detail.SupplierId,
                        detail.Quantity,
                        detail.RequestedDate.Value,
                        null,
                        canAuthorise);

                    if (orderResult.Success)
                    {
                        details.Add(new AutomaticPurchaseOrderDetail
                                        {
                                            Sequence = seq++,
                                            PartNumber = detail.PartNumber,
                                            SupplierId = detail.SupplierId,
                                            SupplierName = detail.SupplierName,
                                            OrderNumber = orderResult.OrderNumber,
                                            Quantity = detail.Quantity,
                                            QuantityRecommended = detail.Quantity,
                                            RecommendationCode = detail.RecommendationCode,
                                            OrderLog = "Created by auto order",
                                            CurrencyCode = detail.CurrencyCode,
                                            CurrencyPrice = detail.CurrencyPrice,
                                            BasePrice = basePrice,
                                            RequestedDate = detail.RequestedDate,
                                            OrderMethod = detail.OrderMethod,
                                            IssuePartsToSupplier = issueParts ? "Y" : "N",
                                            IssueSerialNumbers = "N",
                                            AuthorisedAtCreation = canAuthorise ? "Y" : "N"
                                        });
                    }
                }
            }

            automaticPurchaseOrder.Details = details;

            return automaticPurchaseOrder;
        }
    }
}
