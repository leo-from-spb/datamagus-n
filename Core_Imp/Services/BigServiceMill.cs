using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using NLog;
using Util.Extensions;

namespace Core.Services;


public class BigServiceMill : ServiceMill, IDisposable
{
    private readonly List<Service>            services          = new List<Service>();
    private readonly Dictionary<Type,Service> serviceDictionary = new Dictionary<Type,Service>();

    private bool weAreSunSettings = false;

    private static Logger Log = LogManager.GetCurrentClassLogger();

    internal static void Init()
    {
        Log.Debug("Big Service Mill is starting");
        Debug.Assert(theMill is null, "The service mill is already created");
        theMill = new BigServiceMill();
    }

    public static BigServiceMill GetTheMill()
    {
        var mill = theMill;
        Debug.Assert(mill is not null, "The service mill is not created yet");
        if (theMill is BigServiceMill bsm) return bsm;
        else throw new InvalidOperationException($"The current mill is already set up by another class: {theMill}");
    }

    public static BigServiceMill? GetTheMillWhenInitialized() =>
        theMill as BigServiceMill;


    public S Register<S>(S service)
        where S: class, Service
    {
        if (weAreSunSettings) throw new InvalidOperationException($"The service mill is shutting down, cannot register the service {service.ServiceName}");
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

        return service;
    }


    protected internal override S? FindService<S>()
        where S : class
    {
        Type type = typeof(S);
        object? instance = serviceDictionary.Get(type);
        if (instance is null) return null;
        return (S)instance;
    }


    protected internal void ShutdownAllServices()
    {
        weAreSunSettings = true;
        Thread.Sleep(1);

        int n = services.Count;

        // first, notify we're going to shut down
        for (int i = 0; i < n; i++)
        {
            Service service = services[i];
            try
            {
                service.Finalizing();
            }
            catch (Exception e)
            {
                var message = $"Unexpected exception during finalizing the service ${service.ServiceName} (nr {i}): {e.Message}";
                Log.Error(e, message);
            }
        }

        // then, shut down all of them in the reverse order
        for (int i = n-1; i >= 0; i--)
        {
            Service service = services[i];
            try
            {
                service.Shutdown();
                if (service is IDisposable d) d.Dispose();
            }
            catch (Exception e)
            {
                var message = $"Unexpected exception during shut down the service ${service.ServiceName} (nr {i}): {e.Message}";
                Log.Error(e, message);
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


    public void Dispose()
    {
        var mill = GetTheMillWhenInitialized();
        if (mill is not null)
        {
            ShutdownAllServices();
            theMill = null;
        }
    }
}
