using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using NuclearElephants.Models;

namespace NuclearElephants.ViewModels;

public partial class ElephantWindowViewModel : ViewModelBase
{
    public ObservableCollection<Elephant> Elephants { get; set; } = [];

    public void AddElephant(Elephant elephant) => Elephants.Add(elephant);
}