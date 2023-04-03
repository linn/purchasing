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

        void ReplacePartWith(
            string boardCode,
            string revisionCode,
            int layoutSeq,
            int versionNumber,
            int documentNumber,
            string changeState,
            int enteredBy,
            string cref,
            string partNumber,
            string newPartNumber,
            decimal? newQty,
            string newAsst);
    }
}
