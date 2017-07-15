// -----------------------------------------------------------------------
// <copyright file="AzureManagedDisk.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace TASMOps.Model
{
    /// <summary>
    /// Class that defines the Azure managed disk and its properties
    /// </summary>
    public class AzureManagedDisk
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AzureManagedDisk"/> class
        /// </summary>
        /// <param name="diskDisplayName">Display name of the managed disk</param>
        /// <param name="diskMeterName">Meter name of the managed disk as it appears in the Rate card</param>
        /// <param name="diskSizeInGB">Size of the managed disk in GB</param>
        public AzureManagedDisk(string diskDisplayName, string diskMeterName, int diskSizeInGB)
        {
            this.DiskDisplayName = diskDisplayName;
            this.DiskMeterName = diskMeterName;
            this.DiskSizeInGB = diskSizeInGB;
        }

        /// <summary>
        /// Gets the Display name of the managed disk
        /// </summary>
        public string DiskDisplayName { get; }

        /// <summary>
        /// Gets the Meter name of the managed disk as it appears in the Rate card
        /// </summary>
        public string DiskMeterName { get; }

        /// <summary>
        /// Gets the Size of the managed disk in GB
        /// </summary>
        public int DiskSizeInGB { get; }
    }
}
