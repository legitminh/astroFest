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
    /// A single build install within the Build Installs List response
    /// </summary>
    [Preserve]
    [DataContract(Name = "Build_List_inner_1")]
    internal class BuildListInner1
    {
        /// <summary>
        /// A single build install within the Build Installs List response
        /// </summary>
        /// <param name="fleetName">The name of a fleet for a build install.</param>
        /// <param name="completedMachines">The total number of completed machines within a fleet for a build install.</param>
        /// <param name="pendingMachines">The total number of pending machines within a fleet for a build install.</param>
        /// <param name="failures">A list of install failures for this build</param>
        /// <param name="regions">A list of build installs by region within a fleet for a build install.</param>
        /// <param name="ccdBucketID">The CCD bucket ID for a build within an environment.</param>
        /// <param name="ccd">ccd param</param>
        /// <param name="container">container param</param>
        [Preserve]
        public BuildListInner1(string fleetName, long completedMachines, long pendingMachines, List<BuildListInner1FailuresInner> failures, List<RegionsInner> regions, System.Guid ccdBucketID = default, CCDDetails ccd = default, ContainerImage container = default)
        {
            CcdBucketID = ccdBucketID;
            Ccd = ccd;
            Container = container;
            FleetName = fleetName;
            CompletedMachines = completedMachines;
            PendingMachines = pendingMachines;
            Failures = failures;
            Regions = regions;
        }

        /// <summary>
        /// The CCD bucket ID for a build within an environment.
        /// </summary>
        [Preserve]
        [DataMember(Name = "ccdBucketID", EmitDefaultValue = false)]
        public System.Guid CcdBucketID{ get; }
        
        /// <summary>
        /// Parameter ccd of BuildListInner1
        /// </summary>
        [Preserve]
        [DataMember(Name = "ccd", EmitDefaultValue = false)]
        public CCDDetails Ccd{ get; }
        
        /// <summary>
        /// Parameter container of BuildListInner1
        /// </summary>
        [Preserve]
        [DataMember(Name = "container", EmitDefaultValue = false)]
        public ContainerImage Container{ get; }
        
        /// <summary>
        /// The name of a fleet for a build install.
        /// </summary>
        [Preserve]
        [DataMember(Name = "fleetName", IsRequired = true, EmitDefaultValue = true)]
        public string FleetName{ get; }
        
        /// <summary>
        /// The total number of completed machines within a fleet for a build install.
        /// </summary>
        [Preserve]
        [DataMember(Name = "completedMachines", IsRequired = true, EmitDefaultValue = true)]
        public long CompletedMachines{ get; }
        
        /// <summary>
        /// The total number of pending machines within a fleet for a build install.
        /// </summary>
        [Preserve]
        [DataMember(Name = "pendingMachines", IsRequired = true, EmitDefaultValue = true)]
        public long PendingMachines{ get; }
        
        /// <summary>
        /// A list of install failures for this build
        /// </summary>
        [Preserve]
        [DataMember(Name = "failures", IsRequired = true, EmitDefaultValue = true)]
        public List<BuildListInner1FailuresInner> Failures{ get; }
        
        /// <summary>
        /// A list of build installs by region within a fleet for a build install.
        /// </summary>
        [Preserve]
        [DataMember(Name = "regions", IsRequired = true, EmitDefaultValue = true)]
        public List<RegionsInner> Regions{ get; }
    
        /// <summary>
        /// Formats a BuildListInner1 into a string of key-value pairs for use as a path parameter.
        /// </summary>
        /// <returns>Returns a string representation of the key-value pairs.</returns>
        internal string SerializeAsPathParam()
        {
            var serializedModel = "";

            if (CcdBucketID != null)
            {
                serializedModel += "ccdBucketID," + CcdBucketID + ",";
            }
            if (Ccd != null)
            {
                serializedModel += "ccd," + Ccd.ToString() + ",";
            }
            if (Container != null)
            {
                serializedModel += "container," + Container.ToString() + ",";
            }
            if (FleetName != null)
            {
                serializedModel += "fleetName," + FleetName + ",";
            }
            serializedModel += "completedMachines," + CompletedMachines.ToString() + ",";
            serializedModel += "pendingMachines," + PendingMachines.ToString() + ",";
            if (Failures != null)
            {
                serializedModel += "failures," + Failures.ToString() + ",";
            }
            if (Regions != null)
            {
                serializedModel += "regions," + Regions.ToString();
            }
            return serializedModel;
        }

        /// <summary>
        /// Returns a BuildListInner1 as a dictionary of key-value pairs for use as a query parameter.
        /// </summary>
        /// <returns>Returns a dictionary of string key-value pairs.</returns>
        internal Dictionary<string, string> GetAsQueryParam()
        {
            var dictionary = new Dictionary<string, string>();

            if (CcdBucketID != null)
            {
                var ccdBucketIDStringValue = CcdBucketID.ToString();
                dictionary.Add("ccdBucketID", ccdBucketIDStringValue);
            }
            
            if (FleetName != null)
            {
                var fleetNameStringValue = FleetName.ToString();
                dictionary.Add("fleetName", fleetNameStringValue);
            }
            
            var completedMachinesStringValue = CompletedMachines.ToString();
            dictionary.Add("completedMachines", completedMachinesStringValue);
            
            var pendingMachinesStringValue = PendingMachines.ToString();
            dictionary.Add("pendingMachines", pendingMachinesStringValue);
            
            return dictionary;
        }
    }
}
