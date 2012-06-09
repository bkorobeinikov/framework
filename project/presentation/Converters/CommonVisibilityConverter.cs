using System;
using System.Globalization;
#if METRO
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
#else
using System.Windows;
using System.Windows.Data;
#endif

namespace Bobasoft.Presentation.Converters
{
    public class CommonVisibilityConverter : IValueConverter
    {
        protected enum Operator
        {
            Equal,
            NotEqual,
            GreaterThan,
            LessThan,
            GreaterThanOrEqual,
            LessThanOrEqual,
        }

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
            if (targetType != typeof(Visibility))
                throw new ArgumentOutOfRangeException("targetType", "VisibilityConverter can only convert to Visibility");

            var param = parameter != null ? parameter.ToString() : null;
            if (param != null && param == "null")
                param = null;
            
            Visibility result;
            if (value == null)
                result = param == null ? Visibility.Visible : Visibility.Collapsed;
            else if (value is string)
            {
                if (param == null)
                    result = string.IsNullOrEmpty(value.ToString()) ? Visibility.Visible : Visibility.Collapsed;
                else
                    result = string.Equals(value, param) ? Visibility.Visible : Visibility.Collapsed;
            }
            else if (parameter == null)
                result = Visibility.Collapsed;
            else
            {
                var v = TypeConverterHelper.ConvertFromString(value.GetType(), parameter.ToString());
                result = value.Equals(v) ? Visibility.Visible : Visibility.Collapsed;
            }

            return !Inverse ? result : result == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }

#if WinRT
		public virtual object ConvertBack(object value, Type targetType, object parameter, string language)
#else
        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
#endif
        {
            if (!(value is Visibility))
                throw new ArgumentOutOfRangeException("value", "VisibilityConverter can only convert from Visibility");

            if (targetType == typeof(bool))
                return (Visibility)value == Visibility.Visible;

            throw new ArgumentException("VisibilityConverter can only convert to Boolean", "targetType");
        }

        #endregion
    }
}