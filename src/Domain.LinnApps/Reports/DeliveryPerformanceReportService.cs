namespace Linn.Purchasing.Domain.LinnApps.Reports
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.Reports.Models;

    public class DeliveryPerformanceReportService : IDeliveryPerformanceReportService
    {
        private readonly IQueryRepository<SupplierDeliveryPerformance> deliveryPerformanceRepository;

        private readonly IReportingHelper reportingHelper;

        public DeliveryPerformanceReportService(
            IQueryRepository<SupplierDeliveryPerformance> deliveryPerformanceRepository,
            IReportingHelper reportingHelper)
        {
            this.deliveryPerformanceRepository = deliveryPerformanceRepository;
            this.reportingHelper = reportingHelper;
        }

        public ResultsModel GetDeliveryPerformanceSummary(int startPeriod, int endPeriod, int? supplierId, string vendorManager)
        {
            var data = this.GetData(startPeriod, endPeriod, supplierId, vendorManager);

            var report = new ResultsModel(
                             new[] { "Month Name", "No Of Deliveries", "No On Time", "% On Time", "No Early", "No Late", "No Unack" })
                             {
                                 ReportTitle = new NameModel("Supplier Delivery Performance Summary")
                             };

            IList<CalculationValueModel> calculations = new List<CalculationValueModel>();
            foreach (var dataItem in data)
            {
                var rowId = dataItem.LedgerPeriod.ToString();
                var rowTitle = dataItem.MonthName;
                calculations.Add(new CalculationValueModel
                                     {
                                        RowId = rowId,
                                        RowTitle = rowTitle,
                                        ColumnId = "Month Name",
                                        TextDisplay = dataItem.MonthName
                                     });
                this.CalculateDeliveryValues(calculations, rowId, rowTitle, dataItem);
            }

            this.reportingHelper.AddResultsToModel(report, calculations, CalculationValueModelType.Quantity, true);
            this.reportingHelper.SetValuesForPercentageColumn(
                report,
                report.ColumnIndex("No On Time"),
                report.ColumnIndex("No Of Deliveries"),
                report.ColumnIndex("% On Time"),
                1);
            foreach (var reportRow in report.Rows)
            {
                reportRow.SortOrder = int.Parse(reportRow.RowId);
            }

            report.Columns.First(a => a.ColumnId == "Month Name").ColumnType = GridDisplayType.TextValue;
            var drillDownUri = $"/purchasing/reports/delivery-performance-supplier/report?startPeriod={{rowId}}&endPeriod={{rowId}}&vendorManager={vendorManager}";
            if (supplierId.HasValue)
            {
                drillDownUri = $"{drillDownUri}&supplierId={supplierId}";
            }

            report.ValueDrillDownTemplates.Add(new DrillDownModel("supplier", drillDownUri, null, report.ColumnIndex("Month Name")));

            return report;
        }

        public ResultsModel GetDeliveryPerformanceBySupplier(int startPeriod, int endPeriod, int? supplierId, string vendorManager)
        {
            var data = this.GetData(startPeriod, endPeriod, supplierId, vendorManager);

            var report = new ResultsModel(
                new[] { "Supplier Id", "Supplier Name", "No Of Deliveries", "No On Time", "% On Time", "No Early", "No Late", "No Unack" })
            {
                ReportTitle = new NameModel("Delivery Performance By Supplier")
            };

            IList<CalculationValueModel> calculations = new List<CalculationValueModel>();
            foreach (var dataItem in data)
            {
                var rowId = dataItem.SupplierId.ToString();
                var rowTitle = dataItem.SupplierName;

                calculations.Add(new CalculationValueModel
                {
                    RowId = rowId,
                    RowTitle = rowTitle,
                    ColumnId = "Supplier Id",
                    TextDisplay = dataItem.SupplierId.ToString()
                });
                calculations.Add(new CalculationValueModel
                                     {
                                         RowId = rowId,
                                         RowTitle = rowTitle,
                                         ColumnId = "Supplier Name",
                                         TextDisplay = dataItem.SupplierName
                                     });

                this.CalculateDeliveryValues(calculations, rowId, rowTitle, dataItem);
            }

            this.reportingHelper.AddResultsToModel(report, calculations, CalculationValueModelType.Quantity, true);
            this.reportingHelper.SetValuesForPercentageColumn(
                report,
                report.ColumnIndex("No On Time"),
                report.ColumnIndex("No Of Deliveries"),
                report.ColumnIndex("% On Time"),
                1);
            this.reportingHelper.SortRowsByColumnValue(report, report.ColumnIndex("% On Time"), true);

            return report;
        }

        private void CalculateDeliveryValues(
            ICollection<CalculationValueModel> calculations,
            string rowId,
            string rowTitle,
            SupplierDeliveryPerformance dataItem)
        {
            calculations.Add(
                new CalculationValueModel
                    {
                        RowId = rowId,
                        RowTitle = rowTitle,
                        ColumnId = "No Of Deliveries",
                        Quantity = dataItem.NumberOfDeliveries
                    });
            calculations.Add(
                new CalculationValueModel
                    {
                        RowId = rowId, RowTitle = rowTitle, ColumnId = "No On Time", Quantity = dataItem.NumberOnTime
                    });
            calculations.Add(
                new CalculationValueModel
                    {
                        RowId = rowId, RowTitle = rowTitle, ColumnId = "No Early", Quantity = dataItem.NumberOfEarlyDeliveries
                    });
            calculations.Add(
                new CalculationValueModel
                    {
                        RowId = rowId, RowTitle = rowTitle, ColumnId = "No Late", Quantity = dataItem.NumberOfLateDeliveries
                    });
            calculations.Add(
                new CalculationValueModel
                    {
                        RowId = rowId,
                        RowTitle = rowTitle,
                        ColumnId = "No Unack",
                        Quantity = dataItem.NumberOfUnacknowledgedDeliveries
                    });
        }

        private IEnumerable<SupplierDeliveryPerformance> GetData(
            int startPeriod,
            int endPeriod,
            int? supplierId,
            string vendorManager)
        {
            var data = this.deliveryPerformanceRepository.FilterBy(
                a => a.LedgerPeriod >= startPeriod && a.LedgerPeriod <= endPeriod);

            if (supplierId.HasValue)
            {
                data = data.Where(a => a.SupplierId == supplierId);
            }

            if (!string.IsNullOrEmpty(vendorManager))
            {
                data = data.Where(a => a.VendorManager == vendorManager);
            }

            return data;
        }
    }
}
