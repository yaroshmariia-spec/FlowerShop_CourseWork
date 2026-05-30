using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlowersShop_CourseWork.Models;
using FlowersShop_CourseWork.Services;

namespace FlowersShop_CourseWork.ViewModels;

public partial class CartViewModel : ViewModelBase
{
    [ObservableProperty]
    private ObservableCollection<Flower> _cartItems;
    
    [ObservableProperty]
    private decimal _totalPrice;

    public CartViewModel()
    {
        CartItems = CartService.CartItems;
        
        CartItems.CollectionChanged += (s, e) => CalculateTotal();
        CalculateTotal();
    }

    private void CalculateTotal()
    {
        TotalPrice = CartItems.Sum(x => (decimal)x.Price);
    }

    [RelayCommand]
    private void RemoveFromCart(Flower item)
    {
        if (item != null)
        {
            CartItems.Remove(item);
        }
    }

    [RelayCommand]
    private void Checkout()
    {
        if (!CartItems.Any()) return;
        
        var sale = new Sale
        {
            UserId = Session.CurrentUser?.Email ?? "Guest",
            Items = CartItems.ToList(),
            TotalAmount = TotalPrice,
            PaymentMethod = "Card"
        };
        var saleService = new SaleService();
        saleService.SaveSale(sale);
        var fileService = new FileService("flowers_data.json");
        var allFlowers = fileService.LoadData();

        foreach (var cartItem in CartItems)
        {
            var flowerInDb = allFlowers.FirstOrDefault(f => f.Name == cartItem.Name);
            if (flowerInDb != null && flowerInDb.StockQuantity > 0)
            {
                flowerInDb.StockQuantity -= 1;
            }
        }
        
        fileService.SaveData(allFlowers);
        CartItems.Clear(); 
    }
}