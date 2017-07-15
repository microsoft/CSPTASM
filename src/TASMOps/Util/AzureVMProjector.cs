// -----------------------------------------------------------------------
// <copyright file="AzureVMProjector.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace TASMOps.Util
{
    using System;
    using System.Collections.Generic;
    using TASMOps.Model;

    /// <summary>
    /// Class that has static method to map the Azure VM SKU for the input specifications provided
    /// </summary>
    public class AzureVMProjector
    {
        /// <summary>
        /// The object that can fetch the rates for Azure resources
        /// </summary>
        private static AzureResourceRateCalc rateCalc;

        /// <summary>
        /// The Azure location
        /// </summary>
        private static string location;

        /// <summary>
        /// Maps the instance to an Azure VM SKU
        /// </summary>
        /// <param name="vmRes">The object that will contain the mapping output</param>
        /// <param name="location">The Azure location</param>
        /// <param name="azureVMSizeList">The list of Azure VM SKUs</param>
        /// <param name="rateCalc">The object that can fetch the rates for Azure resources</param>
        /// <param name="listOfOverrideVMSizes">The list of Input Override Azure VM SKUs if provided</param>
        /// <param name="mappingCoefficientCoresSelection">Mapping Coefficient for CPU Cores</param>
        /// <param name="mappingCoefficientMemorySelection">Mapping Coefficient for Memory</param>
        public static void ProjectAzureVM(VMResult vmRes, string location, List<AzureVMSizeListItem> azureVMSizeList, AzureResourceRateCalc rateCalc, List<OverrideVMSpecs> listOfOverrideVMSizes, double mappingCoefficientCoresSelection, double mappingCoefficientMemorySelection)
        {
            AzureVMProjector.rateCalc = rateCalc;
            AzureVMProjector.location = location;

            try
            {
                int dataDiskCount = 0;
                dataDiskCount = GetDataDiskCount(vmRes);

                // Check if Preferred VM Size Exists
                bool isPreferredVMSizeValid = false;
                AzureVMSizeListItem vmListItemForMatchedOverride = null;

                // Override with VM Specific if provided
                if (!string.IsNullOrWhiteSpace(vmRes.Specs.AzureVMOverride))
                {
                    vmListItemForMatchedOverride = azureVMSizeList.Find(x => x.Name.Equals(vmRes.Specs.AzureVMOverride, StringComparison.OrdinalIgnoreCase));
                    if (vmListItemForMatchedOverride != null)
                    {
                        if (vmListItemForMatchedOverride.MaxDataDiskCount >= dataDiskCount)
                        {
                            isPreferredVMSizeValid = true;
                            vmRes.ProjAzureVM.ComputeHoursRate = GetAzureVMCost(vmListItemForMatchedOverride, vmRes.Specs);
                            vmRes.ProjAzureVM.VMSize = vmListItemForMatchedOverride;
                            vmRes.ProjAzureVM.AzureProjectionComments = AzureVMProjectionCommentsLiterals.ProjectionCommentsVMMapasperSpecificOverride;
                        }
                        else
                        {
                            vmRes.ProjAzureVM.AzureProjectionComments = AzureVMProjectionCommentsLiterals.ProjectionCommentsSpecificOverrideExceedDiskCount;
                        }
                    }
                    else
                    {
                        vmRes.ProjAzureVM.AzureProjectionComments = AzureVMProjectionCommentsLiterals.ProjectionCommentsSpecificOverrideNotFound;
                    }
                }

                // Override with generic Override file if match found and not already mapped by specific override value
                if (!isPreferredVMSizeValid && listOfOverrideVMSizes != null)
                {
                    OverrideVMSpecs matchedOverrideVMDetails = listOfOverrideVMSizes.Find(x => (x.IsValid && vmRes.Specs.CPUCores == x.CPUCores && vmRes.Specs.MemoryInMB == x.MemoryInMB && vmRes.Specs.OperatingSystem.Equals(x.OperatingSystem, StringComparison.OrdinalIgnoreCase) && ((x.HasSSDStorage && vmRes.Specs.SSDStorageInGB > 0) || (!x.HasSSDStorage && vmRes.Specs.SSDStorageInGB == 0)) && (x.NumberOfDataDisks == dataDiskCount) && !string.IsNullOrWhiteSpace(x.AzureVMOverride)));
                    if (matchedOverrideVMDetails != null)
                    {
                        vmListItemForMatchedOverride = azureVMSizeList.Find(x => x.Name.Equals(matchedOverrideVMDetails.AzureVMOverride, StringComparison.OrdinalIgnoreCase));
                        if (vmListItemForMatchedOverride != null)
                        {
                            if (vmListItemForMatchedOverride.MaxDataDiskCount >= dataDiskCount)
                            {
                                isPreferredVMSizeValid = true;
                                vmRes.ProjAzureVM.ComputeHoursRate = GetAzureVMCost(vmListItemForMatchedOverride, vmRes.Specs);
                                vmRes.ProjAzureVM.VMSize = vmListItemForMatchedOverride;
                                vmRes.ProjAzureVM.AzureProjectionComments = AzureVMProjectionCommentsLiterals.ProjectionCommentsVMMappedasperGenericOverride;
                            }
                            else
                            {
                                vmRes.ProjAzureVM.AzureProjectionComments = AzureVMProjectionCommentsLiterals.ProjectionCommentsGenericOverrideExceedDiskCount;
                            }
                        }
                        else
                        {
                            vmRes.ProjAzureVM.AzureProjectionComments = AzureVMProjectionCommentsLiterals.ProjectionCommentsGenericOverrideNotFound;
                        }
                    }
                }

                // If PreferredVMSize does not exist or if PreferredVMSize is not valid, loop thru' the list to project using projection algorithm
                if (!isPreferredVMSizeValid)
                {
                    foreach (AzureVMSizeListItem vmListItem in azureVMSizeList)
                    {
                        bool considerListItem = false;

                        // If Current Azure VM Size does not support Premium Disks and VM contains mapped premium disks - skip the size
                        if (vmRes.ProjAzureVM.PremiumDisks != null && vmRes.ProjAzureVM.PremiumDisks.Count > 0 && !AzureVMMeterHelper.CheckIfAzureVMInstanceTypeIsPremiumSupported(vmListItem))
                        {
                            continue;
                        }

                        // If Current Azure VM Size does is for Premium Disks and VM contains does not contains premium disks - skip the size
                        if ((vmRes.ProjAzureVM.PremiumDisks == null || (vmRes.ProjAzureVM.PremiumDisks != null && vmRes.ProjAzureVM.PremiumDisks.Count == 0)) && AzureVMMeterHelper.CheckIfAzureVMInstanceTypeIsPremiumSupported(vmListItem))
                        {
                            continue;
                        }

                        // Skip if number of data disks mapped is more than maximum of current item
                        if (dataDiskCount > vmListItem.MaxDataDiskCount)
                        {
                            continue;
                        }

                        if (vmListItem.NumberOfCores >= mappingCoefficientCoresSelection * vmRes.Specs.CPUCores)
                        {
                            if (vmListItem.MemoryInMB >= mappingCoefficientMemorySelection * vmRes.Specs.MemoryInMB)
                            {
                                considerListItem = true;
                            }
                        }

                        if (considerListItem)
                        {
                            bool mapCurrentItem = false;
                            double currentVMSizeListItemRate = GetAzureVMCost(vmListItem, vmRes.Specs);
                            if (vmRes.ProjAzureVM.VMSize == null)
                            {
                                mapCurrentItem = true;
                            }
                            else
                            {
                                if (vmRes.ProjAzureVM.ComputeHoursRate > currentVMSizeListItemRate)
                                {
                                    mapCurrentItem = true;
                                }
                            }

                            if (mapCurrentItem)
                            {
                                vmRes.ProjAzureVM.IsMappedasperOverride = false;
                                vmRes.ProjAzureVM.VMSize = vmListItem;
                                vmRes.ProjAzureVM.ComputeHoursRate = currentVMSizeListItemRate;
                            }
                        }
                    }
                }
                else
                {
                    vmRes.ProjAzureVM.IsMappedasperOverride = true;
                }

                if (vmRes.ProjAzureVM.VMSize == null)
                {
                    vmRes.ProjAzureVM.IsNoMapFound = true;
                    vmRes.ProjAzureVM.AzureProjectionComments = AzureVMProjectionCommentsLiterals.ProjectionCommentsVMCannotbeMapped;
                }
                else
                {
                    vmRes.ProjAzureVM.IsNoMapFound = false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the unique list of mapped Azure VM SKUs for Override output
        /// </summary>
        /// <param name="listOfProjectedVMs">The list of mapped Azure VMs</param>
        /// <param name="location">The Azure location</param>
        /// <param name="azureVMSizeList">The list of Azure VM SKUs</param>
        /// <param name="rateCalc">The object that can fetch the rates for Azure resources</param>
        /// <param name="mappingCoefficientCoresSelection">Mapping Coefficient for CPU Cores</param>
        /// <param name="mappingCoefficientMemorySelection">Mapping Coefficient for Memory</param>
        /// <returns> Returns the unique list of mapped Azure VM SKUs for Override output</returns>
        public static List<OverrideVMSpecs> GetUniqueProjectedListOfVMs(List<VMResult> listOfProjectedVMs, string location, List<AzureVMSizeListItem> azureVMSizeList, AzureResourceRateCalc rateCalc, double mappingCoefficientCoresSelection, double mappingCoefficientMemorySelection)
        {
            List<OverrideVMSpecs> uniqueListOfProjectedVMs = new List<OverrideVMSpecs>();

            foreach (VMResult vmRes in listOfProjectedVMs)
            {
                if (!vmRes.Specs.IsValid || vmRes.ProjAzureVM == null)
                {
                    continue;
                }

                bool mappedAzureVMHasSSD = (vmRes.ProjAzureVM.PremiumDisks != null && vmRes.ProjAzureVM.PremiumDisks.Count > 0) ? true : false;
                int dataDiskCount = GetDataDiskCount(vmRes);

                if (!uniqueListOfProjectedVMs.Exists(
                    x => (x.OperatingSystem.Equals(vmRes.Specs.OperatingSystem, StringComparison.OrdinalIgnoreCase)
                            && x.CPUCores == vmRes.Specs.CPUCores
                            && x.MemoryInMB == vmRes.Specs.MemoryInMB
                            && x.NumberOfDataDisks == dataDiskCount
                            && x.HasSSDStorage == mappedAzureVMHasSSD)))
                {
                    VMResult vmResUnique = null;
                    if (vmRes.ProjAzureVM.IsMappedasperOverride)
                    {
                        vmResUnique = GetVMResultforMapwithoutOverride(vmRes);
                        ProjectAzureVM(vmResUnique, location, azureVMSizeList, rateCalc, null, mappingCoefficientCoresSelection, mappingCoefficientMemorySelection);
                    }
                    else
                    {
                        vmResUnique = vmRes;
                    }

                    uniqueListOfProjectedVMs.Add(new OverrideVMSpecs()
                    {
                        OperatingSystem = vmRes.Specs.OperatingSystem,
                        CPUCores = vmRes.Specs.CPUCores,
                        MemoryInMB = vmRes.Specs.MemoryInMB,
                        NumberOfDataDisks = dataDiskCount,
                        HasSSDStorage = mappedAzureVMHasSSD,
                        AzureProjVMSize = vmResUnique.ProjAzureVM.VMSize == null ? string.Empty : vmResUnique.ProjAzureVM.VMSize.Name,
                        AzureProjVMCores = vmResUnique.ProjAzureVM.VMSize == null ? 0 : vmResUnique.ProjAzureVM.VMSize.NumberOfCores,
                        AzureProjVMMemory = vmResUnique.ProjAzureVM.VMSize == null ? 0 : vmResUnique.ProjAzureVM.VMSize.MemoryInMB
                    });
                }
            }

            return uniqueListOfProjectedVMs;
        }

        /// <summary>
        /// Gets the mapping output object for specified input without Override input value
        /// </summary>
        /// <param name="res">The mapping output object with input specifications</param>
        /// <returns> Returns the mapping output object for specified input without Override input value</returns>
        public static VMResult GetVMResultforMapwithoutOverride(VMResult res)
        {
            VMSpecs specsWithoutOverride = new VMSpecs()
            {
                InputOriginalValues = null,
                InstanceName = string.Empty,
                OperatingSystem = res.Specs.OperatingSystem,
                CPUCores = res.Specs.CPUCores,
                MemoryInMB = res.Specs.MemoryInMB,
                SSDStorageInGB = res.Specs.SSDStorageInGB,
                SSDNumOfDisks = res.Specs.SSDNumOfDisks,
                HDDStorageInGB = res.Specs.HDDStorageInGB,
                HDDNumOfDisks = res.Specs.HDDNumOfDisks,
                PricePerMonth = 0,
                AzureVMOverride = string.Empty,
                Comments = string.Empty,
                IsValid = true,
                ValidationMessage = string.Empty
            };

            VMResult vmResWithoutInputOverride = new VMResult(specsWithoutOverride);
            vmResWithoutInputOverride.ProjAzureVM.PremiumDisks = res.ProjAzureVM.PremiumDisks;
            vmResWithoutInputOverride.ProjAzureVM.StandardDisks = res.ProjAzureVM.StandardDisks;

            return vmResWithoutInputOverride;
        }

        /// <summary>
        /// Get the data disk count for the mapped managed disks for the instance
        /// </summary>
        /// <param name="vmRes">The mapping output object</param>
        /// <returns> Returns the data disk count for the mapped managed disks for the instance</returns>
        private static int GetDataDiskCount(VMResult vmRes)
        {
            int dataDiskCount = 0;
            dataDiskCount = (vmRes.ProjAzureVM.PremiumDisks != null && vmRes.ProjAzureVM.PremiumDisks.Count > 0) ? GetDiskCount(vmRes.ProjAzureVM.PremiumDisks) : dataDiskCount;
            dataDiskCount = (vmRes.ProjAzureVM.StandardDisks != null && vmRes.ProjAzureVM.StandardDisks.Count > 0) ? (dataDiskCount + GetDiskCount(vmRes.ProjAzureVM.StandardDisks)) : dataDiskCount;

            // Exclude OS Disk from count
            if (dataDiskCount > 0)
            {
                dataDiskCount--;
            }

            return dataDiskCount;
        }

        /// <summary>
        /// Get the disk count for the specified list of mapped managed disks
        /// </summary>
        /// <param name="disksList">The list of disks mapped</param>
        /// <returns> Returns the disk count for the specified list of mapped managed disks</returns>
        private static int GetDiskCount(List<AzureMappedDisks> disksList)
        {
            int totalDiskCount = 0;
            foreach (AzureMappedDisks mappedDisks in disksList)
            {
                totalDiskCount = totalDiskCount + mappedDisks.DiskCount;
            }

            return totalDiskCount;
        }

        /// <summary>
        /// Get the Azure VM SKU cost
        /// </summary>
        /// <param name="vmListItem">The Azure VM SKU</param>
        /// <param name="specs">The input specifications</param>
        /// <returns> Returns the Azure VM SKU cost</returns>
        private static double GetAzureVMCost(AzureVMSizeListItem vmListItem, VMSpecs specs)
        {
            double rate = 0;
            try
            {
                AzureResourceInfo resourceInfo = AzureVMMeterHelper.GetAzureResourceInfoForAzureVM(vmListItem.Name, specs.OperatingSystem, location);
                rate = rateCalc.GetResourceRate(resourceInfo);
            }
            catch (Exception)
            {
                throw;
            }
            
            return rate;
        }      
    }
}
