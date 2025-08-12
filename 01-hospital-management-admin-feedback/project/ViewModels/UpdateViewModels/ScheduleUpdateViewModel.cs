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
            bool hasErrors = false;
            StringBuilder errorMessages = new StringBuilder();

            foreach (Schedule schedule in Schedules)
            {
                if (!ValidateSchedule(schedule))
                {
                    hasErrors = true;
                    errorMessages.AppendLine("Schedule " + schedule.ScheduleID + ": " + ErrorMessage);
                }
                else
                {
                    bool success = _scheduleModel.UpdateSchedule(schedule);
                    if (!success)
                    {
                        errorMessages.AppendLine("Failed to save changes for schedule: " + schedule.DoctorID);
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

        private bool ValidateSchedule(Schedule schedule)
        {
            bool doctorExists = _scheduleModel.DoesDoctorExist(schedule.DoctorID);
            if (!doctorExists)
            {
                ErrorMessage = "DoctorID doesn’t exist in the Doctors Records";
                return false;
            }

            bool shiftExists = _scheduleModel.DoesShiftExist(schedule.ShiftID);
            if (!shiftExists)
            {
                ErrorMessage = "ShiftID doesn’t exist in the Shifts Records";
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
