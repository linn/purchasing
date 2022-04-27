namespace Linn.Purchasing.Domain.LinnApps.ExternalServices
{
    public interface IForecastingPack
    {
        void ApplyAcrossBoardPlanChange(decimal change, int startPeriod, int endPeriod);

        void SetAutoForecastChange(decimal change, int startWeek, int endWeek);
    }
}
