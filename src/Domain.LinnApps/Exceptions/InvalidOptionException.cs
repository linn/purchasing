namespace Linn.Purchasing.Domain.LinnApps.Exceptions
{
    using System;

    using Linn.Common.Domain.Exceptions;

    public class InvalidOptionException : DomainException
    {
        public InvalidOptionException(string message)
            : base(message)
        {
        }

        public InvalidOptionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
