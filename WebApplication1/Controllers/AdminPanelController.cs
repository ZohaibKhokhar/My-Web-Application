using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    //[Authorize(Policy = "AdminPolicy")]
    public class AdminPanelController : Controller
    {
        private readonly ILogger<AdminPanelController> _logger;
        private readonly IWebHostEnvironment _env;

        public AdminPanelController(ILogger<AdminPanelController> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public IActionResult Index()
        {
            ProductsRepository productsRepository = new ProductsRepository();
            List<Products> products = productsRepository.GetAll();
            return View(products);
        }

        public IActionResult Edit(int id)
        {
            ProductsRepository repository = new ProductsRepository();   
            Products product = repository.Get(id); 
            return View(product);
        }
        [HttpPost]
        public IActionResult Edit(Products product, IFormFile ImageUrl)
        {
            if (ImageUrl != null && ImageUrl.Length > 0)
            {
                string wwwrootPath = _env.WebRootPath;
                string path = Path.Combine(wwwrootPath, "ProductImages");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string filePath = Path.Combine(path, ImageUrl.FileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    ImageUrl.CopyTo(fileStream);
                }

                product.ImageUrl = $"/ProductImages/{ImageUrl.FileName}";
            }

            ProductsRepository repo = new ProductsRepository();
            repo.Update(product);
            return RedirectToAction("Index", "AdminPanel");
        }

        public IActionResult Remove(int id)
        {
            ProductsRepository repo=new ProductsRepository();
            repo.DeleteById(id);
            return RedirectToAction("Index", "AdminPanel");
        }
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]

        public IActionResult Add(string productName, decimal price, decimal discountedPrice,int quantity, IFormFile img)
        {
            string wwwrootPath = _env.WebRootPath;
            string path = Path.Combine(wwwrootPath, "ProductImages");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            if (img != null && img.Length > 0)
            {

                string filePath = Path.Combine(path, img.FileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    img.CopyTo(fileStream);

                }

            }
            string imageUrl = $"/ProductImages/{img.FileName}";

            ProductsRepository repo = new ProductsRepository();
            repo.Add(new Products()
            {
                PName = productName,
                Price = price,
                DiscountedPrice = discountedPrice,
                Quantity = quantity,
                ImageUrl = imageUrl
            });
            return RedirectToAction("Add", "AdminPanel");
        }
        public IActionResult AllOrders()
        {
            OrderRepository repo = new OrderRepository();
            List<Order> orders=repo.GetAll();
            return View(orders);
        }

        public IActionResult OrderDetail(int id)
        {
            OrderItemRepository repo=new OrderItemRepository();
            List<OrderItem> items = new List<OrderItem>();
            items=repo.GetAllByOrderId(id);
            return View(items);
        }

        public IActionResult CustomerDetail(int id)
        {
            CustomerRepository repository = new CustomerRepository();
            return View(repository.GetCustomerById(id));
        }

        public IActionResult ProductDetail(int id)
        {
           ProductsRepository repository = new ProductsRepository();
            return View(repository.Get(id));
        }
        public IActionResult AllCustomers()
        {
            CustomerRepository repository=new CustomerRepository();
            return View(repository.GetAllCustomers());
        }

        public IActionResult ShowAll()
        {
            AppointmentRepository repo = new AppointmentRepository();
            return View(repo.GetAll());
        }
    }
}
