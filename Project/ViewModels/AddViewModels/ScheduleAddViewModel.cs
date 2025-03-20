using Project.Models;
using Project.Utils;
using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Windows.Input;
using ScheduleModel = Project.ClassModels.ScheduleModel;

namespace Project.ViewModels.AddViewModels
{
    internal class ScheduleAddViewModel : INotifyPropertyChanged
    {
        private readonly ScheduleModel _scheduleModel = new ScheduleModel();
        private readonly string _connectionString = DatabaseHelper.GetConnectionString();

        private Guid _doctorID;
        public Guid DoctorID
        {
            get => _doctorID;
            set
            {
                _doctorID = value;
                OnPropertyChanged(nameof(DoctorID));
            }
        }

        private Guid _shiftID;
        public Guid ShiftID
        {
            get => _shiftID;
            set
            {
                _shiftID = value;
                OnPropertyChanged(nameof(ShiftID));
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

        public ICommand SaveScheduleCommand { get; }

        public ScheduleAddViewModel()
        {
            SaveScheduleCommand = new RelayCommand(SaveSchedule);
        }

        private void SaveSchedule()
        {
            var schedule = new Schedule
            {
                ScheduleID = Guid.NewGuid(),
                DoctorID = DoctorID,
                ShiftID = ShiftID
            };

            if (ValidateSchedule(schedule))
            {
                bool success = _scheduleModel.AddSchedule(schedule);
                ErrorMessage = success ? "Schedule added successfully" : "Failed to add schedule";
            }
        }

        private bool ValidateSchedule(Schedule schedule)
        {
            if (schedule.DoctorID == Guid.Empty || !_scheduleModel.DoesDoctorExist(schedule.DoctorID))
            {
                ErrorMessage = "DoctorID doesn’t exist in the Doctors Records.";
                return false;
            }

            if (schedule.ShiftID == Guid.Empty || !_scheduleModel.DoesShiftExist(schedule.ShiftID))
            {
                ErrorMessage = "ShiftID doesn’t exist in the Shifts Records.";
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
