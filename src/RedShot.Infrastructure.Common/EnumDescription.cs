using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace RedShot.Infrastructure.Common
{
    public class EnumDescription<TEnum>
    {
        public EnumDescription(TEnum enumValue, string enumDescription)
        {
            EnumValue = enumValue;
            Description = enumDescription;
        }

        public TEnum EnumValue { get; private set; }

        public string Description { get; private set; }

        public static IEnumerable<EnumDescription<TEnum>> GetEnumDescriptions(Type enumType)
        {
            var array = Enum.GetValues(enumType);

            var list = new List<EnumDescription<TEnum>>(array.Length);

            foreach (var value in array)
            {
                list.Add(CreateEnumDescription((TEnum)value));
            }

            return list;
        }

        public static string GetDescriptionName(object enumValue)
        {
            var fi = enumValue.GetType().GetField(enumValue.ToString());

            if (fi != null)
            {
                var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attributes.Length > 0)
                {
                    return attributes[0].Description;
                }
            }

            return enumValue.ToString();
        }

        public override string ToString()
        {
            return Description;
        }

        private static EnumDescription<TEnum> CreateEnumDescription(TEnum value)
        {
            var field = value.GetType().GetField(value.ToString());

            if (field != null)
            {
                var attributes = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attributes.Length > 0)
                {
                    return new EnumDescription<TEnum>(value, attributes[0].Description);
                }
            }

            return new EnumDescription<TEnum>(value, value.ToString());
        }
    }
}
