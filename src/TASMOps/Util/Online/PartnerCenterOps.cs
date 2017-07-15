// -----------------------------------------------------------------------
// <copyright file="PartnerCenterOps.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace TASMOps.Util.Online
{
    using System;
    using System.Net.Http;
    using TASMOps.Model;

    /// <summary>
    /// Class that has static method to assist in making Partner Center API calls
    /// </summary>
    public class PartnerCenterOps
    {
        /// <summary>
        /// Gets the Azure CSP Rate card
        /// </summary>
        /// <param name="cspCreds">CSP Account credentials object. A token will be generated using these credentials and used for making the online Partner Center API call</param>
        /// <returns> Returns the Aure CSP Rate card in json string format</returns>
        public static string GetAzureCSPRateCard(CSPAccountCreds cspCreds)
        {
            string jsonResult = null;

            try
            {
                // Fetch AzureADToken
                string aadToken = AuthManager.GetAzureADTokenAppUser(cspCreds.CSPPartnerTenantID, cspCreds.CSPPartnerCenterAppId, cspCreds.CSPAdminAgentUserName, cspCreds.CSPAdminAgentPassword, APIConstants.PCAPIUrl);

                // Get Rate Card for CSP
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + aadToken);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("MS-CorrelationId", Guid.NewGuid().ToString());
                client.DefaultRequestHeaders.Add("MS-RequestId", Guid.NewGuid().ToString());
                client.DefaultRequestHeaders.Add("X-Locale", cspCreds.CSPLocale);
                client.Timeout = new TimeSpan(0, APIConstants.APICallDefaultLimit, 0);

                var path = string.Format(APIConstants.PCAzureCSPRateCardAPIUrl, cspCreds.CSPCurrency, cspCreds.CSPRegion);
                Uri uri = new Uri(path);
                HttpResponseMessage response = client.GetAsync(uri).Result;

                if (response.IsSuccessStatusCode)
                {
                    jsonResult = response.Content.ReadAsStringAsync().Result;
                }
                else
                {
                    string jsonErrorResult = response.Content.ReadAsStringAsync().Result;
                    throw new Exception(string.Format(APICallErrorLiterals.PCRateCardFetchNotSuccessRespCodeError, response.StatusCode, jsonErrorResult));
                }
            }
            catch (Exception)
            {
                throw;
            }

            return jsonResult;
        }
    }
}
