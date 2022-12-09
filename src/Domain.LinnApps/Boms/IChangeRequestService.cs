namespace Linn.Purchasing.Domain.LinnApps.Boms
{
    using System.Collections.Generic;

    using Linn.Purchasing.Domain.LinnApps.Parts;

    public interface IChangeRequestService
    {
        Part ValidPartNumber(string partNumber);

        ChangeRequest Approve(int documentNumber, IEnumerable<string> privileges = null);

        ChangeRequest Cancel(int documentNumber, int cancelledById, IEnumerable<int> selectedBomChangeIds, IEnumerable<int> selectedPcasChangeIds, IEnumerable<string> privileges = null);
    }
}
