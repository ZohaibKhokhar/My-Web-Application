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

        public IActionResult MyOrder()
        {
            List<int> customerIds = new List<int>();
            CustomerRepository repository = new CustomerRepository();
            customerIds = repository.GetCustomerId(User.Identity.Name);
            List<Order> listOrders = new List<Order>();
            if (customerIds.Count != 0)
            {
                OrderRepository repo = new OrderRepository();
                foreach (var cusId in customerIds)
                { 
                    int orderId = repo.GetOrderIdByCustomerId(cusId);
                    Order order=new Order();
                    order=repo.getOrderById(orderId);
                    listOrders.Add(order);
                }

                return View(listOrders);
            }
            else
            {

                return View(listOrders);
            }
        }
        public IActionResult YourOrder(int id)
        {
            OrderItemRepository repo = new OrderItemRepository();
            List<OrderItem> items = new List<OrderItem>();
            items = repo.GetAllByOrderId(id);
            return View(items);
        }

        public IActionResult YourDetail(int id)
        {
            CustomerRepository repository = new CustomerRepository();
            return View(repository.GetCustomerById(id));
        }

    }
}
