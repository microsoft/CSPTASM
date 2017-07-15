// -----------------------------------------------------------------------
// <copyright file="Constants.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace TASM
{
    using System.Collections.Generic;
    using System.Linq;
    using TASMOps;
    using TASMOps.Model;
    using TASMOps.Util;

    /// <summary>
    /// Class that contains constants used in the TASM UI
    /// </summary>
    public class Constants
    {
        public const string NameoftheTool = @"TCO & Azure SKU Mapper (TASM)";
        public const string ToolLimitations = @"The SKU mappings and cost estimate provided by the tool are a first pass/draft recommendation based on a limited set of criteria. These recommendations are meant to be a starting point for more detailed analysis and mapping revisions which need to be performed by a certified Azure Architect based on the compute requirements as dictated by the workloads running inside the VM.";

        public const string CSPLocale = "en-US";
        public const int ExceptionMsgLengthLimit = 200;
        public const int OverallProgressTotalSteps = 9;
        public const string RateCardFileExt = ".txt";
        public const string SavedDataDir = "SavedData//";

        public const int CustomDialogBoxLeft = 100;
        public const int CustomDialogBoxTop = 100;

        public const string ConfigTestButtonText = "Test";
        public const string ConfigTestinProgressButtonText = "Testing...";
        public const string ConfigCancelinProgressButtonText = "Cancelling...";

        public const string GBStr = "GB";

        public static readonly Dictionary<string, string> LocationAsPerARMSpecsMap = new Dictionary<string, string>
        {
            { "East Asia", "eastasia" },
            { "Southeast Asia", "southeastasia" },
            { "Australia East", "australiaeast" },
            { "Australia Southeast", "australiasoutheast" },
            { "Brazil South", "brazilsouth" },
            { "Canada Central", "canadacentral" },
            { "Canada East", "canadaeast" },
            { "North Europe", "northeurope" },
            { "West Europe", "westeurope" },
            { "Central India", "centralindia" },
            { "South India", "southindia" },
            { "West India", "westindia" },
            { "Japan East", "japaneast" },
            { "Japan West", "japanwest" },
            { "Korea Central", "koreacentral" },
            { "Korea South", "koreasouth" },
            { "UK South", "uksouth" },
            { "UK West", "ukwest" },
            { "Central US", "centralus" },
            { "East US", "eastus" },
            { "East US 2", "eastus2" },
            { "North Central US", "northcentralus" },
            { "South Central US", "southcentralus" },
            { "West US", "westus" },
            { "West US 2", "westus2" },
            { "West Central US", "westcentralus" }
        };

        public static readonly Dictionary<string, string> LocationAsPerPricingSpecsMap = new Dictionary<string, string>
        {
            { "East Asia", "AP East" },
            { "Southeast Asia", "AP Southeast" },
            { "Australia East", "AU East" },
            { "Australia Southeast", "AU Southeast" },
            { "Brazil South", "BR South" },
            { "Canada Central", "CA Central" },
            { "Canada East", "CA East" },
            { "North Europe", "EU North" },
            { "West Europe", "EU West" },
            { "Central India", "IN Central" },
            { "South India", "IN South" },
            { "West India", "IN West" },
            { "Japan East", "JA East" },
            { "Japan West", "JA West" },
            { "Korea Central", "KR Central" },
            { "Korea South", "KR South" },
            { "UK South", "UK South" },
            { "UK West", "UK West" },
            { "Central US", "US Central" },
            { "East US", "US East" },
            { "East US 2", "US East 2" },
            { "North Central US", "US North Central" },
            { "South Central US", "US South Central" },
            { "West US", "US West" },
            { "West US 2", "US West 2" },
            { "West Central US", "US West Central" }
        };
    }

    /// <summary>
    /// Class that contains constants for status messages on TASM UI
    /// </summary>
    public class UIStatusMessages
    {
        public const string TASMUIOperationInitiateCancel = "Cancelling operation...";
        public const string TASMUIOperationCancelled = "Operation Cancelled.";
        public const string TASMUIOperationError = "Operation Failed: {0}";

        public const string FetchRateCardInitiated = "Getting CSP Azure rate card for CSP region: {0}. This may take a few minutes, please be patient...";
        public const string FetchRateCardCredsMissing = "Rate card get operation cannot be initiated as required CSP Account credentials are missing. Please provide the credentials in the configuration and then rerun this step.";
        public const string FetchRateCardCompleted = "Rate card get operation complete.";
        public const string FetchRateCardInitiateFailed = "Initiation of rate card get failed: {0}";
        public const string FetchRateCardFailed = "Rate card get operation failed: {0}";

        public const string FetchAzureVMSizesInitiated = "Getting Azure VM SKUs for Azure region: {0}. This may take a few minutes, please be patient...";
        public const string FetchAzureVMSizesCredsMissing = "Azure VM SKU get operation cannot be initiated as required CSP Account credentials are missing. Please provide the credentials in the configuration and then rerun this step.";
        public const string FetchAzureVMSizesCompleted = "Azure VM SKU get complete.";
        public const string FetchAzureVMSizesInitiateFailed = "Initiation of Azure VM SKU get failed: {0}";
        public const string FetchAzureVMSizesFailed = "Azure VM SKU get failed: {0}";

        public const string FileDialogFailed = "File Dialog failed: {0}";

        public const string MapnEstimateInitiated = "Initiated operation to map and estimate Azure VMs...";
        public const string MapnEstimateRateCardMissing = "Mapping and Estimation cannot be initiated without the CSP Azure rate card. Please get the rate Card and then rerun this step.";
        public const string MapnEstimateAzureVMSizesMissing = "Mapping and Estimation cannot be initiated without the Azure VM SKUs. Please get the Azure VM SKUs and then rerun this step.";
        public const string MapnEstimateVMSpecsFileMissing = "Mapping and Estimation cannot be initiated without the 'VM Specifications file'. Please select the file and then rerun this step.";
        public const string MapnEstimateOutputFileMissing = "Mapping and Estimation cannot be initiated without 'Mapping file'. Please select the file and then rerun this step.";
        public const string MapnEstimateOverrideOutputFileMissing = "Mapping and Estimation cannot be initiated without 'Output Override file' when 'Include overrides using files' is checked. Please select the file or uncheck the 'Include overrides using files' and then rerun this step.";
        public const string MapnEstimateCompleted = "Mapping and Estimation operation Complete.";
        public const string MapnEstimateInitiateFailed = "Initiation of Mapping and Estimation failed: {0}";
        public const string MapnEstimateFailed = "Mapping and Estimation operation failed: {0}";

        public const string Step = "Step ";
        public const string SubStepDelimiter = ".";

        // Step 1
        public const string ReadFileinProgress = "{0}: Reading file(s)...";
        public const string ReadVMSpecsFileCompleted = "{0}: 'VM Specifications file' read complete: {1} lines found, {2} rows extracted.";
        public const string ReadFileNoData = "{0}: No data found, check 'VM Specifications file' and try again.";
        public const string ReadOverrideFileProgress = "{0}: Reading 'Input Override file'...";
        public const string ReadOverrideFileCompleted = "{0}: 'Input Override file' read complete: {1} lines found, {2} rows extracted.";
        public const string ReadNoOverrideFile = "{0}: No 'Input Override file' specified, moving on to next step.";

        // Step 2
        public const string LoadFileinProgress = "{0}: Preparing data from read file(s)...";
        public const string LoadVMSpecsFileCompleted = "{0}: VM Specifications data preparation complete: {1} rows prepared.";
        public const string LoadOverrideVMSpecsFileCompleted = "{0}: Input Override data preparation complete: {1} rows prepared.";

        // Step 3
        public const string ValidateinProgress = "{0}: Validating data...";
        public const string ValidateVMSpecsFileCompleted = "{0}: VM Specifications data validation complete: {1} rows processed, {2} rows valid.";
        public const string ValidateOverrideVMSpecsFileCompleted = "{0}: Input Override data validation complete: {1} rows processed, {2} rows valid.";

        // Step 4
        public const string MapStorageinProgress = "{0}: Mapping storage to Azure managed disks...";
        public const string MapStorageCompleted = "{0}: Mapping of storage to Azure managed disks complete: {1} rows processed.";

        // Step 5
        public const string ProjectAzureVMinProgress = "{0}: Mapping VM Specifications to Azure VM SKUs...";
        public const string ProjectAzureVMCompleted = "{0}: Mapping of VM Specification to Azure VM SKU complete: {1} rows processed.";

        // Step 6
        public const string EstimateAzureVMinProgress = "{0}: Calculating cost estimates for mapped Azure VMs...";
        public const string EstimateAzureVMCompleted = "{0}: Cost estimate calculation for mapped Azure VMs complete: {1} rows processed.";

        // Step 7
        public const string FetchOutputOverrideVMListinProgress = "{0}: Getting the unique list of combinations from the mapped Azure VMs for generating Override output...";
        public const string FetchOutputOverrideVMListCompleted = "{0}: Obtained Override output from Azure VMs mapped: {1} rows processed, {2} unique rows obtained.";

        // Step 8
        public const string UnloadinProgress = "{0}: Preparing data for writing files...";

        // Step 8.1
        public const string UnloadVMResultsCompleted = "{0}: Data preparation for mapped Azure VMs and its estimates complete: {1} rows processed.";
        
        // Step 8.2
        public const string UnloadOutputOverrideCompleted = "{0}: Data preparation for Output Override complete: {1} rows processed.";
        
        // Step 8.3
        public const string UnloadInputOverrideCompleted = "{0}: Data preparation for Input Override complete: {1} rows processed.";

        // Step 9
        public const string WriteinProgress = "{0}: Writing data into files...";
        
        // Step 9.1
        public const string WriteVMResultsFileCompleted = "{0}: 'Mapping file' write complete: {1} lines written.";
        
        // Step 9.2
        public const string WriteInputOverrideFileCompleted = "{0}: Validated 'Input Override' file write complete: {1} lines written.";
        
        // Step 9.3
        public const string WriteOutputOverrideFileCompleted = "{0}: 'Output Override' file write complete: {1} lines written.";

        // Config Status Messages
        public const string PCAPITestMessageHeaderValMissing = "Missing value";
        public const string PartnerTenantIdMissing = "'Partner Tenant ID' required, please provide a value and try again.";
        public const string PCNativeAppIdMissing = "'Native App Id' required, please provide a value and try again.";
        public const string PCUsernameMissing = "'Username' required, please provide a value and try again.";
        public const string PCPasswordMissing = "'Password' required, please provide a value and try again.";
        public const string APITestMessageHeaderResult = "Test results";
        public const string APITestSuccess = "Test successful!";
        public const string APITestFailed = "Test failed: {0}";
        public const string APITestUnabletoInitiate = "Initiation of test failed: {0}";
        public const string APITestCannotPerform = "Failed to perform test: {0}";

        public const string PCARMAppIdMissing = "'ARM Native App ID' required, please provide a value and try again.";
        public const string CustomerTenantIdMissing = "'Customer Tenant ID' required, please provide a value and try again.";
        public const string CSPAzureSubTenantIdMissing = "'CSP Azure Subscription ID' required, please provide a value and try again.";

        public const string ConfigLoadFailed = "Failed to load the saved configuration details. Error details: {0}";
        public const string ConfigSaveFailed = "Failed to save configuration details. Error details: {0}";

        public const string RatecardFileCheckFailed = "Failed to check for saved Rate card data. Error details: {0}";
        public const string ActivityLogSaveFailed = "Failed to save Activity log. Error details: {0}";
        public const string LoadConfigDialogBoxFailed = "Failed to load TASM Configuration window. Error details: {0}";
        public const string LoadOptionsDialogBoxFailed = "Failed to load TASM Options window. Error details: {0}";

        public const string OptionsSaveFailed = "Failed to save changes made to Options. Error details: {0}";
        public const string OptionsLoadFailed = "Failed to load Options. Error details: {0}";

        public const string KeywordsMsgBoxHeader = "Keywords for OS";
        public const string WindowsKeywordMissing = "Type a keyword for Windows OS and then click on Add";
        public const string KeywordAlreadyPresent = "The specified keyword is already present in the list";
        public const string LinuxKeywordMissing = "Type a keyword for Linux OS and then click on Add";
        public const string KeywordisDefault = "The keyword cannot be removed as it is a default keyword";

        public const string KeywordsVMSKUSeriesHeader = "Azure VM SKUs";
        public const string AtleastOneSeries = "Atleast one Azure VM SKU series has to be included.";

        public const string GenerateVMSpecsSampleFailed = "Failed to generate VM Specifications file sample. Error details: {0}";
    }

    /// <summary>
    /// Enumeration of TASM UI operations
    /// </summary>
    public enum TASMUIOperations
    {
        NoOperation, FetchRateCard, FetchAzureVMSizeList, MapnEstimate
    }

    /// <summary>
    /// Enumeration of TASM Config operations
    /// </summary>
    public enum TASMUIConfigOperations
    {
        NoOperation, TestPCAPI, TestARMAPI
    }

    /// <summary>
    /// Class that constants related to CSV file
    /// </summary>
    public class CSVFileConstants
    {
        public static readonly VMSpecs VMSpecsSample = new VMSpecs()
        {
            InstanceName = "WebVM",
            OperatingSystem = "Windows",
            CPUCores = 4,
            MemoryInMB = 6144,
            SSDNumOfDisks = 2,
            SSDStorageInGB = 0,
            HDDNumOfDisks = 2,
            HDDStorageInGB = 1500,
            PricePerMonth = 325,
            AzureVMOverride = string.Empty,
            Comments = "This VM is hosting an internal website"
        };
    }

    /// <summary>
    /// Enumeration of file types
    /// </summary>
    public enum FileType
    {
        CSV, Text
    }

    /// <summary>
    /// Class that containts constants related to file and directory names
    /// </summary>
    public class PersistDataFileConstants
    {
        public const string SavedConfigDir = "SavedData//";
        public const string ConfigFileName = "config.txt";
        public const string OptionsFileName = "options.txt";
    }

    /// <summary>
    /// Class that containts constants for Option selections
    /// </summary>
    public class OptionsConstants
    {
        public static readonly List<int> VMSpecsSkipLines = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
        public static readonly List<int> InputOverrideSkipLines = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
        public static readonly List<double> MappingCoefficientsCores = new List<double>() { 0.9, 0.91, 0.92, 0.92, 0.93, 0.94, 0.95, 0.96, 0.97, 0.98, 0.99, 1 };
        public static readonly List<double> MappingCoefficientsMemory = new List<double>() { 0.9, 0.91, 0.92, 0.92, 0.93, 0.94, 0.95, 0.96, 0.97, 0.98, 0.99, 1 };
        public static readonly List<string> OSDiskHDDSizeList = AzureStorageMapper.GetHDDManagedDiskDisplayNameList(); 
        public static readonly List<string> OSDiskSSDSizeList = AzureStorageMapper.GetSSDManagedDiskDisplayNameList();
        public static readonly List<string> MemoryGBMBList = new List<string>() { "GB", "MB" };
        public static readonly List<int> HoursinaMonthList = new List<int>() { 744, 720, 696, 672 };

        public static readonly VMSpecsSequenceId[] DefaultVMSpecsInputSeq = new VMSpecsSequenceId[]
        {
            TASMOps.VMSpecsSequenceId.InstanceName,
            TASMOps.VMSpecsSequenceId.OperatingSystem,
            TASMOps.VMSpecsSequenceId.CPUCores,
            TASMOps.VMSpecsSequenceId.MemoryInMB,
            TASMOps.VMSpecsSequenceId.SSDStorageInGB,
            TASMOps.VMSpecsSequenceId.SSDNumOfDisks,
            TASMOps.VMSpecsSequenceId.HDDStorageInGB,
            TASMOps.VMSpecsSequenceId.HDDNumOfDisks,
            TASMOps.VMSpecsSequenceId.PricePerMonth,
            TASMOps.VMSpecsSequenceId.AzureVMOverride,
            TASMOps.VMSpecsSequenceId.Comments
        };
        public static readonly OverrideVMSpecsSequenceId[] DefaultOverrideVMSpecsInputSeq = new OverrideVMSpecsSequenceId[]
        {
            TASMOps.OverrideVMSpecsSequenceId.OperatingSystem,
            TASMOps.OverrideVMSpecsSequenceId.CPUCores,
            TASMOps.OverrideVMSpecsSequenceId.MemoryInMB,
            TASMOps.OverrideVMSpecsSequenceId.NumberOfDataDisks,
            TASMOps.OverrideVMSpecsSequenceId.HasSSDStorage,
            TASMOps.OverrideVMSpecsSequenceId.AzureVMOverride,
            TASMOps.OverrideVMSpecsSequenceId.AzureProjVMSize,
            TASMOps.OverrideVMSpecsSequenceId.AzureProjVMCores,
            TASMOps.OverrideVMSpecsSequenceId.AzureProjVMMemory,
            TASMOps.OverrideVMSpecsSequenceId.Comments
        };

        public const int DefaultVMSpecsSkipLines = 1;
        public const int DefaultInputOverrideSkipLines = 1;

        public static readonly List<string> DefaultWindowsKeywordsList = new List<string>{ "Windows"};
        public static readonly List<string> DefaultLinuxKeywordsList = new List<string> { "Linux", "CentOS", "Ubuntu"};

        public static readonly List<AzureVMSeriesTypes> DefaultAzureVMSKUSeriesIncluded = new List<AzureVMSeriesTypes>()
        {
            AzureVMSeriesTypes.Basic_A,
            AzureVMSeriesTypes.Standard_A,
            AzureVMSeriesTypes.Standard_D,
            AzureVMSeriesTypes.Standard_F,
            AzureVMSeriesTypes.Standard_G,
            AzureVMSeriesTypes.Standard_H,
            AzureVMSeriesTypes.Standard_L,

            // AzureVMSeriesTypes.AzureVMSeriesStandard_M,
            AzureVMSeriesTypes.Standard_NC,
            AzureVMSeriesTypes.Standard_NV
        };
        public static readonly List<AzureVMSeriesTypes> DefaultAzureVMSKUSeriesExcluded = new List<AzureVMSeriesTypes>();

        public const double DefaultMappingCoefCores = 1;
        public const double DefaultMappingCoefMemory = 1;

        public static readonly string DefaultOSDiskHDDSize = OSDiskHDDSizeList.First();
        public static readonly string DefaultOSDiskSSDSize = OSDiskSSDSizeList.First();
        public static readonly string DefaultMemoryGBMB = MemoryGBMBList.First();
        public static readonly int DefaultHoursinaMonth = HoursinaMonthList.First();
    }
}
