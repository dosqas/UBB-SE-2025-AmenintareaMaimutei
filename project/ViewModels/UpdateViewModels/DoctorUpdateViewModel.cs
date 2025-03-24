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
using Windows.Media.Playback;

namespace Project.ViewModel
{
    class DoctorUpdateViewModel : INotifyPropertyChanged
    {
        private readonly DoctorModel _doctorModel = new DoctorModel();
        private readonly UserModel _userModel = new UserModel();
        public ObservableCollection<Doctor> Doctors { get; set; } = new ObservableCollection<Doctor>();



        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public ICommand SaveChangesCommand { get; }

        public DoctorUpdateViewModel()
        {
            _errorMessage = string.Empty;
            SaveChangesCommand = new RelayCommand(SaveChanges);
            LoadDoctors();
        }

        private void LoadDoctors()
        {
            Doctors.Clear();
            foreach (Doctor doctor in _doctorModel.GetDoctors())
            {
                Doctors.Add(doctor);
            }
        }

        private void SaveChanges()
        {
            bool hasErrors = false;
            StringBuilder errorMessages = new StringBuilder();

            foreach (Doctor doctor in Doctors)
            {
                if (!ValidateDoctor(doctor))
                {
                    hasErrors = true;
                    errorMessages.AppendLine("Doctor " + doctor.DoctorID + ": " + ErrorMessage);
                }
                else
                {
                    bool success = _doctorModel.UpdateDoctor(doctor);
                    if (!success)
                    {
                        errorMessages.AppendLine("Failed to save changes for doctor: " + doctor.DoctorID);
                        hasErrors = true;
                    }
                }
            }
            if (hasErrors)
            {
                ErrorMessage = errorMessages.ToString();
            }
            else
            {
                ErrorMessage = "Changes saved successfully";
            }
        }

        private bool ValidateDoctor(Doctor doctor)
        {
            bool userExistsAsDoctor = _userModel.UserExistsWithRole(doctor.UserID, "Doctor");
            bool userIsDoctor = _doctorModel.UserExistsInDoctors(doctor.UserID);
            if (!userExistsAsDoctor || userIsDoctor) { ErrorMessage = "UserID doesn’t exist or has already been approved"; return false; }

            bool departmentExists = _doctorModel.DoesDepartmentExist(doctor.DepartmentID);
            if (!departmentExists) { ErrorMessage = "DepartmentID doesn’t exist in the Departments Records"; return false; }

            if (doctor.Experience < 0) { ErrorMessage = "The experience provided should be a positive number"; return false; }

            if (!System.Text.RegularExpressions.Regex.IsMatch(doctor.LicenseNumber, @"^[a-zA-Z0-9 ]*$")) { ErrorMessage = "License Number should contain only alphanumeric characters"; return false; }

            return true;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
