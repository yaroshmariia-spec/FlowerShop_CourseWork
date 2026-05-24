using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using FlowersShop_CourseWork.Models;

namespace FlowersShop_CourseWork.ViewModels;

public partial class StorefrontViewModel : ViewModelBase
{
    public ObservableCollection<StorefrontItem> Products { get; } = new()
    {
        new StorefrontItem { Name = "Червона троянда", Price = "1500 грн", IsHit = true, Emoji = "🌹" },
        new StorefrontItem { Name = "Орхідея Фаленопсис", Price = "800 грн", Emoji = "🌸" },
        new StorefrontItem { Name = "Букет Тюльпанів", Price = "1200 грн", IsHit = true, Emoji = "🌷" },
        new StorefrontItem { Name = "Півонії (Коробка)", Price = "2500 грн", Emoji = "🌺" },
        new StorefrontItem { Name = "Соняшники", Price = "950 грн", Emoji = "🌻" },
        new StorefrontItem { Name = "Польові квіти", Price = "600 грн", Emoji = "🌼" }
    };

    [RelayCommand]
    private void AddToCart(StorefrontItem item)
    {
    }
}