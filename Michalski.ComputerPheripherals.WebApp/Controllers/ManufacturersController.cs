using Microsoft.AspNetCore.Mvc;
using Michalski.ComputerPheripherals.BL;
using System.Linq;
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
                ModelState.AddModelError("Name", "Nazwa producenta nie mo\u017Ce by\u0107 pusta.");
            }
            return View("Create", newManufacturer);
        }

        // GET: Manufacturers/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manufacturer = _blc.GetManufacturers().FirstOrDefault(m => m.Id == id);
            if (manufacturer == null)
            {
                return NotFound();
            }

            ViewBag.ProductsCount = _blc.GetProducts().Count(p => p.ManufacturerId == id);
            return View(manufacturer);
        }

        // POST: Manufacturers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _blc.DeleteManufacturer(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
