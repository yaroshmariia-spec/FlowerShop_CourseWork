using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using Avalonia;
using Avalonia.Controls;
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
        reportBuilder.AppendLine(GetLocalizedString("ReportTitle")); 
        reportBuilder.AppendLine($"{GetLocalizedString("ReportDate")}: {DateTime.Now}");
        reportBuilder.AppendLine($"{GetLocalizedString("ReportAdmin")}: {Session.CurrentUser?.Email ?? "System"}\n");
        reportBuilder.AppendLine(GetLocalizedString("ReportCheckResults"));

        int totalWrittenOff = 0;
        bool changesMade = false;

        foreach (var flower in inventory)
        {
            int quantityToWriteOff = 0;
            string reason = "";
            string reasonKey = "";
            if (flower.StockQuantity > 50)  
            {
                quantityToWriteOff = 2;
                reason = "Damaged";
                reasonKey = "ReasonDamaged";
            }
            else if (flower.Price < 100 && flower.StockQuantity > 5 && flower.StockQuantity < 20)
            {
                quantityToWriteOff = 1;
                reason = "Expired";
                reasonKey = "ReasonExpired";    
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
                string writtenOffWord = GetLocalizedString("ReportWrittenOffWord");
                string qtyWord = GetLocalizedString("ReportQtyWord");
                string reasonWord = GetLocalizedString("ReportReasonWord");
                string translatedReason = GetLocalizedString(reasonKey);

                reportBuilder.AppendLine($"- [СПИСАНО] {flower.Name} | К-ть: {quantityToWriteOff} шт. | Причина: {reason}");
            }
        }

        if (changesMade)
        {
            fileService.SaveData(inventory);
            string totalTemplate = GetLocalizedString("ReportTotalWrittenOff");
            reportBuilder.AppendLine($"\n{string.Format(totalTemplate, totalWrittenOff)}");
        }
        else
        {
            reportBuilder.AppendLine($"\n{GetLocalizedString("ReportAllGood")}");
        }
        ReportText = reportBuilder.ToString();
        File.WriteAllText("report.txt", ReportText);
        
        string logTemplate = GetLocalizedString("LogAnalysisPerformed");
        File.AppendAllText("log.txt", $"{DateTime.Now} - {string.Format(logTemplate, "Admin", totalWrittenOff)}\n");
    }
    private string GetLocalizedString(string key)
    {
        if (Application.Current != null && 
            Application.Current.TryFindResource(key, out object resource) && 
            resource is string translatedText)
        {
            return translatedText;
        }
        
        return key; 
    }
}