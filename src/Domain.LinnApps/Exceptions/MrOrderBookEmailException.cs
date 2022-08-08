namespace Linn.Purchasing.Domain.LinnApps.Exceptions
{
    using System;

    using Linn.Common.Domain.Exceptions;

    public class MrOrderBookEmailException : DomainException
    {
        public MrOrderBookEmailException(string message)
            : base(message)
        {
        }

        public MrOrderBookEmailException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
