// -----------------------------------------------------------------------
// <copyright file="AzureVMCostHelper.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace TASMOps.Util
{
    using System;
    using TASMOps.Model;

    /// <summary>
    /// Class that has static method to load monthly cost for Azure VM
    /// </summary>
    public class AzureVMCostHelper
    {
        /// <summary>
        /// Loads the monthly cost for the compute hours component of the mapped Azure VM
        /// </summary>
        /// <param name="vmRes">The mapping output object</param>
        /// <param name="hoursinaMonth">Hours in a month</param>
        public static void LoadComputeHoursMonthlyCost(VMResult vmRes, int hoursinaMonth)
        {
            try
            {
                if (vmRes.ProjAzureVM != null && !vmRes.ProjAzureVM.IsNoMapFound)
                {
                    vmRes.ProjAzureVM.ComputeHoursMonthlyCost = vmRes.ProjAzureVM.ComputeHoursRate * hoursinaMonth;
                    vmRes.ProjAzureVM.AzureVMTotalMonthlyCost = vmRes.ProjAzureVM.ComputeHoursMonthlyCost + vmRes.ProjAzureVM.PremiumDisksMonthlyCost + vmRes.ProjAzureVM.StandardDisksMonthlyCost;
                    vmRes.ProjAzureVM.MonthlyGrossMarginEstimates = vmRes.Specs.PricePerMonth - vmRes.ProjAzureVM.AzureVMTotalMonthlyCost;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
