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
                using (SqlCommand command = new SqlCommand("INSERT INTO Orders (OrderId,CustomerId, OrderDate, TotalPrice) VALUES (@id,@CustomerId, @OrderDate, @TotalPrice)", connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@id", order.OrderId);
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
        public int GetMaxOrderId()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SELECT MAX(OrderId) FROM Orders", connection))
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    return result == null || result == DBNull.Value ? 0 : (int)result;
                }
            }
        }

        public int GetOrderIdByCustomerId(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SELECT OrderId FROM Orders WHERE CustomerId = @CustomerId", connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@CustomerId", id);
                    object result = command.ExecuteScalar();
                    return result == null || result == DBNull.Value ? 0 : (int)result;
                }
            }
        }
        public Order getOrderById(int id)
        {
            Order order = new Order();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SELECT * FROM Orders WHERE OrderId = @OrderId", connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@OrderId", id);
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        order.OrderId = reader.GetInt32(0);
                        order.CustomerId = reader.GetInt32(1);
                        order.OrderDate = reader.GetDateTime(2);
                        order.TotalPrice = reader.GetDecimal(3);
                    }
                }
            }

            return order;
        }
        public void deleteOrderById(int id)
        {
            Order order = new Order();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("DELETE FROM Orders WHERE OrderId = @OrderId", connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@OrderId", id);
                    int affectedRows = command.ExecuteNonQuery();
                }
            }
        }
        public int getCustomerIdByOrderId(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SELECT CustomerId FROM Orders WHERE OrderId = @OrderId", connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@OrderId", id);
                    object result = command.ExecuteScalar();
                    return result == null || result == DBNull.Value ? 0 : (int)result;
                }
            }
        }
    }
}



