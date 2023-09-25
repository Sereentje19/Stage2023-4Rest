using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Back_end.Services
{
    public static class EnumExtensions
{
    public static string GetDisplayName(this Enum enumValue)
    {
        FieldInfo fieldInfo = enumValue.GetType().GetField(enumValue.ToString());

        if (fieldInfo != null)
        {
            if (Attribute.GetCustomAttribute(fieldInfo, typeof(EnumDisplayNameAttribute)) is EnumDisplayNameAttribute attribute)
            {
                return attribute.DisplayName;
            }
        }

        return enumValue.ToString();
    }

public static Models.Type GetEnumValueFromDisplayName(string displayName)
{
    var enumType = typeof(Models.Type);
    var field = enumType.GetFields()
        .FirstOrDefault(f => f.GetCustomAttributes(typeof(EnumDisplayNameAttribute), false)
            .Any(attr => ((EnumDisplayNameAttribute)attr).DisplayName == displayName));

    if (field != null)
    {
        return (Models.Type)field.GetValue(null);
    }

    // Handle the case where the display name doesn't match any enum value
    throw new ArgumentException("Invalid display name", nameof(displayName));
}

}
}