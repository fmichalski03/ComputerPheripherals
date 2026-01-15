using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
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
        private IManufacturer _selectedManufacturer;
        private IManufacturer _selectedNewManufacturer;
        private string _newManufacturerName;
        private string _newProductName;
        private string _newProductPrice;
        private PeripheralType _newProductType;
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
            DeleteManufacturerCommand = new RelayCommand(DeleteManufacturer, CanDeleteManufacturer);
        }

        private void LoadData()
        {
            Products = new ObservableCollection<IProduct>(_blc.GetProducts());
            ProductsView = CollectionViewSource.GetDefaultView(Products);
            ProductsView.Filter = FilterProducts;
            Manufacturers = new ObservableCollection<IManufacturer>(_blc.GetManufacturers());
            SelectedNewManufacturer = Manufacturers.FirstOrDefault();
            NewProductType = PeripheralType.Mouse;
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

        public IManufacturer SelectedManufacturer
        {
            get => _selectedManufacturer;
            set
            {
                _selectedManufacturer = value;
                OnPropertyChanged();
            }
        }

        public IManufacturer SelectedNewManufacturer
        {
            get => _selectedNewManufacturer;
            set
            {
                _selectedNewManufacturer = value;
                OnPropertyChanged();
            }
        }

        public string NewManufacturerName
        {
            get => _newManufacturerName;
            set
            {
                _newManufacturerName = value;
                OnPropertyChanged();
            }
        }

        public string NewProductName
        {
            get => _newProductName;
            set
            {
                _newProductName = value;
                OnPropertyChanged();
            }
        }

        public string NewProductPrice
        {
            get => _newProductPrice;
            set
            {
                _newProductPrice = value;
                OnPropertyChanged();
            }
        }

        public PeripheralType NewProductType
        {
            get => _newProductType;
            set
            {
                _newProductType = value;
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
        public ICommand DeleteManufacturerCommand { get; }

        private bool CanModify(object obj) => SelectedProduct != null;

        private bool CanDeleteManufacturer(object obj) => SelectedManufacturer != null;

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
            var name = (NewProductName ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                StatusMessage = "Podaj nazw\u0119 produktu.";
                return;
            }

            if (!TryParsePrice(NewProductPrice, out var price))
            {
                StatusMessage = "Podaj poprawn\u0105 cen\u0119 produktu.";
                return;
            }

            if (SelectedNewManufacturer == null)
            {
                StatusMessage = "Wybierz producenta.";
                return;
            }

            var newProduct = _blc.CreateProduct();
            newProduct.Name = name;
            newProduct.Price = price;
            newProduct.Type = NewProductType;
            newProduct.Manufacturer = SelectedNewManufacturer;
            newProduct.ManufacturerId = SelectedNewManufacturer.Id;

            _blc.AddProduct(newProduct);
            Products.Add(newProduct);
            SelectedProduct = newProduct;
            NewProductName = string.Empty;
            NewProductPrice = string.Empty;
            StatusMessage = "Dodano nowy produkt.";
        }

        private void UpdateProduct(object obj)
        {
            if (SelectedProduct == null) return;

            if (string.IsNullOrWhiteSpace(SelectedProduct.Name) || SelectedProduct.Name.Length < 2)
            {
                StatusMessage = "B\u0142\u0105d: Nazwa jest zbyt kr\u00F3tka!";
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
            StatusMessage = "Usuni\u0119to produkt.";
        }

        private void AddManufacturer(object obj)
        {
            var name = (NewManufacturerName ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                StatusMessage = "Podaj nazw\u0119 producenta.";
                return;
            }

            var man = _blc.CreateManufacturer();
            man.Name = name;
            _blc.AddManufacturer(man);
            Manufacturers.Add(man);
            SelectedManufacturer = man;
            SelectedNewManufacturer = man;
            NewManufacturerName = string.Empty;
            StatusMessage = "Dodano nowego producenta.";
        }

        private void DeleteManufacturer(object obj)
        {
            if (SelectedManufacturer == null) return;

            var manufacturerId = SelectedManufacturer.Id;
            var productsToRemove = Products.Where(p => p.ManufacturerId == manufacturerId).ToList();
            foreach (var product in productsToRemove)
            {
                Products.Remove(product);
            }

            if (SelectedProduct != null && SelectedProduct.ManufacturerId == manufacturerId)
            {
                SelectedProduct = null;
            }

            _blc.DeleteManufacturer(manufacturerId);
            Manufacturers.Remove(SelectedManufacturer);
            SelectedManufacturer = null;

            if (SelectedNewManufacturer != null && SelectedNewManufacturer.Id == manufacturerId)
            {
                SelectedNewManufacturer = Manufacturers.FirstOrDefault();
            }

            StatusMessage = productsToRemove.Count == 0
                ? "Usuni\u0119to producenta."
                : $"Usuni\u0119to producenta i {productsToRemove.Count} powi\u0105zane produkty.";
        }

        private static bool TryParsePrice(string priceString, out decimal price)
        {
            price = 0;
            if (string.IsNullOrWhiteSpace(priceString))
            {
                return false;
            }

            var numberStyles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign;
            var currentCulture = CultureInfo.CurrentCulture;
            if (decimal.TryParse(priceString, numberStyles, currentCulture, out price))
            {
                return true;
            }

            var polishCulture = new CultureInfo("pl-PL");
            if (!Equals(currentCulture, polishCulture)
                && decimal.TryParse(priceString, numberStyles, polishCulture, out price))
            {
                return true;
            }

            return decimal.TryParse(priceString, numberStyles, CultureInfo.InvariantCulture, out price);
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
