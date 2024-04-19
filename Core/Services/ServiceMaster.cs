using System;
using System.Diagnostics;

namespace Core.Services;


public abstract class ServiceMaster
{

    protected static ServiceMaster? theMaster = null;


    public static S GetService<S>()
        where S: class
    {
        Debug.Assert(theMaster != null);
        S? service = theMaster!.FindService<S>();
        if (service == null) throw new Exception("No service " + typeof(S).Name);
        return service;
    }


    protected internal abstract S? FindService<S>()
        where S : class;

    protected internal abstract void ShutdownAllServices();

}
