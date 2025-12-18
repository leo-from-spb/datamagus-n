using System;
using System.Threading;
using Core.Services;

namespace DataMagus.App;


/// <summary>
/// DataMagus Application bootstrap class.
/// </summary>
public static class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        Sunrise();

        try
        {
            Gui.Application.Program.Main(args);
        }
        finally
        {
            Thread.Sleep(1);
            Shutdown();
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
}
