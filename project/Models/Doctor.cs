// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Doctor.cs" company="YourCompany">
//   Copyright (c) YourCompany. All rights reserved.
// </copyright>
// <summary>
//   Defines the Doctor class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Project.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents a doctor with associated details such as ID, user, department, experience, rating, and license number.
    /// </summary>
    public class Doctor
    {
        /// <summary>
        /// Gets or sets the unique identifier for the doctor.
        /// </summary>
        public int DoctorID { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the user associated with the doctor.
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the department the doctor belongs to.
        /// </summary>
        public int DepartmentID { get; set; }

        /// <summary>
        /// Gets or sets the years of experience the doctor has.
        /// </summary>
        public float Experience { get; set; }

        /// <summary>
        /// Gets or sets the rating of the doctor.
        /// </summary>
        public float Rating { get; set; }

        /// <summary>
        /// Gets or sets the license number of the doctor.
        /// </summary>
        public string LicenseNumber { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Doctor"/> class with default values.
        /// </summary>
        public Doctor()
        {
            this.LicenseNumber = "0";
            this.Rating = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Doctor"/> class with specified values.
        /// </summary>
        /// <param name="doctorID">The unique identifier for the doctor.</param>
        /// <param name="userID">The unique identifier for the user associated with the doctor.</param>
        /// <param name="departmentID">The unique identifier for the department the doctor belongs to.</param>
        /// <param name="experience">The years of experience the doctor has.</param>
        /// <param name="rating">The rating of the doctor.</param>
        /// <param name="licenseNumber">The license number of the doctor.</param>
        public Doctor(int doctorID, int userID, int departmentID, float experience, float rating, string licenseNumber)
        {
            this.DoctorID = doctorID;
            this.UserID = userID;
            this.DepartmentID = departmentID;
            this.Experience = experience;
            this.Rating = rating;
            this.LicenseNumber = licenseNumber;
        }
    }
}