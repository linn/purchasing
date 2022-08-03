﻿namespace Linn.Purchasing.Domain.LinnApps.Reports
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Layouts;
    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.MaterialRequirements;

    public class MrOrderBookReportService : IMrOrderBookReportService
    {
        private readonly IQueryRepository<MrPurchaseOrderDetail> repository;

        private readonly ISingleRecordRepository<MrMaster> mrMaster;

        private readonly IReportingHelper reportingHelper;

        public MrOrderBookReportService(
            IQueryRepository<MrPurchaseOrderDetail> repository,
            ISingleRecordRepository<MrMaster> mrMaster,
            IReportingHelper reportingHelper)
        {
            this.repository = repository;
            this.mrMaster = mrMaster;
            this.reportingHelper = reportingHelper;
        }

        public IEnumerable<ResultsModel> GetOrderBookReport(int supplierId)
        {
            var jobRef = this.mrMaster.GetRecord().JobRef;
            var data = this.repository.FilterBy(
                    x => x.SupplierId.Equals(supplierId) && x.PartSupplierRecord != null && x.JobRef.Equals(jobRef)
                         && !x.DateCancelled.HasValue && !string.IsNullOrEmpty(x.AuthorisedBy)
                         && x.OurQuantity > x.QuantityReceived && !string.IsNullOrEmpty(x.AuthorisedBy))
                .OrderBy(x => x.OrderNumber).ToList();

            var resultsModels = new List<ResultsModel>();

            var partGroups = data.GroupBy(x => x.PartNumber);

            var reportLayout = new SimpleGridLayout(this.reportingHelper, CalculationValueModelType.Value, null, null);

            reportLayout.AddColumnComponent(
                null,
                new List<AxisDetailsModel>
                    {
                        new("Delivery", "Delivery", GridDisplayType.TextValue),
                        new("Date", "Date", GridDisplayType.TextValue),
                        new("QtyOnOrder", "Order Qty", GridDisplayType.Value) { DecimalPlaces = 2 },
                        new("Qty", "Qty", GridDisplayType.Value) { DecimalPlaces = 2 },
                        new("QtyReceived", "Qty Received", GridDisplayType.Value) { DecimalPlaces = 2 },
                        new("QtyInvoiced", "Qty Invoiced", GridDisplayType.Value) { DecimalPlaces = 2 },
                        new("DateRequested", "DateRequested", GridDisplayType.TextValue),
                        new("DateAdvised", "DateAdvised", GridDisplayType.TextValue)
                    });

            foreach (var group in partGroups)
            {
                var values = new List<CalculationValueModel>();

                foreach (var member in group)
                foreach (var delivery in member.Deliveries.Where(d => d.Quantity > d.QuantityReceived)
                             .OrderBy(c => c.DeliverySequence))
                {
                    var rowId = $"{delivery.OrderNumber}/{delivery.OrderLine}/{delivery.DeliverySequence}";
                    values.Add(
                        new CalculationValueModel
                            {
                                RowId = rowId,
                                ColumnId = "Delivery",
                                TextDisplay = $"{delivery.OrderNumber}/{delivery.DeliverySequence}"
                            });
                    values.Add(
                        new CalculationValueModel
                            {
                                RowId = rowId,
                                ColumnId = "Date",
                                TextDisplay = delivery.CallOffDate?.ToShortDateString()
                            });
                    values.Add(
                        new CalculationValueModel
                            {
                                RowId = rowId, ColumnId = "QtyOnOrder", Value = member.OurQuantity
                            });
                    values.Add(
                        new CalculationValueModel { RowId = rowId, ColumnId = "Qty", Value = delivery.Quantity });
                    values.Add(
                        new CalculationValueModel
                            {
                                RowId = rowId, ColumnId = "QtyReceived", Value = delivery.QuantityReceived
                            });
                    values.Add(
                        new CalculationValueModel
                            {
                                RowId = rowId, ColumnId = "QtyInvoiced", Value = member.QuantityInvoiced ?? 0
                            });
                    values.Add(
                        new CalculationValueModel
                            {
                                RowId = rowId,
                                ColumnId = "DateRequested",
                                TextDisplay = delivery.RequestedDeliveryDate?.ToShortDateString()
                            });
                    values.Add(
                        new CalculationValueModel
                            {
                                RowId = rowId,
                                ColumnId = "DateAdvised",
                                TextDisplay = delivery.AdvisedDeliveryDate?.ToShortDateString()
                            });
                }

                reportLayout.SetGridData(values);
                var partSupplier = group.First().PartSupplierRecord;
                reportLayout.ReportTitle = $"{partSupplier.PartNumber} - "
                                           + $"UOM: {partSupplier.Part.OurUnitOfMeasure} - "
                                           + $"LEAD TIME: {partSupplier.LeadTimeWeeks} WEEKS";

                resultsModels.Add(reportLayout.GetResultsModel());
            }

            return resultsModels;
        }

        public ResultsModel GetOrderBookExport(int supplierId)
        {
            var jobRef = this.mrMaster.GetRecord().JobRef;
            var data = this.repository.FilterBy(
                    x => x.SupplierId.Equals(supplierId) && x.PartSupplierRecord != null && x.JobRef.Equals(jobRef)
                         && !x.DateCancelled.HasValue && !string.IsNullOrEmpty(x.AuthorisedBy)
                         && x.OurQuantity > x.QuantityReceived && !string.IsNullOrEmpty(x.AuthorisedBy))
                .OrderBy(x => x.OrderNumber).ToList();

            var reportLayout = new SimpleGridLayout(this.reportingHelper, CalculationValueModelType.Value, null, null);

            reportLayout.AddColumnComponent(
                null,
                new List<AxisDetailsModel>
                    {
                        new("PartNumber", "Part", GridDisplayType.TextValue),
                        new("PartDescription", "Desc", GridDisplayType.TextValue),
                        new("UoM", "Our UoM", GridDisplayType.TextValue),
                        new("LeadTimeWeeks", "Lead Time Weeks", GridDisplayType.TextValue),
                        new("Delivery", "Delivery", GridDisplayType.TextValue),
                        new("Date", "Date", GridDisplayType.TextValue),
                        new("QtyOnOrder", "Order Qty", GridDisplayType.Value) { DecimalPlaces = 2 },
                        new("Qty", "Qty", GridDisplayType.Value) { DecimalPlaces = 2 },
                        new("QtyReceived", "Qty Received", GridDisplayType.Value) { DecimalPlaces = 2 },
                        new("QtyInvoiced", "Qty Invoiced", GridDisplayType.Value) { DecimalPlaces = 2 },
                        new("DateRequested", "DateRequested", GridDisplayType.TextValue),
                        new("DateAdvised", "DateAdvised", GridDisplayType.TextValue)
                    });
            var values = new List<CalculationValueModel>();

            foreach (var datum in data)
            {
                foreach (var delivery in datum.Deliveries.Where(d => d.Quantity > d.QuantityReceived)
                             .OrderBy(c => c.DeliverySequence))
                {
                    var rowId = $"{delivery.OrderNumber}/{delivery.OrderLine}/{delivery.DeliverySequence}";
                    values.Add(
                        new CalculationValueModel
                            {
                                RowId = rowId,
                                ColumnId = "PartNumber",
                                TextDisplay = datum.PartNumber
                            });
                    values.Add(
                        new CalculationValueModel
                            {
                                RowId = rowId,
                                ColumnId = "PartDescription",
                                TextDisplay = datum.PartSupplierRecord.Part.Description
                            });
                    values.Add(
                        new CalculationValueModel
                            {
                                RowId = rowId,
                                ColumnId = "UoM",
                                TextDisplay = datum.PartSupplierRecord.Part.OurUnitOfMeasure
                            });
                    values.Add(
                        new CalculationValueModel
                            {
                                RowId = rowId,
                                ColumnId = "LeadTimeWeeks",
                                TextDisplay = datum.PartSupplierRecord.LeadTimeWeeks.ToString()
                            });
                    values.Add(
                        new CalculationValueModel
                            {
                                RowId = rowId,
                                ColumnId = "Delivery",
                                TextDisplay = $"{delivery.OrderNumber}/{delivery.DeliverySequence}"
                            });
                    values.Add(
                        new CalculationValueModel
                            {
                                RowId = rowId,
                                ColumnId = "Date",
                                TextDisplay = delivery.CallOffDate?.ToShortDateString()
                            });
                    values.Add(
                        new CalculationValueModel
                            {
                                RowId = rowId, ColumnId = "QtyOnOrder", Value = datum.OurQuantity
                            });
                    values.Add(
                        new CalculationValueModel { RowId = rowId, ColumnId = "Qty", Value = delivery.Quantity });
                    values.Add(
                        new CalculationValueModel
                            {
                                RowId = rowId, ColumnId = "QtyReceived", Value = delivery.QuantityReceived
                            });
                    values.Add(
                        new CalculationValueModel
                            {
                                RowId = rowId, ColumnId = "QtyInvoiced", Value = datum.QuantityInvoiced ?? 0
                            });
                    values.Add(
                        new CalculationValueModel
                            {
                                RowId = rowId,
                                ColumnId = "DateRequested",
                                TextDisplay = delivery.RequestedDeliveryDate?.ToShortDateString()
                            });
                    values.Add(
                        new CalculationValueModel
                            {
                                RowId = rowId,
                                ColumnId = "DateAdvised",
                                TextDisplay = delivery.AdvisedDeliveryDate?.ToShortDateString()
                            });
                }
            }
            reportLayout.SetGridData(values);

            return reportLayout.GetResultsModel();
        }
    }
}
