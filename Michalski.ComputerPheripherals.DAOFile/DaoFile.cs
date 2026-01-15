using Michalski.ComputerPheripherals.INTERFACES;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Michalski.ComputerPheripherals.DAOFile
{
    public class DaoFile : IDao
    {
        private readonly string _filePath = "computer_peripherals_data.json";
        private Database _database;

        public DaoFile()
        {
            LoadDatabase();
        }

        private void LoadDatabase()
        {
            if (!File.Exists(_filePath))
            {
                _database = new Database();
                return;
            }

            var json = File.ReadAllText(_filePath);
            _database = JsonConvert.DeserializeObject<Database>(json);

            // Re-link manufacturers to products after deserialization
            foreach (var product in _database.Products)
            {
                product.Manufacturer = _database.Manufacturers.FirstOrDefault(m => m.Id == product.ManufacturerId);
            }
        }

        private void SaveDatabase()
        {
            var json = JsonConvert.SerializeObject(_database, Formatting.Indented);
            File.WriteAllText(_filePath, json);
        }

        public void AddManufacturer(IManufacturer manufacturer)
        {
            LoadDatabase();
            var newId = _database.Manufacturers.Any() ? _database.Manufacturers.Max(m => m.Id) + 1 : 1;
            manufacturer.Id = newId;
            if (manufacturer is Manufacturer daoManufacturer)
            {
                daoManufacturer.Id = newId;
                _database.Manufacturers.Add(daoManufacturer);
            }
            else
            {
                var newManufacturer = new Manufacturer { Id = newId, Name = manufacturer.Name };
                _database.Manufacturers.Add(newManufacturer);
            }
            SaveDatabase();
        }

        public void DeleteManufacturer(int manufacturerId)
        {
            LoadDatabase();
            var manufacturerToRemove = _database.Manufacturers.FirstOrDefault(m => m.Id == manufacturerId);
            if (manufacturerToRemove != null)
            {
                _database.Products.RemoveAll(p => p.ManufacturerId == manufacturerId);
                _database.Manufacturers.Remove(manufacturerToRemove);
                SaveDatabase();
            }
        }

        public void AddProduct(IProduct product)
        {
            LoadDatabase();
            var newId = _database.Products.Any() ? _database.Products.Max(p => p.Id) + 1 : 1;
            var newProduct = new Product
            {
                Id = newId,
                Name = product.Name,
                Price = product.Price,
                Type = product.Type,
                ManufacturerId = product.Manufacturer.Id,
                Manufacturer = product.Manufacturer
            };
            _database.Products.Add(newProduct);
            SaveDatabase();
        }

        public IManufacturer CreateNewManufacturer() => new Manufacturer();

        public IProduct CreateNewProduct() => new Product();

        public void DeleteProduct(int productId)
        {
            LoadDatabase();
            var productToRemove = _database.Products.FirstOrDefault(p => p.Id == productId);
            if (productToRemove != null)
            {
                _database.Products.Remove(productToRemove);
                SaveDatabase();
            }
        }

        public IEnumerable<IManufacturer> GetAllManufacturers()
        {
            LoadDatabase();
            return _database.Manufacturers;
        }

        public IEnumerable<IProduct> GetAllProducts()
        {
            LoadDatabase();
            return _database.Products;
        }

        public void UpdateProduct(IProduct product)
        {
            LoadDatabase();
            var productToUpdate = _database.Products.FirstOrDefault(p => p.Id == product.Id);
            if (productToUpdate != null)
            {
                productToUpdate.Name = product.Name;
                productToUpdate.Price = product.Price;
                productToUpdate.Type = product.Type;
                productToUpdate.ManufacturerId = product.Manufacturer.Id;
                productToUpdate.Manufacturer = product.Manufacturer;
                SaveDatabase();
            }
        }
    }
}
