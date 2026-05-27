using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlowersShop_CourseWork.Models;
using FlowersShop_CourseWork.Services;

namespace FlowersShop_CourseWork.ViewModels;

public partial class LoginViewModel : ViewModelBase
{
    private readonly UserService _userService;

    [ObservableProperty] private string _email = "";
    [ObservableProperty] private string _password = "";
    [ObservableProperty] private string _errorMessage = ""; 
    
    public event Action? OnLoginSuccess; 

    public LoginViewModel()
    {
        _userService = new UserService();
    }

    [RelayCommand]
    private void Login()
    {
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Введіть Email та Пароль!";
            return;
        }
        
        var user = _userService.Authenticate(Email, Password);

        if (user != null)
        {
            ErrorMessage = "";
            
            Session.CurrentUser = user;
            
            OnLoginSuccess?.Invoke();
            
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop &&
                desktop.MainWindow?.DataContext is MainWindowViewModel mainVM)
            {
                mainVM.CompleteLogin(user.Role); 
            }
        }
        else
        {
            ErrorMessage = "Невірний email або пароль!";
        }
    }
}