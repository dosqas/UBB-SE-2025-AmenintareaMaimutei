using Project.ClassModels;
using Project.Models;
using Project.Utils;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace Project.ViewModels.AddViewModels
{
    internal class ScheduleAddViewModel : INotifyPropertyChanged
    {
        private readonly ScheduleModel _scheduleModel = new ScheduleModel();
        public ObservableCollection<Schedule> Schedules { get; set; } = new ObservableCollection<Schedule>();

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

        private int _shiftID;
        public int ShiftID
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
            LoadSchedules();
        }

        private void LoadSchedules()
        {
            Schedules.Clear();
            foreach (Schedule schedule in _scheduleModel.GetSchedules())
            {
                Schedules.Add(schedule);
            }
        }

        private void SaveSchedule()
        {
            var schedule = new Schedule
            {
                ScheduleID = 0,
                DoctorID = DoctorID,
                ShiftID = ShiftID
            };

            if (ValidateSchedule(schedule))
            {
                bool success = _scheduleModel.AddSchedule(schedule);
                ErrorMessage = success ? "Schedule added successfully" : "Failed to add schedule";
                if (success)
                {
                    LoadSchedules();
                }
            }
        }

        private bool ValidateSchedule(Schedule schedule)
        {
            if (schedule.DoctorID == 0 || !_scheduleModel.DoesDoctorExist(schedule.DoctorID))
            {
                ErrorMessage = "DoctorID doesn’t exist in the Doctors Records.";
                return false;
            }

            if (schedule.ShiftID == 0 || !_scheduleModel.DoesShiftExist(schedule.ShiftID))
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

