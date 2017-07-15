// -----------------------------------------------------------------------
// <copyright file="OverrideVMSpecs.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace TASMOps.Model
{
    /// <summary>
    /// Class that defines the data from Override Input File
    /// </summary>
    public class OverrideVMSpecs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OverrideVMSpecs"/> class
        /// </summary>
        public OverrideVMSpecs()
        {
            this.InputOriginalValues = new OverrideVMSpecsInputValues()
            {
                OperatingSystem = null,
                CPUCores = null,
                Memory = null,
                NumberOfDataDisks = null,
                HasSSDStorage = null,
                AzureVMOverride = null,
                AzureProjVMSize = null,
                AzureProjVMCores = null,
                AzureProjVMMemory = null,
                Comments = null
            };

            this.OperatingSystem = string.Empty;
            this.CPUCores = 0;
            this.MemoryInMB = 0;
            this.NumberOfDataDisks = 0;
            this.HasSSDStorage = false;
            this.AzureVMOverride = string.Empty;
            this.AzureProjVMSize = string.Empty;
            this.AzureProjVMCores = 0;
            this.AzureProjVMMemory = 0;
            this.Comments = string.Empty;
            this.IsValid = false;
            this.ValidationMessage = string.Empty;
        }

        /// <summary>
        /// Gets or sets the unmodified values from the input file 
        /// </summary>
        public OverrideVMSpecsInputValues InputOriginalValues { get; set; }

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
        /// Gets or sets the Number of Data Disks
        /// </summary>
        public int NumberOfDataDisks { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether SSD Storage is present or not
        /// </summary>
        public bool HasSSDStorage { get; set; }

        /// <summary>
        /// Gets or sets the Azure VM SKU Override value
        /// </summary>
        public string AzureVMOverride { get; set; }

        /// <summary>
        /// Gets or sets the Azure Mapped VM SKU value
        /// </summary>
        public string AzureProjVMSize { get; set; }

        /// <summary>
        /// Gets or sets the cores for the Azure Mapped VM SKU value
        /// </summary>
        public int AzureProjVMCores { get; set; }

        /// <summary>
        /// Gets or sets the memory of the Azure Mapped VM SKU value
        /// </summary>
        public int AzureProjVMMemory { get; set; }

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
    /// Class that defines the unmodified data from Override Input File
    /// </summary>
    public class OverrideVMSpecsInputValues
    {
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
        /// Gets or sets the unmodified Number of Data Disks value from the input
        /// </summary>
        public string NumberOfDataDisks { get; set; }

        /// <summary>
        /// Gets or sets a unmodified value indicating whether SSD Storage is present or not
        /// </summary>
        public string HasSSDStorage { get; set; }

        /// <summary>
        /// Gets or sets the unmodified Azure VM Override value from the input
        /// </summary>
        public string AzureVMOverride { get; set; }

        /// <summary>
        /// Gets or sets the unmodified mapped Azure VM SKU value from the input
        /// </summary>
        public string AzureProjVMSize { get; set; }

        /// <summary>
        /// Gets or sets the unmodified CPU Cores value from the input
        /// </summary>
        public string AzureProjVMCores { get; set; }

        /// <summary>
        /// Gets or sets the unmodified Memory for the mapped Azure VM SKU value from the input
        /// </summary>
        public string AzureProjVMMemory { get; set; }

        /// <summary>
        /// Gets or sets the unmodified Comments value from the input
        /// </summary>
        public string Comments { get; set; }
    }
}
