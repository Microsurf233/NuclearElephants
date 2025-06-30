using System;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Microsoft.Extensions.DependencyInjection;
using NuclearElephants.ViewModels;

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

    private void AboutButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (sender is Control ctl)
        {
            FlyoutBase.ShowAttachedFlyout(ctl);
        }
    }
}