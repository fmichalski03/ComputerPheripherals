using Michalski.ComputerPheripherals.INTERFACES;
using System.Collections.Generic;

namespace Michalski.ComputerPheripherals.DAOSQL
{
    public class Manufacturer : IManufacturer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
