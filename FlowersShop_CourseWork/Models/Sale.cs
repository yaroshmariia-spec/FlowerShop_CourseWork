using System;
using System.Collections.Generic;

namespace FlowersShop_CourseWork.Models;

public class Sale
{
    public string Id { get; set; } = Guid.NewGuid().ToString(); 
    public string ShopId { get; set; } = "Shop_1"; 
    public string UserId { get; set; } 
    
    public List<Flower> Items { get; set; } = new List<Flower>();
        
    public decimal TotalAmount { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
    public string PaymentMethod { get; set; } = "Card";
}