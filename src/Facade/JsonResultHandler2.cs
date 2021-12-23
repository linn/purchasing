namespace Linn.Purchasing.Facade
{
    using System;

    public class JsonResultHandler2<T> : ResultHandler2<T>
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
