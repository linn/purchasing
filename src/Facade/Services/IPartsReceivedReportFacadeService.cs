namespace Linn.Purchasing.Facade.Services
{
    using System.IO;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Purchasing.Resources.RequestResources;

    public interface IPartsReceivedReportFacadeService
    {
        public IResult<ReportReturnResource> GetReport(PartsReceivedReportRequestResource options);

        public Stream GetReportCsv(PartsReceivedReportRequestResource options);
    }
}

