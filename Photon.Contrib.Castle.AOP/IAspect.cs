namespace Photon.Contrib.Castle.AOP
{
    using System;

    public interface IAspect
    {
        bool HandleForMethodCall(MethodInvocationContext invocationContext);
        MethodVoteOptions PreCall(MethodInvocationContext invocation);
        void PostCall(MethodInvocationContext invocation);
        void OnException(MethodInvocationContext invocation, Exception e);
    }
}