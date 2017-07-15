// -----------------------------------------------------------------------
// <copyright file="RateCard.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace TASMOps.Model
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using Newtonsoft.Json;

    /// <summary>
    /// Class that defines the deserialized data for the JSON of the CSP Azure Rate card
    /// </summary>
    public class RateCard
    {
        [JsonProperty("Meters")]
        public List<Meter> Meters { get; set; }

        [JsonProperty("Currency")]
        public string Currency { get; set; }

        [JsonProperty("Locale")]
        public string Locale { get; set; }

        [JsonProperty("IsTaxIncluded")]
        public bool IsTaxIncluded { get; set; }
    }

    /// <summary>
    /// Class that defines the deserialized data for the JSON of the individual resource meters in the CSP Azure Rate card
    /// </summary>
    public class Meter
    {
        [JsonProperty("id")]
        public string MeterId { get; set; }

        [JsonProperty("name")]
        public string MeterName { get; set; }

        [JsonProperty("category")]
        public string MeterCategory { get; set; }

        [JsonProperty("subcategory")]
        public string MeterSubCategory { get; set; }

        [JsonProperty("unit")]
        public string Unit { get; set; }

        [JsonProperty("region")]
        public string MeterRegion { get; set; }

        [JsonProperty("rates")]
        public OrderedDictionary MeterRates { get; set; }

        [JsonProperty("effectiveDate")]
        public string EffectiveDate { get; set; }

        [JsonProperty("includedQuantity")]
        public double IncludedQuantity { get; set; }
    }
}
