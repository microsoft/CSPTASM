// -----------------------------------------------------------------------
// <copyright file="VMResult.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace TASMOps.Model
{
    using System.Collections.Generic;

    /// <summary>
    /// Class that defines the Mapping output and its properties including the input specifications
    /// </summary>
    public class VMResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VMResult"/> class
        /// </summary>
        /// <param name="specs">Input specifications</param>
        public VMResult(VMSpecs specs)
        {
            this.Specs = specs;
            this.ProjAzureVM = new ProjectedAzureVM()
            {
                IsNoMapFound = false,
                IsMappedasperOverride = false,
                VMSize = null,
                PremiumDisks = null,
                PremiumDisksStr = string.Empty,
                PremiumDisksMonthlyCost = 0,
                StandardDisks = null,
                StandardDisksStr = string.Empty,
                StandardDisksMonthlyCost = 0,
                ComputeHoursRate = 0,
                ComputeHoursMonthlyCost = 0,
                AzureVMTotalMonthlyCost = 0,
                MonthlyGrossMarginEstimates = 0,
                AzureProjectionComments = string.Empty
            };
        }

        /// <summary>
        /// Gets the input specifications
        /// </summary>
        public VMSpecs Specs { get; }

        /// <summary>
        /// Gets or sets the mapping output object
        /// </summary>
        public ProjectedAzureVM ProjAzureVM { get; set; }
    }

    /// <summary>
    /// Class that defines the Mapping output and its properties
    /// </summary>
    public class ProjectedAzureVM
    {
        /// <summary>
        /// Gets or sets a value indicating whether a map for the instance was found or not
        /// </summary>
        public bool IsNoMapFound { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the mapping was as per Override specified if any
        /// </summary>
        public bool IsMappedasperOverride { get; set; }

        /// <summary>
        /// Gets or sets the Azure VM SKU mapped
        /// </summary>
        public AzureVMSizeListItem VMSize { get; set; }

        /// <summary>
        /// Gets or sets a value the list of premium disks mapped for premium storage specified
        /// </summary>
        public List<AzureMappedDisks> PremiumDisks { get; set; }

        /// <summary>
        /// Gets or sets the premium disks formatted as a string
        /// </summary>
        public string PremiumDisksStr { get; set; }

        /// <summary>
        /// Gets or sets the estimated cost of the premium disks
        /// </summary>
        public double PremiumDisksMonthlyCost { get; set; }

        /// <summary>
        /// Gets or sets a value the list of standard disks mapped for standard storage specified
        /// </summary>
        public List<AzureMappedDisks> StandardDisks { get; set; }

        /// <summary>
        /// Gets or sets the standard disks formatted as a string
        /// </summary>
        public string StandardDisksStr { get; set; }

        /// <summary>
        /// Gets or sets the estimated cost of the standard disks
        /// </summary>
        public double StandardDisksMonthlyCost { get; set; }

        /// <summary>
        /// Gets or sets the hourly rate of compute hours component for the mapped Azure VM SKU
        /// </summary>
        public double ComputeHoursRate { get; set; }

        /// <summary>
        /// Gets or sets the estimated cost of compute hours component for the mapped Azure VM
        /// </summary>
        public double ComputeHoursMonthlyCost { get; set; }

        /// <summary>
        /// Gets or sets the estimated cost of the mapped Azure VM
        /// </summary>
        public double AzureVMTotalMonthlyCost { get; set; }

        /// <summary>
        /// Gets or sets the Monthly Gross Margin Estimates
        /// </summary>
        public double MonthlyGrossMarginEstimates { get; set; }

        /// <summary>
        /// Gets or sets the projection comments if any
        /// </summary>
        public string AzureProjectionComments { get; set; }
    }

    /// <summary>
    /// Class that defines the mapping of the storage to managed disks
    /// </summary>
    public class AzureMappedDisks
    {
        /// <summary>
        /// Gets or sets the mapped Azure managed disk
        /// </summary>
        public AzureManagedDisk MappedDisk { get; set; }

        /// <summary>
        /// Gets or sets the disk count mapped
        /// </summary>
        public int DiskCount { get; set; }
    }
}
