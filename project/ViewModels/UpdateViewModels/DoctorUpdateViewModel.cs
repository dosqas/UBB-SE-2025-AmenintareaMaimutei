// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DoctorUpdateViewModel.cs" company="YourCompany">
//   Copyright (c) YourCompany. All rights reserved.
// </copyright>
// <summary>
//   ViewModel responsible for updating doctors, including validation and persistence.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Project.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Project.ClassModels;
    using Project.Models;
    using Project.Utils;

    /// <summary>
    /// ViewModel for updating doctors in the system.
    /// </summary>
    public class DoctorUpdateViewModel : INotifyPropertyChanged
    {
        private readonly DoctorModel doctorModel = new DoctorModel();
        private readonly UserModel userModel = new UserModel();
        private string errorMessage = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="DoctorUpdateViewModel"/> class.
        /// </summary>
        public DoctorUpdateViewModel()
        {
            this.SaveChangesCommand = new RelayCommand(this.SaveChanges);
            this.LoadDoctors();
        }

        /// <summary>
        /// Gets or sets the error message for display.
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
        /// Gets the command for saving changes to doctors.
        /// </summary>
        public ICommand SaveChangesCommand { get; }

        /// <summary>
        /// Gets the list of doctors to be displayed and updated.
        /// </summary>
        public ObservableCollection<Doctor> Doctors { get; set; } = new ObservableCollection<Doctor>();

        /// <summary>
        /// Loads the doctors from the model into the view model.
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
        /// Saves the changes made to each doctor after validation.
        /// </summary>
        private void SaveChanges()
        {
            bool hasErrors = false;
            StringBuilder errorMessages = new StringBuilder();

            foreach (Doctor doctor in this.Doctors)
            {
                if (!this.ValidateDoctor(doctor))
                {
                    hasErrors = true;
                    errorMessages.AppendLine($"Doctor {doctor.DoctorID}: {this.ErrorMessage}");
                }
                else
                {
                    bool success = this.doctorModel.UpdateDoctor(doctor);
                    if (!success)
                    {
                        errorMessages.AppendLine($"Failed to save changes for doctor: {doctor.DoctorID}");
                        hasErrors = true;
                    }
                }
            }

            this.ErrorMessage = hasErrors ? errorMessages.ToString() : "Changes saved successfully";
        }

        /// <summary>
        /// Validates the information of a single doctor.
        /// </summary>
        /// <param name="doctor">The doctor to validate.</param>
        /// <returns><c>true</c> if valid; otherwise, <c>false</c>.</returns>
        private bool ValidateDoctor(Doctor doctor)
        {
            if (!this.userModel.UserExistsWithRole(doctor.UserID, "Doctor") ||
                this.doctorModel.UserExistsInDoctors(doctor.UserID, doctor.DoctorID))
            {
                this.ErrorMessage = "UserID doesn’t exist or has already been approved";
                return false;
            }

            if (!this.doctorModel.DoesDepartmentExist(doctor.DepartmentID))
            {
                this.ErrorMessage = "DepartmentID doesn’t exist in the Departments Records";
                return false;
            }

            if (doctor.Experience < 0)
            {
                this.ErrorMessage = "The experience provided should be a positive number";
                return false;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(doctor.LicenseNumber, @"^[a-zA-Z0-9 ]*$"))
            {
                this.ErrorMessage = "License Number should contain only alphanumeric characters";
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
