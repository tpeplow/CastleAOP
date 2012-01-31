namespace Photon.Contrib.Castle.AOP.Tests
{
    public interface ITestService
    {
        void DoSomething();
        void DoSomethingDifferent(string parameter);
        void AMethodWithAnOverride();
        void AMethodWithAnOverride(string p1);
        void AMethodWithAnOverride(string p1, int p2);
        bool WasCalled { get; }
        bool DisposeCalled { get; }
        void DoSomethingWhichTakesMultipleTypes(object o);
    }


    public interface IAnotherTestService
    {

    }

    public class AnotherTestService : IAnotherTestService
    {

    }
}