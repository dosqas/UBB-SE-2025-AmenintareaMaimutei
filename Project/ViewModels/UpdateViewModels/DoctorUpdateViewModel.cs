using Project.ClassModels;
using Project.Models;
using Project.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Project.ViewModel
{
    class DoctorUpdateViewModel : INotifyPropertyChanged
    {
        private readonly DoctorModel _doctorModel = new DoctorModel();

        private Doctor _doctor = new Doctor();
        public Doctor Doctor
        {
            get => _doctor;
            set
            {
                _doctor = value;
                OnPropertyChanged(nameof(Doctor));
            }
        }

        private string _errorMessage;
        public string ErrorMessage { 
            get => _errorMessage;
            set {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public ICommand SaveDoctorCommand { get; }

        public DoctorUpdateViewModel()
        {
            SaveDoctorCommand = new RelayCommand(SaveDoctor);
        }

        private void SaveDoctor()
        {
            if (ValidateDoctor(Doctor))
            {
                bool success = _doctorModel.UpdateDoctor(Doctor);
                ErrorMessage = success ? "Doctor updated successfully" : "Failed to update doctor";
            }
        }

        private bool ValidateDoctor(Doctor doctor)
        {
            if (doctor.DoctorID == Guid.Empty) { ErrorMessage = "Invalid Doctor ID"; return false; }
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
