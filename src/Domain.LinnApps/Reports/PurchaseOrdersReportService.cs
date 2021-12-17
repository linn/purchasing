namespace Linn.Purchasing.Domain.LinnApps.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Layouts;
    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.PurchaseLedger;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    public class PurchaseOrdersReportService : IPurchaseOrdersReportService
    {
        private readonly IRepository<PurchaseLedger, int> purchaseLedgerRepository;

        private readonly IRepository<PurchaseOrder, int> purchaseOrderRepository;

        private readonly IReportingHelper reportingHelper;

        private readonly IRepository<Supplier, int> supplierRepository;

        public PurchaseOrdersReportService(
            IRepository<PurchaseOrder, int> purchaseOrderRepository,
            IRepository<Supplier, int> supplierRepository,
            IRepository<PurchaseLedger, int> purchaseLedgerRepository,
            IReportingHelper reportingHelper)
        {
            this.purchaseOrderRepository = purchaseOrderRepository;
            this.supplierRepository = supplierRepository;
            this.purchaseLedgerRepository = purchaseLedgerRepository;
            this.reportingHelper = reportingHelper;
        }

        public ResultsModel GetOrdersByPartReport(DateTime from, DateTime to, string partNumber)
        {
            throw new NotImplementedException();
        }

        public ResultsModel GetOrdersBySupplierReport(
            DateTime from,
            DateTime to,
            int supplierId,
            bool includeReturns,
            bool outstandingOnly,
            bool includeCancelled,
            string includeCredits,
            string stockControlled)
        {
            var purchaseOrders = this.purchaseOrderRepository.FilterBy(
                x => x.SupplierId == supplierId && from <= x.OrderDate && x.OrderDate < to
                     && (includeReturns || x.DocumentType != "RO")
                     && (includeCredits == "Y" 
                            || (includeCredits == "N" && x.DocumentType != "CO")
                            || (includeCredits == "O" && x.DocumentType == "CO")));

            // returns Y/N : document type != "RO
            // credit document type CO or ! CO       yes / no / only
            // stock controller: pl_orders_pack.part_is_stock_controlled_sql(part number)

            // all / outstanding: pl_orders_pack.order_s_complete_sql(order number, order line)
            // && (!outstandingOnly || x.)

            // cancelled: Y/N plorh (pl orders), plorl(pl order details) or pl deliveries (plco) cancelled = 'N' (????) or plorh.archive_order = 'Y'

            // && (includeCancelled || (x.Cancelled = "N" && !x.Details.Any(z => z.Cancelled = 'Y' && (!x.Details.Any(z => z.PurchaseDelivery.Cancelled == 'Y') x.ArchiveOrder))
            // part of above should probs go in foreach orderDetails
            var supplier = this.supplierRepository.FindById(supplierId);

            var reportLayout = new SimpleGridLayout(
                this.reportingHelper,
                CalculationValueModelType.TextValue,
                null,
                $"Purchase Orders By Supplier - {supplierId}: {supplier.Name}");

            this.AddReportColumns(reportLayout);

            var values = new List<CalculationValueModel>();

            foreach (var order in purchaseOrders)
            {
                foreach (var orderDetail in order.Details)
                {
                    var ledgersForOrderAndLine = this.purchaseLedgerRepository.FilterBy(
                        pl => pl.OrderNumber == order.OrderNumber && pl.OrderLine == orderDetail.Line);

                    var ledgerQtys = ledgersForOrderAndLine.Select(
                        x => x.PlQuantity.HasValue
                                 ? new { TransType = x.TransactionType.DebitOrCredit, Qty = x.PlQuantity.Value }
                                 : null).ToList();

                    var totalLedgerQty = ledgerQtys.Sum(x => x.TransType == "C" ? x.Qty : -x.Qty);

                    this.ExtractDetails(values, order, orderDetail, totalLedgerQty);
                }
            }

            reportLayout.SetGridData(values);
            var model = reportLayout.GetResultsModel();

            return model;
        }

        private void AddReportColumns(SimpleGridLayout reportLayout)
        {
            reportLayout.AddColumnComponent(
                null,
                new List<AxisDetailsModel>
                    {
                        new AxisDetailsModel(
                            "OrderLine",
                            "Order/Line",
                            GridDisplayType.TextValue) {
                                                          AllowWrap = false 
                                                       },
                        new AxisDetailsModel("PartNo", "Part Number", GridDisplayType.TextValue),
                        new AxisDetailsModel(
                            "SuppliersDesignation",
                            "Suppliers Designation",
                            GridDisplayType.TextValue),
                        new AxisDetailsModel("QtyOrd", "Qty Ordered", GridDisplayType.TextValue),
                        new AxisDetailsModel("QtyRec", "Qty Rec", GridDisplayType.TextValue),
                        new AxisDetailsModel("QtyInv", "Qty Inv", GridDisplayType.TextValue),
                        new AxisDetailsModel("NetTotal", "Net Total", GridDisplayType.TextValue),
                        new AxisDetailsModel("Delivery", "Delivery", GridDisplayType.TextValue),
                        new AxisDetailsModel("Qty", "Qty", GridDisplayType.TextValue),
                        new AxisDetailsModel("ReqDate", "Req Date", GridDisplayType.TextValue),
                        new AxisDetailsModel("AdvisedDate", "Advised Date", GridDisplayType.TextValue)
                    });
        }

        private void ExtractDetails(
            ICollection<CalculationValueModel> values,
            PurchaseOrder purchaseOrder,
            PurchaseOrderDetail orderDetail,
            decimal ledgerQty)
        {
            var currentRowId = $"{purchaseOrder.OrderNumber}/{orderDetail.Line}";
            values.Add(
                new CalculationValueModel
                    {
                        RowId = currentRowId,
                        ColumnId = "OrderLine",
                        TextDisplay = $"{purchaseOrder.OrderNumber}/{orderDetail.Line}"
                    });

            values.Add(
                new CalculationValueModel
                    {
                        RowId = currentRowId, ColumnId = "PartNo", TextDisplay = $"{orderDetail.PartNumber}"
                    });

            values.Add(
                new CalculationValueModel
                    {
                        RowId = currentRowId,
                        ColumnId = "SuppliersDesignation",
                        TextDisplay = $"{orderDetail.SuppliersDesignation}"
                    });

            values.Add(
                new CalculationValueModel
                    {
                        RowId = currentRowId,
                        ColumnId = "QtyOrd",
                        TextDisplay = orderDetail.OurQty.HasValue ? orderDetail.OurQty.Value.ToString() : "0"
                    });

            values.Add(
                new CalculationValueModel
                    {
                        RowId = currentRowId,
                        ColumnId = "QtyRec",
                        TextDisplay = orderDetail.PurchaseDelivery.QtyNetReceived.ToString()
                    });

            values.Add(
                new CalculationValueModel
                    {
                        RowId = currentRowId, ColumnId = "QtyInv", TextDisplay = ledgerQty.ToString()
                    });

            values.Add(
                new CalculationValueModel
                    {
                        RowId = currentRowId, ColumnId = "NetTotal", TextDisplay = $"{orderDetail.NetTotal}"
                    });

            values.Add(
                new CalculationValueModel
                    {
                        RowId = currentRowId,
                        ColumnId = "Delivery",
                        TextDisplay = $"{orderDetail.PurchaseDelivery.DeliverySeq}"
                    });

            values.Add(
                new CalculationValueModel
                    {
                        RowId = currentRowId,
                        ColumnId = "Qty",
                        TextDisplay = $"{orderDetail.PurchaseDelivery.OurDeliveryQty}"
                    });

            values.Add(
                new CalculationValueModel
                    {
                        RowId = currentRowId,
                        ColumnId = "ReqDate",
                        TextDisplay = orderDetail.PurchaseDelivery.DateRequested.ToString("dd-MMM-yyyy")
                    });

            values.Add(
                new CalculationValueModel
                    {
                        RowId = currentRowId,
                        ColumnId = "AdvisedDate",
                        TextDisplay = orderDetail.PurchaseDelivery.DateAdvised.ToString("dd-MMM-yyyy")
                    });
        }
    }
}
