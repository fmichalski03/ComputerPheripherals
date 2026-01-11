using Michalski.ComputerPheripherals.CORE;
using Michalski.ComputerPheripherals.INTERFACES;
using System.ComponentModel.DataAnnotations.Schema;

namespace Michalski.ComputerPheripherals.DAOSQL
{
    public class Product : IProduct
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public PeripheralType Type { get; set; }

        public int ManufacturerId { get; set; }

        [ForeignKey("ManufacturerId")]
        public virtual Manufacturer Manufacturer { get; set; }

        IManufacturer IProduct.Manufacturer 
        { 
            get => Manufacturer; 
            set => Manufacturer = value as Manufacturer; 
        }
    }
}
