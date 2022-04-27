namespace Linn.Purchasing.Domain.LinnApps.Forecasting
{
    using System.Collections.Generic;

    using Linn.Common.Authorisation;
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;

    public class ForecastingService : IForecastingService
    {
        private readonly IForecastingPack forecastingPack;

        private readonly IAuthorisationService authService;

        public ForecastingService(IForecastingPack forecastingPack, IAuthorisationService authService)
        {
            this.forecastingPack = forecastingPack;
            this.authService = authService;
        }

        public ProcessResult ApplyPercentageChange(
            decimal change, int startPeriod, int endPeriod, IEnumerable<string> privileges)
        {
            if (!this.authService.HasPermissionFor(AuthorisedAction.ForecastingApplyPercentageChange, privileges))
            {
                return new ProcessResult(false, "You are not authorised to apply forecast changes.");
            }

            this.forecastingPack.ApplyAcrossBoardPlanChange(change, startPeriod, endPeriod);
            this.forecastingPack.SetAutoForecastChange(change, startPeriod, endPeriod); // todo translate to weeks

            return new ProcessResult(true, "Process complete");
        }
    }
}
