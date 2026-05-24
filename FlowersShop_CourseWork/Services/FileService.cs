using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using FlowersShop_CourseWork.Models;

namespace FlowersShop_CourseWork.Services;

public class FileService
{
    private readonly string _filePath;

    public FileService(string path)
    {
        _filePath = path;
    }
    
    public List<Flower> LoadData()
    {
        if (!File.Exists(_filePath))
        {
            return new List<Flower>();
        }

        string json = File.ReadAllText(_filePath);
        
        return JsonSerializer.Deserialize<List<Flower>>(json) ?? new List<Flower>();
    }
    
    public void SaveData(IEnumerable<Flower> flowers)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        
        string json = JsonSerializer.Serialize(flowers, options);
        
        File.WriteAllText(_filePath, json);
    }
}
