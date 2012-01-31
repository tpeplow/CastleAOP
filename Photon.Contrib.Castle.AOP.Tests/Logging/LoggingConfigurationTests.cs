namespace Photon.Contrib.Castle.AOP.Tests.Logging
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Photon.Contrib.Castle.AOP.Logging;

    using global::Castle.MicroKernel.Registration;
    using global::Castle.Windsor;

    [TestClass]
    public class LoggingConfigurationTests
    {
        [TestInitialize]
        public void Init()
        {
            LoggingConfiguration.Reset();
        }

        [TestMethod]
        public void Can_Confiure_By_Extenstion_When_Registering_Using_ComponentFor()
        {
            var container = new WindsorContainer();
            container.Register(
                Component.For<ITestService>()
                    .ImplementedBy<TestServiceImpl>().IgnoreLogging());

            Assert.IsFalse(LoggingConfiguration.ShouldLogForType(typeof(ITestService)));
        }

        [TestMethod]
        public void Can_Configure_By_Extenstion_When_Registering_Using_AllTypesFromAssembly()
        {
            var container = new WindsorContainer();
            container.Register(
                AllTypes.FromAssembly(typeof(TestServiceImpl).Assembly)
                    .Where(t => t == typeof(TestServiceImpl))
                    .WithService.FirstInterface()
                    .Configure(c => c.IgnoreLogging()));

            Assert.IsFalse(LoggingConfiguration.ShouldLogForType(typeof(ITestService)));
        }

        [TestMethod]
        public void Can_Restrict_To_Types_Only_In_Namespace()
        {
            LoggingConfiguration.ConfigureWith().RestrictedLoggingToTypesInNamespace("Photon.*");

            Assert.IsTrue(LoggingConfiguration.ShouldLogForType(typeof(ITestService)));
            Assert.IsFalse(LoggingConfiguration.ShouldLogForType(typeof(string)));
        }

        [TestMethod]
        public void Can_Configure_LoggingOptions_From_AppConfig()
        {
            LoggingConfiguration.ConfigureWith().LoggingOptionsFromAppConfig();

            AssertLoggingOptionsAreAllTrue();
        }

        [TestMethod]
        public void Can_Configure_LoggingOptions_In_Code()
        {
            LoggingConfiguration.ConfigureWith().LoggingOptions(o =>
            {
                o.Enabled = true;
                o.LogParameterValues = true;
                o.LogReturnValue = true;
            });

            AssertLoggingOptionsAreAllTrue();
        }

        [TestMethod]
        public void Will_Ignore_Types_Which_Derive_From_Service()
        {
            LoggingConfiguration.ConfigureWith().DoNotLogForService(typeof(object));
            Assert.IsFalse(LoggingConfiguration.ShouldLogForType(typeof(string)));
        }

        private interface ISomeService<T>
        { }
        private class SomeImpl<T> : ISomeService<T>
        { }

        [TestMethod]
        public void Will_Ignore_Service_With_Generic_Args()
        {
            LoggingConfiguration.ConfigureWith().DoNotLogForService(typeof(ISomeService<string>));
            Assert.IsFalse(LoggingConfiguration.ShouldLogForType(typeof(SomeImpl<string>)));
        }

        [TestMethod]
        public void Will_Ignore_Service_With_Generic_Args_When_Service_Generic_Args_Are_Not_Specified()
        {
            LoggingConfiguration.ConfigureWith().DoNotLogForService(typeof(ISomeService<>));
            Assert.IsFalse(LoggingConfiguration.ShouldLogForType(typeof(SomeImpl<string>)));
        }

        [TestMethod]
        public void Wont_Ignore_Service_If_Generic_Types_Are_Specified_And_Are_Different()
        {
            LoggingConfiguration.ConfigureWith().DoNotLogForService(typeof(ISomeService<string>));
            Assert.IsTrue(LoggingConfiguration.ShouldLogForType(typeof(SomeImpl<object>)));
        }

        private static void AssertLoggingOptionsAreAllTrue()
        {
            Assert.IsTrue(LoggingConfiguration.LoggingOutputOptions.Enabled);
            Assert.IsTrue(LoggingConfiguration.LoggingOutputOptions.LogParameterValues);
            Assert.IsTrue(LoggingConfiguration.LoggingOutputOptions.LogReturnValue);
        }
    }
}
