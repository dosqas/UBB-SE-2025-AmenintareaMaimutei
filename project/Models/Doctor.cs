using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models
{
    public class Doctor
    {
        public int DoctorID { get; set; }
        public int UserID { get; set; }

        public int DepartmentID { get; set; }
        public float Experience { get; set; }
        public float Rating { get; set; }
        public string LicenseNumber { get; set; }

        public Doctor() { LicenseNumber = "0"; }
        public Doctor(int doctorID, int userID, int departmentID, float experience, float rating, string licenseNumber)
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