namespace Photon.Contrib.Castle.AOP.Logging
{
    using System;

    using global::Castle.Core.Logging;

    public class LoggingAspect : MethodInvocationAspect
    {
        private readonly ILoggerFactory loggingFactory;

        public LoggingAspect(ILoggerFactory loggingFactory)
        {
            if (loggingFactory == null) throw new ArgumentNullException("loggingFactory");
            this.loggingFactory = loggingFactory;
        }

        public override MethodVoteOptions PreCall(MethodInvocationContext invocation)
        {
            var logger = CreateLogger(invocation);
            logger.Debug(GetCallLogInfo(invocation));

            return MethodVoteOptions.Continue;
        }

        public override void PostCall(MethodInvocationContext invocation)
        {
            invocation.GetStateItem<ILogger>().Debug(GetPostCallLog(invocation));
        }

        public override void OnException(MethodInvocationContext invocation, Exception e)
        {
            if (LoggingConfiguration.LoggingOutputOptions.LogErrors)
            {
                invocation.GetStateItem<ILogger>().Error(GetCallLogInfo(invocation), e);
            }
        }

        private static string GetCallLogInfo(MethodInvocationContext invocationContext)
        {
            var logBuilder = new MethodCallLogBuilder(invocationContext);
            logBuilder.AddMethodCall();
            if (LoggingConfiguration.LoggingOutputOptions.LogParameterValues)
            {
                logBuilder.AddParameters();
            }
            return logBuilder.GetLogOutput();
        }

        private static string GetPostCallLog(MethodInvocationContext invocationContext)
        {
            var logBuilder = new MethodCallLogBuilder(invocationContext);
            logBuilder.AddMethodCall();
            logBuilder.AddMethodExecutionTime();
            if (LoggingConfiguration.LoggingOutputOptions.LogReturnValue)
            {
                logBuilder.AddReturnValue();
            }

            return logBuilder.GetLogOutput();
        }

        private ILogger CreateLogger(MethodInvocationContext invocation)
        {
            //TODO implement the caching aspect so we can cache loggers...
            var logger = loggingFactory.Create(invocation.Invocation.TargetType);
            invocation.AddStateItem(logger);
            return logger;
        }
    }
}