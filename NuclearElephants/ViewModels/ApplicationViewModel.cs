using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using NuclearElephants.Views;

namespace NuclearElephants.ViewModels;

public partial class ApplicationViewModel : ViewModelBase
{
    [RelayCommand]
    private static void ExitApplication() => Environment.Exit(0);

    [RelayCommand]
    private static void ShowMainWindow() => App.ServiceProvider.GetRequiredService<MainWindow>().Show();
}