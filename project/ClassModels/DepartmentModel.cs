namespace Project.ClassModels
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Data.SqlClient;
    using Project.Models;
    using Project.Utils;

    /// <summary>
    /// DepartmentModel class handles database operations for departments.
    /// </summary>
    public class DepartmentModel
    {
        private readonly string connectionString = DatabaseHelper.GetConnectionString();

        /// <summary>
        /// Adds a new department to the database.
        /// </summary>
        /// <param name="department">The department to add.</param>
        /// <returns>True if the department was added successfully, otherwise false.</returns>
        public bool AddDepartment(Department department)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                string query = "INSERT INTO Departments (Name) VALUES (@Name)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Name", department.Name);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        /// <summary>
        /// Updates an existing department in the database.
        /// </summary>
        /// <param name="department">The department to update.</param>
        /// <returns>True if the department was updated successfully, otherwise false.</returns>
        public bool UpdateDepartment(Department department)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    string query = "UPDATE Departments SET Name = @Name WHERE DepartmentID = @DepartmentID";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Name", department.Name);
                    command.Parameters.AddWithValue("@DepartmentID", department.DepartmentID);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
                return false;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Invalid Operation: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Error: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Deletes a department from the database.
        /// </summary>
        /// <param name="departmentID">The ID of the department to delete.</param>
        /// <returns>True if the department was deleted successfully, otherwise false.</returns>
        public bool DeleteDepartment(int departmentID)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                string query = "DELETE FROM Departments WHERE DepartmentID = @DepartmentID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@DepartmentID", departmentID);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        /// <summary>
        /// Checks if a department exists in the database.
        /// </summary>
        /// <param name="departmentID">The ID of the department to check.</param>
        /// <returns>True if the department exists, otherwise false.</returns>
        public bool DoesDepartmentExist(int departmentID)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                string query = "SELECT COUNT(*) FROM Departments WHERE DepartmentID = @DepartmentID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@DepartmentID", departmentID);

                connection.Open();
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }

        /// <summary>
        /// Retrieves all departments from the database.
        /// </summary>
        /// <returns>A list of departments.</returns>
        public List<Department> GetDepartments()
        {
            List<Department> departments = new List<Department>();
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                string query = "SELECT * FROM Departments";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    departments.Add(new Department
                    {
                        DepartmentID = reader.GetInt32(reader.GetOrdinal("DepartmentID")),
                        Name = reader.GetString(reader.GetOrdinal("Name")),
                    });
                }
            }

            return departments;
        }
    }
}