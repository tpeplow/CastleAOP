namespace Photon.Contrib.Castle.AOP
{
    using System;

    public interface IAspectFactory
    {
        IAspect Create(Type aspect);
    }
}