// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DoctorDeleteViewModel.cs" company="YourCompanyName">
//   Copyright (c) YourCompanyName. All rights reserved.
// </copyright>
// <summary>
//   ViewModel for deleting doctors.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Project.ViewModels.DeleteViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows.Input;
    using Project.ClassModels;
    using Project.Models;
    using Project.Utils;

    /// <summary>
    /// ViewModel for deleting doctors.
    /// </summary>
    public class DoctorDeleteViewModel : INotifyPropertyChanged
    {
        private readonly DoctorModel doctorModel = new DoctorModel();
        private ObservableCollection<Doctor> doctors;
        private int doctorID;
        private string errorMessage;
        private string messageColor = "Red";

        /// <summary>
        /// Initializes a new instance of the <see cref="DoctorDeleteViewModel"/> class.
        /// </summary>
        public DoctorDeleteViewModel()
        {
            // Initialize non-nullable fields
            this.doctors = new ObservableCollection<Doctor>();
            this.errorMessage = string.Empty;

            // Load doctors for the DataGrid
            this.Doctors = new ObservableCollection<Doctor>(this.doctorModel.GetDoctors());

            this.DeleteDoctorCommand = new RelayCommand(this.RemoveDoctor);

            // Initialize PropertyChanged to avoid nullability issues
            this.PropertyChanged = (sender, args) => { };
        }

        /// <summary>
        /// Gets or sets the collection of doctors displayed in the view.
        /// </summary>
        public ObservableCollection<Doctor> Doctors
        {
            get { return this.doctors; }
            set { this.SetProperty(ref this.doctors, value); }
        }

        /// <summary>
        /// Gets or sets the ID of the doctor to be deleted.
        /// </summary>
        public int DoctorID
        {
            get => this.doctorID;
            set
            {
                this.doctorID = value;
                this.OnPropertyChanged(nameof(this.DoctorID));
                this.OnPropertyChanged(nameof(this.CanDeleteDoctor));
            }
        }

        /// <summary>
        /// Gets or sets the error message to display in the view.
        /// </summary>
        public string ErrorMessage
        {
            get => this.errorMessage ?? string.Empty;
            set
            {
                this.errorMessage = value;
                this.MessageColor = string.IsNullOrEmpty(value) ? "Red" : value.Contains("successfully") ? "Green" : "Red";
                this.OnPropertyChanged(nameof(this.ErrorMessage));
                this.OnPropertyChanged(nameof(this.MessageColor));
            }
        }

        /// <summary>
        /// Gets or sets the color of the message displayed in the view.
        /// </summary>
        public string MessageColor
        {
            get => this.messageColor;
            set
            {
                this.messageColor = value;
                this.OnPropertyChanged(nameof(this.MessageColor));
            }
        }

        /// <summary>
        /// Gets the command to delete a doctor.
        /// </summary>
        public ICommand DeleteDoctorCommand { get; }

        /// <summary>
        /// Gets a value indicating whether the doctor can be deleted.
        /// </summary>
        public bool CanDeleteDoctor => this.DoctorID > 0;

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;


        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event for the specified property.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Sets the specified field to the given value and raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <typeparam name="T">The type of the field.</typeparam>
        /// <param name="field">The field to set.</param>
        /// <param name="value">The value to set the field to.</param>
        /// <param name="propertyName">The name of the property that changed.</param>
        private void SetProperty<T>(ref T field, T value, [System.Runtime.CompilerServices.CallerMemberName] string propertyName = null!)
        {
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                this.OnPropertyChanged(propertyName);
            }
        }

        /// <summary>
        /// Determines whether the delete doctor command can execute.
        /// </summary>
        /// <returns>True if the doctor can be deleted; otherwise, false.</returns>
        private bool CanExecuteDeleteDoctor()
        {
            return this.DoctorID > 0;
        }

        /// <summary>
        /// Removes the doctor with the specified ID from the database.
        /// </summary>
        private void RemoveDoctor()
        {
            if (this.DoctorID == 0)
            {
                this.ErrorMessage = "No doctor was selected";
                return;
            }

            if (!this.doctorModel.DoesDoctorExist(this.DoctorID))
            {
                this.ErrorMessage = "DoctorID doesn't exist in the records";
                return;
            }

            bool success = this.doctorModel.DeleteDoctor(this.DoctorID);
            this.ErrorMessage = success ? "Doctor deleted successfully" : "Failed to delete doctor";

            if (success)
            {
                this.Doctors = new ObservableCollection<Doctor>(this.doctorModel.GetDoctors());
            }
        }
    }
}
