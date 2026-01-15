using Michalski.ComputerPheripherals.INTERFACES;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Michalski.ComputerPheripherals.DAOSQL
{
    public class DaoSql : IDao
    {
        public DaoSql()
        {
            using (var context = new PeripheralsDbContext())
            {
                context.Database.Migrate();
            }
        }

        public void AddManufacturer(IManufacturer manufacturer)
        {
            using (var context = new PeripheralsDbContext())
            {
                var newManufacturer = new Manufacturer { Name = manufacturer.Name };
                context.Manufacturers.Add(newManufacturer);
                context.SaveChanges();
            }
        }

        public void AddProduct(IProduct product)
        {
            using (var context = new PeripheralsDbContext())
            {
                var newProduct = new Product
                {
                    Name = product.Name,
                    Price = product.Price,
                    Type = product.Type,
                    ManufacturerId = product.ManufacturerId
                };
                context.Products.Add(newProduct);
                context.SaveChanges();
            }
        }

        public IManufacturer CreateNewManufacturer()
        {
            return new Manufacturer();
        }

        public IProduct CreateNewProduct()
        {
            return new Product();
        }

        public void DeleteManufacturer(int manufacturerId)
        {
            using (var context = new PeripheralsDbContext())
            {
                var manufacturerToDelete = context.Manufacturers.Find(manufacturerId);
                if (manufacturerToDelete != null)
                {
                    context.Manufacturers.Remove(manufacturerToDelete);
                    context.SaveChanges();
                }
            }
        }

        public void DeleteProduct(int productId)
        {
            using (var context = new PeripheralsDbContext())
            {
                var productToDelete = context.Products.Find(productId);
                if (productToDelete != null)
                {
                    context.Products.Remove(productToDelete);
                    context.SaveChanges();
                }
            }
        }

        public IEnumerable<IManufacturer> GetAllManufacturers()
        {
            using (var context = new PeripheralsDbContext())
            {
                return context.Manufacturers.ToList();
            }
        }

        public IEnumerable<IProduct> GetAllProducts()
        {
            using (var context = new PeripheralsDbContext())
            {
                // Używamy Include, aby załadować powiązanych producentów
                return context.Products.Include(p => p.Manufacturer).ToList();
            }
        }

        public void UpdateManufacturer(IManufacturer manufacturer)
        {
            using (var context = new PeripheralsDbContext())
            {
                var manufacturerToUpdate = context.Manufacturers.Find(manufacturer.Id);
                if (manufacturerToUpdate != null)
                {
                    manufacturerToUpdate.Name = manufacturer.Name;
                    context.SaveChanges();
                }
            }
        }

        public void UpdateProduct(IProduct product)
        {
            using (var context = new PeripheralsDbContext())
            {
                var productToUpdate = context.Products.Find(product.Id);
                if (productToUpdate != null)
                {
                    productToUpdate.Name = product.Name;
                    productToUpdate.Price = product.Price;
                    productToUpdate.Type = product.Type;
                    productToUpdate.ManufacturerId = product.Manufacturer.Id;
                    context.SaveChanges();
                }
            }
        }
    }
}
