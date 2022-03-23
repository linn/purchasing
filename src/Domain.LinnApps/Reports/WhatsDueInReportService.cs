namespace Linn.Purchasing.Domain.LinnApps.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Layouts;
    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.Keys;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Reports.Models;

    using MoreLinq;

    public class WhatsDueInReportService : IWhatsDueInReportService
    {
        private readonly IRepository<PurchaseOrderDelivery, PurchaseOrderDeliveryKey> deliveryRepository;

        private readonly IReportingHelper reportingHelper;

        public WhatsDueInReportService(
            IReportingHelper reportingHelper,
            IRepository<PurchaseOrderDelivery, PurchaseOrderDeliveryKey> deliveryRepository)
        {
            this.deliveryRepository = deliveryRepository;
            this.reportingHelper = reportingHelper;
        }

        public ResultsModel GetReport(
            DateTime fromDate, DateTime toDate, string orderBy, string vendorManager, int? supplier)
        {
            var result = this.deliveryRepository.FilterBy(
                d => (d.DateAdvised.HasValue 
                          ? d.DateAdvised >= fromDate && d.DateAdvised < toDate 
                          : d.DateRequested >= fromDate && d.DateRequested < toDate)
                     && d.Cancelled == "N" 
                     && d.QuantityOutstanding > 0
                     && d.PurchaseOrderDetail.Part.StockControlled == "Y"
                     && d.PurchaseOrderDetail.PurchaseOrder.OrderMethod != "CALL OFF"
                     && d.PurchaseOrderDetail.PurchaseOrder.Cancelled == "N"
                     && d.PurchaseOrderDetail.Cancelled == "N"
                     & new string[] { "RO", "PO" }.Contains(
                         d.PurchaseOrderDetail.PurchaseOrder.DocumentType));

            if (!string.IsNullOrEmpty(vendorManager))
            {
                result = result.Where(x => x.PurchaseOrderDetail.PurchaseOrder.Supplier.VendorManager.Id == vendorManager);
            }

            var data = result.Select(d => new WhatsDueInEntryModel
                            {
                                SupplierId = d.PurchaseOrderDetail.PurchaseOrder.SupplierId,
                                PartNumber = d.PurchaseOrderDetail.Part.PartNumber,
                                AdvisedDate = d.DateAdvised,
                                CallOffDate = d.CallOffDate,
                                DeliverySequence = d.DeliverySeq,
                                DocumentType = d.PurchaseOrderDetail.PurchaseOrder.DocumentType,
                                ExpectedDate = d.DateAdvised ?? d.DateRequested,
                                OrderLine = d.OrderLine,
                                OrderNumber = d.OrderNumber,
                                UnitPrice = d.BaseOurUnitPrice,
                                OurDeliveryQty = d.OurDeliveryQty,
                                QtyOutstanding = d.QuantityOutstanding,
                                RequestedDate = d.DateRequested,
                                StockPoolCode = d.PurchaseOrderDetail.StockPoolCode,
                                SupplierName = d.PurchaseOrderDetail.PurchaseOrder.Supplier.Name
                            });

            if (supplier.HasValue)
            {
                data = data.Where(x => x.SupplierId == supplier);
            }

            data = orderBy switch
            {
                "PART" => data.OrderBy(x => x.PartNumber),
                "EXPECTED DATE" => data.OrderBy(x => x.ExpectedDate),
                "VALUE" => data.OrderByDescending(x => x.QtyOutstanding * x.UnitPrice),
                "SUPPLIER" => data.OrderBy(x => x.SupplierName),
                _ => data.OrderBy(x => x.OrderNumber)
            };

            var reportLayout = new SimpleGridLayout(
                this.reportingHelper,
                CalculationValueModelType.Value,
                null,
                $"Stock controlled parts due in between {fromDate.ToShortDateString()} and {toDate.ToShortDateString()}");

            reportLayout.AddColumnComponent(
                null,
                new List<AxisDetailsModel>
                    {
                        new AxisDetailsModel("OrderNumber", "Order",  GridDisplayType.TextValue),
                        new AxisDetailsModel("OrderLine", "Order Line",  GridDisplayType.Value) { DecimalPlaces = 0 },
                        new AxisDetailsModel("DeliverySequence", "Del", GridDisplayType.Value) { DecimalPlaces = 0 },
                        new AxisDetailsModel("SupplierName", "Supplier", GridDisplayType.TextValue),
                        new AxisDetailsModel("ExpectedDate", "Expected", GridDisplayType.TextValue),
                        new AxisDetailsModel("QtyOutstanding", "Qty", GridDisplayType.Value) { DecimalPlaces = 0 },
                        new AxisDetailsModel("TotalPrice", "Total Price", GridDisplayType.Value) { DecimalPlaces = 2 }
                    });

            var values = new List<CalculationValueModel>();
            foreach (var datum in data)
            {
                var currentRowId = $"{datum.OrderNumber + datum.OrderLine} + {datum.DeliverySequence}";

                values.Add(
                    new CalculationValueModel
                    {
                        RowId = currentRowId,
                        ColumnId = "OrderNumber",
                        TextDisplay = datum.OrderNumber.ToString()
                    });

                values.Add(
                    new CalculationValueModel
                        {
                            RowId = currentRowId,
                            ColumnId = "OrderLine",
                            Value = datum.OrderLine
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = currentRowId,
                            ColumnId = "DeliverySequence",
                            Value = datum.DeliverySequence
                    });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = currentRowId,
                            ColumnId = "SupplierName",
                            TextDisplay = datum.SupplierName
                        });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = currentRowId,
                        ColumnId = "ExpectedDate",
                        TextDisplay = datum.ExpectedDate?.ToShortDateString()
                    });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = currentRowId,
                        ColumnId = "QtyOutstanding",
                        Value = datum.QtyOutstanding ?? 0
                    });
               
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = currentRowId,
                        ColumnId = "TotalPrice",
                        Value = datum.QtyOutstanding * datum.UnitPrice ?? 0
                    });
            }
            reportLayout.SetGridData(values);
            return reportLayout.GetResultsModel();
        }
    }
}
