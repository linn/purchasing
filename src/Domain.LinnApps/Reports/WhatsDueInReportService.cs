namespace Linn.Purchasing.Domain.LinnApps.Reports
{
    using System;
    using System.Linq;
    using System.Xml.Linq;

    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.Keys;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;

    using MoreLinq;

    public class WhatsDueInReportService : IWhatsDueInReportService
    {
        private readonly IRepository<PurchaseOrder, int> purchaseOrderRepository;

        private readonly IRepository<PurchaseOrderDelivery, PurchaseOrderDeliveryKey> deliveryRepository;


        public WhatsDueInReportService(
            IRepository<PurchaseOrder, int> purchaseOrderRepository,
            IRepository<PurchaseOrderDelivery, PurchaseOrderDeliveryKey> deliveryRepository)
        {
            this.purchaseOrderRepository = purchaseOrderRepository;
            this.deliveryRepository = deliveryRepository;
        }

        public ResultsModel GetReport(
            DateTime fromDate, DateTime toDate, string orderBy, string vendorManager, int? supplier)
        {
            var deliveries = this.deliveryRepository.FilterBy(
                d => (d.DateAdvised.HasValue ? d.DateAdvised >= fromDate && d.DateAdvised < toDate : d.DateRequested >= fromDate && d.DateRequested < toDate)
                     && d.Cancelled == "N" 
                     && d.QuantityOutstanding > 0
                     && d.PurchaseOrderDetail.Part.StockControlled == "Y"
                     && d.PurchaseOrderDetail.PurchaseOrder.OrderMethod != "CALL OFF"
                     && d.PurchaseOrderDetail.PurchaseOrder.Cancelled == "N"
                     && d.PurchaseOrderDetail.Cancelled == "N"
                     & new string[2] { "RO", "PO" }.Contains(
                         d.PurchaseOrderDetail.PurchaseOrder.DocumentType)).ToList().OrderBy(x => x.OrderNumber);


            return new ResultsModel();
        }
    }
}
