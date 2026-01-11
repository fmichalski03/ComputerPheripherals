using System.Collections.Generic;

namespace Michalski.ComputerPheripherals.DAOFile
{
    internal class Database
    {
        public List<Manufacturer> Manufacturers { get; set; } = new List<Manufacturer>();
        public List<Product> Products { get; set; } = new List<Product>();
    }
}
