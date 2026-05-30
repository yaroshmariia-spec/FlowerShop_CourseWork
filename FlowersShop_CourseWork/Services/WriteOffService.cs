using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using FlowersShop_CourseWork.Models;

namespace FlowersShop_CourseWork.Services;

public class WriteOffService
{
    private readonly string _filePath = "writeoffs.json";

    public void SaveWriteOff(WriteOff writeOff)
    {
        var writeOffs = GetAllWriteOffs();
        writeOffs.Add(writeOff);
            
        var options = new JsonSerializerOptions { WriteIndented = true };
        File.WriteAllText(_filePath, JsonSerializer.Serialize(writeOffs, options));
    }

    public List<WriteOff> GetAllWriteOffs()
    {
        if (!File.Exists(_filePath)) return new List<WriteOff>();
            
        try
        {
            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<WriteOff>>(json) ?? new List<WriteOff>();
        }
        catch
        {
            return new List<WriteOff>();
        }
    }
}