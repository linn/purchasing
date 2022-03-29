namespace Linn.Purchasing.Proxy
{
    using Linn.Common.Proxy.LinnApps;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;

    public class MrpLoadPack : IMrpLoadPack
    {
        private readonly IDatabaseService databaseService;

        public MrpLoadPack(IDatabaseService databaseService)
        {
            this.databaseService = databaseService;
        }

        public int GetNextRunLogId()
        {
            throw new System.NotImplementedException();
        }

        public ProcessResult ScheduleMrp(int runLogId)
        {
            throw new System.NotImplementedException();
        }
    }
}
