namespace Linn.Purchasing.Domain.LinnApps.PartSuppliers.Exceptions
{
    using System;

    using Linn.Common.Domain.Exceptions;

    public class PartSupplierException : DomainException
    {
        public PartSupplierException(string message)
            : base(message)
        {
        }

        public PartSupplierException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
