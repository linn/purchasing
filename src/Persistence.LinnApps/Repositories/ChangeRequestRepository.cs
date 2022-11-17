namespace Linn.Purchasing.Persistence.LinnApps.Repositories
{
    using System.Linq;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Purchasing.Domain.LinnApps.Boms;

    using Microsoft.EntityFrameworkCore;

    public class ChangeRequestRepository : EntityFrameworkRepository<ChangeRequest, int>
    {
        private readonly ServiceDbContext serviceDbContext;

        public ChangeRequestRepository(ServiceDbContext serviceDbContext)
            : base(serviceDbContext.ChangeRequests)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public override ChangeRequest FindById(int key)
        {
            return this.serviceDbContext
                .ChangeRequests
                .Include(c => c.BomChanges).ThenInclude(c => c.PhaseInWeek)
                .Include(c => c.PcasChanges)
                .First(c => c.DocumentNumber == key && c.DocumentType == "CRF");
        }
    }
}
