using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using FluentAvalonia.UI.Controls;
using Microsoft.Extensions.DependencyInjection;
using NuclearElephants.ViewModels;
using static Avalonia.Controls.Primitives.FlyoutBase;

namespace NuclearElephants.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        DataContext = App.ServiceProvider.GetRequiredService<MainWindowViewModel>();
        InitializeComponent();
    }

    private void Window_OnClosing(object? sender, WindowClosingEventArgs e)
    {
        Hide();
        e.Cancel = true;
    }

    private void NavigationView_OnItemInvoked(object? sender, NavigationViewItemInvokedEventArgs e)
    {
        var vm = DataContext as MainWindowViewModel;
        switch (e.InvokedItem?.ToString())
        {
            case "Image Folder":
                vm?.OpenItemsFolderCommand.Execute(null);
                break;
            case "Sound Effect Folder":
                vm?.OpenAudiosFolderCommand.Execute(null);
                break;
            case "Refresh External Resources":
                vm?.LoadItemsCommand.Execute(null);
                break;
            case "About":
                ShowAttachedFlyout(AboutMenuItem);
                break;
        }
    }
}