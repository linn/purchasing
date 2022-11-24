namespace Linn.Purchasing.Domain.LinnApps.Boms
{
    using System.Collections.Generic;

    public interface IChangeRequestService
    {
        ChangeRequest Approve(int documentNumber, IEnumerable<string> privileges = null);
    }
}
