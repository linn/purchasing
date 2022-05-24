namespace Linn.Purchasing.Domain.LinnApps.Exceptions
{
    using System;

    using Linn.Common.Domain.Exceptions;

    public class PurchaseOrderDeliveryException : DomainException
    {
        public PurchaseOrderDeliveryException(string message)
            : base(message)
        {
        }

        public PurchaseOrderDeliveryException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
