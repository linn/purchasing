namespace Linn.Purchasing.Domain.LinnApps.ExternalServices
{
    public interface IPurchaseLedgerPack
    {
        int GetLedgerPeriod();

        int GetYearStartLedgerPeriod();
    }
}
