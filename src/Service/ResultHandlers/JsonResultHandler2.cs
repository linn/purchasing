namespace Linn.Purchasing.Service.ResultHandlers
{
    using System;

    using Linn.Common.Facade.Carter.Handlers;
    using Linn.Common.Facade.Carter.Serialisers;

    public class JsonResultHandler2<T> : ResultHandler<T>
    {
        public JsonResultHandler2() : base("application/json", new JsonSerialiser())
        {
        }

        public JsonResultHandler2(string contentType) : base(contentType, new JsonSerialiser())
        {
        }

        public override Func<T, string> GenerateLocation => r => string.Empty;
    }
}
