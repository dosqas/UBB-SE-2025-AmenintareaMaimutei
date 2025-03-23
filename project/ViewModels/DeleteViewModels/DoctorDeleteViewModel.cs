using Project.Models;
using Project.Utils;
using System;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows;
using DoctorModel = Project.ClassModels.DoctorModel;

namespace Project.ViewModel
{
    class DoctorDeleteViewModel : INotifyPropertyChanged
    {
        private readonly DoctorModel _doctorModel = new DoctorModel();

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

        private string? _errorMessage;


        public string ErrorMessage
        {
            get => _errorMessage ?? string.Empty;
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }


        public ICommand DeleteDoctorCommand { get; }

        public DoctorDeleteViewModel()
        {
            DeleteDoctorCommand = new RelayCommand(RemoveDoctor);
        }

        private void RemoveDoctor()
        {
            if (DoctorID == Guid.Empty)
            {
                ErrorMessage = "No doctor was selected";
                return;
            }

            if (!_doctorModel.DoesDoctorExist(DoctorID))
            {
                ErrorMessage = "DoctorID doesn't exist in the Doctor Records";
                return;
            }

            // MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete doctor {DoctorID}?", 
            //                                           "Confirm Deletion", 
            //                                           MessageBoxButton.YesNo, 
            //                                           MessageBoxImage.Warning);

            // if (result == MessageBoxResult.Yes)
            // {
            //     bool success = _doctorModel.DeleteDoctor(DoctorID);
            //     ErrorMessage = success ? "Doctor deleted successfully" : "Failed to delete doctor";
            // }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
