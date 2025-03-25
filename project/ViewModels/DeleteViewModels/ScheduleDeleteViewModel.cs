using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Project.ClassModels;
using Project.Models;
using Project.Utils;

namespace Project.ViewModels.DeleteViewModels
{
    class ScheduleDeleteViewModel : INotifyPropertyChanged
    {
        private readonly ScheduleModel _scheduleModel = new ScheduleModel();
        private ObservableCollection<Schedule> _schedules;
        private int _scheduleID;
        private string _errorMessage;
        private string _messageColor = "Red"; 

        public ObservableCollection<Schedule> Schedules
        {
            get { return _schedules; }
            set { SetProperty(ref _schedules, value); }
        }

        public int ScheduleID
        {
            get => _scheduleID;
            set
            {
                _scheduleID = value;
                OnPropertyChanged(nameof(ScheduleID));
                OnPropertyChanged(nameof(CanDeleteSchedule));
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage ?? string.Empty;
            set
            {
                _errorMessage = value;
                // Set MessageColor based on success or error
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

        public ICommand DeleteScheduleCommand { get; }

        public bool CanDeleteSchedule => ScheduleID > 0;

        public ScheduleDeleteViewModel()
        {
            // Load schedules for the DataGrid
            Schedules = new ObservableCollection<Schedule>(_scheduleModel.GetSchedules());

            DeleteScheduleCommand = new RelayCommand(RemoveSchedule);
        }

        private void RemoveSchedule()
        {
            if (ScheduleID == 0)
            {
                ErrorMessage = "No schedule was selected";
                return;
            }

            if (!_scheduleModel.DoesScheduleExist(ScheduleID))
            {
                ErrorMessage = "ScheduleID doesn't exist in the records";
                return;
            }

            bool success = _scheduleModel.DeleteSchedule(ScheduleID);
            ErrorMessage = success ? "Schedule deleted successfully" : "Failed to delete schedule";

            if (success)
            {
                Schedules = new ObservableCollection<Schedule>(_scheduleModel.GetSchedules());
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
