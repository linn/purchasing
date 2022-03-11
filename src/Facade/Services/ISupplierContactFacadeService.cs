namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Purchasing.Resources;

    public interface ISupplierContactFacadeService
    {
        IResult<SupplierContactResource> GetMainContactForSupplier(int supplierId, IEnumerable<string> privileges);
    }
}
