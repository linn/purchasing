namespace Linn.Purchasing.Domain.LinnApps.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.Reports.Models;

    public class ShortagesReportService : IShortagesReportService
    {
        private readonly IQueryRepository<ShortagesEntry> shortagesEntryView;

        private readonly IQueryRepository<ShortagesPlannerEntry> shortagesPlannerEntryView;

        public ShortagesReportService(
            IQueryRepository<ShortagesEntry> shortagesEntryView,
            IQueryRepository<ShortagesPlannerEntry> shortagesPlannerEntryView)
        {
            this.shortagesEntryView = shortagesEntryView;
            this.shortagesPlannerEntryView = shortagesPlannerEntryView;
        }

        public IEnumerable<ResultsModel> GetShortagesReport(
            int purchaseLevel,
            string vendorManager)
        {
            var results = this.shortagesEntryView.FilterBy(x =>
                x.PurchaseLevel <= purchaseLevel && (vendorManager == "ALL" || vendorManager == x.VendorManagerCode)).ToList();

            var returnResults = new List<ResultsModel>();

            foreach (var shortagesForPlanner in results.GroupBy(a => new { a.PlannerName }))
            {
                var model = new ResultsModel();
                model.AddColumn("VendorManagerCode", "Vendor Manager Code");
                model.AddColumn("VendorManagerName", "Vendor Manager Name");

                var titleModel = new NameModel(shortagesForPlanner.Key.PlannerName);
                titleModel.DrillDownList = new List<DrillDownModel>
                                               {
                                                   new DrillDownModel(
                                                       "Shortages",
                                                       $"/purchasing/reports/shortages-planner/report?planner={shortagesForPlanner.FirstOrDefault()?.Planner}")
                                               };
                model.ReportTitle = titleModel;

                var distinctVendorManagers = shortagesForPlanner.DistinctBy(x => x.VendorManagerCode);
                var distinctLevels = shortagesForPlanner.DistinctBy(x => x.PurchaseLevel).OrderBy(x => x.PurchaseLevel);

                foreach (var level in distinctLevels)
                {
                    model.AddColumn(
                        $"PurchaseLevel{level.PurchaseLevel}",
                        level.PurchaseLevel.ToString(),
                        GridDisplayType.Value);
                }

                foreach (var vendorManagerRow in distinctVendorManagers)
                {
                    var row = model.AddRow(vendorManagerRow.VendorManagerCode);

                    model.SetGridTextValue(row.RowIndex, model.ColumnIndex("VendorManagerCode"), vendorManagerRow.VendorManagerCode);
                    model.SetGridTextValue(row.RowIndex, model.ColumnIndex("VendorManagerName"), vendorManagerRow.VendorManagerName);

                    foreach (var level in distinctLevels)
                    {
                        var numberOfShortagesForLevel = shortagesForPlanner.Count(x => x.PurchaseLevel == level.PurchaseLevel && x.VendorManagerCode == vendorManagerRow.VendorManagerCode);
                        model.SetGridValue(row.RowIndex, model.ColumnIndex($"PurchaseLevel{level.PurchaseLevel}"), numberOfShortagesForLevel);
                    }
                }

                returnResults.Add(model);
            }

            return returnResults;
        }

        public IEnumerable<ResultsModel> GetShortagesPlannerReport(int planner)
        {
            var results = this.shortagesPlannerEntryView.FilterBy(x => x.Planner.HasValue && x.Planner.Value == planner).ToList();
            var returnResults = new List<ResultsModel>();

            foreach (var shortagesForPlanner in results.GroupBy(a => new { a.PurchaseLevel }))
            {
                var titleModel = new ResultsModel
                {
                    ReportTitle = new NameModel($"Purchasing danger parts for planner {planner}. Purchase Danger Level {shortagesForPlanner.Key.PurchaseLevel}")
                };
                returnResults.Add(titleModel);

                var distinctPartNumber = shortagesForPlanner.DistinctBy(x => x.PartNumber);

                foreach (var partEntryRow in distinctPartNumber)
                {
                    var model = new ResultsModel();
                    var reportTitle = new NameModel(partEntryRow.PartNumber);
                    reportTitle.DrillDownList = new List<DrillDownModel>
                                                    {
                                                        new DrillDownModel(
                                                            "MR",
                                                            $"/purchasing/material-requirements/report?partNumber={partEntryRow.PartNumber}")
                                                    };
                    model.ReportTitle = reportTitle;
                    model.AddColumn("Description", "Description");
                    model.AddColumn("QtyAvailable", "Qty Available");
                    model.AddColumn("TotalWOReqt", "Needed for Works Orders");
                    model.AddColumn("TotalBEReqt", "To achieve Minimum Build");
                    model.AddColumn("TotalBIReqt", "To achieve Priority Build");
                    model.AddColumn("TotalBTReqt", "To achieve Ideal Build");
                    model.AddColumn("Supplier", "Supplier");
                    model.AddColumn("SupplierName", "Supplier Name");

                    var row = model.AddRow(partEntryRow.PartNumber);

                    model.SetGridTextValue(row.RowIndex, model.ColumnIndex("Description"), partEntryRow.Description);
                    model.SetGridTextValue(row.RowIndex, model.ColumnIndex("QtyAvailable"), partEntryRow.QtyAvailable.ToString());
                    model.SetGridTextValue(row.RowIndex, model.ColumnIndex("TotalWOReqt"), partEntryRow.TotalWoReqt.ToString());
                    model.SetGridTextValue(row.RowIndex, model.ColumnIndex("TotalBEReqt"), partEntryRow.TotalBeReqt.ToString());
                    model.SetGridTextValue(row.RowIndex, model.ColumnIndex("TotalBIReqt"), partEntryRow.TotalBiReqt.ToString());
                    model.SetGridTextValue(row.RowIndex, model.ColumnIndex("TotalBTReqt"), partEntryRow.TotalBtReqt.ToString());
                    model.SetGridTextValue(row.RowIndex, model.ColumnIndex("Supplier"), partEntryRow.PreferredSupplier.ToString());
                    model.SetGridTextValue(row.RowIndex, model.ColumnIndex("SupplierName"), partEntryRow.SupplierName);

                    returnResults.Add(model);

                    var expectedDeliveriesModel = new ResultsModel
                    {
                        ReportTitle = new NameModel("Expected Deliveries")
                    };
                    expectedDeliveriesModel.AddColumn("OrderNumber", "Order Number");
                    expectedDeliveriesModel.AddColumn("OrderLine", "Line");
                    expectedDeliveriesModel.AddColumn("DeliverySeq", "Del");
                    expectedDeliveriesModel.AddColumn("RequestedDate", "Requested");
                    expectedDeliveriesModel.AddColumn("AdvisedDate", "Advised");


                    foreach (var deliveryRow in shortagesForPlanner.Where(x => x.PartNumber == partEntryRow.PartNumber))
                    {
                        var expectedDeliveryRow = expectedDeliveriesModel.AddRow($"{deliveryRow.OrderNumber}/{deliveryRow.OrderNumber}/{deliveryRow.OrderLine}/{deliveryRow.DeliverySeq}");

                        expectedDeliveriesModel.SetGridTextValue(expectedDeliveryRow.RowIndex, expectedDeliveriesModel.ColumnIndex("OrderNumber"), deliveryRow.OrderNumber.ToString());
                        expectedDeliveriesModel.SetGridTextValue(expectedDeliveryRow.RowIndex, expectedDeliveriesModel.ColumnIndex("OrderLine"), deliveryRow.OrderLine.ToString());
                        expectedDeliveriesModel.SetGridTextValue(expectedDeliveryRow.RowIndex, expectedDeliveriesModel.ColumnIndex("DeliverySeq"), deliveryRow.DeliverySeq.ToString()); 
                        expectedDeliveriesModel.SetGridTextValue(expectedDeliveryRow.RowIndex, expectedDeliveriesModel.ColumnIndex("RequestedDate"), deliveryRow.RequestedDate.ToString());
                        expectedDeliveriesModel.SetGridTextValue(expectedDeliveryRow.RowIndex, expectedDeliveriesModel.ColumnIndex("AdvisedDate"), deliveryRow.AdvisedDate.ToString());
                    }

                    returnResults.Add(expectedDeliveriesModel);
                }
            }

            return returnResults;
        }
    }
}
