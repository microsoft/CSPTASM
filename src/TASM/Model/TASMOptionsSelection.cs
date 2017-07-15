// -----------------------------------------------------------------------
// <copyright file="TASMOptionsSelection.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace TASM.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TASMOps;

    /// <summary>
    /// Class that is instantiated to store the data from options selected.
    /// </summary>
    public class TASMOptionsSelection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TASMOptionsSelection"/> class.
        /// </summary>
        public TASMOptionsSelection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TASMOptionsSelection"/> class.
        /// </summary>
        /// <param name="loadfromDefaults">A value indicating if to load defaults or not</param>
        public TASMOptionsSelection(bool loadfromDefaults)
        {
            if (loadfromDefaults)
            {
                this.VMSpecsInputSequence = OptionsConstants.DefaultVMSpecsInputSeq.ToArray();
                this.OverrideVMSpecsInputSequence = OptionsConstants.DefaultOverrideVMSpecsInputSeq.ToArray();
                this.VMSpecsSkipLines = OptionsConstants.DefaultVMSpecsSkipLines;
                this.InputOverrideSkipLines = OptionsConstants.DefaultInputOverrideSkipLines;
                this.WindowsKeywordsList = OptionsConstants.DefaultWindowsKeywordsList.ToList();
                this.LinuxKeywordsList = OptionsConstants.DefaultLinuxKeywordsList.ToList();
                this.AzureVMSKUSeriesIncluded = OptionsConstants.DefaultAzureVMSKUSeriesIncluded.ToList();
                this.AzureVMSKUSeriesExcluded = OptionsConstants.DefaultAzureVMSKUSeriesExcluded.ToList();
                this.MappingCoefficientCoresSelection = OptionsConstants.DefaultMappingCoefCores;
                this.MappingCoefficientMemorySelection = OptionsConstants.DefaultMappingCoefMemory;
                this.OSDiskHDDSelection = OptionsConstants.DefaultOSDiskHDDSize;
                this.OSDiskSSDSelection = OptionsConstants.DefaultOSDiskSSDSize;
                this.MemoryGBMBSelection = OptionsConstants.DefaultMemoryGBMB;
                this.HoursinaMonthSelection = OptionsConstants.DefaultHoursinaMonth;
            }
        }

        /// <summary>
        /// Gets or sets the value of Skip lines for Input VM specifications (Options)
        /// </summary>
        public int VMSpecsSkipLines { get; set; }

        /// <summary>
        /// Gets or sets the value of Skip lines for Input VM Override specifications (Options)
        /// </summary>
        public int InputOverrideSkipLines { get; set; }

        /// <summary>
        /// Gets or sets the Input VM specifications sequence (Options)
        /// </summary>
        public VMSpecsSequenceId[] VMSpecsInputSequence { get; set; }

        /// <summary>
        /// Gets or sets the Input VM Override specifications sequence (Options)
        /// </summary>
        public OverrideVMSpecsSequenceId[] OverrideVMSpecsInputSequence { get; set; }

        /// <summary>
        /// Gets or sets the Windows Keywords list option
        /// </summary>
        public List<string> WindowsKeywordsList { get; set; }

        /// <summary>
        /// Gets or sets the Linux Keywords list option
        /// </summary>
        public List<string> LinuxKeywordsList { get; set; }

        /// <summary>
        /// Gets or sets the Azure VM SKU List included list option
        /// </summary>
        public List<AzureVMSeriesTypes> AzureVMSKUSeriesIncluded { get; set; }

        /// <summary>
        /// Gets or sets the Azure VM SKU List excluded list option
        /// </summary>
        public List<AzureVMSeriesTypes> AzureVMSKUSeriesExcluded { get; set; }

        /// <summary>
        /// Gets or sets the mapping coefficients value for cores option
        /// </summary>
        public double MappingCoefficientCoresSelection { get; set; }

        /// <summary>
        /// Gets or sets the mapping coefficients value for memory option
        /// </summary>
        public double MappingCoefficientMemorySelection { get; set; }

        /// <summary>
        /// Gets or sets the OS Disk (HDD) value option
        /// </summary>
        public string OSDiskHDDSelection { get; set; }

        /// <summary>
        /// Gets or sets the OS Disk (SSD) value option
        /// </summary>
        public string OSDiskSSDSelection { get; set; }

        /// <summary>
        /// Gets or sets the Memory Units value option
        /// </summary>
        public string MemoryGBMBSelection { get; set; }

        /// <summary>
        /// Gets or sets the Hours in a month value option
        /// </summary>
        public int HoursinaMonthSelection { get; set; }

        /// <summary>
        /// Get the memory unit selection, true if GB and false it not
        /// </summary>
        /// <returns> Returns the memory unit selection, true if GB and false it not</returns>
        public bool IsMemoryGB()
        {
            if (this.MemoryGBMBSelection.Equals(TASM.Constants.GBStr, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
