namespace Photon.Contrib.Castle.AOP
{
    using System;
    using System.Collections.Generic;

    using global::Castle.DynamicProxy;

    public class MethodInvocationContext
    {
        private readonly Dictionary<string, object> stateItems = new Dictionary<string, object>();

        public MethodInvocationContext(Type serviceType, IInvocation invocation)
        {
            if (serviceType == null) throw new ArgumentNullException("serviceType");
            if (invocation == null) throw new ArgumentNullException("invocation");
            ServiceType = serviceType;
            Invocation = invocation;
            AspectInvokeStartTime = DateTime.Now;
        }

        public IInvocation Invocation { get; private set; }

        public DateTime AspectInvokeStartTime { get; private set; }

        public DateTime MethodInvokeStartTime { get; set; }

        public Type ServiceType { get; private set; }

        public void AddStateItem<T>(T item)
        {
            stateItems.Add(typeof(T).FullName, item);
        }

        public void AddStateItem<T>(string key, T item)
        {
            stateItems.Add(key, item);
        }

        public T GetStateItem<T>()
        {
            return GetStateItem<T>(typeof (T).FullName);
        }

        public T GetStateItem<T>(string key)
        {
            return (T)stateItems[key];
        }

        public bool ContainsStateItem<T>()
        {
            return ContainsStateItem(typeof (T).FullName);
        }

        public bool ContainsStateItem(string key)
        {
            return stateItems.ContainsKey(key);
        }
    }
}