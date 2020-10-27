using System;
using System.Linq;
using Eto.Forms;
using Saritasa.Tools.Common.Utils;

namespace RedShot.Infrastructure.Common.Forms
{
    public static class EnumDescriptionHelper
    {
        public static BindableBinding<ListControl, T> BindWithEnum<T>(this ComboBox comboBox)
            where T : struct, Enum
        {
            var enumsDescriptions = EnumUtils.GetNamesWithDescriptions<T>();
            comboBox.DataStore = enumsDescriptions.Values;
            return comboBox.SelectedValueBinding.Convert(
                value =>
                {
                    if (string.IsNullOrWhiteSpace((string)value))
                    {
                        return Enum.Parse<T>(enumsDescriptions.Keys.FirstOrDefault());
                    }
                    else
                    {
                        var enumName = enumsDescriptions.First((kv) => kv.Value == (string)value).Key;
                        return Enum.Parse<T>(enumName);
                    }
                },
                enumValue =>
                {
                    return EnumUtils.GetDescription(enumValue);
                });
        }
    }
}
