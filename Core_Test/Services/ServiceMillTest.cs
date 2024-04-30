namespace Core.Services;



[TestFixture]
public class ServiceMillTest
{

    private class TestServiceMill : ServiceMill
    {
        private readonly MyTestServiceIntf myTestService = new MyTestServiceImpl();

        public static void CreateTestServiceMill()
        {
            theMill = new TestServiceMill();
        }

        protected internal override S? FindService<S>()
            where S: class
        {
            if (typeof(S).IsAssignableFrom(typeof(MyTestServiceIntf))) return (S)myTestService;
            return null;
        }

        protected internal virtual void ShutdownAllServices() { }
    }


    [Service]
    private interface MyTestServiceIntf { }

    private class MyTestServiceImpl : MyTestServiceIntf { }



    [Test]
    public void GetService()
    {
        TestServiceMill.CreateTestServiceMill();
        MyTestServiceIntf service = GetService<MyTestServiceIntf>();
        service.ShouldNotBeNull();
    }

}
