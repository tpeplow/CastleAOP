namespace Photon.Contrib.Castle.AOP
{
    using System;

    public interface IAspectSelector
    {
        bool Enabled { get; }
        bool IsMatch(Type service);
        Type AspectType { get; }
    }
}