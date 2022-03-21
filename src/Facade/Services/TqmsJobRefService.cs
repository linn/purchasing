namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Reports.Models;
    using Linn.Purchasing.Resources;

    public class TqmsJobRefService : ITqmsJobRefService
    {
        private readonly IRepository<TqmsJobref, string> repository;

        public TqmsJobRefService(IRepository<TqmsJobref, string> repository)
        {
            this.repository = repository;
        }

        public IResult<IEnumerable<TqmsJobRefResource>> GetMostRecentJobRefs(int? limit)
        {
            var results = this.repository
                .FindAll()
                .OrderByDescending(r => r.Date)
                .Select(
                r => new TqmsJobRefResource
                         {
                             Date = $"{r.Date.ToShortDateString()} {r.Date.ToShortTimeString()}",
                             Jobref = r.Jobref
                         });

            if (limit.HasValue)
            {
                results = results.Take((int)limit);
            }

            return new SuccessResult<IEnumerable<TqmsJobRefResource>>(results);
        }
    }
}
