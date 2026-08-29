using CommunityToolkit.Mvvm.ComponentModel;
using AvaloniaFunctionKeyTemplate.Shared;

namespace AvaloniaFunctionKeyTemplate.Features.Main;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    public partial string Greeting { get; set; } = "Welcome to Avalonia!";
}
