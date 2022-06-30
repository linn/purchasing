namespace Linn.Purchasing.Persistence.LinnApps.Repositories
{
    using System.Linq;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Purchasing.Domain.LinnApps.MaterialRequirements;

    using Microsoft.EntityFrameworkCore;

    public class PartNumberListRepository : EntityFrameworkRepository<PartNumberList, string>
    {
        private readonly ServiceDbContext serviceDbContext;

        public PartNumberListRepository(ServiceDbContext serviceDbContext)
            : base(serviceDbContext.PartNumberLists)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public override PartNumberList FindById(string key)
        {
            return this.serviceDbContext.PartNumberLists
                .Include(v => v.Elements)
                .SingleOrDefault(v => v.Name == key);
        }
    }
}
