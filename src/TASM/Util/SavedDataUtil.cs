// -----------------------------------------------------------------------
// <copyright file="SavedDataUtil.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace TASM.Util
{
    using System;

    /// <summary>
    /// Class that has static methods that encodes and decodes data
    /// </summary>
    public class SavedDataUtil
    {
        /// <summary>
        /// Gets the data from a text file in form an object
        /// </summary>
        /// <param name="filename">The filename</param>
        /// <typeparam name="T">The class type to be returned</typeparam>
        /// <returns> Returns the object from the data read from a file</returns>
        public static T LoadData<T>(string filename)
        {
            T data = default(T);
            try
            {
                if (FileUtil.CheckFileExists(filename))
                {
                    string persistData = FileUtil.ReadTextFile(filename);
                    if (!string.IsNullOrWhiteSpace(persistData))
                    {
                        data = ConvertDataUtil.GetObjectfromPersistedData<T>(persistData);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return data;
        }

        /// <summary>
        /// Writes the data to a text file
        /// </summary>
        /// <param name="data">The data to be written</param>
        /// <param name="dirName">The directory name where the file is to be written</param>
        /// <param name="fileName">The filename</param>
        /// <typeparam name="T">The class type for the data</typeparam>
        public static void SaveData<T>(T data, string dirName, string fileName)
        {
            try
            {
                string persistData = ConvertDataUtil.GetPersistDatafromObject(data);

                if (!string.IsNullOrWhiteSpace(persistData))
                {
                    FileUtil.CreateIfDirNotExist(dirName);
                    FileUtil.WriteTextFile(fileName, persistData);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the filename for configuration data
        /// </summary>
        /// <returns>Returns the Config filename</returns>
        public static string GetConfigFileName()
        {
            string configFileName = string.Empty;
            try
            {
                configFileName = GetConfigandOptionsDirName() + PersistDataFileConstants.ConfigFileName;
            }
            catch (Exception)
            {
                throw;
            }

            return configFileName;
        }

        /// <summary>
        /// Gets the filename for Options data
        /// </summary>
        /// <returns>Returns the Options filename</returns>
        public static string GetOptionsFileName()
        {
            string optionsFileName = string.Empty;
            try
            {
                optionsFileName = GetConfigandOptionsDirName() + PersistDataFileConstants.OptionsFileName;
            }
            catch (Exception)
            {
                throw;
            }

            return optionsFileName;
        }

        /// <summary>
        /// Gets the directory where Configurations and Options files are stored
        /// </summary>
        /// <returns>Returns the Options filename</returns>
        public static string GetConfigandOptionsDirName()
        {
            string dirName = string.Empty;
            try
            {
                dirName = FileUtil.GetCurrentDirectory() + PersistDataFileConstants.SavedConfigDir;
            }
            catch (Exception)
            {
                throw;
            }

            return dirName;
        }

        /// <summary>
        /// Gets the rate card from file as a string
        /// </summary>
        /// <param name="cspRegionCurrencySelection">The CSP Region and Currency selection</param>
        /// <returns>Returns the rate card</returns>
        public static string LoadRateCardfromFile(string cspRegionCurrencySelection)
        {
            string rateCardStr = string.Empty;
            try
            {
                string rateCardFileName = GetRateCardFileName(cspRegionCurrencySelection);
                if (FileUtil.CheckFileExists(rateCardFileName))
                {
                    rateCardStr = FileUtil.ReadTextFile(rateCardFileName);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return rateCardStr;
        }

        /// <summary>
        /// Save the rate card to a file
        /// </summary>
        /// <param name="cspRegionCurrencySelection">The CSP Region and Currency selection</param>
        /// <param name="rateCardStr">The rate card in form of a string</param>
        public static void SaveRateCardtoFile(string cspRegionCurrencySelection, string rateCardStr)
        {
            try
            {
                FileUtil.CreateIfDirNotExist(GetRateCardDirName());
                FileUtil.WriteTextFile(GetRateCardFileName(cspRegionCurrencySelection), rateCardStr);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Get the filename for the Rate Card file
        /// </summary>
        /// <param name="cspRegionCurrencySelection">The CSP Region and Currency selection</param>
        /// <returns>Returns the filename for the Rate Card file</returns>
        private static string GetRateCardFileName(string cspRegionCurrencySelection)
        {
            string rateCardFileName = string.Empty;
            try
            {
                rateCardFileName = GetRateCardDirName() + cspRegionCurrencySelection + Constants.RateCardFileExt;
            }
            catch (Exception)
            {
                throw;
            }

            return rateCardFileName;
        }

        /// <summary>
        /// Get the directory name for the Rate Card file
        /// </summary>
        /// <returns>Returns the directory name for the Rate Card file</returns>
        private static string GetRateCardDirName()
        {
            string dirName = string.Empty;
            try
            {
                dirName = FileUtil.GetCurrentDirectory() + Constants.SavedDataDir;
            }
            catch (Exception)
            {
                throw;
            }

            return dirName;
        }
    }
}
