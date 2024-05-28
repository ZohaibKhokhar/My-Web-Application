using Microsoft.Data.SqlClient;

namespace WebApplication1.Models
{
    public class OrderRepository
    {
        public const string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=thisdb;Integrated Security=True;";

        public void AddOrder(Order order)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("INSERT INTO Orders (CustomerId, OrderDate, TotalPrice) VALUES (@CustomerId, @OrderDate, @TotalPrice)", connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@CustomerId", order.CustomerId);
                    command.Parameters.AddWithValue("@OrderDate", order.OrderDate);
                    command.Parameters.AddWithValue("@TotalPrice", order.TotalPrice);
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Order> GetAll()
        {
            List<Order> orders = new List<Order>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SELECT * FROM Orders", connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Order order = new Order
                        {
                            OrderId = reader.GetInt32(0),
                            CustomerId = reader.GetInt32(1),
                            OrderDate = reader.GetDateTime(2),
                            TotalPrice = reader.GetDecimal(3)
                        };

                        orders.Add(order);
                    }
                }
            }

            return orders;
        }
    }
}



