namespace Linn.Purchasing.Resources.Boms
{
    using System.Collections.Generic;

    public class CircuitBoardResource : CircuitBoardBaseResource
    {
        public IEnumerable<BoardComponentResource> Components { get; set; }

        public string Discrepancies { get; set; }
    }
}
