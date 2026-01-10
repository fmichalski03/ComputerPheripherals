using Michalski.ComputerPheripherals.CORE;

namespace Michalski.ComputerPheripherals.INTERFACES
{
    public interface IProduct
    {
        int Id { get; set; }
        string Name { get; set; }
        decimal Price { get; set; }
        PeripheralType Type { get; set; }
        int ManufacturerId { get; set; }
        IManufacturer Manufacturer { get; set; }
    }
}