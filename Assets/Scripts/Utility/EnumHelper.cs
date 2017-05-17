using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YAGTSS.Utility.Extensions;

namespace YAGTSS.Utility
{
    public static class EnumHelper
    {
        public static KeyValuePair<TEnum, string>[] GetValueNameMapping<TEnum>()
        {
            return (Enum.GetValues(typeof(TEnum)) as TEnum[])
                .Select(value => new KeyValuePair<TEnum, string>(value, Enum.GetName(typeof(TEnum), value)))
                .ToArray();
        }

        public static KeyValuePair<TEnum, string>[] GetValueNameMappingPretty<TEnum>()
        {
            return (Enum.GetValues(typeof(TEnum)) as TEnum[])
                .Select(value => new KeyValuePair<TEnum, string>(value, Enum.GetName(typeof(TEnum), value).SplitCamelCase()))
                .ToArray();
        }
    }

}
