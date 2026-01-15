using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Configuration;
using Michalski.ComputerPheripherals.INTERFACES;

namespace Michalski.ComputerPheripherals.BL
{
    public class BLC
    {
        private IDao _dao;

        public BLC()
        {
            string libraryName = ConfigurationManager.AppSettings["DAOLibraryName"];
            if (string.IsNullOrEmpty(libraryName))
                throw new Exception("Brak klucza 'DAOLibraryName' w pliku konfiguracyjnym (App.config).");
            
            LoadDaoLibrary(libraryName);
        }

        private void LoadDaoLibrary(string libraryName)
        {
            try
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, libraryName);
                if (!File.Exists(path))
                {
                    throw new FileNotFoundException($"Nie znaleziono pliku biblioteki danych: {path}");
                }

                Assembly assembly = Assembly.LoadFrom(path);
                
                // Kluczowy moment - próba załadowania typów
                Type[] types = assembly.GetTypes();

                Type daoType = types.FirstOrDefault(t => typeof(IDao).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

                if (daoType == null)
                    throw new Exception($"W bibliotece {libraryName} nie znaleziono klasy implementującej IDao.");

                _dao = (IDao)Activator.CreateInstance(daoType);
            }
            catch (ReflectionTypeLoadException ex)
            {
                // To jest najważniejsza część diagnostyczna
                var errorMessages = ex.LoaderExceptions.Select(e => e.Message);
                var fullMessage = string.Join("\n", errorMessages);
                throw new Exception(
                    $"Błąd ładowania typów z biblioteki {libraryName}. Prawdopodobnie brakuje jakiejś zależności (innego pliku .dll) w katalogu wynikowym aplikacji. Szczegóły:\n{fullMessage}", 
                    ex
                );
            }
            catch (Exception ex)
            {
                throw new Exception($"Wystąpił ogólny błąd podczas ładowania biblioteki {libraryName}: {ex.Message}", ex);
            }
        }

        public IEnumerable<IProduct> GetProducts() => _dao.GetAllProducts();
        public IEnumerable<IManufacturer> GetManufacturers() => _dao.GetAllManufacturers();

        public void AddProduct(IProduct product) => _dao.AddProduct(product);
        public void UpdateProduct(IProduct product) => _dao.UpdateProduct(product);
        public void DeleteProduct(int id) => _dao.DeleteProduct(id);
        public void AddManufacturer(IManufacturer manufacturer) => _dao.AddManufacturer(manufacturer);
        public void DeleteManufacturer(int id) => _dao.DeleteManufacturer(id);

        public IProduct CreateProduct() => _dao.CreateNewProduct();
        public IManufacturer CreateManufacturer() => _dao.CreateNewManufacturer();
    }
}
