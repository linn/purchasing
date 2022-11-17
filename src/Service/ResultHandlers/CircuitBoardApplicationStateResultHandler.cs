namespace Linn.Purchasing.Service.ResultHandlers
{
    using Linn.Common.Facade.Carter.Handlers;
    using Linn.Purchasing.Resources.Boms;

    public class CircuitBoardApplicationStateResultHandler : JsonResultHandler<CircuitBoardResource>
    {
        public CircuitBoardApplicationStateResultHandler() : base("application/vnd.linn.application-state+json;version=1") 
        {
        }
    }
}
