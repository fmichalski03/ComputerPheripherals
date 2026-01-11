using Michalski.ComputerPheripherals.CORE;
using Michalski.ComputerPheripherals.INTERFACES;
using Newtonsoft.Json;

namespace Michalski.ComputerPheripherals.DAOFile
{
    public class Product : IProduct
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public PeripheralType Type { get; set; }
        public int ManufacturerId { get; set; }

        [JsonIgnore]
        public IManufacturer Manufacturer { get; set; }
    }
}
