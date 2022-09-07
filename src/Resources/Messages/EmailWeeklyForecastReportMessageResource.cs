namespace Linn.Purchasing.Resources.Messages
{
    using System;

    public class EmailMonthlyForecastReportMessageResource
    {
        public int ForSupplier { get; set; }

        public string ToAddress { get; set; }

        public DateTime Timestamp { get; set; }

        public bool Test { get; set; }
    }
}
