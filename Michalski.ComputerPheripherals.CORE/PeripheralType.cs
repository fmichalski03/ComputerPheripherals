using System.ComponentModel.DataAnnotations;

namespace Michalski.ComputerPheripherals.CORE
{
    public enum PeripheralType
    {
        [Display(Name = "Mysz")]
        Mouse,
        [Display(Name = "Klawiatura")]
        Keyboard,
        [Display(Name = "Monitor")]
        Monitor,
        [Display(Name = "Zestaw s\u0142uchawkowy")]
        Headset,
        [Display(Name = "Kamera internetowa")]
        Webcam,
        [Display(Name = "Drukarka")]
        Printer
    }
}
