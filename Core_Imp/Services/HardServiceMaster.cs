using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Util.Extensions;

namespace Core.Services;


public class HardServiceMaster : ServiceMaster
{
    private readonly List<object>             services          = new List<object>();
    private readonly Dictionary<Type, object> serviceDictionary = new Dictionary<Type, object>();


    public HardServiceMaster()
    {
        ServiceMaster.theMaster = this;
    }


    public void Register(object service)
    {
        var serviceType = service.GetType();
        services.Add(service);
        serviceDictionary[serviceType] = service;

        for (Type? b = serviceType.BaseType; b is not null; b = b.BaseType)
        {
            if (b.GetCustomAttribute<ServiceAttribute>() is not null)
            {
                serviceDictionary[b] = service;
            }
        }

        foreach (Type i in serviceType.GetInterfaces())
        {
            if (i.GetCustomAttribute<ServiceAttribute>() is not null)
            {
                serviceDictionary[i] = service;
            }
        }
    }


    protected internal override S? FindService<S>()
        where S : class
    {
        Type type = typeof(S);
        object? instance = serviceDictionary.Get(type);
        if (instance is null) return null;
        return (S)instance;
    }


    protected internal override void ShutdownAllServices()
    {
        int n = services.Count;
        for (int i = n-1; i >= 0; i--)
        {
            object service = services[i];
            try
            {
                if (service is IDisposable d) d.Dispose();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Disposing service {service.GetType().Name} thrown {e.GetType().Name}: {e.Message}");
                // TODO log the problem
            }
            finally
            {
                var keys = from e in serviceDictionary
                           where e.Value == service
                           select e.Key;
                keys.ToList().ForEach(k => serviceDictionary.Remove(k));
                services.RemoveAt(i);
            }
        }
    }

    internal IReadOnlyList<object> ListAllServices() => services;

}
