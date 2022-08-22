namespace Linn.Purchasing.Domain.LinnApps.Reports
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Layouts;
    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.Reports.Models;

    public class LeadTimesReportService : ILeadTimesReportService
    {
        private readonly IQueryRepository<SuppliersLeadTimesEntry> leadTimesEntryView;

        private readonly IReportingHelper reportingHelper;

        public LeadTimesReportService(IQueryRepository<SuppliersLeadTimesEntry> leadTimesEntryView, IReportingHelper reportingHelper)
        {
            this.leadTimesEntryView = leadTimesEntryView;
            this.reportingHelper = reportingHelper;
        }

        public ResultsModel GetLeadTimesBySupplier(int supplier)
        {
            var results = this.leadTimesEntryView.FilterBy(
                    x => (x.SupplierId == supplier)).OrderBy(s => s.PartNumber);

            var reportLayout = new SimpleGridLayout(
                this.reportingHelper,
                CalculationValueModelType.Value,
                null,
                null);

            var values = new List<CalculationValueModel>();

            foreach (var result in results)
            {
                var rowId = $"{result.PartNumber}";
                        values.Add(
                            new CalculationValueModel
                            {
                                RowId = rowId,
                                ColumnId = "PartNumber",
                                TextDisplay = result.PartNumber
                            });
                        values.Add(
                            new CalculationValueModel
                            {
                                RowId = rowId,
                                ColumnId = "LeadTime",
                                Value = result.LeadTimeWeeks
                            });
            }

            reportLayout.SetGridData(values);

            reportLayout.ReportTitle = $"Lead Times for Supplier : {supplier}";
            var model = reportLayout.GetResultsModel();

            return model;
        }
    }
}
