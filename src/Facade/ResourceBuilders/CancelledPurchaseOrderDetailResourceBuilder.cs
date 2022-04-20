namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System;
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Resources;

    public class CancelledPurchaseOrderDetailResourceBuilder : IBuilder<CancelledOrderDetail>
    {
        public CancelledPurchaseOrderDetailResource Build(CancelledOrderDetail entity, IEnumerable<string> claims)
        {
            return new CancelledPurchaseOrderDetailResource
                       {
                           OrderNumber = entity.OrderNumber,
                           LineNumber = entity.LineNumber,
                           DeliverySequence = entity.DeliverySequence,
                           DateCancelled = entity.DateCancelled,
                           CancelledBy =
                               entity.CancelledBy != null
                                   ? new EmployeeResource
                                         {
                                             Id = entity.CancelledBy.Id, FullName = entity.CancelledBy.FullName
                                         }
                                   : null,
                           DateFilCancelled = entity.DateFilCancelled,
                           FilCancelledBy =
                               entity.FilCancelledBy != null
                                   ? new EmployeeResource
                                         {
                                             Id = entity.FilCancelledBy.Id, FullName = entity.FilCancelledBy.FullName
                                         }
                                   : null,
                           ReasonCancelled = entity.ReasonCancelled,
                           Id = entity.Id,
                           PeriodCancelled = entity.PeriodCancelled,
                           PeriodFilCancelled = entity.PeriodFilCancelled,
                           ValueCancelled = entity.ValueCancelled,
                           DateUncancelled = entity.DateUncancelled,
                           DateFilUncancelled = entity.DateFilUncancelled,
                           DatePreviouslyCancelled = entity.DatePreviouslyCancelled,
                           DatePreviouslyFilCancelled = entity.DatePreviouslyFilCancelled,
                           ValueFilCancelled = entity.ValueFilCancelled,
                           BaseValueFilCancelled = entity.BaseValueFilCancelled
                       };
        }

        public string GetLocation(CancelledOrderDetail p)
        {
            throw new NotImplementedException();
        }

        object IBuilder<CancelledOrderDetail>.Build(CancelledOrderDetail entity, IEnumerable<string> claims)
        {
            return this.Build(entity, claims);
        }

        private IEnumerable<LinkResource> BuildLinks(PurchaseOrder model, IEnumerable<string> claims)
        {
            throw new NotImplementedException();
        }
    }
}
