using System;
using System.Globalization;
using System.Windows.Data;

namespace Bobasoft.Presentation.Converters
{
    public class DateTimeConverter : IValueConverter
    {
        /* 
        String.Format("{0:y yy yyy yyyy}", dt);  // "8 08 008 2008"   year
        String.Format("{0:M MM MMM MMMM}", dt);  // "3 03 Mar March"  month
        String.Format("{0:d dd ddd dddd}", dt);  // "9 09 Sun Sunday" day
        String.Format("{0:h hh H HH}",     dt);  // "4 04 16 16"      hour 12/24
        String.Format("{0:m mm}",          dt);  // "5 05"            minute
        String.Format("{0:s ss}",          dt);  // "7 07"            second
        String.Format("{0:f ff fff ffff}", dt);  // "1 12 123 1230"   sec.fraction
        String.Format("{0:F FF FFF FFFF}", dt);  // "1 12 123 123"    without zeroes
        String.Format("{0:t tt}",          dt);  // "P PM"            A.M. or P.M.
        String.Format("{0:z zz zzz}",      dt);  // "-6 -06 -06:00"   time zone

        // date separator in german culture is "." (so "/" changes to ".")
        String.Format("{0:d/M/yyyy HH:mm:ss}", dt); // "9/3/2008 16:05:07" - english (en-US)
        String.Format("{0:d/M/yyyy HH:mm:ss}", dt); // "9.3.2008 16:05:07" - german (de-DE)


        // month/day numbers without/with leading zeroes
        String.Format("{0:M/d/yyyy}", dt);            // "3/9/2008"
        String.Format("{0:MM/dd/yyyy}", dt);          // "03/09/2008"

        // day/month names
        String.Format("{0:ddd, MMM d, yyyy}", dt);    // "Sun, Mar 9, 2008"
        String.Format("{0:dddd, MMMM d, yyyy}", dt);  // "Sunday, March 9, 2008"

        // two/four digit year
        String.Format("{0:MM/dd/yy}", dt);            // "03/09/08"
        String.Format("{0:MM/dd/yyyy}", dt);          // "03/09/2008"
        */


        public enum DateTimeFormat
        {
            General = 0,            // output: MM/dd/yyyy           -- "03/09/2008"
            Date = 5,               // output: MMM d, yyyy          -- "Mar 9, 2008"
            //DateShort = 7,          // output: ddd, MMM d, yyyy     -- "Sun, Mar 9, 2008"
            //DateLong = 8,
            Time = 10,              // output: HH:mm                -- 16:05
            //TimeShort = 12,
            //TimeLong = 13,
            DateTime = 15,          // output: {0:date} {0:time}        // string format {0} - date; {1} - time;; 
            //DateTimeShort = 16,     // string format {0} - date; {1} - time;; 
            //DateTimeLong = 17,      // string format {0} - date; {1} - time;; 
        }

        //======================================================
        #region _Public methods_

        /// <summary>
        /// Modifies the source data before passing it to the target for display in the UI.
        /// </summary>
        /// <returns>
        /// The value to be passed to the target dependency property.
        /// </returns>
        /// <param name="value">The source data being passed to the target.
        /// </param><param name="targetType">The <see cref="T:System.Type"/> of data expected by the target dependency property.</param><param name="parameter">An optional parameter to be used in the converter logic.</param><param name="culture">The culture of the conversion.</param>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is DateTime))
            {
#if DEBUG
                return value;
#else
                throw new NotSupportedException(string.Format("{0} support only DateTime value to convert.", GetType().Name));
#endif
            }

            var format = DateTimeFormat.General;
            string strFormat = null;
            if (parameter != null)
            {
                var param = parameter.ToString();
                if (!string.IsNullOrEmpty(param))
                {
                    int length = param.IndexOf(':');
                    if (length == -1)
                        format = TypeConverterHelper.ConvertFromString<DateTimeFormat>(param);
                    else
                    {
                        var f = param.Substring(0, length);
                        format = TypeConverterHelper.ConvertFromString<DateTimeFormat>(f);
                        strFormat = param.Substring(length+1, param.Length-(length+1));
                    }
                }
            }

            var d = (DateTime)value;

            strFormat = strFormat == null ? "{0}" : strFormat.Replace("\\n", "\n");

            string date = null;
            if (format == DateTimeFormat.General)
                return string.Format("{0:MM/dd/yyyy}", d);
            if (format == DateTimeFormat.Date || format == DateTimeFormat.DateTime)
                date = string.Format("{0:MMM d, yyyy}", d);

            string time = null;
            if (format == DateTimeFormat.Time)
                return string.Format("{0:HH:mm}", d);
            if (format == DateTimeFormat.DateTime)
                time = string.Format("{0:HH:mm}", d);

            return string.Format(strFormat, date, time);
        }

        /// <summary>
        /// Modifies the target data before passing it to the source object.  This method is called only in <see cref="F:System.Windows.Data.BindingMode.TwoWay"/> bindings.
        /// </summary>
        /// <returns>
        /// The value to be passed to the source object.
        /// </returns>
        /// <param name="value">The target data being passed to the source.</param><param name="targetType">The <see cref="T:System.Type"/> of data expected by the source object.</param><param name="parameter">An optional parameter to be used in the converter logic.</param><param name="culture">The culture of the conversion.</param>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}