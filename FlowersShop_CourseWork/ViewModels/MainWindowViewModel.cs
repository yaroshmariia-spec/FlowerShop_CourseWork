using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlowersShop_CourseWork.ViewModels;

namespace FlowersShop_CourseWork.ViewModels 
{
    public partial class MainWindowViewModel : ViewModelBase 
    {
        [ObservableProperty] private object _currentView;
        
        [ObservableProperty] 
        [NotifyPropertyChangedFor(nameof(IsAdmin))]
        private string _userRole = "User"; 
        
        public bool IsAdmin => UserRole == "Admin";

        [ObservableProperty] 
        private bool _isLoggedIn;

        public MainWindowViewModel()
        {
            ShowLogin();
        }
        public void CompleteLogin(string role)
        {
            UserRole = role;
            IsLoggedIn = true;
            NavigateTo("Dashboard");
        }
        [RelayCommand]
        public void ShowRegistration()
        {
            IsLoggedIn = false;
            var registerVM = new RegisterViewModel();
            CurrentView = registerVM;
        }
        [RelayCommand]
        public void ShowLogin()
        {
            IsLoggedIn = false;
            var loginVM = new LoginViewModel();
            CurrentView = loginVM;
        }
        [RelayCommand]
        public void Logout()
        {
            IsLoggedIn = false;
            ShowLogin();
        }
        [RelayCommand]
        public void NavigateTo(string viewName)
        {
            switch (viewName)
            {
                case "Dashboard":
                    CurrentView = new DashboardViewModel();
                    break;
                case "Catalog":
                    CurrentView = new CatalogViewModel();
                    break;
                case "Orders":
                    CurrentView = new OrdersViewModel();
                    break;
                case "Storefront":
                    CurrentView = new StorefrontViewModel();
                    break;
                case "Cart":
                    CurrentView = new CartViewModel();
                    break;
                case "Settings":
                    CurrentView = new SettingsViewModel();
                    break;
            }
        }
    }
}