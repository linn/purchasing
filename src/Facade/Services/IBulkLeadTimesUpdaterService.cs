﻿namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Purchasing.Resources;

    public interface IBulkLeadTimesUpdaterService
    {
        IResult<ProcessResultResource> BulkUpdateFromCsv(
            string csvString, IEnumerable<string> privileges);
    }
}
