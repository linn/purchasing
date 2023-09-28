namespace Linn.Purchasing.Domain.LinnApps.Boms
{
    using System.Collections.Generic;
    using System.Linq;

    public class CircuitBoard
    {
        public string BoardCode { get; set; }
        
        public string Description { get; set; }
        
        public int? ChangeId { get; set; }

        public string ChangeState { get; set; }

        public string SplitBom { get; set; }

        public string DefaultPcbNumber { get; set; }

        public string VariantOfBoardCode { get; set; }

        public string LoadDirectory { get; set; }

        public int? BoardsPerSheet { get; set; }

        public string CoreBoard { get; set; }

        public string ClusterBoard { get; set; }

        public string IdBoard { get; set; }

        public IList<BoardLayout> Layouts { get; set; }

        public IList<BoardComponent> Components { get; set; }

        public IList<BoardComponent> ComponentsOnRevision(int layoutSequence, int versionNumber, bool liveOnly = false)
        {
            return this.Components.Where(
                a => this.RevisionInRange(
                    layoutSequence,
                    versionNumber,
                    a.FromLayoutVersion,
                    a.FromRevisionVersion,
                    a.ToLayoutVersion,
                    a.ToRevisionVersion,
                    a.ChangeState,
                    liveOnly)).ToList();
        }

        private bool RevisionInRange(
            int layoutSequence,
            int revisionNumber,
            int fromLayoutVersion,
            int fromRevisionVersion,
            int? toLayoutVersion,
            int? toRevisionVersion,
            string changeState,
            bool liveOnly = false)
        {
            if (changeState != "LIVE" && changeState != "PROPOS" && changeState != "ACCEPT")
            {
                return false;
            }

            if ((changeState == "PROPOS" || changeState == "ACCEPT") && liveOnly)
            {
                return false;
            }

            if (fromLayoutVersion > layoutSequence || (toLayoutVersion.HasValue && toLayoutVersion < layoutSequence))
            {
                return false;
            }

            if (fromLayoutVersion == layoutSequence && fromRevisionVersion > revisionNumber)
            {
                return false;
            }

            if (toLayoutVersion.HasValue && toLayoutVersion == layoutSequence && toRevisionVersion < revisionNumber)
            {
                return false;
            }


            return true;
        }
    }
}
