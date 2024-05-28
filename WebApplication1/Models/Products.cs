using Humanizer;

namespace WebApplication1.Models
{
    public class Products
    {
        public int ID { get; set; }
        public String PName { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountedPrice { get; set; }

        public int Quantity { get; set; }
        public string ImageUrl { get; set; }

    }
}
