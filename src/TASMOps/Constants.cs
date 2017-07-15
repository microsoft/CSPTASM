// -----------------------------------------------------------------------
// <copyright file="Constants.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace TASMOps
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Class that contains constants used in the Ops project
    /// </summary>
    public class Constants
    {
        public const string OSWindows = "Windows";
        public const string OSLinux = "Linux";
        public const int MaxLengthAzureVMOverride = 25;
        public const int MaxLengthComments = 1000;
        public const string BoolDisplayValueForTRUE = "Yes";
        public const string BoolDisplayValueForFALSE = "No";
        public const string ManagedDiskStrFormat = "{0} x {1}; ";
        public const string MemoryinGBUnitsSuffix = "(in GB)";
        public const string MemoryinMBUnitsSuffix = "(in MB)";
        public const string CurrencyFormat = "({0})";
        public const string AzureResourceMeterMissing = "Meter not found in Rate Card. MeterCategory:{0}, MeterSubCategory:{1}, MeterName:{2}, Region:{3} ";
        public static readonly string[] HasSSDStorageColumnTRUEValues = { "Yes", "Y" };
        public static readonly string[] HasSSDStorageColumnFALSEValues = { "No", "N" };
    }

    /// <summary>
    /// Class that contains Constants defining text for Virtual machine specifications 
    /// </summary>
    public class VMSpecLiterals
    {
        public const string InstanceName = "Instance Name";
        public const string OperatingSystem = "Operating System";
        public const string CPUCores = "CPU Cores";
        public const string Memory = "Memory";
        public const string SSDStorageInGB = "SSD Storage In GB";
        public const string SSDNumOfDisks = "SSD Number of Disks";
        public const string HDDStorageInGB = "HDD Storage In GB";
        public const string HDDNumOfDisks = "HDD Number of Disks";
        public const string PricePerMonth = "Price Per Month";
        public const string AzureVMOverride = "Azure VM Override";
        public const string Comments = "Comments";

        public const string IsValid = "Is Valid?";
        public const string ValidationMessage = "Validation Message";
    }

    /// <summary>
    /// Class that contains Constants defining text for Override specifications  
    /// </summary>
    public class OverrideVMSpecLiterals
    {
        public const string OperatingSystem = "Operating System";
        public const string CPUCores = "CPU Cores";
        public const string Memory = "Memory";
        public const string NumberOfDataDisks = "Number Of Data Disks";
        public const string HasSSDStorage = "Has SSD Storage";
        public const string AzureVMOverride = "Azure VM Override";
        public const string AzureProjVMSize = "Mapped Azure VM SKU";
        public const string AzureProjVMCores = "Azure VM Cores";
        public const string AzureProjVMMemory = "Azure VM Memory";
        public const string Comments = "Comments";

        public const string IsValid = "Is Valid?";
        public const string ValidationMessage = "Validation Message";
    }

    /// <summary>
    /// Class that contains Constants defining template text for validation messages 
    /// </summary>
    public class ValidationLiterals
    {
        public const string SpecValidationFailed = "Validation Failed: ";
        public const string MissingSpec = "Missing '{0}'; ";
        public const string InvalidSpec = "Invalid '{0}'; ";
        public const string SpecValueAssumed = "'{0}' value:'{1}' assumed as '{2}'; ";
        public const string SpecNotAPositiveInteger = "'{0}' should be a positive integer; ";
        public const string SpecNotAPositiveNumber = "'{0}' should be a positive number; ";
        public const string SpecNotANumber = "'{0}' should be a non-negative number; ";
        public const string SpecExceedsStringLimit = "'{0}' should not exceed {1} chars; ";
        public const string SpecNotYesNo = "'{0}' should be 'Yes' or 'No'; ";
    }

    /// <summary>
    /// Class that contains Constants defining text for mapping output and estimate columns
    /// </summary>
    public class AzureProjVMLiterals
    {
        public const string VMSize = "Mapped Azure VM SKU";
        public const string Cores = "Azure VM Cores";
        public const string Memory = "Azure VM Memory";
        public const string PremiumDisksStr = "Premium Disks";
        public const string StandardDisksStr = "Standard Disks";
        public const string PremiumDisksMonthlyCost = "Premium Disks Monthly Cost";
        public const string StandardDisksMonthlyCost = "Standard Disks Monthly Cost";
        public const string ComputeHoursRate = "Compute Hours Rate";
        public const string ComputeHoursMonthlyCost = "Compute Hours Monthly Cost";
        public const string AzureVMTotalMonthlyCost = "Azure VM Total Monthly Cost";
        public const string MonthlyGrossMarginEstimates = "Monthly Gross Margin Estimates";
        public const string AzureProjectionComments = "Mapping Comments";
    }

    /// <summary>
    /// Class that contains Constants defining template text for mapping related messages
    /// </summary>
    public class AzureVMProjectionCommentsLiterals
    {
        public const string ProjectionCommentsVMMappedasperGenericOverride = "Azure VM has been mapped as per Override file";
        public const string ProjectionCommentsGenericOverrideExceedDiskCount = "Unable to map as per Override file as number of disks mapped for VM exceed max disk count for the specified Override SKU";
        public const string ProjectionCommentsGenericOverrideNotFound = "Unable to map as per Override file as value specified is not found/valid in the Azure region";
        public const string ProjectionCommentsVMMapasperSpecificOverride = "Azure VM has been mapped as per override value in input file";
        public const string ProjectionCommentsSpecificOverrideExceedDiskCount = "Unable to map as per Override value in input file as number of disks mapped for VM exceed max disk count for the specified Override SKU";
        public const string ProjectionCommentsSpecificOverrideNotFound = "Unable to map as per Override value in input file as value specified is not found/valid in the Azure region";
        public const string ProjectionCommentsVMCannotbeMapped = "No Azure VM SKU found to map the input VM specifications. Check the list of included Azure VM SKUs in options";
    }

    /// <summary>
    /// Class that contains Constants related to API calls including API URL templates
    /// </summary>
    public class APIConstants
    {
        public const int APICallDefaultLimit = 2;

        // AuthManager / AAD Ops
        public const string AADURL = "https://login.windows.net/{0}/oauth2/token";

        // Partner Center Ops
        public const string PCAPIUrl = "https://api.partnercenter.microsoft.com";
        public static readonly string PCAzureCSPRateCardAPIUrl = PCAPIUrl + "/v1/ratecards/azure?currency={0}&region={1}";

        // Azure VM / ARM Ops
        public const string ARMAPIResourceURL = "https://management.azure.com/";
        public const string ARMAPIURL = "https://management.azure.com";
        public const string ARMAPIGetAzureVMSizesUrl = "{0}/subscriptions/{1}/providers/Microsoft.Compute/locations/{2}/vmSizes?api-version={3}";
        public const string ARMAPIGetRGListUrl = "{0}/subscriptions/{1}/resourcegroups?api-version={2}";
        public const string ARMComputeAPIVersion = "2017-03-30";
        public const string ARMRGAPIVersion = "2017-05-10";
    }

    /// <summary>
    /// Class that contains Constants containing template text for API error messages
    /// </summary>
    public class APICallErrorLiterals
    {
        public const string AADTokenFetchNotSuccessRespCodeError = "Error occured while fetching Azure AD Token. Details: {0}";
        public const string PCRateCardFetchNotSuccessRespCodeError = "Error occured while fetching Rate Card. Status Code: {0}. Error details: {1}";
        public const string RateCardNullOrZeroRecordsError = "Error occured while fetching Rate Card. No records received.";
        public const string ARMAPIError = "Error occured while running ARM API. Details: {0}";
    }

    /// <summary>
    /// Class that contains Disk Storage mapping related constants
    /// </summary>
    public class DiskConstants
    {
        public const int AzureManagedDiskLevelDepthForDefaultMapping = 2;
    }

    /// <summary>
    /// Class that contains Constants defining text for Azure resource meters
    /// </summary>
    public class AzureResourceMeterConstants
    {
        // VM Meters - Compute
        public const string VMComputeMeterCategory = "Virtual Machines";
        public const string VMComputeMeterName = "Compute Hours";
        public const string VMComputeMeterSubCategoryWindowsString = "{0} VM ({1})";
        public const string VMComputeMeterSubCategoryLinuxString = "{0} VM";

        // Storage VMDisk - Managed
        public const string VMManagedDiskMeterCategory = "Storage";
        public const string VMManagedDiskMeterSubCategory = "Locally Redundant";
    }

    /// <summary>
    /// Class that contains Constants defining Regex strings for Azure VM SKU related
    /// </summary>
    public class AzureVMHelperConstants
    {
        // AzureVMMeterHelper Regex
        public const string AzureVMSupportSSDRegex1 = @"^(Standard_[DG])S(.+)$";
        public const string AzureVMSupportSSDRegex2 = @"^(Standard_[FLM])(.+)S$";
        public const string AzureVMASeriesRegex = @"^(Basic|Standard)_(A.+)$";
        public const string AzureVMBasicSeriesString = "Basic";
        public const string AzureVMSizePromoString = "PROMO";

        public const string AzureVMSupportSSDRegex = @"(_[DG]S|_[FLM]\d{1,3}s)";

        // AzureVMMeterHelper Regex
        public static readonly Dictionary<AzureVMSeriesTypes, string> AzureVMSeriesTypesRegex = new Dictionary<AzureVMSeriesTypes, string>()
        {
            { AzureVMSeriesTypes.Basic_A, @"^Basic_A(.+)$" },
            { AzureVMSeriesTypes.Standard_A, @"^Standard_A(.+)$" },
            { AzureVMSeriesTypes.Standard_D, @"^Standard_D(.+)$" },
            { AzureVMSeriesTypes.Standard_F, @"^Standard_F(.+)$" },
            { AzureVMSeriesTypes.Standard_G, @"^Standard_G(.+)$" },
            { AzureVMSeriesTypes.Standard_H, @"^Standard_H(.+)$" },
            { AzureVMSeriesTypes.Standard_L, @"^Standard_L(.+)$" },
            
            // {AzureVMSeriesTypes.Standard_M, @"^Standard_M(.+)$" },  M - not supported at the moment by this mapping tool
            { AzureVMSeriesTypes.Standard_NC, @"^Standard_NC(.+)$" },
            { AzureVMSeriesTypes.Standard_NV, @"^Standard_NV(.+)$" }
        };
    }

    /// <summary>
    /// Enumeration of columns in input VM specifications file for the purpose of defining sequence 
    /// </summary>
    public enum VMSpecsSequenceId
    {
        InstanceName, OperatingSystem, CPUCores, MemoryInMB, SSDStorageInGB, SSDNumOfDisks, HDDStorageInGB, HDDNumOfDisks, PricePerMonth, AzureVMOverride, Comments
    }

    /// <summary>
    /// Enumeration of columns in input Override file for the purpose of defining sequence 
    /// </summary>
    public enum OverrideVMSpecsSequenceId
    {
        OperatingSystem, CPUCores, MemoryInMB, NumberOfDataDisks, HasSSDStorage, AzureVMOverride, AzureProjVMSize, AzureProjVMCores, AzureProjVMMemory, Comments
    }

    /// <summary>
    /// Class that contains Constants defining text for Azure resource meters
    /// </summary>
    public enum AzureVMSeriesTypes
    {
        Basic_A, Standard_A,
        Standard_D, Standard_F,
        Standard_G, Standard_H,
        Standard_L, // Standard_M, M - not supported at the moment by this mapping tool
        Standard_NC, Standard_NV
    }    
}
