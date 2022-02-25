namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System;
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Resources;

    public class ContactResourceBuilder : IBuilder<Contact>
    {
        public ContactResource Build(Contact entity, IEnumerable<string> claims)
        {
            return new ContactResource
                       {
                          DateInvalid = entity.DateInvalid?.ToString("o"),
                          PhoneNumber = entity.PhoneNumber,
                          DateCreated = entity.DateCreated?.ToString("o"),
                          ContactId = entity.ContactId,
                          JobTitle = entity.JobTitle,
                          PersonId = entity.Person?.Id,
                          Comments = entity.Comments,
                          EmailAddress = entity.EmailAddress,
                          MobileNumber = entity.MobileNumber,
                          FirstName = entity.Person?.FirstName,
                          LastName = entity.Person?.LastName
                       };
        }

        public string GetLocation(Contact p)
        {
            throw new NotImplementedException();
        }

        object IBuilder<Contact>.Build(Contact entity, IEnumerable<string> claims) => this.Build(entity, claims);
    }
}
