namespace Linn.Purchasing.Domain.LinnApps.Reports
{
    using Linn.Common.Reporting.Models;

    public class PartsReceivedReportService : IPartsReceivedReportService
    {
        public ResultsModel GetReport(
            string jobref, 
            int? supplier, 
            string fromDate, 
            string toDDate, 
            bool includeNegativeValues)
        {
            throw new System.NotImplementedException();
        }
    }
}
