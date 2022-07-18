﻿namespace Linn.Purchasing.Facade.Services
{
    using System.Threading.Tasks;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Resources;

    public interface IPurchaseOrderFacadeService : IFacadeResourceService<PurchaseOrder, int,
        PurchaseOrderResource, PurchaseOrderResource>
    {
        Task<string> GetOrderAsHtml(int orderNumber);

        Task<IResult<ProcessResultResource>> EmailOrderPdf(int orderNumber, string emailAddress, bool bcc, int currentUserId);
    }
}
