using System.ComponentModel.DataAnnotations;

namespace Michalski.ComputerPheripherals.INTERFACES
{
    public interface IManufacturer
    {
        int Id { get; set; }
        [Display(Name = "Nazwa")]
        string Name { get; set; }
    }
}