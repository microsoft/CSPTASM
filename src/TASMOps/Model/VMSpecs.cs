// -----------------------------------------------------------------------
// <copyright file="VMSpecs.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace TASMOps.Model
{
    /// <summary>
    /// Class that defines the data from the input specifications file
    /// </summary>
    public class VMSpecs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VMSpecs"/> class
        /// </summary>
        public VMSpecs()
        {
            this.InputOriginalValues = new VMSpecsInputValues()
            {
                InstanceName = null,
                OperatingSystem = null,
                CPUCores = null,
                Memory = null,
                SSDStorageInGB = null,
                SSDNumOfDisks = null,
                HDDStorageInGB = null,
                HDDNumOfDisks = null,
                PricePerMonth = null,
                AzureVMOverride = null,
                Comments = null
            };
            this.InstanceName = string.Empty;
            this.OperatingSystem = string.Empty;
            this.CPUCores = 0;
            this.MemoryInMB = 0;
            this.SSDStorageInGB = 0;
            this.SSDNumOfDisks = 0;
            this.HDDStorageInGB = 0;
            this.HDDNumOfDisks = 0;
            this.PricePerMonth = 0;
            this.AzureVMOverride = string.Empty;
            this.Comments = string.Empty;
            this.IsValid = false;
            this.ValidationMessage = string.Empty;
        }

        /// <summary>
        /// Gets or sets the unmodified values from the input file 
        /// </summary>
        public VMSpecsInputValues InputOriginalValues { get; set; }

        /// <summary>
        /// Gets or sets the instance name 
        /// </summary>
        public string InstanceName { get; set; }

        /// <summary>
        /// Gets or sets the Operating System value
        /// </summary>
        public string OperatingSystem { get; set; }

        /// <summary>
        /// Gets or sets the CPU Cores value
        /// </summary>
        public int CPUCores { get; set; }

        /// <summary>
        /// Gets or sets the Memory value
        /// </summary>
        public double MemoryInMB { get; set; }

        /// <summary>
        /// Gets or sets the SSD Storage for the instance
        /// </summary>
        public double SSDStorageInGB { get; set; }

        /// <summary>
        /// Gets or sets the number of SSD disks for the instance
        /// </summary>
        public int SSDNumOfDisks { get; set; }

        /// <summary>
        /// Gets or sets the HDD Storage for the instance
        /// </summary>
        public double HDDStorageInGB { get; set; }

        /// <summary>
        /// Gets or sets the number of SSD disks for the instance
        /// </summary>
        public int HDDNumOfDisks { get; set; }

        /// <summary>
        /// Gets or sets the Price per month for the instance
        /// </summary>
        public double PricePerMonth { get; set; }

        /// <summary>
        /// Gets or sets the Azure VM Override for the instance
        /// </summary>
        public string AzureVMOverride { get; set; }

        /// <summary>
        /// Gets or sets the comments value
        /// </summary>
        public string Comments { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the record is valid
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Gets or sets a value the validation message
        /// </summary>
        public string ValidationMessage { get; set; }
    }

    /// <summary>
    /// Class that defines the unmodified data from Input specifications file
    /// </summary>
    public class VMSpecsInputValues
    {
        /// <summary>
        /// Gets or sets the unmodified Instance name value from the input
        /// </summary>
        public string InstanceName { get; set; }

        /// <summary>
        /// Gets or sets the unmodified Operating System value from the input
        /// </summary>
        public string OperatingSystem { get; set; }

        /// <summary>
        /// Gets or sets the unmodified CPU Cores value from the input
        /// </summary>
        public string CPUCores { get; set; }

        /// <summary>
        /// Gets or sets the unmodified Memory value from the input
        /// </summary>
        public string Memory { get; set; }

        /// <summary>
        /// Gets or sets the unmodified SSD Storage value from the input
        /// </summary>
        public string SSDStorageInGB { get; set; }

        /// <summary>
        /// Gets or sets the unmodified SSD Number of disks value from the input
        /// </summary>
        public string SSDNumOfDisks { get; set; }

        /// <summary>
        /// Gets or sets the unmodified HDD Storage value from the input
        /// </summary>
        public string HDDStorageInGB { get; set; }

        /// <summary>
        /// Gets or sets the unmodified HDD number of disks value from the input
        /// </summary>
        public string HDDNumOfDisks { get; set; }

        /// <summary>
        /// Gets or sets the unmodified Price per month value from the input
        /// </summary>
        public string PricePerMonth { get; set; }

        /// <summary>
        /// Gets or sets the unmodified Azure VM Override value from the input
        /// </summary>
        public string AzureVMOverride { get; set; }

        /// <summary>
        /// Gets or sets the unmodified Comments value from the input
        /// </summary>
        public string Comments { get; set; }
    }
}
