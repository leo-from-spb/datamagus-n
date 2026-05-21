using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Core.Interaction.Commands;
using Core.Services;
using DataMagus.Main.Main;

namespace DataMagus.Main;

public partial class App : Application
{
    private MainWindow? myMainWindow = null;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        Name = "DataMagus";
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime applicationLifetime)
        {
            CreateMainWindow(applicationLifetime);
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void CreateMainWindow(IClassicDesktopStyleApplicationLifetime applicationLifetime)
    {
        this.myMainWindow  = new MainWindow();
        applicationLifetime.MainWindow = myMainWindow;
    }


    private void ShowAboutMenuItem_OnClick(object? sender, EventArgs e) =>
        ServiceMill.GetService<CommandRegistry>().Execute(MainCommands.ShowAbout);

}
