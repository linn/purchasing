namespace Linn.Purchasing.Service.ResultHandlers
{
    using Linn.Purchasing.Resources;

    public class SigningLimitApplicationStateResultHandler : JsonResultHandler2<SigningLimitResource>
    {
        public SigningLimitApplicationStateResultHandler() : base("application/vnd.linn.application-state+json;version=1")
        {
        }
    }
}
