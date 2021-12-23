namespace Linn.Purchasing.Facade
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public class JsonSerialiser : ISerialiser2
    {
        private readonly JsonSerializerSettings jsonSettings;

        public JsonSerialiser()
        {
            var contractResolver = new DefaultContractResolver
                                       {
                                           NamingStrategy = new CamelCaseNamingStrategy()
                                       };

            this.jsonSettings = new JsonSerializerSettings { ContractResolver = contractResolver };
        }

        public string Serialise(object model)
        {
            return JsonConvert.SerializeObject(model, this.jsonSettings);
        }
    }
}
