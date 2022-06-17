namespace Linn.Purchasing.Domain.LinnApps.Reports
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
            var data = this.repository.FilterBy(x => x.SupplierId.Equals(supplierId) 
                                                     && x.JobRef.Equals(jobRef) 
                                                     && !x.DateCancelled.HasValue
                                                     && x.OurQuantity > x.QuantityReceived
                                                     && !string.IsNullOrEmpty(x.AuthorisedBy))
                .OrderBy(x => x.OrderNumber);

            var resultsModels = new List<ResultsModel>();

            var partGroups = data.GroupBy(x => x.PartNumber);

            var reportLayout = new SimpleGridLayout(
                this.reportingHelper,
                CalculationValueModelType.Value,
                null,
                null);

            reportLayout.AddColumnComponent(
                null,
                new List<AxisDetailsModel>
                    {
                        new AxisDetailsModel("Delivery", "Delivery",  GridDisplayType.TextValue),
                        new AxisDetailsModel("Date", "Date",  GridDisplayType.TextValue),
                        new AxisDetailsModel("Qty", "Qty", GridDisplayType.Value) { DecimalPlaces = 2 },
                        new AxisDetailsModel("QtyReceived", "Qty Received", GridDisplayType.Value) { DecimalPlaces = 2 },
                        new AxisDetailsModel("QtyInvoiced", "Qty Invoiced", GridDisplayType.Value) { DecimalPlaces = 2 },
                        new AxisDetailsModel("DateRequested", "DateRequested",  GridDisplayType.TextValue),
                        new AxisDetailsModel("DateAdvised", "DateAdvised", GridDisplayType.TextValue)
                    });

            foreach (var group in partGroups)
            {
                var values = new List<CalculationValueModel>();

                foreach (var member in group)
                {
                    foreach (var delivery 
                             in member.Deliveries
                                 .Where(d => d.CallOffQty > d.QuantityReceived)
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
                                    TextDisplay = member.DeliveryDate?.ToShortDateString()
                                });
                        values.Add(
                            new CalculationValueModel
                                {
                                    RowId = rowId,
                                    ColumnId = "Qty",
                                    Value = member.OurQuantity
                                });
                        values.Add(
                            new CalculationValueModel
                                {
                                    RowId = rowId,
                                    ColumnId = "QtyReceived",
                                    Value = member.QuantityReceived ?? 0
                                });
                        values.Add(
                            new CalculationValueModel
                                {
                                    RowId = rowId,
                                    ColumnId = "QtyInvoiced",
                                    Value = member.QuantityInvoiced ?? 0
                                });
                        values.Add(
                            new CalculationValueModel
                                {
                                    RowId = rowId,
                                    ColumnId = "DateRequested",
                                    TextDisplay = member.LinnDeliveryDate?.ToShortDateString()
                                });
                        values.Add(
                            new CalculationValueModel
                                {
                                    RowId = rowId,
                                    ColumnId = "DateAdvised",
                                    TextDisplay = member.AdvisedDeliveryDate?.ToShortDateString()
                                });
                    }
                }

                reportLayout.SetGridData(values);

                reportLayout.ReportTitle = $"{group.Key}";

                resultsModels.Add(reportLayout.GetResultsModel());
            }

            return resultsModels;
        }
    }
}
