using System;

namespace Bobasoft
{
    public static class IoC
    {
        //======================================================
        #region _Public properties_

        public static bool IsInitialized
        {
            get { return _resolver != null; }
        }

        public static IDependencyResolver Resolver
        {
            get { return _resolver; }
        }

        #endregion

        //======================================================
        #region _Public methods_

        //[DebuggerStepThrough]
        public static void Initialize(IDependencyResolver resolver)
        {
            _resolver = resolver;
        }

        //[DebuggerStepThrough]
        public static void Register<T>(T instance) where T : class
        {
            _resolver.Register(instance);
        }

        public static void Register<T>(T instance, string name) where T : class
        {
            _resolver.Register(instance, name);
        }

        public static void Register<T>(Lifetime lifetime) where T : class
        {
            _resolver.Register<T>(lifetime);
        }

        public static void Register<T>(Lifetime lifetime, string name) where T : class
        {
            _resolver.Register<T>(lifetime, name);
        }

        //[DebuggerStepThrough]
        /// <summary>
        /// Register with <see cref="Lifetime.Singleton"/> lifetime.
        /// </summary>
        /// <param name="interfaceType"></param>
        /// <param name="instanceType"></param>
        public static void Register(Type interfaceType, Type instanceType)
        {
            _resolver.Register(interfaceType, instanceType, Lifetime.Singleton);
        }

        public static void Register(Type interfaceType, Type instanceType, Lifetime lifetime)
        {
            _resolver.Register(interfaceType, instanceType, lifetime);
        }

        public static void Register(Type interfaceType, Type instanceType, Lifetime lifetime, string name)
        {
            _resolver.Register(interfaceType, instanceType, lifetime, name);
        }

        //[DebuggerStepThrough]
        /// <summary>
        /// Registers with <see cref="Lifetime.Singleton"/> lifetime.
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <typeparam name="TInstance"></typeparam>
        public static void Register<TInterface, TInstance>()
            where TInstance : class, TInterface
        {
            _resolver.Register<TInterface, TInstance>(Lifetime.Singleton);
        }

        //[DebuggerStepThrough]
        public static void Register<TInterface, TInstance>(Lifetime lifetime)
            where TInstance : class, TInterface
        {
            _resolver.Register<TInterface, TInstance>(lifetime);
        }

        public static void Register<TInterface, TInstance>(Lifetime lifetime, string name)
            where TInstance : class, TInterface
        {
            _resolver.Register<TInterface, TInstance>(lifetime, name);
        }

        //[DebuggerStepThrough]
        public static void Inject<T>(T existing)
               where T : class
        {
            _resolver.Inject(existing);
        }

        //[DebuggerStepThrough]
        public static T Resolve<T>() where T : class
        {
            return _resolver.Resolve<T>();
        }

        public static T Resolve<T>(string name) where T : class
        {
            return _resolver.Resolve<T>(name);
        }

        //[DebuggerStepThrough]
        public static object Resolve(Type type)
        {
            return _resolver.Resolve(type);
        }

        public static object Resolve(Type type, string name)
        {
            return _resolver.Resolve(type, name);
        }

        #endregion

        //======================================================
        #region _Private, protected, internal fields_

        private static IDependencyResolver _resolver;

        #endregion
    }
}