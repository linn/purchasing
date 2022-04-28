namespace Linn.Purchasing.Domain.LinnApps.Forecasting
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using Linn.Common.Authorisation;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;

    public class ForecastingService : IForecastingService
    {
        private readonly IForecastingPack forecastingPack;

        private readonly IAuthorisationService authService;

        private readonly IRepository<LinnWeek, int> linnWeekRepository;

        private readonly IRepository<LedgerPeriod, int> ledgerPeriodRepository;

        public ForecastingService(
            IForecastingPack forecastingPack, 
            IAuthorisationService authService,
            IRepository<LinnWeek, int> linnWeekRepository,
            IRepository<LedgerPeriod, int> ledgerPeriodRepository)
        {
            this.forecastingPack = forecastingPack;
            this.authService = authService;
            this.linnWeekRepository = linnWeekRepository;
            this.ledgerPeriodRepository = ledgerPeriodRepository;
        }

        public ProcessResult ApplyPercentageChange(
            decimal change,
            int startMonth,
            int startYear,
            int endMonth,
            int endYear, 
            IEnumerable<string> privileges)
        {
            if (!this.authService.HasPermissionFor(AuthorisedAction.ForecastingApplyPercentageChange, privileges))
            {
                return new ProcessResult(false, "You are not authorised to apply forecast changes.");
            }

            var startMonthName 
                = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(startMonth + 1).ToUpper();

            var endMonthName 
                = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(endMonth + 1).ToUpper();


            var startPeriod = this.ledgerPeriodRepository.FindBy(x 
                => x.MonthName.StartsWith(startMonthName) && x.MonthName.EndsWith(startYear.ToString()))?.PeriodNumber;

            var endPeriod = this.ledgerPeriodRepository
                .FindBy(x => x.MonthName.StartsWith(endMonthName) && x.MonthName.EndsWith(endYear.ToString()))
                ?.PeriodNumber;

            if (endPeriod < startPeriod)
            {
                return new ProcessResult(false, "End month cannot be before start month.");
            }

            if (!startPeriod.HasValue || !endPeriod.HasValue)
            {
                return new ProcessResult(false, "Invalid period entered.");
            }

            this.forecastingPack.ApplyAcrossBoardPlanChange(change, (int)startPeriod, (int)endPeriod);

            // include from the first week that ends in the start month
            var fromWeek =
                this.linnWeekRepository.FilterBy(w => w.WeekNumber > 0 
                                                      && w.EndsOn >= new DateTime(startYear, startMonth + 1, 1))
                    .OrderBy(x => x.StartsOn).First();

            // to the last week that starts in the end month
            var toWeek =
                this.linnWeekRepository.FilterBy(
                        w => w.WeekNumber > 0 && w.StartsOn 
                             <= new DateTime(endYear, endMonth + 1, DateTime.DaysInMonth(endYear, endMonth + 1)))
                    .OrderByDescending(x => x.StartsOn).First();

            this.forecastingPack.SetAutoForecastChange(change, fromWeek.WeekNumber, toWeek.WeekNumber);

            return new ProcessResult(true, "Process complete");
        }
    }
}
