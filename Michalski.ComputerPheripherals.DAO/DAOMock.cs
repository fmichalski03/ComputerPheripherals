using System.Collections.Generic;
using System.Linq;
using Michalski.ComputerPheripherals.CORE;
using Michalski.ComputerPheripherals.INTERFACES;

namespace Michalski.ComputerPheripherals.DAO
{
    public class DAOMock : IDao
    {
        private List<IProduct> _products;
        private List<IManufacturer> _manufacturers;

        public DAOMock()
        {
            _manufacturers = new List<IManufacturer>
            {
                new Manufacturer { Id = 1, Name = "Logitech" },
                new Manufacturer { Id = 2, Name = "Razer" },
                new Manufacturer { Id = 3, Name = "SteelSeries" }
            };

            _products = new List<IProduct>
            {
                new Product { Id = 1, Name = "MX Master 3S", Price = 499.00m, Type = PeripheralType.Mouse, ManufacturerId = 1, Manufacturer = _manufacturers[0] },
                new Product { Id = 2, Name = "BlackWidow V4", Price = 799.00m, Type = PeripheralType.Keyboard, ManufacturerId = 2, Manufacturer = _manufacturers[1] },
                new Product { Id = 3, Name = "Arctis Nova Pro", Price = 1200.00m, Type = PeripheralType.Headset, ManufacturerId = 3, Manufacturer = _manufacturers[2] }
            };
        }

        public IEnumerable<IProduct> GetAllProducts() => _products;
        public IEnumerable<IManufacturer> GetAllManufacturers() => _manufacturers;

        public void AddProduct(IProduct product)
        {
            if (_products.Any())
                product.Id = _products.Max(p => p.Id) + 1;
            else
                product.Id = 1;
            
            _products.Add(product);
        }

        public void UpdateProduct(IProduct product)
        {
            var existing = _products.FirstOrDefault(p => p.Id == product.Id);
            if (existing != null)
            {
                existing.Name = product.Name;
                existing.Price = product.Price;
                existing.Type = product.Type;
                existing.ManufacturerId = product.ManufacturerId;
                existing.Manufacturer = product.Manufacturer;
            }
        }

        public void DeleteProduct(int productId)
        {
            var toRemove = _products.FirstOrDefault(p => p.Id == productId);
            if (toRemove != null) _products.Remove(toRemove);
        }

        public void AddManufacturer(IManufacturer manufacturer)
        {
            if (_manufacturers.Any())
                manufacturer.Id = _manufacturers.Max(m => m.Id) + 1;
            else
                manufacturer.Id = 1;
            
            _manufacturers.Add(manufacturer);
        }

        public IProduct CreateNewProduct() => new Product();
        public IManufacturer CreateNewManufacturer() => new Manufacturer();
    }
}