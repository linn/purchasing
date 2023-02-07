namespace Linn.Purchasing.Domain.LinnApps.ExternalServices
{
    using System;

    using Linn.Purchasing.Domain.LinnApps.PurchaseOrderReqs;

    public interface IPurchaseOrderAutoOrderPack
    {
        CreateOrderFromReqResult CreateMiniOrderFromReq(
            string nominal,
            string department,
            int requestedBy,
            int turnedIntoOrderBy,
            string description,
            string quoteRef,
            string remarksForOrder,
            string partNumber,
            int supplierId,
            decimal qty,
            DateTime? dateRequired,
            decimal ourUnitPrice,
            bool authAllowed,
            string internalRemarksForOrder);
        
        CreateOrderFromReqResult CreateAutoOrder(
            string partNumber,
            int supplierId,
            decimal qty,
            DateTime dateRequired,
            decimal? ourUnitPrice,
            bool authAllowed);
    }
}
