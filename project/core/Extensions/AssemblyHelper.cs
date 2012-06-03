using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#if SILVERLIGHT
using System.Windows;
#else

#endif

namespace Bobasoft
{
    public static class AssemblyHelper
    {
        //======================================================
        #region _Public properties_

#if !PORTABLE
        /// <summary>
        /// Gets all loaded assemblies in current domain
        /// </summary>
        public static ICollection<Assembly> Assemblies
        {
            get { return _assemblies ?? (_assemblies = FindAllAssemblies(null)); }
        }
#endif

        #endregion

        //======================================================
        #region _Public methods_

#if !PORTABLE
        public static IEnumerable<Assembly> FindAssemblies(IEnumerable<string> assembliesName)
        {
            return FindAllAssemblies(assembliesName);
        }
#endif

        public static IEnumerable<Type> FindTypesByInterface<TInterface>(this Assembly assembly)
        {
            return FindTypesByInterface(assembly, typeof (TInterface));
        }

        public static IEnumerable<Type> FindTypesByInterface(this Assembly assembly, Type interfaceType)
        {
#if !PORTABLE
            return assembly.GetTypes().Where(type => type.GetInterface(interfaceType.Name, false) != null);
#else
            return assembly.GetTypes().Where(type => type.GetInterfaces().Contains(interfaceType));
#endif
        }

#if !PORTABLE
        public static IEnumerable<Type> FindAllTypesByInterface<TInterface>(string[] assembliesName = null)
        {
            return FindAllTypesByInterface(typeof (TInterface), assembliesName);
        }

        public static IEnumerable<Type> FindAllTypesByInterface(Type interfaceType, string[] assembliesName = null)
        {
            var assemblies = FindAllAssemblies(assembliesName);
            return assemblies.SelectMany(assembly => assembly.FindTypesByInterface(interfaceType));
        }
#endif

        public static IEnumerable<Type> FindTypesByBaseType(this Assembly assembly, Type baseType)
        {
            return assembly.GetTypes().Where(type => type.IsSubclassOf(baseType));
        }

#if !PORTABLE
        public static IEnumerable<Type> FindAllTypesByBaseType(Type baseType, string[] assembliesName = null)
        {
            var assemblies = FindAllAssemblies(assembliesName);
            return assemblies.SelectMany(assembly => assembly.FindTypesByBaseType(baseType));
        }
#endif

        public static bool HasAttribute<TAttribute>(this Type type) where TAttribute : Attribute
        {
            return type.GetCustomAttributes(typeof (TAttribute), true).Length > 0;
        }

        public static bool HasAttribute<TAttribute>(this PropertyInfo property) where TAttribute : Attribute
        {
            return property.GetCustomAttributes(typeof(TAttribute), true).Length > 0;
        }

        public static TAttribute GetAttribute<TAttribute>(this Type type) where TAttribute : Attribute
        {
            return type.GetCustomAttributes(typeof (TAttribute), true).Cast<TAttribute>().FirstOrDefault();
        }

        public static TAttribute GetAttribute<TAttribute>(this PropertyInfo property) where TAttribute : Attribute
        {
            return property.GetCustomAttributes(typeof(TAttribute), true).Cast<TAttribute>().FirstOrDefault();
        }

        #endregion

        //======================================================
        #region _Private, protected, internal methods_

#if SILVERLIGHT
        private static ICollection<Assembly> FindAllAssemblies(IEnumerable<string> assembliesName)
        {
            var assemblies = new List<Assembly>();
            foreach (var ap in Deployment.Current.Parts)
            {
                if (assembliesName == null || assembliesName.Contains(ap.Source.Replace(".dll", "")))
                {
                    var sri = Application.GetResourceStream(new Uri(ap.Source, UriKind.Relative));
                    assemblies.Add(new AssemblyPart().Load(sri.Stream));
                }
            }

            return assemblies;
        }
#elif !PORTABLE
        private static ICollection<Assembly> FindAllAssemblies(IEnumerable<string> assembliesName)
        {
            if (assembliesName != null)
            {
                var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
                var referencedAssemlies = assembliesName.Select(n => new AssemblyName(n));

                var toLoad = referencedAssemlies.Where(r => !loadedAssemblies.Any(la => la.GetName().Name != r.Name)).ToList();
                toLoad.ForEach(path => loadedAssemblies.Add(AppDomain.CurrentDomain.Load(path.FullName)));
            }

            if (assembliesName != null)
                return AppDomain.CurrentDomain.GetAssemblies().Where(a => assembliesName.Contains(a.GetName().Name)).ToArray();
            return AppDomain.CurrentDomain.GetAssemblies();
        }
#endif

        #endregion

        //======================================================
        #region _Private, protected, internal fields_

        private static ICollection<Assembly> _assemblies;

        #endregion
    }
}