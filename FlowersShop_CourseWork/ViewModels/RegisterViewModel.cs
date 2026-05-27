using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlowersShop_CourseWork.Services;

namespace FlowersShop_CourseWork.ViewModels;

public partial class RegisterViewModel : ViewModelBase
{
    private readonly UserService _userService;

    [ObservableProperty] private string _email = "";
    [ObservableProperty] private string _password = "";
    [ObservableProperty] private bool _isAdmin;

    [ObservableProperty] private string _errorMessage = "";

    public RegisterViewModel()
    {
        _userService = new UserService();
    }

    [RelayCommand]
    private void Register()
    {
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Заповніть всі поля!";
            return;
        }

        bool success = _userService.Register(Email, Password, IsAdmin);

        if (success)
        {
            ErrorMessage = "";
            var role = IsAdmin ? "Admin" : "User";

            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop &&
                desktop.MainWindow?.DataContext is MainWindowViewModel mainVM)
            {
                mainVM.CompleteLogin(role);
            }
        }
        else
        {
            ErrorMessage = "Користувач з таким Email вже існує!";
        }
    }
}