using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace Project.ClassModels
{
    public class DoctorModel
    {
        private readonly string _connectionString = "DE ADAUGAT CONNECTION STRING";

        public List<Doctor> GetDoctors()
        {
            List<Doctor> doctors = new List<Doctor>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Doctors";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    doctors.Add(new Doctor(
                        reader.GetGuid(0),
                        reader.GetGuid(1),
                        reader.GetGuid(2),
                        reader.GetFloat(3),
                        reader.GetFloat(4),
                        reader.GetString(5)
                    ));
                }
            }
            return doctors;
        }
    }
}
