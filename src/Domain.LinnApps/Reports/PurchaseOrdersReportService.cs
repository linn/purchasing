namespace Linn.Purchasing.Domain.LinnApps.Reports
{
    using System;
    using System.Collections.Generic;

    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Layouts;
    using Linn.Common.Reporting.Models;

    public class PurchaseOrdersReportService : IPurchaseOrdersReportService
    {
        private readonly IRepository<PurchaseOrder, int> purchaseOrderRepository;

        private readonly IReportingHelper reportingHelper;

        public PurchaseOrdersReportService(
            IRepository<PurchaseOrder, int> purchaseOrderRepository,
            IReportingHelper reportingHelper)
        {
            this.purchaseOrderRepository = purchaseOrderRepository;
            this.reportingHelper = reportingHelper;
        }

        public ResultsModel GetOrdersByPartReport(DateTime from, DateTime to, string partNumber)
        {
            throw new NotImplementedException();
        }

        public ResultsModel GetOrdersBySupplierReport(DateTime from, DateTime to, int supplierId)
        {
            var purchaseOrders = this.purchaseOrderRepository.FilterBy(x => x.SupplierId == supplierId);

            var reportLayout = new SimpleGridLayout(
                this.reportingHelper,
                CalculationValueModelType.TextValue,
                null,
                "Purchase Orders By Supplier");

            this.AddReportColumns(reportLayout);

            var values = new List<CalculationValueModel>();

            foreach (var order in purchaseOrders)
            {
                foreach (var orderDetail in order.Details)
                {
                    this.ExtractDetails(values, order, orderDetail);
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
            PurchaseOrderDetail orderDetail)
        {
            var currentRowId = $"{purchaseOrder.OrderNumber}/{orderDetail.Line}";
            values.Add(
                new CalculationValueModel
                    {
                        RowId = currentRowId,
                        ColumnId = "OrderLine",
                        TextDisplay = $"{purchaseOrder.OrderNumber}/{orderDetail.Line}",
                        RowTitle = purchaseOrder.OrderNumber.ToString()
                    });

            values.Add(
                new CalculationValueModel
                    {
                        RowId = currentRowId,
                        ColumnId = "PartNo",
                        TextDisplay = $"{orderDetail.PartNumber}",
                        RowTitle = purchaseOrder.OrderNumber.ToString()
                    });

            values.Add(
                new CalculationValueModel
                    {
                        RowId = currentRowId,
                        ColumnId = "SuppliersDesignation",
                        TextDisplay = $"{orderDetail.SuppliersDesignation}",
                        RowTitle = purchaseOrder.OrderNumber.ToString()
                    });

            values.Add(
                new CalculationValueModel
                    {
                        RowId = currentRowId,
                        ColumnId = "QtyOrd",
                        TextDisplay = "todo",
                        RowTitle = purchaseOrder.OrderNumber.ToString()
                    });

            values.Add(
                new CalculationValueModel
                    {
                        RowId = currentRowId,
                        ColumnId = "QtyRec",
                        TextDisplay = "todo",
                        RowTitle = purchaseOrder.OrderNumber.ToString()
                    });

            values.Add(
                new CalculationValueModel
                    {
                        RowId = currentRowId,
                        ColumnId = "QtyInv",
                        TextDisplay = "todo",
                        RowTitle = purchaseOrder.OrderNumber.ToString()
                    });

            values.Add(
                new CalculationValueModel
                    {
                        RowId = currentRowId,
                        ColumnId = "NetTotal",
                        TextDisplay = $"{orderDetail.NetTotal}",
                        RowTitle = purchaseOrder.OrderNumber.ToString()
                    });

            values.Add(
                new CalculationValueModel
                    {
                        RowId = currentRowId,
                        ColumnId = "Qty",
                        TextDisplay = "todo",
                        RowTitle = purchaseOrder.OrderNumber.ToString()
                    });

            values.Add(
                new CalculationValueModel
                    {
                        RowId = currentRowId,
                        ColumnId = "ReqDate",
                        TextDisplay = "todo",
                        RowTitle = purchaseOrder.OrderNumber.ToString()
                    });

            values.Add(
                new CalculationValueModel
                    {
                        RowId = currentRowId,
                        ColumnId = "AdvisedDate",
                        TextDisplay = "todo",
                        RowTitle = purchaseOrder.OrderNumber.ToString()
                    });
        }
    }
}
