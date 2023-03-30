namespace Linn.Purchasing.Domain.LinnApps.Boms
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Linn.Purchasing.Domain.LinnApps.Parts;

    public interface IChangeRequestService
    {
        Part ValidPartNumber(string partNumber);

        bool ChangeRequestAdmin(IEnumerable<string> privileges);

        ChangeRequest Approve(int documentNumber, IEnumerable<string> privileges = null);

        ChangeRequest Cancel(int documentNumber, int cancelledById, IEnumerable<int> selectedBomChangeIds, IEnumerable<int> selectedPcasChangeIds, IEnumerable<string> privileges = null);

        ChangeRequest MakeLive(int documentNumber, int appliedById, IEnumerable<int> selectedBomChangeIds, IEnumerable<int> selectedPcasChangeIds, IEnumerable<string> privileges = null);

        ChangeRequest PhaseInChanges(int documentNumber, int? linnWeekNumber, DateTime? linnWeekStartDate, IEnumerable<int> selectedBomChangeIds, IEnumerable<string> privileges = null);

        ChangeRequest UndoChanges(
            int documentNumber,
            int undoneById,
            IEnumerable<int> selectedBomChangeIds,
            IEnumerable<int> selectedPcasChangeIds,
            IEnumerable<string> privileges = null);

        ChangeRequest Replace(
            int documentNumber,
            int replacedBy,
            bool globalReplace,
            bool hasPcasLines,
            decimal? newQty,
            IEnumerable<int> selectedDetailIds,
            IEnumerable<string> selectedPcasComponents,
            IEnumerable<string> addToBoms,
            IEnumerable<string> privileges = null);

        Expression<Func<ChangeRequest, bool>> SearchExpression(string searchTerm, bool? outstanding, int? lastMonths);
    }
}
