using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlowersShop_CourseWork.Models;
using FlowersShop_CourseWork.Services;

namespace FlowersShop_CourseWork.ViewModels;

public partial class StorefrontViewModel : ViewModelBase
{
    private ObservableCollection<Flower> _allFlowers;
    
    [ObservableProperty] private ObservableCollection<Flower> _products;
    [ObservableProperty]
    private ObservableCollection<string> _categories;
    
    private string _selectedCategory = "Усі";
    public string SelectedCategory
    {
        get => _selectedCategory;
        set
        {
            SetProperty(ref _selectedCategory, value);
            UpdateDisplayList();
        }
    }
    private string _searchText = "";

    public string SearchText
    {
        get => _searchText;
        set
        {
            SetProperty(ref _searchText, value);
            UpdateDisplayList();
        }
    }

    public StorefrontViewModel()
    {
        var fileService = new FileService("flowers_data.json");
        _allFlowers = new ObservableCollection<Flower>(fileService.LoadData());
            
        LoadUniqueCategories();
        UpdateDisplayList();
    }
    private void LoadUniqueCategories()
    {
        var uniqueCategories = new HashSet<string> { "Усі" };
            
        foreach (var flower in _allFlowers)
        {
            if (!string.IsNullOrWhiteSpace(flower.Category))
            {
                uniqueCategories.Add(flower.Category);
            }
        }

        Categories = new ObservableCollection<string>(uniqueCategories);
    }
    [RelayCommand]
    private void AddToCart(Flower item)
    {
        if (item != null)
        {
            CartService.Add(item);
        }
    }
    private void UpdateDisplayList()
    {
        var filteredList = new List<Flower>();

        foreach (var flower in _allFlowers)
        {
            if (SelectedCategory != "Усі" && flower.Category != SelectedCategory)
                continue;
            
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                if (flower.Name.ToLower().Contains(SearchText.ToLower()))
                {
                    filteredList.Add(flower);
                    continue;
                }

                string[] words = flower.Name.Split(' ');
                bool isFuzzyMatch = false;
                foreach (var word in words)
                {
                    int dist = SearchHelper.LevenshteinDistance(word, SearchText);
                    if ((word.Length > 4 && dist <= 2) || (word.Length <= 4 && dist <= 1))
                    {
                        isFuzzyMatch = true;
                        break;
                    }
                }

                if (!isFuzzyMatch) continue;
            }
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                filteredList.Add(flower);
            }
        }
        if (filteredList.Count > 1)
        {
            SearchHelper.QuickSortFlowers(filteredList, 0, filteredList.Count - 1);
        }
        Products = new ObservableCollection<Flower>(filteredList);
    }
}