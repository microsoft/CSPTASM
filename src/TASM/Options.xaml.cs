// -----------------------------------------------------------------------
// <copyright file="Options.xaml.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace TASM
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using TASM.Model;
    using TASM.Util;
    using TASMOps;

    /// <summary>
    /// Interaction logic for Options.xaml
    /// </summary>
    public partial class Options : Window
    {
        /// <summary>
        /// The object that stores the options selection data
        /// </summary>
        private TASMOptionsSelection optionSelection;

        /// <summary>
        /// Initializes a new instance of the <see cref="Options"/> class.
        /// </summary>
        public Options()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Method that runs when window is loaded, loads the values on the UI
        /// </summary>
        /// <param name="sender">the Sender object</param>
        /// <param name="e">the Event Args object</param>
        private void TASMOptions_Loaded(object sender, RoutedEventArgs e)
        {
            this.LoadOptionsValues(false);
        }

        /// <summary>
        /// Loads the Option values onto the UI controls
        /// </summary>
        /// <param name="isLoadDefaults">Whether to load from defaults or from file, loads from defaults if true</param>
        private void LoadOptionsValues(bool isLoadDefaults)
        {
            try
            {
                if (!isLoadDefaults)
                {
                    this.optionSelection = SavedDataUtil.LoadData<TASMOptionsSelection>(SavedDataUtil.GetOptionsFileName());
                }

                if (this.optionSelection == null || isLoadDefaults)
                {
                    this.optionSelection = new TASMOptionsSelection(true);
                }

                this.LoadListBox(this.lstInputVMSpecs, this.optionSelection.VMSpecsInputSequence);
                this.LoadListBox(this.lstInputOverrideSpecs, this.optionSelection.OverrideVMSpecsInputSequence);
                this.LoadListBox(this.lstWindowsKeywords, this.optionSelection.WindowsKeywordsList);
                this.LoadListBox(this.lstLinuxKeywords, this.optionSelection.LinuxKeywordsList);
                this.LoadListBox(this.lstIncludeSKUSeries, this.optionSelection.AzureVMSKUSeriesIncluded);
                this.LoadListBox(this.lstExcludeSKUSeries, this.optionSelection.AzureVMSKUSeriesExcluded);

                this.LoadComboBox(this.cboVMSpecsSkipLines, OptionsConstants.VMSpecsSkipLines, this.optionSelection.VMSpecsSkipLines);
                this.LoadComboBox(this.cboOverrideSpecsSkipLines, OptionsConstants.InputOverrideSkipLines, this.optionSelection.InputOverrideSkipLines);
                this.LoadComboBox(this.cboMappingCoefCores, OptionsConstants.MappingCoefficientsCores, this.optionSelection.MappingCoefficientCoresSelection);
                this.LoadComboBox(this.cboMappingCoefMemory, OptionsConstants.MappingCoefficientsMemory, this.optionSelection.MappingCoefficientMemorySelection);
                this.LoadComboBox(this.cboOSDiskHDDSize, OptionsConstants.OSDiskHDDSizeList, this.optionSelection.OSDiskHDDSelection);
                this.LoadComboBox(this.cboOSDiskSSDSize, OptionsConstants.OSDiskSSDSizeList, this.optionSelection.OSDiskSSDSelection);
                this.LoadComboBox(this.cboMemoryGBMB, OptionsConstants.MemoryGBMBList, this.optionSelection.MemoryGBMBSelection);
                this.LoadComboBox(this.cboHoursinaMonth, OptionsConstants.HoursinaMonthList, this.optionSelection.HoursinaMonthSelection);
            }
            catch (Exception ex)
            {
                MessageBox.Show(UIStatusMessages.OptionsLoadFailed, ex.Message);
            }
        }

        /// <summary>
        /// Loads the items into the Combobox
        /// </summary>
        /// <param name="cbox">Combobox UI object</param>
        /// <param name="items">List of items to be loaded</param>
        /// <param name="defaultSel">Item to be selected by default</param>
        /// <typeparam name="T">The class type for the items</typeparam>
        private void LoadComboBox<T>(ComboBox cbox, List<T> items, T defaultSel)
        {
            int index = 0;
            cbox.Items.Clear();
            foreach (T item in items)
            {
                cbox.Items.Add(new ComboBoxItem() { Content = item.ToString() });
                if (defaultSel.Equals(item))
                {
                    cbox.SelectedIndex = index;
                }

                index++;
            }
        }

        /// <summary>
        /// Loads the items into the Listbox from an array of items
        /// </summary>
        /// <param name="lstBox">Listbox UI object</param>
        /// <param name="arrayItems">Array of items to be loaded</param>
        /// <typeparam name="T">The class type for the items</typeparam>
        private void LoadListBox<T>(ListBox lstBox, T[] arrayItems)
        {
            lstBox.Items.Clear();
            foreach (T item in arrayItems)
            {
                lstBox.Items.Add(new ListBoxItem() { Content = item.ToString() });
            }
        }

        /// <summary>
        /// Loads the items into the Listbox from a list of items
        /// </summary>
        /// <param name="lstBox">Listbox UI object</param>
        /// <param name="arrayItems">Array of items to be loaded</param>
        /// <typeparam name="T">The class type for the items</typeparam>
        private void LoadListBox<T>(ListBox lstBox, List<T> arrayItems)
        {
            lstBox.Items.Clear();
            foreach (T item in arrayItems)
            {
                lstBox.Items.Add(new ListBoxItem() { Content = item.ToString() });
            }
        }

        /// <summary>
        /// Click event for Move up button for VM Specs, moves the selected item one level up in the listbox on UI
        /// </summary>
        /// <param name="sender">the Sender object</param>
        /// <param name="e">the Event Args object</param>
        private void btnVMSpecsMoveUp_Click(object sender, RoutedEventArgs e)
        {
            this.MoveItemUp<VMSpecsSequenceId>(this.lstInputVMSpecs, this.optionSelection.VMSpecsInputSequence);
        }

        /// <summary>
        /// Moves the selected item one level up in the listbox on UI
        /// </summary>
        /// <param name="lstBox">Listbox UI object</param>
        /// <param name="arrayItems">Array of items to be loaded</param>
        /// <typeparam name="T">The class type for the items</typeparam>
        private void MoveItemUp<T>(ListBox lstBox, T[] arrayItems)
        {
            int currentSelIndex = lstBox.SelectedIndex;
            if (lstBox.SelectedIndex > 0)
            {
                T topItem = arrayItems[lstBox.SelectedIndex - 1];
                arrayItems[lstBox.SelectedIndex - 1] = arrayItems[lstBox.SelectedIndex];
                arrayItems[lstBox.SelectedIndex] = topItem;

                this.LoadListBox<T>(lstBox, arrayItems);
                lstBox.SelectedIndex = currentSelIndex - 1;
            }
        }

        /// <summary>
        /// Moves the selected item one level down in the listbox on UI
        /// </summary>
        /// <param name="lstBox">Listbox UI object</param>
        /// <param name="arrayItems">Array of items to be loaded</param>
        /// <typeparam name="T">The class type for the items</typeparam>
        private void MoveItemDown<T>(ListBox lstBox, T[] arrayItems)
        {
            int currentSelIndex = lstBox.SelectedIndex;
            if (lstBox.Items.Count > 1 && lstBox.SelectedIndex < lstBox.Items.Count - 1)
            {
                T bottomItem = arrayItems[lstBox.SelectedIndex + 1];
                arrayItems[lstBox.SelectedIndex + 1] = arrayItems[lstBox.SelectedIndex];
                arrayItems[lstBox.SelectedIndex] = bottomItem;

                this.LoadListBox<T>(lstBox, arrayItems);
                lstBox.SelectedIndex = currentSelIndex + 1;
            }
        }

        /// <summary>
        /// Click event for Move down button for VM Specs, moves the selected item one level down in the listbox on UI
        /// </summary>
        /// <param name="sender">the Sender object</param>
        /// <param name="e">the Event Args object</param>
        private void btnVMSpecsMoveDown_Click(object sender, RoutedEventArgs e)
        {
            this.MoveItemDown<VMSpecsSequenceId>(this.lstInputVMSpecs, this.optionSelection.VMSpecsInputSequence);
        }

        /// <summary>
        /// Click event for Move up button for Override, moves the selected item one level up in the listbox on UI
        /// </summary>
        /// <param name="sender">the Sender object</param>
        /// <param name="e">the Event Args object</param>
        private void btnOverrideMoveUp_Click(object sender, RoutedEventArgs e)
        {
            this.MoveItemUp<OverrideVMSpecsSequenceId>(this.lstInputOverrideSpecs, this.optionSelection.OverrideVMSpecsInputSequence);
        }

        /// <summary>
        /// Click event for Move down button for Override, moves the selected item one level down in the listbox on UI
        /// </summary>
        /// <param name="sender">the Sender object</param>
        /// <param name="e">the Event Args object</param>
        private void btnOverrideMoveDown_Click(object sender, RoutedEventArgs e)
        {
            this.MoveItemDown<OverrideVMSpecsSequenceId>(this.lstInputOverrideSpecs, this.optionSelection.OverrideVMSpecsInputSequence);
        }

        /// <summary>
        /// Click event for OK button, saves the inputs provided on the UI
        /// </summary>
        /// <param name="sender">the Sender object</param>
        /// <param name="e">the Event Args object</param>
        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            this.PersistOptions();
            this.Close();
        }

        /// <summary>
        /// Persist inputs provided on the UI
        /// </summary>
        private void PersistOptions()
        {
            try
            {
                this.optionSelection.VMSpecsSkipLines = int.Parse(cboVMSpecsSkipLines.SelectedValue.ToString());
                this.optionSelection.InputOverrideSkipLines = int.Parse(cboOverrideSpecsSkipLines.SelectedValue.ToString());
                this.optionSelection.MappingCoefficientCoresSelection = double.Parse(cboMappingCoefCores.SelectedValue.ToString());
                this.optionSelection.MappingCoefficientMemorySelection = double.Parse(cboMappingCoefMemory.SelectedValue.ToString());
                this.optionSelection.OSDiskHDDSelection = cboOSDiskHDDSize.SelectedValue.ToString();
                this.optionSelection.OSDiskSSDSelection = cboOSDiskSSDSize.SelectedValue.ToString();
                this.optionSelection.MemoryGBMBSelection = cboMemoryGBMB.SelectedValue.ToString();
                this.optionSelection.HoursinaMonthSelection = int.Parse(cboHoursinaMonth.SelectedValue.ToString());
                SavedDataUtil.SaveData(this.optionSelection, SavedDataUtil.GetConfigandOptionsDirName(), SavedDataUtil.GetOptionsFileName());
            }
            catch (Exception ex)
            {
                MessageBox.Show(UIStatusMessages.OptionsSaveFailed, ex.Message);
            }
        }

        /// <summary>
        /// Click event for Cancel button, closes window without saving
        /// </summary>
        /// <param name="sender">the Sender object</param>
        /// <param name="e">the Event Args object</param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Click event for Add button for Windows keywords, adds the new item text to the windows keyword listbox on the UI
        /// </summary>
        /// <param name="sender">the Sender object</param>
        /// <param name="e">the Event Args object</param>
        private void btnWindowsAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtAddtoWindows.Text))
            {
                this.CheckandAddItemtoListbox(this.lstWindowsKeywords, this.optionSelection.WindowsKeywordsList, this.txtAddtoWindows.Text);
                txtAddtoWindows.Text = string.Empty;
            }
            else
            {
                MessageBox.Show(UIStatusMessages.WindowsKeywordMissing, UIStatusMessages.KeywordsMsgBoxHeader);
            }
        }

        /// <summary>
        /// Add items to the listbox on the UI, if not already present
        /// </summary>
        /// <param name="lstBox">Listbox UI object</param>
        /// <param name="itemList">List of items</param>
        /// <param name="itemtoAdd">Item to add</param>
        private void CheckandAddItemtoListbox(ListBox lstBox, List<string> itemList, string itemtoAdd)
        {
            itemtoAdd = itemtoAdd.Trim();
            if (!itemList.Contains(itemtoAdd, StringComparer.OrdinalIgnoreCase))
            {
                itemList.Add(itemtoAdd);
                this.LoadListBox(lstBox, itemList);
            }
            else
            {
                MessageBox.Show(UIStatusMessages.KeywordAlreadyPresent, UIStatusMessages.KeywordsMsgBoxHeader);
            }
        }

        /// <summary>
        /// Click event for Add button for Linux keywords, adds the new item text to the Linux keyword listbox on the UI
        /// </summary>
        /// <param name="sender">the Sender object</param>
        /// <param name="e">the Event Args object</param>
        private void btnLinuxAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtAddtoLinux.Text))
            {
                this.CheckandAddItemtoListbox(this.lstLinuxKeywords, this.optionSelection.LinuxKeywordsList, this.txtAddtoLinux.Text);
                txtAddtoLinux.Text = string.Empty;
            }
            else
            {
                MessageBox.Show(UIStatusMessages.LinuxKeywordMissing, UIStatusMessages.KeywordsMsgBoxHeader);
            }
        }

        /// <summary>
        /// Click event for Remove button for Windows keywords, Removes the selected item from the windows keyword listbox on the UI
        /// </summary>
        /// <param name="sender">the Sender object</param>
        /// <param name="e">the Event Args object</param>
        private void btnWindowsRemove_Click(object sender, RoutedEventArgs e)
        {
            this.CheckandRemoveItemfromListBox(this.lstWindowsKeywords, this.optionSelection.WindowsKeywordsList, OptionsConstants.DefaultWindowsKeywordsList);
        }

        /// <summary>
        /// Removes the selected item from the listbox on the UI
        /// </summary>
        /// <param name="lstBox">Listbox UI object</param>
        /// <param name="itemList">List of items</param>
        /// <param name="defaultList">Default list to be loaded</param>
        private void CheckandRemoveItemfromListBox(ListBox lstBox, List<string> itemList, List<string> defaultList)
        {
            if (lstBox.SelectedIndex > -1)
            {
                if (!defaultList.Contains(lstBox.SelectedValue.ToString(), StringComparer.OrdinalIgnoreCase))
                {
                    itemList.Remove(itemList.First(x => x.Equals(lstBox.SelectedValue.ToString(), StringComparison.OrdinalIgnoreCase)));
                    this.LoadListBox(lstBox, itemList);
                    if (lstBox.Items.Count > 0)
                    {
                        lstBox.SelectedIndex = lstBox.Items.Count - 1;
                    }
                }
                else
                {
                    MessageBox.Show(UIStatusMessages.KeywordisDefault, UIStatusMessages.KeywordsMsgBoxHeader);
                }
            }
        }

        /// <summary>
        /// Click event for Remove button for Linux keywords, Removes the selected item from the Linux keyword listbox on the UI
        /// </summary>
        /// <param name="sender">the Sender object</param>
        /// <param name="e">the Event Args object</param>
        private void btnLinuxRemove_Click(object sender, RoutedEventArgs e)
        {
            this.CheckandRemoveItemfromListBox(this.lstLinuxKeywords, this.optionSelection.LinuxKeywordsList, OptionsConstants.DefaultLinuxKeywordsList);
        }

        /// <summary>
        /// Click event for Remove Azure VM SKU Series button, removes the selected item from included series and adds it to the excluded series
        /// </summary>
        /// <param name="sender">the Sender object</param>
        /// <param name="e">the Event Args object</param>
        private void btnSKUSeriesRemove_Click(object sender, RoutedEventArgs e)
        {
            this.MoveSelAzureVMSeries(this.lstIncludeSKUSeries, this.optionSelection.AzureVMSKUSeriesIncluded, this.lstExcludeSKUSeries, this.optionSelection.AzureVMSKUSeriesExcluded, true);
        }

        /// <summary>
        /// Moves selected item (Azure VM SKU Series) from First listbox to Second listbox on UI
        /// </summary>
        /// <param name="lstbox1">First Listbox</param>
        /// <param name="itemList1">List of items for first listbox</param>
        /// <param name="lstbox2">Second Listbox</param>
        /// <param name="itemList2">List of items for second listbox</param>
        /// <param name="isExclude">Whether selected item is to be excluded or not</param>
        private void MoveSelAzureVMSeries(ListBox lstbox1, List<AzureVMSeriesTypes> itemList1, ListBox lstbox2, List<AzureVMSeriesTypes> itemList2, bool isExclude)
        {
            int itemSelIndex = lstbox1.SelectedIndex;

            if (isExclude && lstbox1.Items.Count == 1)
            {
                MessageBox.Show(UIStatusMessages.AtleastOneSeries, UIStatusMessages.KeywordsVMSKUSeriesHeader);
                return;
            }

            if (itemSelIndex > -1)
            {
                itemList2.Add(itemList1[itemSelIndex]);
                itemList1.RemoveAt(itemSelIndex);

                this.LoadListBox(lstbox1, itemList1);
                this.LoadListBox(lstbox2, itemList2);
                if (lstbox1.Items.Count > itemSelIndex)
                {
                    lstbox1.SelectedIndex = itemSelIndex;
                }
                else if (lstbox1.Items.Count > 0)
                {
                    lstbox1.SelectedIndex = lstbox1.Items.Count - 1;
                }
            }
        }

        /// <summary>
        /// Click event for Add Azure VM SKU Series button, Adds the selected item to included series and Removes it to the excluded series
        /// </summary>
        /// <param name="sender">the Sender object</param>
        /// <param name="e">the Event Args object</param>
        private void btnSKUSeriesAdd_Click(object sender, RoutedEventArgs e)
        {
            this.MoveSelAzureVMSeries(this.lstExcludeSKUSeries, this.optionSelection.AzureVMSKUSeriesExcluded, this.lstIncludeSKUSeries, this.optionSelection.AzureVMSKUSeriesIncluded, false);
        }

        /// <summary>
        /// Click event for Default button, resets all option fields to default values
        /// </summary>
        /// <param name="sender">the Sender object</param>
        /// <param name="e">the Event Args object</param>
        private void btnDefault_Click(object sender, RoutedEventArgs e)
        {
            this.LoadOptionsValues(true);
        }
    }
}
