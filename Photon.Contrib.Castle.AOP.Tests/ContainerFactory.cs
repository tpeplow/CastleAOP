namespace Photon.Contrib.Castle.AOP.Tests
{
    using global::Castle.Core.Logging;
    using global::Castle.Facilities.Logging;
    using global::Castle.Windsor;

    public static class ContainerFactory
    {
        public static IWindsorContainer CreateContainer()
        {
            var container = new WindsorContainer();
            container.AddFacility("log4netLogging", new LoggingFacility(LoggerImplementation.Log4net).WithAppConfig());
            LoggerFactory.SetInstance(container.Resolve<ILoggerFactory>());
            return container;
        }
    }
}