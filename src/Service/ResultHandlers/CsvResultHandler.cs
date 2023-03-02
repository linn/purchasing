namespace Linn.Purchasing.Service.ResultHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    using CsvHelper;

    using Linn.Common.Facade.Carter;

    using Microsoft.AspNetCore.Http;

    // todo - move to common
    public class CsvResultHandler : IHandler
    {
        public bool CanHandle(object model, string contentType)
        {
            return contentType.IndexOf("csv", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public async Task Handle(
            HttpRequest req, HttpResponse res, object model, CancellationToken cancellationToken)
        {
            res.ContentType = "text/csv; charset=utf-8";
            res.Headers.ContentDisposition = "attachment; filename=\"data.csv\"";
            var sw = new StringWriter();

            var writer = new CsvWriter(sw, CultureInfo.InvariantCulture);

            // currently only handles lists of lists of strings
            if (model is IEnumerable<IEnumerable<string>> lines)
            {
                foreach (var line in lines)
                {
                    foreach (var field in line)
                    {
                        writer.WriteField(field);
                    }

                    await writer.NextRecordAsync();
                }

                await writer.FlushAsync();
                await res.WriteAsync(sw.ToString(), cancellationToken: cancellationToken);
            }
            else
            {
                // todo - could easily extend this to handle single objects and lists of objects
                throw new NotImplementedException();
            }
        }
    }
}
