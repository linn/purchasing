namespace Linn.Purchasing.Domain.LinnApps.Exceptions
{
    using System;

    using Linn.Common.Domain.Exceptions;

    public class VendorManagerException : DomainException
    {
        public VendorManagerException(string message)
            : base(message)
        {
        }

        public VendorManagerException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
