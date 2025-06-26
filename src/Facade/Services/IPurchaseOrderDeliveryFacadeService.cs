namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Purchasing.Resources;

    public interface IPurchaseOrderDeliveryFacadeService
    {
        IResult<IEnumerable<PurchaseOrderDeliveryResource>> SearchDeliveries(
            string supplierSearchTerm, 
            string orderNumberSearchTerm, 
            bool includeAcknowledged);

        IResult<BatchUpdateProcessResultResourceWithLinks> BatchUpdateDeliveries(
            string csvString, IEnumerable<string> privileges);

        IResult<BatchUpdateProcessResultResourceWithLinks> BatchUpdateDeliveries(
            IEnumerable<PurchaseOrderDeliveryUpdateResource> resource, 
            IEnumerable<string> privileges);

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
