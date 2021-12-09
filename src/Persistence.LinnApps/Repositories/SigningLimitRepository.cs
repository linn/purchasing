namespace Linn.Purchasing.Persistence.LinnApps.Repositories
{
    using System.Linq;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;

    using Microsoft.EntityFrameworkCore;

    public class SigningLimitRepository : EntityFrameworkRepository<SigningLimit, int>
    {
        private readonly ServiceDbContext serviceDbContext;

        public SigningLimitRepository(ServiceDbContext serviceDbContext)
            : base(serviceDbContext.SigningLimits)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public override SigningLimit FindById(int key)
        {
            var signingLimit = this.serviceDbContext.SigningLimits.Find(key);
            this.serviceDbContext.Entry(signingLimit).Reference(p => p.User).Load();
            return signingLimit;
        }

        public override IQueryable<SigningLimit> FindAll()
        {
            return this.serviceDbContext.SigningLimits
                .Include(a => a.User)
                .OrderBy(a => a.User.FullName);
        }
    }
}
