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
            return this.serviceDbContext
                .CircuitBoards
                .Include(c => c.Components).ThenInclude(a => a.AddChange)
                .Include(c => c.Components).ThenInclude(a => a.DeleteChange)
                .Include(a => a.Layouts)
                .ThenInclude(b => b.Revisions)
                .ThenInclude(c => c.RevisionType)
                .FirstOrDefault(c => c.BoardCode == key);
        }
    }
}
