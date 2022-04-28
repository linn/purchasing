namespace Linn.Purchasing.Domain.LinnApps
{
    using System;

    public class LinnWeek
    {
        public int WeekNumber { get; set; }

        public DateTime StartsOn { get; set; }

        public DateTime EndsOn { get; set; }
    }
}
