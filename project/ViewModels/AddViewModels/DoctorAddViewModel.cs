// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DoctorAddViewModel.cs" company="YourCompany">
//   Copyright (c) YourCompany. All rights reserved.
// </copyright>
// <summary>
//   ViewModel responsible for handling doctor addition logic, validation, and data binding.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Project.ViewModels.AddViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows.Input;
    using Project.ClassModels;
    using Project.Models;
    using Project.Utils;

    /// <summary>
    /// ViewModel for adding a new doctor.
    /// </summary>
    public class DoctorAddViewModel : INotifyPropertyChanged
    {
        // Private fields
        private readonly DoctorModel doctorModel = new DoctorModel();
        private int userID;
        private int departmentID;
        private float experience;
        private string licenseNumber = string.Empty;
        private string errorMessage = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="DoctorAddViewModel"/> class.
        /// </summary>
        public DoctorAddViewModel()
        {
            this.SaveDoctorCommand = new RelayCommand(this.SaveDoctor);
            this.LoadDoctors();
        }

        /// <summary>
        /// Gets the list of all doctors.
        /// </summary>
        public ObservableCollection<Doctor> Doctors { get; set; } = new ObservableCollection<Doctor>();

        /// <summary>
        /// Gets or sets the UserID of the doctor.
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
        /// Gets or sets the DepartmentID of the doctor.
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
        /// Gets or sets the years of experience.
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
        /// Gets or sets the error message to display in the UI.
        /// </summary>
        public string ErrorMessage
        {
            get => this.errorMessage;
            set
            {
                this.errorMessage = value;
                this.OnPropertyChanged(nameof(this.ErrorMessage));
            }
        }

        /// <summary>
        /// Gets the command that triggers the SaveDoctor method.
        /// </summary>
        public ICommand SaveDoctorCommand { get; }

        /// <summary>
        /// Loads all doctors into the ObservableCollection.
        /// </summary>
        private void LoadDoctors()
        {
            this.Doctors.Clear();
            foreach (Doctor doctor in this.doctorModel.GetDoctors())
            {
                this.Doctors.Add(doctor);
            }
        }

        /// <summary>
        /// Creates and saves a new doctor if validation passes.
        /// </summary>
        private void SaveDoctor()
        {
            var doctor = new Doctor
            {
                DoctorID = 0,
                UserID = this.UserID,
                DepartmentID = this.DepartmentID,
                Experience = this.Experience,
                LicenseNumber = this.LicenseNumber,
            };

            if (this.ValidateDoctor(doctor))
            {
                bool success = this.doctorModel.AddDoctor(doctor);
                this.ErrorMessage = success ? "Doctor added successfully" : "Failed to add doctor";

                if (success)
                {
                    this.LoadDoctors();
                }
            }
        }

        /// <summary>
        /// Validates the doctor information before saving.
        /// </summary>
        /// <param name="doctor">The doctor to validate.</param>
        /// <returns><c>true</c> if valid; otherwise, <c>false</c>.</returns>
        private bool ValidateDoctor(Doctor doctor)
        {
            if (!this.doctorModel.DoesUserExist(doctor.UserID))
            {
                this.ErrorMessage = "UserID doesn’t exist in the Users Records.";
                return false;
            }

            if (!this.doctorModel.IsUserDoctor(doctor.UserID))
            {
                this.ErrorMessage = "The user with this UserID is not a Doctor.";
                return false;
            }

            if (this.doctorModel.IsUserAlreadyDoctor(doctor.UserID))
            {
                this.ErrorMessage = "The user already exists in the Doctors Records.";
                return false;
            }

            if (!this.doctorModel.DoesDepartmentExist(doctor.DepartmentID))
            {
                this.ErrorMessage = "DepartmentID doesn’t exist in the Departments Records.";
                return false;
            }

            if (doctor.Experience < 0)
            {
                this.ErrorMessage = "Experience should be a positive number.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(doctor.LicenseNumber))
            {
                this.ErrorMessage = "Please enter the License Number.";
                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Triggers the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The name of the changed property.</param>
        protected void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
