namespace Linn.Purchasing.Persistence.LinnApps.Repositories
{
    using System.Linq;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Purchasing.Domain.LinnApps;

    using Microsoft.EntityFrameworkCore;

    public class NominalAccountRepository : EntityFrameworkRepository<NominalAccount, int>
    {
        private readonly ServiceDbContext serviceDbContext;

        public NominalAccountRepository(ServiceDbContext serviceDbContext)
            : base(serviceDbContext.NominalAccounts)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public override NominalAccount FindById(int key)
        {
            return this.serviceDbContext
                .NominalAccounts
                .Include(n => n.Nominal)
                .Include(n => n.Department)
                .First(n => n.AccountId == key);
        }
    }
}
