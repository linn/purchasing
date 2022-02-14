namespace Linn.Purchasing.Domain.LinnApps.Reports
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Layouts;
    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

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

        public ResultsModel GetSpendBySupplierReport(string vendorManagerId)
        {
            var currentLedgerPeriod = this.purchaseLedgerPack.GetLedgerPeriod();
            var yearStartLedgerPeriod = this.purchaseLedgerPack.GetYearStartLedgerPeriod();
            var previousYearStartLedgerPeriod = yearStartLedgerPeriod - 12;

            var supplierSpends = this.spendsRepository.FilterBy(
                        x => 
                            x.Supplier.VendorManager != null &&
                            x.LedgerPeriod >= previousYearStartLedgerPeriod && x.LedgerPeriod <= currentLedgerPeriod
                             && (string.IsNullOrWhiteSpace(vendorManagerId) 
                                 || x.Supplier.VendorManager.VmId == vendorManagerId))
                    .ToList();

            var vendorManagerName = "ALL";

            if (!string.IsNullOrWhiteSpace(vendorManagerId))
            {
                var vendorManager = this.vendorManagerRepository.FindById(vendorManagerId);
                vendorManagerName = $"{vendorManagerId} - {vendorManager.Employee.FullName} ({vendorManager.UserNumber})";
            }

            var reportLayout = new SimpleGridLayout(
                this.reportingHelper,
                CalculationValueModelType.TextValue,
                null,
                $"Spend by supplier report for Vendor Manager: {vendorManagerName}. For this financial year and last, excludes factors & VAT.");

            AddSupplierReportColumns(reportLayout);

            var values = new List<CalculationValueModel>();

            var distinctSupplierSpends = supplierSpends.DistinctBy(x => x.SupplierId).Select(
                x => new SupplierSpendWithTotals
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
                ExtractDetailsForPartReport(values, supplier);
            }

            reportLayout.SetGridData(values);
            var model = reportLayout.GetResultsModel();

            return model;
        }

        private static void AddSupplierReportColumns(SimpleGridLayout reportLayout)
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

        private static void ExtractDetailsForPartReport(ICollection<CalculationValueModel> values, SupplierSpendWithTotals supplier)
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

            var culture = CultureInfo.CreateSpecificCulture("en-GB");
            values.Add(
                new CalculationValueModel
                    {
                        RowId = currentRowId,
                        ColumnId = "LastYear",
                        TextDisplay = supplier.PrevYearTotal.ToString("C", culture)
                    });

            values.Add(
                new CalculationValueModel
                    {
                        RowId = currentRowId, ColumnId = "ThisYear", TextDisplay = supplier.YearTotal.ToString("C", culture)
                    });

            values.Add(
                new CalculationValueModel
                    {
                        RowId = currentRowId,
                        ColumnId = "ThisMonth",
                        TextDisplay = supplier.MonthTotal.ToString("C", culture)
                    });
        }
    }
}
