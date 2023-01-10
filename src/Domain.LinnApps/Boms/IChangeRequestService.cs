namespace Linn.Purchasing.Domain.LinnApps.Boms
{
    using System.Collections.Generic;

    using Linn.Purchasing.Domain.LinnApps.Parts;

    public interface IChangeRequestService
    {
        Part ValidPartNumber(string partNumber);

        bool ChangeRequestAdmin(IEnumerable<string> privileges);

        ChangeRequest Approve(int documentNumber, IEnumerable<string> privileges = null);

        ChangeRequest Cancel(int documentNumber, int cancelledById, IEnumerable<int> selectedBomChangeIds, IEnumerable<int> selectedPcasChangeIds, IEnumerable<string> privileges = null);

        ChangeRequest MakeLive(int documentNumber, int appliedById, IEnumerable<int> selectedBomChangeIds, IEnumerable<int> selectedPcasChangeIds, IEnumerable<string> privileges = null);

        ChangeRequest PhaseInChanges(int documentNumber, int linnWeekNumber, IEnumerable<int> selectedBomChangeIds, IEnumerable<string> privileges = null);
    }
}
