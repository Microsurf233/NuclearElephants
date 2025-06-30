using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using NuclearElephants.ViewModels;

namespace NuclearElephants.Views;

public partial class ElephantWindow : Window
{
    public ElephantWindow()
    {
        DataContext = App.ServiceProvider.GetRequiredService<ElephantWindowViewModel>();
        InitializeComponent();
    }
}