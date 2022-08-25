namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.RequestResources;

    public interface IEdiOrdersFacadeService
    {
        IResult<IEnumerable<EdiSupplierResource>> GetEdiSuppliers();

        IResult<IEnumerable<EdiOrderResource>> GetEdiOrders(int supplierId);

        IResult<ProcessResultResource> SendEdiOrder(SendEdiEmailResource resource);
    }
}
