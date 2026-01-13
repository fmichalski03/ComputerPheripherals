using Microsoft.AspNetCore.Mvc;
using Michalski.ComputerPheripherals.BL;
using System.Threading.Tasks;

namespace Michalski.ComputerPheripherals.WebApp.Controllers
{
    public class ManufacturersController : Controller
    {
        private readonly BLC _blc;

        public ManufacturersController(BLC blc)
        {
            _blc = blc;
        }

        // GET: Manufacturers
        public IActionResult Index()
        {
            return View(_blc.GetManufacturers());
        }

        // GET: Manufacturers/Create
        public IActionResult Create()
        {
            return View(_blc.CreateManufacturer());
        }

        // POST: Manufacturers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create_Post()
        {
            var newManufacturer = _blc.CreateManufacturer();
            if (await TryUpdateModelAsync(newManufacturer, "", m => m.Name))
            {
                if (!string.IsNullOrWhiteSpace(newManufacturer.Name))
                {
                    _blc.AddManufacturer(newManufacturer);
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("Name", "Nazwa producenta nie może być pusta.");
            }
            return View("Create", newManufacturer);
        }
    }
}
