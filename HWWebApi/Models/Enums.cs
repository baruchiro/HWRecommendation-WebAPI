using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HWWebApi.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Architecture
    {
        /// <summary>
        /// x32
        /// </summary>
        [EnumMember(Value = "x32")] X32,

        /// <summary>
        /// x64
        /// </summary>
        [EnumMember(Value = "x64")] X64
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum RamType
    {
        /// <summary>
        /// DDR3
        /// </summary>
        [EnumMember] DDR3,

        /// <summary>
        /// DDR4
        /// </summary>
        [EnumMember] DDR4
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum DiskType
    {
        /// <summary>
        /// SSD
        /// </summary>
        [EnumMember] SSD,

        /// <summary>
        /// HDD
        /// </summary>
        [EnumMember] HDD
    }
}