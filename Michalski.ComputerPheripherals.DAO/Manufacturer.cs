using Michalski.ComputerPheripherals.INTERFACES;

namespace Michalski.ComputerPheripherals.DAO
{
    public class Manufacturer : IManufacturer
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}