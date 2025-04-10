namespace Project.Models
{
    using System;

    /// <summary>
    /// User class.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="userID">The id of the user.</param>
        /// <param name="username">The username of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <param name="mail">The mail of the user.</param>
        /// <param name="role">The role of the user.</param>
        /// <param name="name">The name of the user.</param>
        /// <param name="birthdate">The birthdate of the user.</param>
        /// <param name="cnp">The cnp of the user.</param>
        /// <param name="address">The address of the user.</param>
        /// <param name="phoneNumber">The phoneNumber of the user.</param>
        /// <param name="registrationDate">The registrationDate of the user.</param>
        public User(int userID, string username, string password, string mail, string role, string name, DateOnly birthdate, string cnp, string address, string phoneNumber, DateTime registrationDate)
        {
            this.UserID = userID;
            this.Username = username;
            this.Password = password;
            this.Mail = mail;
            this.Role = role;
            this.Name = name;
            this.Birthdate = birthdate;
            this.Cnp = cnp;
            this.Address = address;
            this.PhoneNumber = phoneNumber;
            this.RegistrationDate = registrationDate;
        }

        /// <summary>
        /// Gets or sets UserID.
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// Gets or sets Username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets Password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets Mail.
        /// </summary>
        public string Mail { get; set; }

        /// <summary>
        /// Gets or sets Role.
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Birthdate.
        /// </summary>
        public DateOnly Birthdate { get; set; }

        /// <summary>
        /// Gets or sets Cnp.
        /// </summary>
        public string Cnp { get; set; }

        /// <summary>
        /// Gets or sets Address.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets PhoneNumber.
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets RegistrationDate.
        /// </summary>
        public DateTime RegistrationDate { get; set; }
    }
}
