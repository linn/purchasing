namespace Linn.Purchasing.Domain.LinnApps.Reports
{
    using Linn.Purchasing.Domain.LinnApps.Reports.Models;

    public class WhatsInInspectionReportService : IWhatsInInspectionReportService
    {
        private readonly IWhatsInInspectionRepository whatsInInspectionRepository;

        public WhatsInInspectionReportService(IWhatsInInspectionRepository whatsInInspectionRepository)
        {
            this.whatsInInspectionRepository = whatsInInspectionRepository;
        }

        public WhatsInInspectionReportModel GetReport(
            bool includePartsWithNoOrderNumber = false,
            bool showStockLocations = true,
            bool includeFailedStock = false,
            bool excludeFinishedGoods = false,
            bool showBackOrdered = true)
        {
            var test = this.whatsInInspectionRepository.GetWhatsInInspection();
            return new WhatsInInspectionReportModel();
        }
    }
}
