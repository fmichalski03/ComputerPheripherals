using Michalski.ComputerPheripherals.CORE;
using Michalski.ComputerPheripherals.INTERFACES;

namespace Michalski.ComputerPheripherals.DAO
{
    public class Product : IProduct
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public PeripheralType Type { get; set; }
        public int ManufacturerId { get; set; }
        public IManufacturer Manufacturer { get; set; }
    }
}