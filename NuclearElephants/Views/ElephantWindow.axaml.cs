using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using NuclearElephants.Utills;
using NuclearElephants.ViewModels;
using System;

namespace NuclearElephants.Views;

public partial class ElephantWindow : Window
{
    public ElephantWindow()
    {
        DataContext = App.ServiceProvider.GetRequiredService<ElephantWindowViewModel>();
        InitializeComponent();
        Loaded += (s, e) =>
        {
            var handle = TryGetPlatformHandle()?.Handle ?? IntPtr.Zero;
            if (handle == IntPtr.Zero)
            {
                throw new Exception("Failed to get platform handle.");
            }
            var r = WallpaperUtils.SetWallpaper(handle);
            Console.WriteLine(r);
        };
    }
}