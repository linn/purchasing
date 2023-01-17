namespace Linn.Purchasing.Domain.LinnApps.Boms
{
    public interface IBomVerificationHistoryService
    {
        BomVerificationHistory CreateBomVerificationHistory(BomVerificationHistory entry);

        BomVerificationHistory GetHistoryEntry(BomVerificationHistory entry);
    }
}
