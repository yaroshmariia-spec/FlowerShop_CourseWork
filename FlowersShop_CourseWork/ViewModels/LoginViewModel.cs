using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace FlowersShop_CourseWork.ViewModels;

public partial class LoginViewModel : ViewModelBase
{
    [ObservableProperty] private string _email = "";
    [ObservableProperty] private string _password;
    
    public event Action? OnLoginSuccess;

    [RelayCommand]
    private void Login()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop &&
            desktop.MainWindow?.DataContext is MainWindowViewModel mainVM)
        {
            mainVM.CompleteLogin("Admin");
        }
    }
}