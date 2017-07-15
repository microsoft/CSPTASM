// -----------------------------------------------------------------------
// <copyright file="IOLoadUtil.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace TASMOps.Util
{
    using System;
    using System.Linq;
    using TASMOps.Model;

    /// <summary>
    /// Class that has static methods to perform loading data into objects from string arrays (from read CSVs) and unloading of data from objects into string arrays (to write CSVs)
    /// </summary>
    public class IOLoadUtil
    {
        /// <summary>
        /// The number of columns in mapping output file
        /// </summary>
        private const int NumberOfTotalColumnsVMResult = 25;

        /// <summary>
        /// The number of columns in input override file
        /// </summary>
        private const int NumberOfTotalColumnsOverrideVMSpecs = 10;

        /// <summary>
        /// The number of columns in output override file
        /// </summary>
        private const int NumberOfTotalColumnsUniqueProjList = 10;

        /// <summary>
        /// The number of columns in Virtual machine specs sample file
        /// </summary>
        private const int NumberOfTotalColumnsVMSpecsSample = 11;

        /// <summary>
        /// Generates the strings array for the header of VM Specs Sample file
        /// </summary>
        /// <param name="isOutputMemoryInGB">A value indicating if Memory is in GB or not</param>
        /// <param name="currency">CSP Currency</param>
        /// <returns> Returns the strings array for the header of VM Specs Sample file</returns>
        public static String[] UnloadVMSpecsSampleHeader(bool isOutputMemoryInGB, string currency)
        {
            string[] outputVMSpecsStr = new string[NumberOfTotalColumnsVMSpecsSample];

            try
            {
                int column = 0;
                outputVMSpecsStr[column++] = VMSpecLiterals.InstanceName;
                outputVMSpecsStr[column++] = VMSpecLiterals.OperatingSystem;
                outputVMSpecsStr[column++] = VMSpecLiterals.CPUCores;
                outputVMSpecsStr[column++] = VMSpecLiterals.Memory + (isOutputMemoryInGB ? Constants.MemoryinGBUnitsSuffix : Constants.MemoryinMBUnitsSuffix);
                outputVMSpecsStr[column++] = VMSpecLiterals.SSDStorageInGB;
                outputVMSpecsStr[column++] = VMSpecLiterals.SSDNumOfDisks;
                outputVMSpecsStr[column++] = VMSpecLiterals.HDDStorageInGB;
                outputVMSpecsStr[column++] = VMSpecLiterals.HDDNumOfDisks;
                outputVMSpecsStr[column++] = VMSpecLiterals.PricePerMonth + string.Format(Constants.CurrencyFormat, currency);
                outputVMSpecsStr[column++] = VMSpecLiterals.AzureVMOverride;
                outputVMSpecsStr[column++] = VMSpecLiterals.Comments;
            }
            catch (Exception)
            {
                throw;
            }

            return outputVMSpecsStr;
        }

        /// <summary>
        /// Generates the strings array for the output of VM Specs Sample file
        /// </summary>
        /// <param name="specs">Object containing the sample data for VM Specs</param>
        /// <param name="isOutputMemoryInGB">A value indicating if Memory is in GB or not</param>
        /// <returns> Returns the strings array for the output of VM Specs Sample file</returns>
        public static String[] UnloadVMSpecsSampleOutput(VMSpecs specs, bool isOutputMemoryInGB)
        {
            string[] outputVMSpecsStr = new string[NumberOfTotalColumnsVMResult];
            try
            {
                // Write Input VM Specs including validation results columns
                int column = 0;
                
                    outputVMSpecsStr[column++] = specs.InstanceName;
                    outputVMSpecsStr[column++] = specs.OperatingSystem;
                    outputVMSpecsStr[column++] = specs.CPUCores.ToString();

                    outputVMSpecsStr[column++] = isOutputMemoryInGB ? Math.Round(specs.MemoryInMB / 1024, 2).ToString()
                            : Math.Round(specs.MemoryInMB, 2).ToString();

                    outputVMSpecsStr[column++] = Math.Round(specs.SSDStorageInGB, 2).ToString();
                    outputVMSpecsStr[column++] = specs.SSDNumOfDisks.ToString();
                    outputVMSpecsStr[column++] = Math.Round(specs.HDDStorageInGB, 2).ToString();
                    outputVMSpecsStr[column++] = specs.HDDNumOfDisks.ToString();
                    outputVMSpecsStr[column++] = Math.Round(specs.PricePerMonth, 2).ToString();
                    outputVMSpecsStr[column++] = specs.AzureVMOverride;
                    outputVMSpecsStr[column++] = specs.Comments;
            }
            catch (Exception)
            {
                throw;
            }

            return outputVMSpecsStr;
        }

        /// <summary>
        /// Generates the strings array for the header of mapping output file
        /// </summary>
        /// <param name="isOutputMemoryInGB">A value indicating if Memory is in GB or not</param>
        /// <param name="currency">CSP Currency</param>
        /// <returns> Returns the strings array for the header of mapping output file</returns>
        public static String[] UnloadVMSpecsHeader(bool isOutputMemoryInGB, string currency)
        {
            string[] outputVMSpecsStr = new string[NumberOfTotalColumnsVMResult];
            try
            {
                int column = 0;

                // Write Input VM Specs including validation results columns
                outputVMSpecsStr[column++] = VMSpecLiterals.InstanceName;
                outputVMSpecsStr[column++] = VMSpecLiterals.OperatingSystem;
                outputVMSpecsStr[column++] = VMSpecLiterals.CPUCores;
                outputVMSpecsStr[column++] = VMSpecLiterals.Memory + (isOutputMemoryInGB ? Constants.MemoryinGBUnitsSuffix : Constants.MemoryinMBUnitsSuffix);
                outputVMSpecsStr[column++] = VMSpecLiterals.SSDStorageInGB;
                outputVMSpecsStr[column++] = VMSpecLiterals.SSDNumOfDisks;
                outputVMSpecsStr[column++] = VMSpecLiterals.HDDStorageInGB;
                outputVMSpecsStr[column++] = VMSpecLiterals.HDDNumOfDisks;
                outputVMSpecsStr[column++] = VMSpecLiterals.PricePerMonth + string.Format(Constants.CurrencyFormat, currency);
                outputVMSpecsStr[column++] = VMSpecLiterals.AzureVMOverride;
                outputVMSpecsStr[column++] = VMSpecLiterals.Comments;
                outputVMSpecsStr[column++] = VMSpecLiterals.IsValid;
                outputVMSpecsStr[column++] = VMSpecLiterals.ValidationMessage;

                // Write Azure VM Projected Key columns
                outputVMSpecsStr[column++] = AzureProjVMLiterals.VMSize;
                outputVMSpecsStr[column++] = AzureProjVMLiterals.Cores;
                outputVMSpecsStr[column++] = AzureProjVMLiterals.Memory + (isOutputMemoryInGB ? Constants.MemoryinGBUnitsSuffix : Constants.MemoryinMBUnitsSuffix);
                outputVMSpecsStr[column++] = AzureProjVMLiterals.ComputeHoursMonthlyCost + string.Format(Constants.CurrencyFormat, currency);
                outputVMSpecsStr[column++] = AzureProjVMLiterals.PremiumDisksMonthlyCost + string.Format(Constants.CurrencyFormat, currency);
                outputVMSpecsStr[column++] = AzureProjVMLiterals.StandardDisksMonthlyCost + string.Format(Constants.CurrencyFormat, currency);
                outputVMSpecsStr[column++] = AzureProjVMLiterals.AzureVMTotalMonthlyCost + string.Format(Constants.CurrencyFormat, currency);
                outputVMSpecsStr[column++] = AzureProjVMLiterals.MonthlyGrossMarginEstimates + string.Format(Constants.CurrencyFormat, currency);

                // Write Azure VM Projected Other columns
                outputVMSpecsStr[column++] = AzureProjVMLiterals.PremiumDisksStr;
                outputVMSpecsStr[column++] = AzureProjVMLiterals.StandardDisksStr;
                outputVMSpecsStr[column++] = AzureProjVMLiterals.ComputeHoursRate + string.Format(Constants.CurrencyFormat, currency);
                outputVMSpecsStr[column++] = AzureProjVMLiterals.AzureProjectionComments;
            }
            catch (Exception)
            {
                throw;
            }

            return outputVMSpecsStr;
        }

        /// <summary>
        /// Generates the strings array for the output of mapping output file
        /// </summary>
        /// <param name="vmRes">Mapping output object</param>
        /// <param name="isOutputMemoryInGB">A value indicating if Memory is in GB or not</param>
        /// <returns> Returns the strings array for the output of mapping output file</returns>
        public static String[] UnloadVMSpecsOutput(VMResult vmRes, bool isOutputMemoryInGB)
        {
            string[] outputVMSpecsStr = new string[NumberOfTotalColumnsVMResult];
            try
            {
                // Write Input VM Specs including validation results columns
                int column = 0;
                if (vmRes.Specs.IsValid)
                {
                    outputVMSpecsStr[column++] = vmRes.Specs.InstanceName;
                    outputVMSpecsStr[column++] = vmRes.Specs.OperatingSystem;
                    outputVMSpecsStr[column++] = vmRes.Specs.CPUCores.ToString();

                    outputVMSpecsStr[column++] = isOutputMemoryInGB ? Math.Round(vmRes.Specs.MemoryInMB / 1024, 2).ToString()
                            : Math.Round(vmRes.Specs.MemoryInMB, 2).ToString();

                    outputVMSpecsStr[column++] = Math.Round(vmRes.Specs.SSDStorageInGB, 2).ToString();
                    outputVMSpecsStr[column++] = vmRes.Specs.SSDNumOfDisks.ToString();
                    outputVMSpecsStr[column++] = Math.Round(vmRes.Specs.HDDStorageInGB, 2).ToString();
                    outputVMSpecsStr[column++] = vmRes.Specs.HDDNumOfDisks.ToString();
                    outputVMSpecsStr[column++] = Math.Round(vmRes.Specs.PricePerMonth, 2).ToString();
                    outputVMSpecsStr[column++] = vmRes.Specs.AzureVMOverride;
                    outputVMSpecsStr[column++] = vmRes.Specs.Comments;

                    outputVMSpecsStr[column++] = vmRes.Specs.IsValid ? Constants.BoolDisplayValueForTRUE : Constants.BoolDisplayValueForFALSE;
                    outputVMSpecsStr[column++] = vmRes.Specs.ValidationMessage;
                }
                else
                {
                    outputVMSpecsStr[column++] = vmRes.Specs.InputOriginalValues.InstanceName;
                    outputVMSpecsStr[column++] = vmRes.Specs.InputOriginalValues.OperatingSystem;
                    outputVMSpecsStr[column++] = vmRes.Specs.InputOriginalValues.CPUCores;
                    outputVMSpecsStr[column++] = vmRes.Specs.InputOriginalValues.Memory;
                    outputVMSpecsStr[column++] = vmRes.Specs.InputOriginalValues.SSDStorageInGB;
                    outputVMSpecsStr[column++] = vmRes.Specs.InputOriginalValues.SSDNumOfDisks;
                    outputVMSpecsStr[column++] = vmRes.Specs.InputOriginalValues.HDDStorageInGB;
                    outputVMSpecsStr[column++] = vmRes.Specs.InputOriginalValues.HDDNumOfDisks;
                    outputVMSpecsStr[column++] = vmRes.Specs.InputOriginalValues.PricePerMonth;
                    outputVMSpecsStr[column++] = vmRes.Specs.InputOriginalValues.AzureVMOverride;
                    outputVMSpecsStr[column++] = vmRes.Specs.InputOriginalValues.Comments;

                    outputVMSpecsStr[column++] = vmRes.Specs.IsValid ? Constants.BoolDisplayValueForTRUE : Constants.BoolDisplayValueForFALSE;
                    outputVMSpecsStr[column++] = vmRes.Specs.ValidationMessage;
                }

                // Write Azure VM Projected Key columns
                if (vmRes.Specs.IsValid)
                {
                    outputVMSpecsStr[column++] = vmRes.ProjAzureVM != null && !vmRes.ProjAzureVM.IsNoMapFound && vmRes.ProjAzureVM.VMSize != null  ?
                                        vmRes.ProjAzureVM.VMSize.Name : string.Empty;
                    outputVMSpecsStr[column++] = vmRes.ProjAzureVM != null && !vmRes.ProjAzureVM.IsNoMapFound && vmRes.ProjAzureVM.VMSize != null ?
                        vmRes.ProjAzureVM.VMSize.NumberOfCores.ToString() : string.Empty;
                    outputVMSpecsStr[column++] = vmRes.ProjAzureVM != null && !vmRes.ProjAzureVM.IsNoMapFound && vmRes.ProjAzureVM.VMSize != null ?
                        (isOutputMemoryInGB ? Math.Round((double)vmRes.ProjAzureVM.VMSize.MemoryInMB / 1024, 2).ToString()
                            : vmRes.ProjAzureVM.VMSize.MemoryInMB.ToString()) : string.Empty;
                    outputVMSpecsStr[column++] = vmRes.ProjAzureVM != null && !vmRes.ProjAzureVM.IsNoMapFound ?
                        Math.Round(vmRes.ProjAzureVM.ComputeHoursMonthlyCost, 2).ToString() : string.Empty;
                    outputVMSpecsStr[column++] = vmRes.ProjAzureVM != null ?
                        Math.Round(vmRes.ProjAzureVM.PremiumDisksMonthlyCost, 2).ToString() : string.Empty;
                    outputVMSpecsStr[column++] = vmRes.ProjAzureVM != null ?
                        Math.Round(vmRes.ProjAzureVM.StandardDisksMonthlyCost, 2).ToString() : string.Empty;
                    outputVMSpecsStr[column++] = vmRes.ProjAzureVM != null && !vmRes.ProjAzureVM.IsNoMapFound ?
                        Math.Round(vmRes.ProjAzureVM.AzureVMTotalMonthlyCost, 2).ToString() : string.Empty;
                    outputVMSpecsStr[column++] = vmRes.ProjAzureVM != null && !vmRes.ProjAzureVM.IsNoMapFound ?
                        Math.Round(vmRes.ProjAzureVM.MonthlyGrossMarginEstimates, 2).ToString() : string.Empty;

                    // Write Azure VM Projected Other columns
                    outputVMSpecsStr[column++] = vmRes.ProjAzureVM != null ?
                        vmRes.ProjAzureVM.PremiumDisksStr : string.Empty;
                    outputVMSpecsStr[column++] = vmRes.ProjAzureVM != null ?
                        vmRes.ProjAzureVM.StandardDisksStr : string.Empty;
                    outputVMSpecsStr[column++] = vmRes.ProjAzureVM != null && !vmRes.ProjAzureVM.IsNoMapFound ?
                        Math.Round(vmRes.ProjAzureVM.ComputeHoursRate, 2).ToString() : string.Empty;
                    outputVMSpecsStr[column++] = vmRes.ProjAzureVM != null ?
                        vmRes.ProjAzureVM.AzureProjectionComments : string.Empty;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return outputVMSpecsStr;
        }

        /// <summary>
        /// Generates the strings array for the header of validated Override input file and Output Override file
        /// </summary>
        /// <param name="isOutputMemoryInGB">A value indicating if Memory is in GB or not</param>
        /// <param name="isUniqueProjectedList">Value indicating if the output to be written is the output override data or not</param>
        /// <returns> Returns the strings array for the header of validated Override and Output Override file</returns>
        public static String[] UnloadOverrideVMSpecsHeader(bool isOutputMemoryInGB, bool isUniqueProjectedList)
        {
            int numberOfColumns = isUniqueProjectedList ? NumberOfTotalColumnsUniqueProjList : NumberOfTotalColumnsOverrideVMSpecs;
            string[] outputOverrideVMSpecsStr = new string[numberOfColumns];
            try
            {
                int column = 0;

                // Write Override VM Specs column with validation results - Header
                outputOverrideVMSpecsStr[column++] = OverrideVMSpecLiterals.OperatingSystem;
                outputOverrideVMSpecsStr[column++] = OverrideVMSpecLiterals.CPUCores;
                outputOverrideVMSpecsStr[column++] = OverrideVMSpecLiterals.Memory + (isOutputMemoryInGB ? Constants.MemoryinGBUnitsSuffix : Constants.MemoryinMBUnitsSuffix);
                outputOverrideVMSpecsStr[column++] = OverrideVMSpecLiterals.NumberOfDataDisks;
                outputOverrideVMSpecsStr[column++] = OverrideVMSpecLiterals.HasSSDStorage;
                outputOverrideVMSpecsStr[column++] = OverrideVMSpecLiterals.AzureVMOverride;
                outputOverrideVMSpecsStr[column++] = OverrideVMSpecLiterals.AzureProjVMSize;

                if (isUniqueProjectedList)
                {
                    outputOverrideVMSpecsStr[column++] = OverrideVMSpecLiterals.AzureProjVMCores;
                    outputOverrideVMSpecsStr[column++] = OverrideVMSpecLiterals.AzureProjVMMemory + (isOutputMemoryInGB ? Constants.MemoryinGBUnitsSuffix : Constants.MemoryinMBUnitsSuffix);
                }

                outputOverrideVMSpecsStr[column++] = OverrideVMSpecLiterals.Comments;

                if (!isUniqueProjectedList)
                {
                    outputOverrideVMSpecsStr[column++] = OverrideVMSpecLiterals.IsValid;
                    outputOverrideVMSpecsStr[column++] = OverrideVMSpecLiterals.ValidationMessage;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return outputOverrideVMSpecsStr;
        }

        /// <summary>
        /// Generates the strings array for the output of validated Override input file and Output Override file
        /// </summary>
        /// <param name="validatedOverrideVMSpecs">Validated Override Specs object / Output Override Specs object</param>
        /// <param name="isOutputMemoryInGB">A value indicating if Memory is in GB or not</param>
        /// <param name="isUniqueProjectedList">Value indicating if the output to be written is the output override data or not</param>
        /// <returns> Returns the strings array for the output of validated Override and Output Override file</returns>
        public static String[] UnloadOverrideVMSpecsOutput(OverrideVMSpecs validatedOverrideVMSpecs, bool isOutputMemoryInGB, bool isUniqueProjectedList)
        {
            string[] outputOverrideVMSpecsStr = new string[NumberOfTotalColumnsOverrideVMSpecs];
            try
            {
                int column = 0;

                // Write Override VM Specs column with validation results - Output
                outputOverrideVMSpecsStr[column++] = validatedOverrideVMSpecs.IsValid || isUniqueProjectedList ? 
                    validatedOverrideVMSpecs.OperatingSystem : validatedOverrideVMSpecs.InputOriginalValues.OperatingSystem;

                outputOverrideVMSpecsStr[column++] = validatedOverrideVMSpecs.IsValid || isUniqueProjectedList ?
                    validatedOverrideVMSpecs.CPUCores.ToString() : validatedOverrideVMSpecs.InputOriginalValues.CPUCores;

                outputOverrideVMSpecsStr[column++] = validatedOverrideVMSpecs.IsValid || isUniqueProjectedList ?
                    (isOutputMemoryInGB ? Math.Round(validatedOverrideVMSpecs.MemoryInMB / 1024, 2).ToString() : Math.Round(validatedOverrideVMSpecs.MemoryInMB, 2).ToString()) : validatedOverrideVMSpecs.InputOriginalValues.Memory;

                outputOverrideVMSpecsStr[column++] = validatedOverrideVMSpecs.IsValid || isUniqueProjectedList ?
                    validatedOverrideVMSpecs.NumberOfDataDisks.ToString() : validatedOverrideVMSpecs.InputOriginalValues.NumberOfDataDisks;

                outputOverrideVMSpecsStr[column++] = validatedOverrideVMSpecs.IsValid || isUniqueProjectedList ?
                    (validatedOverrideVMSpecs.HasSSDStorage ? Constants.BoolDisplayValueForTRUE : Constants.BoolDisplayValueForFALSE) : validatedOverrideVMSpecs.InputOriginalValues.HasSSDStorage;

                outputOverrideVMSpecsStr[column++] = isUniqueProjectedList ? string.Empty : (validatedOverrideVMSpecs.IsValid ?
                    validatedOverrideVMSpecs.AzureVMOverride : validatedOverrideVMSpecs.InputOriginalValues.AzureVMOverride);

                outputOverrideVMSpecsStr[column++] = validatedOverrideVMSpecs.IsValid || isUniqueProjectedList ?
                    validatedOverrideVMSpecs.AzureProjVMSize : validatedOverrideVMSpecs.InputOriginalValues.AzureProjVMSize;

                if (isUniqueProjectedList)
                {
                    outputOverrideVMSpecsStr[column++] = string.IsNullOrWhiteSpace(validatedOverrideVMSpecs.AzureProjVMSize) ? string.Empty : validatedOverrideVMSpecs.AzureProjVMCores.ToString();
                    outputOverrideVMSpecsStr[column++] = string.IsNullOrWhiteSpace(validatedOverrideVMSpecs.AzureProjVMSize) ? string.Empty : 
                        (isOutputMemoryInGB ? Math.Round((double)validatedOverrideVMSpecs.AzureProjVMMemory / 1024, 2).ToString() : validatedOverrideVMSpecs.AzureProjVMMemory.ToString());
                }

                outputOverrideVMSpecsStr[column++] = isUniqueProjectedList ? string.Empty : (validatedOverrideVMSpecs.IsValid ?
                    validatedOverrideVMSpecs.Comments : validatedOverrideVMSpecs.InputOriginalValues.Comments);

                if (!isUniqueProjectedList)
                {
                    outputOverrideVMSpecsStr[column++] = validatedOverrideVMSpecs.IsValid ? Constants.BoolDisplayValueForTRUE : Constants.BoolDisplayValueForFALSE;

                    outputOverrideVMSpecsStr[column++] = validatedOverrideVMSpecs.ValidationMessage;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return outputOverrideVMSpecsStr;
        }

        /// <summary>
        /// Converts the specified Input Specifications data that is in form of strings array to Input specifications object
        /// </summary>
        /// <param name="inputVMSpecsStr">String array containing the Input specifications</param>
        /// <param name="sequenceOfInputVMSpecs">Sequence of Input specifications in the input string array provided</param>
        /// <returns> Returns the Input specifications object with data loaded from the strings array</returns>
        public static VMSpecs LoadVMSpecs(string[] inputVMSpecsStr, VMSpecsSequenceId[] sequenceOfInputVMSpecs)
        {
            VMSpecs inputVMSpecs = null;

            try
            {
                VMSpecs vmSpecsObj = new VMSpecs();

                for (int i = 0; i < sequenceOfInputVMSpecs.Count(); i++)
                {
                    switch (sequenceOfInputVMSpecs[i])
                    {
                        case VMSpecsSequenceId.InstanceName:
                            vmSpecsObj.InputOriginalValues.InstanceName = inputVMSpecsStr[i];
                            break;

                        case VMSpecsSequenceId.OperatingSystem:
                            vmSpecsObj.InputOriginalValues.OperatingSystem = inputVMSpecsStr[i];
                            break;

                        case VMSpecsSequenceId.CPUCores:
                            vmSpecsObj.InputOriginalValues.CPUCores = inputVMSpecsStr[i];
                            break;

                        case VMSpecsSequenceId.MemoryInMB:
                            vmSpecsObj.InputOriginalValues.Memory = inputVMSpecsStr[i];
                            break;

                        case VMSpecsSequenceId.SSDStorageInGB:
                            vmSpecsObj.InputOriginalValues.SSDStorageInGB = inputVMSpecsStr[i];
                            break;

                        case VMSpecsSequenceId.SSDNumOfDisks:
                            vmSpecsObj.InputOriginalValues.SSDNumOfDisks = inputVMSpecsStr[i];
                            break;

                        case VMSpecsSequenceId.HDDStorageInGB:
                            vmSpecsObj.InputOriginalValues.HDDStorageInGB = inputVMSpecsStr[i];
                            break;

                        case VMSpecsSequenceId.HDDNumOfDisks:
                            vmSpecsObj.InputOriginalValues.HDDNumOfDisks = inputVMSpecsStr[i];
                            break;

                        case VMSpecsSequenceId.PricePerMonth:
                            vmSpecsObj.InputOriginalValues.PricePerMonth = inputVMSpecsStr[i];
                            break;

                        case VMSpecsSequenceId.AzureVMOverride:
                            vmSpecsObj.InputOriginalValues.AzureVMOverride = inputVMSpecsStr[i];
                            break;

                        case VMSpecsSequenceId.Comments:
                            vmSpecsObj.InputOriginalValues.Comments = inputVMSpecsStr[i];
                            break;

                        default:
                            break;
                    }
                }

                inputVMSpecs = vmSpecsObj;
            }
            catch (Exception)
            {
                throw;
            }

            return inputVMSpecs;
        }

        /// <summary>
        /// Converts the specified Input Override Specifications data that is in form of strings array to Input Override specifications object
        /// </summary>
        /// <param name="inputOverrideVMSpecsStr">String array containing the Input Override specifications</param>
        /// <param name="sequenceOfInputOverrideVMSpecs">Sequence of Input Override specifications in the input string array provided</param>
        /// <returns> Returns the Input Override specifications object with data loaded from the strings array</returns>
        public static OverrideVMSpecs LoadOverrideVMSpecs(string[] inputOverrideVMSpecsStr, OverrideVMSpecsSequenceId[] sequenceOfInputOverrideVMSpecs = null)
        {
            OverrideVMSpecs inputOverrideVMSpecs = null;

            try
            {
                OverrideVMSpecs overrideVMSpecsObj = new OverrideVMSpecs();

                for (int i = 0; i < sequenceOfInputOverrideVMSpecs.Count(); i++)
                {
                    switch (sequenceOfInputOverrideVMSpecs[i])
                    {
                        case OverrideVMSpecsSequenceId.OperatingSystem:
                            overrideVMSpecsObj.InputOriginalValues.OperatingSystem = inputOverrideVMSpecsStr[i];
                            break;

                        case OverrideVMSpecsSequenceId.CPUCores:
                            overrideVMSpecsObj.InputOriginalValues.CPUCores = inputOverrideVMSpecsStr[i];
                            break;

                        case OverrideVMSpecsSequenceId.MemoryInMB:
                            overrideVMSpecsObj.InputOriginalValues.Memory = inputOverrideVMSpecsStr[i];
                            break;

                        case OverrideVMSpecsSequenceId.NumberOfDataDisks:
                            overrideVMSpecsObj.InputOriginalValues.NumberOfDataDisks = inputOverrideVMSpecsStr[i];
                            break;

                        case OverrideVMSpecsSequenceId.HasSSDStorage:
                            overrideVMSpecsObj.InputOriginalValues.HasSSDStorage = inputOverrideVMSpecsStr[i];
                            break;

                        case OverrideVMSpecsSequenceId.AzureVMOverride:
                            overrideVMSpecsObj.InputOriginalValues.AzureVMOverride = inputOverrideVMSpecsStr[i];
                            break;

                        case OverrideVMSpecsSequenceId.AzureProjVMSize:
                            overrideVMSpecsObj.InputOriginalValues.AzureProjVMSize = inputOverrideVMSpecsStr[i];
                            break;

                        case OverrideVMSpecsSequenceId.AzureProjVMCores:
                            overrideVMSpecsObj.InputOriginalValues.AzureProjVMCores = inputOverrideVMSpecsStr[i];
                            break;

                        case OverrideVMSpecsSequenceId.AzureProjVMMemory:
                            overrideVMSpecsObj.InputOriginalValues.AzureProjVMMemory = inputOverrideVMSpecsStr[i];
                            break;

                        case OverrideVMSpecsSequenceId.Comments:
                            overrideVMSpecsObj.InputOriginalValues.Comments = inputOverrideVMSpecsStr[i];
                            break;

                        default:
                            break;
                    }
                }

                inputOverrideVMSpecs = overrideVMSpecsObj;
            }
            catch (Exception)
            {
                throw;
            }

            return inputOverrideVMSpecs;
        }
    }
}
