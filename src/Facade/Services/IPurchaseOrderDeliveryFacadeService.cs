namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.Keys;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.RequestResources;

    public interface IPurchaseOrderDeliveryFacadeService
    {
        IResult<IEnumerable<PurchaseOrderDeliveryResource>> SearchDeliveries(
            string supplierSearchTerm, string orderNumberSearchTerm, bool includeAcknowledged);

        IResult<PurchaseOrderDeliveryResource> PatchDelivery(
            PurchaseOrderDeliveryKey key,
            PatchRequestResource<PurchaseOrderDeliveryResource> requestResource, 
            IEnumerable<string> privileges);

        IResult<ProcessResult> BatchUpdateDeliveries(string csvString, IEnumerable<string> privileges);
    }
}
