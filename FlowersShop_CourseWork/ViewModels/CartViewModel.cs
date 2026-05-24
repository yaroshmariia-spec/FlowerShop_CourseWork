using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using FlowersShop_CourseWork.Models;

namespace FlowersShop_CourseWork.ViewModels;

public partial class CartViewModel : ViewModelBase
{
    public ObservableCollection<CartItem> Items { get; } = new()
    {
        new CartItem { Name = "Червона троянда (букет)", Price = "1500 грн", Quantity = 1 },
        new CartItem { Name = "Орхідея Фаленопсис", Price = "800 грн", Quantity = 2 }
    };

    [ObservableProperty]
    private string _totalSum = "3100 грн";
}