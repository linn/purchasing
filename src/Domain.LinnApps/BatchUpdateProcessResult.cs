namespace Linn.Purchasing.Domain.LinnApps
{
    using System.Collections.Generic;

    public class BatchUpdateProcessResult : ProcessResult
    {
        public IEnumerable<Error> Errors { get; set; }
    }
}
