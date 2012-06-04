using System;
using System.Collections.Generic;
using System.Linq;
#if WinRT
using System.Reflection;
#endif

namespace Bobasoft
{
    public class DependencyResolver : IDependencyResolver
    {
        //======================================================
        #region _Public methods_

        public void Inject<T>(T existing) where T : class
        {
            lock (_syncLock)
            {
                _resolved.Add(new DependencyKey(typeof(T)), existing);
            }
        }

        public void Register<T>(T instance) where T : class
        {
            lock (_syncLock)
            {
                _resolved.Add(new DependencyKey(typeof(T)), instance);
            }
        }

        public void Register<T>(T instance, string name) where T : class
        {
            lock (_syncLock)
            {
                _resolved.Add(new DependencyKey(typeof(T), name), instance);
            }
        }

        public void Register<T>(Lifetime lifetime) where T : class
        {
            Register(typeof(T), lifetime);
        }

        public void Register<T>(Lifetime lifetime, string name) where T : class
        {
            Register(typeof(T), lifetime, name);
        }

        public void Register(Type type, Lifetime lifetime)
        {
            Register(type, type, lifetime);
        }

        public void Register(Type type, Lifetime lifetime, string name)
        {
            Register(type, type, lifetime, name);
        }

        public void Register<TInterface, TInstance>(Lifetime lifetime) where TInstance : class, TInterface
        {
            Register(typeof(TInterface), typeof(TInstance), lifetime);
        }

        public void Register<TInterface, TInstance>(Lifetime lifetime, string name) where TInstance : class, TInterface
        {
            Register(typeof(TInterface), typeof(TInstance), lifetime, name);
        }

        public void Register(Type interfaceType, Type instanceType, Lifetime lifetime)
        {
            lock (_syncLock)
            {
                _registered.Add(new DependencyKey(interfaceType), new DependencyValue(instanceType, lifetime));
            }
        }

        public void Register(Type interfaceType, Type instanceType, Lifetime lifetime, string name)
        {
            lock (_syncLock)
            {
                _registered.Add(new DependencyKey(interfaceType, name), new DependencyValue(instanceType, lifetime));
            }
        }

        public T Resolve<T>() where T : class
        {
            return (T)Resolve(typeof(T), null);
        }

        public T Resolve<T>(string name) where T : class
        {
            return (T) Resolve(typeof (T), name);
        }

        public object Resolve(Type type)
        {
            return Resolve(type, null);
        }

        public object Resolve(Type type, string name)
        {
            lock (_syncLock)
            {
                var key = new DependencyKey(type, name);
                if (!_resolved.ContainsKey(key))
                {
                    /*if (!_registered.ContainsKey(key))
                    {
                        throw new Exception("Type not registered.");
                    }*/
                    DependencyValue resolveTo;
                    if (!_registered.TryGetValue(key, out resolveTo))
                        resolveTo = new DependencyValue(type);

                    // TODO: create caching
#if WinRT
                	var constructorInfos = resolveTo.Type.GetTypeInfo().DeclaredConstructors.ToArray();
#else
                    var constructorInfos = resolveTo.Type.GetConstructors();
#endif
                    if (constructorInfos.Length > 1)
                        throw new Exception("Cannot resolve a type that has more than one constructor.");
                    var constructor = constructorInfos[0];
                    var parameterInfos = constructor.GetParameters();
                    if (parameterInfos.Length == 0)
                    {
                        var instance = constructor.Invoke(_emptyArguments);
                        if (resolveTo.Lifetime == Lifetime.Singleton)
                            _resolved[key] = instance;

                        return instance;
                    }
                    else
                    {
                        var parameters = new object[parameterInfos.Length];
                        foreach (var parameterInfo in parameterInfos)
                        {
#if WinRT
							var depattr = parameterInfo.GetCustomAttributes(typeof(DependencyAttribute), true).ToArray();
#else
							var depattr = parameterInfo.GetCustomAttributes(typeof (DependencyAttribute), true);
#endif
                            if (depattr.Length > 0)
                                parameters[parameterInfo.Position] = Resolve(parameterInfo.ParameterType, ((DependencyAttribute)depattr[0]).Name);
                            else
                                parameters[parameterInfo.Position] = Resolve(parameterInfo.ParameterType);
                        }

                        var instance = constructor.Invoke(parameters);
                        if (resolveTo.Lifetime == Lifetime.Singleton)
                            _resolved[key] = instance;

                        return instance;
                    }
                }

                return _resolved[key];
            }
        }

        #endregion

        //======================================================
        #region _Private, protected, internal fields_

        private readonly Dictionary<DependencyKey, DependencyValue> _registered = new Dictionary<DependencyKey, DependencyValue>(); 
        private readonly Dictionary<DependencyKey, object> _resolved = new Dictionary<DependencyKey, object>();

        private readonly object[] _emptyArguments = new object[0];
        private readonly object _syncLock = new object();

        #endregion
    }

    internal class DependencyKey
    {
        //======================================================
        #region _Constructors_

        public DependencyKey(Type type)
        {
            Type = type;
            Name = type.Name;
        }

        public DependencyKey (Type type, string name)
        {
            Type = type;
            Name = name ?? type.Name;
        }

        #endregion

        //======================================================
        #region _Public properties_

        public Type Type { get; set; }
        public string Name { get; set; }

        #endregion

        //======================================================
        #region _Public methods_

        public bool Equals(DependencyKey other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return other.Type == Type && other.Name == Name;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != typeof (DependencyKey))
                return false;
            return Equals((DependencyKey) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Type != null ? Type.GetHashCode() : 0)*397) ^ (Name != null ? Name.GetHashCode() : 0);
            }
        }

        #endregion
    }

    internal class DependencyValue
    {
        //======================================================
        #region _Constructors_

        public DependencyValue(Type type)
        {
            Type = type;
        }

        public DependencyValue(Type type, Lifetime lifetime)
        {
            Type = type;
            Lifetime = lifetime;
        }

        #endregion

        //======================================================
        #region _Public properties_

        public Type Type { get; set; }
        public Lifetime Lifetime { get; set; }

        #endregion
    }
}