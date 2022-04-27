namespace Linn.Purchasing.Proxy
{
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;

    public class ForecastingPack : IForecastingPack
    {
        public void ApplyAcrossBoardPlanChange(decimal change, int startPeriod, int endPeriod)
        {
            throw new System.NotImplementedException();
        }

        public void SetAutoForecastChange(decimal change, int startWeek, int endWeek)
        {
            throw new System.NotImplementedException();
        }
    }
}
