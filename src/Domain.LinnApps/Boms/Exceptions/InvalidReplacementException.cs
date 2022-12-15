namespace Linn.Purchasing.Domain.LinnApps.Boms.Exceptions
{
    using System;

    using Linn.Common.Domain.Exceptions;

    public class InvalidReplacementException : DomainException
    {
        public InvalidReplacementException(string message)
            : base(message)
        {
        }

        public InvalidReplacementException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
