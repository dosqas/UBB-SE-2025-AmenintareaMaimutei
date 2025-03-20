using Project.Models;
using Project.Utils;
using System;
using System.ComponentModel;
using System.Windows.Input;
using DoctorModel = Project.ClassModels.DoctorModel;

namespace Project.ViewModels
{
    class DoctorAddViewModel : INotifyPropertyChanged
    {
        private readonly DoctorModel _doctorModel = new DoctorModel();

        private Guid _userID;
        public Guid UserID
        {
            get => _userID;
            set
            {
                _userID = value;
                OnPropertyChanged(nameof(UserID));
            }
        }

        private Guid _departmentID;
        public Guid DepartmentID
        {
            get => _departmentID;
            set
            {
                _departmentID = value;
                OnPropertyChanged(nameof(DepartmentID));
            }
        }

        private float _experience;
        public float Experience
        {
            get => _experience;
            set
            {
                _experience = value;
                OnPropertyChanged(nameof(Experience));
            }
        }

        private string _licenseNumber = "";
        public string LicenseNumber
        {
            get => _licenseNumber;
            set
            {
                _licenseNumber = value;
                OnPropertyChanged(nameof(LicenseNumber));
            }
        }

        private string _errorMessage = "";
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public ICommand SaveDoctorCommand { get; }

        public DoctorAddViewModel()
        {
            SaveDoctorCommand = new RelayCommand(SaveDoctor);
        }

        private void SaveDoctor()
        {
            var doctor = new Doctor
            {
                DoctorID = Guid.NewGuid(),
                UserID = UserID,
                DepartmentID = DepartmentID,
                Experience = Experience,
                LicenseNumber = LicenseNumber
            };

            if (ValidateDoctor(doctor))
            {
                bool success = _doctorModel.AddDoctor(doctor);
                ErrorMessage = success ? "Doctor added successfully" : "Failed to add doctor";
            }
        }

        private bool ValidateDoctor(Doctor doctor)
        {
            if (doctor.UserID == Guid.Empty) { ErrorMessage = "Invalid User ID"; return false; }
            if (doctor.DepartmentID == Guid.Empty) { ErrorMessage = "Invalid Department ID"; return false; }
            if (doctor.Experience < 0) { ErrorMessage = "Experience must be non-negative"; return false; }
            if (string.IsNullOrWhiteSpace(doctor.LicenseNumber)) { ErrorMessage = "License Number is required"; return false; }
            return true;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
