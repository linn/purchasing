namespace Linn.Purchasing.Domain.LinnApps.Reports
{
    using System;
    using System.Collections.Generic;

    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Layouts;
    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.PurchaseLedger;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    public class PurchaseOrdersReportService : IPurchaseOrdersReportService
    {
        private readonly IRepository<PurchaseOrder, int> purchaseOrderRepository;
        private readonly IRepository<Supplier, int> supplierRepository;
        private readonly IRepository<PurchaseLedger, int> purchaseLedgerRepository;


        private readonly IReportingHelper reportingHelper;

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

        public ResultsModel GetOrdersBySupplierReport(DateTime from, DateTime to, int supplierId)
        {
            var purchaseOrders = this.purchaseOrderRepository.FilterBy(x => x.SupplierId == supplierId
                                                                            && from <= x.OrderDate && x.OrderDate < to);

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
                var ledger = this.purchaseLedgerRepository.FindById(order.OrderNumber);
                var ledgerQty = ledger.PlQuantity.HasValue ? (int)ledger.PlQuantity.Value : 0;
                foreach (var orderDetail in order.Details)
                {
                    this.ExtractDetails(values, order, orderDetail, ledgerQty);
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
            int ledgerQty)
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
                        RowId = currentRowId,
                        ColumnId = "PartNo",
                        TextDisplay = $"{orderDetail.PartNumber}"
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
                        TextDisplay = orderDetail.PurchaseDelivery.OrderDeliveryQty.ToString()
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
                        RowId = currentRowId,
                        ColumnId = "QtyInv",
                        TextDisplay = ledgerQty.ToString()
                });
            //^ I think looks to be calculated with:
            //vqty_number as number;
            //select sum(pl_pack.get_payment_value(pl_trans_type, pl_qty)) 
            //into vqty
            //from purchase_ledger
            //where order_number = :order_number
            //and order_line = :order_line
            //return nvl(vqty,0)

            //Function Get_Payment_Value(p_trans in varchar2, p_value in number) return number is
            //    begin
            //if trans_type_is_credit(p_trans) then
            //return p_value;
            //else
            //return p_value * -1;
            //end if;
            //end get_payment_value;


            //so long way of saying if row's there in purchase ledger, value is pl_qty
            //so add a pl ledger repo

            values.Add(
                new CalculationValueModel
                    {
                        RowId = currentRowId,
                        ColumnId = "NetTotal",
                        TextDisplay = $"{orderDetail.NetTotal}"
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
