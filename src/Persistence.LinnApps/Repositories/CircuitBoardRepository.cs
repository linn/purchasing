namespace Linn.Purchasing.Persistence.LinnApps.Repositories
{
    using System.Linq;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Purchasing.Domain.LinnApps.Boms;

    using Microsoft.EntityFrameworkCore;

    public class CircuitBoardRepository : EntityFrameworkRepository<CircuitBoard, string>
    {
        private readonly ServiceDbContext serviceDbContext;

        public CircuitBoardRepository(ServiceDbContext serviceDbContext)
            : base(serviceDbContext.CircuitBoards)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public override CircuitBoard FindById(string key)
        {
            var request = this.serviceDbContext
                .CircuitBoards
                .Include(a => a.Layouts)
                .ThenInclude(b => b.Revisions)
                .ThenInclude(c => c.RevisionType)
                .FirstOrDefault(c => c.BoardCode == key);
            
            if (request != null)
            {
                this.serviceDbContext
                    .Entry(request)
                    .Collection(x => x.Components)
                    .Query()
                    .Include(a => a.AddChange)
                    .Include(a => a.DeleteChange)
                    .Load();
            }

            return request;
        }
    }
}
