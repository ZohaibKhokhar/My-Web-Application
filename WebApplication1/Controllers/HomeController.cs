using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ProductsRepository productsRepository = new ProductsRepository();
            return View(productsRepository.GetAll());
      
        }

      
        public IActionResult AboutUs()
        {
            return View();
        }


		public IActionResult ContactUs()
		{
			return View();
		}
	}
}
