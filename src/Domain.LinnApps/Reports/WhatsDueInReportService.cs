namespace Linn.Purchasing.Domain.LinnApps.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Reporting.Layouts;
    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Reports.Models;

    public class WhatsDueInReportService : IWhatsDueInReportService
    {
        private readonly IPurchaseOrderDeliveryRepository deliveryRepository;

        private readonly IReportingHelper reportingHelper;

        public WhatsDueInReportService(
            IReportingHelper reportingHelper,
            IPurchaseOrderDeliveryRepository deliveryRepository)
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
                     && d.PurchaseOrderDetail.PurchaseOrder.OrderMethodName != "CALL OFF"
                     && d.PurchaseOrderDetail.PurchaseOrder.Cancelled == "N"
                     && d.PurchaseOrderDetail.Cancelled == "N"
                     & new string[] { "RO", "PO" }.Contains(
                         d.PurchaseOrderDetail.PurchaseOrder.DocumentTypeName));

            if (!string.IsNullOrEmpty(vendorManager))
            {
                result = result.Where(x => x.PurchaseOrderDetail.PurchaseOrder.Supplier.VendorManager.Id == vendorManager);
            }

            var data = result.Select(d => new WhatsDueInEntry
                            {
                                SupplierId = d.PurchaseOrderDetail.PurchaseOrder.SupplierId,
                                PartNumber = d.PurchaseOrderDetail.Part.PartNumber,
                                AdvisedDate = d.DateAdvised,
                                CallOffDate = d.CallOffDate,
                                DeliverySequence = d.DeliverySeq,
                                DocumentType = d.PurchaseOrderDetail.PurchaseOrder.DocumentTypeName,
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
                $"Stock controlled parts due in between {fromDate:dd/MM/yyyy} and {toDate:dd/MM/yyyy}");

            reportLayout.AddColumnComponent(
                null,
                new List<AxisDetailsModel>
                    {
                        new AxisDetailsModel("OrderNumber", "Order",  GridDisplayType.TextValue),
                        new AxisDetailsModel("OrderLine", "Order Line",  GridDisplayType.TextValue),
                        new AxisDetailsModel("DeliverySequence", "Del", GridDisplayType.TextValue),
                        new AxisDetailsModel("PartNumber", "Part", GridDisplayType.TextValue),
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
                            TextDisplay = datum.OrderLine.ToString()
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = currentRowId,
                            ColumnId = "DeliverySequence",
                            TextDisplay = datum.DeliverySequence.ToString()
                    });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = currentRowId,
                            ColumnId = "PartNumber",
                            TextDisplay = datum.PartNumber
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
                        TextDisplay = datum.ExpectedDate?.ToString("dd/MM/yyyy")
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
