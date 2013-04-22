using System;
using System.Globalization;
#if WinRT
using System.Reflection;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
#else
using System.Windows;
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

#if WinRT
			if (param != null)
			{
				var p = param.Split('|');
				if (p.Length > 1)
					param = p[0];
			}

#endif
            
            bool result;
            if (value == null)
                result = param == null;
            else if (value is string)
            {
                result = param == null ? string.IsNullOrEmpty(value.ToString()) : string.Equals(value, param);
            }
            else if (param == null)
                result = false;
            else
            {
                var v = TypeConverterHelper.ConvertFromString(value.GetType(), param);
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

			if (targetType == typeof (bool) || targetType == typeof (bool?))
				return !Inverse ? (bool) value : !(bool) value;

			if (parameter == null)
				return null;

			var param = parameter.ToString();
#if WinRT
			// while not fix enum support
			var p = param.Split('|');
			if (p.Length == 1)
				throw new ArgumentException("Parameter value should contains parameter and type separated by '|'");
			param = p[0];
			targetType = Type.GetType(p[1]);

			if (typeof (Enum).GetTypeInfo().IsAssignableFrom(targetType.GetTypeInfo()))
#else
			if (typeof (Enum).IsAssignableFrom(targetType))
#endif
			{
			    var v = !Inverse ? (bool) value : !(bool) value;
			    return v ? TypeConverterHelper.ConvertFromString(targetType, param) : DependencyProperty.UnsetValue;
			}
			else
			{
                var v = !Inverse ? (bool)value : !(bool)value;
                return v ? TypeConverterHelper.ConvertFromString(targetType, param) : DependencyProperty.UnsetValue;
			}

			throw new ArgumentException("Unsupported target type", "targetType");
		}

    	#endregion
    }
}