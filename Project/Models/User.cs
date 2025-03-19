using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models
{
    public class User
    {
        public Guid UserID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Mail { get; set; }
        public string Role { get; set; }
        public string Name { get; set; }
        public DateOnly Birthdate { get; set; }
        public string Cnp { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime RegistrationDate { get; set; }

        public User(Guid userID, string username, string password, string mail, string role, string name, DateOnly birthdate, string cnp, string address, string phoneNumber, DateTime registrationDate)
        {
            UserID = userID;
            Username = username;
            Password = password;
            Mail = mail;
            Role = role;
            Name = name;
            Birthdate = birthdate;
            Cnp = cnp;
            Address = address;
            PhoneNumber = phoneNumber;
            RegistrationDate = registrationDate;
        }
    }
}
