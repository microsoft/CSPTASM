// -----------------------------------------------------------------------
// <copyright file="AzureStorageMapper.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace TASMOps.Util
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TASMOps.Model;

    /// <summary>
    /// Class that has static method to map storage to managed disks
    /// </summary>
    public class AzureStorageMapper
    {
        /// <summary>
        /// the list of all premium managed disks options
        /// </summary>
        private static List<AzureManagedDisk> premiumManagedDiskList = new List<AzureManagedDisk>()
        {
            new AzureManagedDisk("P4", @"Premium Storage - Page Blob/P4 (Units)", 32),

            new AzureManagedDisk("P6", @"Premium Storage - Page Blob/P6 (Units)", 64),

            new AzureManagedDisk("P10", @"Premium Storage - Page Blob/P10 (Units)", 128),

            new AzureManagedDisk("P20", @"Premium Storage - Page Blob/P20 (Units)", 512),

            new AzureManagedDisk("P30", @"Premium Storage - Page Blob/P30 (Units)", 1024),

            new AzureManagedDisk("P40", @"Premium Storage - Page Blob/P40 (Units)", 2048),

            new AzureManagedDisk("P50", @"Premium Storage - Page Blob/P50 (Units)", 4095)
        };

        /// <summary>
        /// the list of all standard managed disks options
        /// </summary>
        private static List<AzureManagedDisk> standardManagedDiskList = new List<AzureManagedDisk>()
        {
            new AzureManagedDisk("S4", @"Standard Managed Disk/S4 (Units)", 32),

            new AzureManagedDisk("S6", "Standard Managed Disk/S6 (Units)", 64),

            new AzureManagedDisk("S10", @"Standard Managed Disk/S10 (Units)", 128),

            new AzureManagedDisk("S20", @"Standard Managed Disk/S20 (Units)", 512),

            new AzureManagedDisk("S30", @"Standard Managed Disk/S30 (Units)", 1024),

            new AzureManagedDisk("S40", @"Standard Managed Disk/S40 (Units)", 2048),

            new AzureManagedDisk("S50", @"Standard Managed Disk/S50 (Units)", 4095)
        };

        /// <summary>
        /// Maps the storage to managed disks
        /// </summary>
        /// <param name="specs">the object containing the input specifications</param>
        /// <param name="vmRes">the mapping output object</param>
        /// <param name="osDiskSSDSelection">The size of the OS Disk to be mapped if SSD</param>
        /// <param name="osDiskHDDSelection">The size of the OS Disk to be mapped if HDD</param>
        public static void MapStorage(VMSpecs specs, VMResult vmRes, string osDiskSSDSelection, string osDiskHDDSelection)
        {
            try
            {
                bool isOSDiskPremium = specs.SSDStorageInGB > 0 ? true : false;

                if (specs.SSDStorageInGB > 0)
                {
                    vmRes.ProjAzureVM.PremiumDisks = MapStorageToManagedDisks(premiumManagedDiskList, specs.SSDStorageInGB, specs.SSDNumOfDisks, isOSDiskPremium);
                }

                if (specs.HDDStorageInGB > 0)
                {
                    vmRes.ProjAzureVM.StandardDisks = MapStorageToManagedDisks(standardManagedDiskList, specs.HDDStorageInGB, specs.HDDNumOfDisks, !isOSDiskPremium);
                }

                if (isOSDiskPremium)
                {
                    vmRes.ProjAzureVM.PremiumDisks = AddOSDisk(premiumManagedDiskList, vmRes.ProjAzureVM.PremiumDisks, osDiskSSDSelection);
                }
                else
                {
                    vmRes.ProjAzureVM.StandardDisks = AddOSDisk(standardManagedDiskList, vmRes.ProjAzureVM.StandardDisks, osDiskHDDSelection);
                }

                if (vmRes.ProjAzureVM.PremiumDisks != null && vmRes.ProjAzureVM.PremiumDisks.Count > 0)
                {
                    vmRes.ProjAzureVM.PremiumDisksStr = GetDiskListStr(vmRes.ProjAzureVM.PremiumDisks);
                }

                if (vmRes.ProjAzureVM.StandardDisks != null && vmRes.ProjAzureVM.StandardDisks.Count > 0)
                {
                    vmRes.ProjAzureVM.StandardDisksStr = GetDiskListStr(vmRes.ProjAzureVM.StandardDisks);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the list of display names of premium managed disks
        /// </summary>
        /// <returns> A list of display names of premium managed disks</returns>
        public static List<string> GetSSDManagedDiskDisplayNameList()
        {
            List<string> diskList = new List<string>();
            try
            {
                diskList.AddRange(premiumManagedDiskList.Select(x => x.DiskDisplayName).ToList());
            }
            catch (Exception)
            {
                throw;
            }

            return diskList;
        }

        /// <summary>
        /// Gets the list of display names of standard managed disks
        /// </summary>
        /// <returns> A list of display names of standard managed disks</returns>
        public static List<string> GetHDDManagedDiskDisplayNameList()
        {
            List<string> diskList = new List<string>();
            try
            {
                diskList.AddRange(standardManagedDiskList.Select(x => x.DiskDisplayName).ToList());
            }
            catch (Exception)
            {
                throw;
            }

            return diskList;
        }

        /// <summary>
        /// Gets the mapped managed disks in string format
        /// </summary>
        /// <param name="listofDisks">List of mapped managed disks</param>
        /// <returns> Returns the mapped managed disks in string format</returns>
        private static string GetDiskListStr(List<AzureMappedDisks> listofDisks)
        {
            string disksStr = string.Empty;
            try
            {
                foreach (AzureMappedDisks mappedDisks in listofDisks)
                {
                    disksStr = disksStr + string.Format(Constants.ManagedDiskStrFormat, mappedDisks.DiskCount, mappedDisks.MappedDisk.DiskDisplayName);
                }

                // Check if last two chars is "; " and remove them
                if ((disksStr.Length > 2) && disksStr.Substring(disksStr.Length - 2).Equals("; "))
                {
                    disksStr = disksStr.Remove(disksStr.Length - 2);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return disksStr;
        }

        /// <summary>
        /// Gets the mapped managed disks for storage
        /// </summary>
        /// <param name="listOfDiskOptions">List of managed disks options</param>
        /// <param name="storageInGB">Storage to be mapped</param>
        /// <param name="numOfDisks">Number of disks to be mapped</param>
        /// <param name="addOSDisk">A value indicating Whether OS disk needs to be added or not</param>
        /// <returns> Returns the mapped managed disks</returns>
        private static List<AzureMappedDisks> MapStorageToManagedDisks(List<AzureManagedDisk> listOfDiskOptions, double storageInGB, int numOfDisks, bool addOSDisk)
        {
            List<AzureMappedDisks> mappedDisks = new List<AzureMappedDisks>();

            try
            {
                if (numOfDisks == 0)
                {
                    double storageInGBRem = storageInGB;
                    int currentDepthLevel = 1;
                    for (int i = listOfDiskOptions.Count - 1; i >= 0 && storageInGBRem > 0; i--)
                    {
                        AzureMappedDisks currentDiskMapped = new AzureMappedDisks() { MappedDisk = listOfDiskOptions[i], DiskCount = 0 };
                        if (storageInGBRem >= listOfDiskOptions[i].DiskSizeInGB)
                        {
                            while (storageInGBRem >= listOfDiskOptions[i].DiskSizeInGB)
                            {
                                storageInGBRem = storageInGBRem - listOfDiskOptions[i].DiskSizeInGB;
                                currentDiskMapped.DiskCount++;
                            }

                            if (currentDepthLevel >= DiskConstants.AzureManagedDiskLevelDepthForDefaultMapping)
                            {
                                storageInGBRem = storageInGBRem - listOfDiskOptions[i].DiskSizeInGB;
                                currentDiskMapped.DiskCount++;
                            }
                            else
                            {
                                currentDepthLevel++;
                            }
                        }
                        else if (currentDepthLevel >= DiskConstants.AzureManagedDiskLevelDepthForDefaultMapping)
                        {
                            storageInGBRem = storageInGBRem - listOfDiskOptions[i].DiskSizeInGB;
                            currentDiskMapped.DiskCount++;
                        }

                        if (currentDiskMapped.DiskCount > 0)
                        {
                            mappedDisks.Add(currentDiskMapped);
                        }
                    }
                }
                else
                {
                    double perDiskSize = storageInGB / numOfDisks;
                    for (int i = 0; i < listOfDiskOptions.Count; i++)
                    {
                        if (listOfDiskOptions[i].DiskSizeInGB >= perDiskSize || i == listOfDiskOptions.Count - 1)
                        {
                            int numOfManagedDisksNeeded = (int)Math.Ceiling(storageInGB / listOfDiskOptions[i].DiskSizeInGB);

                            mappedDisks.Add(new AzureMappedDisks() { MappedDisk = listOfDiskOptions[i], DiskCount = numOfManagedDisksNeeded });
                            break;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return mappedDisks;
        }

        /// <summary>
        /// Gets the mapped managed disks for storage including the OS Disk mapped
        /// </summary>
        /// <param name="listOfDiskOptions">List of managed disks options</param>
        /// <param name="mappedDisks">list of data disks mapped</param>
        /// <param name="osDiskSelection">OS disk selection</param>
        /// <returns> Returns the mapped managed disks for storage including the OS Disk mapped</returns>
        private static List<AzureMappedDisks> AddOSDisk(List<AzureManagedDisk> listOfDiskOptions, List<AzureMappedDisks> mappedDisks, string osDiskSelection)
        {
            AzureManagedDisk osMDisk = listOfDiskOptions.First(x => x.DiskDisplayName.Equals(osDiskSelection, StringComparison.OrdinalIgnoreCase));

            bool isOSDiskAdded = false;
            if (mappedDisks == null)
            {
                mappedDisks = new List<AzureMappedDisks>();
            }

            for (int i = 0; i < mappedDisks.Count; i++)
            {
                if (osMDisk.DiskSizeInGB == mappedDisks[i].MappedDisk.DiskSizeInGB)
                {
                    mappedDisks[i].DiskCount++;
                    isOSDiskAdded = true;
                    break;
                }
            }

            if (!isOSDiskAdded)
            {
                mappedDisks.Add(new AzureMappedDisks() { MappedDisk = osMDisk, DiskCount = 1 });
            }

            return mappedDisks;
        }
    }
}
