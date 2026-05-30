using System;

namespace FlowersShop_CourseWork.Models;

public class WriteOff
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string ShopId { get; set; } = "Shop_1";
    public string ProductId { get; set; }
    public int Quantity { get; set; }
    public string Reason { get; set; } 
    public DateTime Date { get; set; } = DateTime.Now;
    public string UserId { get; set; } 
}