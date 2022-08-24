﻿namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.RequestResources;
    using Linn.Purchasing.Resources.SearchResources;

    public interface IPurchaseOrderFacadeService : IFacadeResourceFilterService<PurchaseOrder, int,
        PurchaseOrderResource, PurchaseOrderResource, PurchaseOrderSearchResource>
    {
        string GetOrderAsHtml(int orderNumber);

        IResult<ProcessResultResource> EmailOrderPdf(int orderNumber, string emailAddress, bool bcc, int currentUserId);

        IResult<ProcessResultResource> EmailSupplierAss(int orderNumber);

        IResult<PurchaseOrderResource> FillOutOrderFromSupplierId(PurchaseOrderResource resource, IEnumerable<string> privileges, int userId);

        IResult<ProcessResultResource> AuthorisePurchaseOrders(PurchaseOrdersProcessRequestResource resource, IEnumerable<string> privileges, int userId);

        IResult<ProcessResultResource> EmailOrderPdfs(PurchaseOrdersProcessRequestResource resource, IEnumerable<string> privileges, int userId);
    }
}
