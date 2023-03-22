namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Boms.Models;

    public class PcasChangeComponentsService : IPcasChangeComponentsService
    {
        private readonly IQueryRepository<PcasChangeComponent> repository;

        public PcasChangeComponentsService(IQueryRepository<PcasChangeComponent> repository)
        {
            this.repository = repository; 
        }

        public IResult<IEnumerable<PcasChangeComponent>> GetChanges(int documentNumber)
        {
            return new SuccessResult<IEnumerable<PcasChangeComponent>>(
                this.repository.FilterBy(x => x.DocumentNumber == documentNumber));
        }
    }
}
