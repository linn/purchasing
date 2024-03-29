﻿namespace Linn.Purchasing.Domain.LinnApps.MaterialRequirements
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Layouts;
    using Linn.Common.Reporting.Models;

    public class MrUsedOnReportService : IMrUsedOnReportService
    {
        private readonly IQueryRepository<MrUsedOnRecord> usedOnView;

        private readonly ISingleRecordRepository<MrMaster> mrMaster;

        private readonly IReportingHelper reportingHelper;

        public MrUsedOnReportService(
            IQueryRepository<MrUsedOnRecord> usedOnView,
            ISingleRecordRepository<MrMaster> mrMaster,
            IReportingHelper reportingHelper)
        {
            this.usedOnView = usedOnView;
            this.mrMaster = mrMaster;
            this.reportingHelper = reportingHelper;
        }

        public ResultsModel GetUsedOn(string partNumber, string jobRef)
        {
            if (string.IsNullOrEmpty(jobRef))
            {
                jobRef = this.mrMaster.GetRecord().JobRef;
            }

            var data = this.usedOnView.FilterBy(
                x => x.JobRef.Equals(jobRef) && x.PartNumber.Equals(partNumber))
                ?.OrderByDescending(x => x.AnnualUsage);

            var reportLayout = new SimpleGridLayout(
                this.reportingHelper,
                CalculationValueModelType.Value,
                null,
                $"Part: {partNumber} - " + $"{(data != null && data.Any() ? data.First().Description : "No results found for part.")}");

            reportLayout.AddColumnComponent(
                null,
                new List<AxisDetailsModel>
                    {
                        new AxisDetailsModel("Assembly", "Assembly",  GridDisplayType.TextValue),
                        new AxisDetailsModel("Description", "Description",  GridDisplayType.TextValue),
                        new AxisDetailsModel("QtyUsed", "QtyUsed", GridDisplayType.Value) { DecimalPlaces = 1 },
                        new AxisDetailsModel("TCoded", "TCoded", GridDisplayType.TextValue),
                        new AxisDetailsModel("AnnualUsage", "AnnualUsage", GridDisplayType.Value) { DecimalPlaces = 1 },
                        new AxisDetailsModel("Mr", string.Empty, GridDisplayType.TextValue)
                    });
            var values = new List<CalculationValueModel>();
            if (data != null)
            {
                foreach (var datum in data)
                {
                    var currentRowId = datum.AssemblyUsedOn;
                    values.Add(
                        new CalculationValueModel
                            {
                                RowId = currentRowId, ColumnId = "Assembly", TextDisplay = datum.AssemblyUsedOn
                            });
                    values.Add(
                        new CalculationValueModel
                            {
                                RowId = currentRowId,
                                ColumnId = "Description",
                                TextDisplay = datum.AssemblyUsedOnDescription
                            });
                    values.Add(
                        new CalculationValueModel { RowId = currentRowId, ColumnId = "QtyUsed", Value = datum.QtyUsed });
                    values.Add(
                        new CalculationValueModel
                            {
                                RowId = currentRowId, ColumnId = "TCoded", TextDisplay = datum.TCoded
                            });
                    values.Add(
                        new CalculationValueModel
                            {
                                RowId = currentRowId, ColumnId = "AnnualUsage", Value = datum.AnnualUsage
                            });
                    values.Add(new CalculationValueModel
                                   {
                                       RowId = currentRowId, ColumnId = "Mr", TextDisplay = "Mr"
                                   });
                }

                reportLayout.AddValueDrillDownDetails(
                    "Mr",
                    $"/purchasing/material-requirements/report?jobRef={jobRef}&partNumber={{rowId}}",
                    null,
                    5,
                    false);
            }

            reportLayout.SetGridData(values);
            return reportLayout.GetResultsModel();
        }
    }
}
