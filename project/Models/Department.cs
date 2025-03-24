using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models
{
    public class Department
    {
        public int DepartmentID { get; set; }
        public string Name { get; set; }

        public Department() { Name = "Unspecified"; }
        public Department(int departmentID, string name)
        {
            DepartmentID = departmentID;
            Name = name;
        }
    }
}
