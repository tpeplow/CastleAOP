namespace Photon.Contrib.Castle.AOP.Logging
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class LoggingConfiguration : ILoggingConfigurer
    {
        private readonly Dictionary<Type, object> typesToIgnore = new Dictionary<Type, object>();
        private readonly List<Regex> namespaceRestrictions = new List<Regex>();
        private static LoggingConfiguration current;

        static LoggingConfiguration()
        {
            Reset();
        }

        public static void Reset()
        {
            current = new LoggingConfiguration();
        }

        public static ILoggingConfigurer ConfigureWith()
        {
            return current;
        }

        public LoggingConfiguration()
        {
            LoggingOutputOptions = new LoggingOutputOptions();
        }

        public ILoggingConfigurer DoNotLogForService(Type type)
        {
            typesToIgnore.Add(type, new object());
            return this;
        }

        public static ILoggingOutputOptions LoggingOutputOptions
        {
            get; private set;
        }

        public ILoggingConfigurer LoggingOptionsFromAppConfig()
        {
            var options = ConfigurationManager.GetSection(LoggingAspectConfigurationSection.SectionName) as ILoggingOutputOptions;
            if (options == null)
                throw new ConfigurationErrorsException("Cannot find config section " + LoggingAspectConfigurationSection.SectionName);
            LoggingOutputOptions = options;

            return this;
        }

        public ILoggingConfigurer LoggingOptions(Action<LoggingOutputOptions> configureOptions)
        {
            var options = new LoggingOutputOptions();
            configureOptions(options);
            LoggingOutputOptions = options;
            return this;
        }

        public ILoggingConfigurer RestrictedLoggingToTypesInNamespace(string namespacePattern)
        {
            return RestrictedLoggingToTypesInNamespace(new Regex(namespacePattern));
        }

        public ILoggingConfigurer RestrictedLoggingToTypesInNamespace(Regex namespaceRegEx)
        {
            namespaceRestrictions.Add(namespaceRegEx);
            return this;
        }

        public static bool ShouldLogForType(Type type)
        {
            return IsWithinRestrictedNamespace(type) 
                && !current.typesToIgnore.ContainsKey(type)
                && !DerivesFromAnyServicesToIgnore(type);
        }

        private static bool DerivesFromAnyServicesToIgnore(Type type)
        {
            return current.typesToIgnore.Keys.Any(ignoredService => ignoredService.IsAssignableFrom(type) 
                || IsTypeAImplementationOfGeneric(ignoredService, type));
        }

        private static bool IsTypeAImplementationOfGeneric(Type baseType, Type type)
        {
            if (!baseType.IsGenericType || !type.IsGenericType || !baseType.IsGenericTypeDefinition)
                return false;

            var baseGenericArguments = baseType.GetGenericArguments();
            var typeGenericArguments = type.GetGenericArguments();
            
            if (baseGenericArguments.Length != typeGenericArguments.Length)
                return false;

            for (int i = 0; i < baseGenericArguments.Length; i++)
            {
                if (baseGenericArguments[i].FullName == null)
                {
                    baseGenericArguments[i] = typeGenericArguments[i];
                }
            }

            var specificImplOfBase = baseType.MakeGenericType(baseGenericArguments);

            return specificImplOfBase.IsAssignableFrom(type);
        }

        private static bool IsWithinRestrictedNamespace(Type type)
        {
            // if we don't have any namespace resitrctions then include the type
            return current.namespaceRestrictions.Count == 0
                || current.namespaceRestrictions.Any(expr => expr.IsMatch(type.Namespace));
        }
    }
}