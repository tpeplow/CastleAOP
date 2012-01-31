namespace Photon.Contrib.Castle.AOP
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using global::Castle.Core;
    using global::Castle.Core.Interceptor;
    using global::Castle.DynamicProxy;

    [DebuggerStepThrough]
    public class AspectInterceptor : IInterceptor, IOnBehalfAware
    {
        private readonly IAspectFactory aspectFactory;
        public const string AspectSelectorsExtendedPropertyName = "aspects";

        private IEnumerable<IAspectSelector> aspectSelectors;
        private readonly List<IAspect> aspectInstances = new List<IAspect>();
        private Type serviceType;
        private readonly object initLock = new object();

        public AspectInterceptor(IAspectFactory aspectFactory)
        {
            if (aspectFactory == null) throw new ArgumentNullException("aspectFactory");
            this.aspectFactory = aspectFactory;
        }

        public void Intercept(IInvocation invocation)
        {
            var context = new MethodInvocationContext(serviceType, invocation);
            CreateAspects();

            var resp = PreCall(context);
            
            if (resp == MethodVoteOptions.Halt)
                return;

            try
            {
                context.MethodInvokeStartTime = DateTime.Now;
                invocation.Proceed();
            }
            catch (Exception ex)
            {
                OnException(context, ex);
                throw;
            }

            PostCall(context);
        }

        private MethodVoteOptions PreCall(MethodInvocationContext context)
        {
            var resp = MethodVoteOptions.Continue;
            var relevantAspects = GetRelevantAspectsForInvocation(context);
            foreach (var aspect in relevantAspects)
            {
                resp = aspect.PreCall(context);
                if (resp == MethodVoteOptions.Halt)
                    break;
            }

            return resp;
        }

        private void PostCall(MethodInvocationContext context)
        {
            foreach (var aspect in GetRelevantAspectsForInvocation(context))
            {
                aspect.PostCall(context);
            }
        }

        private void OnException(MethodInvocationContext context, Exception exception)
        {
            foreach (var aspect in GetRelevantAspectsForInvocation(context))
            {
                aspect.OnException(context, exception);
            }
        }

        private IEnumerable<IAspect> GetRelevantAspectsForInvocation(MethodInvocationContext context)
        {
            return aspectInstances.Where(a => a.HandleForMethodCall(context));
        }

        private void CreateAspects()
        {
            if (aspectInstances.Count > 0)
                return;
            
            lock (initLock)
            {
                if (aspectInstances.Count > 0)
                    return;

                foreach (var aspectSelector in aspectSelectors)
                {
                    aspectInstances.Add(aspectFactory.Create(aspectSelector.AspectType));
                }
            }
        }

        public void SetInterceptedComponentModel(ComponentModel target)
        {
            aspectSelectors = (IEnumerable<IAspectSelector>)target.ExtendedProperties[AspectSelectorsExtendedPropertyName];
            serviceType = target.Service;
        }
    }
}
