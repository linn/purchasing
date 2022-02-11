﻿namespace Linn.Purchasing.Domain.LinnApps.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Layouts;
    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PurchaseLedger;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    public class PurchaseOrdersReportService : IPurchaseOrdersReportService
    {
        private readonly IQueryRepository<Part> partRepository;

        private readonly IRepository<PurchaseLedger, int> purchaseLedgerRepository;

        private readonly IRepository<PurchaseOrder, int> purchaseOrderRepository;

        private readonly IPurchaseOrdersPack purchaseOrdersPack;

        private readonly IReportingHelper reportingHelper;

        private readonly IRepository<Supplier, int> supplierRepository;

        public PurchaseOrdersReportService(
            IRepository<PurchaseOrder, int> purchaseOrderRepository,
            IRepository<Supplier, int> supplierRepository,
            IQueryRepository<Part> partRepository,
            IRepository<PurchaseLedger, int> purchaseLedgerRepository,
            IPurchaseOrdersPack purchaseOrdersPack,
            IReportingHelper reportingHelper)
        {
            this.purchaseOrderRepository = purchaseOrderRepository;
            this.supplierRepository = supplierRepository;
            this.partRepository = partRepository;
            this.purchaseLedgerRepository = purchaseLedgerRepository;
            this.purchaseOrdersPack = purchaseOrdersPack;
            this.reportingHelper = reportingHelper;
        }

        public ResultsModel GetOrdersByPartReport(DateTime from, DateTime to, string partNumber, bool includeCancelled)
        {
            var purchaseOrders = this.purchaseOrderRepository.FilterBy(
                x => x.Details.Any(z => z.PartNumber == partNumber) && from <= x.OrderDate && x.OrderDate < to);

            var reportLayout = new SimpleGridLayout(
                this.reportingHelper,
                CalculationValueModelType.Value,
                null,
                $"Purchase Orders By Part: {partNumber}");

            this.AddPartReportColumns(reportLayout);

            var values = new List<CalculationValueModel>();

            foreach (var order in purchaseOrders)
            {
                if (!includeCancelled && order.Cancelled == "Y")
                {
                    continue;
                }

                foreach (var orderDetail in order.Details.Where(d => d.PartNumber == partNumber))
                {
                    foreach (var delivery in orderDetail.PurchaseDeliveries)
                    {
                        if (!includeCancelled && (orderDetail.Cancelled == "Y" || delivery.Cancelled == "Y"))
                        {
                            continue;
                        }

                        this.ExtractDetailsForPartReport(values, order, orderDetail, delivery, order.CurrencyCode);
                    }
                }
            }

            reportLayout.SetGridData(values);
            var model = reportLayout.GetResultsModel();

            foreach ((var row, int i) in model.Rows.Select((value, i) => (value, i)))
            {
                model.SetGridValue(i, 5, model.GetGridValue(i, 5), decimalPlaces: 2);
                model.SetGridValue(i, 7, model.GetGridValue(i, 7), decimalPlaces: 2);
            }

            return model;
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
                     && (includeCredits == "Y" || (includeCredits == "N" && x.DocumentType != "CO")
                                               || (includeCredits == "O" && x.DocumentType == "CO")));

            var supplier = this.supplierRepository.FindById(supplierId);

            var reportLayout = new SimpleGridLayout(
                this.reportingHelper,
                CalculationValueModelType.Value,
                null,
                $"Purchase Orders By Supplier - {supplierId}: {supplier.Name}");

            this.AddSupplierReportColumns(reportLayout);

            var values = new List<CalculationValueModel>();

            foreach (var order in purchaseOrders)
            {
                if (!includeCancelled && order.Cancelled == "Y")
                {
                    continue;
                }

                foreach (var orderDetail in order.Details)
                {
                    if (outstandingOnly && this.purchaseOrdersPack.OrderIsCompleteSql(
                            orderDetail.OrderNumber,
                            orderDetail.Line))
                    {
                        continue;
                    }

                    foreach (var delivery in orderDetail.PurchaseDeliveries)
                    {
                        if (!includeCancelled && (orderDetail.Cancelled == "Y" || delivery.Cancelled == "Y"))
                        {
                            continue;
                        }

                        var part = this.partRepository.FindBy(x => x.PartNumber == orderDetail.PartNumber);
                        if (stockControlled != "A" && ((stockControlled == "N" && part.StockControlled != "N")
                                                       || (stockControlled == "O" && part.StockControlled != "Y")))
                        {
                            continue;
                        }

                        var ledgersForOrderAndLine = this.purchaseLedgerRepository.FilterBy(
                            pl => pl.OrderNumber == order.OrderNumber && pl.OrderLine == orderDetail.Line);

                        var ledgerQtys = ledgersForOrderAndLine.Select(
                            x => x.PlQuantity.HasValue
                                     ? new { TransType = x.TransactionType.DebitOrCredit, Qty = x.PlQuantity.Value }
                                     : null).ToList();

                        var totalLedgerQty = ledgerQtys.Sum(x => x.TransType == "C" ? x.Qty : -x.Qty);

                        this.ExtractSupplierReportDetails(values, orderDetail, delivery, totalLedgerQty, order.CurrencyCode);
                    }
                }
            }

            reportLayout.SetGridData(values);
            var model = reportLayout.GetResultsModel();

            foreach ((var row, int i) in model.Rows.Select((value, i) => (value, i)))
            {
                model.SetGridValue(i, 6, model.GetGridValue(i, 6), decimalPlaces: 2);
                model.SetGridValue(i, 8, model.GetGridValue(i, 8), decimalPlaces: 2);
            }

            return model;
        }

        private void AddSupplierReportColumns(SimpleGridLayout reportLayout)
        {
            reportLayout.AddColumnComponent(
                null,
                new List<AxisDetailsModel>
                    {
                        new AxisDetailsModel(
                            "OrderLine",
                            "Order/Line",
                            GridDisplayType.TextValue),
                        new AxisDetailsModel("PartNo", "Part Number", GridDisplayType.TextValue),
                        new AxisDetailsModel(
                            "SuppliersDesignation",
                            "Suppliers Designation",
                            GridDisplayType.TextValue),
                        new AxisDetailsModel("QtyOrd", "Qty Ordered", GridDisplayType.TextValue),
                        new AxisDetailsModel("QtyRec", "Qty Rec", GridDisplayType.TextValue),
                        new AxisDetailsModel("QtyInv", "Qty Inv", GridDisplayType.TextValue),
                        new AxisDetailsModel("BaseNetTotal", "Net Total (GBP)", GridDisplayType.Value),
                        new AxisDetailsModel("Currency", "Currency", GridDisplayType.TextValue),
                        new AxisDetailsModel("NetTotalCurrency", "Net Total (Currency)", GridDisplayType.Value),
                        new AxisDetailsModel("Delivery", "Delivery", GridDisplayType.TextValue),
                        new AxisDetailsModel("Qty", "Qty", GridDisplayType.TextValue),
                        new AxisDetailsModel("ReqDate", "Req Date", GridDisplayType.TextValue),
                        new AxisDetailsModel("AdvisedDate", "Advised Date", GridDisplayType.TextValue)
                    });
        }

        private void AddPartReportColumns(SimpleGridLayout reportLayout)
        {
            reportLayout.AddColumnComponent(
                null,
                new List<AxisDetailsModel>
                    {
                        new AxisDetailsModel(
                            "OrderLine",
                            "Order/Line",
                            GridDisplayType.TextValue),
                        new AxisDetailsModel("Date", "Date", GridDisplayType.TextValue),
                        new AxisDetailsModel(
                            "Supplier",
                            "Supplier",
                            GridDisplayType.TextValue),
                        new AxisDetailsModel("QtyOrd", "Qty Ordered", GridDisplayType.Value),
                        new AxisDetailsModel("QtyRec", "Qty Rec", GridDisplayType.Value),
                        new AxisDetailsModel("BaseNetTotal", "Net Total (GBP)", GridDisplayType.Value),
                        new AxisDetailsModel("Currency", "Currency", GridDisplayType.TextValue),
                        new AxisDetailsModel("NetTotalCurrency", "Net Total (Currency)", GridDisplayType.Value),
                        new AxisDetailsModel("Delivery", "Delivery", GridDisplayType.TextValue),
                        new AxisDetailsModel("Qty", "Qty", GridDisplayType.Value)
                    });
        }

        private void ExtractSupplierReportDetails(
            ICollection<CalculationValueModel> values,
            PurchaseOrderDetail orderDetail,
            PurchaseOrderDelivery delivery,
            decimal ledgerQty,
            string currencyCode)
        {
            var currentRowId = $"{orderDetail.OrderNumber}/{orderDetail.Line}";
            values.Add(
                new CalculationValueModel
                    {
                        RowId = currentRowId,
                        ColumnId = "OrderLine",
                        TextDisplay = $"{orderDetail.OrderNumber}/{orderDetail.Line}"
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
                        RowId = currentRowId, ColumnId = "QtyRec", TextDisplay = delivery.QtyNetReceived.ToString()
                    });

            values.Add(
                new CalculationValueModel
                    {
                        RowId = currentRowId, ColumnId = "QtyInv", TextDisplay = ledgerQty.ToString()
                    });

            values.Add(
                new CalculationValueModel
                    {
                        RowId = currentRowId,
                        ColumnId = "BaseNetTotal",
                        Value = orderDetail.BaseNetTotal,
                        CurrencyCode = "GBP"
                    });

            values.Add(
                new CalculationValueModel
                    {
                        RowId = currentRowId,
                        ColumnId = "Currency",
                        TextDisplay = currencyCode
                    });
            values.Add(
                new CalculationValueModel
                    {
                        RowId = currentRowId,
                        ColumnId = "NetTotalCurrency",
                        Value = orderDetail.NetTotalCurrency
                });

            values.Add(
                new CalculationValueModel
                    {
                        RowId = currentRowId, ColumnId = "Delivery", TextDisplay = $"{delivery.DeliverySeq}"
                    });

            values.Add(
                new CalculationValueModel
                    {
                        RowId = currentRowId, ColumnId = "Qty", TextDisplay = $"{delivery.OurDeliveryQty}"
                    });

            values.Add(
                new CalculationValueModel
                    {
                        RowId = currentRowId,
                        ColumnId = "ReqDate",
                        TextDisplay = delivery.DateRequested.ToString("dd-MMM-yyyy")
                    });

            values.Add(
                new CalculationValueModel
                    {
                        RowId = currentRowId,
                        ColumnId = "AdvisedDate",
                        TextDisplay = delivery.DateAdvised?.ToString("dd-MMM-yyyy")
                    });
        }

        private void ExtractDetailsForPartReport(
            ICollection<CalculationValueModel> values,
            PurchaseOrder order,
            PurchaseOrderDetail orderDetail,
            PurchaseOrderDelivery delivery,
            string currencyCode)
        {
            var currentRowId = $"{orderDetail.OrderNumber}/{orderDetail.Line}";
            values.Add(
                new CalculationValueModel
                    {
                        RowId = currentRowId,
                        ColumnId = "OrderLine",
                        TextDisplay = $"{orderDetail.OrderNumber}/{orderDetail.Line}"
                    });

            values.Add(
                new CalculationValueModel
                    {
                        RowId = currentRowId, ColumnId = "Date", TextDisplay = $"{order.OrderDate.ToString("dd-MMM-yyyy")}"
                    });

            values.Add(
                new CalculationValueModel
                    {
                        RowId = currentRowId,
                        ColumnId = "Supplier",
                        TextDisplay = $"{order.Supplier.SupplierId}: {order.Supplier.Name}"
                    });

            values.Add(
                new CalculationValueModel
                    {
                        RowId = currentRowId,
                        ColumnId = "QtyOrd",
                        Value = orderDetail.OurQty.HasValue ? orderDetail.OurQty.Value : 0
                    });

            values.Add(
                new CalculationValueModel
                    {
                        RowId = currentRowId, ColumnId = "QtyRec",
                        Value = delivery.QtyNetReceived
                    });

            values.Add(
                new CalculationValueModel
                    {
                        RowId = currentRowId,
                        ColumnId = "BaseNetTotal",
                        Value = orderDetail.BaseNetTotal
                    });

            values.Add(
                new CalculationValueModel
                    {
                        RowId = currentRowId,
                        ColumnId = "Currency",
                        TextDisplay = currencyCode
                    });

            values.Add(
                new CalculationValueModel
                    {
                        RowId = currentRowId,
                        ColumnId = "NetTotalCurrency",
                        Value = orderDetail.NetTotalCurrency
                    });

            values.Add(
                new CalculationValueModel
                    {
                        RowId = currentRowId, ColumnId = "Delivery", TextDisplay = $"{delivery.DeliverySeq}"
                    });

            values.Add(
                new CalculationValueModel
                    {
                        RowId = currentRowId, ColumnId = "Qty",
                        Value = delivery.OurDeliveryQty
                    });
        }
    }
}
