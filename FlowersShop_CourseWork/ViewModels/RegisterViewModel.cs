using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace FlowersShop_CourseWork.ViewModels;

public partial class RegisterViewModel : ViewModelBase
{
    [ObservableProperty] private string _email;
    [ObservableProperty] private string _password;
    [ObservableProperty] private bool _isAdmin;

    [RelayCommand]
    private void Register()
    {
        var role = IsAdmin ? "Admin" : "User";
        if (App.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop &&
            desktop.MainWindow?.DataContext is MainWindowViewModel mainVM)
        {
            mainVM.CompleteLogin(role);
        }
    }
}