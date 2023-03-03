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
    using Linn.Purchasing.Facade;

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
            dynamic csvResult = model;

            res.ContentType = "text/csv; charset=utf-8";
            res.Headers.ContentDisposition = $"attachment; filename=\"{csvResult.Title}\"";

            var sw = new StringWriter();

            var writer = new CsvWriter(sw, CultureInfo.InvariantCulture);

            if (csvResult.Data is CsvResult<IEnumerable<IEnumerable<string>>> csvGrid)
            {
                foreach (var line in csvGrid.Data)
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
            await res.WriteAsync(sw.ToString(), cancellationToken: cancellationToken);
        }
    }
}
