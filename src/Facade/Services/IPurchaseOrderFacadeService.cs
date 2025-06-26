namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.RequestResources;
    using Linn.Purchasing.Resources.SearchResources;

    public interface IPurchaseOrderFacadeService : IFacadeResourceFilterService<PurchaseOrder, int,
        PurchaseOrderResource, PurchaseOrderResource, PurchaseOrderSearchResource>
    {
        string GetOrderAsHtml(int orderNumber);

        IResult<ProcessResultResource> EmailOrderPdf(
            int orderNumber, string emailAddress, bool bcc, int currentUserId);

        IResult<ProcessResultResource> EmailSupplierAss(int orderNumber);

        IResult<ProcessResultResource> EmailFinanceAuthRequest(int currentUserNumber, int orderNumber);

        IResult<PurchaseOrderResource> FillOutOrderFromSupplierId(
            PurchaseOrderResource resource, IEnumerable<string> privileges, int userId);

        IResult<PurchaseOrderResource> AuthorisePurchaseOrder(
            int orderNumber, IEnumerable<string> privileges, int userId);

        IResult<ProcessResultResource> AuthorisePurchaseOrders(
            PurchaseOrdersProcessRequestResource resource, IEnumerable<string> privileges, int userId);

        IResult<ProcessResultResource> EmailOrderPdfs(
            PurchaseOrdersProcessRequestResource resource, IEnumerable<string> privileges, int userId);

        IResult<ProcessResultResource> EmailDept(int orderNumber, int userNumber);

        IResult<PurchaseOrderResource> PatchOrder(
            PatchRequestResource<PurchaseOrderResource> resource,
            int who,
            IEnumerable<string> privileges);

        IResult<PurchaseOrderResource> SwitchOurQtyPrice(
            int orderNumber,
            int userNumber,
            IList<string> privileges);
    }
}
