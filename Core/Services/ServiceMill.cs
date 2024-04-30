using System;
using System.Diagnostics;
using System.Reflection;

namespace Core.Services;


public abstract class ServiceMill
{

    private protected static ServiceMill? theMill = null;


    public static S GetService<S>()
        where S: class
    {
        Debug.Assert(theMill != null);
        S? service = theMill.FindService<S>();
        if (service == null)
        {
            var serviceType     = typeof(S);
            var serviceAttribute = serviceType.GetCustomAttribute(typeof(ServiceAttribute));
            if (serviceType.IsInterface && serviceAttribute is null)
                throw new Exception($"The interface {serviceType.Name} is not a service");
            throw new Exception("No service " + typeof(S).Name);
        }
        return service;
    }


    protected internal abstract S? FindService<S>()
        where S : class;

}
