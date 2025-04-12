// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DoctorInformation.cs" company="YourCompanyName">
//   Copyright (c) YourCompanyName. All rights reserved.
// </copyright>
// <summary>
//   Represents information about a doctor.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Project.Models
{
    using System;

    /// <summary>
    /// Represents information about a doctor.
    /// </summary>
    public class DoctorInformation
    {
        /// <summary>
        /// Gets or sets the user ID.
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        public string Mail { get; set; }

        /// <summary>
        /// Gets or sets the role of the user.
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// Gets or sets the name of the doctor.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the birthdate of the doctor.
        /// </summary>
        public DateTime Birthdate { get; set; }

        /// <summary>
        /// Gets or sets the CNP (unique identifier).
        /// </summary>
        public string Cnp { get; set; }

        /// <summary>
        /// Gets or sets the address of the doctor.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the phone number of the doctor.
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the registration date of the doctor.
        /// </summary>
        public DateTime RegistrationDate { get; set; }

        /// <summary>
        /// Gets or sets the doctor ID.
        /// </summary>
        public int DoctorID { get; set; }

        /// <summary>
        /// Gets or sets the license number of the doctor.
        /// </summary>
        public string LicenseNumber { get; set; }

        /// <summary>
        /// Gets or sets the years of experience of the doctor.
        /// </summary>
        public float Experience { get; set; }

        /// <summary>
        /// Gets or sets the rating of the doctor.
        /// </summary>
        public float Rating { get; set; }

        /// <summary>
        /// Gets or sets the department ID.
        /// </summary>
        public int DepartmentID { get; set; }

        /// <summary>
        /// Gets or sets the name of the department.
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DoctorInformation"/> class.
        /// </summary>
        /// <param name="userID">The user ID.</param>
        /// <param name="username">The username.</param>
        /// <param name="mail">The email address.</param>
        /// <param name="role">The role of the user.</param>
        /// <param name="name">The name of the doctor.</param>
        /// <param name="birthdate">The birthdate of the doctor.</param>
        /// <param name="cnp">The CNP (unique identifier).</param>
        /// <param name="address">The address of the doctor.</param>
        /// <param name="phoneNumber">The phone number of the doctor.</param>
        /// <param name="registrationDate">The registration date of the doctor.</param>
        /// <param name="doctorID">The doctor ID.</param>
        /// <param name="licenseNumber">The license number of the doctor.</param>
        /// <param name="experience">The years of experience of the doctor.</param>
        /// <param name="rating">The rating of the doctor.</param>
        /// <param name="departmentID">The department ID.</param>
        /// <param name="departmentName">The name of the department.</param>
        public DoctorInformation(
    int userID,
    string username,
    string mail,
    string role,
    string name,
    DateTime birthdate,
    string cnp,
    string address,
    string phoneNumber,
    DateTime registrationDate,
    int doctorID,
    string licenseNumber,
    float experience,
    float rating,
    int departmentID,
    string departmentName)
        {
            this.UserID = userID;
            this.Username = username;
            this.Mail = mail;
            this.Role = role;
            this.Name = name;
            this.Birthdate = birthdate;
            this.Cnp = cnp;
            this.Address = address;
            this.PhoneNumber = phoneNumber;
            this.RegistrationDate = registrationDate;
            this.DoctorID = doctorID;
            this.LicenseNumber = licenseNumber;
            this.Experience = experience;
            this.Rating = rating;
            this.DepartmentID = departmentID;
            this.DepartmentName = departmentName;
        }
    }
}