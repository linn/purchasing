namespace Linn.Purchasing.Domain.LinnApps.Boms.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

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
