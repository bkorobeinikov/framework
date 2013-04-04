using System;
using System.Collections.Generic;
using System.Linq;
#if WinRT
using System.Reflection;
using Windows.UI.Xaml.Data;
#else
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Input;

#endif

namespace Bobasoft.Presentation.Converters
{
    public class EnumConverter : IValueConverter
    {
        //======================================================
        #region _Constructors_

        static EnumConverter()
        {
            Mappings = new Dictionary<Type, Dictionary<long, string>>();
            Mappings[typeof (Key)] = new Dictionary<long, string>
                {
                    {(long) Key.PrintScreen, "Print Screen"},
                    {(long) Key.CapsLock, "Caps Lock"},
                    {(long) Key.NumLock, "Num Lock"},
                    {(long) Key.PageDown, "Page Down"},
                    {(long) Key.PageUp, "Page Up"},
                };
        }

        #endregion

        //======================================================
        #region _Public methods_

#if WinRT
		public object Convert(object value, Type targetType, object parameter, string language)
			{
            if (value == null)
                throw new ArgumentNullException();

            var param = parameter != null ? parameter.ToString() : string.Empty;

            var type = value.GetType().GetTypeInfo();
            if (!Cache.ContainsKey(type))
            {
                var fields = type.DeclaredFields.Where(f => f.IsLiteral).ToArray();
                var values = new Dictionary<long, string>(fields.Count());

                foreach (var fieldInfo in fields)
                {
                	var v = fieldInfo.GetValue(null);
                	values.Add(System.Convert.ToInt64(v), v.ToString());
                }

                Cache.Add(type, values);

                if (param == "array")
                    return values.Values;
                
                return values[System.Convert.ToInt64(value)];
            }

            if (param == "array")
                return Cache[type].Values;

            return Cache[type][System.Convert.ToInt64(value)];
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
                    var key = System.Convert.ToInt64(fieldInfo.GetRawConstantValue());
                    values[key] = GetEnumFieldName(type, key, fieldInfo);
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
			return Enum.Parse(targetType, v, true);
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

            return targetType.IsValueType ? Activator.CreateInstance(targetType) : DependencyProperty.UnsetValue;
        }
#endif

		#endregion

        //======================================================
        #region _Private, protected, internal methods_

        private static string GetEnumFieldName(Type type, long key, FieldInfo field)
        {
            var a = field.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;
            if (a != null)
                return a.Description;

            Dictionary<long, string> mappings;
            if (Mappings.TryGetValue(type, out mappings))
            {
                if (mappings.ContainsKey(key))
                    return mappings[key];
            }

            return field.Name;
        }

        #endregion

		//======================================================
        #region _Fields_

#if WinRT
		protected static Dictionary<TypeInfo, IDictionary<long, string>> Cache = new Dictionary<TypeInfo, IDictionary<long, string>>(3);
#else
        protected static Dictionary<Type, IDictionary<long, string>> _cache = new Dictionary<Type, IDictionary<long, string>>(3);
#endif

        protected static Dictionary<Type, Dictionary<long, string>> Mappings;

        #endregion
    }
}