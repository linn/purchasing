namespace Linn.Purchasing.Domain.LinnApps.Boms.Exceptions
{
    using Linn.Common.Domain.Exceptions;
    using System;

    public class InvalidPhaseInWeekException : DomainException
    {
        public InvalidPhaseInWeekException(string message)
            : base(message)
        {
        }

        public InvalidPhaseInWeekException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
