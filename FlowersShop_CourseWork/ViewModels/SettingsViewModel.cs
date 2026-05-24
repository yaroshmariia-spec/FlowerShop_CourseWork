using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Styling;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using FlowersShop_CourseWork.ViewModels;

namespace FlowersShop_CourseWork.ViewModels
{
    public partial class SettingsViewModel : ViewModelBase
    {
        [ObservableProperty]
        private bool _isDarkTheme;

        [ObservableProperty]
        private string _selectedLanguage = "Українська";
        
        public List<string> Languages { get; } = new List<string> { "Українська", "English" };

        public SettingsViewModel()
        {
            if (Application.Current != null)
            {
                IsDarkTheme = Application.Current.RequestedThemeVariant == ThemeVariant.Dark;
                
                var currentDict = Application.Current.Resources.MergedDictionaries.FirstOrDefault() as ResourceInclude;
                
                if (currentDict != null && currentDict.Source != null)
                {
                    if (currentDict.Source.ToString().Contains("en-US"))
                    {
                        _selectedLanguage = "English"; 
                    }
                    else
                    {
                        _selectedLanguage = "Українська";
                    }
                }
            }
        }

        partial void OnIsDarkThemeChanged(bool value)
        {
            if (Application.Current != null)
            {
                Application.Current.RequestedThemeVariant = value ? ThemeVariant.Dark : ThemeVariant.Light;
            }
        }
        partial void OnSelectedLanguageChanged(string value)
        {
            if (Application.Current == null) return;
            
            string dictionaryPath = value == "English" 
                ? "avares://FlowersShop_CourseWork/Resources/en-US.axaml" 
                : "avares://FlowersShop_CourseWork/Resources/uk-UA.axaml";
            
            var newDictionary = new ResourceInclude(new Uri("avares://FlowersShop_CourseWork/App.axaml"))
            {
                Source = new Uri(dictionaryPath)
            };
            
            Application.Current.Resources.MergedDictionaries[0] = newDictionary;
        }
    }
}