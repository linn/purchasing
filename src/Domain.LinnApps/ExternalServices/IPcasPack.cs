namespace Linn.Purchasing.Domain.LinnApps.ExternalServices
{
    public interface IPcasPack
    {
        string DiscrepanciesOnChange(string boardCode, string revisionCode, int changeId);

        void UndoPcasChange(int changeId, int undoneBy);

        void ReplaceAll(
            string partNumber,
            int documentNumber,
            string changeState,
            int replacedBy,
            string newPartNumber);
    }
}
