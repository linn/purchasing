namespace Linn.Purchasing.Facade.Extensions
{
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Resources.Boms;

    public static class BoardComponentUpdateResourceExtensions
    {
        public static BoardComponent ToDomain(this BoardComponentUpdateResource resource)
        {
            return new BoardComponent
                       {
                           BoardCode = resource.BoardCode,
                           BoardLine = resource.BoardLine,
                           CRef = resource.CRef,
                           PartNumber = resource.PartNumber,
                           AssemblyTechnology = resource.AssemblyTechnology,
                           ChangeState = resource.ChangeState,
                           FromLayoutVersion = resource.FromLayoutVersion,
                           FromRevisionVersion = resource.FromRevisionVersion,
                           ToLayoutVersion = resource.ToLayoutVersion,
                           ToRevisionVersion = resource.ToRevisionVersion,
                           AddChangeId = resource.AddChangeId,
                           DeleteChangeId = resource.DeleteChangeId,
                           Quantity = resource.Quantity
                       };
        }
    }
}
