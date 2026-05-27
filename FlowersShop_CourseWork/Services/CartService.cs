using System.Collections.ObjectModel;
using FlowersShop_CourseWork.Models;

namespace FlowersShop_CourseWork.Services;

public class CartService
{
    public static ObservableCollection<Flower> CartItems { get; } = new ObservableCollection<Flower>();
    public static void Add(Flower flower)
    {
        CartItems.Add(flower);
    }
}