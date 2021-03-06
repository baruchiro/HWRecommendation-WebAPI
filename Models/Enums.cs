using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Models
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
        [EnumMember] Other,
        [EnumMember] Unknown,
        [EnumMember] DRAM,
        [EnumMember] EDRAM,
        [EnumMember] VRAM,
        [EnumMember] SRAM,
        [EnumMember] RAM,
        [EnumMember] ROM,
        [EnumMember] FLASH,
        [EnumMember] EEPROM,
        [EnumMember] FEPROM,
        [EnumMember] EPROM,
        [EnumMember] CDRAM,
        [EnumMember(Value = "3DRAM")] _3DRAM,
        [EnumMember] SDRAM,
        [EnumMember] SGRAM,
        [EnumMember] RDRAM,
        [EnumMember] DDR,
        [EnumMember] DDR2,
        [EnumMember(Value = "DDR2 FB-DIMM")] DDR2_FB_DIMM,
        [EnumMember] Reserved,
        [EnumMember] DDR3,
        [EnumMember] FBD2,
        [EnumMember] DDR4,
        [EnumMember] LPDDR,
        [EnumMember] LPDDR2,
        [EnumMember] LPDDR3,
        [EnumMember] LPDDR4
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum DiskType
    {
        [EnumMember] Unknown,
        /// <summary>
        /// SSD
        /// </summary>
        [EnumMember] SSD,

        /// <summary>
        /// HDD
        /// </summary>
        [EnumMember] HDD
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum Gender
    {
        [Description("Not defined"), EnumMember]
        NOT_DEFINE,
        [Description("Male"), EnumMember]
        MALE,
        [Description("Female"), EnumMember]
        FEMALE
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ComputerType
    {
        [EnumMember]
        LAPTOP,
        [EnumMember]
        DESKTOP
    }
}