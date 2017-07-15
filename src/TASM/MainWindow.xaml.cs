// -----------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace TASM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Threading;
    using Microsoft.Win32;
    using TASM.Model;
    using TASM.Util;
    using TASMOps.Model;
    using TASMOps.Util;
    using TASMOps.Util.Online;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// The Background worker object, used to run operations in background
        /// </summary>
        private BackgroundWorker bworker = new BackgroundWorker();

        /// <summary>
        /// The Timer object, used for updating progress bar and status messages while operation is in progress
        /// </summary>
        private DispatcherTimer dispatcherTimer = new DispatcherTimer();

        /// <summary>
        /// Variable to store the TASM UI operation that is initiated
        /// </summary>
        private TASMUIOperations tasmUIOpInitiated = TASMUIOperations.NoOperation;

        /// <summary>
        /// Variable to store the CSP Account credentials object
        /// </summary>
        private CSPAccountCreds creds;

        /// <summary>
        /// Variable to store the selection from CSP Region Currency Dropdown
        /// </summary>
        private string cspRegionCurrencySelection;

        /// <summary>
        /// Variable to store the value indicatin if the rate card should be loaded from file or from API
        /// </summary>
        private bool loadRateCardfromFile;

        /// <summary>
        /// Variable to store the Azure region selection, value as per ARM specs
        /// </summary>
        private string azureRegionSelectionARM;

        /// <summary>
        /// Variable to store the Azure region selection
        /// </summary>
        private string azureRegionSelection;

        /// <summary>
        /// Variable to store the Input specifications filename
        /// </summary>
        private string vmSpecsFile;

        /// <summary>
        /// Variable to store the Input Override specifications filename
        /// </summary>
        private string inputOverrideFile;

        /// <summary>
        /// Variable to store the mapping output filename
        /// </summary>
        private string vmResultFile;

        /// <summary>
        /// Variable to store the Override output filename
        /// </summary>
        private string overrideOutputFile;

        /// <summary>
        /// Variable to store the value indicating is the Override by file option is checked
        /// </summary>
        private bool isFileOverrideIncluded;

        /// <summary>
        /// Operation progress object
        /// </summary>
        private OperationProgress opProgress;

        /// <summary>
        /// List of meters in the Azure CSP Rate Card
        /// </summary>
        private List<Meter> cspRateCardMeterList;

        /// <summary>
        /// CSP Rate card Currency
        /// </summary>
        private string cspRateCardCurrency;

        /// <summary>
        /// List of Azure VM SKUs
        /// </summary>
        private List<AzureVMSizeListItem> azureVMSizes;

        /// <summary>
        /// Variable to store Azure region selection as per pricing specs
        /// </summary>
        private string azureRegionSelectionPricing;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
            this.bworker.DoWork += this.BWorker_DoWork;
            this.bworker.WorkerSupportsCancellation = true;
            this.bworker.RunWorkerCompleted += this.BWorker_RunWorkerCompleted;
            this.bworker.WorkerReportsProgress = true;
            this.bworker.ProgressChanged += this.BWorker_ProgressChanged;

            this.dispatcherTimer.Tick += this.DispatcherTimer_Tick;
            this.dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 400);

            this.opProgress = new OperationProgress();
        }

        /// <summary>
        /// Method that runs when Timer ticks, updates the progress bar and status message
        /// </summary>
        /// <param name="sender">the Sender object</param>
        /// <param name="e">the Event Args object</param>
        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            this.UpdateProgressBar();
        }

        /// <summary>
        /// Method that updates the progress bar and status message for operation in progress
        /// </summary>
        private void UpdateProgressBar()
        {
            pbarOverallProgress.Value = this.opProgress.GetOverallProgress();
            lblCurrentOperation.Content = this.opProgress.OperationInProgressMsg;
        }

        /// <summary>
        /// Method that runs when Background worker progress is changed
        /// </summary>
        /// <param name="sender">the Sender object</param>
        /// <param name="e">the Event Args object</param>
        private void BWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e != null && e.UserState != null)
            {
                string reportProgressStatusMsg = (string)e.UserState;
                if (!string.IsNullOrWhiteSpace(reportProgressStatusMsg))
                {
                    this.AddStatusMessage(reportProgressStatusMsg);
                }
            }
        }

        /// <summary>
        /// Method that runs when Background worker completes async operation
        /// </summary>
        /// <param name="sender">the Sender object</param>
        /// <param name="e">the Event Args object</param>
        private void BWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                this.StopProgressBarifRunning();
                this.AddStatusMessage(string.Format(UIStatusMessages.TASMUIOperationCancelled));
            }
            else if (e.Error != null)
            {
                this.StopProgressBarifRunning();
                this.AddStatusMessage(string.Format(UIStatusMessages.TASMUIOperationError, e.Error.Message));
            }
            else
            {
                switch (this.tasmUIOpInitiated)
                {
                    case TASMUIOperations.NoOperation:
                        
                        // Do Nothing, This function won't be called for this enum value
                        break;

                    case TASMUIOperations.FetchRateCard:
                        this.AddStatusMessage(string.Format(UIStatusMessages.FetchRateCardCompleted));
                        break;

                    case TASMUIOperations.FetchAzureVMSizeList:
                        this.AddStatusMessage(string.Format(UIStatusMessages.FetchAzureVMSizesCompleted));
                        break;

                    case TASMUIOperations.MapnEstimate:
                        this.StopProgressBarifRunning();
                        this.AddStatusMessage(string.Format(UIStatusMessages.MapnEstimateCompleted));
                        break;

                    default:

                        // Do Nothing, This function won't be called for this enum value
                        break;
                }
            }

            this.ToggleButtonStatus(false, false);
        }

        /// <summary>
        /// Method that runs checks and stops the progress bar if running
        /// </summary>
        private void StopProgressBarifRunning()
        {
            if (this.tasmUIOpInitiated == TASMUIOperations.MapnEstimate)
            {
                this.opProgress.Stop();
                this.dispatcherTimer.Stop();
                this.UpdateProgressBar();
            }
        }

        /// <summary>
        /// Method that runs when Timer ticks, updates the progress bar and status message
        /// </summary>
        /// <param name="sender">the Sender object</param>
        /// <param name="e">the Event Args object</param>
        private void BWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bw = sender as BackgroundWorker;

            switch (this.tasmUIOpInitiated)
            {
                case TASMUIOperations.NoOperation:

                    // Do Nothing, This function won't be called for this enum value
                    break;

                case TASMUIOperations.FetchRateCard:
                    this.DoWork_FetchRateCard(bw, e);
                    break;

                case TASMUIOperations.FetchAzureVMSizeList:
                    this.DoWork_FetchAzureVMSizes(bw, e);
                    break;

                case TASMUIOperations.MapnEstimate:
                    this.DoWork_MapandEstimate(bw, e);
                    break;

                default:

                    // Do Nothing, This function won't be called for this enum value
                    break;
            }
        }

        /// <summary>
        /// Method that fetches the Azure CSP Rate Card
        /// </summary>
        /// <param name="bw">the Background worker object</param>
        /// <param name="e">the Event Args object</param>
        private void DoWork_FetchRateCard(BackgroundWorker bw, DoWorkEventArgs e)
        {
            try
            {
                bool isRateCardfromFile = true;
                string rateCardStr = string.Empty;
                string currency = string.Empty;
                if (this.loadRateCardfromFile)
                {
                    isRateCardfromFile = true;
                    rateCardStr = SavedDataUtil.LoadRateCardfromFile(this.cspRegionCurrencySelection);
                }

                if (string.IsNullOrWhiteSpace(rateCardStr))
                {
                    isRateCardfromFile = false;
                    rateCardStr = RateCardUtil.GetRateCard(this.creds);
                }

                this.cspRateCardMeterList = RateCardUtil.GetRateCard(rateCardStr, out currency);

                if (!string.IsNullOrWhiteSpace(currency))
                {
                    this.cspRateCardCurrency = currency;
                }
                else
                {
                    this.cspRateCardCurrency = string.Empty;
                }

                if (this.CheckIfBWorkerCancelled(bw, e))
                {
                    this.cspRateCardMeterList = null;
                    return;
                }

                if (!isRateCardfromFile)
                {
                    SavedDataUtil.SaveRateCardtoFile(this.cspRegionCurrencySelection, rateCardStr);
                }

                if (this.CheckIfBWorkerCancelled(bw, e))
                {
                    this.cspRateCardMeterList = null;
                    return;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method that checks if Background worker opration is cancelled
        /// </summary>
        /// <param name="bw">the Background worker object</param>
        /// <param name="e">the Event Args object</param>
        /// <returns> Returns true if worker has been cancelled, false otherwise</returns>
        private bool CheckIfBWorkerCancelled(BackgroundWorker bw, DoWorkEventArgs e)
        {
            if (bw.CancellationPending)
            {
                e.Cancel = true;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Loads the options selections from file
        /// </summary>
        /// <returns> Returns the option selection object loaded from file</returns>
        private TASMOptionsSelection LoadOptionSelection()
        {
           TASMOptionsSelection optionSelection = SavedDataUtil.LoadData<TASMOptionsSelection>(SavedDataUtil.GetOptionsFileName());
            if (optionSelection == null)
            {
                optionSelection = new TASMOptionsSelection(true);
            }
            
            return optionSelection;
        }

        /// <summary>
        /// Method that fetches the Azure VM SKU sizes
        /// </summary>
        /// <param name="bw">the Background worker object</param>
        /// <param name="e">the Event Args object</param>
        private void DoWork_FetchAzureVMSizes(BackgroundWorker bw, DoWorkEventArgs e)
        {
            try
            {
                TASMOptionsSelection optionSelection = this.LoadOptionSelection();
                this.azureVMSizes = AzureVMOps.GetAzureVMSizeList(this.creds, this.azureRegionSelectionARM);

                if (this.CheckIfBWorkerCancelled(bw, e))
                {
                    this.azureVMSizes = null;
                    this.azureRegionSelectionPricing = string.Empty;
                    return;
                }

                this.azureVMSizes = AzureVMListHelper.FilterAzureVMSizeList(this.azureVMSizes, optionSelection.AzureVMSKUSeriesIncluded);
                this.azureRegionSelectionPricing = Constants.LocationAsPerPricingSpecsMap[this.azureRegionSelection];

                if (this.CheckIfBWorkerCancelled(bw, e))
                {
                    this.azureVMSizes = null;
                    this.azureRegionSelectionPricing = string.Empty;
                    return;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method that performs Map and Estimate operation
        /// </summary>
        /// <param name="bw">the Background worker object</param>
        /// <param name="e">the Event Args object</param>
        private void DoWork_MapandEstimate(BackgroundWorker bw, DoWorkEventArgs e)
        {
            try
            {
                int bwProgress = 0;
                int opinProgressTotalSteps = 0;

                bool isInputOverrideFileIncluded = this.isFileOverrideIncluded && !string.IsNullOrWhiteSpace(this.inputOverrideFile);
                TASMOptionsSelection optionSelection = this.LoadOptionSelection();

                this.opProgress.Start(Constants.OverallProgressTotalSteps);

                // Step 1 - Read Files
                this.opProgress.NextStep(UIStatusMessages.ReadFileinProgress, isInputOverrideFileIncluded ? 2 : 1);
                
                // Step 1.1
                int numofLinesVMSpecsFile = FileUtil.GetNumOfLinesinFile(this.vmSpecsFile);
                List<string[]> vmSpecsData = FileUtil.ReadCSVFile(this.vmSpecsFile, optionSelection.VMSpecsSkipLines, Enum.GetNames(typeof(TASMOps.VMSpecsSequenceId)).Length);
                bw.ReportProgress(bwProgress++, string.Format(UIStatusMessages.ReadVMSpecsFileCompleted, this.opProgress.GetCurrentSubStep(), numofLinesVMSpecsFile, vmSpecsData.Count()));
                if (vmSpecsData.Count() == 0)
                {
                    bw.ReportProgress(bwProgress++, string.Format(UIStatusMessages.ReadFileNoData));
                    return;
                }

                this.opProgress.IncrementOpinProgressStep();
                this.opProgress.IncrementSubStepNumber();

                // Step 1.2
                List<string[]> inputOverrideData = null;
                if (isInputOverrideFileIncluded)
                {
                    int numofLinesInputOverrideFile = FileUtil.GetNumOfLinesinFile(this.inputOverrideFile);
                    inputOverrideData = FileUtil.ReadCSVFile(this.inputOverrideFile, optionSelection.InputOverrideSkipLines, Enum.GetNames(typeof(TASMOps.OverrideVMSpecsSequenceId)).Length);
                    bw.ReportProgress(bwProgress++, string.Format(UIStatusMessages.ReadOverrideFileCompleted, this.opProgress.GetCurrentSubStep(), numofLinesInputOverrideFile, inputOverrideData.Count()));
                }
                else if (this.isFileOverrideIncluded)
                {
                    bw.ReportProgress(bwProgress++, string.Format(UIStatusMessages.ReadNoOverrideFile, this.opProgress.GetCurrentSubStep()));
                }

                if (this.CheckIfBWorkerCancelled(bw, e))
                {
                    return;
                }

                // Step 2 - Load data from csv strings
                if (isInputOverrideFileIncluded && inputOverrideData.Count() > 0)
                {
                    opinProgressTotalSteps = inputOverrideData.Count() + vmSpecsData.Count();
                }
                else
                {
                    opinProgressTotalSteps = vmSpecsData.Count();
                }

                this.opProgress.NextStep(UIStatusMessages.LoadFileinProgress, opinProgressTotalSteps);

                // Step 2.1
                List<VMSpecs> vmSpecsList = new List<VMSpecs>();
                foreach (string[] vmSpecsRow in vmSpecsData)
                {
                    vmSpecsList.Add(IOLoadUtil.LoadVMSpecs(vmSpecsRow, optionSelection.VMSpecsInputSequence));
                    this.opProgress.IncrementOpinProgressStep();
                }

                bw.ReportProgress(bwProgress++, string.Format(UIStatusMessages.LoadVMSpecsFileCompleted, this.opProgress.GetCurrentSubStep(), vmSpecsList.Count()));

                // Step 2.2
                this.opProgress.IncrementSubStepNumber();
                List<OverrideVMSpecs> overrideVMSpecsList = new List<OverrideVMSpecs>();
                if (isInputOverrideFileIncluded && inputOverrideData.Count() > 0)
                {
                    foreach (string[] overrideVMSpecsRow in inputOverrideData)
                    {
                        overrideVMSpecsList.Add(IOLoadUtil.LoadOverrideVMSpecs(overrideVMSpecsRow, optionSelection.OverrideVMSpecsInputSequence));
                        this.opProgress.IncrementOpinProgressStep();
                    }

                    bw.ReportProgress(bwProgress++, string.Format(UIStatusMessages.LoadOverrideVMSpecsFileCompleted, this.opProgress.GetCurrentSubStep(), overrideVMSpecsList.Count()));
                }

                if (this.CheckIfBWorkerCancelled(bw, e))
                {
                    return;
                }

                // Step 3 - Validate
                if (isInputOverrideFileIncluded && overrideVMSpecsList.Count() > 0)
                {
                    opinProgressTotalSteps = vmSpecsList.Count() + overrideVMSpecsList.Count();
                }
                else
                {
                    opinProgressTotalSteps = vmSpecsList.Count();
                }

                this.opProgress.NextStep(UIStatusMessages.ValidateinProgress, opinProgressTotalSteps);

                // Step 3.1
                List<VMSpecs> validatedVMSpecsList = new List<VMSpecs>();
                foreach (VMSpecs vmSpecsItem in vmSpecsList)
                {
                    validatedVMSpecsList.Add(InputValidationUtil.ValidateVMSpecs(vmSpecsItem, optionSelection.IsMemoryGB(), optionSelection.WindowsKeywordsList, optionSelection.LinuxKeywordsList));
                    this.opProgress.IncrementOpinProgressStep();
                }

                bw.ReportProgress(bwProgress++, string.Format(UIStatusMessages.ValidateVMSpecsFileCompleted, this.opProgress.GetCurrentSubStep(), validatedVMSpecsList.Count(), validatedVMSpecsList.Count(x => x.IsValid)));

                // Step 3.2
                this.opProgress.IncrementSubStepNumber();
                List<OverrideVMSpecs> validatedOverrideVMSpecsList = new List<OverrideVMSpecs>();
                if (isInputOverrideFileIncluded && overrideVMSpecsList != null && overrideVMSpecsList.Count() > 0)
                {
                    foreach (OverrideVMSpecs overrideVMSpecsItem in overrideVMSpecsList)
                    {
                        validatedOverrideVMSpecsList.Add(InputValidationUtil.ValidateOverrideVMSpecs(overrideVMSpecsItem, optionSelection.IsMemoryGB(), optionSelection.WindowsKeywordsList, optionSelection.LinuxKeywordsList));
                        this.opProgress.IncrementOpinProgressStep();
                    }

                    bw.ReportProgress(bwProgress++, string.Format(UIStatusMessages.ValidateOverrideVMSpecsFileCompleted, this.opProgress.GetCurrentSubStep(), validatedOverrideVMSpecsList.Count(), validatedOverrideVMSpecsList.Count(x => x.IsValid)));
                }

                if (this.CheckIfBWorkerCancelled(bw, e))
                {
                    return;
                }

                // Step 4 - Map Storage
                this.opProgress.NextStep(UIStatusMessages.MapStorageinProgress, validatedVMSpecsList.Count());
                List<VMResult> vmResList = new List<VMResult>();
                foreach (VMSpecs vmSpecsItem in validatedVMSpecsList)
                {
                    VMResult vmResItem = new VMResult(vmSpecsItem);
                    if (vmSpecsItem.IsValid)
                    {
                        AzureStorageMapper.MapStorage(vmSpecsItem, vmResItem, optionSelection.OSDiskSSDSelection, optionSelection.OSDiskHDDSelection);
                    }

                    vmResList.Add(vmResItem);
                    this.opProgress.IncrementOpinProgressStep();
                }

                bw.ReportProgress(bwProgress++, string.Format(UIStatusMessages.MapStorageCompleted, this.opProgress.GetCurrentStep(), vmResList.Count(x => x.Specs.IsValid)));

                if (this.CheckIfBWorkerCancelled(bw, e))
                {
                    return;
                }

                // Step 5 - Project VM
                this.opProgress.NextStep(UIStatusMessages.ProjectAzureVMinProgress, vmResList.Count());
                AzureResourceRateCalc rateCalc = new AzureResourceRateCalc(this.cspRateCardMeterList);
                foreach (VMResult vmResItem in vmResList)
                {
                    if (vmResItem.Specs.IsValid)
                    {
                        AzureVMProjector.ProjectAzureVM(vmResItem, this.azureRegionSelectionPricing, this.azureVMSizes, rateCalc, validatedOverrideVMSpecsList, optionSelection.MappingCoefficientCoresSelection, optionSelection.MappingCoefficientMemorySelection);
                    }

                    this.opProgress.IncrementOpinProgressStep();
                }

                bw.ReportProgress(bwProgress++, string.Format(UIStatusMessages.ProjectAzureVMCompleted, this.opProgress.GetCurrentStep(), vmResList.Count(x => x.Specs.IsValid)));

                if (this.CheckIfBWorkerCancelled(bw, e))
                {
                    return;
                }

                // Step 6 - Estimate
                this.opProgress.NextStep(UIStatusMessages.EstimateAzureVMinProgress, vmResList.Count());
                foreach (VMResult vmResItem in vmResList)
                {
                    if (vmResItem.Specs.IsValid)
                    {
                        AzureStorageCostHelper.LoadMonthlyCostForManagedDisks(vmResItem, this.azureRegionSelectionPricing, rateCalc);
                        AzureVMCostHelper.LoadComputeHoursMonthlyCost(vmResItem, optionSelection.HoursinaMonthSelection);
                    }

                    this.opProgress.IncrementOpinProgressStep();
                }

                bw.ReportProgress(bwProgress++, string.Format(UIStatusMessages.EstimateAzureVMCompleted, this.opProgress.GetCurrentStep(), vmResList.Count(x => x.Specs.IsValid)));

                if (this.CheckIfBWorkerCancelled(bw, e))
                {
                    return;
                }

                // Step 7 - Fetch Azure Output Override List
                this.opProgress.NextStep(UIStatusMessages.FetchOutputOverrideVMListinProgress, 1);
                List<OverrideVMSpecs> outputOverrideVMSpecsList = new List<OverrideVMSpecs>();
                outputOverrideVMSpecsList = AzureVMProjector.GetUniqueProjectedListOfVMs(vmResList, this.azureRegionSelectionPricing, this.azureVMSizes, rateCalc, optionSelection.MappingCoefficientCoresSelection, optionSelection.MappingCoefficientMemorySelection);
                bw.ReportProgress(bwProgress++, string.Format(UIStatusMessages.FetchOutputOverrideVMListCompleted, this.opProgress.GetCurrentStep(), vmResList.Count(), outputOverrideVMSpecsList.Count()));

                if (this.CheckIfBWorkerCancelled(bw, e))
                {
                    return;
                }
                
                // Step 8 - Prepare output data
                opinProgressTotalSteps = vmResList.Count() + (this.isFileOverrideIncluded ? (isInputOverrideFileIncluded ? validatedOverrideVMSpecsList.Count() + outputOverrideVMSpecsList.Count() : outputOverrideVMSpecsList.Count()) : 0);
                this.opProgress.NextStep(UIStatusMessages.UnloadinProgress, opinProgressTotalSteps);

                // Step 8.1 - Prepare output data for VM Results
                List<string[]> outputVMSpecsStr = new List<string[]>();
                outputVMSpecsStr.Add(IOLoadUtil.UnloadVMSpecsHeader(optionSelection.IsMemoryGB(), this.cspRateCardCurrency));
                foreach (VMResult vmResItem in vmResList)
                {
                    outputVMSpecsStr.Add(IOLoadUtil.UnloadVMSpecsOutput(vmResItem, optionSelection.IsMemoryGB()));
                    this.opProgress.IncrementOpinProgressStep();
                }

                bw.ReportProgress(bwProgress++, string.Format(UIStatusMessages.UnloadVMResultsCompleted, this.opProgress.GetCurrentSubStep(), outputVMSpecsStr.Count()));

                // Step 8.2- Prepare output data for Output Override
                this.opProgress.IncrementSubStepNumber();
                List<string[]> outputOverrideStr = new List<string[]>();
                List<string[]> inputOverrideStr = new List<string[]>();
                if (this.isFileOverrideIncluded)
                {
                    outputOverrideStr.Add(IOLoadUtil.UnloadOverrideVMSpecsHeader(optionSelection.IsMemoryGB(), true));
                    foreach (OverrideVMSpecs overrideItem in outputOverrideVMSpecsList)
                    {
                        outputOverrideStr.Add(IOLoadUtil.UnloadOverrideVMSpecsOutput(overrideItem, optionSelection.IsMemoryGB(), true));
                        this.opProgress.IncrementOpinProgressStep();
                    }

                    bw.ReportProgress(bwProgress++, string.Format(UIStatusMessages.UnloadOutputOverrideCompleted, this.opProgress.GetCurrentSubStep(), outputOverrideStr.Count()));

                    // Step 8.3 - Prepare output data for Input Override with validation results
                    this.opProgress.IncrementSubStepNumber();
                    
                    if (isInputOverrideFileIncluded)
                    {
                        inputOverrideStr.Add(IOLoadUtil.UnloadOverrideVMSpecsHeader(optionSelection.IsMemoryGB(), false));
                        foreach (OverrideVMSpecs inputOverrideItem in validatedOverrideVMSpecsList)
                        {
                            inputOverrideStr.Add(IOLoadUtil.UnloadOverrideVMSpecsOutput(inputOverrideItem, optionSelection.IsMemoryGB(), false));
                            this.opProgress.IncrementOpinProgressStep();
                        }

                        bw.ReportProgress(bwProgress++, string.Format(UIStatusMessages.UnloadInputOverrideCompleted, this.opProgress.GetCurrentSubStep(), inputOverrideStr.Count()));
                    }
                }

                if (this.CheckIfBWorkerCancelled(bw, e))
                {
                    return;
                }
                
                // Step 9 - Write output data
                opinProgressTotalSteps = this.isFileOverrideIncluded ? (isInputOverrideFileIncluded ? 3 : 2) : 1;
                this.opProgress.NextStep(UIStatusMessages.WriteinProgress, opinProgressTotalSteps);

                // Step 9.1 - Write output data for VMResults
                FileUtil.WriteCSVFile(this.vmResultFile, outputVMSpecsStr);
                int numofLinesOutputFile = FileUtil.GetNumOfLinesinFile(this.vmResultFile);
                this.opProgress.IncrementOpinProgressStep();
                bw.ReportProgress(bwProgress++, string.Format(UIStatusMessages.WriteVMResultsFileCompleted, this.opProgress.GetCurrentSubStep(), numofLinesOutputFile));

                // Step 9.2 - Write output data for Output Override
                this.opProgress.IncrementSubStepNumber();
                if (this.isFileOverrideIncluded)
                {
                    FileUtil.WriteCSVFile(this.overrideOutputFile, outputOverrideStr);
                    numofLinesOutputFile = FileUtil.GetNumOfLinesinFile(this.overrideOutputFile);
                    this.opProgress.IncrementOpinProgressStep();
                    bw.ReportProgress(bwProgress++, string.Format(UIStatusMessages.WriteOutputOverrideFileCompleted, this.opProgress.GetCurrentSubStep(), numofLinesOutputFile));

                    // Step 9.3 - Write output data for Input Override
                    this.opProgress.IncrementSubStepNumber();
                    if (isInputOverrideFileIncluded)
                    {
                        FileUtil.WriteCSVFile(this.inputOverrideFile, inputOverrideStr);
                        numofLinesOutputFile = FileUtil.GetNumOfLinesinFile(this.inputOverrideFile);
                        this.opProgress.IncrementOpinProgressStep();
                        bw.ReportProgress(bwProgress++, string.Format(UIStatusMessages.WriteInputOverrideFileCompleted, this.opProgress.GetCurrentSubStep(), numofLinesOutputFile));
                    }
                }

                this.opProgress.Stop();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Click event for Get Rate Card button, initiates the Rate Card Fetch operation
        /// </summary>
        /// <param name="sender">the Sender object</param>
        /// <param name="e">the Event Args object</param>
        private void btnFetchRateCard_Click(object sender, RoutedEventArgs e)
        {
            this.ValidateandInitiateAsyncFetchRateCard();
        }

        /// <summary>
        /// Validates inputs on UI and initiates the Rate Card Fetch operation async using Background worker object
        /// </summary>
        private void ValidateandInitiateAsyncFetchRateCard()
        {
            bool isValidateSuccess = false;
            try
            {
                this.creds = SavedDataUtil.LoadData<CSPAccountCreds>(SavedDataUtil.GetConfigFileName());

                // Validate before operations
                if (this.creds == null)
                {
                    isValidateSuccess = false;
                    this.AddStatusMessage(string.Format(UIStatusMessages.FetchRateCardCredsMissing));
                }
                else
                {
                    isValidateSuccess = true;
                    this.creds.CSPLocale = Constants.CSPLocale;
                }

                // Initiate Operation
                if (isValidateSuccess)
                {
                    this.cspRegionCurrencySelection = cboCSPRegionCurrency.SelectedValue.ToString();

                    // Extract the Region and Currency from the UI Selection, Set the values in the Creds object
                    char[] charSeparators = new char[] { '-' };
                    string[] cspRegCurrStrSplit = this.cspRegionCurrencySelection.Split(charSeparators, 2, StringSplitOptions.RemoveEmptyEntries);
                    this.creds.CSPRegion = cspRegCurrStrSplit[0];
                    this.creds.CSPCurrency = cspRegCurrStrSplit[1];

                    if (chkLoadRateCardfromFile.IsEnabled)
                    {
                        this.loadRateCardfromFile = chkLoadRateCardfromFile.IsChecked.Value;
                    }
                    else
                    {
                        this.loadRateCardfromFile = false;
                    }

                    this.ToggleButtonStatus(true, false);
                    this.tasmUIOpInitiated = TASMUIOperations.FetchRateCard;
                    this.AddStatusMessage(string.Format(UIStatusMessages.FetchRateCardInitiated, this.creds.CSPRegion));
                    this.bworker.RunWorkerAsync();
                }
            }
            catch (Exception ex)
            {
                this.tasmUIOpInitiated = TASMUIOperations.NoOperation;
                this.AddStatusMessage(string.Format(UIStatusMessages.FetchRateCardInitiateFailed, ex.Message));
                this.ToggleButtonStatus(false, false);
            }
        }

        /// <summary>
        /// Add status message to the UI
        /// </summary>
        /// <param name="msg">the message to be added</param>
        /// <param name="appendNewLine">A value indicating if a new line it to be added along with the message</param>
        private void AddStatusMessage(string msg, bool appendNewLine = true)
        {
            if (msg.Length > Constants.ExceptionMsgLengthLimit)
            {
                msg = msg.Substring(0, Constants.ExceptionMsgLengthLimit) + "...";
            }

            if (appendNewLine)
            {
                msg = string.Format("{0}{1}", msg, Environment.NewLine);
            }

            txtOperationStatusMsg.AppendText(string.Format("{0}", msg));
            txtOperationStatusMsg.ScrollToEnd();
        }

        /// <summary>
        /// Toggles the status of buttons on the UI
        /// </summary>
        /// <param name="isInitiated">A value indicating if the operation is initiated, this will enable the status of buttons, otherwsie will disable</param>
        /// <param name="isCancelled">A value indicating operation is cancelled</param>
        private void ToggleButtonStatus(bool isInitiated, bool isCancelled)
        {
            // Toggle Button Status
            if (isInitiated)
            {
                btnFetchRateCard.IsEnabled = false;
                btnFetchAzureVMSizeList.IsEnabled = false;
                btnBrowseVMSpecsFile.IsEnabled = false;
                btnBrowseInputOverrideFile.IsEnabled = false;
                btnBrowseVMResultFile.IsEnabled = false;
                btnBrowseOverrideOutputFile.IsEnabled = false;
                btnMapnEstimate.IsEnabled = false;
                chkOverrideFile.IsEnabled = false;
                chkLoadRateCardfromFile.IsEnabled = false;

                mnuItemSaveActivityLog.IsEnabled = false;
                mnuItemGenerateVMSpecsSample.IsEnabled = false;
                mnuItemConfig.IsEnabled = false;
                mnuItemOptions.IsEnabled = false;

                btnCancelOperation.IsEnabled = true;
            }
            else if (isCancelled)
            {
                btnCancelOperation.IsEnabled = false;
            }
            else
            {
                btnFetchRateCard.IsEnabled = true;
                btnFetchAzureVMSizeList.IsEnabled = true;
                btnBrowseVMSpecsFile.IsEnabled = true;
                btnBrowseInputOverrideFile.IsEnabled = true;
                btnBrowseVMResultFile.IsEnabled = true;
                btnBrowseOverrideOutputFile.IsEnabled = true;
                btnMapnEstimate.IsEnabled = true;
                chkOverrideFile.IsEnabled = true;

                mnuItemSaveActivityLog.IsEnabled = true;
                mnuItemGenerateVMSpecsSample.IsEnabled = true;
                mnuItemConfig.IsEnabled = true;
                mnuItemOptions.IsEnabled = true;

                btnCancelOperation.IsEnabled = false;
                this.SetLoadfromFileCheckbox();
            }
        }

        /// <summary>
        /// Click event for Get Azure VM SKU button, initiates the Azure VM SKU fetch operation async using Background worker object
        /// </summary>
        /// <param name="sender">the Sender object</param>
        /// <param name="e">the Event Args object</param>
        private void btnFetchAzureVMSizeList_Click(object sender, RoutedEventArgs e)
        {
            this.ValidateandInitiateAsyncFetchAzureVMSizes();
        }

        /// <summary>
        /// Validates the inputs on the UI and initiates the Azure VM SKU fetch operation async using Background worker object
        /// </summary>
        private void ValidateandInitiateAsyncFetchAzureVMSizes()
        {
            bool isValidateSuccess = false;
            try
            {
                this.creds = SavedDataUtil.LoadData<CSPAccountCreds>(SavedDataUtil.GetConfigFileName());

                // Validate before operations
                if (this.creds == null)
                {
                    isValidateSuccess = false;
                    this.AddStatusMessage(string.Format(UIStatusMessages.FetchAzureVMSizesCredsMissing));
                }
                else
                {
                    this.azureRegionSelection = cboAzureRegion.SelectedValue.ToString();
                    isValidateSuccess = true;
                }

                // Initiate Operation
                if (isValidateSuccess)
                {
                    this.ToggleButtonStatus(true, false);
                    this.azureRegionSelectionARM = Constants.LocationAsPerARMSpecsMap[this.azureRegionSelection];
                    this.tasmUIOpInitiated = TASMUIOperations.FetchAzureVMSizeList;
                    this.AddStatusMessage(string.Format(UIStatusMessages.FetchAzureVMSizesInitiated, this.azureRegionSelection));
                    this.bworker.RunWorkerAsync();
                }
            }
            catch (Exception ex)
            {
                this.tasmUIOpInitiated = TASMUIOperations.NoOperation;
                this.AddStatusMessage(string.Format(UIStatusMessages.FetchAzureVMSizesInitiateFailed, ex.Message));
                this.ToggleButtonStatus(false, false);
            }
        }

        /// <summary>
        /// Click event for Cancal button, cancels the operation of Background worker object
        /// </summary>
        /// <param name="sender">the Sender object</param>
        /// <param name="e">the Event Args object</param>
        private void btnCancelOperation_Click(object sender, RoutedEventArgs e)
        {
            this.ToggleButtonStatus(false, true);
            this.AddStatusMessage(string.Format(UIStatusMessages.TASMUIOperationInitiateCancel));
            if (this.bworker.IsBusy)
            {
                this.bworker.CancelAsync();
            }
        }

        /// <summary>
        /// Click event for Browse button for Input Specifications file
        /// </summary>
        /// <param name="sender">the Sender object</param>
        /// <param name="e">the Event Args object</param>
        private void btnBrowseVMSpecsFile_Click(object sender, RoutedEventArgs e)
        {
            string fileName = string.Empty;
            if (this.OpenFileDialogBox(out fileName))
            {
                txtVMSpecsFile.Text = fileName;
            }
        }

        /// <summary>
        /// Opens the file open dialog box
        /// </summary>
        /// <param name="fileName">the filename that has been selected as an out parameter</param>
        /// <returns> Returns true if file has been selected, false if the dialog has been cancelled</returns>
        private bool OpenFileDialogBox(out string fileName)
        {
            bool isFileSelectionSuccess = false;
            fileName = string.Empty;
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "CSV files (*.csv)|*.csv";
                if (openFileDialog.ShowDialog() == true)
                {
                    isFileSelectionSuccess = true;
                    fileName = openFileDialog.FileName;
                }
                else
                {
                    isFileSelectionSuccess = false;
                }
            }
            catch (Exception ex)
            {
                this.AddStatusMessage(string.Format(UIStatusMessages.FileDialogFailed, ex.Message));
            }

            return isFileSelectionSuccess;
        }

        /// <summary>
        /// Opens the file save dialog box
        /// </summary>
        /// <param name="fileName">the filename that has been selected as an out parameter</param>
        /// <param name="typeofFile">Optional parameter, type of file to be saved</param>
        /// <returns> Returns true if file has been selected, false if the dialog has been cancelled</returns>
        private bool SaveFileDialogBox(out string fileName, FileType typeofFile = FileType.CSV)
        {
            bool isFileSelectionSuccess = false;
            fileName = string.Empty;
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                switch (typeofFile)
                {
                    case FileType.CSV:
                        saveFileDialog.Filter = "CSV files (*.csv)|*.csv";
                        break;
                    case FileType.Text:
                        saveFileDialog.Filter = "TXT files (*.txt)|*.txt";
                        break;
                    default:
                        break;
                }

                if (saveFileDialog.ShowDialog() == true)
                {
                    isFileSelectionSuccess = true;
                    fileName = saveFileDialog.FileName;
                }
                else
                {
                    isFileSelectionSuccess = false;
                }
            }
            catch (Exception ex)
            {
                this.AddStatusMessage(string.Format(UIStatusMessages.FileDialogFailed, ex.Message));
            }

            return isFileSelectionSuccess;
        }

        /// <summary>
        /// Click event for Browse button for Input Override file
        /// </summary>
        /// <param name="sender">the Sender object</param>
        /// <param name="e">the Event Args object</param>
        private void btnBrowseInputOverrideFile_Click(object sender, RoutedEventArgs e)
        {
            string fileName = string.Empty;
            if (this.OpenFileDialogBox(out fileName))
            {
                txtInputOverrideFile.Text = fileName;
            }
        }

        /// <summary>
        /// Click event for Browse button for mapping output file
        /// </summary>
        /// <param name="sender">the Sender object</param>
        /// <param name="e">the Event Args object</param>
        private void btnBrowseVMResultFile_Click(object sender, RoutedEventArgs e)
        {
            string fileName = string.Empty;
            if (this.SaveFileDialogBox(out fileName))
            {
                txtVMResultFile.Text = fileName;
            }
        }

        /// <summary>
        /// Click event for Browse button for Output Override file
        /// </summary>
        /// <param name="sender">the Sender object</param>
        /// <param name="e">the Event Args object</param>
        private void btnBrowseOverrideOutputFile_Click(object sender, RoutedEventArgs e)
        {
            string fileName = string.Empty;
            if (this.SaveFileDialogBox(out fileName))
            {
                txtOverrideOutputFile.Text = fileName;
            }
        }

        /// <summary>
        /// Click event for Map and Estimate button, initiates the Map and Estimate operation async using Backgroundworker object
        /// </summary>
        /// <param name="sender">the Sender object</param>
        /// <param name="e">the Event Args object</param>
        private void btnMapnEstimate_Click(object sender, RoutedEventArgs e)
        {
            this.ValidateandInitiateAsyncMapnEstimate();
        }

        /// <summary>
        /// Validates the inputs on the UI and initiates the Map and Estimate operation async using Backgroundworker object
        /// </summary>
        private void ValidateandInitiateAsyncMapnEstimate()
        {
            bool isValidateSuccess = false;
            try
            {
                // Validate before operations
                if (this.cspRateCardMeterList == null)
                {
                    isValidateSuccess = false;
                    this.AddStatusMessage(string.Format(UIStatusMessages.MapnEstimateRateCardMissing));
                }
                else if (this.azureVMSizes == null)
                {
                    isValidateSuccess = false;
                    this.AddStatusMessage(string.Format(UIStatusMessages.MapnEstimateAzureVMSizesMissing));
                }
                else if (string.IsNullOrWhiteSpace(txtVMSpecsFile.Text))
                {
                    isValidateSuccess = false;
                    this.AddStatusMessage(string.Format(UIStatusMessages.MapnEstimateVMSpecsFileMissing));
                }
                else if (string.IsNullOrWhiteSpace(txtVMResultFile.Text))
                {
                    isValidateSuccess = false;
                    this.AddStatusMessage(string.Format(UIStatusMessages.MapnEstimateOutputFileMissing));
                }
                else if (chkOverrideFile.IsChecked.Value && string.IsNullOrWhiteSpace(txtOverrideOutputFile.Text))
                {
                    isValidateSuccess = false;
                    this.AddStatusMessage(string.Format(UIStatusMessages.MapnEstimateOverrideOutputFileMissing));
                }
                else
                {
                    isValidateSuccess = true;
                }

                // Initiate Operation
                if (isValidateSuccess)
                {
                    this.ToggleButtonStatus(true, false);
                    this.vmSpecsFile = txtVMSpecsFile.Text;
                    this.vmResultFile = txtVMResultFile.Text;
                    this.isFileOverrideIncluded = chkOverrideFile.IsChecked.Value;
                    if (chkOverrideFile.IsChecked.Value)
                    {
                        this.inputOverrideFile = txtInputOverrideFile.Text;
                        this.overrideOutputFile = txtOverrideOutputFile.Text;
                    }

                    this.tasmUIOpInitiated = TASMUIOperations.MapnEstimate;
                    this.AddStatusMessage(string.Format(UIStatusMessages.MapnEstimateInitiated));
                    this.dispatcherTimer.Start();
                    this.bworker.RunWorkerAsync();
                }
            }
            catch (Exception ex)
            {
                this.tasmUIOpInitiated = TASMUIOperations.NoOperation;
                this.AddStatusMessage(string.Format(UIStatusMessages.MapnEstimateInitiateFailed, ex.Message));
                this.dispatcherTimer.Stop();
                this.ToggleButtonStatus(false, false);
            }
        }

        /// <summary>
        /// Check event for checkbox option of Override using files
        /// </summary>
        /// <param name="sender">the Sender object</param>
        /// <param name="e">the Event Args object</param>
        private void chkOverrideFile_Checked(object sender, RoutedEventArgs e)
        {
            lblOverrideFile.IsEnabled = true;
            txtInputOverrideFile.IsEnabled = true;
            btnBrowseInputOverrideFile.IsEnabled = true;

            lblOverrideOutputFile.IsEnabled = true;
            txtOverrideOutputFile.IsEnabled = true;
            btnBrowseOverrideOutputFile.IsEnabled = true;
        }

        /// <summary>
        /// Uncheck event for checkbox option of Override using files
        /// </summary>
        /// <param name="sender">the Sender object</param>
        /// <param name="e">the Event Args object</param>
        private void chkOverrideFile_Unchecked(object sender, RoutedEventArgs e)
        {
            lblOverrideFile.IsEnabled = false;
            txtInputOverrideFile.IsEnabled = false;
            btnBrowseInputOverrideFile.IsEnabled = false;

            lblOverrideOutputFile.IsEnabled = false;
            txtOverrideOutputFile.IsEnabled = false;
            btnBrowseOverrideOutputFile.IsEnabled = false;
        }

        /// <summary>
        /// Load event for the window
        /// </summary>
        /// <param name="sender">the Sender object</param>
        /// <param name="e">the Event Args object</param>
        private void TASMMain_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title = Constants.NameoftheTool;
            this.SetLoadfromFileCheckbox();
        }

        /// <summary>
        /// Enables/Disables the Load from file checkbox if saved file exists for CSP Region Currency selection made
        /// </summary>
        private void SetLoadfromFileCheckbox()
        {
            try
            {
                string rateCardFileName = FileUtil.GetCurrentDirectory() + Constants.SavedDataDir + cboCSPRegionCurrency.SelectedValue.ToString() + Constants.RateCardFileExt;
                if (FileUtil.CheckFileExists(rateCardFileName))
                {
                    chkLoadRateCardfromFile.IsEnabled = true;
                }
                else
                {
                    chkLoadRateCardfromFile.IsEnabled = false;
                    chkLoadRateCardfromFile.IsChecked = false;
                }
            }
            catch (Exception ex)
            {
                this.AddStatusMessage(string.Format(UIStatusMessages.RatecardFileCheckFailed, ex.Message));
            }
        }

        /// <summary>
        /// Selection change event for Region Currency combobox
        /// </summary>
        /// <param name="sender">the Sender object</param>
        /// <param name="e">the Event Args object</param>
        private void cboCSPRegionCurrency_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboCSPRegionCurrency.IsLoaded)
            {
                this.SetLoadfromFileCheckbox();
            }
        }

        /// <summary>
        /// Click event for the Save Activity Log menu item, saves the activity log into a file
        /// </summary>
        /// <param name="sender">the Sender object</param>
        /// <param name="e">the Event Args object</param>
        private void mnuItemSaveActivityLog_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string fileName = string.Empty;
                if (this.SaveFileDialogBox(out fileName, FileType.Text))
                {
                    FileUtil.WriteTextFile(fileName, txtOperationStatusMsg.Text);
                }
            }
            catch (Exception ex)
            {
                this.AddStatusMessage(string.Format(UIStatusMessages.ActivityLogSaveFailed, ex.Message));
            }           
        }

        /// <summary>
        /// Click event for Exit menu item, Exits the application, closes the main window
        /// </summary>
        /// <param name="sender">the Sender object</param>
        /// <param name="e">the Event Args object</param>
        private void mnuItemExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Click event for the Config menu item, Loads and displays the Config window
        /// </summary>
        /// <param name="sender">the Sender object</param>
        /// <param name="e">the Event Args object</param>
        private void mnuItemConfig_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Config configDialogBox = new Config();
                configDialogBox.Owner = this;
                configDialogBox.Left = this.Left + Constants.CustomDialogBoxLeft;
                configDialogBox.Top = this.Top + Constants.CustomDialogBoxTop;
                configDialogBox.ShowDialog();
            }
            catch (Exception ex)
            {
                this.AddStatusMessage(string.Format(UIStatusMessages.LoadConfigDialogBoxFailed, ex.Message));
            }            
        }

        /// <summary>
        /// Click event for the Options menu item, Loads and displays the Options window
        /// </summary>
        /// <param name="sender">the Sender object</param>
        /// <param name="e">the Event Args object</param>
        private void mnuItemOptions_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Options optDialogBox = new Options();
                optDialogBox.Owner = this;
                optDialogBox.Left = this.Left + Constants.CustomDialogBoxLeft;
                optDialogBox.Top = this.Top + Constants.CustomDialogBoxTop;
                optDialogBox.ShowDialog();
            }
            catch (Exception ex)
            {
                this.AddStatusMessage(string.Format(UIStatusMessages.LoadOptionsDialogBoxFailed, ex.Message));
            }
        }

        /// <summary>
        /// Click event for the Generate VM Specs Sample menu item, Saves a sample file for Input specifications
        /// </summary>
        /// <param name="sender">the Sender object</param>
        /// <param name="e">the Event Args object</param>
        private void mnuItemGenerateVMSpecsSample_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string fileName = string.Empty;
                if (this.SaveFileDialogBox(out fileName, FileType.CSV))
                {
                    TASMOptionsSelection optionSelection = this.LoadOptionSelection();
                    List<string[]> vmSpecsSample = new List<string[]>();
                    vmSpecsSample.Add(IOLoadUtil.UnloadVMSpecsSampleHeader(optionSelection.IsMemoryGB(), cboCSPRegionCurrency.SelectedValue.ToString()));
                    vmSpecsSample.Add(IOLoadUtil.UnloadVMSpecsSampleOutput(CSVFileConstants.VMSpecsSample, optionSelection.IsMemoryGB()));
                    FileUtil.WriteCSVFile(fileName, vmSpecsSample);
                }
            }
            catch (Exception ex)
            {
                this.AddStatusMessage(string.Format(UIStatusMessages.GenerateVMSpecsSampleFailed, ex.Message));
            }
        }

        /// <summary>
        /// Click event for the About menu item, Displays the About screen
        /// </summary>
        /// <param name="sender">the Sender object</param>
        /// <param name="e">the Event Args object</param>
        private void mnuItemAbout_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                About aboutBox = new About();
                aboutBox.Owner = this;
                aboutBox.Left = this.Left + Constants.CustomDialogBoxLeft;
                aboutBox.Top = this.Top + Constants.CustomDialogBoxTop;
                aboutBox.ShowDialog();
            }
            catch (Exception ex)
            {
                this.AddStatusMessage(string.Format(UIStatusMessages.LoadOptionsDialogBoxFailed, ex.Message));
            }
        }
    }
}
