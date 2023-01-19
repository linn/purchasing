namespace Linn.Purchasing.Domain.LinnApps.Boms.Exceptions
{
    using System;

    using Linn.Common.Domain.Exceptions;

    public class InvalidBomChangeException : DomainException
    {
        public InvalidBomChangeException(string message)
            : base(message)
        {
        }

        public InvalidBomChangeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
