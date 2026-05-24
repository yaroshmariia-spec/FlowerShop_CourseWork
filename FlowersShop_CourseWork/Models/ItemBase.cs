using System;

namespace FlowersShop_CourseWork.Models;

public abstract class ItemBase
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal? Price { get; set; }
    public string Category { get; set; } = string.Empty;
    
    public abstract string GetShortInfo();
}