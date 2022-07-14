namespace Linn.Purchasing.Facade.Services
{
    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.SearchResources;

    public interface IPurchaseOrderFacadeService : IFacadeResourceFilterService<PurchaseOrder, int,
        PurchaseOrderResource, PurchaseOrderResource, PurchaseOrderSearchResource>
    {
        string GetOrderAsHtml(int orderNumber);

        IResult<ProcessResultResource> EmailOrderPdf(int orderNumber, string emailAddress, bool bcc, int currentUserId);
    }
}
