using Michalski.ComputerPheripherals.CORE;
using System.ComponentModel.DataAnnotations;

namespace Michalski.ComputerPheripherals.INTERFACES
{
    public interface IProduct
    {
        int Id { get; set; }
        [Display(Name = "Nazwa")]
        string Name { get; set; }
        [Display(Name = "Cena")]
        decimal Price { get; set; }
        [Display(Name = "Typ")]
        PeripheralType Type { get; set; }
        int ManufacturerId { get; set; }
        IManufacturer Manufacturer { get; set; }
    }
}