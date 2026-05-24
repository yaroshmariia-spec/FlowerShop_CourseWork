using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Linq;
using FlowersShop_CourseWork.Models;
using FlowersShop_CourseWork.Services;
using FlowersShop_CourseWork.ViewModels;

namespace FlowersShop_CourseWork.ViewModels
{
    public partial class CatalogViewModel : ViewModelBase
    {
        private readonly ObservableCollection<Flower> _allFlowers;
        
        private readonly FileService _fileService;
        private readonly string _filePath = "flowers_data.json";

        [ObservableProperty]
        private ObservableCollection<Flower> _flowers;

        [ObservableProperty]
        private string _searchText = string.Empty;
        
        [ObservableProperty]
        private Flower _selectedFlower;
        
        [ObservableProperty]
        private Flower _editingFlower;
        
        [ObservableProperty]
        private bool _isFormVisible;
        
        private bool _isCreatingNew;

        public CatalogViewModel()
        {
            _fileService = new FileService(_filePath);
            
            var loadedData = _fileService.LoadData();

            if (loadedData.Count > 0)
            {
                _allFlowers = new ObservableCollection<Flower>(loadedData);
            }
            else
            {
                _allFlowers = new ObservableCollection<Flower>
                {
                    new Flower { Name = "Червона троянда", Category = "Зрізані квіти", Price = 150, Color = "Червоний", StockQuantity = 50 },
                    new Flower { Name = "Орхідея Фаленопсис", Category = "Вазони", Price = 800, Color = "Білий", StockQuantity = 12 },
                    new Flower { Name = "Тюльпан", Category = "Зрізані квіти", Price = 80, Color = "Жовтий", StockQuantity = 100 }
                };
                _fileService.SaveData(_allFlowers);
            }
            
            Flowers = new ObservableCollection<Flower>(_allFlowers);
        }

        partial void OnSearchTextChanged(string value)
        {
            FilterFlowers();
        }

        private void FilterFlowers()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
                Flowers = new ObservableCollection<Flower>(_allFlowers);
            else
                Flowers = new ObservableCollection<Flower>(_allFlowers.Where(f => f.Name.ToLower().Contains(SearchText.ToLower())));
        }
        
        partial void OnSelectedFlowerChanged(Flower value)
        {
            if (value != null)
            {
                EditingFlower = new Flower
                {
                    Id = value.Id,
                    Name = value.Name,
                    Category = value.Category,
                    Price = value.Price,
                    Color = value.Color,
                    StockQuantity = value.StockQuantity
                };
                _isCreatingNew = false;
                IsFormVisible = true;
            }
        }

        [RelayCommand]
        private void AddNewFlower()
        {
            SelectedFlower = null;
            EditingFlower = new Flower { Name = "", Category = "Зрізані квіти", Price = 0, Color = "", StockQuantity = 1 };
            _isCreatingNew = true;
            IsFormVisible = true;
        }

        [RelayCommand]
        private void DeleteSelected()
        {
            if (SelectedFlower != null)
            {
                _allFlowers.Remove(SelectedFlower);
                _fileService.SaveData(_allFlowers);
                
                FilterFlowers();
                IsFormVisible = false;
            }
        }

        [RelayCommand]
        private void SaveFlower()
        {
            if (string.IsNullOrWhiteSpace(EditingFlower.Name)) return; 
            
            if (EditingFlower.Price == null || EditingFlower.Price < 0) return;
            if (EditingFlower.StockQuantity == null || EditingFlower.StockQuantity < 0) return;

            if (_isCreatingNew)
            {
                _allFlowers.Add(EditingFlower);
            }
            else
            {
                var original = _allFlowers.FirstOrDefault(f => f.Id == EditingFlower.Id);
                if (original != null)
                {
                    original.Name = EditingFlower.Name;
                    original.Category = EditingFlower.Category;
                    original.Price = EditingFlower.Price;
                    original.Color = EditingFlower.Color;
                    original.StockQuantity = EditingFlower.StockQuantity;
                }
            }
            _fileService.SaveData(_allFlowers);

            FilterFlowers();
            IsFormVisible = false;
            SelectedFlower = null; 
        }

        [RelayCommand]
        private void CancelEdit()
        {
            IsFormVisible = false;
            SelectedFlower = null;
        }
    }
}