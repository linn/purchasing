namespace Linn.Purchasing.Domain.LinnApps.Parts.Exceptions
{
    using System;

    using Linn.Common.Domain.Exceptions;

    public class InvalidBomTypeChangeException : DomainException
    {
        public InvalidBomTypeChangeException(string message)
            : base(message)
        {
        }

        public InvalidBomTypeChangeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
