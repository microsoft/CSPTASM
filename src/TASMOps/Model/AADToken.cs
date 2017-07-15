// -----------------------------------------------------------------------
// <copyright file="AADToken.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace TASMOps.Model
{
    using Newtonsoft.Json;

    /// <summary>
    /// Class that defines the deserialized data for the JSON of the Azure AD Token
    /// </summary>
    public class AADToken
    {
        [JsonProperty("token_type")]
        public string Token_type { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }

        [JsonProperty("expires_in")]
        public string Expires_in { get; set; }

        [JsonProperty("ext_expires_in")]
        public string Ext_expires_in { get; set; }

        [JsonProperty("expires_on")]
        public string Expires_on { get; set; }

        [JsonProperty("not_before")]
        public string Not_before { get; set; }

        [JsonProperty("resource")]
        public string Resource { get; set; }

        [JsonProperty("access_token")]
        public string Access_token { get; set; }

        [JsonProperty("refresh_token")]
        public string Refresh_token { get; set; }

        [JsonProperty("id_token")]
        public string Id_token { get; set; }
    }
}
