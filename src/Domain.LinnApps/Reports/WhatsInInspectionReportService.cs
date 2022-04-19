namespace Linn.Purchasing.Domain.LinnApps.Reports
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.Reports.Models;

    public class WhatsInInspectionReportService : IWhatsInInspectionReportService
    {
        private readonly IWhatsInInspectionRepository whatsInInspectionRepository;

        private readonly IQueryRepository<WhatsInInspectionPurchaseOrdersViewModel> whatsInInspectionPurchaseOrdersViewRepository;

        public WhatsInInspectionReportService(
            IWhatsInInspectionRepository whatsInInspectionRepository,
            IQueryRepository<WhatsInInspectionPurchaseOrdersViewModel> whatsInInspectionPurchaseOrdersViewRepository)
        {
            this.whatsInInspectionRepository = whatsInInspectionRepository;
            this.whatsInInspectionPurchaseOrdersViewRepository = whatsInInspectionPurchaseOrdersViewRepository;
        }

        public IEnumerable<WhatsInInspectionReportModel> GetReport(
            bool includePartsWithNoOrderNumber = false,
            bool showStockLocations = true,
            bool includeFailedStock = false,
            bool includeFinishedGoods = true,
            bool showBackOrdered = true)
        {
            var states = new List<string> { "QC" };
            if (includeFailedStock)
            {
                states.Add("FAIL");
            }

            var parts = this.whatsInInspectionRepository.GetWhatsInInspection(includeFailedStock)
                .Where(m => m.MinDate.HasValue).OrderBy(m => m.MinDate).ToList();

            var orders = this.whatsInInspectionPurchaseOrdersViewRepository.FilterBy(d => states.Contains(d.State)).ToList();

            return parts.Select(p => new WhatsInInspectionReportModel
                                         {
                                             PartNumber = p.PartNumber,
                                             Description = p.Description, 
                                             MinDate = p.MinDate,
                                             QtyInStock = p.QtyInStock,
                                             QtyInInspection = p.QtyInInspection,
                                             OrdersBreakdown = this
                                                 .BuildPurchaseOrdersBreakdown(orders.Where(o => o.PartNumber == p.PartNumber))
                                         });
        }

        private ResultsModel BuildPurchaseOrdersBreakdown(
            IEnumerable<WhatsInInspectionPurchaseOrdersViewModel> models)
        {
            return new ResultsModel();
        }
    }
}
