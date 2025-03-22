using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models
{
    public class Doctor
    {
        public Guid DoctorID { get; set; }
        public Guid UserID { get; set; }

        public Guid DepartmentID { get; set; }
        public float Experience { get; set; }
        public float Rating { get; set; }
        public string LicenseNumber { get; set; }

        public Doctor() { LicenseNumber = "0"; }
        public Doctor(Guid doctorID, Guid userID, Guid departmentID, float experience, float rating, string licenseNumber)
        {
            DoctorID = doctorID;
            UserID = userID;
            DepartmentID = departmentID;
            Experience = experience;
            Rating = rating;
            LicenseNumber = licenseNumber;
        }
    }
}