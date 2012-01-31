namespace Photon.Contrib.Castle.AOP.Logging
{
    using System.Configuration;

    public class LoggingAspectConfigurationSection : ConfigurationSection, ILoggingOutputOptions
    {
        public const string SectionName = "loggingAspect";

        [ConfigurationProperty("enabled")]
        public bool Enabled
        {
            get { return (bool) base["enabled"]; }
            set { base["enabled"] = value; }
        }

        [ConfigurationProperty("logParameterValues")]
        public bool LogParameterValues
        {
            get { return (bool)base["logParameterValues"]; }
            set { base["logParameterValues"] = value; }
        }

        [ConfigurationProperty("logReturnValue")]
        public bool LogReturnValue
        {
            get { return (bool)base["logReturnValue"]; }
            set { base["logReturnValue"] = value; }
        }

        [ConfigurationProperty("logErrors")]
        public bool LogErrors
        {
            get { return (bool)base["logErrors"]; }
            set { base["logErrors"] = value; }
        }
    }
}
