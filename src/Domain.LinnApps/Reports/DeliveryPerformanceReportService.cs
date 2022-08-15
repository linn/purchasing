namespace Linn.Purchasing.Domain.LinnApps.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.Reports.Models;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    public class DeliveryPerformanceReportService : IDeliveryPerformanceReportService
    {
        private readonly IQueryRepository<SupplierDeliveryPerformance> deliveryPerformanceRepository;

        private readonly IQueryRepository<DeliveryPerformanceDetail> deliveryPerformanceDetailRepository;

        private readonly IRepository<LedgerPeriod, int> ledgerPeriodRepository;

        private readonly IRepository<Supplier, int> supplierRepository;

        private readonly IReportingHelper reportingHelper;

        public DeliveryPerformanceReportService(
            IQueryRepository<SupplierDeliveryPerformance> deliveryPerformanceRepository,
            IQueryRepository<DeliveryPerformanceDetail> deliveryPerformanceDetailRepository,
            IRepository<LedgerPeriod, int> ledgerPeriodRepository,
            IRepository<Supplier, int> supplierRepository,
            IReportingHelper reportingHelper)
        {
            this.deliveryPerformanceRepository = deliveryPerformanceRepository;
            this.deliveryPerformanceDetailRepository = deliveryPerformanceDetailRepository;
            this.ledgerPeriodRepository = ledgerPeriodRepository;
            this.supplierRepository = supplierRepository;
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
            report.Columns.First(a => a.ColumnId == "Month Name").ColumnType = GridDisplayType.TextValue;

            var calculations = new List<CalculationValueModel>();
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
            var startMonth = this.ledgerPeriodRepository.FindById(startPeriod);
            if (startMonth == null)
            {
                throw new InvalidOptionException($"Ledger Period {startPeriod} not found");
            }

            var endMonth = this.ledgerPeriodRepository.FindById(endPeriod);
            if (endMonth == null)
            {
                throw new InvalidOptionException($"Ledger Period {endPeriod} not found");
            }

            var report = new ResultsModel(
                             new[]
                                 {
                                     "Supplier Id", "Supplier Name", "No Of Deliveries", "No On Time", "% On Time",
                                     "No Early", "No Late", "No Unack"
                                 })
                             {
                                 ReportTitle = new NameModel(
                                     $"Delivery Performance By Supplier {startMonth.MonthName} to {endMonth.MonthName}")
                             };

            report.Columns.First(a => a.ColumnId == "Supplier Id").ColumnType = GridDisplayType.TextValue;
            report.Columns.First(a => a.ColumnId == "Supplier Name").ColumnType = GridDisplayType.TextValue;

            var data = this.GetData(startPeriod, endPeriod, supplierId, vendorManager);

            var calculations = new List<CalculationValueModel>();
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
            this.reportingHelper.SortRowsByColumnValue(report, report.ColumnIndex("% On Time"));
            var drillDownUri = $"/purchasing/reports/delivery-performance-details/report?startPeriod={startPeriod}&endPeriod={endPeriod}&supplierId={{rowId}}";
            report.ValueDrillDownTemplates.Add(new DrillDownModel("details", drillDownUri, null, report.ColumnIndex("Supplier Id")));

            return report;
        }

        public ResultsModel GetDeliveryPerformanceDetails(int startPeriod, int endPeriod, int supplierId)
        {
            var startLedgerPeriod = this.ledgerPeriodRepository.FindById(startPeriod);
            if (startLedgerPeriod == null)
            {
                throw new InvalidOptionException($"Ledger Period {startPeriod} not found");
            }

            var endLedgerPeriod = this.ledgerPeriodRepository.FindById(endPeriod);
            if (endLedgerPeriod == null)
            {
                throw new InvalidOptionException($"Ledger Period {endPeriod} not found");
            }

            var supplier = this.supplierRepository.FindById(supplierId);
            if (supplier == null)
            {
                throw new InvalidOptionException($"Supplier {supplierId} not found");
            }

            var startDate = DateTime.Parse($"01-{startLedgerPeriod.MonthName.Substring(0, 3)}-{startLedgerPeriod.MonthName.Substring(3, 4)}");
            var endDate = DateTime.Parse($"01-{endLedgerPeriod.MonthName.Substring(0, 3)}-{endLedgerPeriod.MonthName.Substring(3, 4)}").AddMonths(1);

            var data = this.deliveryPerformanceDetailRepository.FilterBy(
                a => a.SupplierId == supplierId && a.DateArrived >= startDate && a.DateArrived < endDate);

            var report = new ResultsModel { ReportTitle = new NameModel($"Delivery Performance Details for {supplier.Name} {startLedgerPeriod.MonthName} to {endLedgerPeriod.MonthName}") };
            report.AddSortedColumns(
                new List<AxisDetailsModel>
                    {
                        new AxisDetailsModel("Order", "Order", GridDisplayType.TextValue),
                        new AxisDetailsModel("Line", "Line", GridDisplayType.TextValue),
                        new AxisDetailsModel("Del", "Del", GridDisplayType.TextValue),
                        new AxisDetailsModel("Part Number", "Part Number", GridDisplayType.TextValue),
                        new AxisDetailsModel("Requested", "Requested", GridDisplayType.TextValue),
                        new AxisDetailsModel("Advised", "Advised", GridDisplayType.TextValue),
                        new AxisDetailsModel("Arrived", "Arrived", GridDisplayType.TextValue),
                        new AxisDetailsModel("Reason", "Reason", GridDisplayType.TextValue),
                        new AxisDetailsModel("On Time", "On Time", GridDisplayType.TextValue)
                    });

            var calculations = new List<CalculationValueModel>();
            var rowIdCounter = 0;
            foreach (var dataItem in data
                         .OrderBy(a => a.OrderNumber)
                         .ThenBy(b => b.OrderLine)
                         .ThenBy(c => c.DeliverySequence))
            {
                var rowId = rowIdCounter ++.ToString();

                calculations.Add(new CalculationValueModel
                                     {
                                         RowId = rowId,
                                         ColumnId = "Order",
                                         TextDisplay = dataItem.OrderNumber.ToString()
                                     });
                calculations.Add(new CalculationValueModel
                                     {
                                         RowId = rowId,
                                         ColumnId = "Line",
                                         TextDisplay = dataItem.OrderLine.ToString()
                                     });
                calculations.Add(new CalculationValueModel
                                     {
                                         RowId = rowId,
                                         ColumnId = "Del",
                                         TextDisplay = dataItem.DeliverySequence.ToString()
                                     });
                calculations.Add(new CalculationValueModel
                                     {
                                         RowId = rowId,
                                         ColumnId = "Part Number",
                                         TextDisplay = dataItem.PartNumber
                                     });
                calculations.Add(new CalculationValueModel
                                     {
                                         RowId = rowId,
                                         ColumnId = "Requested",
                                         TextDisplay = dataItem.RequestedDate.ToString("dd-MMM-yyyy")
                                     });
                calculations.Add(new CalculationValueModel
                                     {
                                         RowId = rowId,
                                         ColumnId = "Advised",
                                         TextDisplay = dataItem.AdvisedDate?.ToString("dd-MMM-yyyy")
                                     });
                calculations.Add(new CalculationValueModel
                                     {
                                         RowId = rowId,
                                         ColumnId = "Arrived",
                                         TextDisplay = dataItem.DateArrived.ToString("dd-MMM-yyyy")
                                     });
                calculations.Add(new CalculationValueModel
                                     {
                                         RowId = rowId,
                                         ColumnId = "Reason",
                                         TextDisplay = dataItem.RescheduleReason
                                     });
                calculations.Add(new CalculationValueModel
                                     {
                                         RowId = rowId,
                                         ColumnId = "On Time",
                                         TextDisplay = dataItem.OnTime == 1 ? "Yes" : "No"
                                     });
            }

            this.reportingHelper.AddResultsToModel(report, calculations, CalculationValueModelType.Quantity, true);

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
                a => a.LedgerPeriod >= startPeriod && a.LedgerPeriod <= endPeriod && a.NumberOfDeliveries > 0);

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
