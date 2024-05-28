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
                string query = "INSERT INTO Customers (Name, Address, PhoneNumber, Email) VALUES (@name, @address, @phoneNumber, @email)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
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
    }
}
