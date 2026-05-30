using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlowersShop_CourseWork.Models;
using FlowersShop_CourseWork.Services;

namespace FlowersShop_CourseWork.ViewModels;

public partial class AnalyticsViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _reportText = "Натисніть кнопку нижче, щоб розпочати інтелектуальний аналіз складу.";

    [ObservableProperty] private ObservableCollection<WriteOff> _writeOffHistory;

    public AnalyticsViewModel()
    {
        var writeOffService = new WriteOffService();
        WriteOffHistory = new ObservableCollection<WriteOff>(writeOffService.GetAllWriteOffs());
    }

    [RelayCommand]
    private void RunSmartAnalysis()
    {
        var fileService = new FileService("flowers_data.json");
        var inventory = fileService.LoadData();
        var writeOffService = new WriteOffService();

        var reportBuilder = new StringBuilder();
        reportBuilder.AppendLine("=== АНАЛІТИЧНИЙ ЗВІТ ПРО СПИСАННЯ ПРОДУКЦІЇ ===");
        reportBuilder.AppendLine($"Дата генерації: {DateTime.Now}");
        reportBuilder.AppendLine($"Адміністратор: {Session.CurrentUser?.Email ?? "System"}\n");
        reportBuilder.AppendLine("Результати перевірки складу:");

        int totalWrittenOff = 0;
        bool changesMade = false;

        foreach (var flower in inventory)
        {
            int quantityToWriteOff = 0;
            string reason = "";
            if (flower.StockQuantity > 50)
            {
                quantityToWriteOff = 2;
                reason = "Damaged";
            }
            else if (flower.Price < 100 && flower.StockQuantity > 5 && flower.StockQuantity < 20)
            {
                quantityToWriteOff = 1;
                reason = "Expired";
            }
            
            if (quantityToWriteOff > 0 && flower.StockQuantity >= quantityToWriteOff)
            {
                flower.StockQuantity -= quantityToWriteOff; 
                totalWrittenOff += quantityToWriteOff;
                changesMade = true;
                
                var writeOff = new WriteOff
                {
                    ProductId = flower.Name,
                    Quantity = quantityToWriteOff,
                    Reason = reason,
                    UserId = Session.CurrentUser?.Email ?? "System"
                };

                writeOffService.SaveWriteOff(writeOff); 
                WriteOffHistory.Add(writeOff); 

                reportBuilder.AppendLine(
                    $"- [СПИСАНО] {flower.Name} | К-ть: {quantityToWriteOff} шт. | Причина: {reason}");
            }
        }

        if (changesMade)
        {
            fileService.SaveData(inventory);
            reportBuilder.AppendLine($"\nЗагалом списано одиниць товару: {totalWrittenOff}. Базу даних оновлено.");
        }
        else
        {
            reportBuilder.AppendLine("\nУсі товари в нормі. Прострочених або пошкоджених квітів не виявлено.");
        }
        ReportText = reportBuilder.ToString();
        File.WriteAllText("report.txt", ReportText);
        
        File.AppendAllText("log.txt",
            $"{DateTime.Now} - Admin провів аналіз списання. Списано: {totalWrittenOff} шт.\n");
    }
}