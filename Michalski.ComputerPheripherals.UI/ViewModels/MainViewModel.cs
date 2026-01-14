using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using System.Windows.Input;
using Michalski.ComputerPheripherals.BL;
using Michalski.ComputerPheripherals.CORE;
using Michalski.ComputerPheripherals.INTERFACES;

namespace Michalski.ComputerPheripherals.UI
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly BLC _blc;
        private ObservableCollection<IProduct> _products;
        private ObservableCollection<IManufacturer> _manufacturers;
        private IProduct _selectedProduct;
        private string _statusMessage;
        private string _filterText;

        public event PropertyChangedEventHandler PropertyChanged;

        public MainViewModel()
        {
            _blc = new BLC();
            LoadData();

            AddCommand = new RelayCommand(AddProduct);
            UpdateCommand = new RelayCommand(UpdateProduct, CanModify);
            DeleteCommand = new RelayCommand(DeleteProduct, CanModify);
            AddManufacturerCommand = new RelayCommand(AddManufacturer);
        }

        private void LoadData()
        {
            Products = new ObservableCollection<IProduct>(_blc.GetProducts());
            ProductsView = CollectionViewSource.GetDefaultView(Products);
            ProductsView.Filter = FilterProducts;
            Manufacturers = new ObservableCollection<IManufacturer>(_blc.GetManufacturers());
        }

        public ObservableCollection<IProduct> Products
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<IManufacturer> Manufacturers
        {
            get => _manufacturers;
            set
            {
                _manufacturers = value;
                OnPropertyChanged();
            }
        }

        public IProduct SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged();
            }
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                _statusMessage = value;
                OnPropertyChanged();
            }
        }

        public ICollectionView ProductsView { get; private set; }

        public string FilterText
        {
            get => _filterText;
            set
            {
                _filterText = value;
                OnPropertyChanged();
                ProductsView?.Refresh();
            }
        }

        public IEnumerable<PeripheralType> PeripheralTypes => Enum.GetValues(typeof(PeripheralType)).Cast<PeripheralType>();

        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand AddManufacturerCommand { get; }

        private bool CanModify(object obj) => SelectedProduct != null;

        private bool FilterProducts(object obj)
        {
            if (obj is IProduct product)
            {
                return string.IsNullOrWhiteSpace(FilterText) ||
                       (product.Name != null && product.Name.IndexOf(FilterText, StringComparison.OrdinalIgnoreCase) >= 0);
            }
            return false;
        }

        private void AddProduct(object obj)
        {
            var newProduct = _blc.CreateProduct();
            newProduct.Name = "Nowy produkt";
            newProduct.Price = 0;
            newProduct.Type = PeripheralType.Mouse;

            var defaultMan = Manufacturers.FirstOrDefault();
            if (defaultMan != null)
            {
                newProduct.Manufacturer = defaultMan;
                newProduct.ManufacturerId = defaultMan.Id;
            }

            _blc.AddProduct(newProduct);
            Products.Add(newProduct);
            SelectedProduct = newProduct;
            StatusMessage = "Dodano nowy produkt. Wype�nij dane.";
        }

        private void UpdateProduct(object obj)
        {
            if (SelectedProduct == null) return;

            if (string.IsNullOrWhiteSpace(SelectedProduct.Name) || SelectedProduct.Name.Length < 2)
            {
                StatusMessage = "B��d: Nazwa jest zbyt kr�tka!";
                return;
            }

            _blc.UpdateProduct(SelectedProduct);
            StatusMessage = "Zapisano zmiany.";
        }

        private void DeleteProduct(object obj)
        {
            if (SelectedProduct == null) return;

            _blc.DeleteProduct(SelectedProduct.Id);
            Products.Remove(SelectedProduct);
            SelectedProduct = null;
            StatusMessage = "Usuni�to produkt.";
        }

        private void AddManufacturer(object obj)
        {
            var man = _blc.CreateManufacturer();
            man.Name = "Nowy Producent " + (Manufacturers.Count + 1);
            _blc.AddManufacturer(man);
            Manufacturers.Add(man);
            StatusMessage = "Dodano nowego producenta.";
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}