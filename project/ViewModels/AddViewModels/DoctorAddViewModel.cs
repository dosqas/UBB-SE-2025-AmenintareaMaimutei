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

        private int _userID;
        public int UserID
        {
            get => _userID;
            set
            {
                _userID = value;
                OnPropertyChanged(nameof(UserID));
            }
        }

        private int _departmentID;
        public int DepartmentID
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
                //DoctorID = Guid.NewGuid(),
                DoctorID = 0,
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
            if (!_doctorModel.DoesUserExist(doctor.UserID))
            {
                ErrorMessage = "UserID doesn’t exist in the Users Records.";
                return false;
            }

            if (!_doctorModel.IsUserDoctor(doctor.UserID))
            {
                ErrorMessage = "The user with this UserID is not a Doctor.";
                return false;
            }

            if (_doctorModel.IsUserAlreadyDoctor(doctor.UserID))
            {
                ErrorMessage = "The user already exists in the Doctors Records.";
                return false;
            }

            //if (doctor.DepartmentID == Guid.Empty)
            if (!_doctorModel.DoesDepartmentExist(doctor.DepartmentID))
            {
                ErrorMessage = "DepartmentID doesn’t exist in the Departments Records.";
                return false;
            }

            if (doctor.Experience < 0)
            {
                ErrorMessage = "The information provided should be a positive number.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(doctor.LicenseNumber))
            {
                ErrorMessage = "Please enter the License Number.";
                return false;
            }

            return true;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
