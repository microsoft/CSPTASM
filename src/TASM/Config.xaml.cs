// -----------------------------------------------------------------------
// <copyright file="Config.xaml.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace TASM
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using TASM.Util;
    using TASMOps.Model;
    using TASMOps.Util;

    /// <summary>
    /// Interaction logic for Config.xaml
    /// </summary>
    public partial class Config : Window
    {
        /// <summary>
        /// The background worker object
        /// </summary>
        private BackgroundWorker bWorker = new BackgroundWorker();

        /// <summary>
        /// The object that will have information on TASM UI Configuration operation initiated
        /// </summary>
        private TASMUIConfigOperations opInitiated = TASMUIConfigOperations.NoOperation;

        /// <summary>
        /// The CSP Account credentials object 
        /// </summary>
        private CSPAccountCreds creds;

        /// <summary>
        /// A value that indicates if API test succeeded
        /// </summary>
        private bool isAPITestValid = false;

        /// <summary>
        /// The error message is API test failed
        /// </summary>
        private string apiTestFailedError = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="Config"/> class.
        /// </summary>
        public Config()
        {
            this.InitializeComponent();
            this.bWorker.DoWork += this.BWorker_DoWork;
            this.bWorker.WorkerSupportsCancellation = true;
            this.bWorker.RunWorkerCompleted += this.BWorker_RunWorkerCompleted;
        }

        /// <summary>
        /// Method that is run on Background worker run completion
        /// </summary>
        /// <param name="sender">the Sender object</param>
        /// <param name="e">the Event Args object</param>
        private void BWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                this.Close();
            }
            else if (e.Error != null)
            {
                MessageBox.Show(string.Format(UIStatusMessages.APITestCannotPerform, e.Error.Message), UIStatusMessages.APITestMessageHeaderResult);
            }
            else
            {
                switch (this.opInitiated)
                {
                    case TASMUIConfigOperations.NoOperation:

                        // Do Nothing, This function won't be called for this enum value
                        break;

                    case TASMUIConfigOperations.TestPCAPI:
                    case TASMUIConfigOperations.TestARMAPI:
                        if (this.isAPITestValid)
                        {
                            MessageBox.Show(UIStatusMessages.APITestSuccess, UIStatusMessages.APITestMessageHeaderResult);
                        }
                        else
                        {
                            MessageBox.Show(string.Format(UIStatusMessages.APITestFailed, apiTestFailedError), UIStatusMessages.APITestMessageHeaderResult);
                        }

                        break;

                    default:
                        break;
                }

                this.ToggleButtonStatus(false, this.opInitiated == TASMUIConfigOperations.TestPCAPI ? true : false);
            }
        }

        /// <summary>
        /// Method that is run on Background worker initiation
        /// </summary>
        /// <param name="sender">the Sender object</param>
        /// <param name="e">the Event Args object</param>
        private void BWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bw = sender as BackgroundWorker;

            switch (this.opInitiated)
            {
                case TASMUIConfigOperations.NoOperation:

                    // Do Nothing, This function won't be called for this enum value
                    break;

                case TASMUIConfigOperations.TestPCAPI:
                    this.DoWork_TestPCAPI(bw, e);
                    break;

                case TASMUIConfigOperations.TestARMAPI:
                    this.DoWork_TestARMAPI(bw, e);
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Method to test the credentials for the Partner Center API
        /// </summary>
        /// <param name="bw">the Background worker object</param>
        /// <param name="e">the Event Args object</param>
        private void DoWork_TestPCAPI(BackgroundWorker bw, DoWorkEventArgs e)
        {
            try
            {
                string errorMsg = string.Empty;
                this.isAPITestValid = TestAPIUtil.TestPCAPI(this.creds, out errorMsg);

                if (!this.isAPITestValid)
                {
                    this.apiTestFailedError = errorMsg;
                }

                if (this.CheckIfBWorkerCancelled(bw, e))
                {
                    return;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method to test the credentials for the ARM API
        /// </summary>
        /// <param name="bw">The Background worker object</param>
        /// <param name="e">The Event Args object</param>
        private void DoWork_TestARMAPI(BackgroundWorker bw, DoWorkEventArgs e)
        {
            try
            {
                string errorMsg = string.Empty;
                string armToken = string.Empty;
                this.isAPITestValid = TestAPIUtil.TestARMAPIToken(this.creds, out armToken, out errorMsg);

                if (this.CheckIfBWorkerCancelled(bw, e))
                {
                    return;
                }

                if (!this.isAPITestValid)
                {
                    this.apiTestFailedError = errorMsg;
                }
                else
                {
                    if (TestAPIUtil.TestARMAPI(this.creds, armToken, out errorMsg))
                    {
                        this.isAPITestValid = true;
                    }
                    else
                    {
                        this.apiTestFailedError = errorMsg;
                        this.isAPITestValid = false;
                    }
                }

                if (this.CheckIfBWorkerCancelled(bw, e))
                {
                    return;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Check if the background worker has been cancelled
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
        /// Click event for Test PCAPI Button
        /// </summary>
        /// <param name="sender">the Sender object</param>
        /// <param name="e">the Event Args object</param>
        private void btnTestPCAPI_Click(object sender, RoutedEventArgs e)
        {
            this.ValidateandInitiateAsyncPCCredentials();
        }

        /// <summary>
        /// Validates inputs on UI and initiates the test for PC API async using backgorund worker
        /// </summary>
        private void ValidateandInitiateAsyncPCCredentials()
        {
            try
            {
                bool isPCConfigFormValid = false;
                string validationMsg = string.Empty;
                string validationMsgHeader = UIStatusMessages.PCAPITestMessageHeaderValMissing;
                if (string.IsNullOrWhiteSpace(txtPartnerTenantId.Text))
                {
                    isPCConfigFormValid = false;
                    validationMsg = UIStatusMessages.PartnerTenantIdMissing;
                }
                else if (string.IsNullOrWhiteSpace(txtPCNativeAppId.Text))
                {
                    isPCConfigFormValid = false;
                    validationMsg = UIStatusMessages.PCNativeAppIdMissing;
                }
                else if (string.IsNullOrWhiteSpace(txtUsername.Text))
                {
                    isPCConfigFormValid = false;
                    validationMsg = UIStatusMessages.PCUsernameMissing;
                }
                else if (string.IsNullOrWhiteSpace(psPassword.Password))
                {
                    isPCConfigFormValid = false;
                    validationMsg = UIStatusMessages.PCPasswordMissing;
                }
                else
                {
                    isPCConfigFormValid = true;
                }

                if (!isPCConfigFormValid)
                {
                    MessageBox.Show(validationMsg, validationMsgHeader);
                }
                else
                {
                    this.creds = new CSPAccountCreds()
                    {
                        CSPPartnerTenantID = txtPartnerTenantId.Text,
                        CSPPartnerCenterAppId = txtPCNativeAppId.Text,
                        CSPAdminAgentUserName = txtUsername.Text,
                        CSPAdminAgentPassword = psPassword.Password,
                    };
                    this.ToggleButtonStatus(true, true);
                    this.opInitiated = TASMUIConfigOperations.TestPCAPI;
                    this.isAPITestValid = false;
                    this.bWorker.RunWorkerAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(UIStatusMessages.APITestUnabletoInitiate, ex.Message), UIStatusMessages.APITestMessageHeaderResult);
            }
        }

        /// <summary>
        /// Click event for Test ARMAPI Button
        /// </summary>
        /// <param name="sender">the Sender object</param>
        /// <param name="e">the Event Args object</param>
        private void btnTestARMAPI_Click(object sender, RoutedEventArgs e)
        {
            this.ValidateandInitiateAsyncARMTest();
        }

        /// <summary>
        /// Validates inputs on UI and initiates the test for ARM API async using backgorund worker
        /// </summary>
        private void ValidateandInitiateAsyncARMTest()
        {
            try
            {
                bool isARMConfigFormValid = false;
                string validationMsg = string.Empty;
                string validationMsgHeader = UIStatusMessages.PCAPITestMessageHeaderValMissing;
                if (string.IsNullOrWhiteSpace(txtUsername.Text))
                {
                    isARMConfigFormValid = false;
                    validationMsg = UIStatusMessages.PCUsernameMissing;
                }
                else if (string.IsNullOrWhiteSpace(psPassword.Password))
                {
                    isARMConfigFormValid = false;
                    validationMsg = UIStatusMessages.PCPasswordMissing;
                }
                else if (string.IsNullOrWhiteSpace(txtARMNativeAppId.Text))
                {
                    isARMConfigFormValid = false;
                    validationMsg = UIStatusMessages.PCARMAppIdMissing;
                }
                else if (string.IsNullOrWhiteSpace(txtCustomerTenantId.Text))
                {
                    isARMConfigFormValid = false;
                    validationMsg = UIStatusMessages.CustomerTenantIdMissing;
                }
                else if (string.IsNullOrWhiteSpace(txtCSPAzureSubId.Text))
                {
                    isARMConfigFormValid = false;
                    validationMsg = UIStatusMessages.CSPAzureSubTenantIdMissing;
                }
                else
                {
                    isARMConfigFormValid = true;
                }

                if (!isARMConfigFormValid)
                {
                    MessageBox.Show(validationMsg, validationMsgHeader);
                }
                else
                {
                    this.creds = new CSPAccountCreds()
                    {
                        CSPAdminAgentUserName = txtUsername.Text,
                        CSPAdminAgentPassword = psPassword.Password,
                        CSPARMNativeAppId = txtARMNativeAppId.Text,
                        CSPCustomerTenantId = txtCustomerTenantId.Text,
                        CSPAzureSubscriptionId = txtCSPAzureSubId.Text
                    };
                    this.ToggleButtonStatus(true, false);
                    this.opInitiated = TASMUIConfigOperations.TestARMAPI;
                    this.isAPITestValid = false;
                    this.bWorker.RunWorkerAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(UIStatusMessages.APITestUnabletoInitiate, ex.Message), UIStatusMessages.APITestMessageHeaderResult);
            }
        }

        /// <summary>
        /// Toggles the status of buttons
        /// </summary>
        /// <param name="isInitiated">A value indicating if the operation is initiated, this will disable the buttons if true and enable otherwise</param>
        /// <param name="isTestPCAPI">A value indicating if the operation if Test Partner Center API or not</param>
        private void ToggleButtonStatus(bool isInitiated, bool isTestPCAPI)
        {
            // Toggle Button Status
            if (isInitiated)
            {
                if (isTestPCAPI)
                {
                    btnTestPCAPI.Content = Constants.ConfigTestinProgressButtonText;
                }
                else
                {
                    btnTestARMAPI.Content = Constants.ConfigTestinProgressButtonText;
                }

                btnTestPCAPI.IsEnabled = false;
                btnTestARMAPI.IsEnabled = false;
                btnOK.IsEnabled = false;
                btnClear.IsEnabled = false;
            }
            else
            {
                if (isTestPCAPI)
                {
                    btnTestPCAPI.Content = Constants.ConfigTestButtonText;
                }
                else
                {
                    btnTestARMAPI.Content = Constants.ConfigTestButtonText;
                }

                btnTestPCAPI.IsEnabled = true;
                btnTestARMAPI.IsEnabled = true;
                btnOK.IsEnabled = true;
                btnClear.IsEnabled = true;
            }
        }

        /// <summary>
        /// Click event for Cancel Button, closes window without saving
        /// </summary>
        /// <param name="sender">the Sender object</param>
        /// <param name="e">the Event Args object</param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (this.bWorker.IsBusy)
            {
                btnCancel.Content = Constants.ConfigCancelinProgressButtonText;
                this.bWorker.CancelAsync();
            }
            else
            {
                this.Close();
            }
        }

        /// <summary>
        /// Click event for OK Button, saved the details entered and closes the window
        /// </summary>
        /// <param name="sender">the Sender object</param>
        /// <param name="e">the Event Args object</param>
        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            this.PersistConfigDetails();
            this.Close();
        }

        /// <summary>
        /// Persist the inputs provided on the UI
        /// </summary>
        private void PersistConfigDetails()
        {
            try
            {
                this.creds = new CSPAccountCreds()
                {
                    CSPPartnerTenantID = string.IsNullOrWhiteSpace(txtPartnerTenantId.Text) ? string.Empty : txtPartnerTenantId.Text.Trim(),
                    CSPPartnerCenterAppId = string.IsNullOrWhiteSpace(txtPCNativeAppId.Text) ? string.Empty : txtPCNativeAppId.Text.Trim(),
                    CSPAdminAgentUserName = string.IsNullOrWhiteSpace(txtUsername.Text) ? string.Empty : txtUsername.Text.Trim(),
                    CSPAdminAgentPassword = psPassword.Password,
                    CSPARMNativeAppId = string.IsNullOrWhiteSpace(txtARMNativeAppId.Text) ? string.Empty : txtARMNativeAppId.Text.Trim(),
                    CSPCustomerTenantId = string.IsNullOrWhiteSpace(txtCustomerTenantId.Text) ? string.Empty : txtCustomerTenantId.Text.Trim(),
                    CSPAzureSubscriptionId = string.IsNullOrWhiteSpace(txtCSPAzureSubId.Text) ? string.Empty : txtCSPAzureSubId.Text.Trim()
                };

                SavedDataUtil.SaveData<CSPAccountCreds>(this.creds, SavedDataUtil.GetConfigandOptionsDirName(), SavedDataUtil.GetConfigFileName());
            }
            catch (Exception ex)
            {
                MessageBox.Show(UIStatusMessages.ConfigSaveFailed, ex.Message);
            }
        }

        /// <summary>
        /// Load event for the window
        /// </summary>
        /// <param name="sender">the Sender object</param>
        /// <param name="e">the Event Args object</param>
        private void ConfigWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.LoadConfigDetails();
        }

        /// <summary>
        /// Loads the persisted info to the UI
        /// </summary>
        private void LoadConfigDetails()
        {
            try
            {
                CSPAccountCreds creds = SavedDataUtil.LoadData<CSPAccountCreds>(SavedDataUtil.GetConfigFileName());
                if (creds != null)
                {
                    txtPartnerTenantId.Text = this.GetEmptyStringifNull(creds.CSPPartnerTenantID);
                    txtPCNativeAppId.Text = this.GetEmptyStringifNull(creds.CSPPartnerCenterAppId);
                    txtUsername.Text = this.GetEmptyStringifNull(creds.CSPAdminAgentUserName);
                    psPassword.Password = this.GetEmptyStringifNull(creds.CSPAdminAgentPassword);
                    txtARMNativeAppId.Text = this.GetEmptyStringifNull(creds.CSPARMNativeAppId);
                    txtCustomerTenantId.Text = this.GetEmptyStringifNull(creds.CSPCustomerTenantId);
                    txtCSPAzureSubId.Text = this.GetEmptyStringifNull(creds.CSPAzureSubscriptionId);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(UIStatusMessages.ConfigLoadFailed, ex.Message);
            }
        }

        /// <summary>
        /// Gets an empty string if null string is passed
        /// </summary>
        /// <param name="str">string to be checked</param>
        /// <returns> Returns a string, empty if null is passed</returns>
        private string GetEmptyStringifNull(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                return str;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Click event for Clear button, clears the inputs provided on the UI
        /// </summary>
        /// <param name="sender">the Sender object</param>
        /// <param name="e">the Event Args object</param>
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtPartnerTenantId.Text = string.Empty;
            txtPCNativeAppId.Text = string.Empty;
            txtUsername.Text = string.Empty;
            psPassword.Password = string.Empty;
            txtARMNativeAppId.Text = string.Empty;
            txtCustomerTenantId.Text = string.Empty;
            txtCSPAzureSubId.Text = string.Empty;
        }
    }
}
