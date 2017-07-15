// -----------------------------------------------------------------------
// <copyright file="AzureResourceRateCalc.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace TASMOps.Util
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using TASMOps.Model;

    /// <summary>
    /// Class that has static method to fetch the Azure resource meters
    /// </summary>
    public class AzureResourceRateCalc
    {
        /// <summary>
        /// Variable to store the meter list for Azure CSP Rate Card
        /// </summary>
        private List<Meter> meterList = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureResourceRateCalc"/> class
        /// </summary>
        /// <param name="meterList">the Azure CSP Rate card list of meters</param>
        public AzureResourceRateCalc(List<Meter> meterList)
        {
            this.meterList = meterList;
        }

        /// <summary>
        /// Gets the rate for the specified Azure resource
        /// </summary>
        /// <param name="resourceInfo">the Azure resource</param>
        /// <returns> Returns the rate for the resource</returns>
        public double GetResourceRate(AzureResourceInfo resourceInfo)
        {
            double rate = 0;

            try
            {
                Meter meter = this.GetResourceMeter(resourceInfo);
                OrderedDictionary meterRates = meter.MeterRates;

                if (meterRates.Count >= 1)
                {
                    rate = (double)meterRates[0];
                }
            }
            catch (Exception)
            {
                throw;
            }

            return rate;
        }

        /// <summary>
        /// Gets the Meter for the specified Azure resource
        /// </summary>
        /// <param name="resourceInfo">the Azure resource</param>
        /// <returns> Returns the Meter for the resource</returns>
        private Meter GetResourceMeter(AzureResourceInfo resourceInfo)
        {
            Meter meter = null;
            try
            {
                meter = this.meterList.Find(x => resourceInfo.MeterCategory.Equals(x.MeterCategory, StringComparison.OrdinalIgnoreCase)
                                            && resourceInfo.MeterSubCategory.Equals(x.MeterSubCategory, StringComparison.OrdinalIgnoreCase)
                                            && resourceInfo.MeterName.Equals(x.MeterName, StringComparison.OrdinalIgnoreCase)
                                            && resourceInfo.MeterRegion.Equals(x.MeterRegion, StringComparison.OrdinalIgnoreCase));

                if (meter == null)
                {
                    meter = this.meterList.Find(x => resourceInfo.MeterCategory.Equals(x.MeterCategory, StringComparison.OrdinalIgnoreCase)
                                            && resourceInfo.MeterSubCategory.Equals(x.MeterSubCategory, StringComparison.OrdinalIgnoreCase)
                                            && resourceInfo.MeterName.Equals(x.MeterName, StringComparison.OrdinalIgnoreCase)
                                            && x.MeterRegion.Equals(string.Empty, StringComparison.OrdinalIgnoreCase));
                }

                if (meter == null)
                {
                    throw new Exception(string.Format(Constants.AzureResourceMeterMissing, resourceInfo.MeterCategory, resourceInfo.MeterSubCategory, resourceInfo.MeterName, resourceInfo.MeterRegion));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return meter;
        }
    }
}
