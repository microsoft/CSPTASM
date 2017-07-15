// -----------------------------------------------------------------------
// <copyright file="AzureRGList.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace TASMOps.Model
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// Class that defines the deserialized data for the JSON of the List of resource groups
    /// </summary>
    public class AzureRGList
    {
        [JsonProperty("value")]
        public List<ResourceGroup> Value { get; set; }
    }

    public class Properties
    {
        [JsonProperty("provisioningState")]
        public string ProvisioningState { get; set; }
    }

    public class Tags
    {
    }

    public class ResourceGroup
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("properties")]
        public Properties Properties { get; set; }

        [JsonProperty("tags")]
        public Tags Tags { get; set; }
    }
}
