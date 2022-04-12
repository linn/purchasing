namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Resources.ReportResultResources;

    public interface IWhatsDueInReportFacadeService
    {
        public IResult<ReportReturnResource> GetReport(
            string fromDate, 
            string toDate, 
            string orderBy, 
            string vendorManager, 
            int? supplier);

        public IEnumerable<IEnumerable<string>> GetReportCsv(
            string fromDate,
            string toDate,
            string orderBy,
            string vendorManager,
            int? supplier);
    }
}
