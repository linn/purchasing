﻿namespace Linn.Purchasing.Domain.LinnApps.Boms.Exceptions
{
    using System;
    using Linn.Common.Domain.Exceptions;

    public class InvalidStateChangeException : DomainException
    {
        public InvalidStateChangeException(string message)
            : base(message)
        {
        }

        public InvalidStateChangeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
