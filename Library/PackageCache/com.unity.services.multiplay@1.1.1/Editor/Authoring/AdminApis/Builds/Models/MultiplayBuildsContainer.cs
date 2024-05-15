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
    /// Container image based build details.
    /// </summary>
    [Preserve]
    [DataContract(Name = "multiplay.builds.container")]
    internal class MultiplayBuildsContainer
    {
        /// <summary>
        /// Container image based build details.
        /// </summary>
        /// <param name="imageTag">The container image tag for a build within an environment.</param>
        [Preserve]
        public MultiplayBuildsContainer(string imageTag)
        {
            ImageTag = imageTag;
        }

        /// <summary>
        /// The container image tag for a build within an environment.
        /// </summary>
        [Preserve]
        [DataMember(Name = "imageTag", IsRequired = true, EmitDefaultValue = true)]
        public string ImageTag{ get; }
    
        /// <summary>
        /// Formats a MultiplayBuildsContainer into a string of key-value pairs for use as a path parameter.
        /// </summary>
        /// <returns>Returns a string representation of the key-value pairs.</returns>
        internal string SerializeAsPathParam()
        {
            var serializedModel = "";

            if (ImageTag != null)
            {
                serializedModel += "imageTag," + ImageTag;
            }
            return serializedModel;
        }

        /// <summary>
        /// Returns a MultiplayBuildsContainer as a dictionary of key-value pairs for use as a query parameter.
        /// </summary>
        /// <returns>Returns a dictionary of string key-value pairs.</returns>
        internal Dictionary<string, string> GetAsQueryParam()
        {
            var dictionary = new Dictionary<string, string>();

            if (ImageTag != null)
            {
                var imageTagStringValue = ImageTag.ToString();
                dictionary.Add("imageTag", imageTagStringValue);
            }
            
            return dictionary;
        }
    }
}
