using System.Collections.Generic;

namespace Michalski.ComputerPheripherals.INTERFACES
{
    public interface IDao
    {
        IEnumerable<IProduct> GetAllProducts();
        IEnumerable<IManufacturer> GetAllManufacturers();
        
        void AddProduct(IProduct product);
        void UpdateProduct(IProduct product);
        void DeleteProduct(int productId);

        void AddManufacturer(IManufacturer manufacturer);
        void DeleteManufacturer(int manufacturerId);
        // Metody edycji producenta mozna dodac wg potrzeb.
        
        IProduct CreateNewProduct(); // Factory method dla tworzenia instancji w UI/BL
        IManufacturer CreateNewManufacturer();
    }
}
