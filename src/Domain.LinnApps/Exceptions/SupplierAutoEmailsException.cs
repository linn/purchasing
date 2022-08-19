namespace Linn.Purchasing.Domain.LinnApps.Exceptions
{
    using System;

    using Linn.Common.Domain.Exceptions;

    public class SupplierAutoEmailsException : DomainException
    {
        public SupplierAutoEmailsException(string message)
            : base(message)
        {
        }

        public SupplierAutoEmailsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
