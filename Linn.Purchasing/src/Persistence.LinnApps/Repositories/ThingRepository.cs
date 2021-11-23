namespace Linn.Purchasing.Persistence.LinnApps.Repositories
{
    using System.Linq;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Purchasing.Domain.LinnApps;

    using Microsoft.EntityFrameworkCore;

    public class ThingRepository : EntityFrameworkRepository<Thing, int>
    {
        private readonly ServiceDbContext serviceDbContext;

        public ThingRepository(ServiceDbContext serviceDbContext)
            : base(serviceDbContext.Things)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public override Thing FindById(int key)
        {
            var thing = this.serviceDbContext.Things.Find(key);
            this.serviceDbContext.Entry(thing).Reference(p => p.Code).Load();
            this.serviceDbContext.Entry(thing).Collection(p => p.Details).Load();
            return thing;
        }

        public override IQueryable<Thing> FindAll()
        {
            return this.serviceDbContext.Things
                .Include(a => a.Code)
                .Include(a => a.Details);
        }
    }
}
