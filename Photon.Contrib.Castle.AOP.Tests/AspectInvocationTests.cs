namespace Photon.Contrib.Castle.AOP.Tests
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using global::Castle.MicroKernel.Registration;
    using global::Castle.Windsor;

    public class AspectFacilitySpecifications
    {
        public const string AspectSelectionSpecificationsSubject = "Aspect invocation";

        #region test classes
        protected class TestAspectSelector : IAspectSelector
        {
            public TestAspectSelector()
            {
                Enabled = true;
                Match = true;
            }

            public bool Enabled
            {
                get;
                set;
            }

            public bool Match { get; set; }

            public bool IsMatch(Type service)
            {
                return Match;
            }

            public Type AspectType
            {
                get { return typeof(TestAspect); }
            }
        }

        protected class TestAspect : MethodInvocationAspect
        {
            public static bool PreCallCalled { get; private set; }
            public static bool PostCallCalled { get; private set; }
            public static Exception Exception { get; private set; }
            public static MethodVoteOptions MethodVoteOptions { get; set; }

            public static void Reset()
            {
                MethodVoteOptions = MethodVoteOptions.Continue;
                PreCallCalled = false;
                PostCallCalled = false;
                Exception = null;
            }

            public override MethodVoteOptions PreCall(MethodInvocationContext invocation)
            {
                PreCallCalled = true;
                return MethodVoteOptions;
            }

            public override void PostCall(MethodInvocationContext invocation)
            {
                PostCallCalled = true;
            }

            public override void OnException(MethodInvocationContext invocation, Exception e)
            {
                Exception = e;
            }
        }
        #endregion

        public static IWindsorContainer container;

    }

    public class when_registering_aspect_facility : AspectFacilitySpecifications
    {
        protected static TestAspectSelector aspectSelector;
        protected static ITestService instance;

        [TestInitialize]
        public void EstablishContext()
        {
            TestAspect.Reset();
            container = ContainerFactory.CreateContainer();

            aspectSelector = new TestAspectSelector();
            container.WithAspects(aspectSelector);
        }

        protected static void ResolveInstance()
        {
            container.Register(Component.For<ITestService>().ImplementedBy<TestServiceImpl>());

            instance = container.Resolve<ITestService>();
        }
    }

    [TestClass]
    public class when_registered_aspect_is_matched : when_registering_aspect_facility
    {
        [TestInitialize]
        public void BecauseOf()
        {
            ResolveInstance();
            instance.DoSomething();
        }

        [TestMethod]
        public void It_should_call_aspect_before_method_is_invoked()
        {
            Assert.IsTrue(TestAspect.PreCallCalled);
        }

        [TestMethod]
        public void It_should_call_aspect_after_method_is_invoked()
        {
            Assert.IsTrue(TestAspect.PostCallCalled);
        }

        [TestMethod]
        public void It_should_call_instance()
        {
            Assert.IsTrue(instance.WasCalled);
        }
    }

    [TestClass]
    public class when_registered_aspect_not_matched : when_registering_aspect_facility
    {
        [TestInitialize]
        public void BecauseOf()
        {
            aspectSelector.Match = false;
            ResolveInstance();

            instance.DoSomething();
        }

        [TestMethod]
        public void It_should_not_call_aspect_before_method_is_invoked()
        {
            Assert.IsFalse(TestAspect.PreCallCalled);
        }

        [TestMethod]
        public void It_should_not_call_aspect_after_method_is_invoked()
        {
            Assert.IsFalse(TestAspect.PostCallCalled);
        }

        [TestMethod]
        public void It_should_call_instance()
        {
            Assert.IsTrue(instance.WasCalled);
        }
    }

    [TestClass]
    public class when_registered_aspect_not_enabled : when_registering_aspect_facility
    {
        [TestInitialize]
        public void BecauseOf()
        {
            aspectSelector.Enabled = false;
            ResolveInstance();

            instance.DoSomething();
        }

        [TestMethod]
        public void It_should_not_call_aspect_before_method_is_invoked()
        {
            Assert.IsFalse(TestAspect.PreCallCalled);
        }

        [TestMethod]
        public void It_should_not_call_aspect_after_method_is_invoked()
        {
            Assert.IsFalse(TestAspect.PostCallCalled);
        }

        [TestMethod]
        public void It_should_call_instance()
        {
            Assert.IsTrue(instance.WasCalled);
        }
    }

    [TestClass]
    public class when_instance_throws_exception : when_registering_aspect_facility
    {
        [TestInitialize]
        public void BecauseOf()
        {
            container.Register(Component.For<ITestService>().ImplementedBy<TestServiceImplThatThrowsException>());
            instance = container.Resolve<ITestService>();

            var exceptionWasThrown = false;
            try
            {
                instance.DoSomething();
            }
            catch (Exception)
            {
                exceptionWasThrown = true;
            }
            if (!exceptionWasThrown)
            {
                throw new Exception("Expected an exception to be thrown but one wasn't");
            }
        }

        [TestMethod]
        public void It_should_call_aspect_with_exception_details()
        {
            Assert.IsNotNull(TestAspect.Exception);
        }
    }

    [TestClass]
    public class when_aspect_requests_halt : when_registering_aspect_facility
    {
        [TestInitialize]
        public void BecauseOf()
        {
            TestAspect.MethodVoteOptions = MethodVoteOptions.Halt;
            ResolveInstance();

            instance.DoSomething();
        }

        [TestMethod]
        public void It_should_call_aspect_before_method_is_invoked()
        {
            Assert.IsTrue(TestAspect.PreCallCalled);
        }

        [TestMethod]
        public void It_should_not_call_aspect_after_method_is_invoked()
        {
            Assert.IsFalse(TestAspect.PostCallCalled);
        }

        [TestMethod]
        public void It_should_not_call_instance()
        {
            Assert.IsFalse(instance.WasCalled);
        }

    }
}
