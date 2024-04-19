using System;

namespace Core.Services;


[TestFixture]
public class HardServiceMasterTest
{

    [Service]
    private interface MyTestServiceIntf1 { }
    [Service]
    private interface MyTestServiceIntf2 : MyTestServiceIntf1 { }



    [Service]
    private abstract class MyTestBaseService : MyTestServiceIntf1 { }

    private class MyTestServiceImpl : MyTestBaseService, MyTestServiceIntf2 { }

    private class MyTestServiceImpl2 : IDisposable { public void Dispose() { } }




    [Test]
    public void GetByDirectClass()
    {
        var hsm     = new HardServiceMaster();
        var service = new MyTestServiceImpl();
        hsm.Register(service);
        hsm.FindService<MyTestServiceImpl>().ShouldBeSameAs(service);
    }

    [Test]
    public void GetByBaseClass()
    {
        var hsm     = new HardServiceMaster();
        var service = new MyTestServiceImpl();
        hsm.Register(service);
        hsm.FindService<MyTestBaseService>().ShouldBeSameAs(service);
    }

    [Test]
    public void GetByInterface()
    {
        var hsm     = new HardServiceMaster();
        var service = new MyTestServiceImpl();
        hsm.Register(service);
        hsm.FindService<MyTestServiceIntf2>().ShouldBeSameAs(service);
        hsm.FindService<MyTestServiceIntf1>().ShouldBeSameAs(service);
    }

    [Test]
    public void Shutdown_NoMore()
    {
        var hsm     = new HardServiceMaster();
        hsm.Register(new MyTestServiceImpl());
        hsm.Register(new MyTestServiceImpl2());

        hsm.ShutdownAllServices();

        hsm.ListAllServices().ShouldBeEmpty();
    }
}
