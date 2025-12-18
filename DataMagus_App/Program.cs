using System;
using System.Threading;
using Core.Services;
using Core.Stationery;

namespace DataMagus.App;


/// <summary>
/// DataMagus Application bootstrap class.
/// </summary>
public static class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        SayHello();
        Sunrise();

        try
        {
            Gui.Application.Program.Main(args);
        }
        finally
        {
            Thread.Sleep(1);
            Shutdown();
            SayGoodbye();
        }
    }


    private static void SayHello()
    {
        if (DataMagusInfo.InDebug)
        {
            Console.WriteLine($"DataMagus version {DataMagusInfo.ProductVersion} is in the Debug mode.");
        }
    }


    private static void Sunrise()
    {
        CoreServiceMaster.Sunrise();
    }


    private static void Shutdown()
    {
        CoreServiceMaster.Shutdown();
    }


    private static void SayGoodbye()
    {
        if (DataMagusInfo.InDebug)
        {
            Console.WriteLine("Goodbye!");
        }
    }

}
