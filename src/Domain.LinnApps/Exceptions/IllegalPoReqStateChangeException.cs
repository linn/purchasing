namespace Linn.Purchasing.Domain.LinnApps.Exceptions
{
    using System;

    using Linn.Common.Domain.Exceptions;

    public class IllegalPoReqStateChangeException : DomainException
    {
        public IllegalPoReqStateChangeException (string message)
            : base(message)
        {
        }

        public IllegalPoReqStateChangeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
