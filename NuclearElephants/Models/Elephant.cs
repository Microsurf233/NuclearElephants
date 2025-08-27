using System;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;

namespace NuclearElephants.Models;

public partial class Elephant
{
    public string Name { get; set; } = "Nuclear Elephant";
    public int PositionY { get; set; }
    public TimeSpan FlyOverDuration { get; set; }
    public int Id { get; set; }
    public Bitmap Image { get; set; } = new(AssetLoader.Open(new Uri("avares://NuclearElephants/Assets/Elephant.png")));
}