namespace Linn.Purchasing.Facade.Services
{
    using System;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.MaterialRequirements;
    using Linn.Purchasing.Resources.MaterialRequirements;

    public class MrMasterFacadeService : SingleRecordFacadeResourceService<MrMaster, MrMasterResource>
    {
        public MrMasterFacadeService(ISingleRecordRepository<MrMaster> repository, ITransactionManager transactionManager, IBuilder<MrMaster> builder)
            : base(repository, transactionManager, builder)
        {
        }

        protected override void UpdateFromResource(MrMaster entity, MrMasterResource updateResource)
        {
            throw new NotImplementedException();
        }
    }
}
