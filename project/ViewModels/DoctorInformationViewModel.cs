// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DoctorInformationViewModel.cs" company="YourCompany">
//   Copyright (c) YourCompany. All rights reserved.
// </copyright>
// <summary>
//   ViewModel for managing doctor information.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Project.ViewModels
{
    using System;
    using System.ComponentModel;
    using Project.ClassModels;
    using Project.Models;

    /// <summary>
    /// ViewModel for managing doctor information.
    /// </summary>
    public class DoctorInformationViewModel : INotifyPropertyChanged
    {
        private readonly DoctorInformationModel doctorModel = new DoctorInformationModel();

        // Fields
        private int userID;
        private string username = string.Empty;
        private string mail = string.Empty;
        private string name = string.Empty;
        private DateTime birthdate;
        private string cnp = string.Empty;
        private string address = string.Empty;
        private string phoneNumber = string.Empty;
        private DateTime registrationDate;
        private int doctorID;
        private string licenseNumber = string.Empty;
        private float experience;
        private float rating;
        private int departmentID;
        private string departmentName = string.Empty;
        private decimal salary;

        /// <summary>
        /// Gets or sets the user ID.
        /// </summary>
        public int UserID
        {
            get => this.userID;
            set
            {
                this.userID = value;
                this.OnPropertyChanged(nameof(this.UserID));
            }
        }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username
        {
            get => this.username;
            set
            {
                this.username = value;
                this.OnPropertyChanged(nameof(this.Username));
            }
        }

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        public string Mail
        {
            get => this.mail;
            set
            {
                this.mail = value;
                this.OnPropertyChanged(nameof(this.Mail));
            }
        }

        /// <summary>
        /// Gets or sets the name of the doctor.
        /// </summary>
        public string Name
        {
            get => this.name;
            set
            {
                this.name = value;
                this.OnPropertyChanged(nameof(this.Name));
            }
        }

        /// <summary>
        /// Gets or sets the birthdate of the doctor.
        /// </summary>
        public DateTime Birthdate
        {
            get => this.birthdate;
            set
            {
                this.birthdate = value;
                this.OnPropertyChanged(nameof(this.Birthdate));
            }
        }

        /// <summary>
        /// Gets or sets the CNP (unique identifier).
        /// </summary>
        public string Cnp
        {
            get => this.cnp;
            set
            {
                this.cnp = value;
                this.OnPropertyChanged(nameof(this.Cnp));
            }
        }

        /// <summary>
        /// Gets or sets the address of the doctor.
        /// </summary>
        public string Address
        {
            get => this.address;
            set
            {
                this.address = value;
                this.OnPropertyChanged(nameof(this.Address));
            }
        }

        /// <summary>
        /// Gets or sets the phone number of the doctor.
        /// </summary>
        public string PhoneNumber
        {
            get => this.phoneNumber;
            set
            {
                this.phoneNumber = value;
                this.OnPropertyChanged(nameof(this.PhoneNumber));
            }
        }

        /// <summary>
        /// Gets or sets the registration date of the doctor.
        /// </summary>
        public DateTime RegistrationDate
        {
            get => this.registrationDate;
            set
            {
                this.registrationDate = value;
                this.OnPropertyChanged(nameof(this.RegistrationDate));
            }
        }

        /// <summary>
        /// Gets or sets the doctor ID.
        /// </summary>
        public int DoctorID
        {
            get => this.doctorID;
            set
            {
                this.doctorID = value;
                this.OnPropertyChanged(nameof(this.DoctorID));
            }
        }

        /// <summary>
        /// Gets or sets the license number of the doctor.
        /// </summary>
        public string LicenseNumber
        {
            get => this.licenseNumber;
            set
            {
                this.licenseNumber = value;
                this.OnPropertyChanged(nameof(this.LicenseNumber));
            }
        }

        /// <summary>
        /// Gets or sets the years of experience of the doctor.
        /// </summary>
        public float Experience
        {
            get => this.experience;
            set
            {
                this.experience = value;
                this.OnPropertyChanged(nameof(this.Experience));
            }
        }

        /// <summary>
        /// Gets or sets the rating of the doctor.
        /// </summary>
        public float Rating
        {
            get => this.rating;
            set
            {
                this.rating = value;
                this.OnPropertyChanged(nameof(this.Rating));
            }
        }

        /// <summary>
        /// Gets or sets the department ID.
        /// </summary>
        public int DepartmentID
        {
            get => this.departmentID;
            set
            {
                this.departmentID = value;
                this.OnPropertyChanged(nameof(this.DepartmentID));
            }
        }

        /// <summary>
        /// Gets or sets the name of the department.
        /// </summary>
        public string DepartmentName
        {
            get => this.departmentName;
            set
            {
                this.departmentName = value;
                this.OnPropertyChanged(nameof(this.DepartmentName));
            }
        }

        /// <summary>
        /// Gets or sets the salary of the doctor.
        /// </summary>
        public decimal Salary
        {
            get => this.salary;
            set
            {
                this.salary = value;
                this.OnPropertyChanged(nameof(this.Salary));
            }
        }

        /// <summary>
        /// Loads the doctor information based on the provided doctor ID.
        /// </summary>
        /// <param name="doctorID">The unique identifier of the doctor.</param>
        /// <exception cref="Exception">Thrown if the doctor is not found.</exception>
        public void LoadDoctorInformation(int doctorID)
        {
            var doctorInfo = this.doctorModel.GetDoctorInformation(doctorID);
            if (doctorInfo != null)
            {
                this.UserID = doctorInfo.UserID;
                this.Username = doctorInfo.Username;
                this.Mail = doctorInfo.Mail;
                this.Name = doctorInfo.Name;
                this.Birthdate = doctorInfo.Birthdate;
                this.Cnp = doctorInfo.Cnp;
                this.Address = doctorInfo.Address;
                this.PhoneNumber = doctorInfo.PhoneNumber;
                this.RegistrationDate = doctorInfo.RegistrationDate;
                this.DoctorID = doctorInfo.DoctorID;
                this.LicenseNumber = doctorInfo.LicenseNumber;
                this.Experience = doctorInfo.Experience;
                this.Rating = doctorInfo.Rating;
                this.DepartmentID = doctorInfo.DepartmentID;
                this.DepartmentName = doctorInfo.DepartmentName;
                this.Salary = this.doctorModel.ComputeSalary(doctorID);
            }
            else
            {
                throw new Exception("Doctor not found");
            }
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged = (sender, e) => { };

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}