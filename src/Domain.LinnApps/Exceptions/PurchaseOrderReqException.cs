namespace Linn.Purchasing.Domain.LinnApps.Exceptions
{
    using System;

    using Linn.Common.Domain.Exceptions;

    public class PurchaseOrderReqException : DomainException
    {
        public PurchaseOrderReqException(string message)
            : base(message)
        {
        }

        public PurchaseOrderReqException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
