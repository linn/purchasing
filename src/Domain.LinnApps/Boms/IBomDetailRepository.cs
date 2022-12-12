namespace Linn.Purchasing.Domain.LinnApps.Boms
{
    using System.Collections.Generic;

    using Linn.Common.Persistence;

    public interface IBomDetailRepository : IQueryRepository<BomDetailViewEntry>
    {
        IEnumerable<BomDetailViewEntry> GetLiveBomDetails(string bomName);
    }
}
