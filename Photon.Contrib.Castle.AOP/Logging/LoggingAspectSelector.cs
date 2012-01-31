namespace Photon.Contrib.Castle.AOP.Logging
{
    using System;

    public class LoggingAspectSelector : IAspectSelector
    {
        public bool Enabled
        {
            get
            {
                return LoggingConfiguration.LoggingOutputOptions.Enabled;
            }
        }

        public bool IsMatch(Type service)
        {
            return LoggingConfiguration.ShouldLogForType(service);
        }

        public Type AspectType
        {
            get { return typeof (LoggingAspect); }
        }
    }
}
