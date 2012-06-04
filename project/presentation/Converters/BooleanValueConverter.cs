using System;
using System.Globalization;
#if WinRT
using System.Reflection;
using Windows.UI.Xaml.Data;
#else
using System.Windows.Data;
#endif


namespace Bobasoft.Presentation.Converters
{
    public class BooleanValueConverter : IValueConverter
    {
        //======================================================
        #region _Public properties_

        public bool Inverse { get; set; }

        #endregion

        //======================================================
        #region _Public methods_

#if WinRT
		public virtual object Convert(object value, Type targetType, object parameter, string language)
#else
        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
#endif
        {
            if (targetType != typeof(bool) && targetType != typeof(bool?))
                throw new ArgumentOutOfRangeException("targetType", "BooleanValueConverter can only convert to bool");

            var param = parameter != null ? parameter.ToString() : null;
            if (param != null && param == "null")
                param = null;
            
            bool result;
            if (value == null)
                result = param == null;
            else if (value is string)
            {
                result = param == null ? string.IsNullOrEmpty(value.ToString()) : string.Equals(value, param);
            }
            else if (parameter == null)
                result = false;
            else
            {
                var v = TypeConverterHelper.ConvertFromString(value.GetType(), parameter.ToString());
                result = value.Equals(v);
            }

            return !Inverse ? result : !result;
        }

#if WinRT
		public virtual object ConvertBack(object value, Type targetType, object parameter, string language)
#else
        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
#endif
        {
            if (!(value is bool))
                throw new ArgumentOutOfRangeException("value", "BooleanValueConverter can only convert from bool");

            if (targetType == typeof(bool) || targetType == typeof(bool?))
                return !Inverse ? (bool)value : !(bool)value;

            if (parameter == null)
                return null;

#if WinRT
            if (typeof(Enum).GetTypeInfo().IsAssignableFrom(targetType.GetTypeInfo()))
#else
			if (typeof(Enum).IsAssignableFrom(targetType))
#endif
            {
                var v = !Inverse ? (bool)value : !(bool)value;
                return v ? TypeConverterHelper.ConvertFromString(targetType, parameter.ToString()) : null;
            }

            throw new ArgumentException("Unsupported target type", "targetType");

        }

        #endregion
    }
}