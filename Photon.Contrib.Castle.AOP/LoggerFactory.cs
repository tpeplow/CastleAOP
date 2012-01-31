namespace Photon.Contrib.Castle.AOP
{
    using System;

    using global::Castle.Core.Logging;

    public static class LoggerFactory
    {
        private static ILoggerFactory instance;

        public static void SetInstance(ILoggerFactory loggerFactory)
        {
            if (loggerFactory == null)
            {
                throw new ArgumentNullException("loggerFactory");
            }

            instance = loggerFactory;
        }

        public static ILoggerFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    throw new NullReferenceException("ILoggerFactory not set - make sure you've called SetInstance");
                }

                return instance;
            }
        }
    }
}