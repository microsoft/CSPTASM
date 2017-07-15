// -----------------------------------------------------------------------
// <copyright file="AzureResourceInfo.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace TASMOps.Model
{
    /// <summary>
    /// Class that defines the Azure Resource Meter and its properties
    /// </summary>
    public class AzureResourceInfo
    {
        /// <summary>
        /// Gets or sets the Meter name of the Azure Resource Meter
        /// </summary>
        public string MeterName { get; set; }

        /// <summary>
        /// Gets or sets the Meter Category of the Azure Resource Meter
        /// </summary>
        public string MeterCategory { get; set; }

        /// <summary>
        /// Gets or sets the Meter Sub-Category of the Azure Resource Meter
        /// </summary>
        public string MeterSubCategory { get; set; }

        /// <summary>
        /// Gets or sets the Meter Region of the Azure Resource Meter
        /// </summary>
        public string MeterRegion { get; set; }
    }
}
