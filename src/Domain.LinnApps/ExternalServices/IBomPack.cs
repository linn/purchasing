namespace Linn.Purchasing.Domain.LinnApps.ExternalServices
{
    public interface IBomPack
    {
        void CopyBom(
            string srcPartNumber,
            int destBomId,
            int destChangeId,
            string destChangeState,
            string addOrOverWrite);

        void ExplodeSubAssembly(
            int bomId, int changeId, string changeState, string subAssembly);
    }
}
