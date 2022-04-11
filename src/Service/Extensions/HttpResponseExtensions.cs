namespace Linn.Purchasing.Service.Extensions
{
    using System.Collections.Generic;
    using System.IO;
    using System.Net.Mime;
    using System.Threading.Tasks;
    
    using Carter.Response;
    using Common.Serialization;
    
    using Microsoft.AspNetCore.Http;
    
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
            stream.Position = 0;
            return response.FromStream(
                stream, 
                "text/csv", 
                new ContentDisposition { FileName = fileName });
        }
    }
}
