namespace Photon.Contrib.Castle.AOP.Tests
{
    using System;

    public class TestServiceImplThatThrowsException : ITestService
    {
        public void DoSomethingDifferent(string parameter)
        {
        }

        public void AMethodWithAnOverride()
        {
        }

        public void AMethodWithAnOverride(string p1)
        {
        }

        public void AMethodWithAnOverride(string p1, int p2)
        {
        }

        public bool WasCalled { get; private set; }

        public bool DisposeCalled
        {
            get { throw new NotImplementedException(); }
        }

        public void DoSomething()
        {
            throw new NotImplementedException();
        }


        public void DoSomethingWhichTakesMultipleTypes(object o)
        {
            throw new NotImplementedException();
        }
    }
}