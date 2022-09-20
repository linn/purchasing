namespace Linn.Purchasing.Persistence.LinnApps.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

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
                .Include(c => c.BomChanges).ThenInclude(c => c.PhaseInWeek) // .ThenInclude(d => d.Part)
                .First(c => c.DocumentNumber == key && c.DocumentType == "CRF");
        }
    }
}
