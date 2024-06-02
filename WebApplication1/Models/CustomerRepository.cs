using Microsoft.Data.SqlClient;
using System.Data;

namespace WebApplication1.Models
{
    public class CustomerRepository
    {
        public const string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=thisdb;Integrated Security=True;";

        public void AddCustomer(Customer customer)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Customers (CustomerId,Name, Address, PhoneNumber, Email) VALUES (@id,@name, @address, @phoneNumber, @email)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = customer.CustomerId;
                    cmd.Parameters.Add("@name", SqlDbType.NVarChar, 50).Value = customer.Name;
                    cmd.Parameters.Add("@address", SqlDbType.NVarChar, 200).Value = customer.Address;
                    cmd.Parameters.Add("@phoneNumber", SqlDbType.NVarChar, 20).Value = customer.PhoneNumber;
                    cmd.Parameters.Add("@email", SqlDbType.NVarChar, 50).Value = customer.Email;
                    int rows = cmd.ExecuteNonQuery();
                }
            }
        }
        public List<Customer> GetAllCustomers()
        {
            List<Customer> customers = new List<Customer>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "select * from Customers";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Customer customer = new Customer
                        {
                            CustomerId = (int)reader["CustomerId"],
                            Name = (string)reader["Name"],
                            Address = (string)reader["Address"],
                            PhoneNumber = (string)reader["PhoneNumber"],
                            Email = (string)reader["Email"]
                        };
                        customers.Add(customer);
                    }
                }
            }
            return customers;
        }
        public int getLastId()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("select max(CustomerId) from Customers", conn))
                {
                    object result = cmd.ExecuteScalar();
                    return result == null ? 0 : (int)result;
                }
            }
        }
        public Customer GetCustomerById(int id)
        {
            Customer customer = null;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "select * from Customers where CustomerId = @CustomerId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CustomerId", id);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        customer = new Customer
                        {
                            CustomerId = (int)reader["CustomerId"],
                            Name = (string)reader["Name"],
                            Address = (string)reader["Address"],
                            PhoneNumber = (string)reader["PhoneNumber"],
                            Email = (string)reader["Email"]
                        };
                    }
                }
            }
            return customer;
        }
        public List<int> GetCustomerId(string currentUserName)
        {
            List<int> list = new List<int>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "select CustomerId from Customers where Email=@currentUser";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@currentUser", SqlDbType.NVarChar).Value = currentUserName;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = int.Parse(reader["CustomerId"].ToString());
                            list.Add(id);
                        }
                    }
                }
            }
            return list;
        }
        public void deleteByCustomerId(int customerId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM Customers WHERE CustomerId = @CustomerId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CustomerId", customerId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}

