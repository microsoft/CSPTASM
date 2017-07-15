// -----------------------------------------------------------------------
// <copyright file="AuthManager.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace TASMOps.Util.Online
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using Newtonsoft.Json;
    using TASMOps.Model;

    /// <summary>
    /// Class that has static methods to fetch the Azure AD Token
    /// </summary>
    public class AuthManager
    {
        /// <summary>
        /// Gets the Azure AD Token using the App + User Authentication option
        /// </summary>
        /// <param name="tenantID">TenantID of the Azure AD from which the token is to be fetched</param>
        /// <param name="appId">The Application ID</param>
        /// <param name="userName">UserName of the User</param>
        /// <param name="password">Password of the User</param>
        /// <param name="resource">resource URL</param>
        /// <returns> Returns the Azure AD Token in string format</returns>
        public static string GetAzureADTokenAppUser(string tenantID, string appId, string userName, string password, string resource)
        {
            string token = null;

            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

                    var content = new FormUrlEncodedContent(new[]
                    {
                    new KeyValuePair<string, string>("resource", resource),
                    new KeyValuePair<string, string>("client_id", appId),
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("username", userName),
                    new KeyValuePair<string, string>("password", password),
                    new KeyValuePair<string, string>("scope", "openid")
                });
                    string aadTokenURL = string.Format(APIConstants.AADURL, tenantID);
                    Uri uri = new Uri(aadTokenURL);
                    var response = httpClient.PostAsync(uri, content).Result;
                    string result = string.Empty;
                    if (response.IsSuccessStatusCode)
                    {
                        result = response.Content.ReadAsStringAsync().Result;
                        AADToken tokendetails = JsonConvert.DeserializeObject<AADToken>(result);
                        token = tokendetails.Access_token;
                    }
                    else
                    {
                        result = response.Content.ReadAsStringAsync().Result;
                        AADError aadError = null;
                        string errorMsg = string.Empty;
                        if (result != null)
                        {
                            aadError = JsonConvert.DeserializeObject<AADError>(result);
                        }

                        if (aadError != null && !string.IsNullOrWhiteSpace(aadError.Error_description))
                        {
                            errorMsg = aadError.Error_description;
                        }
                        else if (!string.IsNullOrWhiteSpace(result))
                        {
                            errorMsg = string.Format(APICallErrorLiterals.AADTokenFetchNotSuccessRespCodeError, result);
                        }
                        else
                        {
                            errorMsg = string.Format(APICallErrorLiterals.AADTokenFetchNotSuccessRespCodeError, string.Empty);
                        }

                        throw new Exception(errorMsg);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return token;
        }
    }
}
