// -----------------------------------------------------------------------
// <copyright file="FileUtil.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace TASM.Util
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Class that has static methods for file and directory related operations
    /// </summary>
    public class FileUtil
    {
        /// <summary>
        /// Check if file exists
        /// </summary>
        /// <param name="fileName">The filename</param>
        /// <returns> Returns true if file exists</returns>
        public static bool CheckFileExists(string fileName)
        {
            bool isFileExists = false;

            // Check if file exists and caller has permissions to read
            try
            {
                if (File.Exists(fileName))
                {
                    isFileExists = true;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return isFileExists;
        }

        /// <summary>
        /// Get the number of lines in a text file
        /// </summary>
        /// <param name="fileName">The filename</param>
        /// <returns> Returns the number of lines in a text file</returns>
        public static int GetNumOfLinesinFile(string fileName)
        {
            int numofLines = 0;
            try
            {
                numofLines = File.ReadLines(fileName).Count();
            }
            catch (Exception)
            {
                throw;
            }

            return numofLines;
        }

        /// <summary>
        /// Read the lines in a CSV file 
        /// </summary>
        /// <param name="fileName">The filename</param>
        /// <param name="skipLines">Number of lines to skip while reading</param>
        /// <param name="numberOfColumns">Minimum number of columns to be extracted from the CSV file</param>
        /// <returns> Returns the lines in a CSV file</returns>
        public static List<string[]> ReadCSVFile(string fileName, int skipLines, int numberOfColumns)
        {
            List<string[]> readData = null;
            try
            {
                readData = new List<string[]>();
                int currentLineNum = 0;

                using (StreamReader reader = new StreamReader(fileName))
                {
                    while (!reader.EndOfStream)
                    {
                        currentLineNum++;
                        string line = reader.ReadLine();

                        // Skip Header row
                        if (currentLineNum <= skipLines)
                        {
                            continue;
                        }

                        string[] values = null;
                        if (line != null)
                        {
                            values = line.Split(',');
                        }

                        if (values.Count() >= numberOfColumns)
                        {
                            readData.Add(values);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return readData;
        }

        /// <summary>
        /// Write the lines to a CSV file 
        /// </summary>
        /// <param name="fileName">The filename</param>
        /// <param name="writeData">Lines to be written to the file</param>
        public static void WriteCSVFile(string fileName, List<string[]> writeData)
        {
            StringBuilder csvText = new StringBuilder();
            try
            {
                foreach (string[] rowData in writeData)
                {
                    string currentLine = string.Join(",", rowData);
                    csvText.AppendLine(currentLine);
                }

                File.WriteAllText(fileName, csvText.ToString());
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Write the lines to a text file 
        /// </summary>
        /// <param name="fileName">The filename</param>
        /// <param name="writeData">Lines to be written to the file</param>
        public static void WriteTextFile(string fileName, string writeData)
        {
            try
            {
                File.WriteAllText(fileName, writeData);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Read the lines in a Text file 
        /// </summary>
        /// <param name="fileName">The filename</param>
        /// <returns> Returns the lines in a Text file</returns>
        public static string ReadTextFile(string fileName)
        {
            string readData = string.Empty;
            try
            {
                readData = File.ReadAllText(fileName);
            }
            catch (Exception)
            {
                throw;
            }

            return readData;
        }

        /// <summary>
        /// Get the current directory 
        /// </summary>
        /// <returns> Returns the current directory</returns>
        public static string GetCurrentDirectory()
        {
            string currentDir = string.Empty;
            try
            {
                currentDir = AppDomain.CurrentDomain.BaseDirectory;
            }
            catch (Exception)
            {
                throw;
            }

            return currentDir;
        }

        /// <summary>
        /// Creates a directory if it does not exist
        /// </summary>
        /// <param name="dirName">The directory to be created</param>
        public static void CreateIfDirNotExist(string dirName)
        {
            try
            {
                Directory.CreateDirectory(dirName);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
