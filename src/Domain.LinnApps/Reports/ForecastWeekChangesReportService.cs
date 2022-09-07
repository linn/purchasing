namespace Linn.Purchasing.Domain.LinnApps.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Layouts;
    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.Forecasting;

    public class ForecastWeekChangesReportService : IForecastWeekChangesReportService
    {
        private readonly IQueryRepository<ForecastWeekChange> repository;

        private readonly IReportingHelper reportingHelper;

        public ForecastWeekChangesReportService(
            IQueryRepository<ForecastWeekChange> repository,
            IReportingHelper reportingHelper)
        {
            this.repository = repository;
            this.reportingHelper = reportingHelper;
        }

        public ResultsModel GetReport()
        {
            var data = this.repository
                .FilterBy(x => x.LinnWeek.EndsOn > DateTime.Today).OrderBy(x => x.LinnWeekNumber);
            var reportLayout = new SimpleGridLayout(
                this.reportingHelper,
                CalculationValueModelType.Value,
                null,
                "Percentage Changes applied to the coming weeks: ");

            reportLayout.AddColumnComponent(
                null,
                new List<AxisDetailsModel>
                    {
                        new AxisDetailsModel("EndsOn", "Week Ending",  GridDisplayType.TextValue),
                        new AxisDetailsModel("PercentageChange", "% Change",  GridDisplayType.Value)
                            {
                                DecimalPlaces = 2
                            }
                    });
            var values = new List<CalculationValueModel>();
            foreach (var datum in data)
            {
                var rowId = datum.LinnWeekNumber.ToString();
                var endsOn = datum.LinnWeek.EndsOn;
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            ColumnId = "EndsOn",
                            TextDisplay = $"{endsOn.Day}/{endsOn.Month}/{endsOn.Year}"
                        });

                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            ColumnId = "PercentageChange",
                            Value = datum.PercentageChange
                        });
            }

            reportLayout.SetGridData(values);
            return reportLayout.GetResultsModel();
        }
    }
}
