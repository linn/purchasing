namespace Linn.Purchasing.Persistence.LinnApps.Repositories
{
    using Linn.Common.Persistence.EntityFramework;
    using Linn.Purchasing.Domain.LinnApps.Edi;

    public class EdiOrdersRepository : EntityFrameworkRepository<EdiOrder, int>
    {
        public EdiOrdersRepository(ServiceDbContext serviceDbContext)
            : base(serviceDbContext.EdiOrders)
        {
        }
    }
}

