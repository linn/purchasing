namespace Linn.Purchasing.Domain.LinnApps.Exceptions
{
    using System;

    using Linn.Common.Domain.Exceptions;

    public class InvalidActionException : DomainException
    {
        public InvalidActionException(string message)
            : base(message)
        {
        }

        public InvalidActionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
