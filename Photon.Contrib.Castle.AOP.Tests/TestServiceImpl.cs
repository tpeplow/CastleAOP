namespace Photon.Contrib.Castle.AOP.Tests
{
    using System;

    public class TestServiceImpl : ITestService, IDisposable
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

        public void DoSomething()
        {
            WasCalled = true;
        }

        public bool DisposeCalled { get; private set; }

        public void Dispose()
        {
            DisposeCalled = true;
        }


        public void DoSomethingWhichTakesMultipleTypes(object o)
        {
        }
    }
}