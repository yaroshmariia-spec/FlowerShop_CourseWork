using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using FlowersShop_CourseWork.Models;

namespace FlowersShop_CourseWork.Services;

public class SaleService
{
    private readonly string _filePath = "sales.json";

    public void SaveSale(Sale sale)
    {
        var sales = GetAllSales();
        sales.Add(sale);
            
        var options = new JsonSerializerOptions { WriteIndented = true };
        File.WriteAllText(_filePath, JsonSerializer.Serialize(sales, options));
    }

    public List<Sale> GetAllSales()
    {
        if (!File.Exists(_filePath)) return new List<Sale>();
            
        try
        {
            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<Sale>>(json) ?? new List<Sale>();
        }
        catch
        {
            return new List<Sale>();
        }
    }
}