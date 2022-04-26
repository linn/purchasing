namespace Linn.Purchasing.Domain.LinnApps.Reports
{
    using System.Linq;

    using Linn.Purchasing.Domain.LinnApps.Reports.Models;

    public interface IWhatsInInspectionRepository
    {
        IQueryable<PartsInInspection> GetWhatsInInspection(bool includeFailed = false);
    }
}
