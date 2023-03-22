namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.Boms.Models;

    public interface IPcasChangeComponentsService
    {
        IResult<IEnumerable<PcasChangeComponent>> GetChanges(int documentNumber);
    }
}
