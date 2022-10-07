namespace Linn.Purchasing.Domain.LinnApps.Exceptions
{
    using System;

    using Linn.Common.Domain.Exceptions;

    public class UnauthorisedOrderException : DomainException
    {
        public UnauthorisedOrderException(string message)
            : base(message)
        {
        }

        public UnauthorisedOrderException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
