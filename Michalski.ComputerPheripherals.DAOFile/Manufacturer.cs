using Michalski.ComputerPheripherals.INTERFACES;

namespace Michalski.ComputerPheripherals.DAOFile
{
    public class Manufacturer : IManufacturer
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
