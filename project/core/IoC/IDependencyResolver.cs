using System;

namespace Bobasoft
{
    public interface IDependencyResolver
    {
        //======================================================
        #region _Methods_

        void Register<T>(T instance) where T : class;
        void Register<T>(T instance, string name) where T : class;

        void Register<T>(Lifetime lifetime) where T : class;
        void Register<T>(Lifetime lifetime, string name) where T : class;

        void Register(Type type, Lifetime lifetime);
        void Register(Type type, Lifetime lifetime, string name);
        void Register(Type interfaceType, Type instanceType, Lifetime lifetime);
        void Register(Type interfaceType, Type instanceType, Lifetime lifetime, string name);

        void Register<TInterface, TInstance>(Lifetime lifetime) where TInstance : class, TInterface;
        void Register<TInterface, TInstance>(Lifetime lifetime, string name) where TInstance : class, TInterface;
        void Inject<T>(T existing) where T : class;

        T Resolve<T>() where T : class;
        T Resolve<T>(string name) where T : class;

        object Resolve(Type type);
        object Resolve(Type type, string name);

        #endregion
    }
}