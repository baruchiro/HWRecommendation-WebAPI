using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HW.Bot.Model
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Gender
    {
        [Description("Not defined")]
        NOT_DEFINE,
        [Description("Male")]
        MALE,
        [Description("Female")]
        FEMALE
    }
}
