namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Resources;

    public class SigningLimitResourceBuilder : IBuilder<SigningLimit>
    {
        public SigningLimitResource Build(SigningLimit signingLimit, IEnumerable<string> claims)
        {
            return new SigningLimitResource
            {
                User = new EmployeeResource { FullName = signingLimit.User.FullName, Id = signingLimit.User.Id },
                ProductionLimit = signingLimit.ProductionLimit,
                UserNumber = signingLimit.UserNumber,
                SundryLimit = signingLimit.SundryLimit,
                Unlimited = signingLimit.Unlimited,
                ReturnsAuthorisation = signingLimit.ReturnsAuthorisation,
                Links = this.BuildLinks(signingLimit, claims).ToArray()
            };
        }

        public string GetLocation(SigningLimit p)
        {
            return $"/purchasing/signing-limits/{p.UserNumber}";
        }

        object IBuilder<SigningLimit>.Build(SigningLimit signingLimit, IEnumerable<string> claims) => this.Build(signingLimit, claims);

        private IEnumerable<LinkResource> BuildLinks(SigningLimit signingLimit, IEnumerable<string> claims)
        {
            yield return new LinkResource { Rel = "self", Href = this.GetLocation(signingLimit) };
        }
    }
}
