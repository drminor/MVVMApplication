using DRM.PropBag.ControlModel;
using DRM.TypeSafePropertyBag;
using System;
using System.Windows.Data;
using System.Windows.Markup;

namespace MVVMApplication
{
    public sealed class SelectedItemConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            TwoTypes tt = GetFromParam(parameter);
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            TwoTypes tt = GetFromParam(parameter);

            if (value != null && string.Equals("{NewItemPlaceholder}", value.ToString(), StringComparison.Ordinal))
            {
                return null;
            }

            return value;
        }

        private static TwoTypes GetFromParam(object parameter)
        {
            if (parameter == null)
            {
                return TwoTypes.Empty;
            }

            if (parameter is TwoTypes)
            {
                return (TwoTypes)parameter;
            }

            // Assume string (TODO: Check This.
            Type destinationType = typeof(string);

            if (parameter is Type)
            {
                return new TwoTypes((Type)parameter, destinationType);
            }
            else
            {
                if (parameter is IPropGen)
                {
                    return new TwoTypes(((IPropGen)parameter).Type, destinationType);
                }
            }
            return TwoTypes.Empty;
        }


    }

}
