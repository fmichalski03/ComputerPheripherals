using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Michalski.ComputerPheripherals.INTERFACES;

namespace Michalski.ComputerPheripherals.BL
{
    public class BLC
    {
        private IDao _dao;

        public BLC(string libraryName)
        {
            LoadDaoLibrary(libraryName);
        }

        private void LoadDaoLibrary(string libraryName)
        {
            // Wczytanie biblioteki (Reflection) - wymaganie 2.4
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, libraryName);
            
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Nie znaleziono pliku biblioteki danych: {path}");
            }

            Assembly assembly = Assembly.LoadFrom(path);

            // Znalezienie typu implementującego IDao
            Type daoType = assembly.GetTypes()
                .FirstOrDefault(t => typeof(IDao).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

            if (daoType == null)
                throw new Exception($"W bibliotece {libraryName} nie znaleziono implementacji IDao.");

            // Utworzenie instancji
            _dao = (IDao)Activator.CreateInstance(daoType);
        }

        public IEnumerable<IProduct> GetProducts() => _dao.GetAllProducts();
        public IEnumerable<IManufacturer> GetManufacturers() => _dao.GetAllManufacturers();

        public void AddProduct(IProduct product) => _dao.AddProduct(product);
        public void UpdateProduct(IProduct product) => _dao.UpdateProduct(product);
        public void DeleteProduct(int id) => _dao.DeleteProduct(id);
        public void AddManufacturer(IManufacturer manufacturer) => _dao.AddManufacturer(manufacturer);

        public IProduct CreateProduct() => _dao.CreateNewProduct();
        public IManufacturer CreateManufacturer() => _dao.CreateNewManufacturer();
    }
}