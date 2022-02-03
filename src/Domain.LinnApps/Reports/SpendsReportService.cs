namespace Linn.Purchasing.Domain.LinnApps.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Layouts;
    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;

    using MoreLinq;

    public class SpendsReportService : ISpendsReportService
    {
        private readonly IPurchaseLedgerPack purchaseLedgerPack;

        private readonly IReportingHelper reportingHelper;

        private readonly IQueryRepository<SupplierSpend> spendsRepository;

        private readonly IRepository<VendorManager, string> vendorManagerRepository;


        public SpendsReportService(
            IQueryRepository<SupplierSpend> spendsRepository,
            IRepository<VendorManager, string> vendorManagerRepository,
            IPurchaseLedgerPack purchaseLedgerPack,
            IReportingHelper reportingHelper)
        {
            this.spendsRepository = spendsRepository;
            this.vendorManagerRepository = vendorManagerRepository;
            this.purchaseLedgerPack = purchaseLedgerPack;
            this.reportingHelper = reportingHelper;
        }

        public ResultsModel GetSpendBySupplierReport(string vm)
        {
            var currentLedgerPeriod = this.purchaseLedgerPack.GetLedgerPeriod();
            var yearStartLedgerPeriod = this.purchaseLedgerPack.GetYearStartLedgerPeriod();
            var previousYearStartLedgerPeriod = yearStartLedgerPeriod - 12;

            // do I need to check the current ledger period as an upper limit? Not sure if we'd have stuff in the future
            var supplierSpends = this.spendsRepository.FilterBy(
                        x => x.LedgerPeriod >= previousYearStartLedgerPeriod && x.LedgerPeriod <= currentLedgerPeriod
                             && (string.IsNullOrWhiteSpace(vm) || x.Supplier.VendorManager == vm))
                    .ToList();

            var vmName = "ALL";
            if (!string.IsNullOrWhiteSpace(vm))
            {
                var vendorManager = this.vendorManagerRepository.FindById(vm);
                vmName = $"{vm} - {vendorManager.Employee.FullName} ({vendorManager.UserNumber})";
            }

            var reportLayout = new SimpleGridLayout(
                this.reportingHelper,
                CalculationValueModelType.TextValue,
                null,
                $"Spend by supplier report for Vendor Manager: {vmName}. For this financial year and last, excludes factors & VAT.");

            this.AddSupplierReportColumns(reportLayout);

            var values = new List<CalculationValueModel>();

            var distinctSupplierSpends = supplierSpends.DistinctBy(x => x.SupplierId).Select(
                x => new SupplierSpend
                         {
                             BaseTotal = x.BaseTotal,
                             LedgerPeriod = x.LedgerPeriod,
                             Supplier = x.Supplier,
                             SupplierId = x.SupplierId,
                             MonthTotal =
                                 supplierSpends.Where(
                                         s => s.SupplierId == x.SupplierId && s.LedgerPeriod == currentLedgerPeriod)
                                     .Sum(z => z.BaseTotal),
                             YearTotal =
                                 supplierSpends.Where(
                                         s => s.SupplierId == x.SupplierId && s.LedgerPeriod >= yearStartLedgerPeriod)
                                     .Sum(z => z.BaseTotal),
                             PrevYearTotal = supplierSpends.Where(
                                     s => s.SupplierId == x.SupplierId
                                          && s.LedgerPeriod >= previousYearStartLedgerPeriod
                                          && s.LedgerPeriod < yearStartLedgerPeriod)
                                 .Sum(z => z.BaseTotal)
                         }).OrderByDescending(x => x.PrevYearTotal).ThenByDescending(s => s.YearTotal).ThenByDescending(s => s.MonthTotal);

            foreach (var supplier in distinctSupplierSpends)
            {
                this.ExtractDetailsForPartReport(values, supplier);
            }

            reportLayout.SetGridData(values);
            var model = reportLayout.GetResultsModel();

            return model;

        }

        private void AddSupplierReportColumns(SimpleGridLayout reportLayout)
        {
            reportLayout.AddColumnComponent(
                null,
                new List<AxisDetailsModel>
                    {
                        new AxisDetailsModel("SupplierId", "Supplier Id", GridDisplayType.TextValue)
                            {
                                AllowWrap = false
                            },
                        new AxisDetailsModel("Name", "Name", GridDisplayType.TextValue),
                        new AxisDetailsModel("LastYear", "Last Year", GridDisplayType.TextValue),
                        new AxisDetailsModel("ThisYear", "This Year", GridDisplayType.TextValue),
                        new AxisDetailsModel("ThisMonth", "This Month", GridDisplayType.TextValue)
                    });
        }

        private void ExtractDetailsForPartReport(ICollection<CalculationValueModel> values, SupplierSpend supplier)
        {
            var currentRowId = $"{supplier.SupplierId}";
            values.Add(
                new CalculationValueModel
                    {
                        RowId = currentRowId, ColumnId = "SupplierId", TextDisplay = $"{supplier.SupplierId}"
                    });

            values.Add(
                new CalculationValueModel
                    {
                        RowId = currentRowId, ColumnId = "Name", TextDisplay = supplier.Supplier.Name
                    });

            values.Add(
                new CalculationValueModel
                    {
                        RowId = currentRowId,
                        ColumnId = "LastYear",
                        TextDisplay = supplier.PrevYearTotal.ToString("C")
                    });

            values.Add(
                new CalculationValueModel
                    {
                        RowId = currentRowId, ColumnId = "ThisYear", TextDisplay = supplier.YearTotal.ToString("C")
                    });

            values.Add(
                new CalculationValueModel
                    {
                        RowId = currentRowId,
                        ColumnId = "ThisMonth",
                        TextDisplay = supplier.MonthTotal.ToString("C")
                    });
        }
    }
}
