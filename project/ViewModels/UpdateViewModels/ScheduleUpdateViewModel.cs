using Project.ClassModels;
using Project.Models;
using Project.Utils;
using Project.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Project.ViewModels.UpdateViewModels
{
    class ScheduleUpdateViewModel : INotifyPropertyChanged
    {
        private readonly ScheduleModel _scheduleModel = new ScheduleModel();
        public ObservableCollection<Schedule> Schedules { get; set; } = new ObservableCollection<Schedule>();

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
        public ScheduleUpdateViewModel()
        {
            _errorMessage = string.Empty;
            SaveChangesCommand = new RelayCommand(SaveChanges);
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

        private void SaveChanges()
        {
            foreach (Schedule schedule in Schedules)
            {
                if (ValidateSchedule(schedule))
                {
                    bool success = _scheduleModel.UpdateSchedule(schedule);
                    ErrorMessage = success ? "Changes saved successfully!" : "Failed to save changes.";
                }
            }
        }

        private bool ValidateSchedule(Schedule schedule)
        {
            if (schedule.ScheduleID <= 0)
            {
                ErrorMessage = "Schedule ID is required.";
                return false;
            }
            if (schedule.DoctorID <= 0)
            {
                ErrorMessage = "Doctor ID is required.";
                return false;
            }
            if (schedule.ShiftID <= 0)
            {
                ErrorMessage = "Shift ID is required.";
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
