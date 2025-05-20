namespace Linn.Purchasing.Domain.LinnApps.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Layouts;
    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;
    using Linn.Purchasing.Domain.LinnApps.Reports.Models;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    public class SpendsReportService : ISpendsReportService
    {
        private readonly IPurchaseLedgerPack purchaseLedgerPack;

        private readonly ILedgerPeriodPack ledgerPeriodPack;

        private readonly IReportingHelper reportingHelper;

        private readonly IRepository<LedgerPeriod, int> ledgerPeriodRepository;

        private readonly IQueryRepository<SupplierSpend> spendsRepository;

        private readonly IRepository<VendorManager, string> vendorManagerRepository;

        private readonly IRepository<Supplier, int> supplierRepository;

        public SpendsReportService(
            IQueryRepository<SupplierSpend> spendsRepository,
            IRepository<VendorManager, string> vendorManagerRepository,
            IPurchaseLedgerPack purchaseLedgerPack,
            ILedgerPeriodPack ledgerPeriodPack,
            IRepository<Supplier, int> supplierRepository,
            IReportingHelper reportingHelper,
            IRepository<LedgerPeriod, int> ledgerPeriodRepository)
        {
            this.spendsRepository = spendsRepository;
            this.vendorManagerRepository = vendorManagerRepository;
            this.purchaseLedgerPack = purchaseLedgerPack;
            this.ledgerPeriodPack = ledgerPeriodPack;
            this.reportingHelper = reportingHelper;
            this.ledgerPeriodRepository = ledgerPeriodRepository;
            this.supplierRepository = supplierRepository;
        }

        public ResultsModel GetSpendBySupplierReport(string vendorManagerId)
        {
            var currentLedgerPeriod = this.purchaseLedgerPack.GetLedgerPeriod();
            var yearStartLedgerPeriod = this.purchaseLedgerPack.GetYearStartLedgerPeriod();
            var previousYearStartLedgerPeriod = yearStartLedgerPeriod - 12;
            var yearBeforePreviousStartLedgerPeriod = yearStartLedgerPeriod - 24;

            var supplierSpends = this.spendsRepository.FindAll()
                .Where(
                    x =>
                        x.VendorManager != null &&
                        x.LedgerPeriod >= yearBeforePreviousStartLedgerPeriod && x.LedgerPeriod <= currentLedgerPeriod
                        && (string.IsNullOrWhiteSpace(vendorManagerId) || x.VendorManager == vendorManagerId))
                .ToList();

            var vendorManagerName = "ALL";
            if (!string.IsNullOrWhiteSpace(vendorManagerId))
            {
                var vendorManager = this.vendorManagerRepository.FindById(vendorManagerId);
                vendorManagerName = $"{vendorManagerId} - {vendorManager.Employee.FullName}";
            }

            var reportLayout = new SimpleGridLayout(
                this.reportingHelper,
                CalculationValueModelType.Value,
                null,
                $"Spend by supplier report for Vendor Manager: {vendorManagerName} - GBP (excluding factors & VAT.)");

            reportLayout.AddColumnComponent(
                null,
                new List<AxisDetailsModel>
                    {
                        new AxisDetailsModel("Name", "Name", GridDisplayType.TextValue),
                        new AxisDetailsModel("YearBeforeLast", "Year Before Last", GridDisplayType.Value) { DecimalPlaces = 2 },
                        new AxisDetailsModel("LastYear", "Last Year", GridDisplayType.Value) { DecimalPlaces = 2 },
                        new AxisDetailsModel("ThisYear", "This Year", GridDisplayType.Value) { DecimalPlaces = 2 },
                        new AxisDetailsModel("ThisMonth", "This Month", GridDisplayType.Value) { DecimalPlaces = 2 }
                    });

            var values = new List<CalculationValueModel>();

            var distinctSupplierSpends = supplierSpends.DistinctBy(x => x.SupplierId).Select(
                x => new SupplierSpendWithTotals
                         {
                             BaseTotal = x.BaseTotal ?? 0,
                             LedgerPeriod = x.LedgerPeriod,
                             SupplierId = x.SupplierId,
                             SupplierName = x.SupplierName,
                             MonthTotal =
                                 supplierSpends
                                     .Where(s => s.SupplierId == x.SupplierId && s.LedgerPeriod == currentLedgerPeriod)
                                     .Sum(z => z.BaseTotal ?? 0),
                             YearTotal =
                                 supplierSpends
                                     .Where(s => s.SupplierId == x.SupplierId && s.LedgerPeriod >= yearStartLedgerPeriod)
                                     .Sum(z => z.BaseTotal ?? 0),
                             PrevYearTotal = supplierSpends
                                 .Where(
                                     s => s.SupplierId == x.SupplierId
                                          && s.LedgerPeriod >= previousYearStartLedgerPeriod
                                          && s.LedgerPeriod < yearStartLedgerPeriod)
                                 .Sum(z => z.BaseTotal ?? 0),
                             YearBeforePrevYearTotal = supplierSpends
                                                           .Where(
                                                               s => s.SupplierId == x.SupplierId
                                                                    && s.LedgerPeriod >= yearBeforePreviousStartLedgerPeriod
                                                                    && s.LedgerPeriod < previousYearStartLedgerPeriod)
                                                           .Sum(z => z.BaseTotal ?? 0)
                })
                .OrderByDescending(x => x.PrevYearTotal)
                .ThenByDescending(s => s.YearTotal)
                .ThenByDescending(s => s.MonthTotal);

            foreach (var spend in distinctSupplierSpends)
            {
                var currentRowId = $"{spend.SupplierId}";

                values.Add(
                    new CalculationValueModel
                        {
                            RowId = currentRowId,
                            ColumnId = "Name",
                            TextDisplay = spend.SupplierName
                        });

                values.Add(
                    new CalculationValueModel
                        {
                            RowId = currentRowId,
                            ColumnId = "YearBeforeLast",
                            Value = spend.YearBeforePrevYearTotal
                        });

                values.Add(
                    new CalculationValueModel
                        {
                            RowId = currentRowId,
                            ColumnId = "LastYear",
                            Value = spend.PrevYearTotal
                        });

                values.Add(
                    new CalculationValueModel
                        {
                            RowId = currentRowId,
                            ColumnId = "ThisYear",
                            Value = spend.YearTotal
                        });

                values.Add(
                    new CalculationValueModel
                        {
                            RowId = currentRowId,
                            ColumnId = "ThisMonth",
                            Value = spend.MonthTotal
                        });
            }

            reportLayout.SetGridData(values);
            var model = reportLayout.GetResultsModel();

            model.RowDrillDownTemplates.Add(new DrillDownModel("Id", "/purchasing/reports/spend-by-part/report?id={textValue}"));
            model.RowHeader = "Supplier (Drilldown)";

            return model;
        }

        public ResultsModel GetSpendBySupplierByDateRangeReport(
            string fromDate,
            string toDate,
            string vendorManagerId,
            int? supplierId)
        {
            var from = DateTime.Parse(fromDate).Date;
            var to = DateTime.Parse(toDate).Date.AddDays(1).AddTicks(-1);
            var fromLedgerPeriod = this.ledgerPeriodPack.GetPeriodNumber(from);
            var fromPeriod = this.ledgerPeriodRepository.FindById(fromLedgerPeriod);
            var toLedgerPeriod = this.ledgerPeriodPack.GetPeriodNumber(to);
            var toPeriod = this.ledgerPeriodRepository.FindById(toLedgerPeriod);

            var supplierSpends = this.spendsRepository.FilterBy(
                    x =>
                        x.VendorManager != null &&
                        x.LedgerPeriod >= fromLedgerPeriod && x.LedgerPeriod <= toLedgerPeriod
                        && (string.IsNullOrWhiteSpace(vendorManagerId)
                            || x.VendorManager == vendorManagerId))
                .ToList();

            var vendorManagerName = "ALL";
            if (!string.IsNullOrWhiteSpace(vendorManagerId))
            {
                var vendorManager = this.vendorManagerRepository.FindById(vendorManagerId);
                vendorManagerName = $"{vendorManagerId} - {vendorManager.Employee.FullName} ({vendorManager.UserNumber})";
            }

            if (supplierId.HasValue)
            {
                supplierSpends = supplierSpends.Where(a => a.SupplierId == supplierId).ToList();
            }

            var reportLayout = new SimpleGridLayout(
                this.reportingHelper,
                CalculationValueModelType.Value,
                null,
                $"Spend by supplier report for Vendor Manager: {vendorManagerName} between {fromPeriod.MonthName} and {toPeriod.MonthName}.");

            reportLayout.AddColumnComponent(
               null,
               new List<AxisDetailsModel>
                   {
                        new AxisDetailsModel("Name", "Name", GridDisplayType.TextValue),
                        new AxisDetailsModel(
                            "DateRangeTotal",
                            "Date Range Total",
                            GridDisplayType.Value)
                            {
                                DecimalPlaces = 2
                            },
                   });

            var values = new List<CalculationValueModel>();

            var distinctSupplierSpends = supplierSpends
                .DistinctBy(x => x.SupplierId)
                .Select(x => new SupplierSpendWithTotals
                                 {
                                     BaseTotal = x.BaseTotal.GetValueOrDefault(),
                                     LedgerPeriod = x.LedgerPeriod,
                                     SupplierId = x.SupplierId,
                                     SupplierName = x.SupplierName,
                                     DateRangeTotal = supplierSpends
                                         .Where(s => s.SupplierId == x.SupplierId
                                                     && s.LedgerPeriod >= fromLedgerPeriod
                                                     && s.LedgerPeriod <= toLedgerPeriod)
                                         .Sum(z => z.BaseTotal.GetValueOrDefault())
                                 })
                .OrderBy(x => x.SupplierName);

            foreach (var supplier in distinctSupplierSpends)
            {
                ExtractDetailsForSupplierByDateRangeReport(values, supplier);
            }

            reportLayout.SetGridData(values);
            var model = reportLayout.GetResultsModel();

            model.RowDrillDownTemplates.Add(new DrillDownModel("by part", $"/purchasing/reports/spend-by-part-by-date/report?id={{textValue}}&fromDate={fromDate}&toDate={toDate}&vm={vendorManagerId}"));
            model.RowHeader = "Supplier (Drilldown)";

            return model;
        }

        public ResultsModel GetSpendByPartReport(int supplierId)
        {
            var currentLedgerPeriod = this.purchaseLedgerPack.GetLedgerPeriod();
            var yearStartLedgerPeriod = this.purchaseLedgerPack.GetYearStartLedgerPeriod();
            var previousYearStartLedgerPeriod = yearStartLedgerPeriod - 12;
            var yearBeforePreviousStartLedgerPeriod = yearStartLedgerPeriod - 24;

            var supplierSpends = this.spendsRepository
                .FilterBy(x => x.LedgerPeriod >= yearBeforePreviousStartLedgerPeriod
                               && x.LedgerPeriod <= currentLedgerPeriod
                               && x.SupplierId == supplierId
                               && x.OrderNumber.HasValue
                               && x.OrderLine.HasValue)
                .ToList();

            var supplier = this.supplierRepository.FindById(supplierId);

            var reportLayout = new SimpleGridLayout(
                this.reportingHelper,
                CalculationValueModelType.Value,
                null,
                $"Spend by part report for Supplier: {supplier.Name} ({supplierId}). In GBP, for this financial year and last, excludes factors & VAT.");

            AddPartReportColumns(reportLayout);

            var values = new List<CalculationValueModel>();
            var distinctPartSpends = supplierSpends
                .DistinctBy(x => x.PartNumber)
                .Select(x => new PartSpendWithTotals
                                 {
                                     BaseTotal = x.BaseTotal ?? 0,
                                     LedgerPeriod = x.LedgerPeriod,
                                     PartNumber = x.PartNumber,
                                     PartDescription = x.PartDescription,
                                     MonthTotal = supplierSpends
                                         .Where(s => s.PartNumber == x.PartNumber && s.LedgerPeriod == currentLedgerPeriod)
                                         .Sum(z => z.BaseTotal ?? 0),
                                     YearTotal = supplierSpends
                                         .Where(s => s.PartNumber == x.PartNumber && s.LedgerPeriod >= yearStartLedgerPeriod)
                                         .Sum(z => z.BaseTotal ?? 0),
                                     PrevYearTotal = supplierSpends
                                         .Where(s => s.PartNumber == x.PartNumber
                                                     && s.LedgerPeriod >= previousYearStartLedgerPeriod
                                                     && s.LedgerPeriod < yearStartLedgerPeriod)
                                         .Sum(z => z.BaseTotal ?? 0),
                                     YearBeforeLastTotal = supplierSpends
                                         .Where(s => s.PartNumber == x.PartNumber
                                                     && s.LedgerPeriod >= yearBeforePreviousStartLedgerPeriod
                                                     && s.LedgerPeriod < previousYearStartLedgerPeriod)
                                         .Sum(z => z.BaseTotal ?? 0)
                                 })
                .OrderByDescending(x => x.PrevYearTotal)
                .ThenByDescending(s => s.YearTotal)
                .ThenByDescending(s => s.MonthTotal);

            foreach (var part in distinctPartSpends)
            {
                ExtractDetailsForPartReport(values, part);
            }

            reportLayout.SetGridData(values);
            var model = reportLayout.GetResultsModel();

            return model;
        }

        public ResultsModel GetSpendByPartByDateReport(int supplierId, string fromDate, string toDate)
        {
            var from = DateTime.Parse(fromDate).Date;
            var to = DateTime.Parse(toDate).Date.AddDays(1).AddTicks(-1);
            var fromLedgerPeriod = this.ledgerPeriodPack.GetPeriodNumber(from);
            var toLedgerPeriod = this.ledgerPeriodPack.GetPeriodNumber(to);

            var supplierSpends = this.spendsRepository
                .FilterBy(x => x.LedgerPeriod >= fromLedgerPeriod
                               && x.LedgerPeriod <= toLedgerPeriod
                               && x.SupplierId == supplierId)
                .ToList();

            var supplier = this.supplierRepository.FindById(supplierId);

            var reportLayout = new SimpleGridLayout(
                this.reportingHelper,
                CalculationValueModelType.Value,
                null,
                $"Spend by part report for Supplier: {supplier.Name} ({supplierId}) in GBP between {from.ToString("dd-MMM-yyyy")} and {to.ToString("dd-MMM-yyyy")}");

            reportLayout.AddColumnComponent(
                null,
                new List<AxisDetailsModel>
                    {
                        new AxisDetailsModel("PartNumber", "PartNumber", GridDisplayType.TextValue)
                            {
                                AllowWrap = false
                            },
                        new AxisDetailsModel("Description", "Description", GridDisplayType.TextValue),
                        new AxisDetailsModel("Spend", "Spend", GridDisplayType.Value) { DecimalPlaces = 2 },
                    });

            var values = new List<CalculationValueModel>();
            foreach (var supplierSpend in supplierSpends.GroupBy(a => a.PartNumber))
            {
                var value = supplierSpend.Sum(a => a.BaseTotal ?? 0);
                values.Add(new CalculationValueModel
                               {
                                   Value = value,
                                   BaseValue = value,
                                   ColumnId = "Spend",
                                   RowId = supplierSpend.Key
                               });
                values.Add(new CalculationValueModel
                               {
                                   TextDisplay = supplierSpend.Key,
                                   ColumnId = "PartNumber",
                                   RowId = supplierSpend.Key
                               });
                values.Add(new CalculationValueModel
                               {
                                   TextDisplay = supplierSpend.First().PartDescription,
                                   ColumnId = "Description",
                                   RowId = supplierSpend.Key
                               });
            }

            reportLayout.SetGridData(values);
            var model = reportLayout.GetResultsModel();
            this.reportingHelper.SortRowsByColumnValue(model, model.ColumnIndex("PartNumber"));

            return model;
        }

        private static void AddPartReportColumns(SimpleGridLayout reportLayout)
        {
            reportLayout.AddColumnComponent(
                null,
                new List<AxisDetailsModel>
                    {
                        new AxisDetailsModel("PartNumber", "PartNumber", GridDisplayType.TextValue)
                            {
                                AllowWrap = false
                            },
                        new AxisDetailsModel("Description", "Description", GridDisplayType.TextValue),
                        new AxisDetailsModel("YearBeforeLast", "Year Before Last", GridDisplayType.Value) { DecimalPlaces = 2 },
                        new AxisDetailsModel("LastYear", "Last Year", GridDisplayType.Value) { DecimalPlaces = 2 },
                        new AxisDetailsModel("ThisYear", "This Year", GridDisplayType.Value) { DecimalPlaces = 2 },
                        new AxisDetailsModel("ThisMonth", "This Month", GridDisplayType.Value) { DecimalPlaces = 2 }
                    });
        }


        private static void ExtractDetailsForSupplierByDateRangeReport(
            ICollection<CalculationValueModel> values,
            SupplierSpendWithTotals spend)
        {
            var currentRowId = $"{spend.SupplierId}";

            values.Add(
                new CalculationValueModel
                    {
                        RowId = currentRowId,
                        ColumnId = "Name",
                        TextDisplay = spend.SupplierName
                    });

            values.Add(
                new CalculationValueModel
                    {
                        RowId = currentRowId,
                        ColumnId = "DateRangeTotal",
                        Value = spend.DateRangeTotal
                    });
        }

        private static void ExtractDetailsForPartReport(
            ICollection<CalculationValueModel> values,
            PartSpendWithTotals part)
        {
            var currentRowId = $"{part.PartNumber}";
            values.Add(
                new CalculationValueModel
                    {
                        RowId = currentRowId,
                        ColumnId = "PartNumber",
                        TextDisplay = $"{part.PartNumber}"
                    });

            values.Add(
                new CalculationValueModel
                    {
                        RowId = currentRowId,
                        ColumnId = "Description",
                        TextDisplay = part.PartDescription
                    });
            values.Add(
                new CalculationValueModel
                    {
                        RowId = currentRowId,
                        ColumnId = "YearBeforeLast",
                        Value = part.YearBeforeLastTotal
                    });
            values.Add(
                new CalculationValueModel
                    {
                        RowId = currentRowId,
                        ColumnId = "LastYear",
                        Value = part.PrevYearTotal
                    });

            values.Add(
                new CalculationValueModel
                    {
                        RowId = currentRowId,
                        ColumnId = "ThisYear",
                        Value = part.YearTotal
                    });

            values.Add(
                new CalculationValueModel
                    {
                        RowId = currentRowId,
                        ColumnId = "ThisMonth",
                        Value = part.MonthTotal
                    });
        }
    }
}
