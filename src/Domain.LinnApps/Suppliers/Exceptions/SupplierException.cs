namespace Linn.Purchasing.Domain.LinnApps.Suppliers.Exceptions
{
    using System;

    using Linn.Common.Domain.Exceptions;

    public class SupplierException : DomainException
    {
        public SupplierException(string message)
            : base(message)
        {
        }

        public SupplierException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
