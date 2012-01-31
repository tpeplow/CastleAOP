namespace Photon.Contrib.Castle.AOP.Logging
{
    public class LoggingOutputOptions : ILoggingOutputOptions
    {
        public LoggingOutputOptions()
        {
            Enabled = true;
            LogParameterValues = true;
            LogReturnValue = false;
        }

        public bool Enabled { get; set; }
        public bool LogParameterValues { get; set; }
        public bool LogReturnValue { get; set; }
        public bool LogErrors { get; set; }
    }
}