// -----------------------------------------------------------------------
// <copyright file="TestAPIUtil.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace TASMOps.Util
{
    using System;
    using TASMOps.Model;
    using TASMOps.Util.Online;

    /// <summary>
    /// Class that has static methods to test the CSP Account credentials provided by getting the Azure AD Token for Partner Center API and ARM API call
    /// </summary>
    public class TestAPIUtil
    {
        /// <summary>
        /// Test Partner Center credentials by calling Partner Center API
        /// </summary>
        /// <param name="creds">CSP Account credentials object. A token will be generated using these credentials and used for making the online Partner Center API call</param>
        /// <param name="errorMsg">If test fails, the out parameter will contain the error message returned</param>
        /// <returns> Returns a value indicating is the test was successful</returns>
        public static bool TestPCAPI(CSPAccountCreds creds, out string errorMsg)
        {
            bool isPCCredsValid = false;
            errorMsg = string.Empty;
            try
            {
                AuthManager.GetAzureADTokenAppUser(creds.CSPPartnerTenantID, creds.CSPPartnerCenterAppId, creds.CSPAdminAgentUserName, creds.CSPAdminAgentPassword, APIConstants.PCAPIUrl);
                isPCCredsValid = true;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                isPCCredsValid = false;
            }

            return isPCCredsValid;
        }

        /// <summary>
        /// Test credentials by getting the Azure AD Token for ARM API
        /// </summary>
        /// <param name="creds">CSP Account credentials object. A token will be generated using these credentials and used for making the online Partner Center API call</param>
        /// <param name="armToken">If test is successful, the token obtained from Azure AD for ARM API call is set in this out parameter</param>
        /// <param name="errorMsg">If test fails, the out parameter will contain the error message returned</param>
        /// <returns>Returns a value indicating is the test was successful</returns>
        public static bool TestARMAPIToken(CSPAccountCreds creds, out string armToken, out string errorMsg)
        {
            bool isARMCredsValid = false;
            errorMsg = string.Empty;
            armToken = string.Empty;
            try
            {
                armToken = AuthManager.GetAzureADTokenAppUser(creds.CSPCustomerTenantId, creds.CSPARMNativeAppId, creds.CSPAdminAgentUserName, creds.CSPAdminAgentPassword, APIConstants.ARMAPIResourceURL);
                isARMCredsValid = true;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                isARMCredsValid = false;
            }

            return isARMCredsValid;
        }

        /// <summary>
        /// Test credentials by calling ARM API
        /// </summary>
        /// <param name="creds">CSP Account credentials object. A token will be generated using these credentials and used for making the online Partner Center API call</param>
        /// <param name="armToken">The token obtained from Azure AD for ARM API call</param>
        /// <param name="errorMsg">If test fails, the out parameter will contain the error message returned</param>
        /// <returns>Returns a value indicating is the test was successful</returns>
        public static bool TestARMAPI(CSPAccountCreds creds, string armToken, out string errorMsg)
        {
            bool isARMCredsValid = false;
            errorMsg = string.Empty;
            try
            {
                string url = APIConstants.ARMAPIGetRGListUrl;
                var path = string.Format(url, APIConstants.ARMAPIURL, creds.CSPAzureSubscriptionId, APIConstants.ARMRGAPIVersion);
                AzureRGList result = ARMAPIHelper.GetARMCall<AzureRGList>(armToken, path, APIConstants.APICallDefaultLimit);
                isARMCredsValid = true;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                isARMCredsValid = false;
            }

            return isARMCredsValid;
        }
    }
}
