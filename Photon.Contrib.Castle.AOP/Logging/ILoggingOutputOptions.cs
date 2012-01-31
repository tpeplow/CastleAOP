namespace Photon.Contrib.Castle.AOP.Logging
{
    public interface ILoggingOutputOptions
    {
        bool Enabled { get; }
        bool LogParameterValues { get; }
        bool LogReturnValue { get; }
        bool LogErrors { get; }
    }
}