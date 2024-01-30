namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System;
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Resources;

    public class AddressResourceBuilder : IBuilder<Address>
    {
        private readonly IRepository<FullAddress, int> fullAddressRepository;

        public AddressResourceBuilder(IRepository<FullAddress, int> fullAddressRepository)
        {
            this.fullAddressRepository = fullAddressRepository;
        }

        public AddressResource Build(Address entity, IEnumerable<string> claims)
        {
            return new AddressResource
                       {
                           AddressId = entity.AddressId,
                           Addressee = entity.Addressee,
                           Addressee2 = entity.Addressee2,
                           Line1 = entity.Line1,
                           Line2 = entity.Line2,
                           Line3 = entity.Line3,
                           Line4 = entity.Line4,
                           PostCode = entity.PostCode,
                           CountryCode = entity.Country?.CountryCode,
                           CountryName = entity.Country?.Name,
                           DateInvalid = entity.DateInvalid?.ToString("o"),
                           FullAddress = entity.FullAddress?.AddressString
                                         ?? this.fullAddressRepository.FindById(entity.AddressId)?.AddressString
                       };
        }

        public string GetLocation(Address p)
        {
            throw new NotImplementedException();
        }

        object IBuilder<Address>.Build(Address entity, IEnumerable<string> claims) => this.Build(entity, claims);
    }
}
