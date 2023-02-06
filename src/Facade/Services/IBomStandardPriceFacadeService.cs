namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.Boms.Models;

    public interface IBomStandardPriceFacadeService
    {
        IResult<IEnumerable<BomStandardPrice>> GetData(string searchTerm);
    }
}
