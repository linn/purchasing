namespace Linn.Purchasing.Persistence.LinnApps.Repositories
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

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
            var request = this.serviceDbContext
                .ChangeRequests
                .Include(c => c.BomChanges).ThenInclude(c => c.PhaseInWeek)
                .Include(c => c.BomChanges)
                .ThenInclude(c => c.AddedBomDetails).ThenInclude(d => d.Part)
                .Include(c => c.BomChanges).ThenInclude(c => c.DeletedBomDetails)
                .Include(x => x.ProposedBy)
                .Include(x => x.EnteredBy)
                .Include(x => x.OldPart)
                .Include(x => x.NewPart)
                .Include(c => c.CircuitBoard)
                .First(c => c.DocumentNumber == key && c.DocumentType == "CRF");

            // explicit loading to avoid cartesian load of bomchanges x pcaschanges
            this.serviceDbContext.Entry(request).Collection(x => x.PcasChanges).Load();
            
            return request;
        }

        public override IQueryable<ChangeRequest> FilterBy(Expression<Func<ChangeRequest, bool>> expression)
        {
            return this.serviceDbContext.ChangeRequests
                .Include(x => x.ProposedBy)

                .Where(expression);
        }
    }
}
