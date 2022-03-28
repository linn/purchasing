namespace Linn.Purchasing.Persistence.LinnApps.Repositories
{
    using Linn.Common.Persistence.EntityFramework;
    using Linn.Common.Proxy.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;

    public class OverbookAllowedByLogRespository : EntityFrameworkRepository<OverbookAllowedByLog, int>
    {
        private readonly ServiceDbContext serviceDbContext;
        private readonly IDatabaseService databaseService;

        public OverbookAllowedByLogRespository(ServiceDbContext serviceDbContext, IDatabaseService databaseService)
            : base(serviceDbContext.AllowOverbookLogs)
        {
            this.serviceDbContext = serviceDbContext;
            this.databaseService = databaseService;
        }

        public override void Add(OverbookAllowedByLog entity)
        {
            entity.Id = this.databaseService.GetIdSequence("AN_ARBITRARY_SEQUENCE");
            this.serviceDbContext.AllowOverbookLogs.Add(entity);
        }
    }
}
