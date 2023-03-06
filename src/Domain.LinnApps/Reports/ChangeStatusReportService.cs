using Linn.Common.Reporting.Layouts;
using Linn.Purchasing.Domain.LinnApps.Boms;

namespace Linn.Purchasing.Domain.LinnApps.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Models;

    public class ChangeStatusReportService : IChangeStatusReportService
    {
        private readonly IReportingHelper reportingHelper;

        private readonly IQueryRepository<ChangeRequest> changeRequests;

        public ChangeStatusReportService(
            IQueryRepository<ChangeRequest> changeRequests,
            IReportingHelper reportingHelper
            )
        {
            this.changeRequests = changeRequests;
            this.reportingHelper = reportingHelper;
        }

        public ResultsModel GetChangeStatusReport(
            int months)
        {
            var acceptedChangeRequests = this.changeRequests.FilterBy(x =>
                x.DateAccepted >= DateTime.Today.AddMonths(-months) && x.ChangeState == "ACCEPT").Count();

            var proposedChangeRequests = this.changeRequests.FilterBy(x =>
                x.DateAccepted >= DateTime.Today.AddMonths(-months) && x.ChangeState == "PROPOS").Count();

            var reportLayout = new SimpleGridLayout(this.reportingHelper, CalculationValueModelType.Value, null, null);

            reportLayout.AddColumnComponent(
                null,
                new List<AxisDetailsModel>
                    {
                        new("Count", "Count",  GridDisplayType.Value),
                        new("State", "State", GridDisplayType.TextValue),
                    });

            var values = new List<CalculationValueModel>
            {
                new CalculationValueModel
                {
                    RowId = "1",
                    ColumnId = "Count",
                    Value = acceptedChangeRequests
                },
                new CalculationValueModel
                {
                    RowId = "1",
                    ColumnId = "State",
                    TextDisplay = "ACCEPT ACCEPTED CHANGES"
                },
                new CalculationValueModel
                {
                    RowId = "2",
                    ColumnId = "Count",
                    Value = proposedChangeRequests
                },
                new CalculationValueModel
                {
                    RowId = "2",
                    ColumnId = "State",
                    TextDisplay = "PROPOS PROPOSED CHANGES",
                },
                new CalculationValueModel
                {
                    RowId = "3",
                    ColumnId = "Count",
                    Value = acceptedChangeRequests + proposedChangeRequests
                },
                new CalculationValueModel
                {
                    RowId = "3",
                    ColumnId = "State",
                    TextDisplay = "TOTAL OUTSTANDING CHANGES",
                }
            };
            reportLayout.AddRowDrillDownDetails("1", "purchasing/reports/accepted-changes/report?months={{months}}");
            reportLayout.AddRowDrillDownDetails("2", "purchasing/reports/proposed-changes/report?months={{months}}");
            reportLayout.AddRowDrillDownDetails("3", "purchasing/reports/outstanding-changes/report?months={{months}}");

            reportLayout.ReportTitle = "Change Status Report";
            reportLayout.SetGridData(values);
            return reportLayout.GetResultsModel();
        }

        public ResultsModel GetAcceptedChangesReport(int months)
        {
            var lines = this.changeRequests.FilterBy(x =>
                (x.DateAccepted >= DateTime.Today.AddMonths(-months)) && x.ChangeState == "ACCEPT");

            var reportLayout = new SimpleGridLayout(this.reportingHelper, CalculationValueModelType.Value, null, null);

            reportLayout.AddColumnComponent(
                null,
                new List<AxisDetailsModel>
                    {
                        new("CRFNo", "CRF No.",  GridDisplayType.TextValue),
                        new("State", "State", GridDisplayType.TextValue),
                        new("DateEntered", "Date Entered",  GridDisplayType.TextValue),
                        new("EnteredBy", "Entered By",  GridDisplayType.TextValue),
                        new("Scope", "Scope", GridDisplayType.TextValue),
                        new("ReasonForChange", "ReasonForChange", GridDisplayType.TextValue)
                    });

            var values = new List<CalculationValueModel>();

            foreach (var line in lines)
            {
                var rowId = line.DocumentNumber.ToString();
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId,
                        ColumnId = "CRFNo",
                        Value = line.DocumentNumber
                    });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId,
                        ColumnId = "State",
                        TextDisplay = line.ChangeState
                    });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId,
                        ColumnId = "DateEntered",
                        TextDisplay = line.DateEntered.ToShortDateString()
                    });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId,
                        ColumnId = "EnteredBy",
                        TextDisplay = line.EnteredBy.FullName
                    });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId,
                        ColumnId = "Scope",
                        TextDisplay = line.ChangeRequestType
                    });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId,
                        ColumnId = "ReasonForChange",
                        TextDisplay = line.ReasonForChange
                    });
            }
            reportLayout.AddValueDrillDownDetails(
                "CRFNo",
                $"/purchasing/change-requests/{{rowId}}",
                null,
                0,
                false);
            reportLayout.ReportTitle = "Change Status : Accepted Changes";
            reportLayout.SetGridData(values);
            return reportLayout.GetResultsModel();
        }

        public ResultsModel GetProposedChangesReport(int months)
        {
            var lines = this.changeRequests.FilterBy(x =>
                x.DateAccepted >= DateTime.Today.AddMonths(-months) && x.ChangeState == "PROPOS");

            var reportLayout = new SimpleGridLayout(this.reportingHelper, CalculationValueModelType.Value, null, null);

            reportLayout.AddColumnComponent(
                null,
                new List<AxisDetailsModel>
                    {
                        new("CRFNo", "CRF No.",  GridDisplayType.TextValue),
                        new("State", "State", GridDisplayType.TextValue),
                        new("DateEntered", "Date Entered",  GridDisplayType.TextValue),
                        new("EnteredBy", "Entered By",  GridDisplayType.TextValue),
                        new("Scope", "Scope", GridDisplayType.TextValue),
                        new("ReasonForChange", "ReasonForChange", GridDisplayType.TextValue)
                    });

            var values = new List<CalculationValueModel>();

            foreach (var line in lines)
            {
                var rowId = line.DocumentNumber.ToString();
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId,
                        ColumnId = "CRFNo",
                        Value = line.DocumentNumber
                    });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId,
                        ColumnId = "State",
                        TextDisplay = line.ChangeState
                    });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId,
                        ColumnId = "DateEntered",
                        TextDisplay = line.DateEntered.ToShortDateString()
                    });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId,
                        ColumnId = "EnteredBy",
                        TextDisplay = line.EnteredBy.FullName
                    });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId,
                        ColumnId = "Scope",
                        TextDisplay = line.ChangeRequestType
                    });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId,
                        ColumnId = "ReasonForChange",
                        TextDisplay = line.ReasonForChange
                    });
            }
            reportLayout.AddValueDrillDownDetails(
                "CRFNo",
                $"/purchasing/change-requests/{{rowId}}",
                null,
                0,
                false);
            reportLayout.ReportTitle = "Change Status : Proposed Changes";
            reportLayout.SetGridData(values);
            return reportLayout.GetResultsModel();
        }

        public ResultsModel GetTotalOutstandingChangesReport(int months)
        {
            var lines = this.changeRequests.FilterBy(x =>
                x.DateAccepted >= DateTime.Today.AddMonths(-months) && (x.ChangeState == "PROPOS" || x.ChangeState == "ACCEPT"));

            var reportLayout = new SimpleGridLayout(this.reportingHelper, CalculationValueModelType.Value, null, null);

            reportLayout.AddColumnComponent(
                null,
                new List<AxisDetailsModel>
                    {
                        new("CRFNo", "CRF No.",  GridDisplayType.TextValue),
                        new("State", "State", GridDisplayType.TextValue),
                        new("DateEntered", "Date Entered",  GridDisplayType.TextValue),
                        new("EnteredBy", "Entered By",  GridDisplayType.TextValue),
                        new("Scope", "Scope", GridDisplayType.TextValue),
                        new("ReasonForChange", "ReasonForChange", GridDisplayType.TextValue)
                    });

            var values = new List<CalculationValueModel>();

            foreach (var line in lines)
            {
                var rowId = line.DocumentNumber.ToString();
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId,
                        ColumnId = "CRFNo",
                        Value = line.DocumentNumber
                    });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId,
                        ColumnId = "State",
                        TextDisplay = line.ChangeState
                    });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId,
                        ColumnId = "DateEntered",
                        TextDisplay = line.DateEntered.ToShortDateString()
                    });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId,
                        ColumnId = "EnteredBy",
                        TextDisplay = line.EnteredBy.FullName
                    });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId,
                        ColumnId = "Scope",
                        TextDisplay = line.ChangeRequestType
                    });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId,
                        ColumnId = "ReasonForChange",
                        TextDisplay = line.ReasonForChange
                    });
            }
            reportLayout.AddValueDrillDownDetails(
                "CRFNo",
                $"/purchasing/change-requests/{{rowId}}",
                null,
                0,
                false);
            reportLayout.ReportTitle = "Change Status : Total Outstanding Changes";
            reportLayout.SetGridData(values);
            return reportLayout.GetResultsModel();
        }
    }
}

