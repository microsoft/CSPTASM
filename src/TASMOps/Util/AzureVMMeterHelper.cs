// -----------------------------------------------------------------------
// <copyright file="AzureVMMeterHelper.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace TASMOps.Util
{
    using System;
    using System.Text.RegularExpressions;
    using TASMOps.Model;

    /// <summary>
    /// Class that has static method to get Azure Resource Info objects for specified Azure resources to assist in getting Azure meters
    /// </summary>
    public class AzureVMMeterHelper
    {
        /// <summary>
        /// Get the AzureResourceInfo object for the specified Azure VM SKU
        /// </summary>
        /// <param name="azureVMInstanceType">Azure VM SKU</param>
        /// <param name="operatingSystem">Operating System of the instance</param>
        /// <param name="location">Azure Location</param>
        /// <returns> Returns the AzureResourceInfo object for the specified Azure VM SKU</returns>
        public static AzureResourceInfo GetAzureResourceInfoForAzureVM(string azureVMInstanceType, string operatingSystem, string location)
        {
            AzureResourceInfo resourceInfo = new AzureResourceInfo()
            {
                MeterCategory = AzureResourceMeterConstants.VMComputeMeterCategory,
                MeterName = AzureResourceMeterConstants.VMComputeMeterName,
                MeterRegion = location,
                MeterSubCategory = null
            };

            try
            {
                string modifiedVMSizeString = ModifyVMSizeStringAsPerPricingSpecs(azureVMInstanceType);

                switch (operatingSystem.ToUpper())
                {
                    case "WINDOWS":
                        resourceInfo.MeterSubCategory = string.Format(AzureResourceMeterConstants.VMComputeMeterSubCategoryWindowsString, modifiedVMSizeString, operatingSystem);
                        break;

                    case "LINUX":
                        resourceInfo.MeterSubCategory = string.Format(AzureResourceMeterConstants.VMComputeMeterSubCategoryLinuxString, modifiedVMSizeString);
                        break;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return resourceInfo;
        }

        /// <summary>
        /// Checks if the Azure VM SKU supports premium disks
        /// </summary>
        /// <param name="azureVMListItem">Azure VM SKU Item</param>
        /// <returns> A value indicating if theAzure VM SKU supports premium disks or not</returns>
        public static bool CheckIfAzureVMInstanceTypeIsPremiumSupported(AzureVMSizeListItem azureVMListItem)
        {
            bool isPremiumSupported = false;

            Regex r = new Regex(AzureVMHelperConstants.AzureVMSupportSSDRegex, RegexOptions.IgnoreCase);
            Match m = r.Match(azureVMListItem.Name);
            if (m.Success)
            {
                isPremiumSupported = true;
            }

            return isPremiumSupported;
        }

        /// <summary>
        /// Get the modified Azure VM SKU string as per pricing
        /// </summary>
        /// <param name="vmSize">Azure VM SKU</param>
        /// <returns> Returns the modified Azure VM SKU string as per pricing</returns>
        private static string ModifyVMSizeStringAsPerPricingSpecs(string vmSize)
        {
            string modifiedVMSize = vmSize;

            if (vmSize != null)
            {
                bool isCompletedProcessing = false;

                // Check if A Size, Modify
                if (!isCompletedProcessing)
                {
                    string modifiedSize = null;
                    if (!vmSize.Contains("v2") && GetModifiedVMSizeForASizes(vmSize, out modifiedSize))
                    {
                        isCompletedProcessing = true;
                        modifiedVMSize = modifiedSize;
                    }
                }

                if (!isCompletedProcessing)
                {
                    string modifiedSize = null;
                    if (GetModifiedVMSizeForPremiumSSizes(vmSize, out modifiedSize, AzureVMHelperConstants.AzureVMSupportSSDRegex1))
                    {
                        isCompletedProcessing = true;
                        modifiedVMSize = modifiedSize;
                    }
                }

                if (!isCompletedProcessing)
                {
                    string modifiedSize = null;
                    if (GetModifiedVMSizeForPremiumSSizes(vmSize, out modifiedSize, AzureVMHelperConstants.AzureVMSupportSSDRegex2))
                    {
                        isCompletedProcessing = true;
                        modifiedVMSize = modifiedSize;
                    }
                }
            }

            return modifiedVMSize;
        }

        /// <summary>
        /// Get the modified Azure VM SKU string as per pricing for A Series
        /// </summary>
        /// <param name="vmSize">Azure VM SKU</param>
        /// <param name="modifiedSize">Modified Azure VM SKU passed as an out parameter</param>
        /// <returns> A value indicating if the match was found for A series</returns>
        private static bool GetModifiedVMSizeForASizes(string vmSize, out string modifiedSize)
        {
            bool isCompletedProcessing = false;
            modifiedSize = null;
            Regex r = new Regex(AzureVMHelperConstants.AzureVMASeriesRegex, RegexOptions.IgnoreCase);
            Match m = r.Match(vmSize);
            if (m.Success)
            {
                isCompletedProcessing = true;
                if (m.Groups.Count > 2)
                {
                    string basicOrStandard = m.Groups[1].Value;
                    string size = m.Groups[2].Value;

                    // If VM Size is Basic
                    if (string.Equals(basicOrStandard, AzureVMHelperConstants.AzureVMBasicSeriesString, StringComparison.OrdinalIgnoreCase))
                    {
                        modifiedSize = AzureVMHelperConstants.AzureVMBasicSeriesString + "." + size;
                    }
                    else
                    {
                        modifiedSize = size;
                    }
                }
            }

            return isCompletedProcessing;
        }

        /// <summary>
        /// Get the modified Azure VM SKU string as per pricing for Premium Series
        /// </summary>
        /// <param name="vmSize">Azure VM SKU</param>
        /// <param name="modifiedSize">Modified Azure VM SKU passed as an out parameter</param>
        /// <param name="premiumSRegexString">Regex for premium Azure SKU series type to be matched</param>
        /// <returns> A value indicating if the match was found for premium Azure SKU series type</returns>
        private static bool GetModifiedVMSizeForPremiumSSizes(string vmSize, out string modifiedSize, string premiumSRegexString)
        {
            bool isCompletedProcessing = false;
            modifiedSize = null;

            Regex r = new Regex(premiumSRegexString, RegexOptions.IgnoreCase);
            Match m = r.Match(vmSize);
            if (m.Success)
            {
                isCompletedProcessing = true;
                if (m.Groups.Count > 2)
                {
                    modifiedSize = m.Groups[1].Value + m.Groups[2].Value;
                }
            }

            return isCompletedProcessing;
        }
    }
}
