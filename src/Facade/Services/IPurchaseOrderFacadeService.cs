namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.SearchResources;

    public interface IPurchaseOrderFacadeService : IFacadeResourceFilterService<PurchaseOrder, int,
        PurchaseOrderResource, PurchaseOrderResource, PurchaseOrderSearchResource>
    {
        Task<string> GetOrderAsHtml(int orderNumber);

        Task<IResult<ProcessResultResource>> EmailOrderPdf(int orderNumber, string emailAddress, bool bcc, int currentUserId);

        IResult<PurchaseOrderResource> FillOutOrderFromSupplierId(PurchaseOrderResource resource, IEnumerable<string> privileges);
    }
}
