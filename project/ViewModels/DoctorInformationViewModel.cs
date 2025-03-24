using Project.ClassModels;
using Project.Models;
using System;
using System.ComponentModel;

namespace Project.ViewModels
{
    public class DoctorInformationViewModel : INotifyPropertyChanged
    {
        private readonly DoctorInformationModel _doctorModel = new DoctorInformationModel();

        // Properties...
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

        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        private string _mail;
        public string Mail
        {
            get => _mail;
            set
            {
                _mail = value;
                OnPropertyChanged(nameof(Mail));
            }
        }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private DateTime _birthdate;
        public DateTime Birthdate
        {
            get => _birthdate;
            set
            {
                _birthdate = value;
                OnPropertyChanged(nameof(Birthdate));
            }
        }

        private string _cnp;
        public string Cnp
        {
            get => _cnp;
            set
            {
                _cnp = value;
                OnPropertyChanged(nameof(Cnp));
            }
        }

        private string _address;
        public string Address
        {
            get => _address;
            set
            {
                _address = value;
                OnPropertyChanged(nameof(Address));
            }
        }

        private string _phoneNumber;
        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                _phoneNumber = value;
                OnPropertyChanged(nameof(PhoneNumber));
            }
        }

        private DateTime _registrationDate;
        public DateTime RegistrationDate
        {
            get => _registrationDate;
            set
            {
                _registrationDate = value;
                OnPropertyChanged(nameof(RegistrationDate));
            }
        }

        private int _doctorID;
        public int DoctorID
        {
            get => _doctorID;
            set
            {
                _doctorID = value;
                OnPropertyChanged(nameof(DoctorID));
            }
        }

        private string _licenseNumber;
        public string LicenseNumber
        {
            get => _licenseNumber;
            set
            {
                _licenseNumber = value;
                OnPropertyChanged(nameof(LicenseNumber));
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

        private float _rating;
        public float Rating
        {
            get => _rating;
            set
            {
                _rating = value;
                OnPropertyChanged(nameof(Rating));
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

        private string _departmentName;
        public string DepartmentName
        {
            get => _departmentName;
            set
            {
                _departmentName = value;
                OnPropertyChanged(nameof(DepartmentName));
            }
        }

        public void LoadDoctorInformation(int doctorID)
        {
            var doctorInfo = _doctorModel.GetDoctorInformation(doctorID);
            if (doctorInfo != null)
            {
                UserID = doctorInfo.UserID;
                Username = doctorInfo.Username;
                Mail = doctorInfo.Mail;
                Name = doctorInfo.Name;
                Birthdate = doctorInfo.Birthdate;
                Cnp = doctorInfo.Cnp;
                Address = doctorInfo.Address;
                PhoneNumber = doctorInfo.PhoneNumber;
                RegistrationDate = doctorInfo.RegistrationDate;
                DoctorID = doctorInfo.DoctorID;
                LicenseNumber = doctorInfo.LicenseNumber;
                Experience = doctorInfo.Experience;
                Rating = doctorInfo.Rating;
                DepartmentID = doctorInfo.DepartmentID;
                DepartmentName = doctorInfo.DepartmentName;
            }
            else throw new Exception("Doctor not found");
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}