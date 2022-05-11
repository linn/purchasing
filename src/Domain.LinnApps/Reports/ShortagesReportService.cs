namespace Linn.Purchasing.Domain.LinnApps.Reports
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.Reports.Models;

    public class ShortagesReportService : IShortagesReportService
    {
        private readonly IRepository<ShortagesEntry, string> shortagesEntryView;

        public ShortagesReportService(
            IRepository<ShortagesEntry, string> shortagesEntryView,
            IReportingHelper reportingHelper)
        {
            this.shortagesEntryView = shortagesEntryView;
        }

        public IEnumerable<ResultsModel> GetReport(
            int? purchaseLevel,
            int? supplier,
            string vendorManager)
        {
            var results = this.shortagesEntryView.FilterBy(x =>
                (x.SupplierId == supplier) && purchaseLevel <= x.PurchaseLevel &&
                (vendorManager == "ALL" || vendorManager == x.VendorManagerCode)).ToList();

            var returnResults = new List<ResultsModel>();

            foreach (var shortagesForPlanner in results.GroupBy(a => new { a.PlannerName}))
            {
                var model = new ResultsModel();
                
                model.AddColumn("VendorManagerCode", "Vendor Manager Code");
                model.AddColumn("VendorManagerName", "Vendor Manager Name");

                model.ReportTitle = new NameModel(
                    $"Purchasing shortages planner");

                model.RowHeader = "Planner";

                var distinctVendorManagers = shortagesForPlanner.DistinctBy(x => x.VendorManagerCode);
                var distinctLevels = shortagesForPlanner.DistinctBy(x => x.PurchaseLevel);

                foreach (var vendorManagerRow in distinctVendorManagers)
                {
                    var row = model.AddRow(vendorManagerRow.VendorManagerCode);
                    
                    model.SetGridTextValue(row.RowIndex, model.ColumnIndex("VendorManagerCode"), vendorManagerRow.VendorManagerCode);
                    model.SetGridTextValue(row.RowIndex, model.ColumnIndex("VendorManagerName"), vendorManagerRow.VendorManagerName);

                    foreach (var level in distinctLevels)
                    {
                        model.AddColumn($"PurchaseLevel{level.PurchaseLevel}", level.PurchaseLevel.ToString(), GridDisplayType.Value);

                        var numberOfShortagesForLevel = shortagesForPlanner.Count(x => x.PurchaseLevel == level.PurchaseLevel && x.VendorManagerCode == vendorManagerRow.VendorManagerCode);

                        model.SetGridValue(row.RowIndex, model.ColumnIndex($"PurchaseLevel{level.PurchaseLevel}"), numberOfShortagesForLevel);
                    }
                }

                returnResults.Add(model);
            }

            return returnResults;
        }
    }
}