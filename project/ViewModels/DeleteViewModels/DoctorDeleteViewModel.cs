using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Project.ClassModels;
using Project.Models;
using Project.Utils;

namespace Project.ViewModels.DeleteViewModels
{
    class DoctorDeleteViewModel : INotifyPropertyChanged
    {
        private readonly DoctorModel _doctorModel = new DoctorModel();
        private ObservableCollection<Doctor> _doctors;
        private int _doctorID;
        private string _errorMessage;
        private string _messageColor = "Red";

        public ObservableCollection<Doctor> Doctors
        {
            get { return _doctors; }
            set { SetProperty(ref _doctors, value); }
        }

        public int DoctorID
        {
            get => _doctorID;
            set
            {
                _doctorID = value;
                OnPropertyChanged(nameof(DoctorID));
                OnPropertyChanged(nameof(CanDeleteDoctor));
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage ?? string.Empty;
            set
            {
                _errorMessage = value;
                MessageColor = string.IsNullOrEmpty(value) ? "Red" : value.Contains("successfully") ? "Green" : "Red";
                OnPropertyChanged(nameof(ErrorMessage));
                OnPropertyChanged(nameof(MessageColor));
            }
        }

        public string MessageColor
        {
            get => _messageColor;
            set
            {
                _messageColor = value;
                OnPropertyChanged(nameof(MessageColor));
            }
        }

        public ICommand DeleteDoctorCommand { get; }

        public bool CanDeleteDoctor => DoctorID > 0;

        public DoctorDeleteViewModel()
        {
            // Load doctors for the DataGrid
            Doctors = new ObservableCollection<Doctor>(_doctorModel.GetDoctors());

            DeleteDoctorCommand = new RelayCommand(RemoveDoctor);
        }

        private bool CanExecuteDeleteDoctor()
        {
            return DoctorID > 0;
        }

        private void RemoveDoctor()
        {
            if (DoctorID == 0)
            {
                ErrorMessage = "No doctor was selected";
                return;
            }

            if (!_doctorModel.DoesDoctorExist(DoctorID))
            {
                ErrorMessage = "DoctorID doesn't exist in the records";
                return;
            }

            bool success = _doctorModel.DeleteDoctor(DoctorID);
            ErrorMessage = success ? "Doctor deleted successfully" : "Failed to delete doctor";

            if (success)
            {
                Doctors = new ObservableCollection<Doctor>(_doctorModel.GetDoctors());
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SetProperty<T>(ref T field, T value, [System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                OnPropertyChanged(propertyName);
            }
        }
    }
}
