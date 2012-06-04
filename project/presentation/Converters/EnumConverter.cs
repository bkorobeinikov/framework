using System;
using System.Collections.Generic;
using System.Linq;
#if WinRT
using System.Reflection;
using Windows.UI.Xaml.Data;
#else
using System.Windows.Data;
using System.ComponentModel;
using System.Globalization;
#endif

namespace Bobasoft.Presentation.Converters
{
    public class EnumConverter : IValueConverter
    {
        //======================================================
        #region _Public methods_

#if WinRT
		public object Convert(object value, Type targetType, object parameter, string language)
			{
            if (value == null)
                throw new ArgumentNullException();

            var param = parameter != null ? parameter.ToString() : string.Empty;

            var type = value.GetType().GetTypeInfo();
            if (!_cache.ContainsKey(type))
            {
                var fields = type.DeclaredFields.Where(f => f.IsLiteral).ToArray();
                var values = new Dictionary<long, string>(fields.Count());
                foreach (var fieldInfo in fields)
                {
                    var a = fieldInfo.GetCustomAttributes(typeof (DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;
                    values.Add(System.Convert.ToInt64(fieldInfo.GetRawConstantValue()), a != null ? a.Description : fieldInfo.GetValue(null).ToString());
                }

                _cache.Add(type, values);

                if (param == "array")
                    return values.Values;
                
                return values[System.Convert.ToInt64(value)];
            }

            if (param == "array")
                return _cache[type].Values;

            return _cache[type][System.Convert.ToInt64(value)];
        }
#else
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                throw new ArgumentNullException();

            var param = parameter != null ? parameter.ToString() : string.Empty;

            var type = value.GetType();
            if (!_cache.ContainsKey(type))
            {
                var fields = type.GetFields().Where(f => f.IsLiteral);
                var values = new Dictionary<long, string>(fields.Count());
                foreach (var fieldInfo in fields)
                {
                    var a = fieldInfo.GetCustomAttributes(typeof (DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;
                    values.Add(System.Convert.ToInt64(fieldInfo.GetRawConstantValue()), a != null ? a.Description : fieldInfo.GetValue(null).ToString());
                }

                _cache.Add(type, values);

                if (param == "array")
                    return values.Values;
                
                return values[System.Convert.ToInt64(value)];
            }

            if (param == "array")
                return _cache[type].Values;

            return _cache[type][System.Convert.ToInt64(value)];
        }
#endif

#if WinRT
		public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return null;

            var v = value.ToString();

            var fields = targetType.GetTypeInfo().DeclaredFields.Where(f => f.IsLiteral);
            foreach (var fieldInfo in fields)
            {
                var a = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;
                if (a != null && a.Description == v)
                    return fieldInfo.GetValue(null);
                if (fieldInfo.GetValue(null).ToString() == v)
                    return fieldInfo.GetValue(null);
            }

            return targetType.GetTypeInfo().IsValueType ? Activator.CreateInstance(targetType) : null;
        }
#else
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            var v = value.ToString();

            var fields = targetType.GetFields().Where(f => f.IsLiteral);
            foreach (var fieldInfo in fields)
            {
                var a = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;
                if (a != null && a.Description == v)
                    return fieldInfo.GetValue(null);
                if (fieldInfo.GetValue(null).ToString() == v)
                    return fieldInfo.GetValue(null);
            }

            return targetType.IsValueType ? Activator.CreateInstance(targetType) : null;
        }
#endif

        #endregion

        //======================================================
        #region _Private, protected, internal fields_

#if WinRT
		protected static Dictionary<TypeInfo, IDictionary<long, string>> _cache = new Dictionary<TypeInfo, IDictionary<long, string>>(3);
#else
        protected static Dictionary<Type, IDictionary<long, string>> _cache = new Dictionary<Type, IDictionary<long, string>>(3);
#endif

        #endregion
    }
}