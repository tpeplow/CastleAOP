namespace Photon.Contrib.Castle.AOP
{
    using System;
    using System.Reflection;

    public class ExtraMethodInfo
    {
        private readonly MethodInfo methodInfo;

        public ExtraMethodInfo(MethodInfo methodInfo)
        {
            if (methodInfo == null) throw new ArgumentNullException("methodInfo");
            this.methodInfo = methodInfo;
            CheckMethodSignatureForPropertyInformation();
        }

        private void CheckMethodSignatureForPropertyInformation()
        {
            if (methodInfo.Name.StartsWith("get_")
                && methodInfo.GetParameters().Length == 0
                && methodInfo.ReturnType != null)
            {
                IsGetter = true;
                PropertyName = methodInfo.Name.Substring(4);
                return;
            }

            if (methodInfo.Name.StartsWith("set_")
                && methodInfo.GetParameters().Length == 1
                && methodInfo.ReturnType == typeof(void))
            {
                IsSetter = true;
                PropertyName = methodInfo.Name.Substring(4);
                return;
            }
        }

        public bool IsGetterOrSetterFromProperty
        {
            get { return IsSetter || IsGetter;}
        }

        public bool IsSetter
        {
            get; private set;
        }

        public bool IsGetter
        {
            get; private set;
        }

        public string PropertyName
        {
            get; private set;
        }
    }
}