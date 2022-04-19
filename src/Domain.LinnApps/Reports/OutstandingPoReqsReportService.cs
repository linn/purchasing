namespace Linn.Purchasing.Domain.LinnApps.Reports
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Layouts;
    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrderReqs;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;

    using MoreLinq;

    public class OutstandingPoReqsReportService : IOutstandingPoReqsReportService
    {
        private readonly IRepository<PurchaseOrderReq, int> reqRepository;

        private readonly IReportingHelper reportingHelper;

        public OutstandingPoReqsReportService(
            IRepository<PurchaseOrderReq, int> reqRepository,
            IReportingHelper reportingHelper)
        {
            this.reqRepository = reqRepository;
            this.reportingHelper = reportingHelper;
        }

        public ResultsModel GetReport(string state)
        {
            var data = this.reqRepository.FilterBy(x => x.ReqState.IsFinalState.Equals("N"));

            if (!string.IsNullOrEmpty(state))
            {
                data = data.Where(x => x.ReqState.State.Equals(state));
            }

            data = data.OrderBy(x => x.ReqNumber);

            var reportLayout = new SimpleGridLayout(
                this.reportingHelper,
                CalculationValueModelType.Value,
                null,
                $"{state} Outstanding PO Reqs");

            reportLayout.AddColumnComponent(
                null,
                new List<AxisDetailsModel>
                    {
                        new AxisDetailsModel("ReqState", "State",  GridDisplayType.TextValue),
                        new AxisDetailsModel("Date", "Date", GridDisplayType.TextValue),
                        new AxisDetailsModel("RequestedBy", "Requested By", GridDisplayType.TextValue),
                        new AxisDetailsModel("PartNumber", "Part", GridDisplayType.TextValue),
                        new AxisDetailsModel("Description", "Description", GridDisplayType.TextValue),
                        new AxisDetailsModel("SupplierId", "SupplierId", GridDisplayType.TextValue),
                        new AxisDetailsModel("SupplierName", "SupplierName", GridDisplayType.TextValue),
                        new AxisDetailsModel("Qty", "Qty", GridDisplayType.Value) { DecimalPlaces = 0 },
                        new AxisDetailsModel("UnitPrice", "Unit Price", GridDisplayType.Value) { DecimalPlaces = 2 },
                        new AxisDetailsModel("Carriage", "Carriage",  GridDisplayType.Value) { DecimalPlaces = 2 },
                        new AxisDetailsModel("TotalPrice", "Total Price", GridDisplayType.Value) { DecimalPlaces = 2 }
                    });
            var values = new List<CalculationValueModel>();
            foreach (var datum in data)
            {
                var rowId = datum.ReqNumber.ToString();
             
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            ColumnId = "ReqState",
                            TextDisplay = datum.ReqState.State
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            ColumnId = "Date",
                            TextDisplay = datum.ReqDate.ToShortDateString()
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            ColumnId = "RequestedBy",
                            TextDisplay = datum.RequestedBy?.FullName
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            ColumnId = "PartNumber",
                            TextDisplay = datum.PartNumber
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            ColumnId = "Description",
                            TextDisplay = datum.Description.Length > 199 
                                              ? datum.Description.Substring(0, 200) : datum.Description
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            ColumnId = "SupplierId",
                            TextDisplay = datum.SupplierId.ToString()
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            ColumnId = "SupplierName",
                            TextDisplay = datum.SupplierName
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            ColumnId = "Qty",
                            Value = datum.Qty
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            ColumnId = "UnitPrice",
                            Value = datum.UnitPrice
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            ColumnId = "Carriage",
                            Value = datum.Carriage ?? 0m
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            ColumnId = "TotalPrice",
                            Value = datum.TotalReqPrice ?? 0m
                        });
            }

            reportLayout.SetGridData(values);

            var model = reportLayout.GetResultsModel();

            model.RowDrillDownTemplates.Add(new DrillDownModel("ReqNumber", "/purchasing/purchase-orders/reqs/{textValue}"));
            model.RowHeader = "Req";

            return model;
        }
    }
}
