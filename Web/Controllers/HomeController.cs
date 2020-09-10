using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using FruitSystem.Data.Classes;
using FruitSystem.Data.Repositories;
using System.Linq;

namespace FruitSystem.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductRepository _productRepository;

        public HomeController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public IActionResult Index()
        {
            List<Product> fruit = _productRepository.AllProducts.ToList();

            return View(fruit);
        }
    }
}