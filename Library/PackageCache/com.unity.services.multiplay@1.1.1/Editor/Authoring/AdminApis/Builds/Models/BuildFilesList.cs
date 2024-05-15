//-----------------------------------------------------------------------------
// <auto-generated>
//     This file was generated by the C# SDK Code Generator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//-----------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Scripting;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Unity.Services.Multiplay.Authoring.Editor.AdminApis.Builds.Http;



namespace Unity.Services.Multiplay.Authoring.Editor.AdminApis.Builds.Models
{
    /// <summary>
    /// List of build files for an given build ID.
    /// </summary>
    [Preserve]
    [DataContract(Name = "Build_Files_List")]
    internal class BuildFilesList
    {
        /// <summary>
        /// List of build files for an given build ID.
        /// </summary>
        /// <param name="results">results param</param>
        /// <param name="offset">Offset used for pagination</param>
        /// <param name="limit">Limit used for pagination</param>
        [Preserve]
        public BuildFilesList(List<BuildFilesListResultsInner> results, int offset = default, int limit = default)
        {
            Offset = offset;
            Limit = limit;
            Results = results;
        }

        /// <summary>
        /// Offset used for pagination
        /// </summary>
        [Preserve]
        [DataMember(Name = "offset", EmitDefaultValue = false)]
        public int Offset{ get; }
        
        /// <summary>
        /// Limit used for pagination
        /// </summary>
        [Preserve]
        [DataMember(Name = "limit", EmitDefaultValue = false)]
        public int Limit{ get; }
        
        /// <summary>
        /// Parameter results of BuildFilesList
        /// </summary>
        [Preserve]
        [DataMember(Name = "results", IsRequired = true, EmitDefaultValue = true)]
        public List<BuildFilesListResultsInner> Results{ get; }
    
        /// <summary>
        /// Formats a BuildFilesList into a string of key-value pairs for use as a path parameter.
        /// </summary>
        /// <returns>Returns a string representation of the key-value pairs.</returns>
        internal string SerializeAsPathParam()
        {
            var serializedModel = "";

            serializedModel += "offset," + Offset.ToString() + ",";
            serializedModel += "limit," + Limit.ToString() + ",";
            if (Results != null)
            {
                serializedModel += "results," + Results.ToString();
            }
            return serializedModel;
        }

        /// <summary>
        /// Returns a BuildFilesList as a dictionary of key-value pairs for use as a query parameter.
        /// </summary>
        /// <returns>Returns a dictionary of string key-value pairs.</returns>
        internal Dictionary<string, string> GetAsQueryParam()
        {
            var dictionary = new Dictionary<string, string>();

            var offsetStringValue = Offset.ToString();
            dictionary.Add("offset", offsetStringValue);
            
            var limitStringValue = Limit.ToString();
            dictionary.Add("limit", limitStringValue);
            
            return dictionary;
        }
    }
}
