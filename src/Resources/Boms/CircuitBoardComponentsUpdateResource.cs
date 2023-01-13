namespace Linn.Purchasing.Resources.Boms
{
    using System.Collections.Generic;

    public class CircuitBoardComponentsUpdateResource : CircuitBoardBaseResource
    {
        public IEnumerable<BoardComponentUpdateResource> Components { get; set; }
    }
}
