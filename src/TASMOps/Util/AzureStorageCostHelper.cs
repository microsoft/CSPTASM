// -----------------------------------------------------------------------
// <copyright file="AzureStorageCostHelper.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace TASMOps.Util
{
    using System;
    using System.Collections.Generic;
    using TASMOps.Model;

    /// <summary>
    /// Class that has static method to fetch the monthly cost for mapped managed disks for an instance
    /// </summary>
    public class AzureStorageCostHelper
    {
        /// <summary>
        /// Loads the monthly cost for the mapped managed disks in the mapping output
        /// </summary>
        /// <param name="vmRes">the object containing the mapping output</param>
        /// <param name="location">the Azure location</param>
        /// <param name="rateCalc">the object that can fetch the rates for Azure resources</param>
        public static void LoadMonthlyCostForManagedDisks(VMResult vmRes, string location, AzureResourceRateCalc rateCalc)
        {
            try
            {
                if (vmRes.ProjAzureVM.PremiumDisks != null && vmRes.ProjAzureVM.PremiumDisks.Count > 0)
                {
                    vmRes.ProjAzureVM.PremiumDisksMonthlyCost = GetCostforSpecifiedListofDisks(vmRes.ProjAzureVM.PremiumDisks, location, rateCalc);
                }

                if (vmRes.ProjAzureVM.StandardDisks != null && vmRes.ProjAzureVM.StandardDisks.Count > 0)
                {
                    vmRes.ProjAzureVM.StandardDisksMonthlyCost = GetCostforSpecifiedListofDisks(vmRes.ProjAzureVM.StandardDisks, location, rateCalc);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Get the cost for the specified list of mapped managed disks
        /// </summary>
        /// <param name="listofDisks">the list of mapped disks</param>
        /// <param name="location">the Azure location</param>
        /// <param name="rateCalc">the object that can fetch the rates for Azure resources</param>
        /// <returns> Returns the total estimated cost for the list of mapped managed disks</returns>
        private static double GetCostforSpecifiedListofDisks(List<AzureMappedDisks> listofDisks, string location, AzureResourceRateCalc rateCalc)
        {
            double totalDiskCost = 0;
            try
            {
                foreach (AzureMappedDisks managedDisks in listofDisks)
                {
                    AzureResourceInfo resourceInfo = new AzureResourceInfo()
                    {
                        MeterCategory = AzureResourceMeterConstants.VMManagedDiskMeterCategory,
                        MeterName = managedDisks.MappedDisk.DiskMeterName,
                        MeterRegion = location,
                        MeterSubCategory = AzureResourceMeterConstants.VMManagedDiskMeterSubCategory
                    };

                    totalDiskCost = totalDiskCost + (rateCalc.GetResourceRate(resourceInfo) * managedDisks.DiskCount);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return totalDiskCost;
        }
    }
}
