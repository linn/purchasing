namespace Linn.Purchasing.Service.ResultHandlers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    using CsvHelper;

    using Linn.Common.Facade.Carter;

    using Microsoft.AspNetCore.Http;

    // todo - move to common
    public class CsvFileResultHandler : IHandler
    {
        public bool CanHandle(object model, string contentType)
        {
            return contentType.IndexOf("csv", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public async Task Handle(
            HttpRequest req, HttpResponse res, object model, CancellationToken cancellationToken)
        {
            dynamic csvResult = model;

            var sw = new StringWriter();

            var writer = new CsvWriter(sw, CultureInfo.InvariantCulture);

            if (csvResult.Data is IEnumerable<IEnumerable> csvGrid)
            {
                foreach (var line in csvGrid)
                {
                    foreach (var field in line)
                    {
                        writer.WriteField(field);
                    }

                    await writer.NextRecordAsync();
                }
            }
            else if (csvResult.Data is IEnumerable arrayModel)
            {
                await writer.WriteRecordsAsync(arrayModel, cancellationToken);
            }
            else
            {
                await writer.WriteRecordsAsync(new[] { csvResult.Data }, cancellationToken);
            }
            
            await writer.FlushAsync();

            res.StatusCode = 200;
            res.ContentType = "text/csv; charset=utf-8";
            res.Headers.ContentDisposition = $"attachment; filename=\"{csvResult.Title}\"";

            await res.WriteAsync(sw.ToString(), cancellationToken: cancellationToken);
        }
    }
}
