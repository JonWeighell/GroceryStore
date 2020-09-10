using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using FruitSystem.Data.Classes;
using FruitSystem.Data.Repositories;
using FruitSystem.Web.Classes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICsvHelper<Product, ProductCsvMap> _csvHelper;

        public AdminController(
            IProductRepository productRepository, 
            ICsvHelper<Product, ProductCsvMap> csvHelper
        )
        {
            _productRepository = productRepository;
            _csvHelper = csvHelper;
        }

        public IActionResult Index()
        {
            List<Product> products = _productRepository.AllProducts.ToList();

            return View(products);
        }

        [HttpGet]
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
                return NotFound();

            Product product = _productRepository.GetById(id.Value);

            if (product == null)
                return NotFound();

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid id)
        {
            Product product = _productRepository.GetById(id);

            if (await TryUpdateModelAsync<Product>(product))
            {
                product.UpdatedDated = DateTime.Now;

                _productRepository.Update(product);
                _productRepository.SaveChanges();

                TempData["Success"] = "Product changes saved successfully";

                return RedirectToAction("Index");
            }
            else
            {
                TempData["Error"] = "Please check the product details";

                return View(product);
            }
        }

        public IActionResult Upload(IFormFile file)
        {
            if (_csvHelper.CsvIsValid(file))
            {
                _productRepository.DeleteAllProducts();

                IEnumerable<Product> productsToAdd = _csvHelper.ReadFromCsv(file);

                _productRepository.AddProducts(productsToAdd);
                _productRepository.SaveChanges();

                TempData["Success"] = $"{productsToAdd.Count()} products successfully imported";
            }
            else
                TempData["Error"] = "Please choose a valid .csv file";

            return RedirectToAction("Index");
        }

        public IActionResult Download()
        {
            StringWriter stringWriter = new StringWriter();

            using (CsvWriter csvWriter = new CsvWriter(stringWriter, CultureInfo.InvariantCulture))
            {
                csvWriter.Configuration.RegisterClassMap<ProductCsvMap>();
                csvWriter.WriteRecords(_productRepository.AllProducts);
            }

            return File(Encoding.ASCII.GetBytes(stringWriter.ToString()), "text/csv", "products.csv");
        }
    }
}