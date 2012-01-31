namespace Photon.Contrib.Castle.AOP.Logging
{
    using System;
    using System.Text.RegularExpressions;

    public interface ILoggingConfigurer
    {
        ILoggingConfigurer DoNotLogForService(Type type);
        ILoggingConfigurer LoggingOptionsFromAppConfig();
        ILoggingConfigurer LoggingOptions(Action<LoggingOutputOptions> configureOptions);
        ILoggingConfigurer RestrictedLoggingToTypesInNamespace(string namespacePattern);
        ILoggingConfigurer RestrictedLoggingToTypesInNamespace(Regex namespaceRegEx);
    }
}
