namespace Linn.Purchasing.Domain.LinnApps.Reports
{
    using System;
    using System.Linq;

    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.Reports.Models;

    public class PartsReceivedReportService : IPartsReceivedReportService
    {
        private readonly IQueryRepository<PartsReceivedViewModel> partsReceivedView;

        public PartsReceivedReportService(
            IQueryRepository<PartsReceivedViewModel> partsReceivedView)
        {
            this.partsReceivedView = partsReceivedView;
        }

        public ResultsModel GetReport(
            string jobref, 
            int? supplier, 
            string fromDate, 
            string toDate, 
            bool includeNegativeValues)
        {
            var test2 = this.partsReceivedView.FilterBy(
                x => x.JobRef == jobref
                     && (!supplier.HasValue || x.SupplierId == supplier)
                     && x.DateBooked >= DateTime.Parse(fromDate).Date
                     && x.DateBooked <= DateTime.Parse(toDate).Date.AddDays(1).AddTicks(-1)).ToList();

            throw new System.NotImplementedException();
        }
    }
}
