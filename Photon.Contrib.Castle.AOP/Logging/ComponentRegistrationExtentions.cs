namespace Photon.Contrib.Castle.AOP.Logging
{
    using global::Castle.MicroKernel.Registration;

    public static class ComponentRegistrationExtentions
    {
        public static ComponentRegistration<TService> IgnoreLogging<TService>(this ComponentRegistration<TService> registration)
        {
            LoggingConfiguration.ConfigureWith().DoNotLogForService(registration.ServiceType);
            return registration;
        }

    }
}