// -----------------------------------------------------------------------
// <copyright file="ConvertDataUtil.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace TASM.Util
{
    using System;
    using System.Text;
    using Newtonsoft.Json;

    /// <summary>
    /// Class that has static methods that encodes and decodes data
    /// </summary>
    public class ConvertDataUtil
    {
        /// <summary>
        /// Encodes data from an object
        /// </summary>
        /// <param name="data">The object to be encoded</param>
        /// <returns> Returns the encoded data</returns>
        public static string GetPersistDatafromObject(object data)
        {
            string persistData = string.Empty;
            try
            {
                string serializedData = JsonConvert.SerializeObject(data);
                persistData = Convert.ToBase64String(Encoding.Unicode.GetBytes(serializedData));
            }
            catch (Exception)
            {
                throw;
            }

            return persistData;
        }

        /// <summary>
        /// Decodes data from an string
        /// </summary>
        /// <param name="persistData">The encoded object as a string</param>
        /// <typeparam name="T">The class type to be returned after decode</typeparam>
        /// <returns> Returns the decoded data</returns>
        public static T GetObjectfromPersistedData<T>(string persistData)
        {
            T obj = default(T);
            try
            {
                string serializedData = Encoding.Unicode.GetString(Convert.FromBase64String(persistData));
                obj = JsonConvert.DeserializeObject<T>(serializedData);
            }
            catch (Exception)
            {
                throw;
            }

            return obj;
        }
    }
}
