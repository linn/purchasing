namespace Linn.Purchasing.Service.ResultHandlers
{
    using Linn.Common.Facade.Carter.Handlers;
    using Linn.Purchasing.Resources;

    public class SigningLimitApplicationStateResultHandler : JsonResultHandler<SigningLimitResource>
    {
        public SigningLimitApplicationStateResultHandler() : base("application/vnd.linn.application-state+json;version=1")
        {
        }
    }
}
