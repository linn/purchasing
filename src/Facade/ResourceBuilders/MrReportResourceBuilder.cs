﻿namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Domain.Exceptions;
    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Purchasing.Domain.LinnApps.MaterialRequirements;
    using Linn.Purchasing.Resources.MaterialRequirements;

    using Microsoft.AspNetCore.Routing.Matching;

    using MoreLinq.Extensions;

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
                HasProductionRequirement = entity.HasProductionRequirement,
                HasDeliveryForecast = entity.HasDeliveryForecast,
                VendorManager = entity.VendorManager,
                VendorManagerInitials = entity.VendorManagerInitials,
                Details = this.BuildDetails(entity, runWeekNumber),
                Links = this.BuildHeaderLinks(entity).ToArray()
            };
        }

        private IEnumerable<MrDetailResource> BuildDetails(MrHeader header, int runWeekNumber)
        {
            var detailResources = new List<MrDetailResource>();
            detailResources.AddRange(this.CreateDetails("Week", 6, 0));
            detailResources.AddRange(this.CreateDetails("Ending", 6, 10));
            if (header.HasProductionRequirement == "Y")
            {
                 detailResources.AddRange(this.CreateDetails("Production Reqt", 6, 20));
            }
            
            detailResources.AddRange(this.CreateDetails("Stock", 6, 30));
            detailResources.AddRange(this.CreateDetails("Min Rail", 6, 80));
            detailResources.AddRange(this.CreateDetails("Max Rail", 6, 90));

            foreach (var detail in header.MrDetails)
            {
                var relativeWeek = detail.LinnWeekNumber < 0
                                       ? -1
                                       : Math.Floor((decimal)(detail.LinnWeekNumber - runWeekNumber) - (detail.Segment * 13));
                this.SetValue(
                    detailResources,
                    "Production Reqt",
                    detail.ProductionRequirement,
                    null,
                    relativeWeek,
                    detail.Segment);

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
                    detail.Segment);

                this.SetValue(
                    detailResources,
                    "Stock",
                    detail.Stock,
                    null,
                    relativeWeek,
                    detail.Segment);

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
            }

            return detailResources.OrderBy(a => a.Segment).ThenBy(b => b.DisplaySequence);
        }

        private void SetValue(
            IEnumerable<MrDetailResource> detailResources,
            string title,
            decimal? value,
            string textValue,
            decimal relativeWeek,
            int segment)
        {
            var detail = detailResources.FirstOrDefault(x => x.Title == title && x.Segment == segment);
            if (detail == null)
            {
                return;
            }

            this.SetRelativeWeekValue(detail, relativeWeek, value, textValue);
        }

        private IEnumerable<MrDetailResource> CreateDetails(string title, int segments, int sequence)
        {
            for (int j = 0; j < segments; j++)
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
        }

        private void SetRelativeWeekValue(MrDetailResource detail, decimal relativeWeek, decimal? value, string textValue)
        {
            var stringValue = string.IsNullOrEmpty(textValue) ? value.ToString() : textValue;
            switch (relativeWeek)
            {
                case -1:
                    detail.ImmediateItem = new MrDetailItemResource { Tag = "TagValue", Value = value, TextValue = textValue };
                    detail.Immediate = stringValue;
                    break;
                case 0:
                    detail.Week0 = stringValue;
                    break;
                case 1:
                    detail.Week1 = stringValue;
                    break;
                case 2:
                    detail.Week2 = stringValue;
                    break;
                case 3:
                    detail.Week3 = stringValue;
                    break;
                case 4:
                    detail.Week4 = stringValue;
                    break;
                case 5:
                    detail.Week5 = stringValue;
                    break;
                case 6:
                    detail.Week6 = stringValue;
                    break;
                case 7:
                    detail.Week7 = stringValue;
                    break;
                case 8:
                    detail.Week8 = stringValue;
                    break;
                case 9:
                    detail.Week9 = stringValue;
                    break;
                case 10:
                    detail.Week10 = stringValue;
                    break;
                case 11:
                    detail.Week11 = stringValue;
                    break;
                case 12:
                    detail.Week12 = stringValue;
                    break;
                default:
                    throw new DomainException("Invalid relative week calculation");
            }
        }
    }
}
