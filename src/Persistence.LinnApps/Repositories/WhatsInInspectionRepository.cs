namespace Linn.Purchasing.Persistence.LinnApps.Repositories
{
    using System.Linq;

    using Linn.Purchasing.Domain.LinnApps.Reports;
    using Linn.Purchasing.Domain.LinnApps.Reports.Models;

    public class WhatsInInspectionRepository : IWhatsInInspectionRepository
    {
        private readonly ServiceDbContext serviceDbContext;

        public WhatsInInspectionRepository(ServiceDbContext serviceDbContext)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public IQueryable<WhatsInInspectionViewModel> GetWhatsInInspection(bool includeFailed = false)
        {
            if (includeFailed)
            {
                return this.serviceDbContext.WhatsInInspectionIncludingFailedView.AsQueryable();
            }

            return this.serviceDbContext.WhatsInInspectionExcludingFailedView.AsQueryable();
        }
    }
}
