using System;
using System.Globalization;
using System.Windows.Data;

namespace Bobasoft.Presentation.Converters
{
    public class InverseConverter : IValueConverter
    {
        //======================================================
        #region _Public methods_

        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return new object();

            if (value is bool)
                return !(bool) value;

            throw new NotSupportedException(string.Format("Type {0} not suported by InverseConverter", targetType.Name));
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("ConvertBack not supported by InverseConverter");
        }

        #endregion
    }
}