namespace Linn.Purchasing.Service.Extensions
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using System.Collections.Generic;
    using System.IO;
    using System.Net.Mime;
    using Carter.Response;
    using Common.Serialization;
    
    public static class HttpResponseExtensions
    {
        public static Task FromCsv(
            this HttpResponse response, 
            IEnumerable<IEnumerable<string>> csvData,
            string fileName)
        {
            var stream = new MemoryStream();
            var csvStreamWriter = new CsvStreamWriter(stream);
            csvStreamWriter.WriteModel(csvData);
            return response.FromStream(
                stream, 
                "text/csv", 
                new ContentDisposition {FileName = fileName});
        }
    }
}