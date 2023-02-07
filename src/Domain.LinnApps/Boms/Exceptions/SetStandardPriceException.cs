namespace Linn.Purchasing.Domain.LinnApps.Boms.Exceptions
{
    using System;
    using Linn.Common.Domain.Exceptions;

    public class SetStandardPriceException : DomainException
    {
        public SetStandardPriceException(string message)
            : base(message)
        {
        }

        public SetStandardPriceException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
