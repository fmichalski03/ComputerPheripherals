using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Michalski.ComputerPheripherals.BL;
using Michalski.ComputerPheripherals.INTERFACES;
using System.Linq;
using System.Threading.Tasks;

namespace Michalski.ComputerPheripherals.WebApp.Controllers
{
    public class ProductsController : Controller
    {
        private readonly BLC _blc;

        public ProductsController(BLC blc)
        {
            _blc = blc;
        }

        // GET: Products
        public IActionResult Index(string searchString)
        {
            var products = _blc.GetProducts();

            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase));
            }

            return View(products);
        }

        // GET: Products/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _blc.GetProducts().FirstOrDefault(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewBag.Manufacturers = new SelectList(_blc.GetManufacturers(), "Id", "Name");
            var newProduct = _blc.CreateProduct();
            return View(newProduct);
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create_Post(IFormCollection form)
        {
            var newProduct = _blc.CreateProduct();

            // Ręczne przypisanie wartości z formularza
            newProduct.Name = form["Name"];
            if (decimal.TryParse(form["Price"], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var price))
            {
                newProduct.Price = price;
            }
            if (Enum.TryParse<Michalski.ComputerPheripherals.CORE.PeripheralType>(form["Type"], out var type))
            {
                newProduct.Type = type;
            }
            if (int.TryParse(form["ManufacturerId"], out var manufacturerId))
            {
                newProduct.ManufacturerId = manufacturerId;
            }

            // Ręczna walidacja
            if (string.IsNullOrWhiteSpace(newProduct.Name))
            {
                ModelState.AddModelError("Name", "Nazwa produktu jest wymagana.");
            }
            if (newProduct.ManufacturerId == 0)
            {
                ModelState.AddModelError("ManufacturerId", "Należy wybrać producenta.");
            }

            if (ModelState.IsValid)
            {
                var manufacturer = _blc.GetManufacturers().FirstOrDefault(m => m.Id == newProduct.ManufacturerId);
                if (manufacturer != null)
                {
                    newProduct.Manufacturer = manufacturer;
                    _blc.AddProduct(newProduct);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("ManufacturerId", $"Wybrany producent (ID: {newProduct.ManufacturerId}) jest nieprawidłowy.");
                }
            }

            ViewBag.Manufacturers = new SelectList(_blc.GetManufacturers(), "Id", "Name", newProduct.ManufacturerId);
            return View("Create", newProduct);
        }

        // GET: Products/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _blc.GetProducts().FirstOrDefault(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            ViewBag.Manufacturers = new SelectList(_blc.GetManufacturers(), "Id", "Name", product.ManufacturerId);
            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id)
        {
            var productToUpdate = _blc.GetProducts().FirstOrDefault(p => p.Id == id);
            if(productToUpdate == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync(productToUpdate, "", p => p.Name, p => p.Price, p => p.Type, p => p.ManufacturerId))
            {
                 var manufacturer = _blc.GetManufacturers().FirstOrDefault(m => m.Id == productToUpdate.ManufacturerId);
                if (manufacturer != null)
                {
                    productToUpdate.Manufacturer = manufacturer;
                    _blc.UpdateProduct(productToUpdate);
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("ManufacturerId", "Selected manufacturer is invalid.");
            }

            ViewBag.Manufacturers = new SelectList(_blc.GetManufacturers(), "Id", "Name", productToUpdate.ManufacturerId);
            return View(productToUpdate);
        }

        // GET: Products/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _blc.GetProducts().FirstOrDefault(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _blc.DeleteProduct(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
