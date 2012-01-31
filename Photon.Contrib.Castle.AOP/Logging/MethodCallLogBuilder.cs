namespace Photon.Contrib.Castle.AOP.Logging
{
    using System;
    using System.Text;

    public class MethodCallLogBuilder
    {
        private readonly MethodInvocationContext invocationContext;
        private readonly StringBuilder sb;

        public MethodCallLogBuilder(MethodInvocationContext invocationContext)
        {
            if (invocationContext == null) throw new ArgumentNullException("invocationContext");
            this.invocationContext = invocationContext;
            sb = new StringBuilder();
        }

        public void AddMethodCall()
        {
            sb.Append(invocationContext.Invocation.Method.Name);
        }

        public void AddParameters()
        {
            var firstParam = true;
            var i = 0;
            sb.Append("(");
            foreach (var param in invocationContext.Invocation.Method.GetParameters())
            {
                if (!firstParam) sb.Append(", ");
                firstParam = false;
                sb.Append(param.Name);
                sb.Append("=");
                sb.Append(invocationContext.Invocation.GetArgumentValue(i));
                i++;
            }
            sb.Append(")");
        }

        public string GetLogOutput()
        {
            return sb.ToString();
        }

        public void AddReturnValue()
        {
            Type returnType = invocationContext.Invocation.Method.ReturnType;
            if (returnType != typeof(void))
            {
                sb.Append(" Result: ");
                if (invocationContext.Invocation.ReturnValue == null)
                {
                    sb.Append("null");
                }
                else
                {
                    sb.Append(invocationContext.Invocation.ReturnValue);                    
                }
            }
        }

        public void AddMethodExecutionTime()
        {
            sb.Append("[");
            var callTook = DateTime.Now - invocationContext.MethodInvokeStartTime;
            sb.Append(callTook.TotalMilliseconds);
            sb.Append("ms]");
        }
    }
}
