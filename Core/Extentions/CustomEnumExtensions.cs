using System.ComponentModel;
using System.Reflection;

namespace Core.Extentions
{
    public static class CustomEnumExtensions
    {
        /// <summary>
        /// Description Attribute의 Description을 반환합니다.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDescription(this Enum value)
        {
            FieldInfo? fieldInfo = value.GetType().GetField(value.ToString());

            DescriptionAttribute[]? descriptionAttributes = fieldInfo!.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            if (descriptionAttributes == null || descriptionAttributes.Length == 0)
            {
                return value.ToString();
            }

            return descriptionAttributes[0].Description;
        }
    }
}
