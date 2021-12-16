namespace Linn.Purchasing.Domain.LinnApps
{
    using System;

    public abstract class LogTable : ILogTable
    {
        public int LogId { get; set; }

        public int LogUserNumber { get; set; }

        public string LogAction { get; set; }

        public DateTime LogTime { get; set; }
    }
}
