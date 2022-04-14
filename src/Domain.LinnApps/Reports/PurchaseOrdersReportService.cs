namespace Linn.Purchasing.Domain.LinnApps.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Layouts;
    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PurchaseLedger;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Reports.Models;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    public class PurchaseOrdersReportService : IPurchaseOrdersReportService
    {
        private readonly IQueryRepository<Part> partRepository;

        private readonly IRepository<PurchaseLedger, int> purchaseLedgerRepository;

        private readonly IRepository<PurchaseOrder, int> purchaseOrderRepository;

        private readonly IPurchaseOrdersPack purchaseOrdersPack;

        private readonly IReportingHelper reportingHelper;

        private readonly IQueryRepository<SuppliersWithUnacknowledgedOrders> suppliersWithUnacknowledgedOrdersRepository;

        private readonly IQueryRepository<SupplierGroupsWithUnacknowledgedOrders> supplierGroupsWithUnacknowledgedOrdersRepository;

        private readonly IQueryRepository<UnacknowledgedOrders> unacknowledgedOrdersRepository;

        private readonly IRepository<SupplierGroup, int> supplierGroupRepository;

        private readonly IRepository<Supplier, int> supplierRepository;

        public PurchaseOrdersReportService(
            IRepository<PurchaseOrder, int> purchaseOrderRepository,
            IRepository<Supplier, int> supplierRepository,
            IQueryRepository<Part> partRepository,
            IRepository<PurchaseLedger, int> purchaseLedgerRepository,
            IPurchaseOrdersPack purchaseOrdersPack,
            IReportingHelper reportingHelper,
            IQueryRepository<SuppliersWithUnacknowledgedOrders> suppliersWithUnacknowledgedOrdersRepository,
            IQueryRepository<SupplierGroupsWithUnacknowledgedOrders> supplierGroupsWithUnacknowledgedOrdersRepository,
            IQueryRepository<UnacknowledgedOrders> unacknowledgedOrdersRepository,
            IRepository<SupplierGroup, int> supplierGroupRepository)
        {
            this.purchaseOrderRepository = purchaseOrderRepository;
            this.supplierRepository = supplierRepository;
            this.partRepository = partRepository;
            this.purchaseLedgerRepository = purchaseLedgerRepository;
            this.purchaseOrdersPack = purchaseOrdersPack;
            this.reportingHelper = reportingHelper;
            this.suppliersWithUnacknowledgedOrdersRepository = suppliersWithUnacknowledgedOrdersRepository;
            this.supplierGroupsWithUnacknowledgedOrdersRepository = supplierGroupsWithUnacknowledgedOrdersRepository;
            this.unacknowledgedOrdersRepository = unacknowledgedOrdersRepository;
            this.supplierGroupRepository = supplierGroupRepository;
        }

        public ResultsModel GetOrdersByPartReport(DateTime from, DateTime to, string partNumber, bool includeCancelled)
        {
            var purchaseOrders = this.purchaseOrderRepository.FilterBy(
                x => x.Details.Any(z => z.Part.PartNumber == partNumber) && from <= x.OrderDate && x.OrderDate < to);

            var reportLayout = new SimpleGridLayout(
                this.reportingHelper,
                CalculationValueModelType.Value,
                null,
                $"Purchase Orders By Part: {partNumber}");

            AddPartReportColumns(reportLayout);

            var values = new List<CalculationValueModel>();

            foreach (var order in purchaseOrders)
            {
                if (!includeCancelled && order.Cancelled == "Y")
                {
                    continue;
                }

                foreach (var orderDetail in order.Details.Where(d => d.Part.PartNumber == partNumber))
                {
                    foreach (var delivery in orderDetail.PurchaseDeliveries)
                    {
                        if (!includeCancelled && (orderDetail.Cancelled == "Y" || delivery.Cancelled == "Y"))
                        {
                            continue;
                        }

                        ExtractDetailsForPartReport(values, order, orderDetail, delivery, order.Currency.Code);
                    }
                }
            }

            reportLayout.SetGridData(values);
            var model = reportLayout.GetResultsModel();

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
                     && (includeReturns || x.DocumentTypeName != "RO")
                     && (includeCredits == "Y" || (includeCredits == "N" && x.DocumentTypeName != "CO")
                                               || (includeCredits == "O" && x.DocumentTypeName == "CO")));

            var supplier = this.supplierRepository.FindById(supplierId);

            var reportLayout = new SimpleGridLayout(
                this.reportingHelper,
                CalculationValueModelType.Value,
                null,
                $"Purchase Orders By Supplier - {supplierId}: {supplier.Name}");

            AddSupplierReportColumns(reportLayout);

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

                        var part = this.partRepository.FindBy(x => x.PartNumber == orderDetail.Part.PartNumber);
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

                        ExtractSupplierReportDetails(values, orderDetail, delivery, totalLedgerQty, order.Currency.Code);
                    }
                }
            }

            reportLayout.SetGridData(values);
            var model = reportLayout.GetResultsModel();

            return model;
        }

        public ResultsModel GetSuppliersWithUnacknowledgedOrders(
            int? planner,
            string vendorManager,
            bool useSupplierGroup)
        {
            var results = new ResultsModel(new string[] { "Id", "Name" })
                              {
                                  ReportTitle = new NameModel("Suppliers with unacknowledged orders")
                              };
            results.AddColumn("supplierOrGroup", string.Empty, GridDisplayType.TextValue);
            results.AddColumn("view", string.Empty, GridDisplayType.TextValue);
            results.AddColumn("csv", string.Empty, GridDisplayType.TextValue);

            var supplierResults = useSupplierGroup
                                      ? this.GetResultsBySupplierGroup(vendorManager, planner)
                                      : this.GetResultsBySupplier(vendorManager, planner);

            this.reportingHelper.AddResultsToModel(results, supplierResults, CalculationValueModelType.TextValue, true);
            foreach (var row in results.Rows)
            {
                var href = results.GetGridTextValue(row.RowIndex, 2) == string.Empty
                               ? $"/purchasing/reports/unacknowledged-orders?supplierId={row.RowId}&name={results.GetGridTextValue(row.RowIndex, 1)}"
                               : $"/purchasing/reports/unacknowledged-orders?supplierGroupId={row.RowId}&name={results.GetGridTextValue(row.RowIndex, 1)}";
                results.ValueDrillDownTemplates.Add(new DrillDownModel("view", href, row.RowIndex, 3));

                var exportHref = results.GetGridTextValue(row.RowIndex, 2) == string.Empty
                               ? $"/purchasing/reports/unacknowledged-orders/export?supplierId={row.RowId}&name={results.GetGridTextValue(row.RowIndex, 1)}"
                               : $"/purchasing/reports/unacknowledged-orders/export?supplierGroupId={row.RowId}&name={results.GetGridTextValue(row.RowIndex, 1)}";
                results.ValueDrillDownTemplates.Add(
                    new DrillDownModel(
                        "csv",
                        exportHref,
                        row.RowIndex,
                        4,
                        true));
            }

            this.reportingHelper.SortRowsByTextColumnValues(results, 1);
            return results;
        }

        public ResultsModel GetUnacknowledgedOrders(int? supplierId, int? supplierGroupId)
        {
            IQueryable<UnacknowledgedOrders> orders;
            string title;
            if (!supplierId.HasValue && !supplierGroupId.HasValue)
            {
                title = "All unacknowledged orders";
                orders = this.unacknowledgedOrdersRepository.FindAll();
            }
            else if (supplierId.HasValue)
            {
                var supplier = this.supplierRepository.FindById(supplierId.Value);
                title = $"Unacknowledged orders for {supplier.Name}";
                orders = this.unacknowledgedOrdersRepository.FilterBy(
                    a => a.SupplierId == supplierId);
            }
            else
            {
                var supplierGroup = this.supplierGroupRepository.FindById(supplierGroupId.Value);

                title = $"Unacknowledged orders for {supplierGroup.Name}";
                orders = this.unacknowledgedOrdersRepository.FilterBy(
                    a => a.SupplierGroupId == supplierGroupId);
            }

            var results = new ResultsModel
                              {
                                  ReportTitle = new NameModel(title),
                                  RowHeader = "Order Number"
                              };
            var columns = new List<AxisDetailsModel>
                              {
                                  new AxisDetailsModel("Part Number", GridDisplayType.TextValue) { SortOrder = 0, AllowWrap = false },
                                  new AxisDetailsModel("Description", GridDisplayType.TextValue) { SortOrder = 1 },
                                  new AxisDetailsModel("Order Line", GridDisplayType.Value) { SortOrder = 2, DecimalPlaces = 0 },
                                  new AxisDetailsModel("Delivery No", GridDisplayType.Value) { SortOrder = 3, DecimalPlaces = 0 },
                                  new AxisDetailsModel("Qty", GridDisplayType.Value) { SortOrder = 4 },
                                  new AxisDetailsModel("Currency Code", GridDisplayType.TextValue) { SortOrder = 5 },
                                  new AxisDetailsModel("Unit Price", GridDisplayType.TextValue) { SortOrder = 6 },
                                  new AxisDetailsModel("Requested Delivery", GridDisplayType.TextValue) { SortOrder = 7, AllowWrap = false }
                              };
            results.AddSortedColumns(columns);

            var models = new List<CalculationValueModel>();
            foreach (var order in orders)
            {
                var rowId = $"{order.OrderNumber}/{order.OrderLine}/{order.DeliveryNumber}";
                models.Add(new CalculationValueModel
                               {
                                   RowId = rowId,
                                   ColumnId = "Part Number",
                                   TextDisplay = order.PartNumber,
                                   RowTitle = $"{order.OrderNumber}"
                               });
                models.Add(new CalculationValueModel
                               {
                                   RowId = rowId, ColumnId = "Description",
                                   TextDisplay = order.SuppliersDesignation,
                                   RowTitle = $"{order.OrderNumber}"
                               });
                models.Add(new CalculationValueModel
                               {
                                   RowId = rowId,
                                   ColumnId = "Order Line",
                                   Value = order.OrderLine,
                                   RowTitle = $"{order.OrderNumber}"
                               });
                models.Add(new CalculationValueModel
                               {
                                   RowId = rowId, ColumnId = "Delivery No",
                                   Value = order.DeliveryNumber,
                                   RowTitle = $"{order.OrderNumber}"
                               });
                models.Add(new CalculationValueModel
                               {
                                   RowId = rowId, ColumnId = "Qty",
                                   Value = order.OrderDeliveryQuantity,
                                   RowTitle = $"{order.OrderNumber}"
                               });
                models.Add(new CalculationValueModel
                               {
                                   RowId = rowId,
                                   ColumnId = "Currency Code",
                                   TextDisplay = order.CurrencyCode,
                                   RowTitle = $"{order.OrderNumber}"
                               });
                models.Add(new CalculationValueModel
                               {
                                   RowId = rowId, ColumnId = "Unit Price",
                                   TextDisplay = order.OrderUnitPrice.ToString("###,###,###,##0.00###"),
                                   RowTitle = $"{order.OrderNumber}"
                               });
                models.Add(new CalculationValueModel
                               {
                                   RowId = rowId, ColumnId = "Requested Delivery",
                                   TextDisplay = order.RequestedDate.ToString("dd-MMM-yyyy"),
                                   RowTitle = $"{order.OrderNumber}"
                               });
            }

            this.reportingHelper.AddResultsToModel(results, models, CalculationValueModelType.Value, true);
            this.reportingHelper.SortRowsByTextColumnValues(results, 0, 3);
            return results;
        }

        private static void AddSupplierReportColumns(SimpleGridLayout reportLayout)
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
                        new AxisDetailsModel("BaseNetTotal", "Net Total (GBP)", GridDisplayType.Value) { DecimalPlaces = 2 },
                        new AxisDetailsModel("Currency", "Currency", GridDisplayType.TextValue),
                        new AxisDetailsModel("NetTotalCurrency", "Net Total (Currency)", GridDisplayType.Value) { DecimalPlaces = 2 },
                        new AxisDetailsModel("Delivery", "Delivery", GridDisplayType.TextValue),
                        new AxisDetailsModel("Qty", "Qty", GridDisplayType.TextValue),
                        new AxisDetailsModel("ReqDate", "Req Date", GridDisplayType.TextValue),
                        new AxisDetailsModel("AdvisedDate", "Advised Date", GridDisplayType.TextValue)
                    });
        }

        private static void AddPartReportColumns(SimpleGridLayout reportLayout)
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
                        new AxisDetailsModel("BaseNetTotal", "Net Total (GBP)", GridDisplayType.Value) { DecimalPlaces = 2 },
                        new AxisDetailsModel("Currency", "Currency", GridDisplayType.TextValue),
                        new AxisDetailsModel("NetTotalCurrency", "Net Total (Currency)", GridDisplayType.Value) { DecimalPlaces = 2 },
                        new AxisDetailsModel("Delivery", "Delivery", GridDisplayType.TextValue),
                        new AxisDetailsModel("Qty", "Qty", GridDisplayType.Value)
                    });
        }

        private static void ExtractSupplierReportDetails(
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
                        RowId = currentRowId, ColumnId = "PartNo", TextDisplay = $"{orderDetail.Part.PartNumber}"
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
                        RowId = currentRowId, ColumnId = "QtyInv", TextDisplay = ledgerQty.ToString(CultureInfo.InvariantCulture)
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
                        TextDisplay = delivery.DateRequested?.ToString("dd-MMM-yyyy")
                    });

            values.Add(
                new CalculationValueModel
                    {
                        RowId = currentRowId,
                        ColumnId = "AdvisedDate",
                        TextDisplay = delivery.DateAdvised?.ToString("dd-MMM-yyyy")
                    });
        }

        private static void ExtractDetailsForPartReport(
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
                        RowId = currentRowId, ColumnId = "Date", TextDisplay = $"{order.OrderDate:dd-MMM-yyyy}"
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
                        Value = delivery.QtyNetReceived ?? 0
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
                        Value = delivery.OurDeliveryQty ?? 0
                    });
        }

        private List<CalculationValueModel> GetResultsBySupplier(string vendorManager, int? planner)
        {
            var suppliers = this.suppliersWithUnacknowledgedOrdersRepository.FindAll();
            if (planner.HasValue)
            {
                suppliers = suppliers.Where(a => a.Planner == planner);
            }

            if (!string.IsNullOrEmpty(vendorManager))
            {
                suppliers = suppliers.Where(a => a.VendorManager == vendorManager);
            }

            var supplierResults = new List<CalculationValueModel>();
            foreach (var supplier in suppliers)
            {
                var rowId = supplier.SupplierId.ToString();
                supplierResults.Add(new CalculationValueModel
                                        {
                                            RowId = rowId,
                                            ColumnId = "Id",
                                            TextDisplay = supplier.SupplierId.ToString()
                                        });
                supplierResults.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            ColumnId = "Name",
                            TextDisplay = supplier.SupplierName
                        });
                supplierResults.Add(new CalculationValueModel { RowId = rowId, ColumnId = "view", TextDisplay = "view" });
                supplierResults.Add(new CalculationValueModel { RowId = rowId, ColumnId = "csv", TextDisplay = "csv" });
                supplierResults.Add(new CalculationValueModel { RowId = rowId, ColumnId = "supplierOrGroup", TextDisplay = string.Empty });
            }

            return supplierResults;
        }

        private List<CalculationValueModel> GetResultsBySupplierGroup(string vendorManager, int? planner)
        {
            var suppliers = this.supplierGroupsWithUnacknowledgedOrdersRepository.FindAll();
            if (planner.HasValue)
            {
                suppliers = suppliers.Where(a => a.Planner == planner);
            }

            if (!string.IsNullOrEmpty(vendorManager))
            {
                suppliers = suppliers.Where(a => a.VendorManager == vendorManager);
            }

            var supplierResults = new List<CalculationValueModel>();
            foreach (var supplier in suppliers)
            {
                var rowId = supplier.Id.ToString();
                supplierResults.Add(new CalculationValueModel
                                        {
                                            RowId = rowId,
                                            ColumnId = "Id",
                                            TextDisplay = supplier.Id.ToString()
                                        });
                supplierResults.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            ColumnId = "Name",
                            TextDisplay = supplier.Name
                        });
                supplierResults.Add(new CalculationValueModel { RowId = rowId, ColumnId = "view", TextDisplay = "view" });
                supplierResults.Add(new CalculationValueModel { RowId = rowId, ColumnId = "csv", TextDisplay = "csv" });
                supplierResults.Add(new CalculationValueModel { RowId = rowId, ColumnId = "supplierOrGroup", TextDisplay = supplier.SupplierGroupId.HasValue  ? "(Group)" : string.Empty });
            }

            return supplierResults;
        }
    }
}
