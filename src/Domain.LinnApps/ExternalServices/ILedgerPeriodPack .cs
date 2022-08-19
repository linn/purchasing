namespace Linn.Purchasing.Domain.LinnApps.ExternalServices
{
    using System;

    public interface ILedgerPeriodPack
    {
        int GetPeriodNumber(DateTime date);
    }
}
