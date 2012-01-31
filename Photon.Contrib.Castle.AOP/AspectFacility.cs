namespace Photon.Contrib.Castle.AOP
{
    using System.Collections.Generic;

    using global::Castle.Core;
    using global::Castle.Core.Logging;
    using global::Castle.MicroKernel;
    using global::Castle.MicroKernel.Facilities;
    using global::Castle.MicroKernel.Registration;

    public class AspectFacility : AbstractFacility
    {
        private readonly ILogger logger;

        private readonly IAspectSelector[] aspectSelectors;

        public AspectFacility(ILogger logger, params IAspectSelector[] aspectSelectors)
        {
            this.logger = logger;
            this.aspectSelectors = aspectSelectors;
        }

        protected override void Init()
        {
            Kernel.Register(
                Component.For<AspectInterceptor>().ImplementedBy<AspectInterceptor>().LifeStyle.Transient,
                Component.For<IAspectFactory>().Instance(new KernalAspectFactory(Kernel)));
            RegisterAspectsInKernal();
            Kernel.ComponentRegistered += ComponentRegistered;
        }

        private void RegisterAspectsInKernal()
        {
            foreach (var aspectSelector in aspectSelectors)
            {
                var requiresConfiguration = aspectSelector as IAspectRequiresConfiguration;
                if (requiresConfiguration != null)
                {
                    requiresConfiguration.Configure(Kernel);
                }
                Kernel.Register(Component.For(aspectSelector.AspectType).Named(aspectSelector.GetType().Name).LifeStyle.Singleton);
            }
        }

        void ComponentRegistered(string key, IHandler handler)
        {
            if (!(handler.ComponentModel.Implementation.IsPublic || handler.ComponentModel.Implementation.IsNestedPublic))
                return;

            var matchedAspects = new List<IAspectSelector>();
            foreach (var aspect in aspectSelectors)
            {
                if (aspect.Enabled &&
                    aspect.IsMatch(handler.ComponentModel.Service))
                {
                    if (matchedAspects.Count == 0)
                        handler.ComponentModel.Interceptors.Add(new InterceptorReference(typeof(AspectInterceptor)));
                    matchedAspects.Add(aspect);

                    logger.DebugFormat("Adding aspect {0} to service {1}", aspect, handler.ComponentModel.Service);
                }
            }
            // tell the component what aspects are matched.
            // give it only the matching aspects so it doesn't need to re-run IsMatch
            if (matchedAspects.Count > 0)
            {
                if (!handler.ComponentModel.ExtendedProperties.Contains(AspectInterceptor.AspectSelectorsExtendedPropertyName))
                {
                    handler.ComponentModel.ExtendedProperties.Add(
                        AspectInterceptor.AspectSelectorsExtendedPropertyName, matchedAspects);
                }
            }
        }
    }
}
