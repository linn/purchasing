namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Domain.Exceptions;
    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Purchasing.Domain.LinnApps.MaterialRequirements;
    using Linn.Purchasing.Resources.MaterialRequirements;

    public class MrReportResourceBuilder : IBuilder<MrReport>
    {
        public MrReportResource Build(MrReport entity, IEnumerable<string> claims)
        {
            return new MrReportResource
                       {
                           Results = entity.Headers.Select(h => this.BuildHeader(h, entity.RunWeekNumber))
                       };
        }

        public string GetLocation(MrReport entity)
        {
            throw new NotImplementedException();
        }

        object IBuilder<MrReport>.Build(MrReport entity, IEnumerable<string> claims) => this.Build(entity, claims);

        private MrHeaderResource BuildHeader(MrHeader entity, int runWeekNumber)
        {
            return new MrHeaderResource
            {
                JobRef = entity.JobRef,
                PartNumber = entity.PartNumber,
                PartDescription = entity.PartDescription,
                QuantityInStock = entity.QuantityInStock,
                QuantityForSpares = entity.QuantityForSpares,
                QuantityInInspection = entity.QuantityInInspection,
                QuantityFaulty = entity.QuantityFaulty,
                QuantityAtSupplier = entity.QuantityAtSupplier,
                PreferredSupplierId = entity.PreferredSupplierId,
                PreferredSupplierName = entity.PreferredSupplierName,
                AnnualUsage = entity.AnnualUsage,
                BaseUnitPrice = entity.BaseUnitPrice,
                OurUnits = entity.OurUnits,
                OrderUnits = entity.OrderUnits,
                LeadTimeWeeks = entity.LeadTimeWeeks,
                CurrencyCode = entity.CurrencyCode,
                CurrencyUnitPrice = entity.CurrencyUnitPrice,
                MinimumOrderQuantity = entity.MinimumOrderQuantity,
                MinimumDeliveryQuantity = entity.MinimumDeliveryQuantity,
                OrderIncrement = entity.OrderIncrement,
                VendorManager = entity.VendorManager,
                VendorManagerInitials = entity.VendorManagerInitials,
                Planner = entity.Planner,
                Details = this.BuildDetails(entity, runWeekNumber),
                Links = this.BuildHeaderLinks(entity).ToArray()
            };
        }

        private IEnumerable<MrDetailResource> BuildDetails(MrHeader header, int runWeekNumber)
        {
            var detailResources = this.BuildResourcesTitles(header);
            var leadTimeWeek = 0;
            var dangerWeek = 0;

            if (header.LeadTimeWeeks.HasValue)
            {
                leadTimeWeek = runWeekNumber + header.LeadTimeWeeks.Value;
            }

            if (header.WeeksUntilDangerous.HasValue)
            {
                dangerWeek = runWeekNumber + header.WeeksUntilDangerous.Value;
            }

            foreach (var detail in header.MrDetails)
            {
                var relativeWeek = this.CalculateRelativeWeek(detail.LinnWeekNumber, detail.Segment, runWeekNumber);

                var tags = new List<MrTag>();

                if (leadTimeWeek == detail.LinnWeekNumber)
                {
                    tags.Add(new MrTag("Ending", "redBoxOutline"));
                }

                if (dangerWeek == detail.LinnWeekNumber)
                {
                    tags.Add(new MrTag("Stock", "redBoxOutline"));
                }

                this.SetDetailValuesForWeek(detailResources, relativeWeek, detail, tags);
            }

            return detailResources.OrderBy(a => a.Segment).ThenBy(b => b.DisplaySequence);
        }

        private decimal CalculateRelativeWeek(int linnWeekNumber, int segment, int runWeekNumber)
        {
            return linnWeekNumber < 0
                ? -1
                : Math.Floor((decimal)(linnWeekNumber - runWeekNumber) - (segment * 13));
        }

        private void SetDetailValuesForWeek(
            IList<MrDetailResource> detailResources,
            decimal relativeWeek,
            MrDetail detail,
            IList<MrTag> tags)
        {
            this.SetValue(
                detailResources,
                "Week",
                null,
                detail.WeekAndYear,
                relativeWeek,
                detail.Segment);
            this.SetValue(
                detailResources,
                "Ending",
                null,
                detail.WeekEnding,
                relativeWeek,
                detail.Segment,
                tags.FirstOrDefault(a => a.Title == "Ending"));
            this.SetValue(
                detailResources,
                "Fixed Build",
                detail.FixedBuild,
                null,
                relativeWeek,
                detail.Segment);
            this.SetValue(
                detailResources,
                "Assumed Build",
                detail.AssumedBuild,
                null,
                relativeWeek,
                detail.Segment);
            this.SetValue(
                detailResources,
                "Trigger Build",
                detail.TriggerBuild,
                null,
                relativeWeek,
                detail.Segment);
            this.SetValue(
                detailResources,
                "Purchases",
                detail.PurchaseOrders,
                null,
                relativeWeek,
                detail.Segment);
            this.SetValue(
                detailResources,
                "Unauthorised POs",
                detail.UnauthorisedPurchaseOrders,
                null,
                relativeWeek,
                detail.Segment);
            this.SetValue(
                detailResources,
                "Assumed Purchases",
                detail.AssumedPurchaseOrders,
                null,
                relativeWeek,
                detail.Segment);
            this.SetValue(
                detailResources,
                "Sales Orders",
                detail.SalesOrders,
                null,
                relativeWeek,
                detail.Segment);
            this.SetValue(
                detailResources,
                "Sales Forecast",
                detail.DeliveryForecast,
                null,
                relativeWeek,
                detail.Segment);
            this.SetValue(
                detailResources,
                "Production Reqt",
                detail.ProductionRequirement,
                null,
                relativeWeek,
                detail.Segment);
            this.SetValue(
                detailResources,
                "Prod For Spares",
                detail.ProductionRequirementForSpares,
                null,
                relativeWeek,
                detail.Segment);
            this.SetValue(
                detailResources,
                "Prod For NonProd",
                detail.ProductionRequirementForNonProduction,
                null,
                relativeWeek,
                detail.Segment);
            this.SetValue(
                detailResources,
                "Non Prod Reqt",
                detail.NonProductionRequirement,
                null,
                relativeWeek,
                detail.Segment);
            this.SetValue(
                detailResources,
                "Spares Reqt",
                detail.SparesRequirement,
                null,
                relativeWeek,
                detail.Segment);
            this.SetValue(
                detailResources,
                "Status",
                null,
                detail.Status,
                relativeWeek,
                detail.Segment);
            this.SetValue(
                detailResources,
                "Stock",
                detail.Stock,
                null,
                relativeWeek,
                detail.Segment,
                tags.FirstOrDefault(a => a.Title == "Stock"));
            this.SetValue(
                detailResources,
                "Min Rail",
                detail.MinRail,
                null,
                relativeWeek,
                detail.Segment);
            this.SetValue(
                detailResources,
                "Max Rail",
                detail.MaxRail,
                null,
                relativeWeek,
                detail.Segment);
            this.SetValue(
                detailResources,
                "Ideal Stock",
                detail.IdealStock,
                null,
                relativeWeek,
                detail.Segment);
            this.SetValue(
                detailResources,
                "Recom Orders",
                detail.RecommendedOrders,
                null,
                relativeWeek,
                detail.Segment);
            this.SetValue(
                detailResources,
                "Recom Stock",
                detail.RecommenedStock,
                null,
                relativeWeek,
                detail.Segment);
        }

        private IList<MrDetailResource> BuildResourcesTitles(MrHeader header, bool showAllLines = false)
        {
            var detailResources = new List<MrDetailResource>();
            detailResources.AddRange(this.CreateDetails("Week", 6, 0));
            detailResources.AddRange(this.CreateDetails("Ending", 6, 10));
            if (header.HasFixedBuild == "Y")
            {
                detailResources.AddRange(this.CreateDetails("Fixed Build", 6, 20));
            }

            if (header.HasTriggerBuild == "Y")
            {
                detailResources.AddRange(this.CreateDetails("Trigger Build", 6, 100));
            }

            if (header.HasAssumedBuild == "Y")
            {
                detailResources.AddRange(this.CreateDetails("Assumed Build", 6, 110));
            }

            if (header.HasPurchaseOrders == "Y")
            {
                detailResources.AddRange(this.CreateDetails("Purchases", 6, 150));
            }

            if (header.HasUnauthPurchaseOrders == "Y")
            {
                detailResources.AddRange(this.CreateDetails("Unauthorised POs", 6, 160));
            }

            if (header.HasAssumedPurchaseOrders == "Y")
            {
                detailResources.AddRange(this.CreateDetails("Assumed Purchases", 6, 190));
            }

            if (header.HasDeliveryForecast == "Y")
            {
                detailResources.AddRange(this.CreateDetails("Sales Forecast", 6, 200));
            }

            if (header.HasSalesOrders == "Y")
            {
                detailResources.AddRange(this.CreateDetails("Sales Orders", 6, 300));
            }

            if (header.HasProductionRequirement == "Y")
            {
                detailResources.AddRange(this.CreateDetails("Production Reqt", 6, 400));
            }

            if (header.HasProductionRequirementForSpares == "Y")
            {
                detailResources.AddRange(this.CreateDetails("Prod For Spares", 6, 500));
            }

            if (header.HasProductionRequirementForNonProduction == "Y")
            {
                detailResources.AddRange(this.CreateDetails("Prod For NonProd", 6, 600));
            }

            if (header.HasNonProductionRequirement == "Y")
            {
                detailResources.AddRange(this.CreateDetails("Non Prod Reqt", 6, 700));
            }

            if (header.HasSparesRequirement == "Y")
            {
                detailResources.AddRange(this.CreateDetails("Spares Reqt", 6, 900));
            }

            detailResources.AddRange(this.CreateDetails("Status", 6, 990));
            detailResources.AddRange(this.CreateDetails("Stock", 6, 1000));
            detailResources.AddRange(this.CreateDetails("Min Rail", 6, 1100));
            detailResources.AddRange(this.CreateDetails("Max Rail", 6, 1200));

            if (header.PreferredSupplierId.HasValue && header.PreferredSupplierId != 4415)
            {
                if (showAllLines)
                {
                    detailResources.AddRange(this.CreateDetails("Ideal Stock", 6, 1300));
                    detailResources.AddRange(this.CreateDetails("Recom Stock", 6, 1500));
                }

                detailResources.AddRange(this.CreateDetails("Recom Orders", 6, 1400));
            }

            return detailResources;
        }

        private void SetValue(
            IEnumerable<MrDetailResource> detailResources,
            string title,
            decimal? value,
            string textValue,
            decimal relativeWeek,
            int segment,
            MrTag tag = null)
        {
            var detail = detailResources.FirstOrDefault(x => x.Title == title && x.Segment == segment);
            if (detail == null)
            {
                return;
            }

            this.SetRelativeWeekValue(detail, relativeWeek, value, textValue, tag?.Tag);
        }

        private IEnumerable<MrDetailResource> CreateDetails(string title, int segments, int sequence)
        {
            for (var j = 0; j < segments; j++)
            {
                yield return new MrDetailResource { DisplaySequence = sequence, Segment = j, Title = title };
            }
        }

        private IEnumerable<LinkResource> BuildHeaderLinks(MrHeader entity)
        {
            if (entity == null)
            {
                yield break;
            }

            yield return new LinkResource
                             {
                                 Rel = "part-used-on",
                                 Href = $"/purchasing/material-requirements/used-on-report?partNumber={entity.PartNumber}"
                             };
            yield return new LinkResource { Rel = "part", Href = $"/parts/{entity.PartId}" };
            yield return new LinkResource { Rel = "part-supplier", Href = $"/purchasing/part-suppliers/record?partId={entity.PartId}&supplierId={entity.PreferredSupplierId}" };
        }

        private void SetRelativeWeekValue(
            MrDetailResource detail,
            decimal relativeWeek,
            decimal? value,
            string textValue,
            string tag)
        {
            var stringValue = string.IsNullOrEmpty(textValue) ? value.ToString() : textValue;
            switch (relativeWeek)
            {
                case -1:
                    detail.ImmediateItem = new MrDetailItemResource { Tag = tag, Value = value, TextValue = textValue };
                    detail.Immediate = stringValue;
                    break;
                case 0:
                    detail.Week0Item = new MrDetailItemResource { Tag = tag, Value = value, TextValue = textValue };
                    detail.Week0 = stringValue;
                    break;
                case 1:
                    detail.Week1Item = new MrDetailItemResource { Tag = tag, Value = value, TextValue = textValue };
                    detail.Week1 = stringValue;
                    break;
                case 2:
                    detail.Week2Item = new MrDetailItemResource { Tag = tag, Value = value, TextValue = textValue };
                    detail.Week2 = stringValue;
                    break;
                case 3:
                    detail.Week3Item = new MrDetailItemResource { Tag = tag, Value = value, TextValue = textValue };
                    detail.Week3 = stringValue;
                    break;
                case 4:
                    detail.Week4Item = new MrDetailItemResource { Tag = tag, Value = value, TextValue = textValue };
                    detail.Week4 = stringValue;
                    break;
                case 5:
                    detail.Week5Item = new MrDetailItemResource { Tag = tag, Value = value, TextValue = textValue };
                    detail.Week5 = stringValue;
                    break;
                case 6:
                    detail.Week6Item = new MrDetailItemResource { Tag = tag, Value = value, TextValue = textValue };
                    detail.Week6 = stringValue;
                    break;
                case 7:
                    detail.Week7Item = new MrDetailItemResource { Tag = tag, Value = value, TextValue = textValue };
                    detail.Week7 = stringValue;
                    break;
                case 8:
                    detail.Week8Item = new MrDetailItemResource { Tag = tag, Value = value, TextValue = textValue };
                    detail.Week8 = stringValue;
                    break;
                case 9:
                    detail.Week9Item = new MrDetailItemResource { Tag = tag, Value = value, TextValue = textValue };
                    detail.Week9 = stringValue;
                    break;
                case 10:
                    detail.Week10Item = new MrDetailItemResource { Tag = tag, Value = value, TextValue = textValue };
                    detail.Week10 = stringValue;
                    break;
                case 11:
                    detail.Week11Item = new MrDetailItemResource { Tag = tag, Value = value, TextValue = textValue };
                    detail.Week11 = stringValue;
                    break;
                case 12:
                    detail.Week12Item = new MrDetailItemResource { Tag = tag, Value = value, TextValue = textValue };
                    detail.Week12 = stringValue;
                    break;
                default:
                    throw new DomainException("Invalid relative week calculation");
            }
        }
    }
}
