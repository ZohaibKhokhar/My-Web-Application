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

    public IActionResult Index()
    {
        ProductsRepository productsRepository = new ProductsRepository();
        return View(productsRepository.GetAll());
    }

    [HttpPost]
    public IActionResult AddToCart(int productId, int quantity)
    {
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
        customer.Email = User.Identity.Name;
        CustomerRepository cusRepo=new CustomerRepository();
        cusRepo.AddCustomer(customer);
        return View(customer);
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
    public IActionResult Cart()
    {
        List<CartItem> cartItems = GetCartItemsFromSession(); // or GetCartItemsFromDatabase()
        return View(cartItems);
    }
    [HttpPost]
    public IActionResult RemoveFromCart(int productId)
    {
        var cartItems = GetCartItemsFromSession();
        var itemToRemove = cartItems.Find(item => item.ProductId == productId);

        if (itemToRemove != null)
        {
            cartItems.Remove(itemToRemove);
            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cartItems));
        }

        return RedirectToAction("Cart");
    }

}
 
   
