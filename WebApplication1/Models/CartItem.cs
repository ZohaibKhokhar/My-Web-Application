namespace WebApplication1.Models
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public Products Product { get; set; } // Add this property
    }
}
