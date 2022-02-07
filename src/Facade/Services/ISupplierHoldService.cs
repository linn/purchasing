namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Purchasing.Resources;

    public interface ISupplierHoldService
    {
        IResult<SupplierResource> ChangeSupplierHoldStatus(
            SupplierHoldChangeResource resource, IEnumerable<string> privileges);
    }
}
