namespace Linn.Purchasing.Domain.LinnApps.Boms
{
    using Linn.Purchasing.Domain.LinnApps.Boms.Models;

    public interface IBomChangeService
    {
        void ProcessTreeUpdate(BomTreeNode tree, int changeRequestNumber, int enteredBy);

        void CopyBom(
            string srcPartNumber, string destBomPartNumber, int changedBy, int crfNumber, string addOrOverwrite);

        void DeleteAllFromBom(string bomName, int crfNumber, int changedBy);

        void ExplodeSubAssembly(string bomName, int crfNumber, string subAssembly, int changedBy);

        void ReplaceBomDetail(int detailId, ChangeRequest request, int changedBy, decimal? newQty);

        void ReplaceAllBomDetails(ChangeRequest request, int changedBy, decimal? newQty);
    }
}
