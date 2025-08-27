using System;
using System.Collections.ObjectModel;
using System.IO;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using NuclearElephants.Models;
using NAudio.Wave;
using NuclearElephants.Views;
using MsBox.Avalonia;
using FluentAvalonia.UI.Controls;
using NuclearElephants.Utills;
using NuclearElephants.Helpers;

namespace NuclearElephants.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly ElephantWindowViewModel _elephantWindowVm =
        App.ServiceProvider.GetRequiredService<ElephantWindowViewModel>();

    public ObservableCollection<Elephant> Elephants { get; } =
        App.ServiceProvider.GetRequiredService<ElephantWindowViewModel>().Elephants;

    private readonly ExternalDirectoryHelper _itemsHelper = new("NuclearItems");
    private readonly ExternalDirectoryHelper _audiosHelper = new("Audios");

    [ObservableProperty] private int _extraItemsCount;
    [ObservableProperty] private int _extraAudiosCount;

    [ObservableProperty]
    private WindowTransparencyLevelCollection _transparencyLevel = new([WindowTransparencyLevel.Mica]);

    [RelayCommand]
    private void LoadItems()
    {
        ExtraItemsCount = _itemsHelper.Files.Count;
        ExtraAudiosCount = _audiosHelper.Files.Count;
    }

    [RelayCommand]
    private void OpenItemsFolder() => _itemsHelper.OpenInFileExplorer();

    [RelayCommand]
    private void OpenAudiosFolder() => _audiosHelper.OpenInFileExplorer();

    [RelayCommand]
    private void AddElephant()
    {
        var e = new Elephant();

        if (_itemsHelper.Files.Count != 0)
        {
            var file = _itemsHelper.PickFileRandomly();
            var name = Path.GetFileNameWithoutExtension(file);
            var image = new Bitmap(file);

            e.Image = image;
            e.Name = name;
        }

        e.FlyOverDuration = TimeSpan.FromMilliseconds(Random.Shared.Next(100, 10000));
        e.PositionY = Random.Shared.Next(0, 1000);
        e.Id = e.GetHashCode();

        _elephantWindowVm.AddElephant(e);

        if (_audiosHelper.Files.Count == 0) return;

        var waveOut = new WaveOutEvent();
        waveOut.Init(new AudioFileReader(_audiosHelper.PickFileRandomly()));
        waveOut.Play();
    }

    [RelayCommand]
    private void KillAllElephants() => Elephants.Clear();

    public MainWindowViewModel()
    {
        ExternalDirectoryHelper.CheckOrCreateExternalDirectoryRelative("NuclearItems");
        ExternalDirectoryHelper.CheckOrCreateExternalDirectoryRelative("Audios");
        
        LoadItems();

        AddElephant();

        var elephantWindow = App.ServiceProvider.GetRequiredService<ElephantWindow>();
        elephantWindow.Show();

        var child = elephantWindow.TryGetPlatformHandle()?.Handle ?? IntPtr.Zero;
        if (child == IntPtr.Zero)
        {
            MessageBoxManager.GetMessageBoxStandard("Info", "获取窗口句柄失败。将以其他方式显示Elephants。");
            return;
        }

        var parent = WallpaperUtils.GetWorkerW();
        Win32.User32.SetParent(child, parent);
    }
}