namespace Linn.Purchasing.Domain.LinnApps.Boms
{
    using Linn.Purchasing.Domain.LinnApps.Parts;

    public interface IBomVerificationHistoryService
    {
        Part ValidPartNumber(string partNumber);

        BomVerificationHistory CreateBomVerificationHistory(BomVerificationHistory entry);

        BomVerificationHistory GetHistoryEntry(BomVerificationHistory entry);
    }
}
