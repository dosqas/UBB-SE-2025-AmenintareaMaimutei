using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models
{
    public class Department
    {
        public Guid DepartmentID { get; set; }
        public string Name { get; set; }

        public Department() { Name = "Unspecified"; }
        public Department(Guid departmentID, string name)
        {
            DepartmentID = departmentID;
            Name = name;
        }
    }
}
