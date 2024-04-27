using System;
using System.Diagnostics;

namespace Core.Services;


public abstract class ServiceMill
{

    private protected static ServiceMill? theMill = null;


    public static S GetService<S>()
        where S: class
    {
        Debug.Assert(theMill != null);
        S? service = theMill.FindService<S>();
        if (service == null) throw new Exception("No service " + typeof(S).Name);
        return service;
    }


    protected internal abstract S? FindService<S>()
        where S : class;
    
}
