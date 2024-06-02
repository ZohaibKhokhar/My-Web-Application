using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApplication1.Models;

[Authorize]
public class OrderController : Controller
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public OrderController(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int CartCount
    {
        get
        {
            return GetCartItemsFromSession().Count;
        }
    }

    public IActionResult Index()
    {
        ProductsRepository productsRepository = new ProductsRepository();
        return View(productsRepository.GetAll());
    }

    [HttpPost]
    public IActionResult AddToCart(int productId, int quantity)
    {
        // Update cart count
      
        var cartItems = GetCartItemsFromSession();
        var existingItem = cartItems.Find(item => item.ProductId == productId);

        if (existingItem != null)
        {
            existingItem.Quantity += quantity;
        }
        else
        {
            // Retrieve the product details from the database
            ProductsRepository productsRepository = new ProductsRepository();
            var prod = productsRepository.Get(productId);

            if (prod != null)
            {
                cartItems.Add(new CartItem
                {
                    ProductId = productId,
                    Quantity = quantity,
                    Product = prod
                });
            }
        }

        HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cartItems));

        return RedirectToAction("Index");
    }

    public IActionResult CheckOut()
    { 
        return View();
    }

    [HttpPost]
    public IActionResult PlaceOrder(Customer customer)
    {
        // Autogenerate a unique customer ID
        CustomerRepository cusRepo = new CustomerRepository();
        int id = cusRepo.getLastId();
        customer.CustomerId= ++id;
        
        //create a new customer
        customer.Email = User.Identity.Name; 
        cusRepo.AddCustomer(customer);

        // Autogenerate a unique customer ID
        OrderRepository orderRepo = new OrderRepository();
        id=orderRepo.GetMaxOrderId();
        id++;


        // Created a new order
        Order order = new Order
        {
            OrderId= id,
            CustomerId = customer.CustomerId,
            OrderDate = DateTime.Now,
            TotalPrice = CalculateTotalPrice() // Calculate the total price of the order
        };

        orderRepo.AddOrder(order);

        // Add all order items in repository 
        foreach (var item in GetCartItemsFromSession())
        {
            OrderItem orderItem = new OrderItem
            {
                OrderId = order.OrderId,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                Price = item.Product.DiscountedPrice
            };

            OrderItemRepository orderItemRepo = new OrderItemRepository();
            orderItemRepo.AddOrderItem(orderItem);
        }
        HttpContext.Session.Remove("Cart");
        return RedirectToAction("MyOrder","Home");
    }


    // all cart things using session
    public IActionResult Cart()
    {
        List<CartItem> cartItems = GetCartItemsFromSession(); // or GetCartItemsFromDatabase()
        return View(cartItems);
    }

    [HttpPost]
    public IActionResult RemoveFromCart(int productId)
    {
        // Update cart count
        var cartItems = GetCartItemsFromSession();
        var itemToRemove = cartItems.Find(item => item.ProductId == productId);

        if (itemToRemove != null)
        {
            cartItems.Remove(itemToRemove);
            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cartItems));
        }


        return RedirectToAction("Cart");
    }


    private List<CartItem> GetCartItemsFromSession()
    {
        var cartItems = new List<CartItem>();
        var session = HttpContext.Session;
        var cartSession = session.GetObject<List<CartItem>>("Cart");
        if (cartSession != null)
        {
            cartItems = cartSession;
        }
        return cartItems;
    }
    private decimal CalculateTotalPrice()
    {
        decimal total = 0;
        foreach (var item in GetCartItemsFromSession())
        {
            total += item.Quantity * item.Product.DiscountedPrice;
        }
        return total;
    }
   

}
 
   
