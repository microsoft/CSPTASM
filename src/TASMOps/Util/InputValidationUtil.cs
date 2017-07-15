// -----------------------------------------------------------------------
// <copyright file="InputValidationUtil.cs" company="Microsoft">
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
    /// Class that has static methods to validate the data from the input files for specifications and overrides
    /// </summary>
    public class InputValidationUtil
    {
        /// <summary>
        /// Get the validated virtual machine specifications for the data provided
        /// </summary>
        /// <param name="inputVMSpecs">The Input specifications</param>
        /// <param name="isInputMemoryInGB">A value indicating if the Input memory values are in GB</param>
        /// <param name="os_WindowsList">The list of Windows OS keywords</param>
        /// <param name="os_LinuxList">The list of Linux OS keywords</param>
        /// <returns> Returns the validated virtual machine specifications</returns>
        public static VMSpecs ValidateVMSpecs(VMSpecs inputVMSpecs, bool isInputMemoryInGB, List<string> os_WindowsList, List<string> os_LinuxList)
        {
            VMSpecs validatedVMSpecs = null;

            try
            {
                inputVMSpecs.IsValid = true;

                ValidateInstanceName(inputVMSpecs);
                ValidateOperatingSystem(inputVMSpecs, os_WindowsList, os_LinuxList);
                ValidateCPUCores(inputVMSpecs);
                ValidateMemory(inputVMSpecs, isInputMemoryInGB);
                ValidateSSDStorageInGB(inputVMSpecs);
                ValidateSSDNumOfDisks(inputVMSpecs);
                ValidateHDDStorageInGB(inputVMSpecs);
                ValidateHDDNumOfDisks(inputVMSpecs);
                ValidatePricePerMonth(inputVMSpecs);
                ValidateAzureVMOverride(inputVMSpecs);
                ValidateComments(inputVMSpecs);

                if (!inputVMSpecs.IsValid)
                {
                    inputVMSpecs.ValidationMessage = ValidationLiterals.SpecValidationFailed + inputVMSpecs.ValidationMessage;
                }

                // Check if last two chars is "; " and remove them
                if (inputVMSpecs.ValidationMessage.Length > 2 && inputVMSpecs.ValidationMessage.Substring(inputVMSpecs.ValidationMessage.Length - 2).Equals("; "))
                {
                    inputVMSpecs.ValidationMessage = inputVMSpecs.ValidationMessage.Remove(inputVMSpecs.ValidationMessage.Length - 2);
                }

                validatedVMSpecs = inputVMSpecs;
            }
            catch (Exception)
            {
                throw;
            }

            return validatedVMSpecs;
        }

        /// <summary>
        /// Get the validated Input Override data
        /// </summary>
        /// <param name="inputOverrideVMSpecs">The Input Override data to be validated</param>
        /// <param name="isInputMemoryInGB">A value indicating if the Input memory values are in GB</param>
        /// <param name="os_WindowsList">The list of Windows OS keywords</param>
        /// <param name="os_LinuxList">The list of Linux OS keywords</param>
        /// <returns> Returns the validated Input Override data</returns>
        public static OverrideVMSpecs ValidateOverrideVMSpecs(OverrideVMSpecs inputOverrideVMSpecs, bool isInputMemoryInGB, List<string> os_WindowsList, List<string> os_LinuxList)
        {
            OverrideVMSpecs validatedOverrideVMSpecs = null;

            try
            {
                inputOverrideVMSpecs.IsValid = true;
                ValidateOperatingSystem(inputOverrideVMSpecs, os_WindowsList, os_LinuxList);
                ValidateCPUCores(inputOverrideVMSpecs);
                ValidateMemory(inputOverrideVMSpecs, isInputMemoryInGB);
                ValidateNumberOfDataDisks(inputOverrideVMSpecs);
                ValidateHasSSDStorage(inputOverrideVMSpecs);
                ValidateAzureProjVMSize(inputOverrideVMSpecs);
                ValidateAzureVMOverride(inputOverrideVMSpecs);
                ValidateComments(inputOverrideVMSpecs);

                if (!inputOverrideVMSpecs.IsValid)
                {
                    inputOverrideVMSpecs.ValidationMessage = ValidationLiterals.SpecValidationFailed + inputOverrideVMSpecs.ValidationMessage;
                }

                // Check if last two chars is ", " and remove them
                if (inputOverrideVMSpecs.ValidationMessage.Length > 2 && inputOverrideVMSpecs.ValidationMessage.Substring(inputOverrideVMSpecs.ValidationMessage.Length - 2).Equals("; "))
                {
                    inputOverrideVMSpecs.ValidationMessage = inputOverrideVMSpecs.ValidationMessage.Remove(inputOverrideVMSpecs.ValidationMessage.Length - 2);
                }

                validatedOverrideVMSpecs = inputOverrideVMSpecs;
            }
            catch (Exception)
            {
                throw;
            }

            return validatedOverrideVMSpecs;
        }

        /// <summary>
        /// Validate Azure VM SKU Override value in Input specifications
        /// </summary>
        /// <param name="inputVMSpecs">The Input specifications</param>
        private static void ValidateAzureVMOverride(VMSpecs inputVMSpecs)
        {
            if (!string.IsNullOrWhiteSpace(inputVMSpecs.InputOriginalValues.AzureVMOverride))
            {
                inputVMSpecs.AzureVMOverride = inputVMSpecs.InputOriginalValues.AzureVMOverride.Length > Constants.MaxLengthAzureVMOverride ?
                     inputVMSpecs.InputOriginalValues.AzureVMOverride.Substring(0, Constants.MaxLengthAzureVMOverride) : inputVMSpecs.InputOriginalValues.AzureVMOverride;
            }
            else
            {
                inputVMSpecs.AzureVMOverride = string.Empty;
            }
        }

        /// <summary>
        /// Validate Comments value in Input specifications
        /// </summary>
        /// <param name="inputVMSpecs">The Input specifications</param>
        private static void ValidateComments(VMSpecs inputVMSpecs)
        {
            if (!string.IsNullOrWhiteSpace(inputVMSpecs.InputOriginalValues.Comments))
            {
                if (inputVMSpecs.InputOriginalValues.Comments.Length <= Constants.MaxLengthComments)
                {
                    inputVMSpecs.Comments = inputVMSpecs.InputOriginalValues.Comments;
                }
                else
                {
                    inputVMSpecs.Comments = inputVMSpecs.InputOriginalValues.Comments.Substring(0, Constants.MaxLengthComments);
                    inputVMSpecs.ValidationMessage += string.Format(ValidationLiterals.SpecExceedsStringLimit, VMSpecLiterals.Comments, Constants.MaxLengthComments);
                }
            }
            else
            {
                inputVMSpecs.Comments = string.Empty;
            }
        }

        /// <summary>
        /// Validate Instance name value in Input specifications
        /// </summary>
        /// <param name="inputVMSpecs">The Input specifications</param>
        private static void ValidateInstanceName(VMSpecs inputVMSpecs)
        {
            // Instance Name Validation
            if (!string.IsNullOrWhiteSpace(inputVMSpecs.InputOriginalValues.InstanceName))
            {
                inputVMSpecs.InstanceName = inputVMSpecs.InputOriginalValues.InstanceName;
            }
            else
            {
                // Instance Name is blank
                inputVMSpecs.IsValid = false;
                inputVMSpecs.ValidationMessage += string.Format(ValidationLiterals.MissingSpec, VMSpecLiterals.InstanceName);
            }
        }

        /// <summary>
        /// Validate Operating System value in Input specifications
        /// </summary>
        /// <param name="inputVMSpecs">The Input specifications</param>
        /// <param name="os_WindowsList">The list of Windows OS keywords</param>
        /// <param name="os_LinuxList">The list of Linux OS keywords</param>
        private static void ValidateOperatingSystem(VMSpecs inputVMSpecs, List<string> os_WindowsList, List<string> os_LinuxList)
        {
            // Operating System Validation
            if (!string.IsNullOrWhiteSpace(inputVMSpecs.InputOriginalValues.OperatingSystem))
            {
                // Check if String is any of the "Windows" list - Case insensitive
                if (os_WindowsList.Any(x => inputVMSpecs.InputOriginalValues.OperatingSystem.Equals(x, StringComparison.OrdinalIgnoreCase)))
                {
                    inputVMSpecs.OperatingSystem = Constants.OSWindows;
                }
                else if (os_LinuxList.Any(x => inputVMSpecs.InputOriginalValues.OperatingSystem.Equals(x, StringComparison.OrdinalIgnoreCase)))
                {
                    // String is any of the "Linux" list - Case insensitive
                    inputVMSpecs.OperatingSystem = Constants.OSLinux;
                }
                else if (os_WindowsList.Any(x => inputVMSpecs.InputOriginalValues.OperatingSystem.ToUpper().Contains(x.ToUpper())))
                {
                    // String CONTAINS any of the "Windows" list - Case insensitive
                    inputVMSpecs.OperatingSystem = Constants.OSWindows;
                    inputVMSpecs.ValidationMessage += string.Format(ValidationLiterals.SpecValueAssumed, VMSpecLiterals.OperatingSystem, inputVMSpecs.InputOriginalValues.OperatingSystem, Constants.OSWindows);
                }
                else if (os_LinuxList.Any(x => inputVMSpecs.InputOriginalValues.OperatingSystem.ToUpper().Contains(x.ToUpper())))
                {
                    // String CONTAINS any of the "Linux" list - Case insensitive
                    inputVMSpecs.OperatingSystem = Constants.OSLinux;
                    inputVMSpecs.ValidationMessage += string.Format(ValidationLiterals.SpecValueAssumed, VMSpecLiterals.OperatingSystem, inputVMSpecs.InputOriginalValues.OperatingSystem, Constants.OSLinux);
                }
                else
                {
                    inputVMSpecs.IsValid = false;
                    inputVMSpecs.ValidationMessage += string.Format(ValidationLiterals.InvalidSpec, VMSpecLiterals.OperatingSystem);
                }
            }
            else
            {
                // Operating System is blank
                inputVMSpecs.IsValid = false;
                inputVMSpecs.ValidationMessage += string.Format(ValidationLiterals.MissingSpec, VMSpecLiterals.OperatingSystem);
            }
        }

        /// <summary>
        /// Validate CPU Cores value in Input specifications
        /// </summary>
        /// <param name="inputVMSpecs">The Input specifications</param>
        private static void ValidateCPUCores(VMSpecs inputVMSpecs)
        {
            // CPU Cores Validation
            if (!string.IsNullOrWhiteSpace(inputVMSpecs.InputOriginalValues.CPUCores))
            {
                int result = 0;
                if (int.TryParse(inputVMSpecs.InputOriginalValues.CPUCores, out result) && (result > 0))
                {
                    inputVMSpecs.CPUCores = result;
                }

                if (result <= 0)
                {
                    inputVMSpecs.IsValid = false;
                    inputVMSpecs.ValidationMessage += string.Format(ValidationLiterals.SpecNotAPositiveInteger, VMSpecLiterals.CPUCores);
                }
            }
            else
            {
                // CPU Cores is blank
                inputVMSpecs.IsValid = false;
                inputVMSpecs.ValidationMessage += string.Format(ValidationLiterals.MissingSpec, VMSpecLiterals.CPUCores);
            }
        }

        /// <summary>
        /// Validate Memory value in Input specifications
        /// </summary>
        /// <param name="inputVMSpecs">The Input specifications</param>
        /// <param name="isInputMemoryInGB">Value indicating if input memory is in GB or not</param>
        private static void ValidateMemory(VMSpecs inputVMSpecs, bool isInputMemoryInGB)
        {
            // Memory validation
            if (!string.IsNullOrWhiteSpace(inputVMSpecs.InputOriginalValues.Memory))
            {
                double result = 0;
                if (double.TryParse(inputVMSpecs.InputOriginalValues.Memory, out result) && (result > 0))
                {
                    if (isInputMemoryInGB)
                    {
                        inputVMSpecs.MemoryInMB = 1024 * result;
                    }
                    else
                    {
                        inputVMSpecs.MemoryInMB = result;
                    }
                }
                else
                {
                    inputVMSpecs.IsValid = false;
                    inputVMSpecs.ValidationMessage += string.Format(ValidationLiterals.SpecNotAPositiveNumber, VMSpecLiterals.Memory);
                }
            }
            else
            {
                // Memory is blank
                inputVMSpecs.IsValid = false;
                inputVMSpecs.ValidationMessage += string.Format(ValidationLiterals.MissingSpec, VMSpecLiterals.Memory);
            }
        }

        /// <summary>
        /// Validate SSD Storage value in Input specifications
        /// </summary>
        /// <param name="inputVMSpecs">The Input specifications</param>
        private static void ValidateSSDStorageInGB(VMSpecs inputVMSpecs)
        {
            if (!string.IsNullOrWhiteSpace(inputVMSpecs.InputOriginalValues.SSDStorageInGB))
            {
                double result = 0;
                if (double.TryParse(inputVMSpecs.InputOriginalValues.SSDStorageInGB, out result) && (result >= 0))
                {
                    inputVMSpecs.SSDStorageInGB = result;
                }
                else
                {
                    inputVMSpecs.IsValid = false;
                    inputVMSpecs.ValidationMessage += string.Format(ValidationLiterals.SpecNotANumber, VMSpecLiterals.SSDStorageInGB);
                }
            }
            else
            {
                inputVMSpecs.SSDStorageInGB = 0;
            }
        }

        /// <summary>
        /// Validate SSD Number of Disks value in Input specifications
        /// </summary>
        /// <param name="inputVMSpecs">The Input specifications</param>
        private static void ValidateSSDNumOfDisks(VMSpecs inputVMSpecs)
        {
            // Number of Data Disks Validation
            if (!string.IsNullOrWhiteSpace(inputVMSpecs.InputOriginalValues.SSDNumOfDisks))
            {
                int result = 0;
                if (int.TryParse(inputVMSpecs.InputOriginalValues.SSDNumOfDisks, out result) && result >= 0)
                {
                    inputVMSpecs.SSDNumOfDisks = result;
                }
                else
                {
                    inputVMSpecs.IsValid = false;
                    inputVMSpecs.ValidationMessage += string.Format(ValidationLiterals.SpecNotANumber, VMSpecLiterals.SSDNumOfDisks);
                }
            }
            else
            {
                inputVMSpecs.SSDNumOfDisks = 0;
            }
        }

        /// <summary>
        /// Validate HDD Number of Disks value in Input specifications
        /// </summary>
        /// <param name="inputVMSpecs">The Input specifications</param>
        private static void ValidateHDDNumOfDisks(VMSpecs inputVMSpecs)
        {
            // Number of Data Disks Validation
            if (!string.IsNullOrWhiteSpace(inputVMSpecs.InputOriginalValues.HDDNumOfDisks))
            {
                int result = 0;
                if (int.TryParse(inputVMSpecs.InputOriginalValues.HDDNumOfDisks, out result) && result >= 0)
                {
                    inputVMSpecs.HDDNumOfDisks = result;
                }
                else
                {
                    inputVMSpecs.IsValid = false;
                    inputVMSpecs.ValidationMessage += string.Format(ValidationLiterals.SpecNotANumber, VMSpecLiterals.HDDNumOfDisks);
                }
            }
            else
            {
                inputVMSpecs.HDDNumOfDisks = 0;
            }
        }

        /// <summary>
        /// Validate HDD Storage value in Input specifications
        /// </summary>
        /// <param name="inputVMSpecs">The Input specifications</param>
        private static void ValidateHDDStorageInGB(VMSpecs inputVMSpecs)
        {
            if (!string.IsNullOrWhiteSpace(inputVMSpecs.InputOriginalValues.HDDStorageInGB))
            {
                double result = 0;
                if (double.TryParse(inputVMSpecs.InputOriginalValues.HDDStorageInGB, out result) && (result >= 0))
                {
                    inputVMSpecs.HDDStorageInGB = result;
                }
                else
                {
                    inputVMSpecs.IsValid = false;
                    inputVMSpecs.ValidationMessage += string.Format(ValidationLiterals.SpecNotANumber, VMSpecLiterals.HDDStorageInGB);
                }
            }
            else
            {
                inputVMSpecs.HDDStorageInGB = 0;
            }
        }

        /// <summary>
        /// Validate Price per month value in Input specifications
        /// </summary>
        /// <param name="inputVMSpecs">The Input specifications</param>
        private static void ValidatePricePerMonth(VMSpecs inputVMSpecs)
        {
            if (!string.IsNullOrWhiteSpace(inputVMSpecs.InputOriginalValues.PricePerMonth))
            {
                double result = 0;
                if (double.TryParse(inputVMSpecs.InputOriginalValues.PricePerMonth, out result) && (result >= 0))
                {
                    inputVMSpecs.PricePerMonth = result;
                }
                else
                {
                    inputVMSpecs.IsValid = false;
                    inputVMSpecs.ValidationMessage += string.Format(ValidationLiterals.SpecNotANumber, VMSpecLiterals.PricePerMonth);
                }
            }
            else
            {
                inputVMSpecs.PricePerMonth = 0;
            }
        }

        /// <summary>
        /// Validate Operating System value from Input Override data
        /// </summary>
        /// <param name="inputOverrideVMSpecs">The Input Override VM specifications</param>
        /// <param name="os_WindowsList">The list of Windows OS keywords</param>
        /// <param name="os_LinuxList">The list of Linux OS keywords</param>
        private static void ValidateOperatingSystem(OverrideVMSpecs inputOverrideVMSpecs, List<string> os_WindowsList, List<string> os_LinuxList)
        {
            // Operating System Validation
            if (!string.IsNullOrWhiteSpace(inputOverrideVMSpecs.InputOriginalValues.OperatingSystem))
            {
                // Check if String is any of the "Windows" list - Case insensitive
                if (os_WindowsList.Any(x => inputOverrideVMSpecs.InputOriginalValues.OperatingSystem.Equals(x, StringComparison.OrdinalIgnoreCase)))
                {
                    inputOverrideVMSpecs.OperatingSystem = Constants.OSWindows;
                }
                else if (os_LinuxList.Any(x => inputOverrideVMSpecs.InputOriginalValues.OperatingSystem.Equals(x, StringComparison.OrdinalIgnoreCase)))
                {
                    // String is any of the "Linux" list - Case insensitive
                    inputOverrideVMSpecs.OperatingSystem = Constants.OSLinux;
                }
                else if (os_WindowsList.Any(x => inputOverrideVMSpecs.InputOriginalValues.OperatingSystem.ToUpper().Contains(x.ToUpper())))
                {
                    // String CONTAINS any of the "Windows" list - Case insensitive
                    inputOverrideVMSpecs.OperatingSystem = Constants.OSWindows;
                    inputOverrideVMSpecs.ValidationMessage += string.Format(ValidationLiterals.SpecValueAssumed, OverrideVMSpecLiterals.OperatingSystem, inputOverrideVMSpecs.InputOriginalValues.OperatingSystem, Constants.OSWindows);
                }
                else if (os_LinuxList.Any(x => inputOverrideVMSpecs.InputOriginalValues.OperatingSystem.ToUpper().Contains(x.ToUpper())))
                {
                    // String CONTAINS any of the "Linux" list - Case insensitive
                    inputOverrideVMSpecs.OperatingSystem = Constants.OSLinux;
                    inputOverrideVMSpecs.ValidationMessage += string.Format(ValidationLiterals.SpecValueAssumed, OverrideVMSpecLiterals.OperatingSystem, inputOverrideVMSpecs.InputOriginalValues.OperatingSystem, Constants.OSLinux);
                }
                else
                {
                    inputOverrideVMSpecs.IsValid = false;
                    inputOverrideVMSpecs.ValidationMessage += string.Format(ValidationLiterals.InvalidSpec, VMSpecLiterals.OperatingSystem);
                }
            }
            else
            {
                // Operating System is blank
                inputOverrideVMSpecs.IsValid = false;
                inputOverrideVMSpecs.ValidationMessage += string.Format(ValidationLiterals.MissingSpec, OverrideVMSpecLiterals.OperatingSystem);
            }
        }

        /// <summary>
        /// Validate CPU Cores value from Input Override data
        /// </summary>
        /// <param name="inputOverrideVMSpecs">The Input Override VM specifications</param>
        private static void ValidateCPUCores(OverrideVMSpecs inputOverrideVMSpecs)
        {
            // CPU Cores Validation
            if (!string.IsNullOrWhiteSpace(inputOverrideVMSpecs.InputOriginalValues.CPUCores))
            {
                int result = 0;
                if (int.TryParse(inputOverrideVMSpecs.InputOriginalValues.CPUCores, out result) && (result > 0))
                {
                    inputOverrideVMSpecs.CPUCores = result;
                }

                if (result <= 0)
                {
                    inputOverrideVMSpecs.IsValid = false;
                    inputOverrideVMSpecs.ValidationMessage += string.Format(ValidationLiterals.SpecNotAPositiveInteger, OverrideVMSpecLiterals.CPUCores);
                }
            }
            else
            {
                // CPU Cores is blank
                inputOverrideVMSpecs.IsValid = false;
                inputOverrideVMSpecs.ValidationMessage += string.Format(ValidationLiterals.MissingSpec, OverrideVMSpecLiterals.CPUCores);
            }
        }

        /// <summary>
        /// Validate Memory value from Input Override data
        /// </summary>
        /// <param name="inputOverrideVMSpecs">The Input Override VM specifications</param>
        /// <param name="isInputMemoryInGB">Value indicating if Memory is in GB or not</param>
        private static void ValidateMemory(OverrideVMSpecs inputOverrideVMSpecs, bool isInputMemoryInGB)
        {
            // Memory validation
            if (!string.IsNullOrWhiteSpace(inputOverrideVMSpecs.InputOriginalValues.Memory))
            {
                double result = 0;
                if (double.TryParse(inputOverrideVMSpecs.InputOriginalValues.Memory, out result) && (result > 0))
                {
                    if (isInputMemoryInGB)
                    {
                        inputOverrideVMSpecs.MemoryInMB = 1024 * result;
                    }
                    else
                    {
                        inputOverrideVMSpecs.MemoryInMB = result;
                    }
                }
                else
                {
                    inputOverrideVMSpecs.IsValid = false;
                    inputOverrideVMSpecs.ValidationMessage += string.Format(ValidationLiterals.SpecNotAPositiveNumber, OverrideVMSpecLiterals.Memory);
                }
            }
            else
            {
                // Memory is blank
                inputOverrideVMSpecs.IsValid = false;
                inputOverrideVMSpecs.ValidationMessage += string.Format(ValidationLiterals.MissingSpec, OverrideVMSpecLiterals.Memory);
            }
        }

        /// <summary>
        /// Validate Number of Data Disks value from Input Override data
        /// </summary>
        /// <param name="inputOverrideVMSpecs">The Input Override specifications</param>
        private static void ValidateNumberOfDataDisks(OverrideVMSpecs inputOverrideVMSpecs)
        {
            // Number of Data Disks Validation
            if (!string.IsNullOrWhiteSpace(inputOverrideVMSpecs.InputOriginalValues.NumberOfDataDisks))
            {
                int result = 0;
                if (int.TryParse(inputOverrideVMSpecs.InputOriginalValues.NumberOfDataDisks, out result) && result >= 0)
                {
                    inputOverrideVMSpecs.NumberOfDataDisks = result;
                }
                else
                {
                    inputOverrideVMSpecs.IsValid = false;
                    inputOverrideVMSpecs.ValidationMessage += string.Format(ValidationLiterals.SpecNotANumber, OverrideVMSpecLiterals.NumberOfDataDisks);
                }
            }
            else
            {
                // CPU Cores is blank
                inputOverrideVMSpecs.IsValid = false;
                inputOverrideVMSpecs.ValidationMessage += string.Format(ValidationLiterals.MissingSpec, OverrideVMSpecLiterals.NumberOfDataDisks);
            }
        }

        /// <summary>
        /// Validate Has SSD Storage value from Input Override data
        /// </summary>
        /// <param name="inputOverrideVMSpecs">The Input Override specifications</param>
        private static void ValidateHasSSDStorage(OverrideVMSpecs inputOverrideVMSpecs)
        {
            // Instance Name Validation
            if (!string.IsNullOrWhiteSpace(inputOverrideVMSpecs.InputOriginalValues.HasSSDStorage))
            {
                // Check if String is any of the "True" list - Case insensitive
                if (Constants.HasSSDStorageColumnTRUEValues.Any(x => inputOverrideVMSpecs.InputOriginalValues.HasSSDStorage.Equals(x, StringComparison.OrdinalIgnoreCase)))
                {
                    inputOverrideVMSpecs.HasSSDStorage = true;
                }
                else if (Constants.HasSSDStorageColumnFALSEValues.Any(x => inputOverrideVMSpecs.InputOriginalValues.HasSSDStorage.Equals(x, StringComparison.OrdinalIgnoreCase)))
                {
                    inputOverrideVMSpecs.HasSSDStorage = false;
                }
                else
                {
                    inputOverrideVMSpecs.IsValid = false;
                    inputOverrideVMSpecs.ValidationMessage += string.Format(ValidationLiterals.SpecNotYesNo, OverrideVMSpecLiterals.HasSSDStorage);
                }
            }
            else
            {
                // Instance Name is blank
                inputOverrideVMSpecs.IsValid = false;
                inputOverrideVMSpecs.ValidationMessage += string.Format(ValidationLiterals.MissingSpec, OverrideVMSpecLiterals.HasSSDStorage);
            }
        }

        /// <summary>
        /// Validate Azure VM SKU Override value from Input Override data
        /// </summary>
        /// <param name="inputOverrideVMSpecs">The Input Override specifications</param>
        private static void ValidateAzureVMOverride(OverrideVMSpecs inputOverrideVMSpecs)
        {
            if (!string.IsNullOrWhiteSpace(inputOverrideVMSpecs.InputOriginalValues.AzureVMOverride))
            {
                inputOverrideVMSpecs.AzureVMOverride = inputOverrideVMSpecs.InputOriginalValues.AzureVMOverride.Length > Constants.MaxLengthAzureVMOverride ?
                     inputOverrideVMSpecs.InputOriginalValues.AzureVMOverride.Substring(0, Constants.MaxLengthAzureVMOverride) : inputOverrideVMSpecs.InputOriginalValues.AzureVMOverride;
            }
            else
            {
                inputOverrideVMSpecs.AzureVMOverride = string.Empty;
            }
        }

        /// <summary>
        /// Validate Comments value from Input Override data
        /// </summary>
        /// <param name="inputOverrideVMSpecs">The Input Override specifications</param>
        private static void ValidateComments(OverrideVMSpecs inputOverrideVMSpecs)
        {
            if (!string.IsNullOrWhiteSpace(inputOverrideVMSpecs.InputOriginalValues.Comments))
            {
                if (inputOverrideVMSpecs.InputOriginalValues.Comments.Length <= Constants.MaxLengthComments)
                {
                    inputOverrideVMSpecs.Comments = inputOverrideVMSpecs.InputOriginalValues.Comments;
                }
                else
                {
                    inputOverrideVMSpecs.Comments = inputOverrideVMSpecs.InputOriginalValues.Comments.Substring(0, Constants.MaxLengthComments);
                    inputOverrideVMSpecs.ValidationMessage += string.Format(ValidationLiterals.SpecExceedsStringLimit, OverrideVMSpecLiterals.Comments, Constants.MaxLengthComments);
                }
            }
            else
            {
                inputOverrideVMSpecs.Comments = string.Empty;
            }
        }

        /// <summary>
        /// Validate mapped Azure VM SKU value from Input Override data
        /// </summary>
        /// <param name="inputOverrideVMSpecs">The Input Override specifications</param>
        private static void ValidateAzureProjVMSize(OverrideVMSpecs inputOverrideVMSpecs)
        {
            if (!string.IsNullOrWhiteSpace(inputOverrideVMSpecs.InputOriginalValues.AzureProjVMSize))
            {
                inputOverrideVMSpecs.AzureProjVMSize = inputOverrideVMSpecs.InputOriginalValues.AzureProjVMSize.Length > Constants.MaxLengthAzureVMOverride ?
                     inputOverrideVMSpecs.InputOriginalValues.AzureProjVMSize.Substring(0, Constants.MaxLengthAzureVMOverride) : inputOverrideVMSpecs.InputOriginalValues.AzureProjVMSize;
            }
            else
            {
                inputOverrideVMSpecs.AzureProjVMSize = string.Empty;
            }
        }
    }
}
