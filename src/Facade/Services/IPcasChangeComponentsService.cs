namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;

    using Linn.Purchasing.Domain.LinnApps.Boms.Models;

    public interface IPcasChangeComponentsService
    {
        CsvResult<IEnumerable<PcasChangeComponent>> GetChanges(int documentNumber);
    }
}
