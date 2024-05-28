using Microsoft.Data.SqlClient;
using System.Data;
using WebApplication1.Models;

namespace WebApplication1.Models
{
    public class AppointmentRepository
    {
        public const string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=thisdb;Integrated Security=True;";
        public void Add(Appointment appointment)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "insert into Appointments values(@name,@phone,@pettype,@dateTime,@reason)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@name", SqlDbType.NVarChar, 50).Value = appointment.Name;
                    cmd.Parameters.Add("@phone", SqlDbType.NVarChar, 20).Value = appointment.Phone;
                    cmd.Parameters.Add("@pettype", SqlDbType.NVarChar, 20).Value = appointment.PetType;
                    cmd.Parameters.Add("@dateTime", SqlDbType.DateTime).Value = appointment.preferredDateTime;
                    cmd.Parameters.Add("@reason", SqlDbType.NVarChar, 200).Value = appointment.Reason;
                    int rows = cmd.ExecuteNonQuery();
                }
            }
        }
        public void Update(Appointment appointment)
        { }
        public void Delete(Appointment appointment)
        { }

        public void DeleteAll()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string query = "truncate table Appointments";
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.ExecuteNonQuery();
            connection.Close();
        }
        public void DeleteById(int id)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string query = "delete from Appointments where ID=@id";
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.Add("@id", SqlDbType.Int, 0).Value = id;
            cmd.ExecuteNonQuery();
            connection.Close();
        }
        public void DeleteByName(string name)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string query = "delete from Appointments where Name=@name";
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.Add("@name", SqlDbType.Int, 0).Value = name;
            cmd.ExecuteNonQuery();
            connection.Close();
        }
        public Appointment Get()
        {
            return new Appointment();
        }


        public List<Appointment> GetAll()
        {
            List<Appointment> list = new List<Appointment>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "select * from Appointments";
                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Appointment app = new Appointment()
                    {
                        Id = int.Parse(reader["ID"].ToString()),
                        Name = reader["Name"] as string,
                        Phone = reader["PhoneNumber"] as string,
                        PetType = reader["PetType"] as string,
                        preferredDateTime = DateTime.Parse(reader["AppointmentDateTime"].ToString()),
                        Reason = reader["ReasonForAppointment"] as string
                    };
                    list.Add(app);
                }
            }            
            return list;
        } 
    }


}
    


