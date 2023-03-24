namespace Linn.Purchasing.Domain.LinnApps.Boms
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Purchasing.Domain.LinnApps.Boms.Models;

    public class CircuitBoardSummaryService : ICircuitBoardSummaryService
    {
        public Expression<Func<BoardComponentSummary, bool>> GetFilterExpression(
            string boardCodeSearch,
            string revisionCodeSearch,
            string crefSearch,
            string partNumberSearch)
        {
            var boardSplit = boardCodeSearch?.Split("*").Where(a => !string.IsNullOrEmpty(a)).Select(b => b.ToUpper()).ToList();
            var revisionSplit = revisionCodeSearch?.Split("*").Where(a => !string.IsNullOrEmpty(a)).Select(b => b.ToUpper()).ToList();
            var crefSplit = crefSearch?.Split("*").Where(a => !string.IsNullOrEmpty(a)).Select(b => b.ToUpper()).ToList();
            var partSplit = partNumberSearch?.Split("*").Where(a => !string.IsNullOrEmpty(a)).Select(b => b.ToUpper()).ToList();

            return a => (string.IsNullOrEmpty(boardCodeSearch)
                         || (boardCodeSearch.Contains("*")
                                 ? boardSplit.Count == 1
                                       ? boardCodeSearch.EndsWith("*") && !boardCodeSearch.StartsWith("*")
                                             ? a.BoardCode.StartsWith(boardSplit.First())
                                             : boardCodeSearch.StartsWith("*") && !boardCodeSearch.EndsWith("*")
                                                 ? a.BoardCode.EndsWith(boardSplit.First())
                                                 : a.BoardCode.Contains(boardSplit.First())
                                       : a.BoardCode.StartsWith(boardSplit.First()) && a.BoardCode.EndsWith(boardSplit.Last())
                                 : a.BoardCode == boardCodeSearch.ToUpper()))
                && (string.IsNullOrEmpty(revisionCodeSearch)
                    || (revisionCodeSearch.Contains("*")
                            ? revisionSplit.Count == 1
                                  ? revisionCodeSearch.EndsWith("*") && !revisionCodeSearch.StartsWith("*")
                                        ? a.RevisionCode.StartsWith(revisionSplit.First())
                                        : revisionCodeSearch.StartsWith("*") && !revisionCodeSearch.EndsWith("*")
                                            ? a.RevisionCode.EndsWith(revisionSplit.First())
                                            : a.RevisionCode.Contains(revisionSplit.First())
                                  : a.RevisionCode.StartsWith(revisionSplit.First()) && a.RevisionCode.EndsWith(revisionSplit.Last())
                            : a.RevisionCode == revisionCodeSearch.ToUpper()))
                && (string.IsNullOrEmpty(crefSearch)
                    || (crefSearch.Contains("*")
                            ? crefSplit.Count == 1
                                  ? crefSearch.EndsWith("*") && !crefSearch.StartsWith("*")
                                        ? a.Cref.StartsWith(crefSplit.First())
                                        : crefSearch.StartsWith("*") && !crefSearch.EndsWith("*")
                                            ? a.Cref.EndsWith(crefSplit.First())
                                            : a.Cref.Contains(crefSplit.First())
                                  : a.Cref.StartsWith(crefSplit.First()) && a.Cref.EndsWith(crefSplit.Last())
                            : a.Cref == crefSearch.ToUpper()))
                && (string.IsNullOrEmpty(partNumberSearch)
                    || (partNumberSearch.Contains("*")
                            ? partSplit.Count == 1
                                  ? partNumberSearch.EndsWith("*") && !partNumberSearch.StartsWith("*")
                                        ? a.PartNumber.StartsWith(partSplit.First())
                                        : partNumberSearch.StartsWith("*") && !partNumberSearch.EndsWith("*")
                                            ? a.PartNumber.EndsWith(partSplit.First())
                                            : a.PartNumber.Contains(partSplit.First())
                                  : a.PartNumber.StartsWith(partSplit.First()) && a.PartNumber.EndsWith(partSplit.Last())
                            : a.PartNumber == partNumberSearch.ToUpper()));
        }
    }
}
