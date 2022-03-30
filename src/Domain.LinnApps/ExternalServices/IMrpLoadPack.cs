namespace Linn.Purchasing.Domain.LinnApps.ExternalServices
{
    public interface IMrpLoadPack
    {
        int GetNextRunLogId();

        ProcessResult ScheduleMrp(int runLogId);
    }
}
