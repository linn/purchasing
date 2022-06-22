namespace Linn.Purchasing.Facade.Services
{
    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Resources;

    public interface IPurchaseOrderFacadeService : IFacadeResourceService<PurchaseOrder, int,
        PurchaseOrderResource, PurchaseOrderResource>
    {
        string GetOrderAsHtml(int orderNumber);

        IResult<ProcessResultResource> EmailOrderPdf(int orderNumber, string emailAddress, bool bcc, int currentUserId);
    }
}
