namespace Linn.Purchasing.Domain.LinnApps.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Models;
    using Linn.Common.Reporting.Layouts;
    using Linn.Purchasing.Domain.LinnApps.MaterialRequirements;
    using Linn.Purchasing.Domain.LinnApps.Boms;

    public class ChangeStatusReportService : IChangeStatusReportService
    {
        private readonly IReportingHelper reportingHelper;

        private readonly IQueryRepository<ChangeRequest> changeRequests;

        private readonly IQueryRepository<ChangeRequestPhaseInWeeksView> changeRequestPhaseInWeeksViewRepository;

        private readonly IRepository<LinnWeek, int> weekRepository;

        private readonly IRepository<Employee, int> employeeRepository;

        public ChangeStatusReportService(
            IQueryRepository<ChangeRequest> changeRequests,
            IRepository<LinnWeek, int> weekRepository,
            IRepository<Employee, int> employeeRepository,
            IQueryRepository<ChangeRequestPhaseInWeeksView> changeRequestPhaseInWeeksViewRepository,
            IReportingHelper reportingHelper
            )
        {
            this.changeRequests = changeRequests;
            this.employeeRepository = employeeRepository;
            this.weekRepository = weekRepository;
            this.changeRequestPhaseInWeeksViewRepository = changeRequestPhaseInWeeksViewRepository;
            this.reportingHelper = reportingHelper;
        }

        public ResultsModel GetChangeStatusReport(int months)
        {
            var acceptedChangeRequests = this.changeRequests.FindAll().Where(x =>
                x.DateAccepted >= DateTime.Today.AddMonths(-months) && x.ChangeState == "ACCEPT").Count();

            var proposedChangeRequests = this.changeRequests.FindAll().Where(x =>
                x.DateEntered >= DateTime.Today.AddMonths(-months) && x.ChangeState == "PROPOS").Count();

            var changeRequestsPhaseInWeeks = this.changeRequestPhaseInWeeksViewRepository.FindAll().Where(x =>
                x.DateAccepted >= DateTime.Today.AddMonths(-months)).Count();

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
                },
                new CalculationValueModel 
                {
                    RowId = "4",
                    ColumnId = "Count",
                    Value = changeRequestsPhaseInWeeks
                },
                new CalculationValueModel
                {
                    RowId = "4",
                    ColumnId = "State",
                    TextDisplay = "CHANGES WITH CURRENT PHASE IN WEEK",
                }
            };

            reportLayout.AddValueDrillDownDetails(
                "AcceptedChanges", 
                $"/purchasing/reports/accepted-changes/report?months={months}", 
                0, 
                1, 
                false);

            reportLayout.AddValueDrillDownDetails(
                "ProposedChanges",
                $"/purchasing/reports/proposed-changes/report?months={months}",
                1, 
                1, 
                false);

            reportLayout.AddValueDrillDownDetails(
                "OutstandingChanges", 
                $"/purchasing/reports/outstanding-changes/report?months={months}",
                2, 
                1, 
                false);

            reportLayout.AddValueDrillDownDetails(
                "CurrentPhaseInWeeks",
                $"/purchasing/reports/current-phase-in-weeks/report?months={months}",
                3,
                1,
                false);

            reportLayout.ReportTitle = "Change Status Report";
            reportLayout.SetGridData(values);

            return reportLayout.GetResultsModel();
        }

        public ResultsModel GetAcceptedChangesReport(int months)
        {
            var lines = this.changeRequests.FindAll().Where(x =>
                x.DateAccepted >= DateTime.Today.AddMonths(-months) && (x.ChangeState == "ACCEPT"));

            var reportLayout = new SimpleGridLayout(this.reportingHelper, CalculationValueModelType.Value, null, null);

            reportLayout.AddColumnComponent(
                null,
                new List<AxisDetailsModel>
                    {
                        new("CRFNo", "CRF No.",  GridDisplayType.TextValue),
                        new("State", "State", GridDisplayType.TextValue),
                        new("EnteredBy", "Entered By",  GridDisplayType.TextValue),
                        new("OldPart", "Old Part Number", GridDisplayType.TextValue),
                        new("NewPart", "New Part Number", GridDisplayType.TextValue),
                        new("OldPartStock", "Stock of Old Part", GridDisplayType.Value) ,
                        new("ReasonForChange", "Reason For Change", GridDisplayType.TextValue)
                    });

            var values = new List<CalculationValueModel>();

            foreach (var line in lines)
            {
                var employee = this.employeeRepository.FindById(line.EnteredById);
                var rowId = line.DocumentNumber.ToString();

                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId,
                        ColumnId = "CRFNo",
                        TextDisplay = line.DocumentNumber.ToString()
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
                        ColumnId = "EnteredBy",
                        TextDisplay = employee?.FullName
                    });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId,
                        ColumnId = "OldPart",
                        TextDisplay = line.OldPartNumber
                    });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId,
                        ColumnId = "NewPart",
                        TextDisplay = line.NewPartNumber
                    });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId,
                        ColumnId = "OldPartStock",
                        Value = 0
                    });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId,
                        ColumnId = "NewPart",
                        TextDisplay = line.NewPartNumber
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
            var lines = this.changeRequests.FindAll().Where(x =>
                x.DateEntered >= DateTime.Today.AddMonths(-months) && x.ChangeState == "PROPOS");

            var reportLayout = new SimpleGridLayout(this.reportingHelper, CalculationValueModelType.Value, null, null);

            reportLayout.AddColumnComponent(
                null,
                new List<AxisDetailsModel>
                    {
                        new("CRFNo", "CRF No.",  GridDisplayType.TextValue),
                        new("State", "State", GridDisplayType.TextValue),
                        new("EnteredBy", "Entered By",  GridDisplayType.TextValue),
                        new("OldPart", "Old Part Number", GridDisplayType.TextValue),
                        new("NewPart", "New Part Number", GridDisplayType.TextValue),
                        new("OldPartStock", "Stock of Old Part", GridDisplayType.Value) ,
                        new("ReasonForChange", "Reason For Change", GridDisplayType.TextValue)
                    });

            var values = new List<CalculationValueModel>();

            foreach (var line in lines)
            {
                var employee = this.employeeRepository.FindById(line.EnteredById);
                var rowId = line.DocumentNumber.ToString();

                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId,
                        ColumnId = "CRFNo",
                        TextDisplay = line.DocumentNumber.ToString()
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
                        ColumnId = "EnteredBy",
                        TextDisplay = employee?.FullName
                    });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId,
                        ColumnId = "OldPart",
                        TextDisplay = line.OldPartNumber
                    });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId,
                        ColumnId = "NewPart",
                        TextDisplay = line.NewPartNumber
                    });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId,
                        ColumnId = "OldPartStock",
                        Value = 0
                    });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId,
                        ColumnId = "NewPart",
                        TextDisplay = line.NewPartNumber
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
            var lines = this.changeRequests.FindAll().Where(x =>
                x.DateEntered >= DateTime.Today.AddMonths(-months) && (x.ChangeState == "PROPOS" || x.ChangeState == "ACCEPT"));

            var reportLayout = new SimpleGridLayout(this.reportingHelper, CalculationValueModelType.Value, null, null);

            reportLayout.AddColumnComponent(
                null,
                new List<AxisDetailsModel>
                    {
                        new("CRFNo", "CRF No.",  GridDisplayType.TextValue),
                        new("State", "State", GridDisplayType.TextValue),
                        new("EnteredBy", "Entered By",  GridDisplayType.TextValue),
                        new("OldPart", "Old Part Number", GridDisplayType.TextValue),
                        new("NewPart", "New Part Number", GridDisplayType.TextValue),
                        new("OldPartStock", "Stock of Old Part", GridDisplayType.Value) ,
                        new("ReasonForChange", "Reason For Change", GridDisplayType.TextValue)
                    });

            var values = new List<CalculationValueModel>();

            foreach (var line in lines)
            {
                var employee = this.employeeRepository.FindById(line.EnteredById);
                var rowId = line.DocumentNumber.ToString();

                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId,
                        ColumnId = "CRFNo",
                        TextDisplay = line.DocumentNumber.ToString()
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
                        ColumnId = "EnteredBy",
                        TextDisplay = employee?.FullName
                    });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId,
                        ColumnId = "OldPart",
                        TextDisplay = line.OldPartNumber
                    });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId,
                        ColumnId = "NewPart",
                        TextDisplay = line.NewPartNumber
                    });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId,
                        ColumnId = "OldPartStock",
                        Value = 0
                    });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId,
                        ColumnId = "NewPart",
                        TextDisplay = line.NewPartNumber
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

        public ResultsModel GetCurrentPhaseInWeeksReport(int months)
        {
            var lines = this.changeRequestPhaseInWeeksViewRepository.FindAll().Where(x =>
                x.DateAccepted >= DateTime.Today.AddMonths(-months)).OrderBy(x => x.PhaseInWeek);

            var reportLayout = new SimpleGridLayout(this.reportingHelper, CalculationValueModelType.Value, null, null);

            reportLayout.AddColumnComponent(
                null,
                new List<AxisDetailsModel>
                    {
                        new("PhaseInWeek", "Phase In Week",  GridDisplayType.TextValue),
                        new("DocumentNumber", "Document Number", GridDisplayType.TextValue),
                        new("DisplayName", "What",  GridDisplayType.TextValue),
                        new("OldPartNumber", "Old Part", GridDisplayType.TextValue),
                        new("OldPartStock", "Old Part Stock", GridDisplayType.TextValue),
                        new("NewPartNumber", "New Part", GridDisplayType.Value) ,
                        new("NewPartStock", "New Part Stock", GridDisplayType.TextValue),
                        new("DescriptionOfChange", "Description Of Change", GridDisplayType.TextValue),
                        new("ReasonForChange", "Reason For Change", GridDisplayType.TextValue),
                        new("Notes", "Notes", GridDisplayType.TextValue)
                    });

            var values = new List<CalculationValueModel>();

            foreach (var line in lines)
            {
                var rowId = line.DocumentNumber.ToString();

                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId,
                        ColumnId = "PhaseInWeek",
                        TextDisplay = line.PhaseInWeek
                    });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId,
                        ColumnId = "DocumentNumber",
                        TextDisplay = line.DocumentNumber.ToString()
                    });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId,
                        ColumnId = "DisplayName",
                        TextDisplay = line.DisplayName
                    });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId,
                        ColumnId = "OldPart",
                        TextDisplay = line.OldPartNumber
                    });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId,
                        ColumnId = "OldPartStock",
                        TextDisplay = line.OldPartStock.ToString()
                    });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId,
                        ColumnId = "NewPartNumber",
                        TextDisplay = line.NewPartNumber
                    });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId,
                        ColumnId = "NewPartStock",
                        TextDisplay = line.NewPartStock.ToString()
                    });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId,
                        ColumnId = "DescriptionOfChange",
                        TextDisplay = line.DescriptionOfChange
                    });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            ColumnId = "ReasonForChange",
                            TextDisplay = line.ReasonForChange
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            ColumnId = "Notes",
                            TextDisplay = line.Notes
                        });
            }
            reportLayout.AddValueDrillDownDetails(
                "CRFNo",
                $"/purchasing/change-requests/{{rowId}}",
                null,
                0,
                false);
            reportLayout.ReportTitle = "Change Status : Changes with current Phase In Week";
            reportLayout.SetGridData(values);

            return reportLayout.GetResultsModel();
        }
    }
}

