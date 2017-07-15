// -----------------------------------------------------------------------
// <copyright file="ARMAPIError.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace TASMOps.Model
{
    using Newtonsoft.Json;

    /// <summary>
    /// Class that defines the deserialized data for the JSON of the ARM API Error
    /// </summary>
    public class ARMAPIError
    {
        [JsonProperty("error")]
        public Error Error { get; set; }
    }

    public class Error
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
