namespace Linn.Purchasing.Domain.LinnApps.Suppliers.Exceptions
{
    using System;

    public class SupplierException : AppDomainUnloadedException
    {
        public SupplierException(string message)
            : base(message)
        {
        }

        public SupplierException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
