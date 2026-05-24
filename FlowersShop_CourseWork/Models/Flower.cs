namespace FlowersShop_CourseWork.Models;

public class Flower : ItemBase
{
    public string Color { get; set; } = string.Empty;
    public int? StockQuantity { get; set; }

    public override string GetShortInfo()
    {
        return $"{Name} ({Color}) - {Price} грн. В наявності: {StockQuantity} шт.";
    }
}