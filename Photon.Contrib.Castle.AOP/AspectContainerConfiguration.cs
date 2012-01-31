namespace Photon.Contrib.Castle.AOP
{
    using global::Castle.Windsor;

    public static class AspectContainerConfiguration
    {
        public static IWindsorContainer WithAspects(this IWindsorContainer container, params IAspectSelector[] aspects)
        {
            container.AddFacility("aspectFacility", new AspectFacility(LoggerFactory.Instance.Create(typeof(AspectFacility)), aspects));
            return container;
        }
    }
}
