// -----------------------------------------------------------------------
// <copyright file="AzureVMListHelper.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace TASMOps.Util
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using TASMOps.Model;

    /// <summary>
    /// Class that has static method that filters Azure VM SKU list
    /// </summary>
    public class AzureVMListHelper
    {
        /// <summary>
        /// Gets the filtered list of Azure VM SKUs
        /// </summary>
        /// <param name="sizeList">List of Azure VM SKUs</param>
        /// <param name="azureVMSeriesTypesIncluded">List of Azure VM SKU series types to be included</param>
        /// <returns> Returns the filtered list of Azure VM SKUs</returns>
        public static List<AzureVMSizeListItem> FilterAzureVMSizeList(List<AzureVMSizeListItem> sizeList, List<AzureVMSeriesTypes> azureVMSeriesTypesIncluded)
        {
            List<AzureVMSizeListItem> filteredSizeList = null;
            try
            {
                filteredSizeList = new List<AzureVMSizeListItem>();
                foreach (AzureVMSeriesTypes type in azureVMSeriesTypesIncluded)
                {
                    filteredSizeList.AddRange(GetAzureVMSizeListForSpecifiedSeriesType(sizeList, type));
                }
            }
            catch (Exception)
            {
                throw;
            }

            return filteredSizeList;
        }

        /// <summary>
        /// Gets the list of Azure VM SKUs for the specified Azure VM SKU series type
        /// </summary>
        /// <param name="sizeList">List of Azure VM SKUs</param>
        /// <param name="type">Azure VM SKU series type to be included</param>
        /// <returns> Returns the list of Azure VM SKUs matching specified Azure VM SKU series type</returns>
        private static List<AzureVMSizeListItem> GetAzureVMSizeListForSpecifiedSeriesType(List<AzureVMSizeListItem> sizeList, AzureVMSeriesTypes type)
        {
            const bool ExcludePromoAzureVMSizes = true;
            List<AzureVMSizeListItem> filteredSizeList = null;
            try
            {
                filteredSizeList = new List<AzureVMSizeListItem>();
                Regex r = new Regex(AzureVMHelperConstants.AzureVMSeriesTypesRegex[type], RegexOptions.IgnoreCase);
                foreach (AzureVMSizeListItem item in sizeList)
                {
                    // Skip Promo VM Sizes
                    if (ExcludePromoAzureVMSizes && item.Name.ToUpper().Contains(AzureVMHelperConstants.AzureVMSizePromoString.ToUpper()))
                    {
                        continue;
                    }

                    Match m = r.Match(item.Name);
                    if (m.Success)
                    {
                        filteredSizeList.Add(item);
                    }
                }              
            }
            catch (Exception)
            {
                throw;
            }

            return filteredSizeList;
        }
    }
}
