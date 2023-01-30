namespace Linn.Purchasing.Domain.LinnApps.ExternalServices
{
    public interface IPcasPack
    {
        string DiscrepanciesOnChange(string boardCode, string revisionCode, int changeId);
    }
}
