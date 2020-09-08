using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace RedShot.Infrastructure.Common
{
    /// <summary>
    /// Enum description.
    /// Works with Description attribute.
    /// </summary>
    public class EnumDescription<TEnum>
         where TEnum : Enum
    {
        /// <summary>
        /// Initializes EnumDescription object. 
        /// </summary>
        public EnumDescription(TEnum enumValue, string enumDescription)
        {
            EnumValue = enumValue;
            Description = enumDescription;
        }

        /// <summary>
        /// Enum value.
        /// </summary>
        public TEnum EnumValue { get; }

        /// <summary>
        /// Description of the enum value.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Get enum descriptions via enum type.
        /// </summary>
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

        /// <summary>
        /// Gives description name via enum value.
        /// </summary>
        public static string GetDescriptionName(Enum enumValue)
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

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
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
