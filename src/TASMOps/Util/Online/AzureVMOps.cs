// -----------------------------------------------------------------------
// <copyright file="AzureVMOps.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace TASMOps.Util.Online
{
    using System;
    using System.Collections.Generic;
    using TASMOps.Model;

    /// <summary>
    /// Class that has static methods to fetch the Azure VM SKU list
    /// </summary>
    public class AzureVMOps
    {
        /// <summary>
        /// Gets the Azure VM SKU list
        /// </summary>
        /// <param name="cspCreds">CSP Account credentials object. A token will be generated using these credentials and used for making the online ARM API call</param>
        /// <param name="location">the Azure location</param>
        /// <returns> Returns the list of Azure VM SKUs for the specified location</returns>
        public static List<AzureVMSizeListItem> GetAzureVMSizeList(CSPAccountCreds cspCreds, string location)
        {
            List<AzureVMSizeListItem> sizeList = null;
            try
            {
                // Get AAD Token
                string aadToken = AuthManager.GetAzureADTokenAppUser(cspCreds.CSPCustomerTenantId, cspCreds.CSPARMNativeAppId, cspCreds.CSPAdminAgentUserName, cspCreds.CSPAdminAgentPassword, APIConstants.ARMAPIResourceURL);

                string url = APIConstants.ARMAPIGetAzureVMSizesUrl;
                var path = string.Format(url, APIConstants.ARMAPIURL, cspCreds.CSPAzureSubscriptionId, location, APIConstants.ARMComputeAPIVersion);
                AzureVMSizeList result = ARMAPIHelper.GetARMCall<AzureVMSizeList>(aadToken, path, APIConstants.APICallDefaultLimit);
                sizeList = result.Value;
            }
            catch (Exception)
            {
                throw;
            }

            return sizeList;
        }
    }
}
