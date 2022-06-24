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
                var model = new ResultsModel();
                model.AddColumn("PartNumber", "Part Number");
                model.AddColumn("Description", "Description");
                model.AddColumn("QtyAvailable", "Qty Available");
                model.AddColumn("TotalWOReqt", "Needed for Works Orders");
                model.AddColumn("TotalBEReqt", "To achieve Minimum Build");
                model.AddColumn("TotalBIReqt", "To achieve Priority Build");
                model.AddColumn("TotalBTReqt", "To achieve Ideal Build");
                model.AddColumn("Supplier", "Supplier");
                model.AddColumn("SupplierName", "Supplier Name");

                model.ReportTitle = new NameModel($"Purchasing danger parts for planner {planner}. Purchase Danger Level {shortagesForPlanner.Key.PurchaseLevel}");
                model.RowHeader = "Planner";

                var distinctPartNumber = shortagesForPlanner.DistinctBy(x => x.PartNumber);
                var distinctLevels = shortagesForPlanner.DistinctBy(x => x.PurchaseLevel).OrderBy(x => x.PurchaseLevel);

                foreach (var partEntryRow in distinctPartNumber)
                {
                    var row = model.AddRow(partEntryRow.PartNumber);

                    model.SetGridTextValue(row.RowIndex, model.ColumnIndex("PartNumber"), partEntryRow.PartNumber);
                    model.SetGridTextValue(row.RowIndex, model.ColumnIndex("Description"), partEntryRow.Description);
                    model.SetGridTextValue(row.RowIndex, model.ColumnIndex("QtyAvailable"), partEntryRow.QtyAvailable.ToString());
                    model.SetGridTextValue(row.RowIndex, model.ColumnIndex("TotalWOReqt"), partEntryRow.TotalWoReqt.ToString());
                    model.SetGridTextValue(row.RowIndex, model.ColumnIndex("TotalBEReqt"), partEntryRow.TotalBeReqt.ToString());
                    model.SetGridTextValue(row.RowIndex, model.ColumnIndex("TotalBIReqt"), partEntryRow.TotalBiReqt.ToString());
                    model.SetGridTextValue(row.RowIndex, model.ColumnIndex("TotalBTReqt"), partEntryRow.TotalBtReqt.ToString());
                    model.SetGridTextValue(row.RowIndex, model.ColumnIndex("Supplier"), partEntryRow.PreferredSupplier.ToString());
                    model.SetGridTextValue(row.RowIndex, model.ColumnIndex("SupplierName"), partEntryRow.SupplierName);
                }

                returnResults.Add(model);
            }

            return returnResults;
        }
    }
}
