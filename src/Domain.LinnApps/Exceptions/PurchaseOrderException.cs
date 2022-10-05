namespace Linn.Purchasing.Domain.LinnApps.Exceptions
{
    using System;

    using Linn.Common.Domain.Exceptions;

    public class PurchaseOrderException : DomainException
    {
        public PurchaseOrderException(string message)
            : base(message)
        {
        }

        public PurchaseOrderException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
