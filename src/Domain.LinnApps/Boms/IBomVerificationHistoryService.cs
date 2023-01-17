using Linn.Purchasing.Domain.LinnApps.Parts;

namespace Linn.Purchasing.Domain.LinnApps.Boms
{
    public interface IBomVerificationHistoryService
    {
        Part ValidPartNumber(string partNumber);

        BomVerificationHistory CreateBomVerificationHistory(BomVerificationHistory entry);

        BomVerificationHistory GetHistoryEntry(BomVerificationHistory entry);
    }
}
