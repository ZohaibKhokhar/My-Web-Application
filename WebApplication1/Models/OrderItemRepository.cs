using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace WebApplication1.Models
{
    public class OrderItemRepository
    {


        public const string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=thisdb;Integrated Security=True;";

      
        public void AddOrderItem(OrderItem orderItem)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("INSERT INTO OrderItem (OrderId, ProductId, Quantity, Price) VALUES (@OrderId, @ProductId, @Quantity, @Price)", connection))
                {
                    command.Parameters.AddWithValue("@OrderId", orderItem.OrderId);
                    command.Parameters.AddWithValue("@ProductId", orderItem.ProductId);
                    command.Parameters.AddWithValue("@Quantity", orderItem.Quantity);
                    command.Parameters.AddWithValue("@Price", orderItem.Price);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
        public List<OrderItem> GetAllOrderItems()
        {
            List<OrderItem> orderItems = new List<OrderItem>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SELECT * FROM OrderItem", connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        OrderItem orderItem = new OrderItem
                        {
                            Id = reader.GetInt32(0),
                            OrderId = reader.GetInt32(1),
                            ProductId = reader.GetInt32(2),
                            Quantity = reader.GetInt32(3),
                            Price = reader.GetDecimal(4)
                        };

                        orderItems.Add(orderItem);
                    }
                }
            }

            return orderItems;
        }
        public List<OrderItem> GetAllByOrderId(int orderId)
        {
            List<OrderItem> orderItems = new List<OrderItem>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SELECT * FROM OrderItem WHERE OrderId = @OrderId", connection))
                {
                    command.Parameters.AddWithValue("@OrderId", orderId);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        OrderItem orderItem = new OrderItem
                        {
                            Id = reader.GetInt32(0),
                            OrderId = reader.GetInt32(1),
                            ProductId = reader.GetInt32(2),
                            Quantity = reader.GetInt32(3),
                            Price = reader.GetDecimal(4)
                        };

                        orderItems.Add(orderItem);
                    }
                }
            }

            return orderItems;
        }


    }
}
