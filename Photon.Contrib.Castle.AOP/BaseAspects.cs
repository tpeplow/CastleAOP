namespace Photon.Contrib.Castle.AOP
{
    using System;

    public abstract class AbstractAspect : IAspect
    {
        public abstract bool HandleForMethodCall(MethodInvocationContext invocationContext);
        public abstract MethodVoteOptions PreCall(MethodInvocationContext invocation);
        public abstract void PostCall(MethodInvocationContext invocation);
        public abstract void OnException(MethodInvocationContext invocation, Exception e);
    }

    public abstract class MethodInvocationAspect : AbstractAspect
    {
        public override bool HandleForMethodCall(MethodInvocationContext invocationContext)
        {
            var info = new ExtraMethodInfo(invocationContext.Invocation.Method);
            return !info.IsGetterOrSetterFromProperty;
        }
    }

    public abstract class PropertyInvocationAspect : AbstractAspect
    {
        public override bool HandleForMethodCall(MethodInvocationContext invocationContext)
        {
            var info = new ExtraMethodInfo(invocationContext.Invocation.Method);
            return info.IsGetterOrSetterFromProperty;
        }
    }
}
