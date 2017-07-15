// -----------------------------------------------------------------------
// <copyright file="AADError.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace TASMOps.Model
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// Class that defines the deserialized data for the JSON of the Azure AD Login Error
    /// </summary>
    public class AADError
    {
        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("error_description")]
        public string Error_description { get; set; }

        [JsonProperty("error_codes")]
        public List<int> Error_codes { get; set; }

        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }

        [JsonProperty("trace_id")]
        public string Trace_id { get; set; }

        [JsonProperty("correlation_id")]
        public string Correlation_id { get; set; }
    }
}
