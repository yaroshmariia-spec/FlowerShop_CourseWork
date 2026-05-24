using System.Collections.ObjectModel;
using FlowersShop_CourseWork.Models;

namespace FlowersShop_CourseWork.ViewModels;

public partial class OrdersViewModel : ViewModelBase
{
    public ObservableCollection<OrderItem> NewOrders { get; } = new()
    {
        new OrderItem { OrderNumber = "#1042", CustomerName = "Марія Іваненко", Price = "1500 грн", Time = "10 хв тому" },
        new OrderItem { OrderNumber = "#1043", CustomerName = "Олександр П.", Price = "850 грн", Time = "2 хв тому" }
    };

    public ObservableCollection<OrderItem> ProcessingOrders { get; } = new()
    {
        new OrderItem { OrderNumber = "#1040", CustomerName = "Ірина В.", Price = "2100 грн", Time = "Готується" }
    };

    public ObservableCollection<OrderItem> CompletedOrders { get; } = new()
    {
        new OrderItem { OrderNumber = "#1038", CustomerName = "Тарас Г.", Price = "450 грн", Time = "Відправлено" },
        new OrderItem { OrderNumber = "#1039", CustomerName = "Олена К.", Price = "1200 грн", Time = "Відправлено" }
    };
}