﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ComputerUpgradeStrategies.Recommendations.Disk {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class DiskRecommendations {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal DiskRecommendations() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ComputerUpgradeStrategies.Recommendations.Disk.DiskRecommendations", typeof(DiskRecommendations).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Your disk is too small, we recommend at least 480GB..
        /// </summary>
        public static string Get_More_Capacity {
            get {
                return ResourceManager.GetString("Get_More_Capacity", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to We recommend you to replace your HDD disk to SSD disk. An SSD disk is faster and more resistant to data loss and demage..
        /// </summary>
        public static string Replace_HDD_SDD {
            get {
                return ResourceManager.GetString("Replace_HDD_SDD", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to We do not know what kind of disk you have, but we recommend you to use an SSD disk. An SSD disk is faster and more resistant to data loss and demage..
        /// </summary>
        public static string Replace_Unknown_SDD {
            get {
                return ResourceManager.GetString("Replace_Unknown_SDD", resourceCulture);
            }
        }
    }
}
