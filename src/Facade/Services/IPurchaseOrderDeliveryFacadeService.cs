namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.Keys;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.RequestResources;

    public interface IPurchaseOrderDeliveryFacadeService
    {
        IResult<IEnumerable<PurchaseOrderDeliveryResource>> SearchDeliveries(
            string supplierSearchTerm, 
            string orderNumberSearchTerm, 
            bool includeAcknowledged);

        IResult<PurchaseOrderDeliveryResource> PatchDelivery(
            PurchaseOrderDeliveryKey key,
            PatchRequestResource<PurchaseOrderDeliveryResource> requestResource, 
            IEnumerable<string> privileges);

        IResult<BatchUpdateProcessResultResource> BatchUpdateDeliveriesFromCsv(string csvString, IEnumerable<string> privileges);

        IResult<IEnumerable<PurchaseOrderDeliveryResource>> UpdateDeliveriesForDetail(
            int orderNumber,
            int orderLine,
            IEnumerable<PurchaseOrderDeliveryResource> resource,
            IEnumerable<string> privileges);

        IResult<IEnumerable<PurchaseOrderDeliveryResource>> GetDeliveriesForDetail(
            int orderNumber,
            int orderLine);
    }
}
