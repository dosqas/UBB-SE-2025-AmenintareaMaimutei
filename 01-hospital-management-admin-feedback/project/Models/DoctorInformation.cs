using System;

namespace Project.Models
{
    public class DoctorInformation
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Mail { get; set; }
        public string Role { get; set; }
        public string Name { get; set; }
        public DateTime Birthdate { get; set; }
        public string Cnp { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int DoctorID { get; set; }
        public string LicenseNumber { get; set; }
        public float Experience { get; set; }
        public float Rating { get; set; }
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }

        public DoctorInformation(int userID, string username, string mail, string role, string name, DateTime birthdate, string cnp, string address, string phoneNumber, DateTime registrationDate, int doctorID, string licenseNumber, float experience, float rating, int departmentID, string departmentName)
        {
            UserID = userID;
            Username = username;
            Mail = mail;
            Role = role;
            Name = name;
            Birthdate = birthdate;
            Cnp = cnp;
            Address = address;
            PhoneNumber = phoneNumber;
            RegistrationDate = registrationDate;
            DoctorID = doctorID;
            LicenseNumber = licenseNumber;
            Experience = experience;
            Rating = rating;
            DepartmentID = departmentID;
            DepartmentName = departmentName;
        }
    }
}