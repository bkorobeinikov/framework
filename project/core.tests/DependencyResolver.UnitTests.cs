using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bobasoft.Core.Tests
{
    [TestClass]
    public class DependencyResolverUnitTests
    {
        [TestMethod]
        public void ResolveShouldResolveParametersWithSpecifiedDependencyName()
        {
            var resolver = new DependencyResolver();

            resolver.Register<IDummyToResolveParamter, DummyToResolveParameter2>(Lifetime.Default);
            resolver.Register<IDummyToResolveParamter, DummyToResolveParameter1>(Lifetime.Default, "depname");

            var instance = resolver.Resolve<DummyToResolveWithParameters>();

            Assert.IsInstanceOfType(instance.Param, typeof (DummyToResolveParameter1));
        }

        [TestMethod]
        public void ResolveShouldResolveParametersWithSpecifiedDependencyNameWithInstances()
        {
            var resolver = new DependencyResolver();

            resolver.Register<IDummyToResolveParamter>(new DummyToResolveParameter2());
            resolver.Register<IDummyToResolveParamter>(new DummyToResolveParameter1(), "depname");

            var instance = resolver.Resolve<DummyToResolveWithParameters>();

            Assert.IsInstanceOfType(instance.Param, typeof (DummyToResolveParameter1));
        }

        [TestMethod]
        public void ResolveShouldResolveParameters()
        {
            var resolver = new DependencyResolver();

            resolver.Register<IDummyToResolveParamter, DummyToResolveParameter1>(Lifetime.Default, "depname");
            resolver.Register<IDummyToResolveParamter, DummyToResolveParameter2>(Lifetime.Default);

            var instance = resolver.Resolve<DummyToResolveWithParametersWithoutName>();

            Assert.IsInstanceOfType(instance.Param, typeof(DummyToResolveParameter2));
        }

        [TestMethod]
        public void ResolveShouldResolveParametersWithInstances()
        {
            var resolver = new DependencyResolver();

            resolver.Register<IDummyToResolveParamter>(new DummyToResolveParameter1(), "depname");
            resolver.Register<IDummyToResolveParamter>(new DummyToResolveParameter2());

            var instance = resolver.Resolve<DummyToResolveWithParametersWithoutName>();

            Assert.IsInstanceOfType(instance.Param, typeof(DummyToResolveParameter2));
        }
    }

    public interface IDummyToResolveParamter
    {
        
    }

    public class DummyToResolveParameter1 : IDummyToResolveParamter
    {
    }

    public class DummyToResolveParameter2 : IDummyToResolveParamter
    {
    }

    public class DummyToResolveWithParameters
    {
        public IDummyToResolveParamter Param { get; set; }

        public DummyToResolveWithParameters([Dependency("depname")]IDummyToResolveParamter param)
        {
            Param = param;
        }
    }

    public class DummyToResolveWithParametersWithoutName
    {
        public IDummyToResolveParamter Param { get; set; }

        public DummyToResolveWithParametersWithoutName(IDummyToResolveParamter param)
        {
            Param = param;
        }
    }
}