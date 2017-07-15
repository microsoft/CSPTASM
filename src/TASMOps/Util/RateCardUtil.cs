// -----------------------------------------------------------------------
// <copyright file="RateCardUtil.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace TASMOps.Util
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json;
    using TASMOps.Model;
    using TASMOps.Util.Online;

    /// <summary>
    /// Class that has static methods to fetch the CSP Azure Rate Card
    /// </summary>
    public class RateCardUtil
    {
        /// <summary>
        /// Get the Azure CSP Rate Card Meter List from the Rate Card JSON specified as input
        /// </summary>
        /// <param name="rateCard">The string containing the JSON for Azure CSP Rate Card</param>
        /// <param name="currency">CSP Currency</param>
        /// <returns> Returns the Azure CSP Rate Card Meter List</returns>
        public static List<Meter> GetRateCard(string rateCard, out string currency)
        {
            List<Meter> rateCardMetersList = null;
            currency = string.Empty;

            // Load from string
            try
            {
                RateCard card = JsonConvert.DeserializeObject<RateCard>(rateCard);

                if (card != null && card.Meters != null && card.Meters.Count() > 0)
                {
                    rateCardMetersList = card.Meters;
                    currency = card.Currency;
                }
                else
                {
                    throw new Exception(APICallErrorLiterals.RateCardNullOrZeroRecordsError);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return rateCardMetersList;
        }

        /// <summary>
        /// Get the Azure CSP Rate Card Meter List
        /// </summary>
        /// <param name="cspCreds">CSP Account credentials object. A token will be generated using these credentials and used for making the online Partner Center API call</param>
        /// <returns> Returns the Azure CSP Rate Card in form of JSON string</returns>
        public static string GetRateCard(CSPAccountCreds cspCreds)
        {
            string rateCardStr = string.Empty;

            try
            {
                rateCardStr = PartnerCenterOps.GetAzureCSPRateCard(cspCreds);
            }
            catch (Exception)
            {
                throw;
            }

            return rateCardStr;
        }
    }
}
