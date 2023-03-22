namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;

    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Boms.Models;

    public class PcasChangeComponentsService : IPcasChangeComponentsService
    {
        private readonly IQueryRepository<PcasChangeComponent> repository;

        public PcasChangeComponentsService(IQueryRepository<PcasChangeComponent> repository)
        {
            this.repository = repository; 
        }

        public CsvResult<IEnumerable<PcasChangeComponent>> GetChanges(int documentNumber)
        {
            return new CsvResult<IEnumerable<PcasChangeComponent>>("export.csv")
                       {
                           Data = this.repository.FilterBy(x => x.DocumentNumber == documentNumber)
                       };
        }
    }
}
