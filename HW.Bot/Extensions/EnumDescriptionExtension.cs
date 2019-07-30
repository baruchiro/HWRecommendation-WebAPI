using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace HW.Bot.Extensions
{
    [Localizable(false)]
    internal static class EnumDescriptionExtension
    {
        public static string GetDescription<T>(this T enumerationValue)
            where T : struct
        {
            return typeof(T).GetDescription(enumerationValue.ToString());
        }

        public static string GetDescription(this Type enumerationType, string enumerationName)
        {
            if (!enumerationType.IsEnum)
            {
                throw new ArgumentException("EnumerationValue must be of Enum type", nameof(enumerationType));
            }

            //Tries to find a DescriptionAttribute for a potential friendly name
            //for the enum
            var memberInfo = enumerationType.GetMember(enumerationName);

            if (memberInfo.Length <= 0) return enumerationName;

            var attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attrs.Length > 0 ? ((DescriptionAttribute)attrs[0]).Description : enumerationName;
            //If we have no description attribute, just return the ToString of the enum
        }

        public static IEnumerable<string> GetDescriptions(this Type enumType)
        {
            return Enum.GetNames(enumType).Select(enumType.GetDescription);
        }

        public static string ToEnumDescription<T>(this int intValue)
        where T : struct
        {
            var type = typeof(T);
            if (!type.IsEnum)
            {
                throw new ArgumentException("EnumerationValue must be of Enum type", nameof(T));
            }

            if (!type.IsEnumDefined(intValue))
            {
                throw new ArgumentException($"{intValue} is not a valid ordinal of type {type}.");
            }

            var converted = (T)Enum.ToObject(typeof(T), intValue);
            return converted.GetDescription();
        }
    }
}
