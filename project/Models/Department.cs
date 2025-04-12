namespace Project.Models
{
    /// <summary>
    /// Department class represents a department in a hospital or clinic.
    /// </summary>
    public class Department
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Department"/> class.
        /// </summary>
        public Department()
        {
            this.Name = "Unspecified";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Department"/> class with specified department ID and name.
        /// </summary>
        /// <param name="departmentID">The unique identifier for the department.</param>
        /// <param name="name">The name of the department.</param>
        public Department(int departmentID, string name)
        {
            this.DepartmentID = departmentID;
            this.Name = name;
        }

        /// <summary>
        /// Gets or Sets the department ID.
        /// </summary>
        public int DepartmentID { get; set; }

        /// <summary>
        /// Gets or Sets the name of the department.
        /// </summary>
        public string Name { get; set; }
    }
}
